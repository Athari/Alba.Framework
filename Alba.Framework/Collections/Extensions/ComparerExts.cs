namespace Alba.Framework.Collections;

[PublicAPI]
public static class ComparerExts
{
    public static bool IsEqual<T>(this IComparer<T> @this, T a, T b) => @this.Compare(a, b) == 0;
    public static bool IsLessThan<T>(this IComparer<T> @this, T a, T b) => @this.Compare(a, b) < 0;
    public static bool IsLessThanOrEqual<T>(this IComparer<T> @this, T a, T b) => @this.Compare(a, b) <= 0;
    public static bool IsGreaterThan<T>(this IComparer<T> @this, T a, T b) => @this.Compare(a, b) > 0;
    public static bool IsGreaterThanOrEqual<T>(this IComparer<T> @this, T a, T b) => @this.Compare(a, b) >= 0;
}