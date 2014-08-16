using System;

// ReSharper disable CompareOfFloatsByEqualityOperator
namespace Alba.Framework.Sys
{
    public static class DoubleExts
    {
        public const double Epsilon = 2.2204460492503131e-16;

        public static bool IsCloseTo (this double value1, double value2, double epsilon = Epsilon)
        {
            //in case they are Infinities (then epsilon check does not work)
            if (value1 == value2)
                return true;
            // This computes (|value1-value2| / (|value1| + |value2| + 10.0)) < epsilon
            double eps = (Math.Abs(value1) + Math.Abs(value2) + 10.0) * epsilon;
            double delta = value1 - value2;
            return (-eps < delta) && (eps > delta);
        }

        public static int DoubleToInt (double val)
        {
            return 0.0 >= val ? (int)(val - 0.5) : (int)(val + 0.5);
        }

        public static bool IsGreaterThan (this double value1, double value2)
        {
            return (value1 > value2) && !IsCloseTo(value1, value2);
        }

        public static bool IsGreaterThanOrClose (this double value1, double value2)
        {
            return !(value1 <= value2) || IsCloseTo(value1, value2);
        }

        public static bool IsLessThan (this double value1, double value2)
        {
            return (value1 < value2) && !IsCloseTo(value1, value2);
        }

        public static bool IsLessThanOrClose (this double value1, double value2)
        {
            return !(value1 >= value2) || IsCloseTo(value1, value2);
        }

        public static bool IsNaN (this double value)
        {
            return double.IsNaN(value);
        }

        public static bool IsOne (this double value)
        {
            return Math.Abs(value - 1.0) < 10.0 * Epsilon;
        }

        public static bool IsZero (this double value)
        {
            return Math.Abs(value) < 10.0 * Epsilon;
        }

        public static bool IsBetweenZeroAndOne (this double val)
        {
            return IsGreaterThanOrClose(val, 0.0) && IsLessThanOrClose(val, 1.0);
        }
    }
}