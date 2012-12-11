using System;
using System.Runtime.CompilerServices;

namespace Alba.Framework.Collections
{
    public static class ConditionalWeakTableExts
    {
        public static TValue GetOrDefault<TKey, TValue> (this ConditionalWeakTable<TKey, TValue> @this,
            TKey key, TValue defaultValue = default(TValue))
            where TKey : class where TValue : class
        {
            TValue value;
            return @this.TryGetValue(key, out value) ? value : defaultValue;
        }

        public static TValue GetOrDefault<TKey, TValue> (this ConditionalWeakTable<TKey, TValue> @this,
            TKey key, Func<TValue> getDefaultValue)
            where TKey : class where TValue : class
        {
            TValue value;
            return @this.TryGetValue(key, out value) ? value : getDefaultValue();
        }
    }
}