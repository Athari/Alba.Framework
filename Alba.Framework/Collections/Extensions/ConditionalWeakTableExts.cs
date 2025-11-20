using System.Runtime.CompilerServices;

namespace Alba.Framework.Collections;

public static class ConditionalWeakTableExts
{
    extension<TKey, TValue>(ConditionalWeakTable<TKey, TValue> @this)
        where TKey : class
        where TValue : class
    {
        public TValue? GetOrDefault(TKey key) =>
            @this.TryGetValue(key, out var value) ? value : null;

        public TValue GetOrDefault(TKey key, TValue defaultValue) =>
            @this.TryGetValue(key, out var value) ? value : defaultValue;

        public TValue GetOrDefault(TKey key, Func<TValue> getDefaultValue) =>
            @this.TryGetValue(key, out var value) ? value : getDefaultValue();
    }
}