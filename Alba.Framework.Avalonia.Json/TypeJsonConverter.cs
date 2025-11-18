using System.ComponentModel;

namespace Alba.Framework.Avalonia.Json;

public class TypeJsonConverter<T, TRepr, TConverter> : JsonValueConverter<T>
    where TConverter : TypeConverter, new()
{
    public TypeJsonConverter() =>
        Converter = new JsonTypeConverterRef<T, TRepr, TConverter>();
}