using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Alba.Framework.Win32;

[GeneratedComInterface(Options = ComInterfaceOptions.ComObjectWrapper)]
[Guid(GuidString)]
internal partial interface IPersistFile : IPersist
{
    private const string GuidString = "0000010b-0000-0000-C000-000000000046";
    public new static readonly Guid Guid = new(GuidString);
    static Guid ITypeHasGuid.Guid => Guid;

    [MethodImpl(MethodImplOptions.PreserveSig)] int IsDirty();
    void Load([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, int dwMode);
    void Save([MarshalAs(UnmanagedType.LPWStr)] string? pszFileName, [MarshalAs(UnmanagedType.Bool)] bool fRemember);
    void SaveCompleted([MarshalAs(UnmanagedType.LPWStr)] string pszFileName);
    void GetCurFile([MarshalAs(UnmanagedType.LPWStr)] out string ppszFileName);
}