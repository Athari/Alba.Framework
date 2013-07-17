using System;
using System.Diagnostics;
using System.Linq.Expressions;
using Alba.Framework.Diagnostics;
using Alba.Framework.Reflection;

namespace Alba.Framework.Windows.Mvvm
{
    public abstract class ModelBase<TSelf> : ModelBase
        where TSelf : ModelBase<TSelf>
    {
        protected static ILog GetLog (TraceSource traceSource)
        {
            return new Log<TSelf>(traceSource);
        }

        protected static string GetName<T> (Expression<Func<TSelf, T>> propExpr)
        {
            return Props.GetName(propExpr);
        }
    }
}