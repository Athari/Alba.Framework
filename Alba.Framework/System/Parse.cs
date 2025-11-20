using System.Globalization;

namespace Alba.Framework;

public static class Parse
{
    private static readonly IFormatProvider Inv = CultureInfo.InvariantCulture;

    public static sbyte SByte(string value) => sbyte.Parse(value, Inv);
    public static short Int16(string value) => short.Parse(value, Inv);
    public static int Int32(string value) => int.Parse(value, Inv);
    public static long Int64(string value) => long.Parse(value, Inv);
    public static byte Byte(string value) => byte.Parse(value, Inv);
    public static ushort UInt16(string value) => ushort.Parse(value, Inv);
    public static uint UInt32(string value) => uint.Parse(value, Inv);
    public static ulong UInt64(string value) => ulong.Parse(value, Inv);
    public static float Single(string value) => float.Parse(value, Inv);
    public static double Double(string value) => double.Parse(value, Inv);
    public static decimal Decimal(string value) => decimal.Parse(value, Inv);
    public static char Char(string value) => char.Parse(value);
    public static bool Boolean(string value) => bool.Parse(value);
    public static DateTime DateTime(string value) => System.DateTime.Parse(value, Inv);
    public static DateOnly DateOnly(string value) => System.DateOnly.Parse(value, Inv);
    public static TimeOnly TimeOnly(string value) => System.TimeOnly.Parse(value, Inv);
    public static DateTimeOffset DateTimeOffset(string value) => System.DateTimeOffset.Parse(value, Inv);
    public static TimeSpan TimeSpan(string value) => System.TimeSpan.Parse(value, Inv);
    public static TEnum Enum<TEnum>(string value) => (TEnum)System.Enum.Parse(typeof(TEnum), value);
    public static TEnum Enum<TEnum>(string value, bool ignoreCase) => (TEnum)System.Enum.Parse(typeof(TEnum), value, ignoreCase);
}