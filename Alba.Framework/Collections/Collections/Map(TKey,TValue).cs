using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Alba.Framework.Text;
using Alba.Framework.Sys;

namespace Alba.Framework.Collections
{
    /// <summary>
    /// Map is to <see cref="Dictionary{TKey,TValue}"/> what <see cref="Collection{T}"/> is to <see cref="List{T}"/>.
    /// Allows to override methods of Dictionary.
    /// </summary>
    [Serializable, ComVisible (false)]
    [DebuggerDisplay ("Count = {Count}"), DebuggerTypeProxy (typeof(DictionaryDebugView<,>))]
    public class Map<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
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

        public bool IsReadOnly
        {
            get { return _dictionary.IsReadOnly; }
        }

        public int Count
        {
            get { return _dictionary.Count; }
        }

        public TValue this [TKey key]
        {
            get
            {
                TValue value;
                if (!TryGetItemValue(key, out value))
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

        protected virtual void SetItem (TKey key, TValue value)
        {
            _dictionary[key] = value;
        }

        public void Add (KeyValuePair<TKey, TValue> item)
        {
            if (IsReadOnly)
                throw NewNotSupportedReadOnlyException();
            AddItem(item.Key, item.Value);
        }

        public void Add (TKey key, TValue value)
        {
            if (IsReadOnly)
                throw NewNotSupportedReadOnlyException();
            AddItem(key, value);
        }

        protected virtual void AddItem (TKey key, TValue value)
        {
            _dictionary.Add(key, value);
        }

        public bool Contains (KeyValuePair<TKey, TValue> item)
        {
            TValue value;
            return TryGetItemValue(item.Key, out value) && value.EqualsValue(item.Value);
        }

        public bool ContainsKey (TKey key)
        {
            TValue value;
            return TryGetItemValue(key, out value);
        }

        public bool TryGetValue (TKey key, out TValue value)
        {
            return TryGetItemValue(key, out value);
        }

        protected virtual bool TryGetItemValue (TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public bool Remove (KeyValuePair<TKey, TValue> item)
        {
            if (IsReadOnly)
                throw NewNotSupportedReadOnlyException();
            TValue value;
            if (TryGetItemValue(item.Key, out value) && value.EqualsValue(item.Value))
                RemoveItem(item.Key);
            return false;
        }

        public bool Remove (TKey key)
        {
            if (IsReadOnly)
                throw NewNotSupportedReadOnlyException();
            return RemoveItem(key);
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

        public ICollection<TValue> Values
        {
            get { return _dictionary.Values; }
        }

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        {
            get { return Values; }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator ()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return GetEnumerator();
        }

        public void CopyTo (KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _dictionary.CopyTo(array, arrayIndex);
        }

        private static Exception NewNotSupportedReadOnlyException ()
        {
            return new NotSupportedException("Dictionary is read-only.");
        }
    }
}