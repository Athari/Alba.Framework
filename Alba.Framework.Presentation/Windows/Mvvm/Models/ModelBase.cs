using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows.Threading;
using Alba.Framework.Attributes;
using Alba.Framework.Collections;
using Alba.Framework.Reflection;
using Alba.Framework.Sys;

// ReSharper disable ExplicitCallerInfoArgument
// ReSharper disable ReturnTypeCanBeEnumerable.Local
namespace Alba.Framework.Windows.Mvvm
{
    public abstract class ModelBase : IModel, INotifyPropertyChanging, INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private static readonly string HasErrors_PropName = GetName((ModelBase o) => o.HasErrors);

        private IDictionary<string, List<ValidationMessage>> _validationMessages;

        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected ModelBase ()
        {
            Dispatcher = Dispatcher.CurrentDispatcher;
        }

        [IgnoreDataMember]
        public Dispatcher Dispatcher { get; protected set; }

        [IgnoreDataMember]
        public bool HasErrors
        {
            get
            {
                VerifyAccess();
                return _validationMessages != null && _validationMessages.Count > 0;
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanging ([CallerMemberName] string propName = null)
        {
            var handler = PropertyChanging;
            if (handler != null)
                handler(this, new PropertyChangingEventArgs(propName));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged ([CallerMemberName] string propName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propName));
        }

        protected void OnErrorsChanged (string propName = null)
        {
            var handler = ErrorsChanged;
            if (handler != null)
                handler(this, new DataErrorsChangedEventArgs(propName));
        }

        [NotifyPropertyChangedInvocator ("propName")]
        protected bool Set<T> (ref T field, T value, [CallerMemberName] string propName = null)
        {
            VerifyAccess();
            if (field.EqualsValue(value))
                return false;
            OnPropertyChanging(propName);
            field = value;
            OnPropertyChanged(propName);
            return true;
        }

        [NotifyPropertyChangedInvocator ("propNames")]
        protected bool Set<T> (ref T field, T value, params string[] propNames)
        {
            VerifyAccess();
            if (field.EqualsValue(value))
                return false;
            foreach (string prop in propNames)
                OnPropertyChanging(prop);
            field = value;
            foreach (string prop in propNames)
                OnPropertyChanged(prop);
            return true;
        }

        [NotifyPropertyChangedInvocator ("propName")]
        protected bool SetAndValidate<T> (ref T field, T value, Action validate, [CallerMemberName] string propName = null)
        {
            bool result = Set(ref field, value, propName);
            if (result)
                Validate(validate, propName);
            return result;
        }

        protected T Get<T> (ref T field)
        {
            VerifyAccess();
            return field;
        }

        protected void VerifyAccess ()
        {
            if (Dispatcher != null)
                Dispatcher.VerifyAccess();
        }

        public IDisposable NoAccessCheck (Dispatcher newDispatcher = null)
        {
            Dispatcher oldDispatcher = Dispatcher;
            Dispatcher = null;
            return Disposable.Create(() => { Dispatcher = newDispatcher ?? oldDispatcher; });
        }

        private IReadOnlyList<ValidationMessage> GetErrors (string propName)
        {
            if (!HasErrors)
                return ValidationMessage.EmptyCollection;
            List<ValidationMessage> messages = _validationMessages.GetOrDefault(propName);
            return messages != null ? messages.AsReadOnly() : ValidationMessage.EmptyCollection;
        }

        IEnumerable INotifyDataErrorInfo.GetErrors (string propName)
        {
            return GetErrors(propName);
        }

        private void Validate (Action validate, string propName)
        {
            bool oldHasErrors = HasErrors;
            IReadOnlyList<ValidationMessage> oldMessages = GetErrors(propName);

            RemoveValidationMessages(propName);
            validate();

            if (HasErrors != oldHasErrors) {
                OnPropertyChanging(HasErrors_PropName); // incorrect, because HasErrors value has already changed, but who cares...
                OnPropertyChanged(HasErrors_PropName);
            }

            if (!GetErrors(propName).SequenceEqual(oldMessages)) {
                OnErrorsChanged(propName);
            }
        }

        protected void AddValidationMessage (ValidationMessage message, string propName)
        {
            VerifyAccess();
            EnsureValidationMessages();
            List<ValidationMessage> propMessages = _validationMessages.GetOrAdd(propName, () => new List<ValidationMessage>(1));
            int indexSameCodeMessage = propMessages.IndexOf(m => m.Code == message.Code);
            if (indexSameCodeMessage == -1)
                propMessages.Add(message);
            else
                propMessages[indexSameCodeMessage] = message;
        }

        protected void AddValidationMessage (string message, int code = 0, [CallerMemberName] string propName = null)
        {
            AddValidationMessage(new ValidationMessage { Message = message, Code = code }, propName);
        }

        private void RemoveValidationMessages ([CallerMemberName] string propName = null)
        {
            if (!HasErrors)
                return;
            _validationMessages.Remove(propName);
        }

        private void EnsureValidationMessages ()
        {
            if (_validationMessages == null)
                _validationMessages = new SortedList<string, List<ValidationMessage>>();
        }

        protected static string GetName<TObj, T> (Expression<Func<TObj, T>> propExpr)
        {
            return Props.GetName(propExpr);
        }

        protected static string GetName<T> (Expression<Func<T>> propExpr)
        {
            return Props.GetName(propExpr);
        }
    }
}