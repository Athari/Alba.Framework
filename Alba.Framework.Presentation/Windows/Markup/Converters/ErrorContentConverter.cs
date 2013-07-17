using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Alba.Framework.Windows.Markup
{
    [ValueConversion (typeof(ReadOnlyObservableCollection<ValidationError>), typeof(object))]
    public class ErrorContentConverter : IValueConverter
    {
        public bool IsMultiple { get; set; }

        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (ReferenceEquals(value, DependencyProperty.UnsetValue))
                return DependencyProperty.UnsetValue;

            var errors = (ReadOnlyObservableCollection<ValidationError>)value;
            if (errors.Count == 0)
                return null;
            return IsMultiple ? errors.Select(e => e.ErrorContent) : errors.Single().ErrorContent;
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}