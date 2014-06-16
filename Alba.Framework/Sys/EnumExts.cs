using System;
using System.Collections.Generic;
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
            UInt64 uthis = ToUInt64(@this), uflags = ToUInt64(flags);
            return (uthis & uflags) != 0;
        }

        public static bool HasAll<T> (this T @this, T flags)
            where T : struct, IComparable, IFormattable, IConvertible //, Enum
        {
            UInt64 uthis = ToUInt64(@this), uflags = ToUInt64(flags);
            return (uthis & uflags) == uflags;
        }

        public static IEnumerable<T> EnumerateBits<T> (this T @this)
            where T : struct, IComparable, IFormattable, IConvertible //, Enum
        {
            TypeCode typeCode = Convert.GetTypeCode(@this);
            switch (typeCode) {
                case TypeCode.SByte: {
                    SByte value = Convert.ToSByte(@this, null);
                    for (int i = 0; i < 8; i++)
                        if ((value & 1 << i) != 0)
                            yield return (T)(object)(SByte)(1 << i);
                    break;
                }
                case TypeCode.Byte: {
                    Byte value = Convert.ToByte(@this, null);
                    for (int i = 0; i < 8; i++)
                        if ((value & 1 << i) != 0)
                            yield return (T)(object)(Byte)(1 << i);
                    break;
                }
                case TypeCode.Int16: {
                    Int16 value = Convert.ToInt16(@this, null);
                    for (int i = 0; i < 16; i++)
                        if ((value & 1 << i) != 0)
                            yield return (T)(object)(Int16)(1 << i);
                    break;
                }
                case TypeCode.UInt16: {
                    UInt16 value = Convert.ToUInt16(@this, null);
                    for (int i = 0; i < 16; i++)
                        if ((value & 1 << i) != 0)
                            yield return (T)(object)(UInt16)(1 << i);
                    break;
                }
                case TypeCode.Int32: {
                    Int32 value = Convert.ToInt32(@this, null);
                    for (int i = 0; i < 32; i++)
                        if ((value & 1 << i) != 0)
                            yield return (T)(object)(1 << i);
                    break;
                }
                case TypeCode.UInt32: {
                    UInt32 value = Convert.ToUInt32(@this, null);
                    for (int i = 0; i < 32; i++)
                        if ((value & 1U << i) != 0)
                            yield return (T)(object)(1U << i);
                    break;
                }
                case TypeCode.Int64: {
                    Int64 value = Convert.ToInt64(@this, null);
                    for (int i = 0; i < 64; i++)
                        if ((value & 1L << i) != 0)
                            yield return (T)(object)(1L << i);
                    break;
                }
                case TypeCode.UInt64: {
                    UInt64 value = Convert.ToUInt64(@this, null);
                    for (int i = 0; i < 64; i++)
                        if ((value & 1UL << i) != 0)
                            yield return (T)(object)(1UL << i);
                    break;
                }
                default:
                    throw new InvalidOperationException("Unknown enum type: {0}.".Fmt(typeCode));
            }
        }

        private static UInt64 ToUInt64<T> (T value)
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