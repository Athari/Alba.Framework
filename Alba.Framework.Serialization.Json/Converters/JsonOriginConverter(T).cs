using System;
using System.Collections;
using System.Linq;
using Alba.Framework.Globalization;
using Alba.Framework.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Alba.Framework.Serialization.Json
{
    public abstract class JsonOriginConverter<T> : JsonJObjectConverter<T>
    {
        public override sealed JObject ToJObject (T value, JsonSerializer serializer)
        {
            JObject jo = SerializeToJObject(value, serializer);
            string typeName = serializer.Binder.BindToName(value.GetType());
            if (!typeName.IsNullOrEmpty())
                jo.AddFirst(new JProperty(JsonLinkedContext.TypePropName, typeName));
            return jo;
        }

        public virtual JObject SerializeToJObject (T value, JsonSerializer serializer)
        {
            return base.ToJObject(value, serializer);
        }

        public override sealed T FromJObject (JsonReader reader, JObject jo, Type objectType, T existingValue, JsonSerializer serializer)
        {
            var resolveContext = new JsonResolveLinkContext(objectType, serializer, reader as IJsonLineInfo);
            resolveContext.UpdateTypeFromTypeProperty(objectType, (string)jo.Property(JsonLinkedContext.TypePropName));
            var value = (T)resolveContext.Context.ResolveOrigin(GetId(resolveContext, jo), resolveContext);
            PopulateFromJObject(reader, jo, objectType, ref value, serializer);
            return value;
        }

        public virtual void PopulateFromJObject (JsonReader reader, JObject jo, Type objectType, ref T existingValue, JsonSerializer serializer)
        {
            serializer.Populate(jo.CreateReader(), existingValue);
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
    }
}