using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Alba.Framework.Mvvm.Models;

namespace Alba.Framework.Mvvm.Commands
{
    internal class EventCommandConverter : IValueConverter
    {
        public EventCommand Command { get; set; }

        public EventCommandDisplay Display { get; set; }

        public EventCommandConverter ()
        {}

        public EventCommandConverter (EventCommandDisplay display)
        {
            Display = display;
        }

        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
                return DependencyProperty.UnsetValue;
            if (!(value is IModel))
                throw new ArgumentException("value must be IModel", "value");
            return new EventCommandRef {
                Command = Command ?? Display.Command,
                Display = Display,
                Model = (IModel)value,
            };
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}