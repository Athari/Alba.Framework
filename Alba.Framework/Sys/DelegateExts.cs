using System;

// ReSharper disable RedundantLambdaSignatureParentheses
namespace Alba.Framework.Sys
{
    public static class DelegateExts
    {
        public static Func<T1> Merge<T1> (this Func<T1> @this, Func<T1> other, Func<T1, T1, T1> merge)
        {
            if (@this == null)
                return other;
            if (other == null)
                return @this;
            return () => merge(@this(), other());
        }

        public static Func<T1, T2> Merge<T1, T2> (this Func<T1, T2> @this, Func<T1, T2> other, Func<T2, T2, T2> merge)
        {
            if (@this == null)
                return other;
            if (other == null)
                return @this;
            return (t1) => merge(@this(t1), other(t1));
        }

        public static Func<T1, T2, T3> Merge<T1, T2, T3> (this Func<T1, T2, T3> @this, Func<T1, T2, T3> other, Func<T3, T3, T3> merge)
        {
            if (@this == null)
                return other;
            if (other == null)
                return @this;
            return (t1, t2) => merge(@this(t1, t2), other(t1, t2));
        }

        public static Predicate<T> Merge<T> (this Predicate<T> @this, Predicate<T> other, Func<bool, bool, bool> merge)
        {
            if (@this == null)
                return other;
            if (other == null)
                return @this;
            return (t1) => merge(@this(t1), other(t1));
        }
    }
}