using System.Linq.Expressions;

namespace Alba.Framework.Reflection;

public static class PropertyName
{
    public static string Of<T, TProp>(Expression<Func<T, TProp>> expr) =>
        GetName(expr.Body);

    public static string Of<TProp>(Expression<Func<TProp>> expr) =>
        GetName(expr.Body);

    public static string Of(LambdaExpression expr) =>
        GetName(expr.Body);

    private static string GetName(Expression expr) =>
        ((MemberExpression)((expr as UnaryExpression)?.Operand ?? expr)).Member.Name;
}