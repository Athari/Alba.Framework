using Avalonia;

namespace Alba.Framework.Avalonia;

public static class PointExts
{
    extension(Point @this)
    {
        public Point Ceiling() =>
            new(Math.Ceiling(@this.X), Math.Ceiling(@this.Y));

        public Point Floor() =>
            new(Math.Floor(@this.X), Math.Floor(@this.Y));

        public Point Round(MidpointRounding mode = MidpointRounding.ToEven) =>
            new(Math.Round(@this.X, mode), Math.Round(@this.Y, mode));

        public Point RoundTo(double precision, MidpointRounding mode = MidpointRounding.ToEven) =>
            new(Math.Round(@this.X / precision, mode) * precision,
                Math.Round(@this.Y / precision, mode) * precision);

        public PixelPoint ToPixel(double scale = 1) =>
            PixelPoint.FromPoint(@this, scale);

        public PixelPoint ToPixelRounded(double scale = 1) =>
            new((int)Math.Round(@this.X * scale), (int)Math.Round(@this.Y * scale));
    }
}