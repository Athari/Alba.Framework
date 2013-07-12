using System;
using Alba.Framework.Reflection;
using Alba.Framework.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
            JsonContract contract = serializer.ContractResolver.ResolveContract(type);
            if (contract.DefaultCreator == null)
                throw new JsonSerializationException("Unable to find a default constructor to use for type '{0}'."
                    .Fmt(type.GetFullName()));
            return contract.DefaultCreator();
        }
    }
}