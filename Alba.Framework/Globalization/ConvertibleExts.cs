using System;
using System.Globalization;

namespace Alba.Framework.Globalization
{
    public static partial class ConvertibleExts
    {
        public static Object ToType (this IConvertible @this, Type conversionType)
        {
            return @this.ToType(conversionType, CultureInfo.CurrentCulture);
        }

        public static Object ToTypeInv (this IConvertible @this, Type conversionType)
        {
            return @this.ToType(conversionType, CultureInfo.InvariantCulture);
        }

        public static String ToStringInv (this IConvertible @this)
        {
            return @this.ToString(CultureInfo.InvariantCulture);
        }
    }
}