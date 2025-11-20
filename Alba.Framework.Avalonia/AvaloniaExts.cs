using Avalonia;
using Avalonia.Reactive;
using SkiaSharp;

namespace Alba.Framework.Avalonia;

public static class AvaloniaExts
{
    extension<T>(IObservable<AvaloniaPropertyChangedEventArgs<T>> @this)
    {
        public IDisposable Subscribe<TSender>(Action<TSender, AvaloniaPropertyChangedEventArgs<T>> action) =>
            @this.Subscribe(
                new AnonymousObserver<AvaloniaPropertyChangedEventArgs<T>>(e => {
                    if (e.Sender is not TSender sender)
                        return;
                    action(sender, e);
                })
            );
    }

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