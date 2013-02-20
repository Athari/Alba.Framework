using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Alba.Framework.Markup.Converters
{
    [ValueConversion (typeof(ReadOnlyObservableCollection<ValidationError>), typeof(object))]
    public class ErrorContentConverter : IValueConverter
    {
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (ReferenceEquals(value, DependencyProperty.UnsetValue))
                return DependencyProperty.UnsetValue;

            var errors = (ReadOnlyObservableCollection<ValidationError>)value;
            return errors.Count > 0 ? errors[0].ErrorContent : null;
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}