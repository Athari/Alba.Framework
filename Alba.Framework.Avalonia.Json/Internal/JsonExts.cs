using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using Alba.Framework.Common;
using C = System.TypeCode;

namespace Alba.Framework.Avalonia.Json.Internal;

// TODO Reference Alba.Json.Dynamic or wherever this code ends up

[SuppressMessage("Naming", "CA1822: Mark members as static", Justification = "Compiler bug")]
internal static class Utf8JsonReaderExts
{
    extension(ref Utf8JsonReader @this)
    {
        public object? Value => JsonElementExts.ToValue(JsonElement.ParseValue(ref @this), JNodeOptions.Default);
    }
}

[SuppressMessage("Naming", "CA1822: Mark members as static", Justification = "Compiler bug")]
internal static class Utf8JsonWriterExts
{
    extension(Utf8JsonWriter @this)
    {
        public void WriteValue(object? value)
        {
            var node = ValueTypeExts.ToNewJsonNode(value, new JsonNodeOptions());
            if (node == null)
                @this.WriteNullValue();
            else
                node.WriteTo(@this);
        }
    }
}

// Copied from Alba.Json.Dynamic

[SuppressMessage("Naming", "CA1822: Mark members as static", Justification = "Compiler bug")]
internal static class JsonElementExts
{
    private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;

    extension(in JsonElement @this)
    {
        public ReadOnlySpan<byte> RawValueSpan => JsonMarshal.GetRawUtf8Value(@this);

        public static object? ToValue(in JsonElement el, JNodeOptions options) =>
            el.ValueKind switch {
                JsonValueKind.Undefined => null,
                JsonValueKind.Null => null,
                JsonValueKind.String => el.GetString(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Number => JsonElement.ToNumber(el, options),
                JsonValueKind.Object or JsonValueKind.Array =>
                    throw new InvalidOperationException($"{el.ValueKind} JsonElement in JsonValue"),
                _ => throw new InvalidOperationException($"Unexpected JsonValueKind of JsonElement: {el.ValueKind}"),
            };

        public static object ToNumber(in JsonElement el, JNodeOptions options) =>
            IsFloatingPoint(el.RawValueSpan) ?
                JsonElement.ToNumberType(el, options.FloatTypes) ??
                throw new InvalidOperationException($"Cannot convert {el} to a floating point number.") :
                JsonElement.ToNumberType(el, options.IntegerTypes) ??
                throw new InvalidOperationException($"Cannot convert {el} to an integer number.");

        private static object? ToNumberType(JsonElement el, NumberType[] types) =>
            types.Select(t => JsonElement.ToNumberType(el, t)).FirstOrDefault(v => v != null);

        [SuppressMessage("ReSharper", "SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault", Justification = "Intentional")]
        private static object? ToNumberType(in JsonElement el, NumberType type) =>
            type switch {
                NumberType.SByte => el.TryGetSByte(out var v) ? v : null,
                NumberType.Byte => el.TryGetByte(out var v) ? v : null,
                NumberType.Int16 => el.TryGetInt16(out var v) ? v : null,
                NumberType.UInt16 => el.TryGetUInt16(out var v) ? v : null,
                NumberType.Int32 => el.TryGetInt32(out var v) ? v : null,
                NumberType.UInt32 => el.TryGetUInt32(out var v) ? v : null,
                NumberType.Int64 => el.TryGetInt64(out var v) ? v : null,
                NumberType.UInt64 => el.TryGetUInt64(out var v) ? v : null,
                NumberType.Single => el.TryGetSingle(out var v) ? v : null,
                NumberType.Double => el.TryGetDouble(out var v) ? v : null,
                NumberType.Decimal => el.TryGetDecimal(out var v) ? v : null,
                NumberType.Half => Half.TryParse(el.RawValueSpan, Invariant, out var v) ? v : null,
                NumberType.Int128 => Int128.TryParse(el.RawValueSpan, Invariant, out var v) ? v : null,
                NumberType.UInt128 => UInt128.TryParse(el.RawValueSpan, Invariant, out var v) ? v : null,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, $"A number or string NumberKind expected, got {type}"),
            };

        [SuppressMessage("Style", "IDE0051", Justification = "C #14 Bug")]
        private static bool IsFloatingPoint(ReadOnlySpan<byte> span) =>
            span.IndexOfAny((byte)'.', (byte)'e', (byte)'E') switch {
                -1 => false,
                var i => span[i] == (byte)'.' || i + 1 < span.Length && span[i + 1] == (byte)'-',
            };
    }
}

internal static class ValueTypeExts
{
    private static readonly JsonValue NoJsonValue = Ensure.NotNull(JsonValue.Create("<NOVALUE>"));

    public static JsonNode? ToNewJsonNode<T>(T value, JsonNodeOptions? opts) =>
        ToJsonValueNode(value, out var valueNode, opts)
            ? valueNode
            : value switch {
                // already JsonNode
                JsonNode v => v,
                // element is always stored as JsonValueOfElement
                JsonElement v => JsonValue.Create(v, opts),
                // serialize everything else into node
                JsonDocument v => JsonSerializer.SerializeToNode(v),
                _ => JsonSerializer.SerializeToNode(value),
            };

    public static bool ToJsonValueNode<T>(T value,
        [NotNullIfNotNull(nameof(value))] out JsonValue? valueNode, JsonNodeOptions? opts)
    {
        valueNode = value switch {
            null => null,
            // already JsonValue
            JsonValue v => v,
            // types stored as JsonValuePrimitive<T> (explicit ctor)
            bool v => JsonValue.Create(v, opts),
            char v => JsonValue.Create(v, opts),
            sbyte v => JsonValue.Create(v, opts),
            byte v => JsonValue.Create(v, opts),
            short v => JsonValue.Create(v, opts),
            ushort v => JsonValue.Create(v, opts),
            int v => JsonValue.Create(v, opts),
            uint v => JsonValue.Create(v, opts),
            long v => JsonValue.Create(v, opts),
            ulong v => JsonValue.Create(v, opts),
            float v => JsonValue.Create(v, opts),
            double v => JsonValue.Create(v, opts),
            decimal v => JsonValue.Create(v, opts),
            DateTime v => JsonValue.Create(v, opts),
            DateTimeOffset v => JsonValue.Create(v, opts),
            Guid v => JsonValue.Create(v, opts),
            // types stored as JsonValuePrimitive<T> (no explicit ctor)
            TimeSpan or Uri or Version => JsonValue.Create(value, opts),
            Half => JsonValue.Create(value, opts),
            DateOnly or TimeOnly => JsonValue.Create(value, opts),
            Int128 or UInt128 => JsonValue.Create(value, opts),
            // convert to JsonValueOfElement if it's a value
            JsonElement el => el.ValueKind is not (JsonValueKind.Object or JsonValueKind.Array)
                ? JsonValue.Create(el, opts)
                : NoJsonValue,
            // everything else isn't a value
            _ => NoJsonValue,
        };
        return !ReferenceEquals(valueNode, NoJsonValue);
    }
}

internal sealed class JNodeOptions
{
    internal static readonly JNodeOptions Default = new();
    public NumberType[] IntegerTypes { get; set; } = [ NumberType.Int32, NumberType.Int64, NumberType.UInt64, NumberType.Decimal ];
    public NumberType[] FloatTypes { get; set; } = [ NumberType.Double, NumberType.Decimal ];
}

internal enum NumberType
{
    SByte = C.SByte,
    Byte = C.Byte,
    Int16 = C.Int16,
    UInt16 = C.UInt16,
    Int32 = C.Int32,
    UInt32 = C.UInt32,
    Int64 = C.Int64,
    UInt64 = C.UInt64,
    Single = C.Single,
    Double = C.Double,
    Decimal = C.Decimal,
    Half = 101,
    Int128 = 102,
    UInt128 = 103,
}