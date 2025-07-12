using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Controls;

namespace Alba.Framework.Avalonia.Controls;

public partial class SplitPanel : Panel
{
    public static readonly StyledProperty<double> AspectRatioProperty =
        AvaloniaProperty.Register<SplitPanel, double>(nameof(AspectRatio), 1.0d,
            coerce: (_, v) => Math.Clamp(v, 0.1d, 10.0d));

    private List<Cell> _cells = [ ];

    static SplitPanel()
    {
        AffectsArrange<SplitPanel>(AspectRatioProperty);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        foreach (var child in Children)
            child.Measure(Size.Infinity);
        return new(0, 0);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        UpdateCells(Children.Count, finalSize.Width, finalSize.Height);
        foreach (var (child, cell) in Children.Zip(_cells)) {
            //child.Arrange(new(new(cell.X, cell.Y), child.DesiredSize));
            child.Arrange(new(cell.X, cell.Y, cell.Width, cell.Height));
        }
        return finalSize;
    }

    [SuppressMessage("Style", "IDE0305:Simplify collection initialization", Justification = "Don't mess with LINQ")]
    private void UpdateCells(int count, double width, double height)
    {
        _cells.Clear();
        if (count == 0)
            return;
        _cells = GetAllIntPartitions(count)
            .Select(p => LayoutCells(p).ToList())
            .MinBy(QualityInv)!
            .ToList();

        double Sqr(double v) => v * v;

        double QualityInv(List<Cell> cells) => cells.Sum(c => Sqr(c.Width - c.Height * AspectRatio));

        IEnumerable<Cell> LayoutCells(List<int> p)
        {
            var h = height / p.Count;
            for (var j = 0; j < p.Count; ++j) {
                var w = width / p[j];
                for (var i = 0; i < p[j]; ++i)
                    yield return new(i * w, j * h, w, h);
            }
        }

        IEnumerable<List<int>> GetAllIntPartitions(int n)
        {
            var p = new int[n];
            var k = 0;
            p[k] = n;
            while (true) {
                yield return p.Take(k + 1).ToList();
                var rem = 0;
                while (k >= 0 && p[k] == 1)
                    rem += p[k--];
                if (k < 0)
                    yield break;
                p[k]--;
                rem++;
                while (rem > p[k]) {
                    p[k + 1] = p[k];
                    rem -= p[k++];
                }
                p[++k] = rem;
            }
        }
    }

    private record class Cell(double X, double Y, double Width, double Height);
}