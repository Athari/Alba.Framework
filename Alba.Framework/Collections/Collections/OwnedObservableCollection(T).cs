using Alba.Framework.Common;

namespace Alba.Framework.Collections
{
    public class OwnedObservableCollection<T, TOwner> : OwnedObservableCollectionBase<T>
        where T : class, IOwned<TOwner>
    {
        private readonly TOwner _owner;

        protected TOwner Owner
        {
            get { return _owner; }
        }

        public OwnedObservableCollection (TOwner owner)
        {
            _owner = owner;
        }

        protected override void OwnItem (T item)
        {
            item.SetNewOwner(_owner);
        }

        protected override void UnownItem (T item)
        {
            item.ResetOwner();
        }
    }
}