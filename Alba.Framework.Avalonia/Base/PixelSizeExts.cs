using Avalonia;

namespace Alba.Framework.Avalonia;

public static class PixelSizeExts
{
    extension(PixelSize @this)
    {
        public bool IsZero => @this == new PixelSize();

        public static PixelSize Min(PixelSize a, PixelSize b) =>
            Extremum(a, b, Math.Min);

        public static PixelSize Max(PixelSize a, PixelSize b) =>
            Extremum(a, b, Math.Max);

        private static PixelSize Extremum(PixelSize a, PixelSize b, Func<int, int, int> f) =>
            new(f(a.Width, b.Width), f(a.Height, b.Height));
    }
}