using System;
using System.Windows;

// ReSharper disable CompareOfFloatsByEqualityOperator
namespace Alba.Framework.Sys
{
    public static class DoubleExts
    {
        private const double Epsilon = 2.2204460492503131e-16;

        public static bool IsCloseTo (this double value1, double value2)
        {
            //in case they are Infinities (then epsilon check does not work)
            if (value1 == value2)
                return true;
            // This computes (|value1-value2| / (|value1| + |value2| + 10.0)) < Epsilon
            double eps = (Math.Abs(value1) + Math.Abs(value2) + 10.0) * Epsilon;
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

        public static bool IsCloseTo (this Point point1, Point point2)
        {
            return IsCloseTo(point1.X, point2.X) && IsCloseTo(point1.Y, point2.Y);
        }

        public static bool IsCloseTo (this Rect rect1, Rect rect2)
        {
            return rect1.IsEmpty ? rect2.IsEmpty : !rect2.IsEmpty && IsCloseTo(rect1.X, rect2.X) &&
                IsCloseTo(rect1.Y, rect2.Y) && IsCloseTo(rect1.Height, rect2.Height) &&
                IsCloseTo(rect1.Width, rect2.Width);
        }

        public static bool IsCloseTo (this Size size1, Size size2)
        {
            return IsCloseTo(size1.Width, size2.Width) && IsCloseTo(size1.Height, size2.Height);
        }

        public static bool IsCloseTo (this Vector vector1, Vector vector2)
        {
            return IsCloseTo(vector1.X, vector2.X) && IsCloseTo(vector1.Y, vector2.Y);
        }

        public static bool HasNaN (this Rect r)
        {
            return IsNaN(r.X) || IsNaN(r.Y) || IsNaN(r.Height) || IsNaN(r.Width);
        }
    }
}