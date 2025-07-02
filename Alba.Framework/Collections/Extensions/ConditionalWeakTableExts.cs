using System.Runtime.CompilerServices;

namespace Alba.Framework.Collections;

[PublicAPI]
public static class ConditionalWeakTableExts
{
    public static TValue GetOrDefault<TKey, TValue>(this ConditionalWeakTable<TKey, TValue> @this,
        TKey key, TValue defaultValue = null!)
        where TKey : class where TValue : class? =>
        @this.TryGetValue(key, out var value) ? value : defaultValue;

    public static TValue GetOrDefault<TKey, TValue>(this ConditionalWeakTable<TKey, TValue> @this,
        TKey key, Func<TValue> getDefaultValue)
        where TKey : class where TValue : class? =>
        @this.TryGetValue(key, out var value) ? value : getDefaultValue();
}