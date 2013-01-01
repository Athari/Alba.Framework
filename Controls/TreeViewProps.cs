using System.Windows.Controls;
using Alba.Framework.Events;
using RpChangedEventArgs = System.Windows.RoutedPropertyChangedEventArgs<object>;

namespace Alba.Framework.Controls
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
                TreeView_SelectedItemChanged(tree, new RpChangedEventArgs(item, item));
            }
        }

        private static void TreeView_SelectedItemChanged (object sender, RpChangedEventArgs args)
        {
            var tree = (TreeView)sender;
            tree.SetCurrentValue(SelectedItemProperty, args.NewValue);
        }

        private static void SelectedItem_Changed (TreeView tree, DpChangedEventArgs<object> args)
        {}
    }
}