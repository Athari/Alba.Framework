using System;
using Alba.Framework.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Alba.Framework.Serialization.Json
{
    public class JsonTypedObjectConverter : JsonConverter
    {
        public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
        {
            var jobj = JObject.FromObject(value, serializer);
            string typeName = serializer.Binder.BindToName(value.GetType());
            if (!typeName.IsNullOrEmpty())
                jobj.AddFirst(new JProperty(JsonLinkedContext.TypePropName, typeName));
            serializer.Serialize(writer, jobj);
        }

        public override object ReadJson (JsonReader reader, Type type, object existingValue, JsonSerializer serializer)
        {
            var jobj = JObject.Load(reader);
            var typeName = (string)jobj.Property(JsonLinkedContext.TypePropName);
            if (typeName != null) {
                jobj.Remove(JsonLinkedContext.TypePropName);
                type = serializer.Binder.BindToType(type, typeName);
            }
            var value = existingValue != null && existingValue.GetType() == type ? existingValue : serializer.Create(type);
            serializer.Populate(jobj.CreateReader(), value);
            return value;
        }

        public override bool CanConvert (Type type)
        {
            return true;
        }
    }
}