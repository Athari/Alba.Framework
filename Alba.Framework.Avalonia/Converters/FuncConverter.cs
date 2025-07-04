using System.Globalization;

namespace Alba.Framework.Avalonia.Converters;

public class FuncConverter<T, TResult>(Func<T, TResult> func) : ValueConverterBase<T, TResult>
{
    public override TResult Convert(T value, Type targetType, CultureInfo culture) => func(value);
}

public static class FuncConverter
{
    public static FuncConverter<T, TResult> Create<T, TResult>(Func<T, TResult> func) => new(func);

    public static MultiFuncConverter<T1, T2, TTo> CreateMulti<T1, T2, TTo>(Func<T1, T2, TTo> func) => new(func);
}

public class MultiFuncConverter<T1, T2, TResult>(Func<T1, T2, TResult> func) : MultiValueConverterBase<T1, T2, TResult>
{
    public override TResult Convert(T1 a1, T2 a2, Type targetType, CultureInfo culture) => func(a1, a2);
}