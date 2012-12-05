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
    }
}