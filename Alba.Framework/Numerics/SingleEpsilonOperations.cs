namespace Alba.Framework.Numerics;

public class SingleEpsilonOperations : IFloatingPointEpsilonOperations<float>
{
    public const float Epsilon = 1.19209290e-07f;

    static float IFloatingPointEpsilonOperations<float>.Epsilon => Epsilon;

    public static int ToInt(float @this) => 0.0 >= @this ? (int)(@this - 0.5f) : (int)(@this + 0.5f);

    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator", Justification = "Handles infinities")]
    public static bool IsCloseTo(float @this, float v, float epsilon = Epsilon)
    {
        if (@this == v)
            return true;
        // This computes (|a - b| / (|a| + |b| + 10.0)) < epsilon
        float eps = (Math.Abs(@this) + Math.Abs(v) + 10.0f) * epsilon;
        float delta = @this - v;
        return -eps < delta && eps > delta;
    }

    public static bool IsGreaterThan(float @this, float v) => @this > v && !IsCloseTo(@this, v);
    public static bool IsGreaterThanOrClose(float @this, float v) => !(@this <= v) || IsCloseTo(@this, v);
    public static bool IsLessThan(float @this, float v) => @this < v && !IsCloseTo(@this, v);
    public static bool IsLessThanOrClose(float @this, float v) => !(@this >= v) || IsCloseTo(@this, v);
    public static bool IsOne(float @this) => Math.Abs(@this - 1.0f) < 10.0f * Epsilon;
    public static bool IsZero(float @this) => Math.Abs(@this) < 10.0f * Epsilon;
    public static bool IsBetweenZeroAndOne(float @this) => IsGreaterThanOrClose(@this, 0.0f) && IsLessThanOrClose(@this, 1.0f);
}