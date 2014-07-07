using System.ComponentModel;
using System.Globalization;

namespace Alba.Framework.ComponentModel
{
    public static class TypeConverterExts
    {
        public static string ConvertToString (this TypeConverter @this, object value,
            CultureInfo culture = null, ITypeDescriptorContext context = null)
        {
            return (string)@this.ConvertTo(context, culture ?? CultureInfo.CurrentCulture, value, typeof(string));
        }

        public static T ConvertFromString<T> (this TypeConverter @this, string value,
            CultureInfo culture = null, ITypeDescriptorContext context = null)
        {
            return (T)@this.ConvertFrom(context, culture ?? CultureInfo.CurrentCulture, value);
        }
    }
}