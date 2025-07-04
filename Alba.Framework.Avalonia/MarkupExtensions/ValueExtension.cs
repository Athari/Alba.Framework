using Avalonia.Markup.Xaml;

namespace Alba.Framework.Avalonia.MarkupExtensions;

public abstract class ValueExtension<T>(T value) : MarkupExtension
{
    public T Value { get; set; } = value;
    public override object ProvideValue(IServiceProvider serviceProvider) => Value!;
}

public class Int32Extension(int value) : ValueExtension<int>(value);

public class DoubleExtension(double value) : ValueExtension<double>(value);

public class StringExtension(string value) : ValueExtension<string>(value);