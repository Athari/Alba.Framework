using System;
using System.Runtime.Serialization;
using Alba.Framework.Text;

namespace Alba.Framework.Serialization.Json
{
    public static class SerializationBinderExts
    {
        public static string BindToName (this SerializationBinder @this, Type serializedType)
        {
            string assemblyName;
            string typeName;
            @this.BindToName(serializedType, out assemblyName, out typeName);
            return assemblyName.IsNullOrEmpty() ? typeName : "{0}, {1}".Fmt(typeName, assemblyName);
        }

        public static Type BindToType (this SerializationBinder @this, string typeName)
        {
            return @this.BindToType("", typeName);
        }

        public static Type BindToType (this SerializationBinder @this, Type baseType, string typeName)
        {
            var contextualBinder = @this as DictionarySerializationBinder;
            return contextualBinder != null ? contextualBinder.ContextualBindToType(baseType, "", typeName) : @this.BindToType("", typeName);
        }
    }
}