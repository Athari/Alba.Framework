using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Alba.Framework.Attributes;
using Alba.Framework.Sys;
using Alba.Framework.Text;

// ReSharper disable LoopCanBeConvertedToQuery
namespace Alba.Framework.Collections
{
    [Serializable, ComVisible (false)]
    [DebuggerDisplay ("Count = {Count}"), DebuggerTypeProxy (typeof(DictionaryDebugView<,>))]
    public class ListDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, IDictionary
    {
        private readonly List<KeyValuePair<TKey, TValue>> _list;
        private readonly IEqualityComparer<TKey> _comparer;
        [NonSerialized]
        private KeyCollection _keys;
        [NonSerialized]
        private ValueCollection _values;

        public ListDictionary (int capacity = 0, IEqualityComparer<TKey> comparer = null)
        {
            _list = new List<KeyValuePair<TKey, TValue>>(capacity);
            _comparer = comparer ?? EqualityComparer<TKey>.Default;
        }

        public ListDictionary (IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer = null)
        {
            _list = new List<KeyValuePair<TKey, TValue>>(collection);
            _comparer = comparer ?? EqualityComparer<TKey>.Default;
        }

        bool IDictionary.IsFixedSize
        {
            get { return ((IList)_list).IsFixedSize; }
        }

        public bool IsReadOnly
        {
            get { return ((ICollection<KeyValuePair<TKey, TValue>>)_list).IsReadOnly; }
        }

        bool ICollection.IsSynchronized
        {
            get { return ((ICollection)_list).IsSynchronized; }
        }

        public int Count
        {
            get { return _list.Count; }
        }

        object ICollection.SyncRoot
        {
            get { return ((ICollection)_list).SyncRoot; }
        }

        public TValue this [[CanBeNull] TKey key]
        {
            get
            {
                TValue value;
                if (TryGetValue(key, out value))
                    return value;
                throw new KeyNotFoundException("Key '{0}' not found.".FmtInv(key));
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

        object IDictionary.this [[CanBeNull] object key]
        {
            get { return this[(TKey)key]; }
            set { this[(TKey)key] = (TValue)value; }
        }

        public void Add (TKey key, TValue value)
        {
            TValue oldValue;
            if (TryGetValue(key, out oldValue))
                throw new ArgumentException("Key '{0}' already exists.".FmtInv(key));
            _list.Add(new KeyValuePair<TKey, TValue>(key, oldValue));
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add (KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        void IDictionary.Add ([CanBeNull] object key, object value)
        {
            Add((TKey)key, (TValue)value);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains (KeyValuePair<TKey, TValue> item)
        {
            TValue value;
            return TryGetValue(item.Key, out value) && value.EqualsValue(item.Value);
        }

        public bool ContainsKey ([CanBeNull] TKey key)
        {
            TValue value;
            return TryGetValue(key, out value);
        }

        bool IDictionary.Contains ([CanBeNull] object key)
        {
            return ContainsKey((TKey)key);
        }

        public bool TryGetValue ([CanBeNull] TKey key, out TValue value)
        {
            foreach (KeyValuePair<TKey, TValue> pair in _list) {
                if (_comparer.Equals(pair.Key, key)) {
                    value = pair.Value;
                    return true;
                }
            }
            value = default(TValue);
            return false;
        }

        public bool Remove (TKey key)
        {
            for (int i = 0; i < _list.Count; i++) {
                if (_comparer.Equals(_list[i].Key, key)) {
                    _list.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove (KeyValuePair<TKey, TValue> item)
        {
            TValue value;
            if (TryGetValue(item.Key, out value) && value.EqualsValue(item.Value))
                return Remove(item.Key);
            return false;
        }

        void IDictionary.Remove (object key)
        {
            Remove((TKey)key);
        }

        public void Clear ()
        {
            _list.Clear();
        }

        public KeyCollection Keys
        {
            get { return _keys ?? (_keys = new KeyCollection(this)); }
        }

        ICollection<TKey> IDictionary<TKey, TValue>.Keys
        {
            get { return Keys; }
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
        {
            get { return Keys; }
        }

        ICollection IDictionary.Keys
        {
            get { return Keys; }
        }

        public ValueCollection Values
        {
            get { return _values ?? (_values = new ValueCollection(this)); }
        }

        ICollection<TValue> IDictionary<TKey, TValue>.Values
        {
            get { return Values; }
        }

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        {
            get { return Values; }
        }

        ICollection IDictionary.Values
        {
            get { return Values; }
        }

        public List<KeyValuePair<TKey, TValue>>.Enumerator GetEnumerator ()
        {
            return _list.GetEnumerator();
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator ()
        {
            return GetEnumerator();
        }

        IDictionaryEnumerator IDictionary.GetEnumerator ()
        {
            return new DictionaryEnumerator(GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return GetEnumerator();
        }

        public void CopyTo (KeyValuePair<TKey, TValue>[] array, int index)
        {
            _list.CopyTo(array, index);
        }

        void ICollection.CopyTo (Array array, int index)
        {
            ((ICollection)_list).CopyTo(array, index);
        }

        [Serializable]
        [DebuggerDisplay ("Count = {Count}"), DebuggerTypeProxy (typeof(CollectionDebugView<>))]
        public sealed class KeyCollection : ICollection<TKey>, ICollection
        {
            private readonly ListDictionary<TKey, TValue> _owner;

            internal KeyCollection (ListDictionary<TKey, TValue> owner)
            {
                _owner = owner;
            }

            public int Count
            {
                get { return _owner.Count; }
            }

            public bool IsReadOnly
            {
                get { return true; }
            }

            bool ICollection.IsSynchronized
            {
                get { return ((ICollection)_owner).IsSynchronized; }
            }

            object ICollection.SyncRoot
            {
                get { return ((ICollection)_owner).SyncRoot; }
            }

            public IEnumerator<TKey> GetEnumerator ()
            {
                foreach (KeyValuePair<TKey, TValue> pair in _owner._list)
                    yield return pair.Key;
            }

            IEnumerator IEnumerable.GetEnumerator ()
            {
                return GetEnumerator();
            }

            void ICollection<TKey>.Add (TKey item)
            {
                throw NewCollectionReadOnlyException();
            }

            void ICollection<TKey>.Clear ()
            {
                throw NewCollectionReadOnlyException();
            }

            public bool Contains (TKey item)
            {
                foreach (KeyValuePair<TKey, TValue> pair in _owner._list)
                    if (_owner._comparer.Equals(pair.Key, item))
                        return true;
                return false;
            }

            public void CopyTo (TKey[] array, int index)
            {
                for (int i = 0; i < _owner._list.Count; i++)
                    array[index + i] = _owner._list[i].Key;
            }

            void ICollection.CopyTo (Array array, int index)
            {
                for (int i = 0; i < _owner._list.Count; i++)
                    array.SetValue(_owner._list[i].Key, index + i);
            }

            bool ICollection<TKey>.Remove (TKey item)
            {
                throw NewCollectionReadOnlyException();
            }
        }

        [Serializable]
        [DebuggerDisplay ("Count = {Count}"), DebuggerTypeProxy (typeof(CollectionDebugView<>))]
        public sealed class ValueCollection : ICollection<TValue>, ICollection
        {
            private readonly ListDictionary<TKey, TValue> _owner;

            internal ValueCollection (ListDictionary<TKey, TValue> owner)
            {
                _owner = owner;
            }

            public int Count
            {
                get { return _owner.Count; }
            }

            public bool IsReadOnly
            {
                get { return true; }
            }

            bool ICollection.IsSynchronized
            {
                get { return ((ICollection)_owner).IsSynchronized; }
            }

            object ICollection.SyncRoot
            {
                get { return ((ICollection)_owner).SyncRoot; }
            }

            public IEnumerator<TValue> GetEnumerator ()
            {
                foreach (KeyValuePair<TKey, TValue> pair in _owner._list)
                    yield return pair.Value;
            }

            IEnumerator IEnumerable.GetEnumerator ()
            {
                return GetEnumerator();
            }

            void ICollection<TValue>.Add (TValue item)
            {
                throw NewCollectionReadOnlyException();
            }

            void ICollection<TValue>.Clear ()
            {
                throw NewCollectionReadOnlyException();
            }

            public bool Contains (TValue item)
            {
                foreach (KeyValuePair<TKey, TValue> pair in _owner._list)
                    if (pair.Value.EqualsValue(item))
                        return true;
                return false;
            }

            public void CopyTo (TValue[] array, int index)
            {
                for (int i = 0; i < _owner._list.Count; i++)
                    array[index + i] = _owner._list[i].Value;
            }

            void ICollection.CopyTo (Array array, int index)
            {
                for (int i = 0; i < _owner._list.Count; i++)
                    array.SetValue(_owner._list[i].Value, index + i);
            }

            bool ICollection<TValue>.Remove (TValue item)
            {
                throw NewCollectionReadOnlyException();
            }
        }

        private class DictionaryEnumerator : IDictionaryEnumerator
        {
            private List<KeyValuePair<TKey, TValue>>.Enumerator _enumerator;

            public DictionaryEnumerator (List<KeyValuePair<TKey, TValue>>.Enumerator enumerator)
            {
                _enumerator = enumerator;
            }

            public bool MoveNext ()
            {
                return _enumerator.MoveNext();
            }

            public void Reset ()
            {
                ((IEnumerator)_enumerator).Reset();
            }

            public object Current
            {
                get { return _enumerator.Current; }
            }

            public object Key
            {
                get { return _enumerator.Current.Key; }
            }

            public object Value
            {
                get { return _enumerator.Current.Value; }
            }

            public DictionaryEntry Entry
            {
                get { return new DictionaryEntry(Key, Value); }
            }
        }

        private static Exception NewCollectionReadOnlyException ()
        {
            return new NotSupportedException("Collection is read-only.");
        }
    }
}