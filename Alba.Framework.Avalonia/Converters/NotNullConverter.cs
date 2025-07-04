using System.Globalization;

namespace Alba.Framework.Avalonia.Converters;

public class NotNullConverter : ValueConverterBase<object?, bool>
{
    public override bool Convert(object? value, Type targetType, CultureInfo culture) => value != null;
}