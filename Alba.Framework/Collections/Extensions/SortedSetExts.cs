namespace Alba.Framework.Collections;

public static class SortedSetExts
{
    extension<T>(SortedSet<T> @this)
    {
        public T? GetOrDefault(T value) =>
            @this.TryGetValue(value, out var v) ? v : default;
    }

    extension<T>(SortedSet<T> @this) where T : struct
    {
        public T? GetOrNull(T value) =>
            @this.TryGetValue(value, out var v) ? v : null;
    }
}