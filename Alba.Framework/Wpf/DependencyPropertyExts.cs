using System.Windows;

namespace Alba.Framework.Wpf
{
    public static class DependencyPropertyExts
    {
        public static PropertyPath ToPath (this DependencyProperty @this)
        {
            return new PropertyPath(@this);
        }
    }
}