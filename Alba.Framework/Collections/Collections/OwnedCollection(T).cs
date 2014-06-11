using System.Collections.Generic;
using Alba.Framework.Common;

namespace Alba.Framework.Collections
{
    public class OwnedCollection<T, TOwner> : OwnedCollectionBase<T>
        where T : class, IOwned<TOwner>
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

        protected override void OwnItem (T item)
        {
            item.Owner = _owner;
        }

        protected override void UnownItem (T item)
        {
            item.Owner = default (TOwner);
        }
    }
}