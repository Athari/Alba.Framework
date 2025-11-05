using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls.Selection;
using Avalonia.Input;
using Avalonia.Platform;
using SkiaSharp;

namespace Alba.Framework.Avalonia;

[PublicAPI]
public static class AvaloniaExts
{
    extension<T>(SelectionModel<T> @this)
    {
        public IEnumerable<T> AllSelectedItems =>
            @this.SelectedItems.OfType<T>();

        public IEnumerable<int> AllSelectedIndexes =>
            @this.SelectedIndexes;

        public void ToggleSelect(int index, bool? toggle)
        {
            toggle ??= !@this.IsSelected(index);
            if (toggle == true)
                @this.Select(index);
            else if (toggle == false)
                @this.Deselect(index);
        }
    }

    extension<T>(SelectionModelSelectionChangedEventArgs<T> @this)
    {
        public IEnumerable<T> AllSelectedItems =>
            @this.SelectedItems.OfType<T>();

        public IEnumerable<T> AllDeselectedItems =>
            @this.DeselectedItems.OfType<T>();
    }

    public static int GetStride<T>(this ILockedFramebuffer @this) =>
        @this.RowBytes / Marshal.SizeOf<T>();

    public static Span<T> GetRow<T>(this ILockedFramebuffer @this, int y) where T : unmanaged
    {
        var stride = @this.GetStride<T>();
        return @this.Address.ToSpanAtOffset<T>(y * stride, stride);
    }

    public static bool ContainsFiles(this IDataObject @this) =>
        @this.Contains(DataFormats.Files);

    public static Size Ceiling(this Size @this) =>
        new(Math.Ceiling(@this.Width), Math.Ceiling(@this.Height));

    public static Size Floor(this Size @this) =>
        new(Math.Floor(@this.Width), Math.Floor(@this.Height));

    public static Size Round(this Size @this, MidpointRounding mode = MidpointRounding.ToEven) =>
        new(Math.Round(@this.Width, mode), Math.Round(@this.Height, mode));

    public static Size RoundTo(this Size @this, double precision, MidpointRounding mode = MidpointRounding.ToEven) =>
        new(Math.Round(@this.Width / precision, mode) * precision,
            Math.Round(@this.Height / precision, mode) * precision);

    public static PixelSize ToPixel(this Size @this, double scale = 1) =>
        PixelSize.FromSize(@this, scale);

    public static PixelSize ToPixelRounded(this Size @this, double scale = 1) =>
        new((int)Math.Round(@this.Width * scale), (int)Math.Round(@this.Height * scale));

    public static Point Ceiling(this Point @this) =>
        new(Math.Ceiling(@this.X), Math.Ceiling(@this.Y));

    public static Point Floor(this Point @this) =>
        new(Math.Floor(@this.X), Math.Floor(@this.Y));

    public static Point Round(this Point @this, MidpointRounding mode = MidpointRounding.ToEven) =>
        new(Math.Round(@this.X, mode), Math.Round(@this.Y, mode));

    public static Point RoundTo(this Point @this, double precision, MidpointRounding mode = MidpointRounding.ToEven) =>
        new(Math.Round(@this.X / precision, mode) * precision,
            Math.Round(@this.Y / precision, mode) * precision);

    public static PixelPoint ToPixel(this Point @this, double scale = 1) =>
        PixelPoint.FromPoint(@this, scale);

    public static PixelPoint ToPixelRounded(this Point @this, double scale = 1) =>
        new((int)Math.Round(@this.X * scale), (int)Math.Round(@this.Y * scale));

    public static Size ToSize(this SKSize @this) => new(@this.Width, @this.Height);
    public static PixelSize ToPixelSize(this SKSizeI @this) => new(@this.Width, @this.Height);
    public static Size ToSize(this SKSizeI @this) => new(@this.Width, @this.Height);
}