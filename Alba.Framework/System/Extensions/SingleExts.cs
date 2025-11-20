using Alba.Framework.Numerics;

namespace Alba.Framework;

public static class SingleExts
{
    public const float Epsilon = SingleEpsilonOperations.Epsilon;

    extension(float @this)
    {
        public bool IsNaN => float.IsNaN(@this);
        public bool IsOne => SingleEpsilonOperations.IsOne(@this);
        public bool IsZero => SingleEpsilonOperations.IsZero(@this);
        public bool IsBetweenZeroAndOne => SingleEpsilonOperations.IsBetweenZeroAndOne(@this);

        public bool IsCloseTo(float v, float epsilon = Epsilon) => SingleEpsilonOperations.IsCloseTo(@this, v, epsilon);
        public bool IsGreaterThan(float v) => SingleEpsilonOperations.IsGreaterThan(@this, v);
        public bool IsGreaterThanOrClose(float v) => SingleEpsilonOperations.IsGreaterThanOrClose(@this, v);
        public bool IsLessThan(float v) => SingleEpsilonOperations.IsLessThan(@this, v);
        public bool IsLessThanOrClose(float v) => SingleEpsilonOperations.IsLessThanOrClose(@this, v);

        public int ToInt() => SingleEpsilonOperations.ToInt(@this);
    }
}