using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace Alba.Framework.Avalonia.Markup.Converters;

public static class MultiValue
{
    public static T C<T>(object? value) => value is UnsetValueType ? default! : (T)value!;
}

public record class MultiValue<T, T1> : IMultiValueConverter
{
    public required T Context { get; init; }
    public required T1 Arg1 { get; init; }

    public object Convert(IList<object?> v, Type targetType, object? parameter, CultureInfo culture) =>
        new MultiValue<T, T1> { Context = MultiValue.C<T>(v[0]), Arg1 = MultiValue.C<T1>(v[1]) };

    public MultiValue<T, T1> ProvideValue() => this;
}

public record class MultiValue<T, T1, T2> : IMultiValueConverter
{
    public required T Context { get; init; }
    public required T1 Arg1 { get; init; }
    public required T2 Arg2 { get; init; }

    public object Convert(IList<object?> v, Type targetType, object? parameter, CultureInfo culture) =>
        new MultiValue<T, T1, T2> { Context = MultiValue.C<T>(v[0]), Arg1 = MultiValue.C<T1>(v[1]), Arg2 = MultiValue.C<T2>(v[2]) };

    public MultiValue<T, T1, T2> ProvideValue() => this;
}

public record class MultiValue<T, T1, T2, T3> : IMultiValueConverter
{
    public required T Context { get; init; }
    public required T1 Arg1 { get; init; }
    public required T2 Arg2 { get; init; }
    public required T3 Arg3 { get; init; }

    public object Convert(IList<object?> v, Type targetType, object? parameter, CultureInfo culture) =>
        new MultiValue<T, T1, T2, T3> { Context = MultiValue.C<T>(v[0]), Arg1 = MultiValue.C<T1>(v[1]), Arg2 = MultiValue.C<T2>(v[2]), Arg3 = MultiValue.C<T3>(v[3]) };

    public MultiValue<T, T1, T2, T3> ProvideValue() => this;
}