using System.ComponentModel;

namespace Alba.Framework.Text;

// From https://stackoverflow.com/a/66354540
public class NaturalStringComparer(StringComparison comparison) : IComparer<string>
{
    [field: MaybeNull]
    public static NaturalStringComparer Ordinal => field ??= new(StringComparison.Ordinal);

    [field: MaybeNull]
    public static NaturalStringComparer OrdinalIgnoreCase => field ??= new(StringComparison.OrdinalIgnoreCase);

    [field: MaybeNull]
    public static NaturalStringComparer CurrentCulture => field ??= new(StringComparison.CurrentCulture);

    [field: MaybeNull]
    public static NaturalStringComparer CurrentCultureIgnoreCase => field ??= new(StringComparison.CurrentCultureIgnoreCase);

    [field: MaybeNull]
    public static NaturalStringComparer InvariantCulture => field ??= new(StringComparison.InvariantCulture);

    [field: MaybeNull]
    public static NaturalStringComparer InvariantCultureIgnoreCase => field ??= new(StringComparison.InvariantCultureIgnoreCase);

    public static NaturalStringComparer FromComparison(StringComparison comparison) =>
        comparison switch {
            StringComparison.CurrentCulture => CurrentCulture,
            StringComparison.CurrentCultureIgnoreCase => CurrentCultureIgnoreCase,
            StringComparison.InvariantCulture => InvariantCulture,
            StringComparison.InvariantCultureIgnoreCase => InvariantCultureIgnoreCase,
            StringComparison.Ordinal => Ordinal,
            StringComparison.OrdinalIgnoreCase => OrdinalIgnoreCase,
            _ => throw new InvalidEnumArgumentException(nameof(comparison), (int)comparison, typeof(StringComparison)),
        };

    public int Compare(string? x, string? y)
    {
        if (x is null || y is null)
            return string.Compare(x, y, comparison);

        var xs = GetSegments(x);
        var ys = GetSegments(y);

        while (xs.MoveNext() && ys.MoveNext()) {
            int cmp;
            if (xs.IsNumber && ys.IsNumber) {
                cmp = long.Parse(xs.Span).CompareTo(long.Parse(ys.Span));
                if (cmp != 0)
                    return cmp;
            }
            else if (xs.IsNumber)
                return -1;
            else if (ys.IsNumber)
                return 1;
            cmp = xs.Span.CompareTo(ys.Span, comparison);
            if (cmp != 0)
                return cmp;
        }

        return x.Length.CompareTo(y.Length);
    }

    private static StringSegmentEnumerator GetSegments(string s) => new(s);

    private struct StringSegmentEnumerator(string str)
    {
        private int _start = -1;
        private int _length = 0;

        public bool IsNumber { get; private set; } = false;

        public ReadOnlySpan<char> Span => str.AsSpan(_start, _length);

        public bool MoveNext()
        {
            var currentPosition = _start >= 0 ? _start + _length : 0;
            if (currentPosition >= str.Length)
                return false;

            int start = currentPosition;
            bool isFirstCharDigit = char.IsDigit(str[currentPosition]);

            while (++currentPosition < str.Length && char.IsDigit(str[currentPosition]) == isFirstCharDigit) { }

            _start = start;
            _length = currentPosition - start;
            IsNumber = isFirstCharDigit;
            return true;
        }
    }
}