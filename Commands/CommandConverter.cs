using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Alba.Framework.Commands
{
    internal class CommandConverter : IValueConverter
    {
        public EventCommand Command { get; set; }

        public CommandDisplay Display { get; set; }

        public CommandConverter ()
        {}

        public CommandConverter (CommandDisplay display)
        {
            Display = display;
        }

        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;
            return new EventCommandRef {
                Command = Command ?? Display.Command,
                Display = Display,
                Model = value,
            };
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}