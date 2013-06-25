using System.Collections.Generic;
using System.Linq;
using Alba.Framework.Collections;

namespace Alba.Framework.Common
{
    public static class OwnerExts
    {
        public static void UpdateOwners (this IOwner @this, object owner = null)
        {
            UpdateOwners((object)@this, owner);
        }

        public static void UpdateOwners (object @this, object owner = null)
        {
            var iowned = @this as IOwned;
            if (iowned != null)
                iowned.Owner = owner;
            var iowner = @this as IOwner;
            if (iowner != null)
                iowner.Owned.ForEach(owned => UpdateOwners(owned, @this));
        }

        public static IEnumerable<T> TraverseToSingleLeaf<T> (this T root) where T : class, IOwner<T>
        {
            return root.TraverseList(i => i.Owned.SingleOrDefault());
        }

        public static IEnumerable<T> TraverseTree<T> (this T root) where T : IOwner<T>
        {
            return root.TraverseTree(i => i.Owned);
        }

        public static IEnumerable<T> TraverseTreeDepth<T> (this T root) where T : IOwner<T>
        {
            return root.TraverseTreeDepth(i => i.Owned);
        }

        public static IEnumerable<T> TraverseTreeBreadth<T> (this T root) where T : IOwner<T>
        {
            return root.TraverseTreeBreadth(i => i.Owned);
        }

        public static IEnumerable<T> TraverseGraph<T> (this T root) where T : IOwner<T>
        {
            return root.TraverseGraph(i => i.Owned);
        }
    }
}