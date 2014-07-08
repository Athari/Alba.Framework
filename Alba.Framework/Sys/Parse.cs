using System;
using System.Globalization;

namespace Alba.Framework.Sys
{
    public static class Parse
    {
        private static readonly IFormatProvider _inv = CultureInfo.InvariantCulture;

        public static SByte SByte (string value)
        {
            return System.SByte.Parse(value, _inv);
        }

        public static Int16 Int16 (string value)
        {
            return System.Int16.Parse(value, _inv);
        }

        public static Int32 Int32 (string value)
        {
            return System.Int32.Parse(value, _inv);
        }

        public static Int64 Int64 (string value)
        {
            return System.Int64.Parse(value, _inv);
        }

        public static Byte Byte (string value)
        {
            return System.Byte.Parse(value, _inv);
        }

        public static UInt16 UInt16 (string value)
        {
            return System.UInt16.Parse(value, _inv);
        }

        public static UInt32 UInt32 (string value)
        {
            return System.UInt32.Parse(value, _inv);
        }

        public static UInt64 UInt64 (string value)
        {
            return System.UInt64.Parse(value, _inv);
        }

        public static Single Single (string value)
        {
            return System.Single.Parse(value, _inv);
        }

        public static Double Double (string value)
        {
            return System.Double.Parse(value, _inv);
        }

        public static Decimal Decimal (string value)
        {
            return System.Decimal.Parse(value, _inv);
        }

        public static Char Char (string value)
        {
            return System.Char.Parse(value);
        }

        public static Boolean Boolean (string value)
        {
            return System.Boolean.Parse(value);
        }

        public static DateTime DateTime (string value)
        {
            return System.DateTime.Parse(value, _inv);
        }

        public static DateTimeOffset DateTimeOffset (string value)
        {
            return System.DateTimeOffset.Parse(value, _inv);
        }

        public static TimeSpan TimeSpan (string value)
        {
            return System.TimeSpan.Parse(value, _inv);
        }

        public static TEnum Enum<TEnum> (string value)
        {
            return (TEnum)System.Enum.Parse(typeof(TEnum), value);
        }

        public static TEnum Enum<TEnum> (string value, bool ignoreCase)
        {
            return (TEnum)System.Enum.Parse(typeof(TEnum), value, ignoreCase);
        }
    }
}