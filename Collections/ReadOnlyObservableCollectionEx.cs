using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Alba.Framework.Collections
{
    [Serializable]
    public class ReadOnlyObservableCollectionEx<T> : ReadOnlyCollection<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        [field: NonSerialized]
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected ObservableCollectionEx<T> ObservableItems
        {
            get { return (ObservableCollectionEx<T>)Items; }
        }

        public ReadOnlyObservableCollectionEx (ObservableCollectionEx<T> list)
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
            if (CollectionChanged != null)
                CollectionChanged(this, args);
        }

        protected virtual void OnPropertyChanged (PropertyChangedEventArgs args)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, args);
        }
    }
}