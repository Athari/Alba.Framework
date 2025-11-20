namespace Alba.Framework.Numerics;

public class DoubleEpsilonOperations : IFloatingPointEpsilonOperations<double>
{
    public const double Epsilon = 2.2204460492503131e-16;

    static double IFloatingPointEpsilonOperations<double>.Epsilon => Epsilon;

    public static int ToInt(double @this) => 0.0 >= @this ? (int)(@this - 0.5) : (int)(@this + 0.5);

    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator", Justification = "Handles infinities")]
    public static bool IsCloseTo(double @this, double v, double epsilon = Epsilon)
    {
        if (@this == v)
            return true;
        // This computes (|a - b| / (|a| + |b| + 10.0)) < epsilon
        double eps = (Math.Abs(@this) + Math.Abs(v) + 10.0) * epsilon;
        double delta = @this - v;
        return -eps < delta && eps > delta;
    }

    public static bool IsGreaterThan(double @this, double v) => @this > v && !IsCloseTo(@this, v);
    public static bool IsGreaterThanOrClose(double @this, double v) => !(@this <= v) || IsCloseTo(@this, v);
    public static bool IsLessThan(double @this, double v) => @this < v && !IsCloseTo(@this, v);
    public static bool IsLessThanOrClose(double @this, double v) => !(@this >= v) || IsCloseTo(@this, v);
    public static bool IsOne(double @this) => Math.Abs(@this - 1.0) < 10.0 * Epsilon;
    public static bool IsZero(double @this) => Math.Abs(@this) < 10.0 * Epsilon;
    public static bool IsBetweenZeroAndOne(double @this) => IsGreaterThanOrClose(@this, 0.0) && IsLessThanOrClose(@this, 1.0);
}