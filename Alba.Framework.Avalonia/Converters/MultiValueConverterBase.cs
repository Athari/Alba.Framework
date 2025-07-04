using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;

namespace Alba.Framework.Avalonia.Converters;

using static MultiValue;

public abstract class MultiValueParamConverterBase<T1, T2, TResult, TParam> : MarkupExtension, IMultiValueConverter
{
    object? IMultiValueConverter.Convert(IList<object?> v, Type targetType, object? parameter, CultureInfo culture) =>
        Convert(C<T1>(v[0]), C<T2>(v[1]), targetType, (TParam)parameter!, culture);

    public abstract TResult Convert(T1 a1, T2 a2, Type targetType, TParam parameter, CultureInfo culture);

    public sealed override object ProvideValue(IServiceProvider serviceProvider) => this;
}

public abstract class MultiValueConverterBase<T1, T2, TResult> : MarkupExtension, IMultiValueConverter
{
    object? IMultiValueConverter.Convert(IList<object?> v, Type targetType, object? parameter, CultureInfo culture) =>
        Convert(C<T1>(v[0]), C<T2>(v[1]), targetType, culture);

    public abstract TResult Convert(T1 a1, T2 a2, Type targetType, CultureInfo culture);

    public sealed override object ProvideValue(IServiceProvider serviceProvider) => this;
}