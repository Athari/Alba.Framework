using System.Linq;

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
    }
}