using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Alba.Framework.Text;

namespace Alba.Framework.Markup.Converters
{
    [ValueConversion (typeof(int), typeof(string))]
    [ValueConversion (typeof(long), typeof(string))]
    [ValueConversion (typeof(decimal), typeof(string))]
    public class ToByteSizeConverter : IValueConverter
    {
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (ReferenceEquals(value, null) || ReferenceEquals(value, DependencyProperty.UnsetValue))
                return DependencyProperty.UnsetValue;

            long i = System.Convert.ToInt32(value);
            double readable;
            string suffix;

            if (i < 0x400)
                return i.ToString("0 b", culture);
            else if (i < 0x100000) {
                suffix = "KB";
                readable = i;
            }
            else if (i < 0x40000000) {
                suffix = "MB";
                readable = i >> 10;
            }
            else if (i < 0x10000000000) {
                suffix = "GB";
                readable = i >> 20;
            }
            else if (i < 0x4000000000000) {
                suffix = "TB";
                readable = i >> 30;
            }
            else if (i < 0x1000000000000000) {
                suffix = "PB";
                readable = i >> 40;
            }
            else {
                suffix = "EB";
                readable = i >> 50;
            }
            readable = readable / 1024;

            return (readable < 10 ? "{0:0.00} {1}" : "{0:0.0} {1}").Fmt(culture, readable, suffix);
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}