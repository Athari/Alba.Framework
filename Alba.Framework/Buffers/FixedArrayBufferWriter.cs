using System.Buffers;
using Alba.Framework.Common;

namespace Alba.Framework.Buffers;

public sealed class FixedArrayBufferWriter<T>(int size) : IBufferWriter<T>
{
    private readonly T[] _buffer = new T[Ensure.GreaterThanOrEqualTo(size, 0)];

    public int Index { get; private set; }

    public ReadOnlyMemory<T> WrittenMemory => _buffer.AsMemory(0, Index);
    public ReadOnlySpan<T> WrittenSpan => _buffer.AsSpan(0, Index);
    public int Size => _buffer.Length;
    public int Free => _buffer.Length - Index;

    public void Clear()
    {
        _buffer.AsSpan(0, Index).Clear();
        ResetIndex();
    }

    public void ResetIndex()
    {
        Index = 0;
    }

    public void Advance(int count)
    {
        Ensure.GreaterThanOrEqualTo(count, 0);
        if (Index > Size - count)
            throw new InvalidOperationException($"Buffer advanced past its fixed size of {Size}.");
        Index += count;
    }

    public Memory<T> GetMemory(int sizeHint = 0)
    {
        Ensure.LessThanOrEqualTo(sizeHint, Size - Index);
        return _buffer.AsMemory(Index);
    }

    public Span<T> GetSpan(int sizeHint = 0)
    {
        Ensure.LessThanOrEqualTo(sizeHint, Size - Index);
        return _buffer.AsSpan(Index);
    }
}