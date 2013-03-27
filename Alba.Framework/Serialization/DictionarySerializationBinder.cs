using System;
using System.Collections.Generic;
using System.Reflection;
using Alba.Framework.Sys;
using Alba.Framework.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

// TODO Local subtypes: name resolution depending on property type
namespace Alba.Framework.Serialization
{
    public class DictionarySerializationBinder : DefaultSerializationBinder
    {
        private readonly IDictionary<Type, string> _typeToName = new Dictionary<Type, string>();
        private readonly IDictionary<string, Type> _nameToType = new Dictionary<string, Type>();

        public DictionarySerializationBinder Add (Type type, string name = null)
        {
            if (name == null) {
                var attr = type.GetCustomAttribute<JsonObjectAttribute>();
                if (attr == null || attr.Id == null)
                    throw new ArgumentException("Name for type '{0}' not specified. Name must be specified either directly or by JsonObjectAttribute.Id.".Fmt(type), "name");
                name = attr.Id;
            }
            _typeToName[type] = name;
            _nameToType[name] = type;
            return this;
        }

        public DictionarySerializationBinder AddSubtypes (Type type)
        {
            foreach (Type subtype in type.GetConcreteSubtypes())
                Add(subtype);
            return this;
        }

        public DictionarySerializationBinder Add<T> (string name = null)
        {
            return Add(typeof(T), name);
        }

        public DictionarySerializationBinder AddSubtypes<T> ()
        {
            return AddSubtypes(typeof(T));
        }

        public override Type BindToType (string assemblyName, string typeName)
        {
            if (!assemblyName.IsNullOrEmpty())
                throw new InvalidOperationException(string.Format("Type '{1}' (assembly={0}) not found.",
                    typeName, assemblyName));
            try {
                return _nameToType[typeName];
            }
            catch (KeyNotFoundException e) {
                throw new KeyNotFoundException(string.Format("Type '{0}' not found.", typeName), e);
            }
        }

        public override void BindToName (Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = _typeToName.ContainsKey(serializedType) ? _typeToName[serializedType] : null;
        }
    }
}