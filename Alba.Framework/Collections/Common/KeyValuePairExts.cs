namespace Alba.Framework.Collections;

[PublicAPI]
public static class KeyValuePairExts
{
    public static KeyValuePair<TValue, TKey> Reverse<TKey, TValue>(this KeyValuePair<TKey, TValue> @this) =>
        new(@this.Value, @this.Key);
}