using System;
using System.Globalization;

namespace Alba.Framework.Globalization;

public static partial class ConvertibleExts
{
    extension(IConvertible @this)
    {
        public Boolean ToBoolean() => @this.ToBoolean(CultureInfo.CurrentCulture);
        public Boolean ToBooleanInv() => @this.ToBoolean(CultureInfo.InvariantCulture);
        public Char ToChar() => @this.ToChar(CultureInfo.CurrentCulture);
        public Char ToCharInv() => @this.ToChar(CultureInfo.InvariantCulture);
        public SByte ToSByte() => @this.ToSByte(CultureInfo.CurrentCulture);
        public SByte ToSByteInv() => @this.ToSByte(CultureInfo.InvariantCulture);
        public Byte ToByte() => @this.ToByte(CultureInfo.CurrentCulture);
        public Byte ToByteInv() => @this.ToByte(CultureInfo.InvariantCulture);
        public Int16 ToInt16() => @this.ToInt16(CultureInfo.CurrentCulture);
        public Int16 ToInt16Inv() => @this.ToInt16(CultureInfo.InvariantCulture);
        public UInt16 ToUInt16() => @this.ToUInt16(CultureInfo.CurrentCulture);
        public UInt16 ToUInt16Inv() => @this.ToUInt16(CultureInfo.InvariantCulture);
        public Int32 ToInt32() => @this.ToInt32(CultureInfo.CurrentCulture);
        public Int32 ToInt32Inv() => @this.ToInt32(CultureInfo.InvariantCulture);
        public UInt32 ToUInt32() => @this.ToUInt32(CultureInfo.CurrentCulture);
        public UInt32 ToUInt32Inv() => @this.ToUInt32(CultureInfo.InvariantCulture);
        public Int64 ToInt64() => @this.ToInt64(CultureInfo.CurrentCulture);
        public Int64 ToInt64Inv() => @this.ToInt64(CultureInfo.InvariantCulture);
        public UInt64 ToUInt64() => @this.ToUInt64(CultureInfo.CurrentCulture);
        public UInt64 ToUInt64Inv() => @this.ToUInt64(CultureInfo.InvariantCulture);
        public Single ToSingle() => @this.ToSingle(CultureInfo.CurrentCulture);
        public Single ToSingleInv() => @this.ToSingle(CultureInfo.InvariantCulture);
        public Double ToDouble() => @this.ToDouble(CultureInfo.CurrentCulture);
        public Double ToDoubleInv() => @this.ToDouble(CultureInfo.InvariantCulture);
        public Decimal ToDecimal() => @this.ToDecimal(CultureInfo.CurrentCulture);
        public Decimal ToDecimalInv() => @this.ToDecimal(CultureInfo.InvariantCulture);
        public DateTime ToDateTime() => @this.ToDateTime(CultureInfo.CurrentCulture);
        public DateTime ToDateTimeInv() => @this.ToDateTime(CultureInfo.InvariantCulture);
    }
}