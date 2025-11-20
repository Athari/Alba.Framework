namespace Alba.Framework;

public static class DelegateExts
{
    public static Func<T1> Merge<T1>(this Func<T1>? @this, Func<T1>? other, Func<T1, T1, T1> merge) =>
        (@this, other) switch {
            (null, null) => throw new ArgumentNullException(nameof(@this)),
            (null, _) => other,
            (_, null) => @this,
            (_, _) => () => merge(@this(), other()),
        };

    public static Func<T1, T2> Merge<T1, T2>(this Func<T1, T2>? @this, Func<T1, T2>? other, Func<T2, T2, T2> merge) =>
        (@this, other) switch {
            (null, null) => throw new ArgumentNullException(nameof(@this)),
            (null, _) => other,
            (_, null) => @this,
            (_, _) => t1 => merge(@this(t1), other(t1)),
        };

    public static Func<T1, T2, T3> Merge<T1, T2, T3>(this Func<T1, T2, T3>? @this, Func<T1, T2, T3>? other, Func<T3, T3, T3> merge) =>
        (@this, other) switch {
            (null, null) => throw new ArgumentNullException(nameof(@this)),
            (null, _) => other,
            (_, null) => @this,
            (_, _) => (t1, t2) => merge(@this(t1, t2), other(t1, t2)),
        };

    extension<T>(Predicate<T>? @this)
    {
        public Predicate<T> Merge(Predicate<T>? other, Func<bool, bool, bool> merge) =>
            (@this, other) switch {
                (null, null) => throw new ArgumentNullException(nameof(@this)),
                (null, _) => other,
                (_, null) => @this,
                (_, _) => t1 => merge(@this(t1), other(t1)),
            };

        public Predicate<T> MergeAnd(Predicate<T>? other) =>
            Merge(@this, other, (a, b) => a && b);

        public Predicate<T> MergeOr(Predicate<T>? other) =>
            Merge(@this, other, (a, b) => a || b);
    }
}