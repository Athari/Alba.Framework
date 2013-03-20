using System;
using Newtonsoft.Json;

namespace Alba.Framework.Serialization.Json
{
    public static class JsonSerializerExts
    {
        public static T Create<T> (this JsonSerializer serializer)
        {
            return (T)serializer.Create(typeof(T));
        }

        public static object Create (this JsonSerializer serializer, Type type)
        {
            return serializer.ContractResolver.ResolveContract(type).DefaultCreator();
        }
    }
}