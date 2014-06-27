using System;
using System.Collections.Generic;
using Alba.Framework.Collections;

namespace Alba.Framework.Common
{
    public static class OwnedExts
    {
        public static IEnumerable<T> TraverseToRootOwner<T> (this T root) where T : class, IOwned
        {
            return root.TraverseList(i => i.Owner as T);
        }

        public static IEnumerable<T> TraverseToRootOwnerTyped<T> (this T root) where T : class, IOwned<T>
        {
            return root.TraverseList(i => i.Owner);
        }

        public static void SetNewOwner<TOwner> (this IOwned<TOwner> @this, TOwner owner)
        {
            if (!Equals(@this.Owner, default(TOwner)))
                throw new InvalidOperationException("Cannot set owner. Item is already owned.");
            @this.Owner = owner;
        }

        public static void ResetOwner<TOwner> (this IOwned<TOwner> @this)
        {
            if (Equals(@this.Owner, default(TOwner)))
                throw new InvalidOperationException("Cannot reset owner. Item is not yet owned.");
            @this.Owner = default(TOwner);
        }
    }
}