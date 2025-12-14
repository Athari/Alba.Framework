using System.ComponentModel;

namespace Alba.Framework.Collections;

public static class ComparerExts
{
    extension<T>(IComparer<T> @this)
    {
        public bool IsEqual(T a, T b) => @this.Compare(a, b) == 0;
        public bool IsLessThan(T a, T b) => @this.Compare(a, b) < 0;
        public bool IsLessThanOrEqual(T a, T b) => @this.Compare(a, b) <= 0;
        public bool IsGreaterThan(T a, T b) => @this.Compare(a, b) > 0;
        public bool IsGreaterThanOrEqual(T a, T b) => @this.Compare(a, b) >= 0;

        public IComparer<T> WithDirection(ListSortDirection direction = ListSortDirection.Ascending) =>
            direction == ListSortDirection.Ascending
                ? @this
                : new ReverseComparer<T>(@this);
    }

    private class ReverseComparer<T>(IComparer<T> source) : IComparer<T>
    {
        public int Compare(T? x, T? y) => source.Compare(y, x);
    }
}