using System.Windows;
using System.Windows.Data;

namespace Alba.Framework.Commands
{
    public class CommandExtension : Binding
    {
        public CommandExtension ()
        {
            Converter = new CommandConverter();
        }

        public CommandExtension (CommandDisplay display)
        {
            Display = display;
        }

        public CommandExtension (CommandDisplay display, string elementName)
        {
            Display = display;
            Path = new PropertyPath("DataContext");
            ElementName = elementName;
        }

        public CommandDisplay Display
        {
            get { return CommandConverter.Display; }
            set { CommandConverter.Display = value; }
        }

        public EventCommand Command
        {
            get { return CommandConverter.Command; }
            set { CommandConverter.Command = value; }
        }

        private CommandConverter CommandConverter
        {
            get { return (CommandConverter)(Converter ?? (Converter = new CommandConverter())); }
        }
    }
}