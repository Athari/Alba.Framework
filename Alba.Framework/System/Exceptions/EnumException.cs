using Alba.Framework.Reflection;

namespace Alba.Framework;

public static class EnumException
{
    public static EnumException<T> Create<T>([InvokerParameterName] string? paramName, T actualValue, string? message = null)
        => new(paramName, actualValue, message);

    public static EnumException<T> Create<T>([InvokerParameterName] string? paramName, string? message = null)
        => new(paramName, message);
}

public class EnumException<T> : ArgumentOutOfRangeException
{
    public EnumException() { }

    public EnumException(string? message, Exception? innerException) : base(message, innerException) { }

    public EnumException([InvokerParameterName] string? paramName, T actualValue, string? message = null)
        : base(paramName, actualValue, message ?? FormatDefaultMessage(actualValue)) { }

    public EnumException([InvokerParameterName] string? paramName, string? message)
        : base(paramName, message ?? FormatDefaultMessage()) { }

    private static string FormatDefaultMessage(object? actualValue = null) =>
        $"{actualValue ?? "Argument"} is not a valid {typeof(T).GetFullName()}";
}