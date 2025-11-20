namespace Alba.Framework.Numerics;

public class HalfEpsilonOperations : IFloatingPointEpsilonOperations<Half>
{
    public static readonly Half Epsilon = (Half)9.765625e-04f;

    private static readonly Half Zero = Half.Zero;
    private static readonly Half HalfOne = (Half)0.5;
    private static readonly Half One = Half.One;
    private static readonly Half Ten = (Half)10.0;

    static Half IFloatingPointEpsilonOperations<Half>.Epsilon => Epsilon;

    public static int ToInt(Half @this) => Zero >= @this ? (int)(@this - HalfOne) : (int)(@this + HalfOne);

    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator", Justification = "Handles infinities")]
    public static bool IsCloseTo(Half @this, Half v, Half epsilon = default)
    {
        if (@this == v)
            return true;
        if (epsilon == default)
            epsilon = Epsilon;
        // This computes (|a - b| / (|a| + |b| + 10.0)) < epsilon
        Half eps = (Half.Abs(@this) + Half.Abs(v) + Ten) * epsilon;
        Half delta = @this - v;
        return -eps < delta && eps > delta;
    }

    public static bool IsGreaterThan(Half @this, Half v) => @this > v && !IsCloseTo(@this, v);
    public static bool IsGreaterThanOrClose(Half @this, Half v) => !(@this <= v) || IsCloseTo(@this, v);
    public static bool IsLessThan(Half @this, Half v) => @this < v && !IsCloseTo(@this, v);
    public static bool IsLessThanOrClose(Half @this, Half v) => !(@this >= v) || IsCloseTo(@this, v);
    public static bool IsOne(Half @this) => Half.Abs(@this - One) < Ten * Epsilon;
    public static bool IsZero(Half @this) => Half.Abs(@this) < Ten * Epsilon;
    public static bool IsBetweenZeroAndOne(Half @this) => IsGreaterThanOrClose(@this, Zero) && IsLessThanOrClose(@this, One);
}