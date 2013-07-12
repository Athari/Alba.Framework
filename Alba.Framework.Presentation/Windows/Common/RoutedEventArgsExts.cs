using System.Windows;

namespace Alba.Framework.Windows
{
    public static class RoutedEventArgsExts
    {
        public static T GetSourceDataContext<T> (this RoutedEventArgs @this)
        {
            return (T)((FrameworkElement)@this.Source).DataContext;
        }
    }
}