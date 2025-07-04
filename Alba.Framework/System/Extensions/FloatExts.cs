using System.Diagnostics.CodeAnalysis;

namespace Alba.Framework;

[PublicAPI]
public static class FloatExts
{
    public const float Epsilon = 1.192092896e-7f;

    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public static bool IsCloseTo(this float @this, float v, float epsilon = Epsilon)
    {
        //in case they are Infinities (then epsilon check does not work)
        if (@this == v)
            return true;
        // This computes (|value1-value2| / (|value1| + |value2| + 10.0)) < epsilon
        float eps = (Math.Abs(@this) + Math.Abs(v) + 10.0f) * epsilon;
        float delta = @this - v;
        return -eps < delta && eps > delta;
    }

    public static int FloatToInt(float @this) => 0.0f >= @this ? (int)(@this - 0.5f) : (int)(@this + 0.5f);
    public static bool IsGreaterThan(this float @this, float v) => @this > v && !IsCloseTo(@this, v);
    public static bool IsGreaterThanOrClose(this float @this, float v) => !(@this <= v) || IsCloseTo(@this, v);
    public static bool IsLessThan(this float @this, float v) => @this < v && !IsCloseTo(@this, v);
    public static bool IsLessThanOrClose(this float @this, float v) => !(@this >= v) || IsCloseTo(@this, v);
    public static bool IsNaN(this float @this) => float.IsNaN(@this);
    public static bool IsOne(this float @this) => Math.Abs(@this - 1.0f) < 10.0f * Epsilon;
    public static bool IsZero(this float @this) => Math.Abs(@this) < 10.0f * Epsilon;
    public static bool IsBetweenZeroAndOne(this float @this) => IsGreaterThanOrClose(@this, 0.0f) && IsLessThanOrClose(@this, 1.0f);
}