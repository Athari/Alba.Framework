namespace Alba.Framework;

/// <summary>
/// Methods for checking enum flags. Works with integer values too.
/// </summary>
public static class EnumExts
{
    extension<T>(T @this) where T : struct, IComparable, IFormattable, IConvertible //, Enum
    {
        // TODO Contract check: number of flags in `flags` equals 1
        public bool Has(T flags) => @this.HasAll(flags);

        public bool HasAny(T flags) => (ToUInt64(@this) & ToUInt64(flags)) != 0;

        public bool HasAll(T flags) => (ToUInt64(@this) & ToUInt64(flags)) == ToUInt64(flags);

        public T With(T flag) => @this.With(flag, true);

        public T Without(T flag) => @this.With(flag, false);

        public T Toggle(T flag) => @this.With(flag, !@this.Has(flag));

        public T With(T flag, bool isSet)
        {
            TypeCode typeCode = Convert.GetTypeCode(@this);
            switch (typeCode) {
                case TypeCode.SByte: {
                    sbyte ithis = Convert.ToSByte(@this, null), iflag = Convert.ToSByte(flag, null);
                    return (T)(object)(isSet ? (ithis | iflag) : (ithis & ~iflag));
                }
                case TypeCode.Byte: {
                    byte ithis = Convert.ToByte(@this, null), iflag = Convert.ToByte(flag, null);
                    return (T)(object)(isSet ? (ithis | iflag) : (ithis & ~iflag));
                }
                case TypeCode.Int16: {
                    short ithis = Convert.ToInt16(@this, null), iflag = Convert.ToInt16(flag, null);
                    return (T)(object)(isSet ? (ithis | iflag) : (ithis & ~iflag));
                }
                case TypeCode.UInt16: {
                    ushort ithis = Convert.ToUInt16(@this, null), iflag = Convert.ToUInt16(flag, null);
                    return (T)(object)(isSet ? (ithis | iflag) : (ithis & ~iflag));
                }
                case TypeCode.Int32: {
                    int ithis = Convert.ToInt32(@this, null), iflag = Convert.ToInt32(flag, null);
                    return (T)(object)(isSet ? (ithis | iflag) : (ithis & ~iflag));
                }
                case TypeCode.UInt32: {
                    uint ithis = Convert.ToUInt32(@this, null), iflag = Convert.ToUInt32(flag, null);
                    return (T)(object)(isSet ? (ithis | iflag) : (ithis & ~iflag));
                }
                case TypeCode.Int64: {
                    long ithis = Convert.ToInt64(@this, null), iflag = Convert.ToInt64(flag, null);
                    return (T)(object)(isSet ? (ithis | iflag) : (ithis & ~iflag));
                }
                case TypeCode.UInt64: {
                    ulong ithis = Convert.ToUInt64(@this, null), iflag = Convert.ToUInt64(flag, null);
                    return (T)(object)(isSet ? (ithis | iflag) : (ithis & ~iflag));
                }
                default:
                    throw new InvalidOperationException($"Unknown enum type: {typeCode}.");
            }
        }

        public IEnumerable<T> EnumerateBits()
        {
            TypeCode typeCode = Convert.GetTypeCode(@this);
            switch (typeCode) {
                case TypeCode.SByte: {
                    sbyte value = Convert.ToSByte(@this, null);
                    for (int i = 0; i < 8; i++)
                        if ((value & 1 << i) != 0)
                            yield return (T)(object)(sbyte)(1 << i);
                    break;
                }
                case TypeCode.Byte: {
                    byte value = Convert.ToByte(@this, null);
                    for (int i = 0; i < 8; i++)
                        if ((value & 1 << i) != 0)
                            yield return (T)(object)(byte)(1 << i);
                    break;
                }
                case TypeCode.Int16: {
                    short value = Convert.ToInt16(@this, null);
                    for (int i = 0; i < 16; i++)
                        if ((value & 1 << i) != 0)
                            yield return (T)(object)(short)(1 << i);
                    break;
                }
                case TypeCode.UInt16: {
                    ushort value = Convert.ToUInt16(@this, null);
                    for (int i = 0; i < 16; i++)
                        if ((value & 1 << i) != 0)
                            yield return (T)(object)(ushort)(1 << i);
                    break;
                }
                case TypeCode.Int32: {
                    int value = Convert.ToInt32(@this, null);
                    for (int i = 0; i < 32; i++)
                        if ((value & 1 << i) != 0)
                            yield return (T)(object)(1 << i);
                    break;
                }
                case TypeCode.UInt32: {
                    uint value = Convert.ToUInt32(@this, null);
                    for (int i = 0; i < 32; i++)
                        if ((value & 1U << i) != 0)
                            yield return (T)(object)(1U << i);
                    break;
                }
                case TypeCode.Int64: {
                    long value = Convert.ToInt64(@this, null);
                    for (int i = 0; i < 64; i++)
                        if ((value & 1L << i) != 0)
                            yield return (T)(object)(1L << i);
                    break;
                }
                case TypeCode.UInt64: {
                    ulong value = Convert.ToUInt64(@this, null);
                    for (int i = 0; i < 64; i++)
                        if ((value & 1UL << i) != 0)
                            yield return (T)(object)(1UL << i);
                    break;
                }
                default:
                    throw new InvalidOperationException($"Unknown enum type: {typeCode}.");
            }
        }
    }

    private static ulong ToUInt64<T>(T value)
    {
        // Helper function to silently convert the value to UInt64 from the other base types for enum without throwing an exception.
        // This is needed since the Convert functions do overflow checks.
        TypeCode typeCode = Convert.GetTypeCode(value);

        return typeCode switch {
            TypeCode.SByte or TypeCode.Int16 or TypeCode.Int32 or TypeCode.Int64 => (ulong)Convert.ToInt64(value, null),
            TypeCode.Byte or TypeCode.UInt16 or TypeCode.UInt32 or TypeCode.UInt64 => Convert.ToUInt64(value, null),
            _ => throw new InvalidOperationException($"Unknown enum type: {typeCode}."),
        };
    }
}