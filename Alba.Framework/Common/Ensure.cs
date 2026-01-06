using System.Runtime.CompilerServices;
using MethodAttribute = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Alba.Framework.Common;

public static class Ensure
{
    private const MethodImplOptions Inline = MethodImplOptions.AggressiveInlining;

    [Method(Inline), ContractAnnotation("param:null => halt")]
    public static T NotNull<T>([NotNull] T? param,
        [InvokerParameterName, CallerArgumentExpression(nameof(param))] string? paramName = null)
    {
        if (param is not null)
            return param;
        throw new ArgumentNullException(paramName, $"Argument {paramName} must not be null");
    }

    [Method(Inline), ContractAnnotation("param:null => halt")]
    public static T NotNull<T>([NotNull] T? param,
        [InvokerParameterName, CallerArgumentExpression(nameof(param))] string? paramName = null)
        where T : struct
    {
        if (param is not null)
            return param.Value;
        throw new ArgumentNullException(paramName, $"Argument {paramName} must not be null");
    }

    [Method(Inline)]
    public static T OfTypeOrNullable<T>(object? param,
        [InvokerParameterName, CallerArgumentExpression(nameof(param))] string? paramName = null)
    {
        if (default(T) is null && param is null)
            return default!;
        if (param is T p)
            return p;
        throw new ArgumentException($"Argument {nameof(param)} must be of type {typeof(T)} and not null", paramName);
    }

    [Method(Inline)]
    public static bool TryOfTypeOrNullable<T>(object? param, out T result)
    {
        if (default(T) is null && param is null) {
            result = default!;
            return true;
        }
        else if (param is T p) {
            result = p;
            return true;
        }
        else {
            result = default!;
            return false;
        }
    }

    [Method(Inline)]
    public static bool IfOfTypeOrNullable<T>(object? param, Action<T> action)
    {
        if (default(T) is null && param is null) {
            action(default!);
            return true;
        }
        else if (param is T p) {
            action(p);
            return true;
        }
        else {
            return false;
        }
    }

    [Method(Inline)]
    public static T OfType<T>(object? param,
        [InvokerParameterName, CallerArgumentExpression(nameof(param))] string? paramName = null)
    {
        if (param is T p)
            return p;
        throw new ArgumentException($"Argument {nameof(param)} must be of type {typeof(T)}", paramName);
    }

    [Method(Inline)]
    public static bool TryOfType<T>(object? param, out T result)
    {
        if (param is T p) {
            result = p;
            return true;
        }
        else {
            result = default!;
            return false;
        }
    }

    [Method(Inline)]
    public static bool IfOfType<T>(object? param, Action<T> action)
    {
        if (param is T p) {
            action(p);
            return true;
        }
        else {
            return false;
        }
    }

    [Method(Inline)]
    public static int GreaterThanOrEqualTo(int param, int value,
        [InvokerParameterName, CallerArgumentExpression(nameof(param))] string? paramName = null)
    {
        if (param >= value)
            return param;
        throw new ArgumentOutOfRangeException(paramName, param, $"Argument {paramName} must be greater than our equal to ${value}.");
    }

    [Method(Inline)]
    public static int LessThanOrEqualTo(int param, int value,
        [InvokerParameterName, CallerArgumentExpression(nameof(param))] string? paramName = null)
    {
        if (param <= value)
            return param;
        throw new ArgumentOutOfRangeException(paramName, param, $"Argument {paramName} must be less than our equal to ${value}.");
    }

    [Method(Inline)]
    public static T[] Count<T>(T[] param, int value,
        [InvokerParameterName, CallerArgumentExpression(nameof(param))] string? paramName = null)
    {
        if (param.Length == value)
            return param;
        throw new ArgumentOutOfRangeException(paramName, param, $"Argument {paramName} must contain ${value} elements.");
    }
}