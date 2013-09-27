using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Alba.Framework.Sys;

// ReSharper disable LoopCanBeConvertedToQuery
namespace Alba.Framework.Collections
{
    [Serializable]
    [DebuggerDisplay ("Count = {Count}"), DebuggerTypeProxy (typeof(DictionaryDebugView<,>))]
    public abstract class IndexDictionaryBase<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, IDictionary
    {
        private readonly List<TValue> _values;

        protected IndexDictionaryBase ()
        {
            _values = new List<TValue>();
        }

        protected IndexDictionaryBase (int capacity)
        {
            _values = new List<TValue>(capacity);
        }

        public int Count
        {
            get { return _values.Count; }
        }

        object ICollection.SyncRoot
        {
            get { return ((ICollection)_values).SyncRoot; }
        }

        bool ICollection.IsSynchronized
        {
            get { return ((ICollection)_values).IsSynchronized; }
        }

        bool IDictionary.IsFixedSize
        {
            get { return false; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public TValue this [TKey key]
        {
            get
            {
                int index = KeyToIndex(key);
                return HasIndex(index) ? _values[index] : default(TValue);
            }
            set
            {
                int index = KeyToIndex(key);
                if (index >= _values.Capacity * 2)
                    _values.Capacity = index;
                while (_values.Count <= index)
                    _values.Add(default(TValue));
                _values[index] = value;
            }
        }

        object IDictionary.this [object key]
        {
            get { return this[(TKey)key]; }
            set { this[(TKey)key] = (TValue)value; }
        }

        public ICollection<TKey> Keys
        {
            get { return new KeyCollection(this); }
        }

        ICollection IDictionary.Keys
        {
            get { return new KeyCollection(this); }
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
        {
            get { return Keys; }
        }

        public ICollection<TValue> Values
        {
            get { return new ValueCollection(this); }
        }

        ICollection IDictionary.Values
        {
            get { return new ValueCollection(this); }
        }

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        {
            get { return Values; }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator ()
        {
            for (int i = 0; i < _values.Count; i++)
                yield return new KeyValuePair<TKey, TValue>(IndexToKey(i), _values[i]);
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return GetEnumerator();
        }

        IDictionaryEnumerator IDictionary.GetEnumerator ()
        {
            throw new NotImplementedException();
        }

        public void Add (TKey key, TValue value)
        {
            if (!this[key].EqualsValue(default(TValue)))
                throw new ArgumentException("An item with the same key has already been added.");
            this[key] = value;
        }

        void IDictionary.Add (object key, object value)
        {
            Add((TKey)key, (TValue)value);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add (KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public bool ContainsKey (TKey key)
        {
            int index = KeyToIndex(key);
            return HasIndex(index);
        }

        bool IDictionary.Contains (object key)
        {
            return ContainsKey((TKey)key);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains (KeyValuePair<TKey, TValue> item)
        {
            int index = KeyToIndex(item.Key);
            return HasIndex(index) && _values[index].EqualsValue(item.Value);
        }

        public bool TryGetValue (TKey key, out TValue value)
        {
            int index = KeyToIndex(key);
            if (HasIndex(index)) {
                value = _values[index];
                return true;
            }
            else {
                value = default(TValue);
                return false;
            }
        }

        public bool Remove (TKey key)
        {
            int index = KeyToIndex(key);
            if (!HasIndex(index))
                return true;
            if (_values[index].EqualsValue(default(TValue)))
                return false;
            _values[index] = default(TValue);
            return true;
        }

        void IDictionary.Remove (object key)
        {
            Remove((TKey)key);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove (KeyValuePair<TKey, TValue> item)
        {
            int index = KeyToIndex(item.Key);
            if (!HasIndex(index) || !_values[index].EqualsValue(item.Value))
                return false;
            _values[index] = default(TValue);
            return true;
        }

        public void Clear ()
        {
            _values.Clear();
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo (KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection)this).CopyTo(array, arrayIndex);
        }

        void ICollection.CopyTo (Array array, int arrayIndex)
        {
            // from SortedList<TKey, TValue>
            if (array == null || array.Rank != 1 || array.GetLowerBound(0) != 0)
                throw new ArgumentException("Wrong array.", "array");
            if (arrayIndex < 0 || arrayIndex > array.Length || array.Length - arrayIndex < Count)
                throw new ArgumentException("Wrong arrayIndex.", "array");

            var pairArray = array as KeyValuePair<TKey, TValue>[];
            if (pairArray != null) {
                for (int i = 0; i < _values.Count; i++)
                    pairArray[i + arrayIndex] = new KeyValuePair<TKey, TValue>(IndexToKey(i), _values[i]);
            }
            else {
                var objArray = array as object[];
                if (objArray == null)
                    throw new ArgumentException("Wrong array.", "array");
                try {
                    for (int i = 0; i < _values.Count; i++)
                        objArray[i + arrayIndex] = new KeyValuePair<TKey, TValue>(IndexToKey(i), _values[i]);
                }
                catch (ArrayTypeMismatchException) {
                    throw new ArgumentException("Wrong array.", "array");
                }
            }
        }

        protected abstract int KeyToIndex (TKey key);

        protected abstract TKey IndexToKey (int index);

        private bool HasIndex (int index)
        {
            return 0 <= index && index < _values.Count;
        }

        private static Exception GetReadOnlyException ()
        {
            return new NotSupportedException("Collection is read-only.");
        }

        private class KeyCollection : ICollection<TKey>, ICollection
        {
            private readonly IndexDictionaryBase<TKey, TValue> _dictionary;

            public KeyCollection (IndexDictionaryBase<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
            }

            bool ICollection.IsSynchronized
            {
                get { return ((ICollection)_dictionary).IsSynchronized; }
            }

            public bool IsReadOnly
            {
                get { return true; }
            }

            public int Count
            {
                get { return _dictionary.Count; }
            }

            object ICollection.SyncRoot
            {
                get { return ((ICollection)_dictionary).SyncRoot; }
            }

            public IEnumerator<TKey> GetEnumerator ()
            {
                for (int i = 0; i < _dictionary._values.Count; i++)
                    yield return _dictionary.IndexToKey(i);
            }

            IEnumerator IEnumerable.GetEnumerator ()
            {
                return GetEnumerator();
            }

            void ICollection<TKey>.Add (TKey item)
            {
                throw GetReadOnlyException();
            }

            void ICollection<TKey>.Clear ()
            {
                throw GetReadOnlyException();
            }

            public bool Contains (TKey item)
            {
                return _dictionary.ContainsKey(item);
            }

            void ICollection<TKey>.CopyTo (TKey[] array, int arrayIndex)
            {
                for (int i = 0; i < _dictionary._values.Count; i++)
                    array[i + arrayIndex] = _dictionary.IndexToKey(i);
            }

            void ICollection.CopyTo (Array array, int arrayIndex)
            {
                for (int i = 0; i < _dictionary._values.Count; i++)
                    array.SetValue(_dictionary.IndexToKey(i), arrayIndex + i);
            }

            bool ICollection<TKey>.Remove (TKey item)
            {
                throw GetReadOnlyException();
            }
        }

        private class ValueCollection : ICollection<TValue>, ICollection
        {
            private readonly IndexDictionaryBase<TKey, TValue> _dictionary;

            public ValueCollection (IndexDictionaryBase<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
            }

            bool ICollection.IsSynchronized
            {
                get { return ((ICollection)_dictionary).IsSynchronized; }
            }

            public bool IsReadOnly
            {
                get { return true; }
            }

            public int Count
            {
                get { return _dictionary.Count; }
            }

            object ICollection.SyncRoot
            {
                get { return ((ICollection)_dictionary).SyncRoot; }
            }

            public IEnumerator<TValue> GetEnumerator ()
            {
                return _dictionary._values.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator ()
            {
                return GetEnumerator();
            }

            void ICollection<TValue>.Add (TValue item)
            {
                throw GetReadOnlyException();
            }

            void ICollection<TValue>.Clear ()
            {
                throw GetReadOnlyException();
            }

            public bool Contains (TValue item)
            {
                return _dictionary._values.Contains(item);
            }

            void ICollection<TValue>.CopyTo (TValue[] array, int arrayIndex)
            {
                for (int i = 0; i < _dictionary._values.Count; i++)
                    array[i + arrayIndex] = _dictionary._values[i];
            }

            void ICollection.CopyTo (Array array, int arrayIndex)
            {
                for (int i = 0; i < _dictionary._values.Count; i++)
                    array.SetValue(_dictionary.IndexToKey(i), arrayIndex + i);
            }

            bool ICollection<TValue>.Remove (TValue item)
            {
                throw GetReadOnlyException();
            }
        }
    }
}