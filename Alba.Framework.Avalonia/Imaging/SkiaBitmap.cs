using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Skia;
using SkiaSharp;

namespace Alba.Framework.Avalonia.Imaging;

public static class AvaloniaImageExts
{
    public static PixelSize GetPixelSize(this SKBitmap @this) =>
        new(@this.Width, @this.Height);

    public static WriteableBitmap ToWriteableBitmap(this SKBitmap @this)
    {
        var bitmap = new WriteableBitmap(
            @this.GetPixelSize(), SkiaPlatform.DefaultDpi,
            @this.ColorType.ToPixelFormat(), @this.AlphaType.ToAlphaFormat());
        using var buffer = bitmap.Lock();
        @this.GetPixelSpan().CopyTo(buffer.Address.ToSpan<byte>(@this.ByteCount));
        return bitmap;
    }

    public static Bitmap ToBitmap(this SKBitmap @this) =>
        new(@this.ColorType.ToPixelFormat(), @this.AlphaType.ToAlphaFormat(),
            @this.GetPixels(), @this.GetPixelSize(), SkiaPlatform.DefaultDpi, @this.RowBytes);
}

//using E = System.Linq.Expressions.Expression;
//public class SkiaBitmap : IImage, IDisposable
//{
//    private static readonly Func<RenderTargetBitmap, SKBitmap> GetBitmap;
//    private static readonly Func<RenderTargetBitmap, object> GetLock;

//    private bool _isDisposed;

//    public SKCanvas SKCanvas { get; }
//    public SKBitmap Bitmap { get; }
//    public RenderTargetBitmap AvaloniaBitmap { get; set; }
//    public Size Size { get; }
//    public int Width { get; }
//    public int Height { get; }
//    public object Lock { get; }

//    static SkiaBitmap()
//    {
//        var renderTargetBitmapImplType = typeof(SkiaPlatform).Assembly.GetType($"{typeof(SkiaPlatform).Namespace}.WriteableBitmapImpl")!;
//        var platformImplProp = typeof(RenderTargetBitmap)
//            .GetProperty("PlatformImpl", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)!;

//        var exprBitmapParam = E.Parameter(typeof(RenderTargetBitmap));
//        var exprGetIRefItem = E.Property(E.Property(exprBitmapParam, platformImplProp), "Item");

//        GetBitmap = E.Lambda<Func<RenderTargetBitmap, SKBitmap>>(
//            E.Field(E.Convert(exprGetIRefItem, renderTargetBitmapImplType), "_bitmap"), exprBitmapParam).Compile();
//        GetLock = E.Lambda<Func<RenderTargetBitmap, object>>(
//            E.Field(E.Convert(exprGetIRefItem, renderTargetBitmapImplType), "_lock"), exprBitmapParam).Compile();
//    }

//    public SkiaBitmap(int width, int height)
//    {
//        AvaloniaBitmap = new(new(width, height), SkiaPlatform.DefaultDpi);
//        Lock = GetLock(AvaloniaBitmap);
//        Bitmap = GetBitmap(AvaloniaBitmap);

//        SKCanvas = new(Bitmap);
//        Size = new(width, height);
//        Width = width;
//        Height = height;
//    }

//    public void Draw(DrawingContext context, Rect sourceRect, Rect destRect)
//    {
//        context.DrawImage(AvaloniaBitmap, sourceRect, destRect);
//    }

//    protected virtual void Dispose(bool disposing)
//    {
//        if (_isDisposed)
//            return;
//        if (disposing) {
//            SKCanvas.Dispose();
//            AvaloniaBitmap.Dispose();
//        }
//        _isDisposed = true;
//    }

//    public void Dispose()
//    {
//        Dispose(disposing: true);
//        GC.SuppressFinalize(this);
//    }

//    public void Indispose()
//    {
//        if (_isDisposed)
//            return;
//        SKCanvas.Dispose();
//        _isDisposed = true;
//    }
//}