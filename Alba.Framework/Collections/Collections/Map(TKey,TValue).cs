using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Alba.Framework.Common;

namespace Alba.Framework.Collections;

/// <summary>
/// Map is to <see cref="Dictionary{TKey,TValue}"/> what <see cref="Collection{T}"/> is to <see cref="List{T}"/>.
/// Allows to override methods of Dictionary.
/// Some members of non-generic interface are only supported if the underlying dictionary supports <see cref="IDictionary"/> interface.
/// </summary>
[DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(DictionaryDebugView<,>))]
public class Map<TKey, TValue>
    (IDictionary<TKey, TValue> dictionary, CollectionOptions options = CollectionOptions.Default)
    : IDictionary<TKey, TValue>, IDictionary, IReadOnlyDictionary<TKey, TValue>
    where TKey : notnull
{
    private readonly IDictionary<TKey, TValue> _dictionary = Ensure.NotNull(dictionary);
    private readonly CollectionOptions _options = options | CollectionOptions.None;

    public Map() : this(new Dictionary<TKey, TValue>()) { }

    private IDictionary? IdSafe => _dictionary as IDictionary;
    private IDictionary Id => IdSafe ?? throw MissingNonGeneric();

    public int Count => _dictionary.Count;
    public bool IsReadOnly => _dictionary.IsReadOnly || (_options & CollectionOptions.ReadOnly) != 0;

    bool IDictionary.IsFixedSize => IdSafe?.IsFixedSize ?? false;
    bool ICollection.IsSynchronized => IdSafe?.IsSynchronized ?? false;
    object ICollection.SyncRoot => IdSafe?.SyncRoot ?? this;

    // public

    public TValue this[TKey key] {
        get => TryGetItem(key, out TValue value) ? value : throw KeyNotFound($"{key}");
        set => EnsureNotReadOnly(() => SetItem(key, value));
    }

    object? IDictionary.this[object key] {
        get => TryGetItem(Ensure.NotNullObject<TKey>(key), out TValue v) ? v : throw KeyNotFound($"{key}");
        set => EnsureNotReadOnly(() => SetItem(Ensure.NotNullObject<TKey>(key), Ensure.NullableOrNotNullObject<TValue>(value)));
    }

    public ICollection<TKey> Keys => _dictionary.Keys;

    public ICollection<TValue> Values => _dictionary.Values;

    public void Add(TKey key, TValue value) =>
        EnsureNotReadOnly(() => AddItem(key, value));

    public bool ContainsKey(TKey key) =>
        TryGetItem(key, out _);

    private bool Contains(TKey key, TValue value) =>
        TryGetItem(key, out TValue v) && v.EqualsValue(value);

    public bool TryGetValue(TKey key, out TValue value) =>
        TryGetItem(key, out value);

    public bool Remove(TKey key) =>
        IfNotReadOnly(() => RemoveItem(key));

    public void Clear() =>
        EnsureNotReadOnly(ClearItems);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index) =>
        _dictionary.CopyTo(array, index);

    // IReadOnlyDictionary<TKey, TValue>

    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

    // IDictionary

    ICollection IDictionary.Keys => Id.Keys;

    ICollection IDictionary.Values => Id.Values;

    void IDictionary.Add(object key, object? value) =>
        Add(Ensure.NotNullObject<TKey>(key), Ensure.NullableOrNotNullObject<TValue>(value));

    bool IDictionary.Contains(object key) =>
        Ensure.TryNotNullObject<TKey>(key, out var k) && ContainsKey(k);

    void IDictionary.Remove(object key) =>
        IfNotReadOnly(() => Ensure.IfNotNullObject<TKey>(key, k => RemoveItem(k)));

    IDictionaryEnumerator IDictionary.GetEnumerator() =>
        Id.GetEnumerator();

    // ICollection<KeyValuePair<TKey, TValue>>

    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) =>
        Add(item.Key, item.Value);

    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) =>
        Contains(item.Key, item.Value);

    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) =>
        IfNotReadOnly(() => Contains(item.Key, item.Value) && RemoveItem(item.Key));

    // ICollection

    void ICollection.CopyTo(Array array, int index) =>
        Id.CopyTo(array, index);

    // IEnumerable

    IEnumerator IEnumerable.GetEnumerator() =>
        Id.GetEnumerator();

    // virtual

    protected virtual bool TryGetItem(TKey key, out TValue value) =>
        _dictionary.TryGetValue(key, out value!);

    protected virtual void SetItem(TKey key, TValue value) =>
        _dictionary[key] = value;

    protected virtual void AddItem(TKey key, TValue value) =>
        _dictionary.Add(key, value);

    protected virtual bool RemoveItem(TKey key) =>
        _dictionary.Remove(key);

    protected virtual void ClearItems() =>
        _dictionary.Clear();

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() =>
        _dictionary.GetEnumerator();

    // utility

    private static NotSupportedException ReadOnly() =>
        new("Dictionary is read-only.");

    private static NotSupportedException MissingNonGeneric() =>
        new("Underlying dictionary does not implement non-generic IDictionary interface.");

    private static KeyNotFoundException KeyNotFound(string key) =>
        new($"Key '{key}' not found.");

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