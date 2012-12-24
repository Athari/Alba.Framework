using System;
using System.Runtime.InteropServices;
using System.Windows;

// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable FieldCanBeMadeReadOnly.Local
namespace Alba.Framework.Sys
{
    public static class DoubleExts
    {
        private const double Epsilon = 2.2204460492503131E-16;

        public static bool IsCloseTo (this double value1, double value2)
        {
            if (value1 == value2)
                return true;
            double num2 = (Math.Abs(value1) + Math.Abs(value2) + 10.0)*Epsilon;
            double num = value1 - value2;
            return -num2 < num && num2 > num;
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

        public static int DoubleToInt (double val)
        {
            return 0.0 >= val ? (int)(val - 0.5) : (int)(val + 0.5);
        }

        public static bool IsGreaterThan (this double value1, double value2)
        {
            return value1 > value2 && !IsCloseTo(value1, value2);
        }

        public static bool IsGreaterThanOrClose (this double value1, double value2)
        {
            return !(value1 <= value2) || IsCloseTo(value1, value2);
        }

        public static bool IsBetweenZeroAndOne (this double val)
        {
            return IsGreaterThanOrClose(val, 0.0) && IsLessThanOrClose(val, 1.0);
        }

        public static bool IsNaN (this double value)
        {
            var union = new NanUnion { DoubleValue = value };
            ulong num = union.UintValue & 18442240474082181120L;
            ulong num2 = union.UintValue & 0xfffffffffffffL;
            if (num != 0x7ff0000000000000L && num != 18442240474082181120L)
                return false;
            return num2 != 0L;
        }

        public static bool IsOne (this double value)
        {
            return Math.Abs(value - 1.0) < 2.2204460492503131E-15;
        }

        public static bool IsZero (this double value)
        {
            return Math.Abs(value) < 2.2204460492503131E-15;
        }

        public static bool IsLessThan (this double value1, double value2)
        {
            return value1 < value2 && !IsCloseTo(value1, value2);
        }

        public static bool IsLessThanOrClose (this double value1, double value2)
        {
            return !(value1 >= value2) || IsCloseTo(value1, value2);
        }

        public static bool IsHasNaN (Rect r)
        {
            return IsNaN(r.X) || IsNaN(r.Y) || IsNaN(r.Height) || IsNaN(r.Width);
        }

        [StructLayout (LayoutKind.Explicit)]
        private struct NanUnion
        {
            [FieldOffset (0)]
            internal double DoubleValue;
            [FieldOffset (0)]
            internal ulong UintValue;
        }
    }
}