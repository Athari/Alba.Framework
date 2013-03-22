using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Alba.Framework.Collections;

namespace Alba.Framework.Serialization.Json
{
    public class CustomJsonSerializer<T>
    {
        protected virtual JsonSerializer CreateJsonSerializer ()
        {
            var serializer = new JsonSerializer {
                DefaultValueHandling = DefaultValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ContractResolver = GetContractResolver(),
                Binder = BindTypeName(new DictionarySerializationBinder()),
                Context = new StreamingContext(StreamingContextStates.All,
                    new JsonLinkedContext(linkProviders: GetLinkProviders())),
            };
            serializer.Converters.AddRange(GetConverters());
            return serializer;
        }

        protected virtual IContractResolver GetContractResolver ()
        {
            return new JsonLinkedContractResolver(shareCache: true) {
                IgnoreSerializableAttribute = false,
            };
        }

        protected IEnumerable<JsonConverter> GetConverters ()
        {
            yield return new EnumFlagsConverter();
        }

        protected virtual IEnumerable<IJsonLinkProvider> GetLinkProviders ()
        {
            yield break;
        }

        protected virtual DictionarySerializationBinder BindTypeName (DictionarySerializationBinder binder)
        {
            return binder;
        }

        protected virtual void RememberLinks (T value, JsonLinkedContext context)
        {}

        private async Task SerializeToStream (T value, Stream stream)
        {
            JsonSerializer serializer = CreateJsonSerializer();
            var context = JsonLinkedContext.Get(serializer.Context);
            RememberLinks(value, context);

            using (var streamWriter = new StreamWriter(stream))
            using (var jsonWriter = new JsonTextWriter(streamWriter)) {
                jsonWriter.Formatting = Formatting.Indented;
                jsonWriter.Indentation = 2;
                jsonWriter.QuoteName = false;

                serializer.Serialize(jsonWriter, value);
                await streamWriter.FlushAsync().ConfigureAwait(false);
            }
        }

        private void PopulateFromStream (T value, Stream stream)
        {
            JsonSerializer serializer = CreateJsonSerializer();
            var context = JsonLinkedContext.Get(serializer.Context);

            using (var streamReader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(streamReader)) {
                serializer.Populate(jsonReader, value);
            }

            context.ValidateLinksResolved();
        }

        private T DeserializeFromStream (Stream stream)
        {
            JsonSerializer serializer = CreateJsonSerializer();
            var context = JsonLinkedContext.Get(serializer.Context);
            T value;

            using (var streamReader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(streamReader)) {
                value = serializer.Deserialize<T>(jsonReader);
            }

            context.ValidateLinksResolved();
            return value;
        }
    }
}