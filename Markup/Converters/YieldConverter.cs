using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Alba.Framework.Markup.Converters
{
    [ValueConversion (typeof(object), typeof(IEnumerable<object>))]
    public class YieldConverter : IValueConverter, IMultiValueConverter
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

        public object Convert (object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
                throw new ArgumentException("Two values expected: single item and boolean condition.", "values");
            if (values.Any(v => ReferenceEquals(v, DependencyProperty.UnsetValue)))
                return DependencyProperty.UnsetValue;
            return (bool)values[1] ? EnumerableEx.Return(values[0]) : null;
        }

        public object[] ConvertBack (object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (ReferenceEquals(value, DependencyProperty.UnsetValue))
                return new[] { DependencyProperty.UnsetValue };
            var item = ((IEnumerable<object>)value).SingleOrDefault();
            return new[] { item, item != null };
        }
    }
}