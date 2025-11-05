namespace Alba.Framework.Collections;

[PublicAPI]
public static class ArrayExts
{
    extension<T>(T[] @this)
    {
        public void Fill(T value) =>
            Array.Fill(@this, value);

        public void Fill(T value, int startIndex, int count) =>
            Array.Fill(@this, value, startIndex, count);

        public T[] ReverseArray()
        {
            Array.Reverse(@this);
            return @this;
        }
    }

    public static EquatableArray<T> AsEquatable<T>(this T[]? @this) where T : IEquatable<T> =>
        @this != null ? new(@this) : EquatableArray<T>.Empty;

    public static RefEquatableArray<T> AsRefEquatable<T>(this T[]? @this) =>
        @this != null ? new(@this) : RefEquatableArray<T>.Empty;
}