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
                throw new InvalidOperationException("Model for command not specified.");
            return new EventCommandRef {
                Command = Command ?? Display.Command,
                Display = Display,
                Model = GetDataContext(value, false),
            };
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        private static object GetDataContext (object targetObject, bool throwIfNotControl = true)
        {
            object dataContext;
            var element = targetObject as FrameworkElement;
            if (element != null)
                dataContext = element.DataContext;
            else {
                var contentElement = targetObject as FrameworkContentElement;
                if (contentElement != null)
                    dataContext = contentElement.DataContext;
                else if (throwIfNotControl)
                    throw new InvalidOperationException(String.Format("Cannot get DataContext from {0}.", targetObject.GetType()));
                else
                    dataContext = targetObject;
            }
            return dataContext;
        }
    }
}