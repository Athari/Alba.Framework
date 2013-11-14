using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Alba.Framework.Attributes;

namespace Alba.Framework.Windows.Mvvm
{
    public class LazyExpand<T> : INotifyPropertyChanged
    {
        private static readonly IEnumerable<T> Empty = new[] { default(T) };

        private Func<IEnumerable<T>> _getItems;
        private IEnumerable<T> _items = Empty;
        private bool _isExpanded;

        public event PropertyChangedEventHandler PropertyChanged;

        public LazyExpand (Func<IEnumerable<T>> getItems)
        {
            if (getItems == null)
                throw new ArgumentNullException("getItems");
            _getItems = getItems;
        }

        public bool IsEnumerated
        {
            get { return _getItems == null; }
        }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (_isExpanded == value)
                    return;
                if (value)
                    EnsureEnumerated();
                _isExpanded = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<T> Items
        {
            get { return _items; }
        }

        public void EnsureEnumerated ()
        {
            if (_getItems == null)
                return;
            _items = _getItems();
            _getItems = null;
            OnPropertyChanged("IsEnumerated");
            OnPropertyChanged("Items");
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged ([CallerMemberName] string propName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propName));
        }
    }
}