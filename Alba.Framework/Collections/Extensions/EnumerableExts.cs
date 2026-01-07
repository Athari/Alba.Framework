using System.Collections;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Alba.Framework.Collections;

[SuppressMessage("Style", "IDE0305: Use collection expression for fluent", Justification = "LINQ chains")]
public static partial class EnumerableExts
{
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

        public IEnumerable<T> Do(Action<T> action)
        {
            foreach (var item in @this) {
                action(item);
                yield return item;
            }
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

    extension<T>(IEnumerable<T> @this) where T : struct
    {
        public T? FirstOrNull() =>
            @this.TryGetFirst(out _);

        public T? FirstOrNull(Func<T, bool> predicate) =>
            @this.TryGetFirst(predicate, out _);

        public T? LastOrNull() =>
            @this.TryGetLast(out _);

        public T? LastOrNull(Func<T, bool> predicate) =>
            @this.TryGetLast(predicate, out _);

        public T? AtOrNull(int index) =>
            @this.TryGetElementAt(index, out _);

        public T? AtOrNull(Index index)
        {
            if (!index.IsFromEnd)
                return @this.AtOrNull(index.Value);
            if (@this.TryGetNonEnumeratedCount(out int count))
                return @this.AtOrNull(count - index.Value);
            TryGetElementFromEnd(@this, index.Value, out var item);
            return item;
        }

        private T? TryGetFirst(out bool found)
        {
            if (@this is IList<T> list) {
                if (list.Count > 0) {
                    found = true;
                    return list[0];
                }
            }
            else {
                using var it = @this.GetEnumerator();
                if (it.MoveNext()) {
                    found = true;
                    return it.Current;
                }
            }
            found = false;
            return null;
        }

        private T? TryGetFirst(Func<T, bool> predicate, out bool found)
        {
            if (@this.TryGetSpan(out ReadOnlySpan<T> span)) {
                foreach (var item in span) {
                    if (predicate(item)) {
                        found = true;
                        return item;
                    }
                }
            }
            else {
                foreach (var item in @this) {
                    if (predicate(item)) {
                        found = true;
                        return item;
                    }
                }
            }
            found = false;
            return null;
        }

        private T? TryGetLast(out bool found)
        {
            if (@this is IList<T> list) {
                var count = list.Count;
                if (count > 0) {
                    found = true;
                    return list[count - 1];
                }
            }
            else {
                using var it = @this.GetEnumerator();
                if (it.MoveNext()) {
                    T result;
                    do {
                        result = it.Current;
                    } while (it.MoveNext());
                    found = true;
                    return result;
                }
            }
            found = false;
            return null;
        }

        private T? TryGetLast(Func<T, bool> predicate, out bool found)
        {
            if (@this is IList<T> list) {
                for (var i = list.Count - 1; i >= 0; --i) {
                    var result = list[i];
                    if (predicate(result)) {
                        found = true;
                        return result;
                    }
                }
            }
            else {
                using var it = @this.GetEnumerator();
                while (it.MoveNext()) {
                    var result = it.Current;
                    if (predicate(result)) {
                        while (it.MoveNext()) {
                            var item = it.Current;
                            if (predicate(item))
                                result = item;
                        }
                        found = true;
                        return result;
                    }
                }
            }

            found = false;
            return null;
        }

        private T? TryGetElementAt(int index, out bool found)
        {
            if (@this is IList<T> list) {
                found = (uint)index < (uint)list.Count;
                return found ? list[index] : null;
            }
            if (index >= 0) {
                using var it = @this.GetEnumerator();
                while (it.MoveNext()) {
                    if (index == 0) {
                        found = true;
                        return it.Current;
                    }
                    index--;
                }
            }
            found = false;
            return null;
        }

        private bool TryGetElementFromEnd(int indexFromEnd, [MaybeNullWhen(false)] out T? item)
        {
            if (indexFromEnd > 0) {
                using IEnumerator<T> e = @this.GetEnumerator();
                if (e.MoveNext()) {
                    Queue<T> queue = new();
                    queue.Enqueue(e.Current);
                    while (e.MoveNext()) {
                        if (queue.Count == indexFromEnd)
                            queue.Dequeue();
                        queue.Enqueue(e.Current);
                    }
                    if (queue.Count == indexFromEnd) {
                        item = queue.Dequeue();
                        return true;
                    }
                }
            }
            item = null;
            return false;
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

    extension(int @this)
    {
        public IEnumerable<int> Range() =>
            Enumerable.Range(0, @this);

        public IEnumerable<int> Range(int count) =>
            Enumerable.Range(@this, count);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool TryGetSpan<T>([NoEnumeration] this IEnumerable<T> @this, out ReadOnlySpan<T> span)
    {
        if (@this.GetType() == typeof(T[])) {
            span = Unsafe.As<T[]>(@this);
            return true;
        }
        else if (@this.GetType() == typeof(List<T>)) {
            span = CollectionsMarshal.AsSpan(Unsafe.As<List<T>>(@this));
            return true;
        }
        else {
            span = default;
            return false;
        }
    }

    private static class EmptyEnumerable<T>
    {
        [field: MaybeNull]
        public static ReadOnlyCollection<T> EmptyList => field ??= new(Array.Empty<T>());
    }
}