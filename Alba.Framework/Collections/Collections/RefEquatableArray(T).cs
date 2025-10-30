using System.Collections;

namespace Alba.Framework.Collections;

[PublicAPI]
public readonly struct RefEquatableArray<T>(T[] array) : IEquatable<RefEquatableArray<T>>, IEnumerable<T>
{
    public static readonly RefEquatableArray<T> Empty = new([ ]);

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

    public bool Equals(RefEquatableArray<T> o) => this.SequenceEqual(o);
    public override bool Equals(object? o) => o is RefEquatableArray<T> v && Equals(v);
    public static bool operator ==(RefEquatableArray<T> a, RefEquatableArray<T> b) => a.Equals(b);
    public static bool operator !=(RefEquatableArray<T> a, RefEquatableArray<T> b) => !a.Equals(b);
}