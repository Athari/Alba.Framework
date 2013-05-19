using System.Windows;

namespace Alba.Framework.Wpf
{
    public static class DependencyPropertyExts
    {
        public static PropertyPath ToPath (this DependencyProperty dp)
        {
            return new PropertyPath(dp);
        }

        public static TObject SetDp<TObject> (this TObject d, DependencyProperty dp, object value)
            where TObject : DependencyObject
        {
            d.SetValue(dp, value);
            return d;
        }

        public static TProp GetDp<TProp> (this DependencyObject d, DependencyProperty dp)
            where TProp : DependencyObject
        {
            return (TProp)d.GetValue(dp);
        }
    }
}