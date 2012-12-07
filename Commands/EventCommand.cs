using System;
using System.Collections.Generic;
using System.Windows.Input;
using Alba.Framework.Events;

namespace Alba.Framework.Commands
{
    public class EventCommand : ICommand
    {
        private event EventHandler _execute;
        private event EventHandler<CanExecuteEventArgs> _canExecute;
        private List<WeakReference> _canExecuteChanged;
        private bool _isAutomaticRequery = true;

        public EventCommand (bool isAutomaticRequery = true)
        {
            _isAutomaticRequery = isAutomaticRequery;
        }

        public EventCommand Subscribe (Action execute, Func<bool> canExecute = null)
        {
            _execute += (s, a) => execute();
            if (canExecute != null)
                _canExecute += (s, a) => { a.CanExecute = canExecute(); };
            return this;
        }

        public EventCommand NoAutoRequery ()
        {
            IsAutomaticRequery = false;
            return this;
        }

        public bool CanExecute ()
        {
            var can = new CanExecuteEventArgs();
            _canExecute.NullableInvoke(this, can);
            return can.CanExecute;
        }

        public void Execute ()
        {
            _execute.NullableInvoke(this);
        }

        public bool IsAutomaticRequery
        {
            get { return _isAutomaticRequery; }
            set
            {
                if (_isAutomaticRequery == value)
                    return;
                if (value)
                    WeakEvents.Call(_canExecuteChanged, h => CommandManager.RequerySuggested += h);
                else
                    WeakEvents.Call(_canExecuteChanged, h => CommandManager.RequerySuggested -= h);
                _isAutomaticRequery = value;
            }
        }

        public void RaiseCanExecuteChanged ()
        {
            OnCanExecuteChanged();
        }

        protected virtual void OnCanExecuteChanged ()
        {
            WeakEvents.Call(_canExecuteChanged, h => h(this, EventArgs.Empty));
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_isAutomaticRequery)
                    CommandManager.RequerySuggested += value;
                WeakEvents.AddHandler(ref _canExecuteChanged, value, 2);
            }
            remove
            {
                if (_isAutomaticRequery)
                    CommandManager.RequerySuggested -= value;
                WeakEvents.RemoveHandler(_canExecuteChanged, value);
            }
        }

        bool ICommand.CanExecute (object parameter)
        {
            return CanExecute();
        }

        void ICommand.Execute (object parameter)
        {
            Execute();
        }

        private class CanExecuteEventArgs : EventArgs
        {
            public bool CanExecute { get; set; }

            public CanExecuteEventArgs (bool canExecute = true)
            {
                CanExecute = canExecute;
            }
        }
    }
}