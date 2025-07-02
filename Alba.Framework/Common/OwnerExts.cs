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

    public static IEnumerable<T> TraverseToSingleLeaf<T>(this T root) where T : class, IOwner<T>
    {
        return root.TraverseList(i => i.Owned.SingleOrDefault());
    }

    public static IEnumerable<T> TraverseTree<T>(this T root) where T : IOwner<T>
    {
        return root.TraverseTree(i => i.Owned);
    }

    public static IEnumerable<T> TraverseTreeDepth<T>(this T root) where T : IOwner<T>
    {
        return root.TraverseTreeDepth(i => i.Owned);
    }

    public static IEnumerable<T> TraverseTreeBreadth<T>(this T root) where T : IOwner<T>
    {
        return root.TraverseTreeBreadth(i => i.Owned);
    }

    public static IEnumerable<T> TraverseGraph<T>(this T root) where T : IOwner<T>
    {
        return root.TraverseGraph(i => i.Owned);
    }
}