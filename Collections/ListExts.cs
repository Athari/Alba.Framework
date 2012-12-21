using System.Collections;
using System.Collections.Generic;

namespace Alba.Framework.Collections
{
    public static class ListExts
    {
        public static void InsertRange<T> (this IList<T> @this, int index, IEnumerable<T> items)
        {
            foreach (T item in items)
                @this.Insert(index++, item);
        }

        public static void AddRange (this IList @this, IEnumerable items)
        {
            foreach (object item in items)
                @this.Add(item);
        }

        public static void RemoveRange (this IList @this, IEnumerable items)
        {
            foreach (object item in items)
                @this.Remove(item);
        }

        public static void Replace (this IList @this, IEnumerable items)
        {
            @this.Clear();
            @this.AddRange(items);
        }
    }
}