namespace Alba.Framework.Collections;

[PublicAPI]
public static class KeyValuePairExts
{
    extension<TValue, TKey>(KeyValuePair<TKey, TValue> @this)
    {
        public KeyValuePair<TValue, TKey> Reverse() => new(@this.Value, @this.Key);
    }
}