using System.Globalization;

namespace Alba.Framework.Avalonia.Markup.Converters;

public class NotEqualsAnyParamConverter : ValueConverterBase<object?, bool, IEnumerable<object?>?>
{
    public override bool Convert(object? value, Type targetType, IEnumerable<object?>? parameter, CultureInfo culture) =>
        !(parameter?.Any(p => Equals(value, p)) ?? false);
}