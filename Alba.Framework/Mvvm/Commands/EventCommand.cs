using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Threading;
using Alba.Framework.Events;
using Alba.Framework.Mvvm.Models;
using Alba.Framework.Sys;

namespace Alba.Framework.Mvvm.Commands
{
    public class EventCommand
    {
        private bool _isAutoRequery = true;
        private List<WeakReference> _canExecuteChanged;
        private event EventHandler<ExecuteEventArgs> _execute;
        private event EventHandler<CanExecuteEventArgs> _canExecute;

        public EventCommand (string name, bool isAutoRequery = true)
        {
            Name = name;
            _isAutoRequery = isAutoRequery;
        }

        public void Subscribe (EventHandler<ExecuteEventArgs> execute, EventHandler<CanExecuteEventArgs> canExecute = null)
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

        public bool CanExecute (IModel model, object parameter)
        {
            var args = new CanExecuteEventArgs(model, parameter);
            _canExecute.NullableInvoke(this, args);
            return args.CanExecute;
        }

        public void Execute (IModel model, object parameter)
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

        public void RaiseCanExecuteChanged (IModel model)
        {
            if (model.Dispatcher != null)
                model.Dispatcher.QueueExecute(DispatcherPriority.Background, () => OnCanExecuteChanged(model));
            else
                OnCanExecuteChanged(model);
        }

        protected virtual void OnCanExecuteChanged (IModel model)
        {
            foreach (EventCommandRef commandRef in EventCommands.GetCommandRefs(model, this))
                WeakEvents.Call(_canExecuteChanged, h => h(commandRef, EventArgs.Empty));
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
        public IModel Model { get; set; }
        public object Parameter { get; set; }

        internal ExecuteEventArgs (IModel model, object parameter)
        {
            Model = model;
            Parameter = parameter;
        }
    }

    public class CanExecuteEventArgs : ExecuteEventArgs
    {
        public bool CanExecute { get; set; }

        internal CanExecuteEventArgs (IModel model, object parameter) : base(model, parameter)
        {
            CanExecute = true;
        }
    }
}