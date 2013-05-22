using System;
using System.Collections.Generic;
using System.Linq;
using Alba.Framework.Collections;
using Alba.Framework.Text;

namespace Alba.Framework.Sys
{
    public static class TypeExts
    {
        /// <summary>Find non-abstract classes either inheriting from a type or implementing an interface.</summary>
        /// <param name="this">Class or interface.</param>
        /// <param name="types">Types to search within. If not specified, types of the assembly <paramref name="this"/> originates from are searched.</param>
        public static IEnumerable<Type> GetConcreteSubtypes (this Type @this, IEnumerable<Type> types = null)
        {
            if (types == null)
                types = @this.Assembly.GetTypes();
            return @this.IsClass
                ? types.Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(@this))
                : types.Where(t => t.IsClass && !t.IsAbstract && @this.IsAssignableFrom(t));
        }

        /// <summary>Return "medium name" of a type: full name for type, short names for type arguments.</summary>
        public static string GetMediumName (this Type @this)
        {
            string name = @this.FullName;
            int genericGrave = name.IndexOf('`');
            if (genericGrave != -1) {
                name = "{0}<{1}>".Fmt(
                    name.Sub(0, genericGrave),
                    @this.GenericTypeArguments.Select(t => t.Name).JoinString(", "));
            }
            return name;
        }

        /// <summary>Return "full sharp name" of a type: full name for type, full names for type arguments. Unlike <see cref="Type.FullName"/> property, type arguments do not include full assembly info.</summary>
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

        /// <summary>Equivalent to "is" keyword.</summary>
        /// <remarks><see cref="Type.IsAssignableFrom"/> is "reverse is" and as such is confusing.</remarks>
        public static bool IsAssignableTo (this Type @this, Type type)
        {
            return type.IsAssignableFrom(@this);
        }

        public static string GetShortAssemblyName (this Type @this)
        {
            return @this.Assembly.GetName().Name;
        }
    }
}