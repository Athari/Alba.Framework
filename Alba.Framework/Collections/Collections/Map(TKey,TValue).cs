using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Alba.Framework.Sys;
using Alba.Framework.Text;

// ReSharper disable CompareNonConstrainedGenericWithNull
namespace Alba.Framework.Collections
{
    /// <summary>
    /// Map is to <see cref="Dictionary{TKey,TValue}"/> what <see cref="Collection{T}"/> is to <see cref="List{T}"/>.
    /// Allows to override methods of Dictionary.
    /// Some members of non-generic interface are only supported if the underlying dictionary supports <see cref="IDictionary"/> interface.
    /// </summary>
    [Serializable, ComVisible (false)]
    [DebuggerDisplay ("Count = {Count}"), DebuggerTypeProxy (typeof(DictionaryDebugView<,>))]
    public class Map<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, IDictionary
    {
        private readonly IDictionary<TKey, TValue> _dictionary;

        public Map ()
        {
            _dictionary = new Dictionary<TKey, TValue>();
        }

        public Map (IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");
            _dictionary = dictionary;
        }

        bool IDictionary.IsFixedSize
        {
            get { return ((IDictionary)_dictionary).IsFixedSize; }
        }

        public bool IsReadOnly
        {
            get { return _dictionary.IsReadOnly; }
        }

        bool ICollection.IsSynchronized
        {
            get { return ((ICollection)_dictionary).IsSynchronized; }
        }

        public int Count
        {
            get { return _dictionary.Count; }
        }

        object ICollection.SyncRoot
        {
            get { return ((ICollection)_dictionary).SyncRoot; }
        }

        public TValue this [TKey key]
        {
            get
            {
                TValue value;
                if (!TryGetItem(key, out value))
                    throw new KeyNotFoundException("Key '{0}' not found.".Fmt(key));
                return value;
            }
            set
            {
                if (IsReadOnly)
                    throw NewNotSupportedReadOnlyException();
                SetItem(key, value);
            }
        }

        object IDictionary.this [object key]
        {
            get
            {
                if (key == null)
                    throw new ArgumentNullException("key");
                if (!(key is TKey))
                    return null;
                TValue value;
                return TryGetItem((TKey)key, out value) ? value : (object)null;
            }
            set
            {
                if (IsReadOnly)
                    throw NewNotSupportedReadOnlyException();
                if (key == null)
                    throw new ArgumentNullException("key");
                if (!(key is TKey))
                    throw new ArgumentException("Wrong key type.", "key");
                if (value == null && default(TValue) != null)
                    throw new ArgumentNullException("value");
                if (!(value is TValue))
                    throw new ArgumentException("Wrong value type.", "value");
                SetItem((TKey)key, (TValue)value);
            }
        }

        protected virtual void SetItem (TKey key, TValue value)
        {
            _dictionary[key] = value;
        }

        public void Add (TKey key, TValue value)
        {
            if (IsReadOnly)
                throw NewNotSupportedReadOnlyException();
            AddItem(key, value);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add (KeyValuePair<TKey, TValue> item)
        {
            if (IsReadOnly)
                throw NewNotSupportedReadOnlyException();
            AddItem(item.Key, item.Value);
        }

        void IDictionary.Add (object key, object value)
        {
            // The value "{0}" is not of type "{1}" and cannot be used in this generic collection.
            if (IsReadOnly)
                throw NewNotSupportedReadOnlyException();
            if (key == null)
                throw new ArgumentNullException("key");
            if (!(key is TKey))
                throw new ArgumentException("Wrong key type.", "key");
            if (value == null && default(TValue) != null)
                throw new ArgumentNullException("value");
            if (!(value is TValue))
                throw new ArgumentException("Wrong value type.", "value");
            AddItem((TKey)key, (TValue)value);
        }

        protected virtual void AddItem (TKey key, TValue value)
        {
            _dictionary.Add(key, value);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains (KeyValuePair<TKey, TValue> item)
        {
            TValue value;
            return TryGetItem(item.Key, out value) && value.EqualsValue(item.Value);
        }

        public bool ContainsKey (TKey key)
        {
            TValue value;
            return TryGetItem(key, out value);
        }

        bool IDictionary.Contains (object key)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            return key is TKey && ContainsKey((TKey)key);
        }

        public bool TryGetValue (TKey key, out TValue value)
        {
            return TryGetItem(key, out value);
        }

        protected virtual bool TryGetItem (TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public bool Remove (TKey key)
        {
            if (IsReadOnly)
                throw NewNotSupportedReadOnlyException();
            return RemoveItem(key);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove (KeyValuePair<TKey, TValue> item)
        {
            if (IsReadOnly)
                throw NewNotSupportedReadOnlyException();
            TValue value;
            if (TryGetItem(item.Key, out value) && value.EqualsValue(item.Value))
                RemoveItem(item.Key);
            return false;
        }

        void IDictionary.Remove (object key)
        {
            if (IsReadOnly)
                throw NewNotSupportedReadOnlyException();
            if (key == null)
                throw new ArgumentNullException("key");
            if (!(key is TKey))
                return;
            RemoveItem((TKey)key);
        }

        protected virtual bool RemoveItem (TKey key)
        {
            return _dictionary.Remove(key);
        }

        public void Clear ()
        {
            if (IsReadOnly)
                throw NewNotSupportedReadOnlyException();
            ClearItems();
        }

        protected virtual void ClearItems ()
        {
            _dictionary.Clear();
        }

        public ICollection<TKey> Keys
        {
            get { return _dictionary.Keys; }
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
        {
            get { return Keys; }
        }

        ICollection IDictionary.Keys
        {
            get { return ((IDictionary)_dictionary).Keys; }
        }

        public ICollection<TValue> Values
        {
            get { return _dictionary.Values; }
        }

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        {
            get { return Values; }
        }

        ICollection IDictionary.Values
        {
            get { return ((IDictionary)_dictionary).Values; }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator ()
        {
            return _dictionary.GetEnumerator();
        }

        IDictionaryEnumerator IDictionary.GetEnumerator ()
        {
            return ((IDictionary)_dictionary).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return ((IEnumerable)_dictionary).GetEnumerator();
        }

        public void CopyTo (KeyValuePair<TKey, TValue>[] array, int index)
        {
            _dictionary.CopyTo(array, index);
        }

        void ICollection.CopyTo (Array array, int index)
        {
            ((ICollection)_dictionary).CopyTo(array, index);
        }

        private static Exception NewNotSupportedReadOnlyException ()
        {
            return new NotSupportedException("Dictionary is read-only.");
        }
    }
}