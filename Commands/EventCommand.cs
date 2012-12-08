using System;
using System.Collections.Generic;
using System.Windows.Input;
using Alba.Framework.Events;
using ExecuteEventHandler = System.EventHandler<Alba.Framework.Commands.ExecuteEventArgs>;
using CanExecuteEventHandler = System.EventHandler<Alba.Framework.Commands.CanExecuteEventArgs>;

namespace Alba.Framework.Commands
{
    public class EventCommand
    {
        private bool _isAutoRequery = true;
        private List<WeakReference> _canExecuteChanged;
        private event ExecuteEventHandler _execute;
        private event CanExecuteEventHandler _canExecute;

        public EventCommand (string name, bool isAutoRequery = true)
        {
            Name = name;
            _isAutoRequery = isAutoRequery;
        }

        public void Subscribe (ExecuteEventHandler execute, CanExecuteEventHandler canExecute = null)
        {
            _execute += execute;
            if (canExecute != null)
                _canExecute += canExecute;
        }

        public EventCommand NoAutoRequery ()
        {
            IsAutoRequery = false;
            return this;
        }

        public bool CanExecute (object model, object parameter)
        {
            var args = new CanExecuteEventArgs(model, parameter);
            _canExecute.NullableInvoke(this, args);
            return args.CanExecute;
        }

        public void Execute (object model, object parameter)
        {
            _execute.NullableInvoke(this, new ExecuteEventArgs(model, parameter));
        }

        public string Name { get; private set; }

        public bool IsAutoRequery
        {
            get { return _isAutoRequery; }
            set
            {
                if (_isAutoRequery == value)
                    return;
                if (value)
                    WeakEvents.Call(_canExecuteChanged, h => CommandManager.RequerySuggested += h);
                else
                    WeakEvents.Call(_canExecuteChanged, h => CommandManager.RequerySuggested -= h);
                _isAutoRequery = value;
            }
        }

        public void RaiseCanExecuteChanged ()
        {
            OnCanExecuteChanged();
        }

        public virtual void OnCanExecuteChanged ()
        {
            WeakEvents.Call(_canExecuteChanged, h => h(this, EventArgs.Empty));
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_isAutoRequery)
                    CommandManager.RequerySuggested += value;
                WeakEvents.AddHandler(ref _canExecuteChanged, value, 2);
            }
            remove
            {
                if (_isAutoRequery)
                    CommandManager.RequerySuggested -= value;
                WeakEvents.RemoveHandler(_canExecuteChanged, value);
            }
        }
    }

    public class ExecuteEventArgs : EventArgs
    {
        public object Model { get; set; }
        public object Parameter { get; set; }

        internal ExecuteEventArgs (object model, object parameter)
        {
            Model = model;
            Parameter = parameter;
        }
    }

    public class CanExecuteEventArgs : ExecuteEventArgs
    {
        public bool CanExecute { get; set; }

        internal CanExecuteEventArgs (object model, object parameter) : base(model, parameter)
        {
            CanExecute = true;
        }
    }
}