using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Alba.Framework.Serialization.Json
{
    public class JsonJObjectConverter<T> : JsonConverter<T>
    {
        private readonly bool _useExistingValue = true;

        public JsonJObjectConverter (bool useExistingValue = true)
        {
            _useExistingValue = useExistingValue;
        }

        public override sealed void WriteJson (JsonWriter writer, T value, JsonSerializer serializer)
        {
            writer.WriteToken(ToJObject(value, serializer).CreateReader());
        }

        public virtual JObject ToJObject (T value, JsonSerializer serializer)
        {
            return JObject.FromObject(value, serializer);
        }

        public override sealed T ReadJson (JsonReader reader, Type objectType, T existingValue, JsonSerializer serializer)
        {
            return FromJObject(reader, JObject.Load(reader), objectType, existingValue, serializer);
        }

        public virtual T FromJObject (JsonReader reader, JObject jo, Type objectType, T existingValue, JsonSerializer serializer)
        {
            // ReSharper disable once CompareNonConstrainedGenericWithNull
            if (_useExistingValue && existingValue != null) {
                serializer.Populate(jo.CreateReader(), existingValue);
                return existingValue;
            }
            else {
                return (T)serializer.Deserialize(jo.CreateReader(), objectType);
            }
        }
    }
}