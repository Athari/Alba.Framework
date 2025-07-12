using System.Globalization;

namespace Alba.Framework.Avalonia.Markup.Converters;

public class EqualsParamConverter : ValueConverterBase<object?, bool, object?>
{
    public override bool Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        Equals(value, parameter);
}