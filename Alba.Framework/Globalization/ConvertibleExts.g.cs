using System;
using System.Globalization;

namespace Alba.Framework.Globalization
{
    public static partial class ConvertibleExts
    {
        public static Boolean ToBoolean (this IConvertible @this)
        {
            return @this.ToBoolean(CultureInfo.CurrentCulture);
        }

        public static Boolean ToBooleanInv (this IConvertible @this)
        {
            return @this.ToBoolean(CultureInfo.InvariantCulture);
        }

        public static Char ToChar (this IConvertible @this)
        {
            return @this.ToChar(CultureInfo.CurrentCulture);
        }

        public static Char ToCharInv (this IConvertible @this)
        {
            return @this.ToChar(CultureInfo.InvariantCulture);
        }

        public static SByte ToSByte (this IConvertible @this)
        {
            return @this.ToSByte(CultureInfo.CurrentCulture);
        }

        public static SByte ToSByteInv (this IConvertible @this)
        {
            return @this.ToSByte(CultureInfo.InvariantCulture);
        }

        public static Byte ToByte (this IConvertible @this)
        {
            return @this.ToByte(CultureInfo.CurrentCulture);
        }

        public static Byte ToByteInv (this IConvertible @this)
        {
            return @this.ToByte(CultureInfo.InvariantCulture);
        }

        public static Int16 ToInt16 (this IConvertible @this)
        {
            return @this.ToInt16(CultureInfo.CurrentCulture);
        }

        public static Int16 ToInt16Inv (this IConvertible @this)
        {
            return @this.ToInt16(CultureInfo.InvariantCulture);
        }

        public static UInt16 ToUInt16 (this IConvertible @this)
        {
            return @this.ToUInt16(CultureInfo.CurrentCulture);
        }

        public static UInt16 ToUInt16Inv (this IConvertible @this)
        {
            return @this.ToUInt16(CultureInfo.InvariantCulture);
        }

        public static Int32 ToInt32 (this IConvertible @this)
        {
            return @this.ToInt32(CultureInfo.CurrentCulture);
        }

        public static Int32 ToInt32Inv (this IConvertible @this)
        {
            return @this.ToInt32(CultureInfo.InvariantCulture);
        }

        public static UInt32 ToUInt32 (this IConvertible @this)
        {
            return @this.ToUInt32(CultureInfo.CurrentCulture);
        }

        public static UInt32 ToUInt32Inv (this IConvertible @this)
        {
            return @this.ToUInt32(CultureInfo.InvariantCulture);
        }

        public static Int64 ToInt64 (this IConvertible @this)
        {
            return @this.ToInt64(CultureInfo.CurrentCulture);
        }

        public static Int64 ToInt64Inv (this IConvertible @this)
        {
            return @this.ToInt64(CultureInfo.InvariantCulture);
        }

        public static UInt64 ToUInt64 (this IConvertible @this)
        {
            return @this.ToUInt64(CultureInfo.CurrentCulture);
        }

        public static UInt64 ToUInt64Inv (this IConvertible @this)
        {
            return @this.ToUInt64(CultureInfo.InvariantCulture);
        }

        public static Single ToSingle (this IConvertible @this)
        {
            return @this.ToSingle(CultureInfo.CurrentCulture);
        }

        public static Single ToSingleInv (this IConvertible @this)
        {
            return @this.ToSingle(CultureInfo.InvariantCulture);
        }

        public static Double ToDouble (this IConvertible @this)
        {
            return @this.ToDouble(CultureInfo.CurrentCulture);
        }

        public static Double ToDoubleInv (this IConvertible @this)
        {
            return @this.ToDouble(CultureInfo.InvariantCulture);
        }

        public static Decimal ToDecimal (this IConvertible @this)
        {
            return @this.ToDecimal(CultureInfo.CurrentCulture);
        }

        public static Decimal ToDecimalInv (this IConvertible @this)
        {
            return @this.ToDecimal(CultureInfo.InvariantCulture);
        }

        public static DateTime ToDateTime (this IConvertible @this)
        {
            return @this.ToDateTime(CultureInfo.CurrentCulture);
        }

        public static DateTime ToDateTimeInv (this IConvertible @this)
        {
            return @this.ToDateTime(CultureInfo.InvariantCulture);
        }

    }
}