using System.ComponentModel;
using System.Globalization;

namespace Alba.Framework.ComponentModel;

[PublicAPI]
public static class TypeConverterExts
{
    public static string? ConvertToString(this TypeConverter @this, object value,
        CultureInfo? culture = null, ITypeDescriptorContext? context = null) =>
        (string?)@this.ConvertTo(context, culture ?? CultureInfo.CurrentCulture, value, typeof(string));

    public static T? ConvertFromString<T>(this TypeConverter @this, string value,
        CultureInfo? culture = null, ITypeDescriptorContext? context = null) =>
        (T?)@this.ConvertFrom(context, culture ?? CultureInfo.CurrentCulture, value);
}