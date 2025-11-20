namespace Alba.Framework.Avalonia.Markup.MarkupExtensions;

public abstract class ValueExtension<T>(T value) : IMarkupExtension
{
    public T Value { get; set; } = value;

    public object ProvideValue(IServiceProvider serviceProvider) => Value!;
}

public class BoolExtension(bool value) : ValueExtension<bool>(value);

public class Int32Extension(int value) : ValueExtension<int>(value);

public class DoubleExtension(double value) : ValueExtension<double>(value);

public class StringExtension(string value) : ValueExtension<string>(value);