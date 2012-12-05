using System;
using System.Linq.Expressions;

namespace Alba.Framework.Linq
{
    public static class Expressions
    {
        public static string GetPropertyName<TObj, TProp> (Expression<Func<TObj, TProp>> expr)
        {
            return ((MemberExpression)expr.Body).Member.Name;
        }
    }
}