using System.Globalization;

namespace Alba.Framework.Avalonia.Converters;

public class ToBoolConverter : ValueConverterBase<object?, bool>
{
    public override bool Convert(object? value, Type targetType, CultureInfo culture) => 
        System.Convert.GetTypeCode(value) switch {
            TypeCode.Empty => false,
            TypeCode.DBNull => false,
            TypeCode.Object => true,
            TypeCode.Boolean => (bool)value!,
            TypeCode.Char => (char)value! != 0,
            TypeCode.SByte => (sbyte)value! != 0,
            TypeCode.Byte => (byte)value! != 0,
            TypeCode.Int16 => (short)value! != 0,
            TypeCode.UInt16 => (ushort)value! != 0,
            TypeCode.Int32 => (int)value! != 0,
            TypeCode.UInt32 => (uint)value! != 0,
            TypeCode.Int64 => (long)value! != 0,
            TypeCode.UInt64 => (ulong)value! != 0,
            TypeCode.Single => (float)value! != 0,
            TypeCode.Double => (double)value! != 0,
            TypeCode.Decimal => (decimal)value! != 0,
            TypeCode.DateTime => (DateTime)value! != DateTime.UnixEpoch && (DateTime)value! != DateTime.MinValue,
            TypeCode.String => ((string)value!).Length > 0,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
        };
}