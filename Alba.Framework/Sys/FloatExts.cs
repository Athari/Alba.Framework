using System;

// ReSharper disable CompareOfFloatsByEqualityOperator
namespace Alba.Framework.Sys
{
    public static class FloatExts
    {
        private const float Epsilon = 1.192092896e-7f;

        public static bool IsCloseTo (this float value1, float value2)
        {
            //in case they are Infinities (then epsilon check does not work)
            if (value1 == value2)
                return true;
            // This computes (|value1-value2| / (|value1| + |value2| + 10.0f)) < Epsilon
            float eps = (Math.Abs(value1) + Math.Abs(value2) + 10.0f) * Epsilon;
            float delta = value1 - value2;
            return (-eps < delta) && (eps > delta);
        }

        public static int FloatToInt (float val)
        {
            return 0.0f >= val ? (int)(val - 0.5f) : (int)(val + 0.5f);
        }

        public static bool IsGreaterThan (this float value1, float value2)
        {
            return (value1 > value2) && !IsCloseTo(value1, value2);
        }

        public static bool IsGreaterThanOrClose (this float value1, float value2)
        {
            return !(value1 <= value2) || IsCloseTo(value1, value2);
        }

        public static bool IsLessThan (this float value1, float value2)
        {
            return (value1 < value2) && !IsCloseTo(value1, value2);
        }

        public static bool IsLessThanOrClose (this float value1, float value2)
        {
            return !(value1 >= value2) || IsCloseTo(value1, value2);
        }

        public static bool IsNaN (this float value)
        {
            return float.IsNaN(value);
        }

        public static bool IsOne (this float value)
        {
            return Math.Abs(value - 1.0f) < 10.0f * Epsilon;
        }

        public static bool IsZero (this float value)
        {
            return Math.Abs(value) < 10.0f * Epsilon;
        }

        public static bool IsBetweenZeroAndOne (this float val)
        {
            return IsGreaterThanOrClose(val, 0.0f) && IsLessThanOrClose(val, 1.0f);
        }
    }
}