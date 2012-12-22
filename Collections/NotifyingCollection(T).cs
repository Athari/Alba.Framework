using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Alba.Framework.Collections
{
    public class NotifyingCollection<T> : ObservableCollectionEx<T>, INotifyCollectionItemChanged
        where T : INotifyPropertyChanged
    {
        public event PropertyChangingEventHandler ItemPropertyChanging;
        public event PropertyChangedEventHandler ItemPropertyChanged;

        protected override void OnCollectionChanged (NotifyCollectionChangedEventArgs e)
        {
            Unsubscribe(e.OldItems);
            Subscribe(e.NewItems);
            base.OnCollectionChanged(e);
        }

        protected override void ClearItems ()
        {
            Unsubscribe(Items);
            base.ClearItems();
        }

        public override void Replace (IEnumerable<T> items)
        {
            var array = items.ToArray();
            Unsubscribe(Items);
            Subscribe(array);
            base.Replace(array);
        }

        private void Subscribe (IEnumerable items)
        {
            if (items == null)
                return;
            foreach (T item in items) {
                item.PropertyChanged += ItemPropertyChangedHandler;
                var changing = item as INotifyPropertyChanging;
                if (changing != null)
                    changing.PropertyChanging += ItemPropertyChangingHandler;
            }
        }

        private void Unsubscribe (IEnumerable items)
        {
            if (items == null)
                return;
            foreach (T item in items) {
                item.PropertyChanged -= ItemPropertyChangedHandler;
                var changing = item as INotifyPropertyChanging;
                if (changing != null)
                    changing.PropertyChanging -= ItemPropertyChangingHandler;
            }
        }

        private void ItemPropertyChangingHandler (object item, PropertyChangingEventArgs e)
        {
            OnItemPropertyChanging(item, e);
        }

        private void ItemPropertyChangedHandler (object item, PropertyChangedEventArgs e)
        {
            OnItemPropertyChanged(item, e);
        }

        protected virtual void OnItemPropertyChanging (object item, PropertyChangingEventArgs e)
        {
            var handler = ItemPropertyChanging;
            if (handler != null)
                handler(item, e);
        }

        protected virtual void OnItemPropertyChanged (object item, PropertyChangedEventArgs e)
        {
            var handler = ItemPropertyChanged;
            if (handler != null)
                handler(item, e);
        }
    }
}