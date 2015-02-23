using System;
using System.Globalization;
using System.Windows.Data;
using Alba.Framework.Text;

namespace Alba.Framework.Windows.Markup
{
    [ValueConversion (typeof(object), typeof(string))]
    public class StringFormatConverter : IValueConverter, IMultiValueConverter
    {
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)parameter).Fmt(culture, value);
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public object Convert (object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)parameter).Fmt(culture, values);
        }

        public object[] ConvertBack (object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}