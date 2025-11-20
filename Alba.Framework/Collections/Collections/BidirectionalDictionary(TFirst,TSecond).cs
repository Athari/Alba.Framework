using System.Collections;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Alba.Framework.Collections;

[Serializable]
[DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(DictionaryDebugView<,>))]
[Obsolete("Use Sdcb.Collections.BidirectionalDictionary instead https://www.nuget.org/packages/Sdcb.Collections")]
public class BidirectionalDictionary<TFirst, TSecond>
    : IDictionary<TFirst, TSecond>, IReadOnlyDictionary<TFirst, TSecond>, IDictionary
    where TFirst : notnull
    where TSecond : notnull
{
    private readonly Dictionary<TFirst, TSecond> _firstToSecond;
    [NonSerialized]
    private readonly Dictionary<TSecond, TFirst> _secondToFirst;
    [NonSerialized]
    private readonly ReverseDictionary _reverseDictionary;

    public BidirectionalDictionary() : this(EqualityComparer<TFirst>.Default, EqualityComparer<TSecond>.Default) { }

    public BidirectionalDictionary(IEqualityComparer<TFirst>? firstComparer, IEqualityComparer<TSecond>? secondComparer)
    {
        _firstToSecond = new(firstComparer);
        _secondToFirst = new(secondComparer);
        _reverseDictionary = new(this);
    }

    private ICollection FirstToSecondIc => _firstToSecond;
    private ICollection<KeyValuePair<TFirst, TSecond>> FirstToSecondIcg => _firstToSecond;
    private ICollection<KeyValuePair<TSecond, TFirst>> SecondToFirstIcg => _secondToFirst;
    private IDictionary FirstToSecondId => _firstToSecond;
    private IReadOnlyDictionary<TFirst, TSecond> FirstToSecondIrodg => _firstToSecond;
    private IDictionary SecondToFirstId => _secondToFirst;
    private IReadOnlyDictionary<TSecond, TFirst> SecondToFirstIrodg => _secondToFirst;

    public IDictionary<TSecond, TFirst> Reverse => _reverseDictionary;

    public int Count => _firstToSecond.Count;
    object ICollection.SyncRoot => FirstToSecondIc.SyncRoot;
    bool ICollection.IsSynchronized => FirstToSecondIc.IsSynchronized;
    bool IDictionary.IsFixedSize => FirstToSecondId.IsFixedSize;
    public bool IsReadOnly => FirstToSecondId.IsReadOnly || SecondToFirstId.IsReadOnly;

    public ICollection<TFirst> Keys => _firstToSecond.Keys;
    ICollection IDictionary.Keys => FirstToSecondId.Keys;
    IEnumerable<TFirst> IReadOnlyDictionary<TFirst, TSecond>.Keys => FirstToSecondIrodg.Keys;

    public ICollection<TSecond> Values => _firstToSecond.Values;
    ICollection IDictionary.Values => FirstToSecondId.Values;
    IEnumerable<TSecond> IReadOnlyDictionary<TFirst, TSecond>.Values => FirstToSecondIrodg.Values;

    public IEnumerator<KeyValuePair<TFirst, TSecond>> GetEnumerator() => _firstToSecond.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    IDictionaryEnumerator IDictionary.GetEnumerator() => FirstToSecondId.GetEnumerator();

    public bool ContainsKey(TFirst key) => _firstToSecond.ContainsKey(key);
    bool IDictionary.Contains(object key) => FirstToSecondId.Contains(key);
    bool ICollection<KeyValuePair<TFirst, TSecond>>.Contains(KeyValuePair<TFirst, TSecond> item) => _firstToSecond.Contains(item);

    public bool TryGetValue(TFirst key, out TSecond value) => _firstToSecond.TryGetValue(key, out value!);

    public TSecond this[TFirst key] {
        get => _firstToSecond[key];
        set {
            _firstToSecond[key] = value;
            _secondToFirst[value] = key;
        }
    }

    object? IDictionary.this[object key] {
        get => FirstToSecondId[key];
        set {
            Guard.IsNotNull(value);
            FirstToSecondId[key] = value;
            SecondToFirstId[value] = key;
        }
    }

    public void Add(TFirst key, TSecond value)
    {
        _firstToSecond.Add(key, value);
        _secondToFirst.Add(value, key);
    }

    void IDictionary.Add(object key, object? value)
    {
        Guard.IsNotNull(value);
        FirstToSecondId.Add(key, value);
        SecondToFirstId.Add(value, key);
    }

    void ICollection<KeyValuePair<TFirst, TSecond>>.Add(KeyValuePair<TFirst, TSecond> item)
    {
        _firstToSecond.Add(item.Key, item.Value);
        _secondToFirst.Add(item.Value, item.Key);
    }

    public bool Remove(TFirst key)
    {
        if (!_firstToSecond.Remove(key, out var value))
            return false;
        _secondToFirst.Remove(value);
        return true;
    }

    void IDictionary.Remove(object key)
    {
        var firstToSecond = FirstToSecondId;
        if (!firstToSecond.Contains(key))
            return;
        var value = firstToSecond[key];
        Guard.IsNotNull(value);
        firstToSecond.Remove(key);
        SecondToFirstId.Remove(value);
    }

    bool ICollection<KeyValuePair<TFirst, TSecond>>.Remove(KeyValuePair<TFirst, TSecond> item)
    {
        if (!FirstToSecondIcg.Remove(item))
            return false;
        SecondToFirstIcg.Remove(item.Reverse());
        return true;
    }

    public void Clear()
    {
        _firstToSecond.Clear();
        _secondToFirst.Clear();
    }

    void ICollection<KeyValuePair<TFirst, TSecond>>.CopyTo(KeyValuePair<TFirst, TSecond>[] array, int arrayIndex) => FirstToSecondIcg.CopyTo(array, arrayIndex);
    void ICollection.CopyTo(Array array, int index) => FirstToSecondId.CopyTo(array, index);

    [OnDeserialized]
    internal void OnDeserialized(StreamingContext context)
    {
        _secondToFirst.Clear();
        foreach (var item in _firstToSecond)
            _secondToFirst.Add(item.Value, item.Key);
    }

    private class ReverseDictionary(BidirectionalDictionary<TFirst, TSecond> owner) :
        IDictionary<TSecond, TFirst>, IReadOnlyDictionary<TSecond, TFirst>, IDictionary
    {
        public int Count => owner._secondToFirst.Count;
        object ICollection.SyncRoot => ((ICollection)owner._secondToFirst).SyncRoot;
        bool ICollection.IsSynchronized => ((ICollection)owner._secondToFirst).IsSynchronized;
        bool IDictionary.IsFixedSize => owner.SecondToFirstId.IsFixedSize;
        public bool IsReadOnly => owner.SecondToFirstId.IsReadOnly || owner.FirstToSecondId.IsReadOnly;

        public ICollection<TSecond> Keys => owner._secondToFirst.Keys;
        ICollection IDictionary.Keys => owner.SecondToFirstId.Keys;
        IEnumerable<TSecond> IReadOnlyDictionary<TSecond, TFirst>.Keys => owner.SecondToFirstIrodg.Keys;

        public ICollection<TFirst> Values => owner._secondToFirst.Values;
        ICollection IDictionary.Values => owner.SecondToFirstId.Values;
        IEnumerable<TFirst> IReadOnlyDictionary<TSecond, TFirst>.Values => owner.SecondToFirstIrodg.Values;

        public IEnumerator<KeyValuePair<TSecond, TFirst>> GetEnumerator() => owner._secondToFirst.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        IDictionaryEnumerator IDictionary.GetEnumerator() => owner.SecondToFirstId.GetEnumerator();

        public bool ContainsKey(TSecond key) => owner._secondToFirst.ContainsKey(key);
        bool IDictionary.Contains(object key) => owner.SecondToFirstId.Contains(key);
        bool ICollection<KeyValuePair<TSecond, TFirst>>.Contains(KeyValuePair<TSecond, TFirst> item) => owner._secondToFirst.Contains(item);

        void ICollection<KeyValuePair<TSecond, TFirst>>.CopyTo(KeyValuePair<TSecond, TFirst>[] array, int arrayIndex) => owner.SecondToFirstIcg.CopyTo(array, arrayIndex);
        void ICollection.CopyTo(Array array, int index) => owner.SecondToFirstId.CopyTo(array, index);

        public bool TryGetValue(TSecond key, out TFirst value) => owner._secondToFirst.TryGetValue(key, out value!);

        public TFirst this[TSecond key] {
            get => owner._secondToFirst[key];
            set {
                owner._secondToFirst[key] = value;
                owner._firstToSecond[value] = key;
            }
        }

        object? IDictionary.this[object key] {
            get => owner.SecondToFirstId[key];
            set {
                Guard.IsNotNull(value);
                owner.SecondToFirstId[key] = value;
                owner.FirstToSecondId[value] = key;
            }
        }

        public void Add(TSecond key, TFirst value)
        {
            owner._secondToFirst.Add(key, value);
            owner._firstToSecond.Add(value, key);
        }

        void IDictionary.Add(object key, object? value)
        {
            Guard.IsNotNull(value);
            owner.SecondToFirstId.Add(key, value);
            owner.FirstToSecondId.Add(value, key);
        }

        void ICollection<KeyValuePair<TSecond, TFirst>>.Add(KeyValuePair<TSecond, TFirst> item)
        {
            owner._secondToFirst.Add(item.Key, item.Value);
            owner._firstToSecond.Add(item.Value, item.Key);
        }

        public bool Remove(TSecond key)
        {
            if (!owner._secondToFirst.Remove(key, out var value))
                return false;
            owner._firstToSecond.Remove(value);
            return true;
        }

        void IDictionary.Remove(object key)
        {
            var firstToSecond = owner.SecondToFirstId;
            if (!firstToSecond.Contains(key))
                return;
            var value = firstToSecond[key];
            Guard.IsNotNull(value);
            firstToSecond.Remove(key);
            owner.FirstToSecondId.Remove(value);
        }

        bool ICollection<KeyValuePair<TSecond, TFirst>>.Remove(KeyValuePair<TSecond, TFirst> item)
        {
            if (!owner.SecondToFirstIcg.Remove(item))
                return false;
            owner.FirstToSecondIcg.Remove(item.Reverse());
            return true;
        }

        public void Clear()
        {
            owner._secondToFirst.Clear();
            owner._firstToSecond.Clear();
        }
    }
}