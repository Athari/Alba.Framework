using System.Windows;
using System.Windows.Media;

namespace Alba.Framework.Wpf
{
    public static class VisualExts
    {
        public static DependencyObject GetParent (this Visual visual)
        {
            return VisualTreeHelper.GetParent(visual);
        }

        public static TParent GetParent<TParent> (this Visual visual)
            where TParent : class
        {
            DependencyObject ctl;
            for (ctl = visual; ctl != null && !(ctl is TParent); ctl = VisualTreeHelper.GetParent(ctl)) {}
            return ctl as TParent;
        }

        public static Visual GetRootParent (this Visual visual)
        {
            DependencyObject root = visual;
            for (DependencyObject d = root; d != null; d = VisualTreeHelper.GetParent(d))
                root = d;
            return (Visual)root;
        }
    }
}