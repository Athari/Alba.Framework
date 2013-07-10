// ReSharper disable RedundantUsingDirective
// ReSharper disable RedundantCast
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using FPM = System.Windows.FrameworkPropertyMetadata;
using FPMO = System.Windows.FrameworkPropertyMetadataOptions;

namespace Alba.Framework.Windows.Controls
{
    public static partial class TreeViewProps
    {
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.RegisterAttached(
            "SelectedItem", typeof(object), typeof(TreeViewProps),
            new FPM(default(object), SelectedItem_Changed));

        public static readonly DependencyProperty SelectedItemSyncProperty = DependencyProperty.RegisterAttached(
            "SelectedItemSync", typeof(bool), typeof(TreeViewProps),
            new FPM(default(bool), SelectedItemSync_Changed));

        public static object GetSelectedItem (TreeView d)
        {
            return (object)d.GetValue(SelectedItemProperty);
        }

        public static void SetSelectedItem (TreeView d, object value)
        {
            d.SetValue(SelectedItemProperty, value);
        }

        private static void SelectedItem_Changed (DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            SelectedItem_Changed((TreeView)d, new DpChangedEventArgs<object>(args));
        }

        public static bool GetSelectedItemSync (TreeView d)
        {
            return (bool)d.GetValue(SelectedItemSyncProperty);
        }

        public static void SetSelectedItemSync (TreeView d, bool value)
        {
            d.SetValue(SelectedItemSyncProperty, value);
        }

        private static void SelectedItemSync_Changed (DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            SelectedItemSync_Changed((TreeView)d, new DpChangedEventArgs<bool>(args));
        }

    }
}
