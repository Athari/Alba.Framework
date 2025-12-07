using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Alba.Framework.Avalonia.Markup.MarkupExtensions;

public class NotEqualsAnyExtension : ParamBinding<object?>
{
    private static readonly EqualsAnyExtension.EqualsAnyConverter _convEqualsAny = new();
    private static readonly NotEqualsAnyConverter _convNotEqualsAny = new();

    public NotEqualsAnyExtension(IBinding b1, object? param) : base(b1, param) => Converter = _convNotEqualsAny;

    internal class NotEqualsAnyConverter : IMultiValueConverter
    {
        public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture) =>
            !(bool)_convEqualsAny.Convert(values, targetType, parameter, culture);
    }
}