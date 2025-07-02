using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using Alba.Framework.Security;

namespace Alba.Framework.Win32;

[GeneratedComInterface(Options = ComInterfaceOptions.ComObjectWrapper)]
[Guid(GuidString)]
internal partial interface IZoneIdentifier : ITypeHasGuid
{
    private const string GuidString = "CD45F185-1B21-48E2-967B-EAD743A8914E";
    public new static readonly Guid Guid = new(GuidString);
    static Guid ITypeHasGuid.Guid => Guid;

    UrlZone GetId();
    void SetId(UrlZone zone);
    void Remove();
}