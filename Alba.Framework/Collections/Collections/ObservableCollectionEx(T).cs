using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using NcChangedAction = System.Collections.Specialized.NotifyCollectionChangedAction;
using NcChangedEventArgs = System.Collections.Specialized.NotifyCollectionChangedEventArgs;

namespace Alba.Framework.Collections
{
    public class ObservableCollectionEx<T> : ObservableCollection<T>, INotifyPropertyChanged
    {
        protected const string CountPropertyName = "Count";
        protected const string IndexerPropertyName = "Item[]";

        public new event PropertyChangedEventHandler PropertyChanged
        {
            add { base.PropertyChanged += value; }
            remove { base.PropertyChanged -= value; }
        }

        public new T Add (T item)
        {
            base.Add(item);
            return item;
        }

        public virtual void AddRange (IEnumerable<T> items)
        {
            foreach (T item in items)
                Add(item);
        }

        public virtual void RemoveRange (IEnumerable<T> items)
        {
            foreach (T item in items)
                Remove(item);
        }

        public virtual void InsertRange (int index, IEnumerable<T> items)
        {
            foreach (T item in items)
                Insert(index++, item);
        }

        public virtual void Replace (IEnumerable<T> items)
        {
            Items.Clear();
            Items.AddRange(items);
            OnPropertyChanged(CountPropertyName);
            OnPropertyChanged(IndexerPropertyName);
            OnCollectionChanged(new NcChangedEventArgs(NcChangedAction.Reset));
        }

        protected void OnPropertyChanged (string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }
}