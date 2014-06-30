using System.Collections.Generic;
using NcChangedAction = System.Collections.Specialized.NotifyCollectionChangedAction;
using NcChangedEventArgs = System.Collections.Specialized.NotifyCollectionChangedEventArgs;

namespace Alba.Framework.Collections
{
    public abstract class OwnedObservableCollectionBase<T> : ObservableCollectionEx<T>, IOwnedList<T>
        where T : class
    {
        protected abstract void OwnItem (T item);

        protected abstract void UnownItem (T item);

        protected override void InsertItem (int index, T item)
        {
            base.InsertItem(index, item);
            OwnItem(item);
        }

        protected override void RemoveItem (int index)
        {
            UnownItem(Items[index]);
            base.RemoveItem(index);
        }

        protected override void SetItem (int index, T item)
        {
            UnownItem(Items[index]);
            base.SetItem(index, item);
            OwnItem(Items[index]);
        }

        protected override void ClearItems ()
        {
            for (int i = 0; i < Count; i++)
                UnownItem(Items[i]);
            base.ClearItems();
        }

        public override void Replace (IEnumerable<T> items)
        {
            for (int i = 0; i < Count; i++)
                UnownItem(Items[i]);
            Items.Clear();
            Items.AddRange(items);
            for (int i = 0; i < Count; i++)
                OwnItem(Items[i]);
            OnPropertyChanged(CountPropertyName);
            OnPropertyChanged(IndexerPropertyName);
            OnCollectionChanged(new NcChangedEventArgs(NcChangedAction.Reset));
        }

        public void SwapAt (int index, int indexOther)
        {
            T temp = this[index];
            base.SetItem(index, this[indexOther]);
            base.SetItem(indexOther, temp);
        }
    }
}