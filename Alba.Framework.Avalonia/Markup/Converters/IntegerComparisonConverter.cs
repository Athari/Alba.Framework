using System.ComponentModel;
using System.Globalization;
using System.Numerics;

namespace Alba.Framework.Avalonia.Markup.Converters;

[PublicAPI]
public abstract class IntegerComparisonConverter<T> : ValueConverterBase<T?, bool, T?>
    where T : IBinaryInteger<T>
{
    public NumberComparisonOperator Operator { get; set; }

    protected IntegerComparisonConverter() { }

    protected IntegerComparisonConverter(NumberComparisonOperator @operator) => Operator = @operator;

    public override bool Convert(T? value, Type targetType, T? parameter, CultureInfo culture) =>
        value is not null && parameter is not null && Operator switch {
            NumberComparisonOperator.Equal =>
                value == parameter,
            NumberComparisonOperator.NotEqual =>
                value != parameter,
            NumberComparisonOperator.Greater =>
                value > parameter,
            NumberComparisonOperator.GreaterOrEqual =>
                value >= parameter,
            NumberComparisonOperator.Less =>
                value < parameter,
            NumberComparisonOperator.LessOrEqual =>
                value <= parameter,
            _ => throw new InvalidEnumArgumentException($"Invalid {nameof(Operator)} value."),
        };
}