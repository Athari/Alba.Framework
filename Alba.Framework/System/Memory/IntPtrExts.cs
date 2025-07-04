using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Alba.Framework;

public static class IntPtrExts
{
    public static Span<T> ToSpan<T>(this nint @this, int length) =>
        MemoryMarshal.CreateSpan(ref @this.ToRef<T>(), length);

    public static Span<T> ToSpanAtOffset<T>(this nint @this, int offset, int length) =>
        MemoryMarshal.CreateSpan(ref @this.ToRefAtOffset<T>(offset), length);

    public static ReadOnlySpan<T> ToReadOnlySpan<T>(this nint @this, int length) =>
        MemoryMarshal.CreateReadOnlySpan(ref @this.ToRef<T>(), length);

    public static ReadOnlySpan<T> ToReadOnlySpanAtOffset<T>(this nint @this, int offset, int length) =>
        MemoryMarshal.CreateReadOnlySpan(ref @this.ToRefAtOffset<T>(offset), length);

    public static ref T ToRef<T>(this nint @this) =>
        ref Unsafe.AddByteOffset(ref Unsafe.NullRef<T>(), @this);

    public static ref T ToRefAtOffset<T>(this nint @this, int offset) =>
        ref Unsafe.Add(ref @this.ToRef<T>(), offset);

    public static ref T ToRefAtByteOffset<T>(this nint @this, nint offset) =>
        ref Unsafe.AddByteOffset(ref Unsafe.NullRef<T>(), @this + offset);
}