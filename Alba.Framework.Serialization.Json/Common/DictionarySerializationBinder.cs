using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Alba.Framework.Collections;
using Alba.Framework.Reflection;
using Alba.Framework.Text;
using Newtonsoft.Json;

// ReSharper disable LoopCanBeConvertedToQuery
namespace Alba.Framework.Serialization.Json
{
    public class DictionarySerializationBinder : SerializationBinder
    {
        private readonly Dictionary<Type, BiDictionary<Type, string>> _baseTypeToTypeToName = new Dictionary<Type, BiDictionary<Type, string>>();

        public DictionarySerializationBinder Add (Type baseType, Type type, string name = null)
        {
            return Add(baseType, type, name, false);
        }

        public DictionarySerializationBinder AddGlobal (Type type, string name = null)
        {
            return Add(typeof(object), type, name, false);
        }

        public DictionarySerializationBinder AddSubtypes (Type type, bool ignoreUnnamed = false)
        {
            foreach (Type subtype in type.GetConcreteSubtypes())
                Add(type, subtype, null, ignoreUnnamed);
            return this;
        }

        public DictionarySerializationBinder AddSubtypes (Type baseType, Type type, bool ignoreUnnamed = false)
        {
            foreach (Type subtype in type.GetConcreteSubtypes())
                Add(baseType, subtype, null, ignoreUnnamed);
            return this;
        }

        public DictionarySerializationBinder AddSubtypesGlobal (Type type, bool ignoreUnnamed = false)
        {
            foreach (Type subtype in type.GetConcreteSubtypes())
                Add(typeof(object), subtype, null, ignoreUnnamed);
            return this;
        }

        public DictionarySerializationBinder Add<TBase, T> (string name = null)
        {
            return Add(typeof(TBase), typeof(T), name, false);
        }

        public DictionarySerializationBinder AddGlobal<T> (string name = null)
        {
            return Add(typeof(object), typeof(T), name, false);
        }

        public DictionarySerializationBinder AddSubtypes<T> (bool ignoreUnnamed = false)
        {
            return AddSubtypes(typeof(T), ignoreUnnamed);
        }

        public DictionarySerializationBinder AddSubtypes<TBase, T> (bool ignoreUnnamed = false)
        {
            return AddSubtypes(typeof(TBase), typeof(T), ignoreUnnamed);
        }

        public DictionarySerializationBinder AddSubtypesGlobal<T> (bool ignoreUnnamed = false)
        {
            return AddSubtypesGlobal(typeof(T), ignoreUnnamed);
        }

        private DictionarySerializationBinder Add (Type baseType, Type type, string name, bool ignoreUnnamed)
        {
            name = GetName(type, name);
            if (name != null)
                _baseTypeToTypeToName.GetOrAdd(baseType, () => new BiDictionary<Type, string>()).Add(type, name);
            else if (!ignoreUnnamed)
                throw new ArgumentException("Name for type '{0}' not specified. Name must be specified either directly or by JsonObjectAttribute.Id.".Fmt(type), "name");
            return this;
        }

        private static string GetName (Type type, string name)
        {
            if (name != null) 
                return name;
            var attr = type.GetCustomAttribute<JsonObjectAttribute>();
            return attr == null || attr.Id == null ? null : attr.Id;
        }

        public override Type BindToType (string assemblyName, string typeName)
        {
            return ContextualBindToType(typeof(object), assemblyName, typeName);
        }

        public Type ContextualBindToType (Type baseType, string assemblyName, string typeName)
        {
            if (!assemblyName.IsNullOrEmpty())
                throw new InvalidOperationException("Type '{1}' (assembly={0}) not defined. Using types from arbitrary assemblies not allowed.".Fmt(typeName, assemblyName));
            Type serializedType;
            BiDictionary<Type, string> typeToName;
            foreach (Type type in baseType.GetInterfaces().Concat(baseType.TraverseList(t => t.BaseType)))
                if (_baseTypeToTypeToName.TryGetValue(type, out typeToName) && typeToName.Reverse.TryGetValue(typeName, out serializedType))
                    return serializedType;
            throw new KeyNotFoundException("Type with id '{0}' not registered.".Fmt(typeName));
        }

        public override void BindToName (Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            BiDictionary<Type, string> typeToName;
            foreach (Type type in serializedType.GetInterfaces().Concat(serializedType.TraverseList(t => t.BaseType)))
                if (_baseTypeToTypeToName.TryGetValue(type, out typeToName) && typeToName.TryGetValue(serializedType, out typeName))
                    return;
            typeName = null;
            //throw new KeyNotFoundException("Type '{0}' not registered.".Fmt(serializedType.GetFullSharpName()));
        }
    }
}