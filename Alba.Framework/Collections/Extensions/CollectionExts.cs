using System.Collections;

namespace Alba.Framework.Collections;

[PublicAPI]
public static class CollectionExts
{
    public static void AddRange<T>(this ICollection<T> @this, IEnumerable<T> items)
    {
        foreach (T item in items)
            @this.Add(item);
    }

    public static void RemoveRange<T>(this ICollection<T> @this, IEnumerable<T> items)
    {
        foreach (T item in items)
            @this.Remove(item);
    }

    public static void ReplaceAll<T>(this ICollection<T> @this, IEnumerable<T> items)
    {
        @this.Clear();
        @this.AddRange(items);
    }

    public static void RemoveStaleReferences<T>(this ICollection<WeakReference<T>> @this)
        where T : class
    {
        if (@this is List<WeakReference<T>> list) {
            list.RemoveAll(wr => !wr.TryGetTarget(out _));
            list.Capacity = list.Count;
        }
        else {
            foreach (var wr in @this.ToArray())
                if (!wr.TryGetTarget(out T? _))
                    @this.Remove(wr);
        }
    }

    public static IReadOnlyList<T> ToTyped<T>(this ICollection @this)
    {
        return new TypedCollection<T>(@this);
    }
}