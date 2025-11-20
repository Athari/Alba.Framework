using System.Runtime.Versioning;
using System.Security.Principal;

namespace Alba.Framework.Security;

[SupportedOSPlatform("windows")]
public static class WellKnownSidTypeExts
{
    public static SecurityIdentifier ToIdentifier(this WellKnownSidType @this, SecurityIdentifier? security = null) =>
        new(@this, security);
}