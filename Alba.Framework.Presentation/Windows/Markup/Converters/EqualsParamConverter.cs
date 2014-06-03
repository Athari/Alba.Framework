using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Alba.Framework.Sys;

namespace Alba.Framework.Windows.Markup
{
    [ValueConversion (typeof(object), typeof(bool))]
    public class EqualsParamConverter : IValueConverter
    {
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue)
                return DependencyProperty.UnsetValue;
            return value.EqualsValue(parameter);
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool && (bool)value ? parameter : Binding.DoNothing;
        }
    }
}