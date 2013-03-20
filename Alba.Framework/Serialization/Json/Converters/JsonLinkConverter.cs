using System;
using Alba.Framework.Common;
using Newtonsoft.Json;

namespace Alba.Framework.Serialization.Json
{
    public class JsonLinkConverter : JsonConverter
    {
        public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(JsonLinkedContext.Get(serializer.Context).GetTypedLink(value, serializer));
        }

        public override object ReadJson (JsonReader reader, Type type, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String)
                throw new Exception("Link value must be a string.");
            var resolveContext = new JsonResolveLinkContext(type, serializer, reader as IJsonLineInfo);
            return JsonLinkedContext.Get(serializer.Context).ResolveLink(reader.Value.ToString(), resolveContext);
        }

        public override bool CanConvert (Type type)
        {
            return type.IsAssignableFrom(typeof(IIdentifiable<string>));
        }
    }
}