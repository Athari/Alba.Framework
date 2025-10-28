using Avalonia;
using Avalonia.Media.Imaging;

namespace Alba.Framework.Avalonia.Imaging;

public static class BitmapExts
{
    extension(Bitmap @this)
    {
        public PixelRect PixelRect => new(new(0, 0), @this.PixelSize);
        public int PixelWidth => @this.PixelSize.Width;
        public int PixelHeight => @this.PixelSize.Height;

        private Bitmap ToNonWriteableBitmap()
        {
            if (@this is not WriteableBitmap writeable)
                return @this;
            var pixelFormat = @this.Format ?? throw new ArgumentException("Bitmap has no pixel format.");
            var alphaFormat = @this.AlphaFormat ?? throw new ArgumentException("Bitmap has no alpha format.");
            using var buf = writeable.Lock();
            return new(pixelFormat, alphaFormat, buf.Address, @this.PixelSize, @this.Dpi, buf.RowBytes);
        }

        // TODO: Remove this workaround when https://github.com/AvaloniaUI/Avalonia/issues/8444 is fixed
        public Bitmap CreateResized(PixelSize size, BitmapInterpolationMode interpolationMode = BitmapInterpolationMode.HighQuality)
        {
            var bmp = @this;
            if (bmp is WriteableBitmap)
                bmp = bmp.ToNonWriteableBitmap();
            return bmp.CreateScaledBitmap(size, interpolationMode);
        }
    }
}