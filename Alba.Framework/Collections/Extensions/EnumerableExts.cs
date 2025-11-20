using System.Collections;
using System.Collections.ObjectModel;

namespace Alba.Framework.Collections;

[PublicAPI, SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("Style", "IDE0305: Use collection expression for fluent", Justification = "LINQ chains")]
public static partial class EnumerableExts
{
    private const string ErrorNoElements = "Sequence contains no elements.";

    private static readonly Random Rnd = new();

    public static ReadOnlyCollection<T> EmptyList<T>() => EmptyEnumerable<T>.EmptyList;

    public static ReadOnlyDictionary<TKey, TValue> EmptyDictionary<TKey, TValue>()
        where TKey : notnull =>
        ReadOnlyDictionary<TKey, TValue>.Empty;

    extension<T>(IEnumerable<T> @this)
    {
        public T? AtOrDefault(int index) =>
            @this.ElementAtOrDefault(index);

        public T? AtOrDefault(Index index) =>
            @this.ElementAtOrDefault(index);

        //[Obsolete("Broken priority")]
        //public IEnumerable<T> Concat(params IEnumerable<T> values)
        //{
        //    foreach (var item in @this)
        //        yield return item;
        //    foreach (var value in values)
        //        yield return value;
        //}

        //[Obsolete("Broken priority")]
        //public IEnumerable<T> Concat(params IEnumerable<IEnumerable<T>> enumerables)
        //{
        //    foreach (var item in @this)
        //        yield return item;
        //    foreach (T value in enumerables.Flatten())
        //        yield return value;
        //}

        public IEnumerable<T> Concat(T value)
        {
            foreach (T item in @this)
                yield return item;
            yield return value;
        }

        public int IndexOf(T value, IEqualityComparer<T>? comparer)
        {
            comparer ??= EqualityComparer<T>.Default;
            int i = 0;
            foreach (T item in @this) {
                if (comparer.Equals(item, value))
                    return i;
                i++;
            }
            return -1;
        }

        public int IndexOf(Func<T, bool> predicate)
        {
            int i = 0;
            foreach (T item in @this) {
                if (predicate(item))
                    return i;
                i++;
            }
            return -1;
        }

        public int LastIndexOf(T value, IEqualityComparer<T>? comparer)
        {
            comparer ??= EqualityComparer<T>.Default;
            int i = 0, index = -1;
            foreach (T item in @this) {
                if (comparer.Equals(item, value))
                    index = i;
                i++;
            }
            return index;
        }

        public int LastIndexOf(Func<T, bool> predicate)
        {
            int i = 0, index = -1;
            foreach (T item in @this) {
                if (predicate(item))
                    index = i;
                i++;
            }
            return index;
        }

        public IEnumerable<T> OrderByIndices(IEnumerable<int> indices)
        {
            IList<T> list = @this.AsList();
            return indices.Select(index => list[index]);
        }

        public IEnumerable<T> Inverse()
        {
            if (@this is IList<T> list) {
                for (int i = list.Count - 1; i >= 0; i--)
                    yield return list[i];
            }
            else {
                foreach (T item in @this.Reverse())
                    yield return item;
            }
        }

        public IEnumerable<IEnumerable<T>> Batch(int size)
        {
            T[]? bucket = null;
            int count = 0;
            foreach (T item in @this) {
                bucket ??= new T[size];
                bucket[count++] = item;
                if (count != size)
                    continue;
                yield return bucket;
                bucket = null;
                count = 0;
            }
            if (bucket != null && count > 0)
                yield return bucket.Take(count);
        }

        public IEnumerable<T> Shuffle(Random? rnd = null)
        {
            return @this.OrderBy(_ => (rnd ?? Rnd).NextDouble());
        }

        public IEnumerable<T> WhereNotDefault()
        {
            var def = default(T);
            return @this.Where(i => !i.EqualsValue(def));
        }

        public IEnumerable<(T, T2)> SelectWith<T2>(Func<T, T2> secondSelector)
        {
            using var e = @this.GetEnumerator();
            while (e.MoveNext())
                yield return (e.Current, secondSelector(e.Current));
        }

        public IEnumerable<(T, T2, T3)> SelectWith<T2, T3>(Func<T, T2> secondSelector, Func<T, T3> thirdSelector)
        {
            using var e = @this.GetEnumerator();
            while (e.MoveNext())
                yield return (e.Current, secondSelector(e.Current), thirdSelector(e.Current));
        }

        public IEnumerable<TResult> SelectWith<T2, TResult>(Func<T, T2> secondSelector, Func<T, T2, TResult> resultSelector)
        {
            using var e = @this.GetEnumerator();
            while (e.MoveNext())
                yield return resultSelector(e.Current, secondSelector(e.Current));
        }

        public IEnumerable<TResult> SelectWith<T2, T3, TResult>(Func<T, T2> secondSelector, Func<T, T3> thirdSelector, Func<T, T2, T3, TResult> resultSelector)
        {
            using var e = @this.GetEnumerator();
            while (e.MoveNext())
                yield return resultSelector(e.Current, secondSelector(e.Current), thirdSelector(e.Current));
        }

        public IEnumerable<(T, T2, T3)> Zip<T2, T3>(IEnumerable<T2> second, IEnumerable<T3> third)
        {
            using var e1 = @this.GetEnumerator();
            using var e2 = second.GetEnumerator();
            using var e3 = third.GetEnumerator();
            while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext())
                yield return (e1.Current, e2.Current, e3.Current);
        }

        public IEnumerable<TResult> Zip<T2, T3, TResult>(IEnumerable<T2> second, IEnumerable<T3> third, Func<T, T2, T3, TResult> selector)
        {
            using var e1 = @this.GetEnumerator();
            using var e2 = second.GetEnumerator();
            using var e3 = third.GetEnumerator();
            while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext())
                yield return selector(e1.Current, e2.Current, e3.Current);
        }

        public bool AllEqual()
        {
            bool isFirst = true;
            var first = default(T)!;
            foreach (T item in @this) {
                if (isFirst) {
                    first = item;
                    isFirst = false;
                }
                else if (!first.EqualsValue(item))
                    return false;
            }
            return true;
        }

        public bool AllEqual(out T value)
        {
            bool isFirst = true;
            value = default!;
            foreach (T item in @this) {
                if (isFirst) {
                    value = item;
                    isFirst = false;
                }
                else if (!value.EqualsValue(item))
                    return false;
            }
            if (isFirst)
                throw new InvalidOperationException("Sequence is empty.");
            return true;
        }
    }

    extension<T>(IEnumerable<T?> @this)
    {
        public IEnumerable<T> WhereNotNull()
        {
            return @this.Where(i => i != null)!;
        }
    }

    extension(IEnumerable @this)
    {
        public int UntypedCount()
        {
            switch (@this) {
                case ICollection c:
                    return c.Count;
                case IReadOnlyCollection<object> roc:
                    return roc.Count;
                default: {
                    using var _ = @this.GetEnumerator().As(out var e) as IDisposable;
                    int count = 0;
                    while (e.MoveNext())
                        count++;
                    return count;
                }
            }
        }
    }

    public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> @this)
    {
        return @this.SelectMany(i => i);
    }

    public static IEnumerable<T> Flatten<TKey, T>(this IEnumerable<IDictionary<TKey, T>> @this)
    {
        return @this.SelectMany(i => i.Values);
    }

    public static IEnumerable<T> Flatten<TKey, T>(this IDictionary<TKey, IEnumerable<T>> @this)
    {
        return @this.Values.SelectMany(i => i);
    }

    public static IEnumerable<T> Flatten<TKey1, TKey2, T>(this IDictionary<TKey1, IDictionary<TKey2, T>> @this)
    {
        return @this.Values.SelectMany(i => i.Values);
    }

    extension<T>(IEnumerable<T> @this) where T : notnull
    {
        public IEnumerable<TResult> Intersect<TResult>(IEnumerable<T> second, Func<T, T, TResult> resultSelector, IEqualityComparer<T> comparer)
        {
            var dic = second.ToDictionary(i => i, comparer);
            foreach (T item in @this)
                if (dic.TryGetValue(item, out var value))
                    yield return resultSelector(item, value);
        }
    }

    private static class EmptyEnumerable<T>
    {
        [field: MaybeNull]
        public static ReadOnlyCollection<T> EmptyList => field ??= new(Array.Empty<T>());
    }
}