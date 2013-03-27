using System;
using System.Collections.Generic;
using System.Linq;

namespace Alba.Framework.Sys
{
    public static class TypeExts
    {
        public static IEnumerable<Type> GetConcreteSubtypes (this Type @this)
        {
            return @this.Assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(@this));
        }
    }
}