using System.Collections;
using System.Collections.ObjectModel;

namespace Alba.Framework.Collections;

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

        public HashSet<TKey> ToHashSet<TKey>(Func<T, TKey> keySelector, IEqualityComparer<TKey>? comparer) =>
            @this.Select(keySelector).ToHashSet(comparer);

        public SortedSet<T> ToSortedSet() =>
            new(@this);

        public SortedSet<T> ToSortedSet(IComparer<T>? comparer) =>
            new(@this, comparer);

        public SortedSet<TKey> ToSortedSet<TKey>(Func<T, TKey> keySelector) =>
            new(@this.Select(keySelector));

        public SortedSet<TKey> ToSortedSet<TKey>(Func<T, TKey> keySelector, IComparer<TKey>? comparer) =>
            new(@this.Select(keySelector), comparer);

        public Dictionary<TKey, T> ToDictionarySafe<TKey>(Func<T, TKey> keySelector)
            where TKey : notnull =>
            @this.ToDictionarySafe(keySelector, null);

        public Dictionary<TKey, T> ToDictionarySafe<TKey>(Func<T, TKey> keySelector, IEqualityComparer<TKey>? comparer)
            where TKey : notnull
        {
            var dic = new Dictionary<TKey, T>(comparer);
            foreach (var item in @this)
                dic[keySelector(item)] = item;
            return dic;
        }

        public Dictionary<TKey, TValue> ToDictionarySafe<TKey, TValue>(Func<T, TKey> keySelector, Func<T, TValue> elementSelector)
            where TKey : notnull =>
            @this.ToDictionarySafe(keySelector, elementSelector, null);

        public Dictionary<TKey, TValue> ToDictionarySafe<TKey, TValue>(Func<T, TKey> keySelector, Func<T, TValue> elementSelector, IEqualityComparer<TKey>? comparer)
            where TKey : notnull
        {
            var dic = new Dictionary<TKey, TValue>(comparer);
            foreach (var item in @this)
                dic[keySelector(item)] = elementSelector(item);
            return dic;
        }

        public ReadOnlyDictionary<TKey, T> ToReadOnlyDictionary<TKey>(Func<T, TKey> keySelector)
            where TKey : notnull =>
            new(@this.ToDictionary(keySelector));

        public ReadOnlyDictionary<TKey, T> ToReadOnlyDictionary<TKey>(Func<T, TKey> keySelector, IEqualityComparer<TKey>? comparer)
            where TKey : notnull =>
            new(@this.ToDictionary(keySelector, comparer));

        public ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(Func<T, TKey> keySelector, Func<T, TValue> elementSelector)
            where TKey : notnull =>
            new(@this.ToDictionary(keySelector, elementSelector));

        public ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(Func<T, TKey> keySelector, Func<T, TValue> elementSelector, IEqualityComparer<TKey>? comparer)
            where TKey : notnull =>
            new(@this.ToDictionary(keySelector, elementSelector, comparer));

        public ReadOnlyDictionary<TKey, T> ToReadOnlyDictionarySafe<TKey>(Func<T, TKey> keySelector)
            where TKey : notnull =>
            new(@this.ToDictionarySafe(keySelector));

        public ReadOnlyDictionary<TKey, T> ToReadOnlyDictionarySafe<TKey>(Func<T, TKey> keySelector, IEqualityComparer<TKey>? comparer)
            where TKey : notnull =>
            new(@this.ToDictionarySafe(keySelector, comparer));

        public ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionarySafe<TKey, TValue>(Func<T, TKey> keySelector, Func<T, TValue> elementSelector)
            where TKey : notnull =>
            new(@this.ToDictionarySafe(keySelector, elementSelector));

        public ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionarySafe<TKey, TValue>(Func<T, TKey> keySelector, Func<T, TValue> elementSelector, IEqualityComparer<TKey>? comparer)
            where TKey : notnull =>
            new(@this.ToDictionarySafe(keySelector, elementSelector, comparer));

        public ReadOnlyCollection<T> ToReadOnlyList() =>
            @this.ToList().AsReadOnly();
    }

    extension<T, TKey>(IEnumerable<KeyValuePair<TKey, T>> @this) where TKey : notnull
    {
        public Dictionary<TKey, T> ToDictionary() =>
            @this.ToDictionary(p => p.Key, p => p.Value);

        public Dictionary<TKey, T> ToDictionarySafe() =>
            @this.ToDictionarySafe(p => p.Key, p => p.Value);
    }

    extension(IEnumerable<DictionaryEntry> @this)
    {
        public Dictionary<TKey, T> ToDictionary<T, TKey>()
            where TKey : notnull =>
            @this.ToDictionary(p => (TKey)p.Key, p => (T)p.Value!);

        public Dictionary<TKey, T> ToDictionarySafe<T, TKey>()
            where TKey : notnull =>
            @this.ToDictionarySafe(p => (TKey)p.Key, p => (T)p.Value!);
    }
}