using System.Globalization;
using Avalonia.Data.Converters;

namespace Alba.Framework.Avalonia.Json;

public class JsonAvaloniaConverterRef<T, TRepr, TConverter>
    (TConverter? converter = null)
    : JsonValueConverterRef<T, TRepr>
    where TConverter : class, IValueConverter, new()
{
    protected static readonly TConverter DefaultConverter = new();

    public TConverter Converter { get; init; } = converter ?? DefaultConverter;
    public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;
    public object? Parameter { get; set; }

    protected override object? ValueToReprOverride(object? o) =>
        Converter.Convert(o, ReprType, Parameter, Culture);

    protected override object? ReprToValueOverride(object? o) =>
        Converter.ConvertBack(o, ValueType, Parameter, Culture);
}