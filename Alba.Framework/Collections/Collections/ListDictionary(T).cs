using System.Collections;
using System.Diagnostics;

namespace Alba.Framework.Collections;

[Serializable]
[DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(DictionaryDebugView<,>))]
public class ListDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, IDictionary
{
    private readonly List<KeyValuePair<TKey, TValue>> _list;
    private readonly IEqualityComparer<TKey> _comparer;

    public ListDictionary(int capacity = 0, IEqualityComparer<TKey>? comparer = null)
    {
        _list = new(capacity);
        _comparer = comparer ?? EqualityComparer<TKey>.Default;
    }

    public ListDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey>? comparer = null)
    {
        _list = [.. collection];
        _comparer = comparer ?? EqualityComparer<TKey>.Default;
    }

    private ICollection ListIc => _list;
    private ICollection<KeyValuePair<TKey, TValue>> ListIcg => _list;
    private IList ListIl => _list;

    public int Count => _list.Count;
    bool IDictionary.IsFixedSize => ListIl.IsFixedSize;
    bool IDictionary.IsReadOnly => ListIcg.IsReadOnly;
    bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => ListIcg.IsReadOnly;
    bool ICollection.IsSynchronized => ListIc.IsSynchronized;
    object ICollection.SyncRoot => ListIc.SyncRoot;

    void IDictionary.Add(object key, object? value) => Add((TKey)key, (TValue)value!);
    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

    public bool ContainsKey(TKey key) => TryGetValue(key, out var _);
    bool IDictionary.Contains(object key) => ContainsKey((TKey)key);

    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) =>
        TryGetValue(item.Key, out TValue value) && value.EqualsValue(item.Value);

    void IDictionary.Remove(object key) => Remove((TKey)key);

    public void Clear() => _list.Clear();

    public KeyCollection Keys => new(this);
    ICollection<TKey> IDictionary<TKey, TValue>.Keys => Keys;
    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;
    ICollection IDictionary.Keys => Keys;

    public ValueCollection Values => new(this);
    ICollection<TValue> IDictionary<TKey, TValue>.Values => Values;
    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;
    ICollection IDictionary.Values => Values;

    public List<KeyValuePair<TKey, TValue>>.Enumerator GetEnumerator() => _list.GetEnumerator();
    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => GetEnumerator();
    IDictionaryEnumerator IDictionary.GetEnumerator() => new DictionaryEnumerator(GetEnumerator());
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index) => _list.CopyTo(array, index);
    void ICollection.CopyTo(Array array, int index) => ListIc.CopyTo(array, index);

    public TValue this[TKey key]
    {
        get
        {
            if (TryGetValue(key, out TValue value))
                return value;
            throw new KeyNotFoundException($"Key '{key}' not found.");
        }
        set
        {
            var newPair = new KeyValuePair<TKey, TValue>(key, value);
            for (int i = 0; i < _list.Count; i++) {
                if (_comparer.Equals(_list[i].Key, key)) {
                    _list[i] = newPair;
                    return;
                }
            }
            _list.Add(newPair);
        }
    }

    object? IDictionary.this[object key]
    {
        get => this[(TKey)key];
        set => this[(TKey)key] = (TValue)value!;
    }

    public void Add(TKey key, TValue value)
    {
        if (TryGetValue(key, out TValue oldValue))
            throw new ArgumentException($"Key '{key}' already exists.");
        _list.Add(new(key, oldValue));
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        foreach (var pair in _list) {
            if (_comparer.Equals(pair.Key, key)) {
                value = pair.Value;
                return true;
            }
        }
        value = default!;
        return false;
    }

    public bool Remove(TKey key)
    {
        for (int i = 0; i < _list.Count; i++) {
            if (_comparer.Equals(_list[i].Key, key)) {
                _list.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
    {
        if (TryGetValue(item.Key, out TValue value) && value.EqualsValue(item.Value))
            return Remove(item.Key);
        return false;
    }

    [Serializable]
    [DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    public sealed class KeyCollection(ListDictionary<TKey, TValue> owner) : ICollection<TKey>, ICollection
    {
        public int Count => owner.Count;
        bool ICollection<TKey>.IsReadOnly => true;
        bool ICollection.IsSynchronized => owner.ListIc.IsSynchronized;
        object ICollection.SyncRoot => owner.ListIc.SyncRoot;

        public IEnumerator<TKey> GetEnumerator()
        {
            foreach (KeyValuePair<TKey, TValue> pair in owner._list)
                yield return pair.Key;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool Contains(TKey item)
        {
            foreach (KeyValuePair<TKey, TValue> pair in owner._list)
                if (owner._comparer.Equals(pair.Key, item))
                    return true;
            return false;
        }

        public void CopyTo(TKey[] array, int index)
        {
            for (int i = 0; i < owner._list.Count; i++)
                array[index + i] = owner._list[i].Key;
        }

        void ICollection.CopyTo(Array array, int index)
        {
            for (int i = 0; i < owner._list.Count; i++)
                array.SetValue(owner._list[i].Key, index + i);
        }

        void ICollection<TKey>.Add(TKey item) => throw NewCollectionReadOnlyException();
        void ICollection<TKey>.Clear() => throw NewCollectionReadOnlyException();
        bool ICollection<TKey>.Remove(TKey item) => throw NewCollectionReadOnlyException();
    }

    [Serializable]
    [DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    public sealed class ValueCollection(ListDictionary<TKey, TValue> owner) : ICollection<TValue>, ICollection
    {
        public int Count => owner.Count;
        bool ICollection<TValue>.IsReadOnly => true;
        bool ICollection.IsSynchronized => owner.ListIc.IsSynchronized;
        object ICollection.SyncRoot => owner.ListIc.SyncRoot;

        public IEnumerator<TValue> GetEnumerator()
        {
            foreach (KeyValuePair<TKey, TValue> pair in owner._list)
                yield return pair.Value;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool Contains(TValue item)
        {
            foreach (KeyValuePair<TKey, TValue> pair in owner._list)
                if (pair.Value.EqualsValue(item))
                    return true;
            return false;
        }

        public void CopyTo(TValue[] array, int index)
        {
            for (int i = 0; i < owner._list.Count; i++)
                array[index + i] = owner._list[i].Value;
        }

        void ICollection.CopyTo(Array array, int index)
        {
            for (int i = 0; i < owner._list.Count; i++)
                array.SetValue(owner._list[i].Value, index + i);
        }

        void ICollection<TValue>.Add(TValue item) => throw NewCollectionReadOnlyException();
        void ICollection<TValue>.Clear() => throw NewCollectionReadOnlyException();
        bool ICollection<TValue>.Remove(TValue item) => throw NewCollectionReadOnlyException();
    }

    private class DictionaryEnumerator(List<KeyValuePair<TKey, TValue>>.Enumerator enumerator) : IDictionaryEnumerator
    {
        private List<KeyValuePair<TKey, TValue>>.Enumerator _enumerator = enumerator;

        public object Current => _enumerator.Current;
        public object Key => _enumerator.Current.Key!;
        public object Value => _enumerator.Current.Value!;
        public DictionaryEntry Entry => new(Key, Value);

        public bool MoveNext() => _enumerator.MoveNext();
        void IEnumerator.Reset() => ((IEnumerator)_enumerator).Reset();
    }

    private static NotSupportedException NewCollectionReadOnlyException() => new("Collection is read-only.");
}