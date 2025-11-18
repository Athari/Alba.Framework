using System.Text.Json;
using System.Text.Json.Serialization;
using Alba.Framework.Avalonia.Json.Internal;

namespace Alba.Framework.Avalonia.Json;

public class JsonValueConverter<T> : JsonConverter<T>
{
    public JsonValueConverterRef? Converter { get; set; }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var r = Converter?.ValueToRepr(value);
        writer.WriteValue(r);
    }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var v = Converter?.ReprToValue(reader.Value);
        return v is T t ? t : default!;
    }
}