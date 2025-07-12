using System.Globalization;

namespace Alba.Framework.Avalonia.Markup.Converters;

public class NotNullConverter : ValueConverterBase<object?, bool>
{
    public override bool Convert(object? value, Type targetType, CultureInfo culture) => value != null;
}