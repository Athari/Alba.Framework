using System.Collections.Generic;
using System.Collections.ObjectModel;
using Alba.Framework.Common;

namespace Alba.Framework.Collections
{
    public class OwnedCollection<T, TOwner> : Collection<T>
        where T : IOwned<TOwner>
    {
        private readonly TOwner _owner;

        public OwnedCollection (TOwner owner)
        {
            _owner = owner;
        }

        public OwnedCollection (IList<T> list, TOwner owner) : base(list)
        {
            _owner = owner;
        }

        protected override void InsertItem (int index, T item)
        {
            base.InsertItem(index, item);
            item.Owner = _owner;
        }

        protected override void RemoveItem (int index)
        {
            Items[index].Owner = default(TOwner);
            base.RemoveItem(index);
        }

        protected override void SetItem (int index, T item)
        {
            Items[index].Owner = default(TOwner);
            base.SetItem(index, item);
            Items[index].Owner = _owner;
        }

        protected override void ClearItems ()
        {
            for (int i = 0; i < Count; i++)
                Items[i].Owner = default(TOwner);
            base.ClearItems();
        }
    }
}