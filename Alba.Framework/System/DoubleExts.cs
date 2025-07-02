namespace Alba.Framework;

[PublicAPI]
public static class DoubleExts
{
    public const double Epsilon = 2.2204460492503131e-16;

    public static bool IsCloseTo(this double @this, double v, double epsilon = Epsilon)
    {
        //in case they are Infinities (then epsilon check does not work)
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (@this == v)
            return true;
        // This computes (|value1-value2| / (|value1| + |value2| + 10.0)) < epsilon
        double eps = (Math.Abs(@this) + Math.Abs(v) + 10.0) * epsilon;
        double delta = @this - v;
        return -eps < delta && eps > delta;
    }

    public static int DoubleToInt(double @this) => 0.0 >= @this ? (int)(@this - 0.5) : (int)(@this + 0.5);
    public static bool IsGreaterThan(this double @this, double v) => @this > v && !IsCloseTo(@this, v);
    public static bool IsGreaterThanOrClose(this double @this, double v) => !(@this <= v) || IsCloseTo(@this, v);
    public static bool IsLessThan(this double @this, double v) => @this < v && !IsCloseTo(@this, v);
    public static bool IsLessThanOrClose(this double @this, double v) => !(@this >= v) || IsCloseTo(@this, v);
    public static bool IsNaN(this double @this) => double.IsNaN(@this);
    public static bool IsOne(this double @this) => Math.Abs(@this - 1.0) < 10.0 * Epsilon;
    public static bool IsZero(this double @this) => Math.Abs(@this) < 10.0 * Epsilon;
    public static bool IsBetweenZeroAndOne(this double @this) => IsGreaterThanOrClose(@this, 0.0) && IsLessThanOrClose(@this, 1.0);
}