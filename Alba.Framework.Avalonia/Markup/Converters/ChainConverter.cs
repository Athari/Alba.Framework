using System.Globalization;
using Alba.Framework.Collections;
using Avalonia.Data.Converters;
using Avalonia.Metadata;

namespace Alba.Framework.Avalonia.Markup.Converters;

public class ChainConverter : IValueConverter
{
    [Content]
    public IList<ConverterRef> Converters { get; } = [ ];

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        Converters.Aggregate(value, (v, conv) =>
            conv.Converter.Convert(v, typeof(object), conv.GetParameter(parameter), culture));


    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        Converters.Inverse().Aggregate(value, (v, conv) =>
            conv.Converter.ConvertBack(v, typeof(object), conv.GetParameter(parameter), culture));
}

public class ConverterRef
{
    [Content]
    public required IValueConverter Converter { get; set; }

    public object? Parameter { get; set; }

    public bool PassParameter { get; set; }

    public ConverterRef() { }

    [SetsRequiredMembers]
    public ConverterRef(IValueConverter converter) => Converter = converter;

    internal object? GetParameter(object? chainParameter) => PassParameter ? chainParameter : Parameter;
}