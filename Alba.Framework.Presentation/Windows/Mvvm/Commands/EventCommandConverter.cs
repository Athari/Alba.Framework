using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Alba.Framework.Windows.Mvvm
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

            var model = value as IModel;
            if (model == null)
                throw new ArgumentException("value must be IModel", "value");
            EventCommand command = Command ?? Display.Command;

            var commandRef = new EventCommandRef {
                Command = command,
                Display = Display,
                Model = model,
            };
            EventCommands.AddCommandRef(model, command, commandRef);
            return commandRef;
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}