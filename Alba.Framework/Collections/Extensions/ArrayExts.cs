namespace Alba.Framework.Collections;

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

    extension<T>(T[]? @this)
    {
        public RefEquatableArray<T> AsRefEquatable() =>
            @this != null ? new(@this) : RefEquatableArray<T>.Empty;
    }

    extension<T>(T[]? @this) where T : IEquatable<T>
    {
        public EquatableArray<T> AsEquatable() =>
            @this != null ? new(@this) : EquatableArray<T>.Empty;
    }
}