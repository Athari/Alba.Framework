using System;
using System.Globalization;

namespace Alba.Framework.Sys
{
    public static class ConvertibleExts
    {
        public static string ToStringInvariant (this IConvertible @this)
        {
            return @this.ToString(CultureInfo.InvariantCulture);
        }
    }
}