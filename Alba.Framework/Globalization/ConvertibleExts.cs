using System.Globalization;

namespace Alba.Framework.Globalization;

[PublicAPI]
public static partial class ConvertibleExts
{
    public static object ToType(this IConvertible @this, Type conversionType, CultureInfo? culture = null) =>
        @this.ToType(conversionType, culture ?? CultureInfo.CurrentCulture);

    public static object ToTypeInv(this IConvertible @this, Type conversionType) =>
        @this.ToType(conversionType, CultureInfo.InvariantCulture);

    public static string ToStringInv(this IConvertible @this) =>
        @this.ToString(CultureInfo.InvariantCulture);
}