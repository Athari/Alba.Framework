using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Alba.Framework.Collections;
using Alba.Framework.Sys;
using Alba.Framework.Text;

// ReSharper disable MemberCanBePrivate.Local
namespace Alba.Framework.Windows.Controls
{
    public static partial class SelectorProps
    {
        private static void SelectedItems_Changed (Selector selector, DpChangedEventArgs<IList> e)
        {
            SelectedItemsSync sync = GetSelectedItemsSync(selector);
            if (sync != null) {
                sync.ModelSelectedItems = e.NewValue;
            }
            else {
                SetSelectedItemsSync(selector, new SelectedItemsSync(selector, e.NewValue));
                selector.Unloaded += Selector_Unloaded;
            }
        }

        private static void Selector_Unloaded (object sender, RoutedEventArgs e)
        {
            var selector = (Selector)sender;
            GetSelectedItemsSync(selector).Dispose();
            SetSelectedItemsSync(selector, null);
        }

        private class SelectedItemsSync : IDisposable
        {
            private const string ErrorUnsupportedSelectorType = "Unsupported selector type: {0}";

            private IList _modelSelectedItems;

            public Selector Selector { get; private set; }

            public IList ModelSelectedItems
            {
                get { return _modelSelectedItems; }
                set
                {
                    DetachSelectedItems();
                    _modelSelectedItems = value;
                    AttachSelectedItems();
                }
            }

            private IList ListSelectedItems
            {
                get
                {
                    var multiSelector = Selector as MultiSelector;
                    if (multiSelector != null)
                        return multiSelector.SelectedItems;
                    var listBox = Selector as ListBox;
                    if (listBox != null)
                        return listBox.SelectedItems;
                    throw new InvalidOperationException(ErrorUnsupportedSelectorType.Fmt(Selector.GetTypeFullName()));
                }
            }

            public SelectedItemsSync (Selector selector, IList modelSelectedItems)
            {
                if (!(selector is MultiSelector) && !(selector is ListBox))
                    throw new ArgumentException(ErrorUnsupportedSelectorType.Fmt(Selector.GetTypeFullName()), "selector");
                if (modelSelectedItems != null && !(modelSelectedItems is INotifyCollectionChanged))
                    throw new ArgumentException("Selected items collection must implement INotifyCollectionChanged", "modelSelectedItems");
                Selector = selector;
                _modelSelectedItems = modelSelectedItems;
                AttachSelectedItems();
            }

            private void AttachSelectedItems ()
            {
                Selector.SelectionChanged -= Selector_SelectionChanged;

                if (ModelSelectedItems == null)
                    return;
                ((INotifyCollectionChanged)ModelSelectedItems).CollectionChanged += ModelSelectedItems_CollectionChanged;
                ListSelectedItems.ReplaceUntyped(ModelSelectedItems);

                Selector.SelectionChanged += Selector_SelectionChanged;
            }

            private void DetachSelectedItems ()
            {
                if (ModelSelectedItems == null)
                    return;
                ((INotifyCollectionChanged)ModelSelectedItems).CollectionChanged -= ModelSelectedItems_CollectionChanged;
            }

            private void Selector_SelectionChanged (object sender, SelectionChangedEventArgs e)
            {
                if (ModelSelectedItems == null)
                    return;
                foreach (object item in e.AddedItems)
                    ModelSelectedItems.Add(item);
                foreach (object item in e.RemovedItems)
                    ModelSelectedItems.Remove(item);
            }

            private void ModelSelectedItems_CollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
            {
                Selector.SelectionChanged -= Selector_SelectionChanged;

                if (e.NewItems != null)
                    foreach (Object item in e.NewItems)
                        ListSelectedItems.Add(item);
                if (e.OldItems != null)
                    foreach (Object item in e.OldItems)
                        ListSelectedItems.Remove(item);
                if (e.Action == NotifyCollectionChangedAction.Reset)
                    ListSelectedItems.ReplaceUntyped(ModelSelectedItems);

                Selector.SelectionChanged += Selector_SelectionChanged;
            }

            public void Dispose ()
            {
                if (Selector == null)
                    return;
                Selector.SelectionChanged -= Selector_SelectionChanged;
                DetachSelectedItems();
                Selector = null;
            }
        }
    }
}