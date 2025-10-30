using System.Collections;

namespace Alba.Framework.Collections;

[PublicAPI]
public readonly struct EquatableArray<T>(T[] array) : IEquatable<EquatableArray<T>>, IEnumerable<T>
    where T : IEquatable<T>
{
    public static readonly EquatableArray<T> Empty = new([ ]);

    public readonly T[]? Array = array;

    public int Count => Array?.Length ?? 0;

    public T[] ArrayOrEmpty => Array ?? [ ];

    public ReadOnlySpan<T> AsSpan() => Array.AsSpan();

    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)(Array ?? [ ])).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override int GetHashCode()
    {
        if (Array == null)
            return 0;
        var hc = new HashCode();
        foreach (T item in Array)
            hc.Add(item);
        return hc.ToHashCode();
    }

    public bool Equals(EquatableArray<T> o) => AsSpan().SequenceEqual(o.AsSpan());
    public override bool Equals(object? o) => o is EquatableArray<T> v && Equals(v);
    public static bool operator ==(EquatableArray<T> a, EquatableArray<T> b) => a.Equals(b);
    public static bool operator !=(EquatableArray<T> a, EquatableArray<T> b) => !a.Equals(b);
}