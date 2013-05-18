using System.Windows;

namespace Alba.Framework.Wpf
{
    public static class RoutedEventArgsExts
    {
        public static T GetSourceDataContext<T> (this RoutedEventArgs @this)
        {
            return (T)((FrameworkElement)@this.Source).DataContext;
        }
    }
}