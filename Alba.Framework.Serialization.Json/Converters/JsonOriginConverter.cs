using System;
using System.Collections;
using System.Linq;
using Alba.Framework.Common;
using Alba.Framework.Globalization;
using Alba.Framework.Reflection;
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

            resolveContext.UpdateTypeFromTypeProperty(type, (string)jobj.Property(JsonLinkedContext.TypePropName));

            var value = resolveContext.Context.ResolveOrigin(GetId(resolveContext, jobj), resolveContext);
            serializer.Populate(jobj.CreateReader(), value);
            return value;
        }

        private static string GetId (JsonResolveLinkContext resolveContext, JObject jobj)
        {
            string idProp = resolveContext.IdProp;
            if (idProp != JsonLinkedContext.IndexPropName)
                return (string)jobj.Property(idProp);
            else {
                var collection = resolveContext.Context.Stack.Last() as ICollection;
                return collection != null ? collection.Count.ToStringInv() : "";
            }
        }

        public override bool CanConvert (Type type)
        {
            return type.Is<IIdentifiable<string>>();
        }
    }
}