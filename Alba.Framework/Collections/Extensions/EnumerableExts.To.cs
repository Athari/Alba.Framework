using System.Collections;
using System.Collections.ObjectModel;

namespace Alba.Framework.Collections;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static partial class EnumerableExts
{
    extension<T>(IEnumerable<T> @this)
    {
        public IList<T> AsList() =>
            @this as IList<T> ?? [ ..@this ];

        public TResult[] ToArray<TResult>(Func<T, TResult> selector) =>
            @this.Select(selector).ToArray();

        public TResult[] ToArray<TResult>(Func<T, int, TResult> selector) =>
            @this.Select(selector).ToArray();

        public List<TResult> ToList<TResult>(Func<T, TResult> selector) =>
            @this.Select(selector).ToList();

        public List<TResult> ToList<TResult>(Func<T, int, TResult> selector) =>
            @this.Select(selector).ToList();

        public HashSet<TKey> ToHashSet<TKey>(Func<T, TKey> keySelector) =>
            @this.Select(keySelector).ToHashSet();

        public HashSet<TKey> ToHashSet<TKey>(Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer) =>
            @this.Select(keySelector).ToHashSet(comparer);

        public SortedSet<T> ToSortedSet() =>
            new(@this);

        public SortedSet<T> ToSortedSet(IComparer<T> comparer) =>
            new(@this, comparer);

        public SortedSet<TKey> ToSortedSet<TKey>(Func<T, TKey> keySelector) =>
            new(@this.Select(keySelector));

        public SortedSet<TKey> ToSortedSet<TKey>(Func<T, TKey> keySelector, IComparer<TKey> comparer) =>
            new(@this.Select(keySelector), comparer);

        public ReadOnlyDictionary<TKey, T> ToReadOnlyDictionary<TKey>(Func<T, TKey> keySelector)
            where TKey : notnull =>
            new(@this.ToDictionary(keySelector));

        public ReadOnlyDictionary<TKey, T> ToReadOnlyDictionary<TKey>(Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer)
            where TKey : notnull =>
            new(@this.ToDictionary(keySelector, comparer));

        public ReadOnlyDictionary<TKey, T> ToReadOnlyDictionary<TKey>(Func<T, TKey> keySelector, Func<T, T> elementSelector)
            where TKey : notnull =>
            new(@this.ToDictionary(keySelector, elementSelector));

        public ReadOnlyDictionary<TKey, T> ToReadOnlyDictionary<TKey>(Func<T, TKey> keySelector, Func<T, T> elementSelector, IEqualityComparer<TKey> comparer)
            where TKey : notnull =>
            new(@this.ToDictionary(keySelector, elementSelector, comparer));

        public ReadOnlyCollection<T> ToReadOnlyList() =>
            @this.ToList().AsReadOnly();
    }

    public static Dictionary<TKey, T> ToDictionary<T, TKey>(this IEnumerable<KeyValuePair<TKey, T>> @this)
        where TKey : notnull =>
        @this.ToDictionary(p => p.Key, p => p.Value);

    public static Dictionary<TKey, T> ToDictionary<T, TKey>(this IEnumerable<DictionaryEntry> @this)
        where TKey : notnull =>
        @this.ToDictionary(p => (TKey)p.Key, p => (T)p.Value!);
}