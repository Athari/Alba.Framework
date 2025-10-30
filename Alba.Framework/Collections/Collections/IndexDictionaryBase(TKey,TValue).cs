using System.Collections;
using System.Diagnostics;

namespace Alba.Framework.Collections;

[Serializable]
[DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(DictionaryDebugView<,>))]
public abstract class IndexDictionaryBase<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, IDictionary
{
    private readonly List<TValue> _values;

    protected IndexDictionaryBase() => _values = [ ];

    protected IndexDictionaryBase(int capacity) => _values = new(capacity);

    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor", Justification = "These virtual methods must be effectively static.")]
    protected IndexDictionaryBase(IEnumerable<KeyValuePair<TKey, TValue>> collection)
    {
        int? capacity = collection switch {
            ICollection<KeyValuePair<TKey, TValue>> icg => icg.Count,
            ICollection ic => ic.Count,
            _ => null,
        };
        _values = capacity is { } cap ? new(cap) : new();
        foreach (var kvp in collection)
            _values[KeyToIndex(kvp.Key)] = kvp.Value;
    }

    private ICollection ValuesIc => _values;

    public int Count => _values.Count;
    object ICollection.SyncRoot => ValuesIc.SyncRoot;
    bool ICollection.IsSynchronized => ValuesIc.IsSynchronized;
    bool IDictionary.IsFixedSize => false;
    bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;
    bool IDictionary.IsReadOnly => false;

    public TValue this[TKey key] {
        get {
            int index = KeyToIndex(key);
            return HasIndex(index) ? _values[index] : throw new KeyNotFoundException();
        }
        set {
            int index = KeyToIndex(key);
            if (index >= _values.Capacity * 2)
                _values.Capacity = index;
            while (_values.Count <= index)
                _values.Add(default!);
            _values[index] = value;
        }
    }

    object? IDictionary.this[object key] {
        get { return this[(TKey)key]; }
        set { this[(TKey)key] = (TValue)value!; }
    }

    public ICollection<TKey> Keys => new KeyCollection(this);
    ICollection IDictionary.Keys => new KeyCollection(this);
    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

    public ICollection<TValue> Values => new ValueCollection(this);
    ICollection IDictionary.Values => new ValueCollection(this);
    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        for (int i = 0; i < _values.Count; i++)
            yield return new(IndexToKey(i), _values[i]);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    IDictionaryEnumerator IDictionary.GetEnumerator() => throw new NotImplementedException();

    public void Add(TKey key, TValue value)
    {
        if (!this[key].EqualsValue(default))
            throw new ArgumentException("An item with the same key has already been added.");
        this[key] = value;
    }

    void IDictionary.Add(object key, object? value) => Add((TKey)key, (TValue)value!);
    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

    public bool ContainsKey(TKey key) => HasIndex(KeyToIndex(key));
    bool IDictionary.Contains(object key) => ContainsKey((TKey)key);

    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
    {
        int index = KeyToIndex(item.Key);
        return HasIndex(index) && _values[index].EqualsValue(item.Value);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        int index = KeyToIndex(key);
        if (HasIndex(index) && !_values[index].EqualsValue(default)) {
            value = _values[index];
            return true;
        }
        else {
            value = default!;
            return false;
        }
    }

    public bool Remove(TKey key)
    {
        int index = KeyToIndex(key);
        if (!HasIndex(index))
            return true;
        if (_values[index].EqualsValue(default))
            return false;
        _values[index] = default!;
        return true;
    }

    void IDictionary.Remove(object key)
    {
        Remove((TKey)key);
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
    {
        int index = KeyToIndex(item.Key);
        if (!HasIndex(index) || !_values[index].EqualsValue(item.Value))
            return false;
        _values[index] = default!;
        return true;
    }

    public void Clear()
    {
        _values.Clear();
    }

    void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        ((ICollection)this).CopyTo(array, arrayIndex);
    }

    void ICollection.CopyTo(Array array, int arrayIndex)
    {
        // from SortedList<TKey, TValue>
        if (array == null || array.Rank != 1 || array.GetLowerBound(0) != 0)
            throw new ArgumentException("Wrong array.", nameof(array));
        if (arrayIndex < 0 || arrayIndex > array.Length || array.Length - arrayIndex < Count)
            throw new ArgumentException("Wrong arrayIndex.", nameof(array));

        if (array is KeyValuePair<TKey, TValue>[] pairArray) {
            for (int i = 0; i < _values.Count; i++)
                pairArray[i + arrayIndex] = new(IndexToKey(i), _values[i]);
        }
        else {
            if (array is not object[] a)
                throw new ArgumentException("Wrong array.", nameof(array));
            try {
                for (int i = 0; i < _values.Count; i++)
                    a[i + arrayIndex] = new KeyValuePair<TKey, TValue>(IndexToKey(i), _values[i]);
            }
            catch (ArrayTypeMismatchException) {
                throw new ArgumentException("Wrong array.", nameof(array));
            }
        }
    }

    protected abstract int KeyToIndex(TKey key);

    protected abstract TKey IndexToKey(int index);

    private bool HasIndex(int index)
    {
        return 0 <= index && index < _values.Count;
    }

    private static NotSupportedException GetReadOnlyException() => new("Collection is read-only.");

    private class KeyCollection(IndexDictionaryBase<TKey, TValue> owner) : ICollection<TKey>, ICollection
    {
        private ICollection OwnerIc => owner;

        public int Count => owner.Count;
        bool ICollection.IsSynchronized => OwnerIc.IsSynchronized;
        object ICollection.SyncRoot => OwnerIc.SyncRoot;
        bool ICollection<TKey>.IsReadOnly => true;

        public bool Contains(TKey item) => owner.ContainsKey(item);

        public IEnumerator<TKey> GetEnumerator()
        {
            for (int i = 0; i < owner._values.Count; i++)
                yield return owner.IndexToKey(i);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void ICollection<TKey>.CopyTo(TKey[] array, int arrayIndex)
        {
            for (int i = 0; i < owner._values.Count; i++)
                array[i + arrayIndex] = owner.IndexToKey(i);
        }

        void ICollection.CopyTo(Array array, int arrayIndex)
        {
            for (int i = 0; i < owner._values.Count; i++)
                array.SetValue(owner.IndexToKey(i), arrayIndex + i);
        }

        void ICollection<TKey>.Add(TKey item) => throw GetReadOnlyException();
        void ICollection<TKey>.Clear() => throw GetReadOnlyException();
        bool ICollection<TKey>.Remove(TKey item) => throw GetReadOnlyException();
    }

    private class ValueCollection(IndexDictionaryBase<TKey, TValue> owner) : ICollection<TValue>, ICollection
    {
        private ICollection OwnerIc => owner;

        public int Count => owner.Count;
        bool ICollection.IsSynchronized => OwnerIc.IsSynchronized;
        object ICollection.SyncRoot => OwnerIc.SyncRoot;
        bool ICollection<TValue>.IsReadOnly => true;

        public IEnumerator<TValue> GetEnumerator() => owner._values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public bool Contains(TValue item) => owner._values.Contains(item);

        void ICollection<TValue>.CopyTo(TValue[] array, int arrayIndex)
        {
            for (int i = 0; i < owner._values.Count; i++)
                array[i + arrayIndex] = owner._values[i];
        }

        void ICollection.CopyTo(Array array, int arrayIndex)
        {
            for (int i = 0; i < owner._values.Count; i++)
                array.SetValue(owner.IndexToKey(i), arrayIndex + i);
        }

        void ICollection<TValue>.Add(TValue item) => throw GetReadOnlyException();
        void ICollection<TValue>.Clear() => throw GetReadOnlyException();
        bool ICollection<TValue>.Remove(TValue item) => throw GetReadOnlyException();
    }
}