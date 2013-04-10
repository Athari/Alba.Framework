using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Alba.Framework.Collections
{
    internal class DictionaryDebugView<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _dictionary;

        [DebuggerBrowsable (DebuggerBrowsableState.RootHidden)]
        public KeyValuePair<TKey, TValue>[] Items
        {
            get
            {
                var array = new KeyValuePair<TKey, TValue>[_dictionary.Count];
                _dictionary.CopyTo(array, 0);
                return array;
            }
        }

        public DictionaryDebugView (IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");
            _dictionary = dictionary;
        }
    }
}