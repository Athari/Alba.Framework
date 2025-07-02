namespace Alba.Framework;

[PublicAPI]
public static class ObjectExts
{
    [Pure]
    public static bool Compare<T>(T x, T y, Func<bool> compare)
    {
        if (ReferenceEquals(x, y))
            return true;
        if (x is null || y is null || x.GetType() != y.GetType())
            return false;
        return compare();
    }

    [Pure]
    public static bool Equals<T>(this T @this, object? obj, Func<T, bool> equals)
    {
        if (obj is null) return false;
        if (ReferenceEquals(obj, @this)) return true;
        return obj.GetType() == @this?.GetType() && equals((T)obj);
    }

    //public static T CloneSerializable<T> (this T @this)
    //{
    //    if (!typeof(T).IsSerializable)
    //        throw new ArgumentException("Type '{0}' is not serializable.".Fmt(typeof(T).GetFullName()), "this");
    //    if (ReferenceEquals(@this, null))
    //        return default(T);

    //    var formatter = new BinaryFormatter();
    //    using (var stream = new MemoryStream()) {
    //        formatter.Serialize(stream, @this);
    //        stream.Position = 0;
    //        return (T)formatter.Deserialize(stream);
    //    }
    //}

    [Pure]
    public static bool EqualsAny<T>(this T @this, params T[] values)
    {
        return values.Any(v => @this.EqualsValue(v));
    }

    [Pure]
    public static bool EqualsValue<T>(this T @this, T value)
    {
        return EqualityComparer<T>.Default.Equals(@this, value);
    }

    [Pure]
    public static string GetTypeFullName(this object? @this)
    {
        return @this == null ? "null" : @this.GetType().FullName!;
    }

    [Pure]
    public static T To<T>(this object @this)
    {
        return (T)@this;
    }

    [Pure]
    public static bool IsAnyType(this object? @this, params Type[] types)
    {
        return @this == null || types.Any(@this.GetType().IsAssignableTo);
    }

    [Pure]
    public static bool IsAnyType<T1>(this object @this)
    {
        return @this is T1;
    }

    [Pure]
    public static bool IsAnyType<T1, T2>(this object @this)
    {
        return @this is T1 || @this is T2;
    }
}