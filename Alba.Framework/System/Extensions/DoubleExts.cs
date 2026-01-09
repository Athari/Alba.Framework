using System.Runtime.CompilerServices;
using Alba.Framework.Numerics;
using MethodAttribute = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Alba.Framework;

public static class DoubleExts
{
    public const double Epsilon = DoubleEpsilonOperations.Epsilon;

    private const MethodImplOptions Inline = MethodImplOptions.AggressiveInlining;

    extension(double @this)
    {
        public bool IsNumber {
            [Method(Inline)] get => !double.IsNaN(@this);
        }

        public bool IsInvalid {
            [Method(Inline)] get => double.IsNaN(@this);
        }

        public bool IsFin {
            [Method(Inline)] get => double.IsFinite(@this);
        }

        public bool isInf {
            [Method(Inline)] get => double.IsInfinity(@this);
        }

        public bool IsPosInf {
            [Method(Inline)] get => double.IsPositiveInfinity(@this);
        }

        public bool IsNegInf {
            [Method(Inline)] get => double.IsNegativeInfinity(@this);
        }

        public bool IsOne {
            [Method(Inline)] get => DoubleEpsilonOperations.IsOne(@this);
        }

        public bool IsZero {
            [Method(Inline)] get => DoubleEpsilonOperations.IsZero(@this);
        }

        public bool IsBetweenZeroAndOne {
            [Method(Inline)] get => DoubleEpsilonOperations.IsBetweenZeroAndOne(@this);
        }

        [Method(Inline)]
        public bool IsCloseTo(double v, double epsilon = Epsilon) => DoubleEpsilonOperations.IsCloseTo(@this, v, epsilon);

        [Method(Inline)]
        public bool IsGreaterThan(double v) => DoubleEpsilonOperations.IsGreaterThan(@this, v);

        [Method(Inline)]
        public bool IsGreaterThanOrClose(double v) => DoubleEpsilonOperations.IsGreaterThanOrClose(@this, v);

        [Method(Inline)]
        public bool IsLessThan(double v) => DoubleEpsilonOperations.IsLessThan(@this, v);

        [Method(Inline)]
        public bool IsLessThanOrClose(double v) => DoubleEpsilonOperations.IsLessThanOrClose(@this, v);

        [Method(Inline)]
        public int ToInt() => DoubleEpsilonOperations.ToInt(@this);
    }
}