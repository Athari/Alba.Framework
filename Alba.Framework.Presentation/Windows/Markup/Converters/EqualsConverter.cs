using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Alba.Framework.Sys;

namespace Alba.Framework.Windows.Markup
{
    public class EqualsConverter : IMultiValueConverter
    {
        public object Convert (object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
                throw new ArgumentException("Two or more values required.", "values");
            if (values.Any(v => ReferenceEquals(v, DependencyProperty.UnsetValue)))
                return DependencyProperty.UnsetValue;
            return values.Skip(1).All(v => v.EqualsValue(values[0]));
        }

        public object[] ConvertBack (object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}