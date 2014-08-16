using System.Windows;
using Alba.Framework.Sys;

namespace Alba.Framework.Windows
{
    public static class RectExts
    {
        public static bool IsCloseTo (this Rect rect1, Rect rect2)
        {
            return rect1.IsEmpty ? rect2.IsEmpty : !rect2.IsEmpty && rect1.X.IsCloseTo(rect2.X) &&
                rect1.Y.IsCloseTo(rect2.Y) && rect1.Height.IsCloseTo(rect2.Height) &&
                rect1.Width.IsCloseTo(rect2.Width);
        }

        public static bool HasNaN (this Rect r)
        {
            return r.X.IsNaN() || r.Y.IsNaN() || r.Height.IsNaN() || r.Width.IsNaN();
        }
    }
}