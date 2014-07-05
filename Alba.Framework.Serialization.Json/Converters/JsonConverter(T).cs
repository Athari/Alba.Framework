using System;
using System.Runtime.CompilerServices;
using Alba.Framework.Reflection;
using Alba.Framework.Text;
using Newtonsoft.Json;

namespace Alba.Framework.Serialization.Json
{
    public abstract class JsonConverter<T> : JsonConverter
    {
        public override sealed void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
        {
            WriteJson(writer, (T)value, serializer);
        }

        public virtual void WriteJson (JsonWriter writer, T value, JsonSerializer serializer)
        {
            throw CreateNotSupportedException();
        }

        public override sealed object ReadJson (JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return ReadJson(reader, objectType, existingValue != null ? (T)existingValue : default(T), serializer);
        }

        public virtual T ReadJson (JsonReader reader, Type objectType, T existingValue, JsonSerializer serializer)
        {
            throw CreateNotSupportedException();
        }

        public override sealed bool CanConvert (Type objectType)
        {
            return objectType.Is<T>();
        }

        protected TEnum ParseEnum<TEnum> (string value) where TEnum : struct
        {
            TEnum result;
            if (!Enum.TryParse(value, out result))
                throw new JsonSerializationException("Unexpected enum value of {0} '{1}'.".Fmt(typeof(TEnum).Name, value));
            return result;
        }

        protected Exception CreateNotSupportedException ([CallerMemberName] string memberName = null)
        {
            return new NotSupportedException("{0} does not support {1}.".Fmt(GetType().GetFullName(), memberName));
        }
    }
}