using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Alba.Framework.Reflection;
using Alba.Framework.Text;
using Newtonsoft.Json.Linq;

// ReSharper disable CheckNamespace
namespace Alba.Framework.Windows.Markup.Json
{
    [ValueConversion (typeof(JValue), typeof(object))]
    public class JsonTokenConverter : IValueConverter
    {
        public JTokenType TokenType { get; set; }

        public JsonTokenConverter ()
        {
            TokenType = JTokenType.String;
        }

        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue || value == null)
                return DependencyProperty.UnsetValue;
            try {
                return ((JValue)value).Value;
            }
            catch {
                return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue)
                return DependencyProperty.UnsetValue;

            try {
                switch (TokenType) {
                    case JTokenType.Integer:
                        return new JValue(Convert<long>(value));
                    case JTokenType.Float:
                        return new JValue(Convert<double>(value));
                    case JTokenType.String:
                        return new JValue(Convert<string>(value));
                    case JTokenType.Boolean:
                        return new JValue(Convert<bool>(value));
                    case JTokenType.Null:
                        return new JValue((object)null);
                    case JTokenType.Date:
                        return new JValue(Convert<DateTime>(value));
                    case JTokenType.Bytes:
                        return new JValue(Convert<byte[]>(value));
                    case JTokenType.Guid:
                        return new JValue(Convert<Guid>(value));
                    case JTokenType.Uri:
                        return new JValue(Convert<Uri>(value));
                    case JTokenType.TimeSpan:
                        return new JValue(Convert<TimeSpan>(value));
                    default:
                        throw new NotSupportedException("Token type '{0}' not supported.".Fmt(TokenType));
                }
            }
            catch {
                return DependencyProperty.UnsetValue;
            }
        }

        private static object Convert<TTarget> (object value)
        {
            return Convert(value, typeof(TTarget));
        }

        private static object Convert (object value, Type targeType)
        {
            if (value == null)
                return null;
            Type sourceType = value.GetType();

            if (sourceType.Is<IConvertible>() && targeType.Is<IConvertible>())
                return System.Convert.ChangeType(value, targeType);

            TypeConverter conv = TypeDescriptor.GetConverter(sourceType);
            if (conv.CanConvertTo(targeType))
                return conv.ConvertTo(value, targeType);

            conv = TypeDescriptor.GetConverter(targeType);
            if (conv.CanConvertFrom(sourceType))
                return conv.ConvertFrom(value);

            throw new InvalidOperationException("Can't convert '{0}' to '{1}'.".Fmt(sourceType.GetFullName(), targeType.GetFullName()));
        }
    }
}