using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Alba.Framework.Win32;

internal class SafeComHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    public SafeComHandle() : base(true) { }

    public SafeComHandle(nint handle, bool ownsHandle = true) : base(ownsHandle) => SetHandle(handle);

    protected override bool ReleaseHandle()
    {
        Marshal.Release(DangerousGetHandle());
        return true;
    }
}