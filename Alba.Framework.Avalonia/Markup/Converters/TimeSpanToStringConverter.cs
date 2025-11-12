using System.Globalization;
using System.Text;

namespace Alba.Framework.Avalonia.Markup.Converters;

public class TimeSpanToStringConverter : ValueConverterBase<TimeSpan, string>
{
    public int Precision { get; set; } = 0;
    public bool TrailingZeroes { get; set; }

    public override string Convert(TimeSpan value, Type targetType, CultureInfo culture)
    {
        var sepDecimal = EscapeFormat(culture.NumberFormat.NumberDecimalSeparator);
        var sepTime = EscapeFormat(culture.DateTimeFormat.TimeSeparator);

        var format = "s";
        if (value.Minutes > 0)
            format = $"mm{sepTime}s" + format;
        if (value.Days > 0)
            format = $"dd{sepTime}" + format;
        if (Precision > 0)
            format += sepDecimal + new string(TrailingZeroes ? 'f' : 'F', Precision);

        // .NET tries to parse single 's' as generic format and fails
        return format == "s" ? value.Seconds.ToString("0") : value.ToString(format, culture);

        static string EscapeFormat(string s)
        {
            var sb = new StringBuilder(s.Length * 2 + 2);
            sb.Append('\'');
            foreach (var c in s) {
                sb.Append('\\');
                sb.Append(c);
            }
            sb.Append('\'');
            return sb.ToString();
        }
    }
}