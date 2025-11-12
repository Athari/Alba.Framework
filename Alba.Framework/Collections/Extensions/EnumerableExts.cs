using System.Collections;
using System.Collections.ObjectModel;

namespace Alba.Framework.Collections;

[PublicAPI]
[SuppressMessage("Style", "IDE0305: Use collection expression for fluent", Justification = "LINQ chains")]
public static class EnumerableExts
{
    private const string ErrorNoElements = "Sequence contains no elements.";

    private static readonly Random Rnd = new();

    public static ReadOnlyCollection<T> EmptyList<T>()
    {
        return EmptyEnumerable<T>.EmptyList;
    }

    public static ReadOnlyDictionary<TKey, TValue> EmptyDictionary<TKey, TValue>()
        where TKey : notnull
    {
        return ReadOnlyDictionary<TKey, TValue>.Empty;
    }

    public static T? AtOrDefault<T>(this IEnumerable<T> @this, int index) =>
        @this.ElementAtOrDefault(index);

    public static T? AtOrDefault<T>(this IEnumerable<T> @this, Index index) =>
        @this.ElementAtOrDefault(index);

    //public static IEnumerable<T> Concat<T>(this IEnumerable<T> @this, params IEnumerable<T> values)
    //{
    //    foreach (var item in @this)
    //        yield return item;
    //    foreach (var value in values)
    //        yield return value;
    //}

    //public static IEnumerable<T> Concat<T>(this IEnumerable<T> @this, params IEnumerable<IEnumerable<T>> enumerables)
    //{
    //    foreach (var item in @this)
    //        yield return item;
    //    foreach (T value in enumerables.Flatten())
    //        yield return value;
    //}

    public static IEnumerable<T> Concat<T>(this IEnumerable<T> @this, T value)
    {
        foreach (T item in @this)
            yield return item;
        yield return value;
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

    public static int IndexOf<T>(this IEnumerable<T> @this, T value, IEqualityComparer<T>? comparer)
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

    public static int IndexOf<T>(this IEnumerable<T> @this, Func<T, bool> predicate)
    {
        int i = 0;
        foreach (T item in @this) {
            if (predicate(item))
                return i;
            i++;
        }
        return -1;
    }

    public static int LastIndexOf<T>(this IEnumerable<T> @this, T value, IEqualityComparer<T>? comparer)
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

    public static int LastIndexOf<T>(this IEnumerable<T> @this, Func<T, bool> predicate)
    {
        int i = 0, index = -1;
        foreach (T item in @this) {
            if (predicate(item))
                index = i;
            i++;
        }
        return index;
    }

    public static IEnumerable<T> OrderByIndices<T>(this IEnumerable<T> @this, IEnumerable<int> indices)
    {
        IList<T> list = @this.AsList();
        return indices.Select(index => list[index]);
    }

    public static IEnumerable<TResult> Intersect<T, TResult>(this IEnumerable<T> first, IEnumerable<T> second, Func<T, T, TResult> resultSelector, IEqualityComparer<T> comparer)
        where T : notnull
    {
        var dic = second.ToDictionary(i => i, comparer);
        foreach (T item in first)
            if (dic.TryGetValue(item, out var value))
                yield return resultSelector(item, value);
    }

    public static IEnumerable<T> Inverse<T>(this IEnumerable<T> @this)
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

    public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> @this, int size)
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

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> @this, Random? rnd = null)
    {
        return @this.OrderBy(_ => (rnd ?? Rnd).NextDouble());
    }

    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> @this)
    {
        return @this.Where(i => i != null)!;
    }

    public static IEnumerable<T> WhereNotDefault<T>(this IEnumerable<T> @this)
    {
        var def = default(T);
        return @this.Where(i => !i.EqualsValue(def));
    }

    public static IEnumerable<(T1, T2)> SelectWith<T1, T2>(this IEnumerable<T1> source, Func<T1, T2> secondSelector)
    {
        using var e = source.GetEnumerator();
        while (e.MoveNext())
            yield return (e.Current, secondSelector(e.Current));
    }

    public static IEnumerable<(T1, T2, T3)> SelectWith<T1, T2, T3>(this IEnumerable<T1> source, Func<T1, T2> secondSelector, Func<T1, T3> thirdSelector)
    {
        using var e = source.GetEnumerator();
        while (e.MoveNext())
            yield return (e.Current, secondSelector(e.Current), thirdSelector(e.Current));
    }

    public static IEnumerable<TResult> SelectWith<T1, T2, TResult>(this IEnumerable<T1> source, Func<T1, T2> secondSelector, Func<T1, T2, TResult> resultSelector)
    {
        using var e = source.GetEnumerator();
        while (e.MoveNext())
            yield return resultSelector(e.Current, secondSelector(e.Current));
    }

    public static IEnumerable<TResult> SelectWith<T1, T2, T3, TResult>(this IEnumerable<T1> source, Func<T1, T2> secondSelector, Func<T1, T3> thirdSelector, Func<T1, T2, T3, TResult> resultSelector)
    {
        using var e = source.GetEnumerator();
        while (e.MoveNext())
            yield return resultSelector(e.Current, secondSelector(e.Current), thirdSelector(e.Current));
    }

    public static IEnumerable<(T1, T2, T3)> Zip<T1, T2, T3, TResult>(this IEnumerable<T1> source, IEnumerable<T2> second, IEnumerable<T3> third)
    {
        using var e1 = source.GetEnumerator();
        using var e2 = second.GetEnumerator();
        using var e3 = third.GetEnumerator();
        while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext())
            yield return (e1.Current, e2.Current, e3.Current);
    }

    public static IEnumerable<TResult> Zip<T1, T2, T3, TResult>(this IEnumerable<T1> source, IEnumerable<T2> second, IEnumerable<T3> third, Func<T1, T2, T3, TResult> selector)
    {
        using var e1 = source.GetEnumerator();
        using var e2 = second.GetEnumerator();
        using var e3 = third.GetEnumerator();
        while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext())
            yield return selector(e1.Current, e2.Current, e3.Current);
    }

    public static bool AllEqual<T>(this IEnumerable<T> @this)
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

    public static bool AllEqual<T>(this IEnumerable<T> @this, out T value)
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

    public static string JoinString<T>(this IEnumerable<T> @this, string separator) =>
        string.Join(separator, @this);

    public static string JoinString(this IEnumerable<string> @this, string separator) =>
        string.Join(separator, @this);

    public static string ConcatString<T>(this IEnumerable<T> @this) =>
        string.Concat(@this);

    public static string ConcatString(this IEnumerable<string> @this) =>
        string.Concat(@this);

    public static IList<T> AsList<T>(this IEnumerable<T> @this) =>
        @this as IList<T> ?? [ .. @this ];

    public static TResult[] ToArray<T, TResult>(this IEnumerable<T> @this, Func<T, TResult> selector) =>
        @this.Select(selector).ToArray();

    public static TResult[] ToArray<T, TResult>(this IEnumerable<T> @this, Func<T, int, TResult> selector) =>
        @this.Select(selector).ToArray();

    public static List<TResult> ToList<T, TResult>(this IEnumerable<T> @this, Func<T, TResult> selector) =>
        @this.Select(selector).ToList();

    public static List<TResult> ToList<T, TResult>(this IEnumerable<T> @this, Func<T, int, TResult> selector) =>
        @this.Select(selector).ToList();

    public static Dictionary<TKey, T> ToDictionary<TKey, T>(this IEnumerable<KeyValuePair<TKey, T>> @this)
        where TKey : notnull =>
        @this.ToDictionary(p => p.Key, p => p.Value);

    public static Dictionary<TKey, T> ToDictionary<TKey, T>(this IEnumerable<DictionaryEntry> @this)
        where TKey : notnull =>
        @this.ToDictionary(p => (TKey)p.Key, p => (T)p.Value!);

    [SuppressMessage("Style", "IDE0305:Simplify collection initialization", Justification = "Readability")]
    public static HashSet<TKey> ToHashSet<TKey, T>(this IEnumerable<T> @this, Func<T, TKey> keySelector) =>
        @this.Select(keySelector).ToHashSet();

    public static HashSet<TKey> ToHashSet<TKey, T>(this IEnumerable<T> @this, Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer) =>
        @this.Select(keySelector).ToHashSet(comparer);

    public static SortedSet<T> ToSortedSet<T>(this IEnumerable<T> @this) =>
        [ .. @this ];

    public static SortedSet<T> ToSortedSet<T>(this IEnumerable<T> @this, IComparer<T> comparer) =>
        new(@this, comparer);

    public static SortedSet<TKey> ToSortedSet<TKey, T>(this IEnumerable<T> @this, Func<T, TKey> keySelector) =>
        [ .. @this.Select(keySelector) ];

    public static SortedSet<TKey> ToSortedSet<TKey, T>(this IEnumerable<T> @this, Func<T, TKey> keySelector, IComparer<TKey> comparer) =>
        new(@this.Select(keySelector), comparer);

    public static ReadOnlyDictionary<TKey, T> ToReadOnlyDictionary<TKey, T>(this IEnumerable<T> @this,
        Func<T, TKey> keySelector)
        where TKey : notnull =>
        new(@this.ToDictionary(keySelector));

    public static ReadOnlyDictionary<TKey, T> ToReadOnlyDictionary<TKey, T>(this IEnumerable<T> @this,
        Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer)
        where TKey : notnull =>
        new(@this.ToDictionary(keySelector, comparer));

    public static ReadOnlyDictionary<TKey, T> ToReadOnlyDictionary<TKey, T>(this IEnumerable<T> @this,
        Func<T, TKey> keySelector, Func<T, T> elementSelector)
        where TKey : notnull =>
        new(@this.ToDictionary(keySelector, elementSelector));

    public static ReadOnlyDictionary<TKey, T> ToReadOnlyDictionary<TKey, T>(this IEnumerable<T> @this,
        Func<T, TKey> keySelector, Func<T, T> elementSelector, IEqualityComparer<TKey> comparer)
        where TKey : notnull =>
        new(@this.ToDictionary(keySelector, elementSelector, comparer));

    public static ReadOnlyCollection<T> ToReadOnlyList<T>(this IEnumerable<T> @this) =>
        @this.ToList().AsReadOnly();

    public static IEnumerable<T> TraverseList<T>(this T root, Func<T, T?> getNext) where T : class
    {
        for (T? current = root; current != null; current = getNext(current))
            yield return current;
    }

    public static IEnumerable<T> TraverseTree<T>(this T root, Func<T, IEnumerable<T>> getChildren)
    {
        return TraverseTreeDepth(root, getChildren);
    }

    public static IEnumerable<T> TraverseTreeDepth<T>(this T root, Func<T, IEnumerable<T>> getChildren)
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

    public static IEnumerable<T> TraverseTreeBreadth<T>(this T root, Func<T, IEnumerable<T>> getChildren)
    {
        var queue = new Queue<T>();
        queue.Enqueue(root);

        while (queue.Count != 0) {
            T item = queue.Dequeue();
            yield return item;
            queue.EnqueueRange(getChildren(item));
        }
    }

    public static IEnumerable<T> TraverseGraph<T>(this T root, Func<T, IEnumerable<T>> getChildren)
    {
        var seen = new HashSet<T>();
        var stack = new Stack<T>();
        stack.Push(root);

        while (stack.Count != 0) {
            T item = stack.Pop();
            if (!seen.Add(item))
                continue;
            yield return item;
            stack.PushRange(getChildren(item));
        }
    }

    public static IEnumerable<T> ReturnEnumerable<T>(this T item)
    {
        yield return item;
    }

    public static IEnumerable<int> Range(this int count) =>
        Enumerable.Range(0, count);

    public static IEnumerable<int> Range(this int start, int count) =>
        Enumerable.Range(start, count);

    public static IEnumerable<int> ToRange(this Range @this, bool endInclusive = false)
    {
        if (@this.Start.IsFromEnd || @this.End.IsFromEnd)
            throw new ArgumentOutOfRangeException(nameof(@this), @this, "Range start and end must both be from start");
        (int offset, int length) = @this.GetOffsetAndLength(int.MaxValue);
        return Enumerable.Range(offset, length + (endInclusive ? 1 : 0));
    }

    public static IEnumerable<int> ToRange(this Range @this, int count, bool endInclusive = false)
    {
        (int offset, int length) = @this.GetOffsetAndLength(count);
        return Enumerable.Range(offset, length + (endInclusive ? 1 : 0));
    }

    private static class EmptyEnumerable<T>
    {
        [field: MaybeNull]
        public static ReadOnlyCollection<T> EmptyList => field ??= new(Array.Empty<T>());
    }
}