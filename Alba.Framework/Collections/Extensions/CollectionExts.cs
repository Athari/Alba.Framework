using System.Collections;

namespace Alba.Framework.Collections;

[PublicAPI]
public static class CollectionExts
{
    extension<T>(ICollection<T> @this)
    {
        public void AddRange([InstantHandle] IEnumerable<T> items)
        {
            foreach (var item in items)
                @this.Add(item);
        }

        public void RemoveRange([InstantHandle] IEnumerable<T> items)
        {
            foreach (var item in items)
                @this.Remove(item);
        }

        public void ReplaceAll([InstantHandle] IEnumerable<T> items)
        {
            @this.Clear();
            @this.AddRange(items);
        }
    }

    extension(ICollection @this)
    {
        public TypedCollection<T> ToTyped<T>() => new(@this);
    }

    public static void RemoveStaleReferences<T>(this ICollection<WeakReference<T>> @this)
        where T : class
    {
        if (@this is List<WeakReference<T>> list) {
            list.RemoveAll(wr => !wr.TryGetTarget(out _));
            if (list.Capacity > list.Count * 2)
                list.Capacity = list.Count;
        }
        else {
            foreach (var wr in @this.ToArray())
                if (!wr.TryGetTarget(out T? _))
                    @this.Remove(wr);
        }
    }
}