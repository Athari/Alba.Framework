using System;
using System.Collections.Generic;
using Alba.Framework.Commands;

namespace Alba.Framework.Mvvm
{
    public class ViewModelBase : ModelBase
    {
        private readonly List<object> refs = new List<object>();

        protected void Subscribe (EventCommand command, Action execute, Func<bool> canExecute = null)
        {
            Commands.Commands.Subscribe(refs, this, command, execute, canExecute);
        }

        protected void Subscribe<T> (EventCommand command, Action<T> execute, Func<T, bool> canExecute = null)
        {
            Commands.Commands.Subscribe(refs, this, command, execute, canExecute);
        }
    }
}