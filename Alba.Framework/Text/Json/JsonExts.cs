using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Alba.Framework.Text.Json;

public static class JsonExts
{
    private static readonly JsonSerializerOptions CloneOptions = new() {
        DefaultIgnoreCondition = JsonIgnoreCondition.Never,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        IgnoreReadOnlyFields = true,
        IgnoreReadOnlyProperties = true,
        ReferenceHandler = ReferenceHandler.Preserve,
        WriteIndented = false,
    };

    public static T JsonClone<T>(this T @this)
    {
        if (@this == null)
            return default!;

        var stream = new MemoryStream();
        JsonSerializer.Serialize(stream, @this, CloneOptions);
        stream.Position = 0;
        return JsonSerializer.Deserialize<T>(stream, CloneOptions)!;
    }
}