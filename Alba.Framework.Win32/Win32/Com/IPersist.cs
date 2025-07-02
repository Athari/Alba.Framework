using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Alba.Framework.Win32;

[GeneratedComInterface(Options = ComInterfaceOptions.ComObjectWrapper)]
[Guid(GuidString)]
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal partial interface IPersist : ITypeHasGuid, IDisposable
{
    private const string GuidString = "0000010c-0000-0000-C000-000000000046";
    public new static readonly Guid Guid = new(GuidString);
    static Guid ITypeHasGuid.Guid => Guid;

    void GetClassID(out Guid pClassID);
}