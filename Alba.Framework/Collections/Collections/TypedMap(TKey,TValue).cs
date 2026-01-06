using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Alba.Framework.Common;
using Alba.Framework.Text;

namespace Alba.Framework.Collections;

[DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(DictionaryDebugView<,>))]
internal class TypedMap<TKey, TValue>
    (IDictionary dictionary, CollectionOptions options = CollectionOptions.Default)
    : IDictionary<TKey, TValue>, IDictionary, IReadOnlyDictionary<TKey, TValue>
    where TKey : notnull
{
    private readonly IDictionary _dictionary = Ensure.NotNull(dictionary);
    private readonly CollectionOptions _options = options | CollectionOptions.None;

    public int Count => _dictionary.Count;
    public bool IsReadOnly => _dictionary.IsReadOnly || (_options & CollectionOptions.ReadOnly) != 0;
    private bool IsStrictApi => (_options & CollectionOptions.StrictApi) != 0;

    bool IDictionary.IsFixedSize => _dictionary.IsFixedSize;
    bool ICollection.IsSynchronized => _dictionary.IsSynchronized;
    object ICollection.SyncRoot => _dictionary.SyncRoot;

    // public

    public TValue this[TKey key] {
        get => TryGetItem(key, out TValue value) ? value : throw KeyNotFound($"{key}");
        set => EnsureNotReadOnly(() => SetItem(key, value));
    }

    [field: MaybeNull]
    public ICollection<TKey> Keys => field ??= new ReadOnlyTypedCollection<TKey>(_dictionary.Keys);

    [field: MaybeNull]
    public ICollection<TValue> Values => field ??= new ReadOnlyTypedCollection<TValue>(_dictionary.Values);

    private bool Contains(TKey key, TValue value) =>
        TryGetItem(key, out TValue v) && v.EqualsValue(value);

    public bool ContainsKey(TKey key) =>
        TryGetItem(key, out _);

    public bool TryGetValue(TKey key, out TValue value) =>
        TryGetItem(key, out value);

    public void Add(TKey key, TValue value) =>
        EnsureNotReadOnly(() => AddItem(key, value));

    public bool Remove(TKey key) =>
        IfNotReadOnly(() => RemoveItem(key));

    public void Clear() =>
        EnsureNotReadOnly(ClearItems);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index) =>
        _dictionary.CopyTo(array, index);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        var it = _dictionary.GetEnumerator();
        try {
            while (it.MoveNext())
                yield return new((TKey)it.Key, (TValue)it.Value!);
        }
        finally {
            (it as IDisposable)?.Dispose();
        }
    }

    // IReadOnlyDictionary<TKey, TValue>

    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

    // IDictionary

    ICollection IDictionary.Keys => _dictionary.Keys;

    ICollection IDictionary.Values => _dictionary.Values;

    object? IDictionary.this[object key] {
        get => this[Ensure.OfType<TKey>(key)];
        set => this[Ensure.OfType<TKey>(key)] = Ensure.OfTypeOrNullable<TValue>(value);
    }

    void IDictionary.Add(object key, object? value) =>
        Add(Ensure.OfType<TKey>(key), Ensure.OfTypeOrNullable<TValue>(value));

    bool IDictionary.Contains(object key) =>
        Ensure.TryOfType<TKey>(key, out var k) && ContainsKey(k);

    void IDictionary.Remove(object key) =>
        IfNotReadOnly(() => Ensure.IfOfType<TKey>(key, k => RemoveItem(k)));

    IDictionaryEnumerator IDictionary.GetEnumerator() =>
        _dictionary.GetEnumerator();

    // ICollection<KeyValuePair<TKey, TValue>>

    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) =>
        Add(item.Key, item.Value);

    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) =>
        Contains(item.Key, item.Value);

    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) =>
        IfNotReadOnly(() => Contains(item.Key, item.Value) && RemoveItem(item.Key));

    // ICollection

    void ICollection.CopyTo(Array array, int index) =>
        _dictionary.CopyTo(array, index);

    // IEnumerable

    IEnumerator IEnumerable.GetEnumerator() =>
        _dictionary.GetEnumerator();

    // virtual

    protected virtual bool TryGetItem(TKey key, out TValue value)
    {
        if (IsStrictApi) {
            var contains = _dictionary.Contains(key);
            value = contains ? (TValue)_dictionary[key]! : default!;
            return contains;
        }
        else {
            try {
                value = (TValue)_dictionary[key]!;
                return true;
            }
            catch (KeyNotFoundException) {
                value = default!;
                return false;
            }
        }
    }

    protected virtual void SetItem(TKey key, TValue value) =>
        _dictionary[key] = value;

    protected virtual void AddItem(TKey key, TValue value) =>
        _dictionary.Add(key, value);

    protected virtual bool RemoveItem(TKey key)
    {
        if (IsStrictApi) {
            var contains = _dictionary.Contains(key);
            if (contains)
                _dictionary.Remove(key);
            return contains;
        }
        else {
            _dictionary.Remove(key);
            return true;
        }
    }

    protected virtual void ClearItems() =>
        _dictionary.Clear();

    // utility

    private static NotSupportedException ReadOnly() =>
        new("Dictionary is read-only.");

    private static KeyNotFoundException KeyNotFound(string key) =>
        new($"Key {key.Qt2()} not found.");

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnsureNotReadOnly()
    {
        if (IsReadOnly)
            throw ReadOnly();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnsureNotReadOnly(Action action)
    {
        EnsureNotReadOnly();
        action();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IfNotReadOnly(Func<bool> fun)
    {
        return !IsReadOnly && fun();
    }
}