using System;
using System.Linq;

namespace Alba.Framework.Sys
{
    public static class ObjectExts
    {
        public static bool Compare<T> (T x, T y, Func<bool> compare)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null) || x.GetType() != y.GetType())
                return false;
            return compare();
        }

        public static bool EqualsAny<T> (this T @this, params T[] values)
        {
            return values.Any(v => Equals(@this, v));
        }

        public static string GetTypeFullName (this object @this)
        {
            return @this == null ? "null" : @this.GetType().FullName;
        }

        public static T To<T> (this object @this)
        {
            return (T)@this;
        }

        public static string NullableToString (this object @this)
        {
            return @this == null ? "" : @this.ToString();
        }
    }
}