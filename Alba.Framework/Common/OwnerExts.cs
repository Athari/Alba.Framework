using Alba.Framework.Collections;

namespace Alba.Framework.Common;

public static class OwnerExts
{
    public static void UpdateOwners(this IOwner @this, object? owner = null)
    {
        UpdateOwners((object)@this, owner);
    }

    public static void UpdateOwners(object @this, object? owner = null)
    {
        switch (@this) {
            case IOwned iowned:
                iowned.Owner = owner;
                break;
            case IOwner iowner:
                foreach (var owned in iowner.Owned)
                    UpdateOwners(owned, @this);
                break;
        }
    }

    extension<T>(T root) where T : class, IOwner<T>
    {
        public IEnumerable<T> TraverseToSingleLeaf() => root.TraverseList(i => i.Owned.SingleOrDefault());
    }

    extension<T>(T root) where T : IOwner<T>
    {
        public IEnumerable<T> TraverseOwnedTree() => root.TraverseTree(i => i.Owned);
        public IEnumerable<T> TraverseOwnedTreeDepth() => root.TraverseTreeDepth(i => i.Owned);
        public IEnumerable<T> TraverseOwnedTreeBreadth() => root.TraverseTreeBreadth(i => i.Owned);
        public IEnumerable<T> TraverseOwnedGraph() => root.TraverseGraph(i => i.Owned);
    }
}