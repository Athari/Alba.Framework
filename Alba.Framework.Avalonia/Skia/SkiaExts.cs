using Avalonia;
using SkiaSharp;

namespace Alba.Framework.Avalonia.Skia;

public static class SkiaExts
{
    extension(SKSize @this)
    {
        public Size ToSize() => new(@this.Width, @this.Height);
    }

    extension(SKSizeI @this)
    {
        public PixelSize ToPixelSize() => new(@this.Width, @this.Height);
        public Size ToSize() => new(@this.Width, @this.Height);
    }
}