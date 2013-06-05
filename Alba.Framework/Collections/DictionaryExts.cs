using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Alba.Framework.Collections
{
    public static class DictionaryExts
    {
        public static void AddRange<TKey, TValue> (this Dictionary<TKey, TValue> @this,
            IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            foreach (var kvp in items)
                @this.Add(kvp.Key, kvp.Value);
        }

        public static Dictionary<TValue, TKey> Invert<TKey, TValue> (this IDictionary<TKey, TValue> @this)
        {
            return @this.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        }

        public static TValue GetOrDefault<TKey, TValue> (this IDictionary<TKey, TValue> @this,
            TKey key, TValue defaultValue = default(TValue))
        {
            TValue value;
            return @this.TryGetValue(key, out value) ? value : defaultValue;
        }

        public static TValue GetOrDefault<TKey, TValue> (this IDictionary<TKey, TValue> @this,
            TKey key, Func<TValue> getDefaultValue)
        {
            TValue value;
            return @this.TryGetValue(key, out value) ? value : getDefaultValue();
        }

        public static TValue? GetOrDefault<TKey, TValue> (this IDictionary<TKey, TValue> @this,
            TKey key, TValue? defaultValue = null) where TValue : struct
        {
            TValue value;
            return @this.TryGetValue(key, out value) ? value : defaultValue;
        }

        public static TValue GetOrAdd<TKey, TValue> (this IDictionary<TKey, TValue> @this,
            TKey key, Func<TValue> getDefaultValue)
        {
            TValue value;
            if (!@this.TryGetValue(key, out value))
                @this[key] = value = getDefaultValue();
            return value;
        }

        public static TValue GetOrAdd<TKey, TValue> (this IDictionary<TKey, TValue> @this, TKey key)
            where TValue : new()
        {
            TValue value;
            if (!@this.TryGetValue(key, out value))
                @this[key] = value = new TValue();
            return value;
        }

        public static ExpandoObject ToExpando<TValue> (this IDictionary<string, TValue> @this)
        {
            var expando = new ExpandoObject();
            var dic = expando as IDictionary<string, object>;
            foreach (KeyValuePair<string, TValue> kvp in @this)
                dic.Add(kvp.Key, kvp.Value);
            return expando;
        }
    }
}