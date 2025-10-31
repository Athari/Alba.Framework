using System.Collections;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Alba.Framework.Collections;

[PublicAPI]
public static class DictionaryExts
{
    private const string ErrorKeyNotFound = "Key '{0}' not found.";

    extension<TKey, TValue>(Dictionary<TKey, TValue> @this)
        where TKey : notnull
    {
        public ref TValue GetRefOrNull(TKey key) =>
            ref CollectionsMarshal.GetValueRefOrNullRef(@this, key);

        public ref TValue GetRefOrNull(TKey key, out bool exists)
        {
            ref var value = ref @this.GetRefOrNull(key);
            exists = Unsafe.IsNullRef(ref value);
            return ref value!;
        }

        public ref TValue? GetRefOrAdd(TKey key, out bool exists) =>
            ref CollectionsMarshal.GetValueRefOrAddDefault(@this, key, out exists);

        public ref TValue? GetRefOrAdd(TKey key) =>
            ref @this.GetRefOrAdd(key, out _);

        public TValue GetOrAdd(TKey key, TValue defaultValue, out bool exists)
        {
            ref var value = ref @this.GetRefOrAdd(key, out exists);
            if (!exists)
                value = defaultValue;
            return value!;
        }

      #if !HAS_DOTNEXT
        public TValue GetOrAdd(TKey key, TValue defaultValue) =>
            @this.GetOrAdd(key, defaultValue, out _);
      #endif

        public TValue? GetOrAddDefault(TKey key, out bool exists) =>
            @this.GetRefOrAdd(key, out exists);

        public TValue? GetOrAddDefault(TKey key) =>
            @this.GetOrAddDefault(key, out _);
    }

    extension<TKey, TValue>(IDictionary<TKey, TValue> @this)
        where TKey : notnull
    {
        public void AddRange([InstantHandle] IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            foreach (var kvp in items)
                @this.Add(kvp.Key, kvp.Value);
        }

        [OverloadResolutionPriority(-1)]
        public TValue? Get(TKey key) =>
            @this.TryGetValue(key, out var value) ? value : default;

        [OverloadResolutionPriority(-1)]
        public TValue Get(TKey key, TValue defaultValue) =>
            @this.TryGetValue(key, out var value) ? value : defaultValue;

        [OverloadResolutionPriority(-1)]
        public TValue Get(TKey key, Func<TValue> getDefaultValue) =>
            @this.TryGetValue(key, out var value) ? value : getDefaultValue();

        public TValue GetOrAdd(TKey key, Func<TValue> getDefaultValue)
        {
            if (!@this.TryGetValue(key, out var value))
                @this[key] = value = getDefaultValue();
            return value;
        }

        [OverloadResolutionPriority(-1)]
        public bool TryGet(TKey key, [MaybeNullWhen(false)] out TValue value) =>
            @this.TryGetValue(key, out value);
    }

    extension(IDictionary @this)
    {
        public IDictionary<TKey, TValue> AsTyped<TKey, TValue>()
            where TKey : notnull =>
            new TypedMap<TKey, TValue>(@this);
    }

    extension<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> @this)
        where TKey : notnull
    {
        public TValue? Get(TKey key) =>
            @this.GetValueOrDefault(key);

        public TValue Get(TKey key, TValue defaultValue) =>
            @this.GetValueOrDefault(key, defaultValue);

        public TValue Get(TKey key, Func<TValue> getDefaultValue) =>
            @this.TryGetValue(key, out var value) ? value : getDefaultValue();

        public bool TryGet(TKey key, [MaybeNullWhen(false)] out TValue value) =>
            @this.TryGetValue(key, out value);
    }

    extension<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> @this)
    {
        public IEnumerable<T> KeysOfType<T>() where T : TKey
        {
            foreach (var (key, _) in @this)
                if (key is T k)
                    yield return k;
        }

        public IEnumerable<KeyValuePair<T, TValue>> WithKeysOfType<T>() where T : TKey
        {
            foreach (var (key, value) in @this)
                if (key is T k)
                    yield return new(k, value);
        }

        public IEnumerable<T> ValuesOfType<T>() where T : TValue
        {
            foreach (var (_, value) in @this)
                if (value is T v)
                    yield return v;
        }

        public IEnumerable<KeyValuePair<TKey, T>> WithValuesOfType<T>() where T : TValue
        {
            foreach (var (key, value) in @this)
                if (value is T v)
                    yield return new(key, v);
        }
    }

    public static int GetAndIncrement<TKey>(this Dictionary<TKey, int> @this, TKey key, int startValue = 0)
        where TKey : notnull
    {
        ref var value = ref @this.GetRefOrAdd(key, out bool exists);
        if (exists)
            value++;
        else
            value = startValue;
        return value;
    }

    public static Dictionary<TValue, TKey> Invert<TKey, TValue>(this IDictionary<TKey, TValue> @this)
        where TValue : notnull =>
        @this.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

    public static ExpandoObject ToExpando<TValue>(this IDictionary<string, TValue> @this)
    {
        var expando = new ExpandoObject();
        IDictionary<string, object?> dic = expando;
        foreach (var kvp in @this)
            dic.Add(kvp.Key, kvp.Value!);
        return expando;
    }
}