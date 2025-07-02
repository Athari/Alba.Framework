using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using Windows.Win32.System.Com;
using Alba.Framework.Win32;

// ReSharper disable once CheckNamespace
namespace Windows.Win32;

internal partial class Native
{
    private static readonly StrategyBasedComWrappers ComWrappers = new();

    public static TI CoCreateInstanceUnique<T, TI>()
        where T : ITypeHasGuid
        where TI : ITypeHasGuid
    {
        var hr = CoCreateInstance(T.Guid, 0, (int)CLSCTX.CLSCTX_INPROC_SERVER, TI.Guid, out object ppv);
        Marshal.ThrowExceptionForHR(hr);
        return (TI)ppv;
    }

    public static T CoCreateInstance<T, TI>(CreateObjectFlags flags = CreateObjectFlags.UniqueInstance)
        where T : IComWrapper<TI>, ITypeHasGuid, new()
        where TI : ITypeHasGuid
    {
        var hr = CoCreateInstance(T.Guid, 0, (int)CLSCTX.CLSCTX_INPROC_SERVER, TI.Guid, out nint ppv);
        Marshal.ThrowExceptionForHR(hr);
        return new() {
            Ptr = ppv,
            Com = (TI)ComWrappers.GetOrCreateObjectForComInstance(ppv, flags),
        };
    }

    public static void Inc<T>(ref nint ptr)
    {
        ptr = (int)ptr + Marshal.SizeOf<T>();
    }

    public static void StructureToPtrInc<T>(T structure, ref nint ptr, bool fDeleteOld = false)
        where T : notnull
    {
        Marshal.StructureToPtr(structure, ptr, fDeleteOld);
        Inc<T>(ref ptr);
    }

    public static nint AllocHGlobalArray<T>(int size)
    {
        return Marshal.AllocHGlobal(Marshal.SizeOf<T>() * size);
    }

    public static void FreeHGlobalArray<T>(nint hglobal, int size)
    {
        if (hglobal == 0)
            return;
        nint ptr = hglobal;
        for (int i = 0; i < size; ++i) {
            Marshal.DestroyStructure<T>(ptr);
            Inc<T>(ref ptr);
        }
        Marshal.FreeHGlobal(hglobal);
    }

    [LibraryImport(Dll.Ole)]
    public static partial int CoCreateInstance(
        in Guid rclsid, nint pUnkOuter, int dwClsContext, in Guid riid,
        [MarshalUsing(typeof(UniqueComInterfaceMarshaller<object>))] out object ppv);

    [LibraryImport(Dll.Ole)]
    public static partial int CoCreateInstance(
        in Guid rclsid, nint pUnkOuter, int dwClsContext, in Guid riid,
        out nint ppv);
}