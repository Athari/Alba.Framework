using Avalonia;

namespace Alba.Framework.Avalonia;

public static class SizeExts
{
    extension(Size @this)
    {
        public Size Ceiling() =>
            new(Math.Ceiling(@this.Width), Math.Ceiling(@this.Height));

        public Size Floor() =>
            new(Math.Floor(@this.Width), Math.Floor(@this.Height));

        public Size Round(MidpointRounding mode = MidpointRounding.ToEven) =>
            new(Math.Round(@this.Width, mode), Math.Round(@this.Height, mode));

        public Size RoundTo(double precision, MidpointRounding mode = MidpointRounding.ToEven) =>
            new(Math.Round(@this.Width / precision, mode) * precision,
                Math.Round(@this.Height / precision, mode) * precision);

        public PixelSize ToPixel(double scale = 1) =>
            PixelSize.FromSize(@this, scale);

        public PixelSize ToPixelRounded(double scale = 1) =>
            new((int)Math.Round(@this.Width * scale), (int)Math.Round(@this.Height * scale));
    }
}