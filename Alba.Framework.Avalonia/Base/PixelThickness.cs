using Avalonia;

namespace Alba.Framework.Avalonia;

public record struct PixelThickness(int left, int top, int right, int bottom)
{
    public readonly int Left = left;
    public readonly int Top = top;
    public readonly int Right = right;
    public readonly int Bottom = bottom;

    public PixelThickness(int horizontal, int vertical) : this(horizontal, vertical, horizontal, vertical) { }

    public PixelThickness(int uniform) : this(uniform, uniform, uniform, uniform) { }

    public PixelThickness() : this(0, 0, 0, 0) { }

    public bool IsZero => this == new PixelThickness();

    public static PixelThickness Min(PixelThickness a, PixelThickness b) =>
        Extremum(a, b, Math.Min);

    public static PixelThickness Max(PixelThickness a, PixelThickness b) =>
        Extremum(a, b, Math.Max);

    private static PixelThickness Extremum(PixelThickness a, PixelThickness b, Func<int, int, int> f) =>
        new(f(a.Left, b.Left), f(a.Top, b.Top), f(a.Right, b.Right), f(a.Bottom, b.Bottom));

    public Thickness ToFloat() => new(Left, Top, Right, Bottom);
}