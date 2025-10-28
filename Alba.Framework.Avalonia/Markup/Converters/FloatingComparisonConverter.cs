using System.ComponentModel;
using System.Globalization;
using System.Numerics;
using Alba.Framework.Numerics;

namespace Alba.Framework.Avalonia.Markup.Converters;

[PublicAPI]
public abstract class FloatingComparisonConverter<T, TEpsilonOperations> : ValueConverterBase<T?, bool, T?>
    where T : IFloatingPoint<T>
    where TEpsilonOperations : IFloatingPointEpsilonOperations<T>
{
    public NumberComparisonOperator Operator { get; set; }

    protected FloatingComparisonConverter() { }

    protected FloatingComparisonConverter(NumberComparisonOperator @operator) => Operator = @operator;

    public override bool Convert(T? value, Type targetType, T? parameter, CultureInfo culture) =>
        value is not null && parameter is not null && Operator switch {
            NumberComparisonOperator.Equal =>
                TEpsilonOperations.IsCloseTo(value, parameter, TEpsilonOperations.Epsilon),
            NumberComparisonOperator.NotEqual =>
                !TEpsilonOperations.IsCloseTo(value, parameter, TEpsilonOperations.Epsilon),
            NumberComparisonOperator.Greater =>
                TEpsilonOperations.IsGreaterThan(value, parameter),
            NumberComparisonOperator.GreaterOrEqual =>
                TEpsilonOperations.IsGreaterThanOrClose(value, parameter),
            NumberComparisonOperator.Less =>
                TEpsilonOperations.IsLessThan(value, parameter),
            NumberComparisonOperator.LessOrEqual =>
                TEpsilonOperations.IsLessThanOrClose(value, parameter),
            _ => throw new InvalidEnumArgumentException($"Invalid {nameof(Operator)} value."),
        };
}