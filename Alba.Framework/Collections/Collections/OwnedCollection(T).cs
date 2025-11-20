using Alba.Framework.Common;

namespace Alba.Framework.Collections;

public class OwnedCollection<T, TOwner>(TOwner owner) : OwnedCollectionBase<T>
    where T : class, IOwned<TOwner>
{
    protected TOwner Owner { get; } = owner;

    protected override void OwnItem(T item) => item.SetNewOwner(Owner);

    protected override void UnownItem(T item) => item.ResetOwner();
}