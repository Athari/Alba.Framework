using System;

namespace Alba.Framework.Interop
{
    /// <summary>
    /// ASSOCF enumeration.
    /// http://msdn.microsoft.com/en-us/library/windows/desktop/bb762471(v=vs.85).aspx
    /// </summary>
    [Flags]
    public enum ASSOCF
    {
        /// <summary>None of the following options are set.</summary>
        NONE = 0x00000000,
        /// <summary>Instructs IQueryAssociations interface methods not to map CLSID values to ProgID values.</summary>
        INIT_NOREMAPCLSID = 0x00000001,
        /// <summary>Identifies the value of the pwszAssoc parameter of IQueryAssociations::Init as an executable file name. If this flag is not set, the root key will be set to the ProgID associated with the .exe key instead of the executable file's ProgID.</summary>
        INIT_BYEXENAME = 0x00000002,
        /// <summary>Identical to ASSOCF_INIT_BYEXENAME.</summary>
        OPEN_BYEXENAME = 0x00000002,
        /// <summary>Specifies that when an IQueryAssociations method does not find the requested value under the root key, it should attempt to retrieve the comparable value from the * subkey.</summary>
        INIT_DEFAULTTOSTAR = 0x00000004,
        /// <summary>Specifies that when a IQueryAssociations method does not find the requested value under the root key, it should attempt to retrieve the comparable value from the Folder subkey.</summary>
        INIT_DEFAULTTOFOLDER = 0x00000008,
        /// <summary>Specifies that only HKEY_CLASSES_ROOT should be searched, and that HKEY_CURRENT_USER should be ignored.</summary>
        NOUSERSETTINGS = 0x00000010,
        /// <summary>Specifies that the return string should not be truncated. Instead, return an error value and the required size for the complete string.</summary>
        NOTRUNCATE = 0x00000020,
        /// <summary>Instructs IQueryAssociations methods to verify that data is accurate. This setting allows IQueryAssociations methods to read data from the user's hard disk for verification. For example, they can check the friendly name in the registry against the one stored in the .exe file. Setting this flag typically reduces the efficiency of the method.</summary>
        VERIFY = 0x00000040,
        /// <summary>Instructs IQueryAssociations methods to ignore Rundll.exe and return information about its target. Typically IQueryAssociations methods return information about the first .exe or .dll in a command string. If a command uses Rundll.exe, setting this flag tells the method to ignore Rundll.exe and return information about its target.</summary>
        REMAPRUNDLL = 0x00000080,
        /// <summary>Instructs IQueryAssociations methods not to fix errors in the registry, such as the friendly name of a function not matching the one found in the .exe file.</summary>
        NOFIXUPS = 0x00000100,
        /// <summary>Specifies that the BaseClass value should be ignored.</summary>
        IGNOREBASECLASS = 0x00000200,
        /// <summary>Introduced in Windows 7. Specifies that the "Unknown" ProgID should be ignored; instead, fail.</summary>
        INIT_IGNOREUNKNOWN = 0x00000400,
        /// <summary>Introduced in Windows 8.</summary>
        INIT_FIXED_PROGID = 0x00000800,
        /// <summary>Introduced in Windows 8.</summary>
        IS_PROTOCOL = 0x00001000,
    }
}