using System.Globalization;
using Alba.Framework.Collections;
using Avalonia;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Alba.Framework.Avalonia.Markup.MarkupExtensions;

public class NotEqualsExtension : ParamBinding<object?>
{
    private static readonly EqualsConverter _conv = new();

    public NotEqualsExtension(IBinding b1, object? param) : base(b1, param) => Converter = _conv;

    private class EqualsConverter : IMultiValueConverter
    {
        public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture) =>
            !Equals(values.AtOrDefault(0, AvaloniaProperty.UnsetValue), parameter);
    }
}