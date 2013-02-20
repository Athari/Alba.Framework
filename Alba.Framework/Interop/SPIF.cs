using System;

namespace Alba.Framework.Interop
{
    [Flags]
    public enum SPIF
    {
        NONE = 0x00,
        /// <summary>Writes the new system-wide parameter setting to the user profile.</summary>
        UPDATEINIFILE = 0x01,
        /// <summary>Broadcasts the WM_SETTINGCHANGE message after updating the user profile.</summary>
        SENDCHANGE = 0x02,
        /// <summary>Same as SPIF_SENDCHANGE.</summary>
        SENDWININICHANGE = 0x02
    }
}