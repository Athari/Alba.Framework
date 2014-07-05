using System;
using Newtonsoft.Json;

// TODO Modify Json.Net to make JsonConverter.WriteJson include container property type argument (also in JsonOriginConverter)
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
            return resolveContext.Context.ResolveLink(reader.Value.ToString(), resolveContext);
        }

        public override bool CanConvert (Type type)
        {
            return true;
        }
    }
}