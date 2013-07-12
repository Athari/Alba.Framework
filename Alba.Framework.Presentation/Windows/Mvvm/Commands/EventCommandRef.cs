using System;
using System.Windows.Input;

namespace Alba.Framework.Windows.Mvvm
{
    internal class EventCommandRef : ICommand
    {
        public EventCommand Command { get; set; }
        public EventCommandDisplay Display { get; set; }
        public IModel Model { get; set; }

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