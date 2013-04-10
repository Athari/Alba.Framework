using System;
using System.Collections.Generic;
using System.Reflection;
using Alba.Framework.Collections;
using Alba.Framework.Sys;
using Alba.Framework.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

// ReSharper disable LoopCanBeConvertedToQuery
namespace Alba.Framework.Serialization
{
    public class DictionarySerializationBinder : DefaultSerializationBinder
    {
        private readonly Dictionary<Type, BiDictionary<Type, string>> _baseTypeToTypeToName = new Dictionary<Type, BiDictionary<Type, string>>();

        public DictionarySerializationBinder Add (Type baseType, Type type, string name = null)
        {
            name = GetName(type, name);
            _baseTypeToTypeToName.GetOrAdd(baseType, () => new BiDictionary<Type, string>()).Add(type, name);
            return this;
        }

        public DictionarySerializationBinder AddGlobal (Type type, string name = null)
        {
            Add(typeof(object), type, name);
            return this;
        }

        public DictionarySerializationBinder AddSubtypes (Type type)
        {
            foreach (Type subtype in type.GetConcreteSubtypes())
                Add(type, subtype);
            return this;
        }

        public DictionarySerializationBinder AddSubtypes (Type baseType, Type type)
        {
            foreach (Type subtype in type.GetConcreteSubtypes())
                Add(baseType, subtype);
            return this;
        }

        public DictionarySerializationBinder AddSubtypesGlobal (Type type)
        {
            foreach (Type subtype in type.GetConcreteSubtypes())
                Add(typeof(object), subtype);
            return this;
        }

        public DictionarySerializationBinder Add<TBase, T> (string name = null)
        {
            return Add(typeof(TBase), typeof(T), name);
        }

        public DictionarySerializationBinder AddGlobal<T> (string name = null)
        {
            return Add(typeof(object), typeof(T), name);
        }

        public DictionarySerializationBinder AddSubtypes<T> ()
        {
            return AddSubtypes(typeof(T));
        }

        public DictionarySerializationBinder AddSubtypes<TBase, T> ()
        {
            return AddSubtypes(typeof(TBase), typeof(T));
        }

        public DictionarySerializationBinder AddSubtypesGlobal<T> ()
        {
            return AddSubtypesGlobal(typeof(T));
        }

        private string GetName (Type type, string name)
        {
            if (name == null) {
                var attr = type.GetCustomAttribute<JsonObjectAttribute>();
                if (attr == null || attr.Id == null)
                    throw new ArgumentException("Name for type '{0}' not specified. Name must be specified either directly or by JsonObjectAttribute.Id.".Fmt(type), "name");
                return attr.Id;
            }
            return name;
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
            foreach (Type type in baseType.TraverseList(t => t.BaseType))
                if (_baseTypeToTypeToName.TryGetValue(type, out typeToName) && typeToName.Reverse.TryGetValue(typeName, out serializedType))
                    return serializedType;
            throw new KeyNotFoundException("Type '{0}' not defined.".Fmt(typeName));
        }

        public override void BindToName (Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            BiDictionary<Type, string> typeToName;
            foreach (Type type in serializedType.TraverseList(t => t.BaseType))
                if (_baseTypeToTypeToName.TryGetValue(type, out typeToName) && typeToName.TryGetValue(serializedType, out typeName))
                    return;
            typeName = null;
        }
    }
}