using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Alba.Framework.Markup.Converters
{
    [ValueConversion (typeof(object), typeof(IEnumerable<object>))]
    public class YieldConverter : IValueConverter
    {
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (ReferenceEquals(value, DependencyProperty.UnsetValue))
                return DependencyProperty.UnsetValue;
            return EnumerableEx.Return(value);
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (ReferenceEquals(value, DependencyProperty.UnsetValue))
                return DependencyProperty.UnsetValue;
            return ((IEnumerable<object>)value).Single();
        }
    }
}