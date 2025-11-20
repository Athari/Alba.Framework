using System.ComponentModel;
using System.Globalization;
using System.Numerics;

namespace Alba.Framework.Avalonia.Markup.Converters;

public abstract class IntegerComparisonConverter<T> : ValueConverterBase<T?, bool>
    where T : IBinaryInteger<T>
{
    public NumberComparisonOperator Operator { get; set; }

    public T Parameter { get; set; } = T.Zero;

    protected IntegerComparisonConverter() { }

    protected IntegerComparisonConverter(NumberComparisonOperator @operator) =>
        Operator = @operator;

    protected IntegerComparisonConverter(NumberComparisonOperator @operator, T parameter) =>
        (Operator, Parameter) = (@operator, parameter);

    public override bool Convert(T? value, Type targetType, CultureInfo culture) =>
        value is not null && Operator switch {
            NumberComparisonOperator.Equal =>
                value == Parameter,
            NumberComparisonOperator.NotEqual =>
                value != Parameter,
            NumberComparisonOperator.Greater =>
                value > Parameter,
            NumberComparisonOperator.GreaterOrEqual =>
                value >= Parameter,
            NumberComparisonOperator.Less =>
                value < Parameter,
            NumberComparisonOperator.LessOrEqual =>
                value <= Parameter,
            _ => throw new InvalidEnumArgumentException($"Invalid {nameof(Operator)} value."),
        };
}