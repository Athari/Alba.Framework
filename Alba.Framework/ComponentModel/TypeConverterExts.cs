using System.ComponentModel;
using System.Globalization;

namespace Alba.Framework.ComponentModel;

public static class TypeConverterExts
{
    extension(TypeConverter @this)
    {
        public string? ConvertToString(object value,
            CultureInfo? culture = null, ITypeDescriptorContext? context = null) =>
            (string?)@this.ConvertTo(context, culture ?? CultureInfo.CurrentCulture, value, typeof(string));

        public T? ConvertFromString<T>(string value,
            CultureInfo? culture = null, ITypeDescriptorContext? context = null) =>
            (T?)@this.ConvertFrom(context, culture ?? CultureInfo.CurrentCulture, value);

        public string? ConvertToStringInv(object value, ITypeDescriptorContext? context = null) =>
            (string?)@this.ConvertTo(context, CultureInfo.InvariantCulture, value, typeof(string));

        public T? ConvertFromStringInv<T>(string value, ITypeDescriptorContext? context = null) =>
            (T?)@this.ConvertFrom(context, CultureInfo.InvariantCulture, value);
    }
}