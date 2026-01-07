using Alba.Framework.Numerics;

namespace Alba.Framework;

public static class DoubleExts
{
    public const double Epsilon = DoubleEpsilonOperations.Epsilon;

    extension(double @this)
    {
        public bool IsValid => !double.IsNaN(@this);
        public bool IsInvalid => double.IsNaN(@this);
        public bool IsInf => double.IsInfinity(@this);
        public bool IsPositiveInf => double.IsPositiveInfinity(@this);
        public bool IsNegativeInf => double.IsNegativeInfinity(@this);
        public bool IsOne => DoubleEpsilonOperations.IsOne(@this);
        public bool IsZero => DoubleEpsilonOperations.IsZero(@this);
        public bool IsBetweenZeroAndOne => DoubleEpsilonOperations.IsBetweenZeroAndOne(@this);

        public bool IsCloseTo(double v, double epsilon = Epsilon) => DoubleEpsilonOperations.IsCloseTo(@this, v, epsilon);
        public bool IsGreaterThan(double v) => DoubleEpsilonOperations.IsGreaterThan(@this, v);
        public bool IsGreaterThanOrClose(double v) => DoubleEpsilonOperations.IsGreaterThanOrClose(@this, v);
        public bool IsLessThan(double v) => DoubleEpsilonOperations.IsLessThan(@this, v);
        public bool IsLessThanOrClose(double v) => DoubleEpsilonOperations.IsLessThanOrClose(@this, v);

        public int ToInt() => DoubleEpsilonOperations.ToInt(@this);
    }
}