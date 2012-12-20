using System;
using Alba.Framework.Mvvm.Commands;

namespace Alba.Framework.Mvvm.Models
{
    public class ViewModelBase : ModelBase
    {
        protected void Subscribe (EventCommand command, Action execute, Func<bool> canExecute = null)
        {
            EventCommands.Subscribe(this, command, execute, canExecute);
        }

        protected void Subscribe<T> (EventCommand command, Action<T> execute, Func<T, bool> canExecute = null)
        {
            EventCommands.Subscribe(this, command, execute, canExecute);
        }

        protected void Unsubscribe (EventCommand command)
        {
            EventCommands.Unsubscribe(this, command);
        }

        protected void Unsubscribe ()
        {
            EventCommands.Unsubscribe(this);
        }
    }
}