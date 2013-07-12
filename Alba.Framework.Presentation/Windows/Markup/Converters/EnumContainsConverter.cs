using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Alba.Framework.Windows.Markup
{
    [ValueConversion (typeof(object), typeof(bool))]
    public class EnumContainsConverter : MarkupExtension, IValueConverter
    {
        public object Value { get; set; }

        public EnumContainsConverter ()
        {}

        public EnumContainsConverter (object value)
        {
            Value = value;
        }

        public object Convert (object value, Type targetType, object param, CultureInfo culture)
        {
            if (ReferenceEquals(value, DependencyProperty.UnsetValue))
                return DependencyProperty.UnsetValue;
            var uvalue = (int)value;
            var uparam = (int)(param ?? Value);
            return (uvalue & uparam) == uparam;
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public override object ProvideValue (IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}