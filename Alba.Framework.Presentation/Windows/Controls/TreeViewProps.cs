using System.Windows;
using System.Windows.Controls;
using Alba.Framework.Events;

namespace Alba.Framework.Windows.Controls
{
    public partial class TreeViewProps
    {
        private static void SelectedItemSync_Changed (TreeView tree, DpChangedEventArgs<bool> args)
        {
            if (args.OldValue)
                tree.SelectedItemChanged -= TreeView_SelectedItemChanged;
            if (args.NewValue) {
                tree.SelectedItemChanged += TreeView_SelectedItemChanged;
                var item = GetSelectedItem(tree);
                TreeView_SelectedItemChanged(tree, new RoutedPropertyChangedEventArgs<object>(item, item));
            }
        }

        private static void TreeView_SelectedItemChanged (object sender, RoutedPropertyChangedEventArgs<object> args)
        {
            var tree = (TreeView)sender;
            tree.SetCurrentValue(SelectedItemProperty, args.NewValue);
        }

        private static void SelectedItem_Changed (TreeView tree, DpChangedEventArgs<object> args)
        {}
    }
}