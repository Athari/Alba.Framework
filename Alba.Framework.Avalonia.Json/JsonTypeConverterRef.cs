using System.ComponentModel;
using System.Globalization;

namespace Alba.Framework.Avalonia.Json;

public class JsonTypeConverterRef<T, TRepr, TConverter> : JsonValueConverterRef<T, TRepr>
    where TConverter : TypeConverter, new()
{
    private static readonly TConverter _DefaultConverter = new();

    public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;

    public JsonTypeConverterRef() => ConvertNull = false;

    public TConverter Converter => _DefaultConverter;

    protected override object? ValueToReprOverride(object? o) =>
        Converter.ConvertTo(null, Culture, o, ReprType);

    protected override object? ReprToValueOverride(object? o) =>
        Converter.ConvertFrom(null, Culture, o!);
}