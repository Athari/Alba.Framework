using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Alba.Framework.Text;

namespace Alba.Framework.Markup.Converters
{
    [ValueConversion (typeof(object), typeof(string))]
    public class StringFormatConverter : IValueConverter
    {
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (ReferenceEquals(value, DependencyProperty.UnsetValue))
                return DependencyProperty.UnsetValue;
            return ((string)parameter).Fmt(culture, value);
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}