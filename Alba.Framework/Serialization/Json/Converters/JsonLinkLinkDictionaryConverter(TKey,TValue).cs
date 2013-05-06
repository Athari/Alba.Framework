using System;
using System.Collections.Generic;
using System.Linq;
using Alba.Framework.Common;
using Alba.Framework.Sys;
using Newtonsoft.Json;

namespace Alba.Framework.Serialization.Json
{
    public class JsonLinkLinkDictionaryConverter<TKey, TValue> : JsonConverter
        where TKey : IIdentifiable<string>
        where TValue : IIdentifiable<string>
    {
        public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
        {
            var context = JsonLinkedContext.Get(serializer.Context);
            serializer.Serialize(writer, ((IDictionary<TKey, TValue>)value).ToDictionary(
                p => context.GetTypedLink(p.Key, serializer),
                p => context.GetTypedLink(p.Value, serializer)
                ));
        }

        public override object ReadJson (JsonReader reader, Type type, object existingValue, JsonSerializer serializer)
        {
            var resolveContext = new JsonResolveLinkContext(type, serializer, reader as IJsonLineInfo);
            return serializer.Deserialize<Dictionary<string, string>>(reader).ToDictionary(
                p => resolveContext.Context.ResolveLink(p.Key, resolveContext),
                p => resolveContext.Context.ResolveLink(p.Value, resolveContext)
                );
        }

        public override bool CanConvert (Type type)
        {
            return type.IsAssignableTo(typeof(IDictionary<TKey, TValue>));
        }
    }
}