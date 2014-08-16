using System.Windows;
using Alba.Framework.Sys;

namespace Alba.Framework.Windows
{
    public static class SizeExts
    {
        public static bool IsCloseTo (this Size size1, Size size2)
        {
            return size1.Width.IsCloseTo(size2.Width) && size1.Height.IsCloseTo(size2.Height);
        }
    }
}