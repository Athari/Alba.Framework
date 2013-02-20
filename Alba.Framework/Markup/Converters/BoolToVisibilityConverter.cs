using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Alba.Framework.Markup.Converters
{
    [ValueConversion (typeof(bool), typeof(Visibility), ParameterType = typeof(BoolToVisibilityMode))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        public BoolToVisibilityMode? Mode { get; set; }

        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (ReferenceEquals(value, DependencyProperty.UnsetValue))
                return DependencyProperty.UnsetValue;

            var visibility = (bool)value;
            var mode = Mode ?? (parameter != null ? (BoolToVisibilityMode)parameter : BoolToVisibilityMode.VisibleCollapsed);
            switch (mode) {
                case BoolToVisibilityMode.VisibleCollapsed:
                    return visibility ? Visibility.Visible : Visibility.Collapsed;
                case BoolToVisibilityMode.VisibleHidden:
                    return visibility ? Visibility.Visible : Visibility.Hidden;
                case BoolToVisibilityMode.CollapsedVisible:
                    return visibility ? Visibility.Collapsed : Visibility.Visible;
                case BoolToVisibilityMode.HiddenVisible:
                    return visibility ? Visibility.Hidden : Visibility.Visible;
                default:
                    throw new ArgumentOutOfRangeException("parameter");
            }
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public enum BoolToVisibilityMode
    {
        VisibleCollapsed,
        VisibleHidden,
        CollapsedVisible,
        HiddenVisible,
    }
}