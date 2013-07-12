using System;
using System.Collections;
using System.Collections.Generic;

namespace Alba.Framework.Collections
{
    public static class ListExts
    {
        private static readonly Random _rnd = new Random();

        public static T RandomItem<T> (this IList<T> @this)
        {
            return @this[_rnd.Next(@this.Count)];
        }

        public static void InsertRange<T> (this IList<T> @this, int index, IEnumerable<T> items)
        {
            foreach (T item in items)
                @this.Insert(index++, item);
        }

        public static void AddRangeUntyped (this IList @this, IEnumerable items)
        {
            foreach (object item in items)
                @this.Add(item);
        }

        public static void RemoveRangeUntyped (this IList @this, IEnumerable items)
        {
            foreach (object item in items)
                @this.Remove(item);
        }

        public static void ReplaceUntyped (this IList @this, IEnumerable items)
        {
            @this.Clear();
            @this.AddRangeUntyped(items);
        }
    }
}