using System.Windows;
using System.Windows.Controls;

namespace Alba.Framework.Wpf
{
    public static class ItemsControlExts
    {
        public static FrameworkElement GetItemContainer (this ItemsControl @this, object item)
        {
            return (FrameworkElement)@this.ItemContainerGenerator.ContainerFromItem(item);
        }
    }
}