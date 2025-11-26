using System.Linq.Expressions;
using Alba.Framework.Text;

namespace Alba.Framework.Reflection;

public static class NameOf
{
    private const StringComparison Ord = StringComparison.Ordinal;

    // instance property getter
    public static string Prop<T, TProp>(Expression<Func<T, TProp>> expr) =>
        GetName(expr.Body);

    // static property getter
    public static string Prop<TProp>(Expression<Func<TProp>> expr) =>
        GetName(expr.Body);

    // property getter/setter
    public static string Prop(LambdaExpression expr) =>
        GetName(expr.Body);

    // instance dependency property field
    public static string DepProp<TProp>(Expression<Func<TProp>> expr) =>
        GetName(expr.Body).RemovePostfix("Property", Ord);

    // nameof dependency property field
    public static string DepProp(string name) =>
        name.RemovePostfix("Property", Ord);

    // static attached dependency property getter
    public static string AttachedProp<TProp>(Expression<Func<TProp>> expr) =>
        GetName(expr.Body).RemovePrefix("Get", Ord);

    public static string AttachedProp(LambdaExpression expr) =>
        GetName(expr.Body).RemovePrefix("Get", Ord);

    private static string GetName(Expression expr) =>
        expr switch {
            UnaryExpression unaryExpr => unaryExpr.Operand,
            _ => expr,
        } switch {
            MemberExpression memberExpr => memberExpr.Member.Name,
            MethodCallExpression callExpr => callExpr.Method.Name,
            _ => throw new ArgumentException($"Unexpected expr type: {expr.GetType().Name}"),
        };
}