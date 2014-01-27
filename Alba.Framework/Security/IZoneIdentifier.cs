using System.Runtime.InteropServices;

namespace Alba.Framework.Security
{
    [ComImport, Guid ("CD45F185-1B21-48E2-967B-EAD743A8914E"), InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IZoneIdentifier
    {
        UrlZone GetId ();
        void SetId (UrlZone zone);
        void Remove ();
    }
}