using System.Windows;
using System.Windows.Data;
using Alba.Framework.Windows.Mvvm;

namespace Alba.Framework.Windows.Markup
{
    public class CommandExtension : Binding
    {
        public CommandExtension ()
        {
            Converter = new EventCommandConverter();
        }

        public CommandExtension (EventCommandDisplay display)
        {
            Display = display;
        }

        public CommandExtension (EventCommandDisplay display, string elementName)
        {
            Display = display;
            Path = new PropertyPath("DataContext");
            ElementName = elementName;
        }

        public EventCommandDisplay Display
        {
            get { return CommandConverter.Display; }
            set { CommandConverter.Display = value; }
        }

        public EventCommand Command
        {
            get { return CommandConverter.Command; }
            set { CommandConverter.Command = value; }
        }

        private EventCommandConverter CommandConverter
        {
            get { return (EventCommandConverter)(Converter ?? (Converter = new EventCommandConverter())); }
        }
    }
}