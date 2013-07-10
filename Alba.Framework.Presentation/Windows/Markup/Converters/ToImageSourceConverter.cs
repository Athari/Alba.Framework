using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Alba.Framework.Text;

namespace Alba.Framework.Windows.Markup.Converters
{
    public class ToImageSourceConverter : IValueConverter
    {
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value.ToString();
            var strParam = parameter as string;
            if (strParam != null)
                str = strParam.Fmt(culture, str);
            return new BitmapImage(new Uri(str, UriKind.RelativeOrAbsolute));
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}