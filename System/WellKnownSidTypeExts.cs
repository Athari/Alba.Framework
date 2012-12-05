using System.Security.Principal;

namespace Alba.Framework.System
{
    public static class WellKnownSidTypeExts
    {
        public static SecurityIdentifier ToIdentifier (this WellKnownSidType @this)
        {
            return new SecurityIdentifier(@this, null);
        }
    }
}