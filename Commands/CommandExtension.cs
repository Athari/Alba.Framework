using System.Windows;
using System.Windows.Data;

namespace Alba.Framework.Commands
{
    public class CommandExtension : Binding
    {
        public EventCommand Command { get; set; }

        private CommandDisplay _display;
        public CommandDisplay Display
        {
            get { return _display; }
            set
            {
                _display = value;
                CommandConverter.Display = _display;
            }
        }

        public CommandExtension ()
        {}

        public CommandExtension (CommandDisplay display)
        {
            Display = display;
        }

        public CommandExtension (string elementName, CommandDisplay display)
        {
            Path = new PropertyPath("DataContext");
            ElementName = elementName;
            Display = display;
        }

        private CommandConverter CommandConverter
        {
            get { return (CommandConverter)(Converter ?? (Converter = new CommandConverter(Display))); }
        }
    }
}