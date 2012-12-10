using System;
using System.Collections.Generic;
using Alba.Framework.Commands;

namespace Alba.Framework.Mvvm
{
    public class ViewModelBase : ModelBase
    {
        protected void Subscribe (EventCommand command, Action execute, Func<bool> canExecute = null)
        {
            Commands.Commands.Subscribe(this, command, execute, canExecute);
        }

        protected void Subscribe<T> (EventCommand command, Action<T> execute, Func<T, bool> canExecute = null)
        {
            Commands.Commands.Subscribe(this, command, execute, canExecute);
        }
    }
}