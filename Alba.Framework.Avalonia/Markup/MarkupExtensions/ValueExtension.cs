namespace Alba.Framework.Avalonia.Markup.MarkupExtensions;

[PublicAPI]
public abstract class ValueExtension<T>(T value) : IMarkupExtension
{
    public T Value { get; set; } = value;

    public object ProvideValue(IServiceProvider serviceProvider) => Value!;
}

[PublicAPI]
public class Int32Extension(int value) : ValueExtension<int>(value);

[PublicAPI]
public class DoubleExtension(double value) : ValueExtension<double>(value);

[PublicAPI]
public class StringExtension(string value) : ValueExtension<string>(value);