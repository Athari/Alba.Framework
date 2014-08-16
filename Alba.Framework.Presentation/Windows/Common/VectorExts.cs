using System.Windows;
using Alba.Framework.Sys;

namespace Alba.Framework.Windows
{
    public static class VectorExts
    {
        public static bool IsCloseTo (this Vector vector1, Vector vector2)
        {
            return vector1.X.IsCloseTo(vector2.X) && vector1.Y.IsCloseTo(vector2.Y);
        }
    }
}