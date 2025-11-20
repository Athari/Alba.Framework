using System.ComponentModel;
using System.Globalization;
using System.Numerics;
using Alba.Framework.Numerics;

namespace Alba.Framework.Avalonia.Markup.Converters;

public abstract class FloatingComparisonConverter<T, TEpsilonOperations> : ValueConverterBase<T?, bool>
    where T : IFloatingPoint<T>
    where TEpsilonOperations : IFloatingPointEpsilonOperations<T>
{
    public NumberComparisonOperator Operator { get; set; }

    public T Parameter { get; set; } = T.Zero;

    protected FloatingComparisonConverter() { }

    protected FloatingComparisonConverter(NumberComparisonOperator @operator) =>
        Operator = @operator;

    protected FloatingComparisonConverter(NumberComparisonOperator @operator, T parameter) =>
        (Operator, Parameter) = (@operator, parameter);

    public override bool Convert(T? value, Type targetType, CultureInfo culture) =>
        value is not null && Operator switch {
            NumberComparisonOperator.Equal =>
                TEpsilonOperations.IsCloseTo(value, Parameter, TEpsilonOperations.Epsilon),
            NumberComparisonOperator.NotEqual =>
                !TEpsilonOperations.IsCloseTo(value, Parameter, TEpsilonOperations.Epsilon),
            NumberComparisonOperator.Greater =>
                TEpsilonOperations.IsGreaterThan(value, Parameter),
            NumberComparisonOperator.GreaterOrEqual =>
                TEpsilonOperations.IsGreaterThanOrClose(value, Parameter),
            NumberComparisonOperator.Less =>
                TEpsilonOperations.IsLessThan(value, Parameter),
            NumberComparisonOperator.LessOrEqual =>
                TEpsilonOperations.IsLessThanOrClose(value, Parameter),
            _ => throw new InvalidEnumArgumentException($"Invalid {nameof(Operator)} value."),
        };
}