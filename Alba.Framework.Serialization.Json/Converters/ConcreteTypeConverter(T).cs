using System;
using Alba.Framework.Reflection;
using Newtonsoft.Json;

namespace Alba.Framework.Serialization.Json
{
    public class ConcreteTypeConverter<T> : JsonConverter
    {
        public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override object ReadJson (JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize<T>(reader);
        }

        public override bool CanConvert (Type type)
        {
            return type.IsAssignableTo(typeof(T));
        }
    }
}