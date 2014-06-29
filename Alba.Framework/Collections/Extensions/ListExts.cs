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

        public static T AtWrapped<T> (this IList<T> @this, int index)
        {
            return @this[WrapIndex(index, @this.Count)];
        }

        public static void SetAtWrapped<T> (this IList<T> @this, int index, T value)
        {
            @this[WrapIndex(index, @this.Count)] = value;
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

        private static int WrapIndex (int index, int count)
        {
            if (count == 0)
                throw new IndexOutOfRangeException();
            else if (index >= count)
                index = index % count;
            else if (index < 0) {
                index = index % count;
                if (index != 0)
                    index += count;
            }
            return index;
        }
    }
}