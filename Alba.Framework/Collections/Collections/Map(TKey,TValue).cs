using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Alba.Framework.Collections;

/// <summary>
/// Map is to <see cref="Dictionary{TKey,TValue}"/> what <see cref="Collection{T}"/> is to <see cref="List{T}"/>.
/// Allows to override methods of Dictionary.
/// Some members of non-generic interface are only supported if the underlying dictionary supports <see cref="IDictionary"/> interface.
/// </summary>
[Serializable, ComVisible(false)]
[DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(DictionaryDebugView<,>))]
public class Map<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, IDictionary
    where TKey : notnull
{
    private static readonly bool IsValueNullable = default(TValue) is null;

    private readonly IDictionary<TKey, TValue> _dictionary;

    public Map()
    {
        _dictionary = new Dictionary<TKey, TValue>();
    }

    public Map(IDictionary<TKey, TValue> dictionary)
    {
        Guard.IsNotNull(dictionary, nameof(dictionary));
        _dictionary = dictionary;
    }

    private IDictionary? DictionaryId => _dictionary as IDictionary;
    private ICollection? DictionaryIc => _dictionary as ICollection;

    public int Count => _dictionary.Count;

    bool IDictionary.IsFixedSize => DictionaryId?.IsFixedSize ?? false;
    bool IDictionary.IsReadOnly => _dictionary.IsReadOnly;
    bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => _dictionary.IsReadOnly;
    bool ICollection.IsSynchronized => DictionaryIc?.IsSynchronized ?? false;
    object ICollection.SyncRoot => DictionaryIc?.SyncRoot ?? this;

    public TValue this[TKey key]
    {
        get
        {
            if (!TryGetItem(key, out TValue value))
                throw new KeyNotFoundException($"Key '{key}' not found.");
            return value;
        }
        set
        {
            GuardNotReadOnly();
            SetItem(key, value);
        }
    }

    object? IDictionary.this[object key]
    {
        get
        {
            Guard.IsNotNull(key, nameof(key));
            if (key is not TKey k)
                return null;
            return TryGetItem(k, out TValue value) ? value : default;
        }
        set
        {
            GuardNotReadOnly();
            Guard.IsNotNull(key, nameof(key));
            if (key is not TKey k)
                throw new ArgumentException("Wrong key type.", nameof(key));
            if (!IsValueNullable)
                Guard.IsNotNull(value, nameof(value));
            if (value is not TValue v)
                throw new ArgumentException("Wrong value type.", nameof(value));
            SetItem(k, v);
        }
    }

    protected virtual void SetItem(TKey key, TValue value)
    {
        _dictionary[key] = value;
    }

    public void Add(TKey key, TValue value)
    {
        GuardNotReadOnly();
        AddItem(key, value);
    }

    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
    {
        GuardNotReadOnly();
        AddItem(item.Key, item.Value);
    }

    void IDictionary.Add(object key, object? value)
    {
        GuardNotReadOnly();
        Guard.IsNotNull(key, nameof(key));
        if (key is not TKey k)
            throw new ArgumentException("Wrong key type.", nameof(key));
        if (!IsValueNullable)
            Guard.IsNotNull(value, nameof(value));
        if (value is not TValue v)
            throw new ArgumentException("Wrong value type.", nameof(value));
        AddItem(k, v);
    }

    protected virtual void AddItem(TKey key, TValue value)
    {
        _dictionary.Add(key, value);
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
    {
        return TryGetItem(item.Key, out TValue value) && value.EqualsValue(item.Value);
    }

    public bool ContainsKey(TKey key)
    {
        return TryGetItem(key, out _);
    }

    bool IDictionary.Contains(object key)
    {
        Guard.IsNotNull(key, nameof(key));
        return key is TKey k && ContainsKey(k);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return TryGetItem(key, out value);
    }

    protected virtual bool TryGetItem(TKey key, out TValue value)
    {
        return _dictionary.TryGetValue(key, out value!);
    }

    public bool Remove(TKey key)
    {
        GuardNotReadOnly();
        return RemoveItem(key);
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
    {
        GuardNotReadOnly();
        if (TryGetItem(item.Key, out TValue value) && value.EqualsValue(item.Value))
            RemoveItem(item.Key);
        return false;
    }

    void IDictionary.Remove(object key)
    {
        GuardNotReadOnly();
        Guard.IsNotNull(key, nameof(key));
        if (key is not TKey k)
            return;
        RemoveItem(k);
    }

    protected virtual bool RemoveItem(TKey key)
    {
        return _dictionary.Remove(key);
    }

    public void Clear()
    {
        GuardNotReadOnly();
        ClearItems();
    }

    protected virtual void ClearItems()
    {
        _dictionary.Clear();
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
    {
        _dictionary.CopyTo(array, index);
    }

    void ICollection.CopyTo(Array array, int index)
    {
        GuardHasNonGeneric();
        DictionaryId.CopyTo(array, index);
    }

    public ICollection<TKey> Keys => _dictionary.Keys;
    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;
    ICollection IDictionary.Keys => DictionaryId?.Keys ?? Keys as ICollection ?? throw GetMissingNonGenericException();

    public ICollection<TValue> Values => _dictionary.Values;
    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;
    ICollection IDictionary.Values => DictionaryId?.Values ?? Values as ICollection ?? throw GetMissingNonGenericException();

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();
    IDictionaryEnumerator IDictionary.GetEnumerator() => DictionaryId?.GetEnumerator() ?? throw GetMissingNonGenericException();
    IEnumerator IEnumerable.GetEnumerator() => DictionaryId?.GetEnumerator() ?? throw GetMissingNonGenericException();

    private static NotSupportedException GetReadOnlyException() =>
        new("Dictionary is read-only.");

    private static NotSupportedException GetMissingNonGenericException() =>
        new("Underlying dictionary does not implement non-generic IDictionary interface.");

    private void GuardNotReadOnly()
    {
        if (DictionaryId?.IsReadOnly ?? false)
            throw GetReadOnlyException();
    }

    [MemberNotNull(nameof(DictionaryId))]
    private void GuardHasNonGeneric()
    {
        if (DictionaryId == null)
            throw GetMissingNonGenericException();
    }
}