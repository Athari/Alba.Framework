using System.ComponentModel;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;

namespace Alba.Framework.Avalonia.Markup.Converters;

public class MultiBoolConverter : MarkupExtension, IMultiValueConverter
{
    public BinaryBoolOperator Operator { get; set; }

    public MultiBoolConverter() { }

    public MultiBoolConverter(BinaryBoolOperator @operator) =>
        Operator = @operator;

    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture) =>
        Operator switch {
            BinaryBoolOperator.And => values.Select(ToBool).All(v => v),
            BinaryBoolOperator.Or => values.Select(ToBool).Any(v => v),
            BinaryBoolOperator.Xor => values.Select(ToBool).Aggregate(false, (a, v) => a ^ v),
            _ => throw new InvalidEnumArgumentException($"Invalid {nameof(Operator)} value."),
        };

    private static bool ToBool(object? v) =>
        v is bool b ? b : v != AvaloniaProperty.UnsetValue && v is not null && System.Convert.ToBoolean(v);

    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}