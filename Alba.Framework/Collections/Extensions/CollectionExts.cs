using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Alba.Framework.Collections
{
    public static class CollectionExts
    {
        public static void AddRange<T> (this ICollection<T> @this, IEnumerable<T> items)
        {
            foreach (T item in items)
                @this.Add(item);
        }

        public static void RemoveRange<T> (this ICollection<T> @this, IEnumerable<T> items)
        {
            foreach (T item in items)
                @this.Remove(item);
        }

        public static void Replace<T> (this ICollection<T> @this, IEnumerable<T> items)
        {
            @this.Clear();
            @this.AddRange(items);
        }

        public static void RemoveStaleReferences<T> (this ICollection<WeakReference<T>> @this)
            where T : class
        {
            T target;
            var list = @this as List<WeakReference<T>>;
            if (list != null) {
                list.RemoveAll(wr => !wr.TryGetTarget(out target));
                list.Capacity = list.Count;
            }
            else {
                foreach (var wr in @this.ToArray()) {
                    if (!wr.TryGetTarget(out target))
                        @this.Remove(wr);
                }
            }
        }

        public static IReadOnlyList<T> ToTyped<T> (this ICollection @this)
        {
            return new TypedCollection<T>(@this);
        }
    }
}