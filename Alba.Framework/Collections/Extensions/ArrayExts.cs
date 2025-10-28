namespace Alba.Framework.Collections;

public static class EquatableArrayExts
{
    public static EquatableArray<T> AsEquatable<T>(this T[]? @this) where T : IEquatable<T> =>
        @this != null ? new(@this) : EquatableArray<T>.Empty;

    public static RefEquatableArray<T> AsRefEquatable<T>(this T[]? @this) =>
        @this != null ? new(@this) : RefEquatableArray<T>.Empty;
}