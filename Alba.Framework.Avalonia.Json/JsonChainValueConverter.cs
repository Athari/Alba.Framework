using System.Text.Json;
using System.Text.Json.Serialization;
using Alba.Framework.Avalonia.Json.Internal;

namespace Alba.Framework.Avalonia.Json;

public class JsonChainValueConverter<T> : JsonConverter<T>
{
    public IList<JsonValueConverterRef> Converters { get; } = [ ];

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var r = Converters.Aggregate((object?)value, (v, conv) => conv.ValueToRepr(v));
        writer.WriteValue(r);
    }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var v = Converters.Reverse().Aggregate(reader.Value, (r, conv) => conv.ReprToValue(r));
        return v is T t ? t : default!;
    }
}