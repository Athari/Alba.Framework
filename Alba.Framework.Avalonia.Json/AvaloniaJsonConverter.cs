using Avalonia.Data.Converters;

namespace Alba.Framework.Avalonia.Json;

public class AvaloniaJsonConverter<T, TRepr, TConverter> : JsonValueConverter<T>
    where TConverter : class, IValueConverter, new()
{
    public AvaloniaJsonConverter(TConverter? converter = null) =>
        Converter = new JsonAvaloniaConverterRef<T, TRepr, TConverter>(converter);
}