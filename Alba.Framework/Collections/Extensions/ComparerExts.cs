using System.Collections.Generic;

namespace Alba.Framework.Collections
{
    public static class ComparerExts
    {
        public static bool IsEqual<T> (this IComparer<T> @this, T a, T b)
        {
            return @this.Compare(a, b) == 0;
        }

        public static bool IsLessThan<T> (this IComparer<T> @this, T a, T b)
        {
            return @this.Compare(a, b) < 0;
        }

        public static bool IsLessThanOrEqual<T> (this IComparer<T> @this, T a, T b)
        {
            return @this.Compare(a, b) <= 0;
        }

        public static bool IsMoreThan<T> (this IComparer<T> @this, T a, T b)
        {
            return @this.Compare(a, b) > 0;
        }

        public static bool IsMoreThanOrEqual<T> (this IComparer<T> @this, T a, T b)
        {
            return @this.Compare(a, b) >= 0;
        }
    }
}