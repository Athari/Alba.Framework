using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Alba.Framework.Common;

namespace Alba.Framework.Collections
{
    public static class EnumerableExts
    {
        private static readonly Random _rnd = new Random();

        public static IEnumerable<T> Concat<T> (params IEnumerable<T>[] enumerables)
        {
            return enumerables.SelectMany(e => e);
        }

        public static IEnumerable<T> Concat<T> (this IEnumerable<T> @this, params T[] values)
        {
            foreach (T item in @this)
                yield return item;
            foreach (T value in values)
                yield return value;
        }

        public static IEnumerable<T> Concat<T> (this IEnumerable<T> @this, T value)
        {
            foreach (T item in @this)
                yield return item;
            yield return value;
        }

        public static IEnumerable<T> Flatten<T> (this IEnumerable<IEnumerable<T>> @this)
        {
            return @this.SelectMany(i => i);
        }

        public static IEnumerable<T> Flatten<TKey, T> (this IEnumerable<IDictionary<TKey, T>> @this)
        {
            return @this.SelectMany(i => i.Values);
        }

        public static IEnumerable<T> Flatten<TKey, T> (this IDictionary<TKey, IEnumerable<T>> @this)
        {
            return @this.Values.SelectMany(i => i);
        }

        public static IEnumerable<T> Flatten<TKey1, TKey2, T> (this IDictionary<TKey1, IDictionary<TKey2, T>> @this)
        {
            return @this.Values.SelectMany(i => i.Values);
        }

        public static int IndexOf<T> (this IEnumerable<T> @this, T value)
        {
            int i = 0;
            foreach (T item in @this) {
                if (Equals(item, value))
                    return i;
                i++;
            }
            return -1;
        }

        public static int IndexOf<T> (this IEnumerable<T> @this, Func<T, bool> predicate)
        {
            int i = 0;
            foreach (T item in @this) {
                if (predicate(item))
                    return i;
                i++;
            }
            return -1;
        }

        public static int LastIndexOf<T> (this IEnumerable<T> @this, T value)
        {
            int i = 0, index = -1;
            foreach (T item in @this) {
                if (Equals(item, value))
                    index = i;
                i++;
            }
            return index;
        }

        public static int LastIndexOf<T> (this IEnumerable<T> @this, Func<T, bool> predicate)
        {
            int i = 0, index = -1;
            foreach (T item in @this) {
                if (predicate(item))
                    index = i;
                i++;
            }
            return index;
        }

        public static IEnumerable<TResult> Intersect<T, TResult> (this IEnumerable<T> first, IEnumerable<T> second, Func<T, T, TResult> resultSelector, IEqualityComparer<T> comparer)
        {
            var dic = second.ToDictionary(i => i, comparer);
            foreach (T item in first) {
                T value;
                if (dic.TryGetValue(item, out value))
                    yield return resultSelector(item, value);
            }
        }

        public static IEnumerable<T> Shuffle<T> (this IEnumerable<T> @this)
        {
            return @this.OrderBy(i => _rnd.NextDouble());
        }

        public static IEnumerable<TResult> Zip<T1, T2, T3, TResult> (this IEnumerable<T1> source, IEnumerable<T2> second, IEnumerable<T3> third, Func<T1, T2, T3, TResult> selector)
        {
            using (var e1 = source.GetEnumerator())
            using (var e2 = second.GetEnumerator())
            using (var e3 = third.GetEnumerator())
                while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext())
                    yield return selector(e1.Current, e2.Current, e3.Current);
        }

        public static string JoinString<T> (this IEnumerable<T> @this, string separator)
        {
            return string.Join(separator, @this);
        }

        public static string JoinString (this IEnumerable<string> @this, string separator)
        {
            return string.Join(separator, @this);
        }

        public static string ConcatString<T> (this IEnumerable<T> @this)
        {
            return string.Concat(@this);
        }

        public static string ConcatString (this IEnumerable<string> @this)
        {
            return string.Concat(@this);
        }

        public static Dictionary<TKey, T> ToDictionary<TKey, T> (this IEnumerable<KeyValuePair<TKey, T>> @this)
        {
            return @this.ToDictionary(p => p.Key, p => p.Value);
        }

        public static Dictionary<TKey, T> ToDictionary<TKey, T> (this IEnumerable<DictionaryEntry> @this)
        {
            return @this.ToDictionary(p => (TKey)p.Key, p => (T)p.Value);
        }

        public static ReadOnlyDictionary<TKey, T> ToReadOnlyDictionary<TKey, T> (this IEnumerable<T> @this,
            Func<T, TKey> keySelector)
        {
            return new ReadOnlyDictionary<TKey, T>(@this.ToDictionary(keySelector));
        }

        public static ReadOnlyDictionary<TKey, T> ToReadOnlyDictionary<TKey, T> (this IEnumerable<T> @this,
            Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return new ReadOnlyDictionary<TKey, T>(@this.ToDictionary(keySelector, comparer));
        }

        public static ReadOnlyDictionary<TKey, T> ToReadOnlyDictionary<TKey, T> (this IEnumerable<T> @this,
            Func<T, TKey> keySelector, Func<T, T> elementSelector)
        {
            return new ReadOnlyDictionary<TKey, T>(@this.ToDictionary(keySelector, elementSelector));
        }

        public static ReadOnlyDictionary<TKey, T> ToReadOnlyDictionary<TKey, T> (this IEnumerable<T> @this,
            Func<T, TKey> keySelector, Func<T, T> elementSelector, IEqualityComparer<TKey> comparer)
        {
            return new ReadOnlyDictionary<TKey, T>(@this.ToDictionary(keySelector, elementSelector, comparer));
        }

        public static ReadOnlyCollection<T> ToReadOnlyList<T> (this IEnumerable<T> @this)
        {
            return @this.ToList().AsReadOnly();
        }

        public static IEnumerable<T> TraverseList<T> (this T root, Func<T, T> getNext) where T : class
        {
            for (T current = root; current != null; current = getNext(current))
                yield return current;
        }

        public static IEnumerable<T> TraverseList<T> (this T root) where T : class, IOwner<T>
        {
            return root.TraverseList(i => i.Owned.SingleOrDefault());
        }

        public static IEnumerable<T> TraverseTree<T> (this T root, Func<T, IEnumerable<T>> getChildren)
        {
            return TraverseTreeDepth(root, getChildren);
        }

        public static IEnumerable<T> TraverseTree<T> (this T root) where T : IOwner<T>
        {
            return TraverseTree(root, i => i.Owned);
        }

        public static IEnumerable<T> TraverseTreeDepth<T> (this T root, Func<T, IEnumerable<T>> getChildren)
        {
            var stack = new Stack<T>();
            stack.Push(root);

            while (stack.Count != 0) {
                T item = stack.Pop();
                yield return item;
                foreach (var child in getChildren(item).Reverse())
                    stack.Push(child);
            }
        }

        public static IEnumerable<T> TraverseTreeDepth<T> (this T root) where T : IOwner<T>
        {
            return root.TraverseTreeDepth(i => i.Owned);
        }

        public static IEnumerable<T> TraverseTreeBreadth<T> (this T root, Func<T, IEnumerable<T>> getChildren)
        {
            var queue = new Queue<T>();
            queue.Enqueue(root);

            while (queue.Count != 0) {
                T item = queue.Dequeue();
                yield return item;
                queue.EnqueueRange(getChildren(item));
            }
        }

        public static IEnumerable<T> TraverseTreeBreadth<T> (this T root) where T : IOwner<T>
        {
            return root.TraverseTreeBreadth(i => i.Owned);
        }

        public static IEnumerable<T> TraverseGraph<T> (this T root, Func<T, IEnumerable<T>> getChildren)
        {
            var seen = new HashSet<T>();
            var stack = new Stack<T>();
            stack.Push(root);

            while (stack.Count != 0) {
                T item = stack.Pop();
                if (seen.Contains(item))
                    continue;
                seen.Add(item);
                yield return item;
                stack.PushRange(getChildren(item));
            }
        }

        public static IEnumerable<T> TraverseGraph<T> (this T root) where T : IOwner<T>
        {
            return root.TraverseGraph(i => i.Owned);
        }

        public static IEnumerable<int> Range (this int count)
        {
            return Enumerable.Range(0, count);
        }

        public static IEnumerable<int> Range (this int start, int count)
        {
            return Enumerable.Range(start, count);
        }
    }
}