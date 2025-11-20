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
    }
}