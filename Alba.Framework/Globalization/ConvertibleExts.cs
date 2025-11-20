using System.Globalization;

namespace Alba.Framework.Globalization;

public static partial class ConvertibleExts
{
    extension(IConvertible @this)
    {
        public object ToType(Type conversionType, CultureInfo? culture = null) =>
            @this.ToType(conversionType, culture ?? CultureInfo.CurrentCulture);

        public object ToTypeInv(Type conversionType) =>
            @this.ToType(conversionType, CultureInfo.InvariantCulture);
    }
}