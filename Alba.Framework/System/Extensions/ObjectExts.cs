namespace Alba.Framework;

public static class ObjectExts
{
    extension(object @this)
    {
        [Pure]
        public T To<T>() => (T)@this;
    }

    extension(object? @this)
    {
        [Pure]
        public bool IsAnyType(params IEnumerable<Type> types) =>
            @this != null && types.Any(@this.GetType().IsAssignableTo);
    }

    extension<T>(T @this)
    {
        [Pure]
        public bool Equals(object? obj, Func<T, bool> equals)
        {
            if (obj is null) return false;
            if (ReferenceEquals(obj, @this)) return true;
            return obj.GetType() == @this?.GetType() && equals((T)obj);
        }

        //public T CloneSerializable()
        //{
        //    if (!typeof(T).IsSerializable)
        //        throw new ArgumentException("Type '{0}' is not serializable.".Fmt(typeof(T).GetFullName()), "this");
        //    if (ReferenceEquals(@this, null))
        //        return default;

        //    var formatter = new BinaryFormatter();
        //    using var stream = new MemoryStream();
        //    formatter.Serialize(stream, @this);
        //    stream.Position = 0;
        //    return (T)formatter.Deserialize(stream);
        //}

        [Pure]
        public bool EqualsAny(params IEnumerable<T> values) => values.Any(v => @this.EqualsValue(v));

        [Pure]
        public bool EqualsValue(T value) => EqualityComparer<T>.Default.Equals(@this, value);

        public T As(out T var) => var = @this;
    }

    [Pure]
    public static bool Compare<T>(T x, T y, Func<bool> compare)
    {
        if (ReferenceEquals(x, y))
            return true;
        if (x is null || y is null || x.GetType() != y.GetType())
            return false;
        return compare();
    }
}