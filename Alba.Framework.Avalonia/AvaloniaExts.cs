using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls.Selection;
using Avalonia.Input;
using Avalonia.Platform;
using SkiaSharp;

namespace Alba.Framework.Avalonia;

public static class AvaloniaExts
{
    public static void ToggleSelect<T>(this SelectionModel<T> @this, int index, bool? toggle)
    {
        if (toggle == true)
            @this.Select(index);
        else if (toggle == false)
            @this.Deselect(index);
    }

    public static IEnumerable<T> GetSelectedItems<T>(this SelectionModel<T> @this) =>
        @this.SelectedItems.OfType<T>();

    public static IEnumerable<T> GetSelectedItems<T>(this SelectionModelSelectionChangedEventArgs<T> @this) =>
        @this.SelectedItems.OfType<T>();

    public static IEnumerable<T> GetDeselectedItems<T>(this SelectionModelSelectionChangedEventArgs<T> @this) =>
        @this.DeselectedItems.OfType<T>();

    public static int GetStride<T>(this ILockedFramebuffer @this) =>
        @this.RowBytes / Marshal.SizeOf<T>();

    public static Span<T> GetRow<T>(this ILockedFramebuffer @this, int y) where T : unmanaged
    {
        var stride = @this.GetStride<T>();
        return @this.Address.ToSpanAtOffset<T>(y * stride, stride);
    }

    public static bool ContainsFiles(this IDataObject @this) =>
        @this.Contains(DataFormats.Files);

    public static PixelSize ToPixel(this Size @this, double scale = 1) =>
        PixelSize.FromSize(@this, scale);

    public static PixelSize ToPixelRounded(this Size @this, double scale = 1) =>
        new((int)Math.Round(@this.Width * scale), (int)Math.Round(@this.Height * scale));

    public static PixelPoint ToPixel(this Point @this, double scale = 1) =>
        PixelPoint.FromPoint(@this, scale);

    public static PixelPoint ToPixelRounded(this Point @this, double scale = 1) =>
        new((int)Math.Round(@this.X * scale), (int)Math.Round(@this.Y * scale));

    public static Size ToSize(this SKSize @this) => new(@this.Width, @this.Height);
    public static PixelSize ToPixelSize(this SKSizeI @this) => new(@this.Width, @this.Height);
    public static Size ToSize(this SKSizeI @this) => new(@this.Width, @this.Height);
}