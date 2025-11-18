using Alba.Framework.Avalonia.Markup.Converters;

namespace Alba.Framework.Avalonia.Json;

public class JsonTypedAvaloniaConverterRef<T, TRepr, TConverter>
    (TConverter? converter = null)
    : JsonAvaloniaConverterRef<T, TRepr, TConverter>(converter)
    where TConverter : ValueConverterBase<T, TRepr>, new();