using System.Collections;
using System.Globalization;
using Alba.Framework.Collections;
using Avalonia;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Alba.Framework.Avalonia.Markup.MarkupExtensions;

public class EqualsAnyExtension : ParamBinding<object?>
{
    private static readonly EqualsAnyConverter _convEqualsAny = new();

    public EqualsAnyExtension(IBinding b1, object? param) : base(b1, param) => Converter = _convEqualsAny;

    internal class EqualsAnyConverter : IMultiValueConverter
    {
        public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            var value = values.AtOrDefault(0, AvaloniaProperty.UnsetValue);
            IEnumerable<object?> items = parameter switch {
                IEnumerable<object?> ieo => ieo,
                IEnumerable ie => ie.Cast<object?>(),
                _ => [],
            };
            return items.Any(i => Equals(value, i));
        }
    }
}