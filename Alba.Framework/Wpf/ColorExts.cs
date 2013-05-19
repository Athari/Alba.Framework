using System.Windows.Media;

namespace Alba.Framework.Wpf
{
    public static class ColorExts
    {
        public static HslColor ToHsl (this Color @this)
        {
            return new HslColor(@this);
        }

        public static Color Invert (this Color @this)
        {
            return new Color { ScA = @this.ScA, ScR = 1 - @this.ScR, ScG = 1 - @this.ScG, ScB = 1 - @this.ScB };
        }

        public static Color Darker (this Color @this, float percent)
        {
            return new HslColor(@this).Darker(percent).ToColor();
        }

        public static Color Lighter (this Color @this, float percent)
        {
            return new HslColor(@this).Lighter(percent).ToColor();
        }
    }
}