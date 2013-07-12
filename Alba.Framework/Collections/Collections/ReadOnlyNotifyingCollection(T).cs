using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Alba.Framework.Collections
{
    [Serializable]
    public class ReadOnlyNotifyingCollection<T> : ReadOnlyCollection<T>, INotifyCollectionChanged, INotifyPropertyChanged, INotifyCollectionItemChanged
        where T : INotifyPropertyChanged
    {
        [field: NonSerialized]
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler ItemPropertyChanging
        {
            add { ObservableItems.ItemPropertyChanging += value; }
            remove { ObservableItems.ItemPropertyChanging -= value; }
        }

        public event PropertyChangedEventHandler ItemPropertyChanged
        {
            add { ObservableItems.ItemPropertyChanged += value; }
            remove { ObservableItems.ItemPropertyChanged -= value; }
        }

        protected NotifyingCollection<T> ObservableItems
        {
            get { return (NotifyingCollection<T>)Items; }
        }

        public ReadOnlyNotifyingCollection (NotifyingCollection<T> list)
            : base(list)
        {
            ObservableItems.CollectionChanged += CollectionChangedHandler;
            ObservableItems.PropertyChanged += PropertyChangedHandler;
        }

        private void CollectionChangedHandler (object sender, NotifyCollectionChangedEventArgs args)
        {
            OnCollectionChanged(args);
        }

        private void PropertyChangedHandler (object sender, PropertyChangedEventArgs args)
        {
            OnPropertyChanged(args);
        }

        protected virtual void OnCollectionChanged (NotifyCollectionChangedEventArgs args)
        {
            var handler = CollectionChanged;
            if (handler != null)
                handler(this, args);
        }

        protected virtual void OnPropertyChanged (PropertyChangedEventArgs args)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, args);
        }
    }
}