using Alba.Framework.Collections;

namespace Alba.Framework.Common;

public static class OwnedExts
{
    extension<TOwner>(IOwned<TOwner> @this)
    {
        public void SetNewOwner(TOwner owner)
        {
            if (!Equals(@this.Owner, default(TOwner)))
                throw new InvalidOperationException("Cannot set owner. Item is already owned.");
            @this.Owner = owner;
        }

        public void ResetOwner()
        {
            if (Equals(@this.Owner, default(TOwner)))
                throw new InvalidOperationException("Cannot reset owner. Item is not yet owned.");
            @this.Owner = default;
        }
    }

    extension<T>(T root) where T : class, IOwned
    {
        public IEnumerable<T> TraverseToRootOwner()
        {
            return root.TraverseList(i => i.Owner as T);
        }
    }

    extension<T>(T root) where T : class, IOwned<T>
    {
        public IEnumerable<T> TraverseToRootOwnerTyped()
        {
            return root.TraverseList(i => i.Owner);
        }
    }
}