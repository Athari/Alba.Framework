using System;
using System.Windows.Input;

namespace Alba.Framework.Commands
{
    public class EventCommandRef : ICommand
    {
        public EventCommand Command { get; set; }
        public CommandDisplay Display { get; set; }
        public object Model { get; set; }

        bool ICommand.CanExecute (object parameter)
        {
            return Command.CanExecute(Model, parameter);
        }

        void ICommand.Execute (object parameter)
        {
            Command.Execute(Model, parameter);
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add { Command.CanExecuteChanged += value; }
            remove { Command.CanExecuteChanged -= value; }
        }
    }
}