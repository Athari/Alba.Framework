using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Alba.Framework;

public static class IntPtrExts
{
    extension(nint @this)
    {
        public Span<T> ToSpan<T>(int length) =>
            MemoryMarshal.CreateSpan(ref @this.ToRef<T>(), length);

        public Span<T> ToSpanAtOffset<T>(int offset, int length) =>
            MemoryMarshal.CreateSpan(ref @this.ToRefAtOffset<T>(offset), length);

        public ReadOnlySpan<T> ToReadOnlySpan<T>(int length) =>
            MemoryMarshal.CreateReadOnlySpan(ref @this.ToRef<T>(), length);

        public ReadOnlySpan<T> ToReadOnlySpanAtOffset<T>(int offset, int length) =>
            MemoryMarshal.CreateReadOnlySpan(ref @this.ToRefAtOffset<T>(offset), length);

        public ref T ToRef<T>() =>
            ref Unsafe.AddByteOffset(ref Unsafe.NullRef<T>(), @this);

        public ref T ToRefAtOffset<T>(int offset) =>
            ref Unsafe.Add(ref @this.ToRef<T>(), offset);

        public ref T ToRefAtByteOffset<T>(nint offset) =>
            ref Unsafe.AddByteOffset(ref Unsafe.NullRef<T>(), @this + offset);
    }
}