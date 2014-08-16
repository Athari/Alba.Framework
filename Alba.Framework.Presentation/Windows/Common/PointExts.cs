using System.Windows;
using Alba.Framework.Sys;

namespace Alba.Framework.Windows
{
    public static class PointExts
    {
        public static bool IsCloseTo (this Point point1, Point point2)
        {
            return point1.X.IsCloseTo(point2.X) && point1.Y.IsCloseTo(point2.Y);
        }
    }
}