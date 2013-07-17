using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using Alba.Framework.Reflection;
using Alba.Framework.Sys;
using Alba.Framework.Text;

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

        public static int IndexOf<T> (this IEnumerable<T> @this, T value, IEqualityComparer<T> comparer)
        {
            if (comparer == null)
                comparer = EqualityComparer<T>.Default;
            int i = 0;
            foreach (T item in @this) {
                if (comparer.Equals(item, value))
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

        public static int LastIndexOf<T> (this IEnumerable<T> @this, T value, IEqualityComparer<T> comparer)
        {
            if (comparer == null)
                comparer = EqualityComparer<T>.Default;
            int i = 0, index = -1;
            foreach (T item in @this) {
                if (comparer.Equals(item, value))
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

        public static T FirstOrException<T> (this IEnumerable<T> @this, Func<T, bool> predicate, Type exceptionType = null)
        {
            Contract.Requires<ArgumentNullException>(@this != null, "this");
            Contract.Requires<ArgumentNullException>(predicate != null, "predicate");
            Contract.Requires<ArgumentException>(exceptionType == null || exceptionType.Is<Exception>(), "exceptionType");
            foreach (T item in @this)
                if (predicate(item))
                    return item;
            throw ExceptionExts.Create(exceptionType ?? typeof(InvalidOperationException),
                "Sequence of '{0}' contains no matching element.".Fmt(typeof(T).GetMediumName()));
        }

        public static T FirstOrException<T> (this IEnumerable<T> @this, Func<T, bool> predicate, Func<Exception> createException)
        {
            Contract.Requires<ArgumentNullException>(@this != null, "this");
            Contract.Requires<ArgumentNullException>(predicate != null, "predicate");
            Contract.Requires<ArgumentNullException>(createException != null, "createException");
            foreach (T item in @this)
                if (predicate(item))
                    return item;
            throw createException();
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

        public static IEnumerable<T> Inverse<T> (this IEnumerable<T> @this)
        {
            var list = @this as IList<T>;
            if (list != null) {
                for (int i = list.Count - 1; i >= 0; i--)
                    yield return list[i];
            }
            else {
                foreach (T item in @this.Reverse())
                    yield return item;
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

        public static IEnumerable<T> TraverseTree<T> (this T root, Func<T, IEnumerable<T>> getChildren)
        {
            return TraverseTreeDepth(root, getChildren);
        }

        public static IEnumerable<T> TraverseTreeDepth<T> (this T root, Func<T, IEnumerable<T>> getChildren)
        {
            var stack = new Stack<T>();
            stack.Push(root);

            while (stack.Count != 0) {
                T item = stack.Pop();
                yield return item;
                foreach (var child in getChildren(item).Inverse())
                    stack.Push(child);
            }
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

        public static IEnumerable<T> ReturnEnumerable<T> (this T item)
        {
            yield return item;
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