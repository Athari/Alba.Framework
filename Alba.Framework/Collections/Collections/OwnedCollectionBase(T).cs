using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Alba.Framework.Collections
{
    public abstract class OwnedCollectionBase<T> : Collection<T>
        where T : class
    {
        protected OwnedCollectionBase ()
        {}

        protected OwnedCollectionBase (IList<T> list) : base(list)
        {}

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
    }
}