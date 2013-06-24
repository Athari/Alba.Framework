using System;
using Alba.Framework.Text;

namespace Alba.Framework.Sys
{
    /// <summary>
    /// Methods for checking enum flags. Works with integer values too.
    /// </summary>
    public static class EnumExts
    {
        public static bool Has<T> (this T @this, T flags)
            where T : struct, IComparable, IFormattable, IConvertible //, Enum
        {
            // TODO Contract check: number of flags in `flags` equals 1
            return @this.HasAll(flags);
        }

        public static bool HasAny<T> (this T @this, T flags)
            where T : struct, IComparable, IFormattable, IConvertible //, Enum
        {
            ulong uthis = ToUInt64(@this), uflags = ToUInt64(flags);
            return (uthis & uflags) != 0;
        }

        public static bool HasAll<T> (this T @this, T flags)
            where T : struct, IComparable, IFormattable, IConvertible //, Enum
        {
            ulong uthis = ToUInt64(@this), uflags = ToUInt64(flags);
            return (uthis & uflags) == uflags;
        }

        private static ulong ToUInt64<T> (T value)
        {
            // Helper function to silently convert the value to UInt64 from the other base types for enum without throwing an exception.
            // This is need since the Convert functions do overflow checks.
            TypeCode typeCode = Convert.GetTypeCode(value);

            switch (typeCode) {
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    return (UInt64)Convert.ToInt64(value, null);

                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return Convert.ToUInt64(value, null);

                default:
                    throw new InvalidOperationException("Unknown enum type: {0}.".Fmt(typeCode));
            }
        }
    }
}