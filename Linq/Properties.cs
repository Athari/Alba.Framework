using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows;

namespace Alba.Framework.Linq
{
    public static class Properties
    {
        public static string GetName<TObj, TProp> (Expression<Func<TObj, TProp>> expr)
        {
            return ((MemberExpression)expr.Body).Member.Name;
        }

        public static string GetName<TProp> (Expression<Func<TProp>> expr)
        {
            return ((MemberExpression)expr.Body).Member.Name;
        }

        public static T SetDpValue<T> (DependencyObject d, string propName, T value)
        {
            Type type = d.GetType();
            DependencyPropertyDescriptor.FromName(propName, type, type).SetValue(d, value);
            return value;
        }

        public static T GetDpValue<T> (DependencyObject d, string propName)
        {
            Type type = d.GetType();
            return (T)DependencyPropertyDescriptor.FromName(propName, type, type).GetValue(d);
        }

        public static T SetValue<T> (object o, string propName, T value)
        {
            o.GetType().GetProperty(propName).SetValue(o, value);
            return value;
        }

        public static T GetValue<T> (object o, string propName)
        {
            return (T)o.GetType().GetProperty(propName).GetValue(o);
        }
    }
}