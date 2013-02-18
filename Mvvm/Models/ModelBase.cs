using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using Alba.Framework.Attributes;
using Alba.Framework.Linq;
using Alba.Framework.Events;
using Alba.Framework.Text;

namespace Alba.Framework.Mvvm.Models
{
    public abstract class ModelBase<TSelf> : IModel, INotifyPropertyChanging, INotifyPropertyChanged, IDataErrorInfo
        where TSelf : ModelBase<TSelf>
    {
        // ReSharper disable StaticFieldInGenericType
        private static readonly string IsLoading_PropName = GetName(o => o.IsLoading);
        // ReSharper restore StaticFieldInGenericType

        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler IsLoadingChanged;

        protected ModelBase ()
        {
            Dispatcher = Dispatcher.CurrentDispatcher;
        }

        public Dispatcher Dispatcher { get; private set; }

        public bool IsValid
        {
            get { return ValidatedProps.All(propName => ValidateProp(propName).IsNullOrEmpty()); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return Get(ref _isLoading); }
            set { Set(ref _isLoading, value); }
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
            if (propName == IsLoading_PropName)
                IsLoadingChanged.NullableInvoke(this);
        }

        [NotifyPropertyChangedInvocator ("propName")]
        protected bool Set<T> (ref T field, T value, [CallerMemberName] string propName = null)
        {
            VerifyAccess();
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            OnPropertyChanging(propName);
            field = value;
            OnPropertyChanged(propName);
            return true;
        }

        [NotifyPropertyChangedInvocator ("propName")]
        protected bool SetAndValidate<T> (ref T field, T value, [CallerMemberName] string propName = null)
        {
            VerifyAccess();
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            OnPropertyChanging(propName);
            OnPropertyChanging("IsValid");
            field = value;
            OnPropertyChanged(propName);
            OnPropertyChanged("IsValid");
            return ValidateProp(propName) == null;
        }

        [NotifyPropertyChangedInvocator ("propNames")]
        protected bool Set<T> (ref T field, T value, params string[] propNames)
        {
            VerifyAccess();
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            foreach (string prop in propNames)
                OnPropertyChanging(prop);
            field = value;
            foreach (string prop in propNames)
                OnPropertyChanged(prop);
            return true;
        }

        [NotifyPropertyChangedInvocator ("propNames")]
        protected bool SetAndValidate<T> (ref T field, T value, params string[] propNames)
        {
            VerifyAccess();
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            foreach (string prop in propNames)
                OnPropertyChanging(prop);
            OnPropertyChanging("IsValid");
            field = value;
            foreach (string prop in propNames)
                OnPropertyChanged(prop);
            OnPropertyChanged("IsValid");
            return ValidateProp(propNames[0]) == null;
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

        protected virtual string[] ValidatedProps
        {
            get { return new string[0]; }
        }

        protected virtual string ValidateProp (string propName)
        {
            return null;
        }

        string IDataErrorInfo.this [string propName]
        {
            get { return ValidateProp(propName); }
        }

        string IDataErrorInfo.Error
        {
            get { return null; }
        }

        protected static string GetName<T> (Expression<Func<T>> propExpr)
        {
            return Props.GetName(propExpr);
        }

        protected static string GetName<T> (Expression<Func<TSelf, T>> propExpr)
        {
            return Props.GetName(propExpr);
        }
    }
}