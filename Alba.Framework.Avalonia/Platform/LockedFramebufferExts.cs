using System.Runtime.InteropServices;
using Avalonia.Platform;

namespace Alba.Framework.Avalonia.Platform;

public static class LockedFramebufferExts
{
    extension(ILockedFramebuffer @this)
    {
        public int GetStride<T>() =>
            @this.RowBytes / Marshal.SizeOf<T>();

        public Span<T> GetRow<T>(int y) where T : unmanaged
        {
            var stride = @this.GetStride<T>();
            return @this.Address.ToSpanAtOffset<T>(y * stride, stride);
        }
    }
}