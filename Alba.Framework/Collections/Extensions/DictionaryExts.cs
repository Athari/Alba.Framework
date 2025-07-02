using System.Dynamic;

namespace Alba.Framework.Collections;

[PublicAPI]
public static class DictionaryExts
{
    private const string ErrorKeyNotFound = "Key '{0}' not found.";

    public static void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> @this, IEnumerable<KeyValuePair<TKey, TValue>> items)
        where TKey : notnull
    {
        foreach (var kvp in items)
            @this.Add(kvp.Key, kvp.Value);
    }

    public static Dictionary<TValue, TKey> Invert<TKey, TValue>(this IDictionary<TKey, TValue> @this)
        where TValue : notnull =>
        @this.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

    public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, TValue defaultValue = default!) =>
        @this.TryGetValue(key, out var value) ? value : defaultValue;

    public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, Func<TValue> getDefaultValue) =>
        @this.TryGetValue(key, out var value) ? value : getDefaultValue();

    public static TValue? GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, TValue? defaultValue = null)
        where TValue : struct =>
        @this.TryGetValue(key, out TValue value) ? value : defaultValue;

    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, Func<TValue> getDefaultValue)
    {
        if (!@this.TryGetValue(key, out var value))
            @this[key] = value = getDefaultValue();
        return value;
    }

    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key)
        where TValue : new()
    {
        if (!@this.TryGetValue(key, out var value))
            @this[key] = value = new();
        return value;
    }

    public static int GetAndIncrement<TKey>(this IDictionary<TKey, int> @this, TKey key, int startValue = 0)
    {
        if (@this.TryGetValue(key, out int value))
            @this[key] = ++value;
        else
            @this[key] = value = startValue;
        return value;
    }

    public static ExpandoObject ToExpando<TValue>(this IDictionary<string, TValue> @this)
    {
        var expando = new ExpandoObject();
        var dic = expando as IDictionary<string, object>;
        foreach (var kvp in @this)
            dic.Add(kvp.Key, kvp.Value!);
        return expando;
    }
}