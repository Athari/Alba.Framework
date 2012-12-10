using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Alba.Framework.Mvvm
{
    public class ModelBase : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

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

        [NotifyPropertyChangedInvocator ("propName")]
        protected bool Set<T> (ref T field, T value, [CallerMemberName] string propName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            OnPropertyChanging(propName);
            field = value;
            OnPropertyChanged(propName);
            return true;
        }

        [NotifyPropertyChangedInvocator ("propNames")]
        protected bool Set<T> (ref T field, T value, params string[] propNames)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            foreach (string prop in propNames)
                OnPropertyChanging(prop);
            field = value;
            foreach (string prop in propNames)
                OnPropertyChanged(prop);
            return true;
        }
    }
}