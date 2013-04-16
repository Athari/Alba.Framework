using System;
using System.Collections.Generic;
using System.Linq;
using Alba.Framework.Collections;
using Alba.Framework.Text;

namespace Alba.Framework.Sys
{
    public static class TypeExts
    {
        public static IEnumerable<Type> GetConcreteSubtypes (this Type @this, IEnumerable<Type> types = null)
        {
            if (types == null)
                types = @this.Assembly.GetTypes();
            return @this.IsClass
                ? types.Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(@this))
                : types.Where(t => t.IsClass && !t.IsAbstract && @this.IsAssignableFrom(t));
        }

        public static string GetFullSharpName (this Type @this)
        {
            string name = @this.FullName;
            int genericGrave = name.IndexOf('`');
            if (genericGrave != -1) {
                name = "{0}<{1}>".Fmt(
                    name.Sub(0, genericGrave),
                    @this.GenericTypeArguments.Select(GetFullSharpName).JoinString(", "));
            }
            return name;
        }

        public static bool IsAssignableTo (this Type @this, Type type)
        {
            return type.IsAssignableFrom(@this);
        }
    }
}