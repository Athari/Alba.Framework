namespace Alba.Framework.Security;

/// <summary>URLZONE enumeration. Contains all the predefined zones used by Windows Internet Explorer.</summary>
/// <remarks>Header: UrlMon.h</remarks>
public enum UrlZone
{
    /// <summary>URLZONE_INVALID: Internet Explorer 7. Invalid zone. Used only if no appropriate zone is available.</summary>
    Invalid = -1,
    /// <summary>URLZONE_LOCAL_MACHINE: Zone used for content already on the user's local computer. This zone is not exposed by the user interface.</summary>
    LocalMachine = 0,
    /// <summary>URLZONE_INTRANET: Zone used for content found on an intranet.</summary>
    Intranet,
    /// <summary>URLZONE_TRUSTED: Zone used for trusted Websites on the Internet.</summary>
    Trusted,
    /// <summary>URLZONE_INTERNET: Zone used for most of the content on the Internet.</summary>
    Internet,
    /// <summary>URLZONE_UNTRUSTED: Zone used for Websites that are not trusted.</summary>
    Untrusted,
    /// <summary>URLZONE_PREDEFINED_MIN: Minimum value for a predefined zone.</summary>
    PredefinedMin = LocalMachine,
    /// <summary>URLZONE_PREDEFINED_MAX: Maximum value for a predefined zone.</summary>
    PredefinedMax = 999,
    /// <summary>URLZONE_USER_MIN: Minimum value allowed for a user-defined zone.</summary>
    UserMin = 1000,
    /// <summary>URLZONE_USER_MAX: Maximum value allowed for a user-defined zone.</summary>
    UserMax = 10000,
    /// <summary>URLZONE_ESC_FLAG: Enhanced Security Configuration zone mapping flag for IInternetSecurityManager::SetZoneMapping.</summary>
    EscFlag = 256,
}