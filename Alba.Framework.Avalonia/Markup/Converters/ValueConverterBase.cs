using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;

namespace Alba.Framework.Avalonia.Markup.Converters;

public class FunctionValueConverter<TFrom, TTo>(Func<TFrom, TTo> fn) : ValueConverterBase<TFrom, TTo>
{
    public override TTo Convert(TFrom value, Type targetType, CultureInfo culture) => fn(value);
}

public class FunctionValueConverter<TFrom, TTo, TParam>(Func<TFrom, TParam, TTo> fn) : ValueConverterBase<TFrom, TTo, TParam>
{
    public override TTo Convert(TFrom value, Type targetType, TParam parameter, CultureInfo culture) => fn(value, parameter);
}

public abstract class ValueConverterBase<TFrom, TTo, TParam> : MarkupExtension, IValueConverter
{
    object? IValueConverter.Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        Convert((TFrom)value!, targetType, (TParam)parameter!, culture);

    object? IValueConverter.ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        ConvertBackOptional((TTo)value!, targetType, (TParam)parameter!, culture).Value;

    public abstract TTo Convert(TFrom value, Type targetType, TParam parameter, CultureInfo culture);

    public virtual ConversionResult<TFrom> ConvertBackOptional(TTo value, Type targetType, TParam parameter, CultureInfo culture) =>
        new(ConvertBack(value, targetType, parameter, culture));

    public virtual TFrom ConvertBack(TTo value, Type targetType, TParam parameter, CultureInfo culture) =>
        throw new NotSupportedException($"${GetType().Name} doesn't support back conversion");

    public sealed override object ProvideValue(IServiceProvider serviceProvider) => this;
}

public abstract class ValueConverterBase<TFrom, TTo> : MarkupExtension, IValueConverter
{
    object? IValueConverter.Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        Convert((TFrom)value!, targetType, culture);

    object? IValueConverter.ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        ConvertBackOptional((TTo)value!, targetType, culture).Value;

    public abstract TTo Convert(TFrom value, Type targetType, CultureInfo culture);

    public virtual ConversionResult<TFrom> ConvertBackOptional(TTo value, Type targetType, CultureInfo culture) =>
        new(ConvertBack(value, targetType, culture));

    public virtual TFrom ConvertBack(TTo value, Type targetType, CultureInfo culture) =>
        throw new NotSupportedException($"${GetType().Name} doesn't support back conversion");

    public sealed override object ProvideValue(IServiceProvider serviceProvider) => this;
}

public struct ConversionResult<T>
{
    public object? Value { get; }

    public ConversionResult(T value) => Value = value;
    public ConversionResult(object value) => Value = value;
}