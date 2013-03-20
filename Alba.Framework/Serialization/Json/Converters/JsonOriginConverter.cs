using System;
using Alba.Framework.Common;
using Alba.Framework.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Alba.Framework.Serialization.Json
{
    public class JsonOriginConverter : JsonConverter
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
            var resolveContext = new JsonResolveLinkContext(type, serializer, reader as IJsonLineInfo);
            var jobj = JObject.Load(reader);

            resolveContext.UpdateTypeFromTypeProperty((string)jobj.Property(JsonLinkedContext.TypePropName));
            var id = (string)jobj.Property(resolveContext.IdProp);

            var value = resolveContext.Context.ResolveOrigin(id, resolveContext);
            serializer.Populate(jobj.CreateReader(), value);
            return value;
        }

        public override bool CanConvert (Type type)
        {
            return type.IsAssignableFrom(typeof(IIdentifiable<string>));
        }
    }
}