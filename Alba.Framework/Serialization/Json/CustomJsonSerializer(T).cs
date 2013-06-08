using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Alba.Framework.Attributes;
using Alba.Framework.Collections;
using Alba.Framework.Common;
using Alba.Framework.IO;
using Alba.Framework.Logs;
using Alba.Framework.Sys;
using Alba.Framework.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

// ReSharper disable StaticFieldInGenericType
namespace Alba.Framework.Serialization.Json
{
    public class CustomJsonSerializer<T> : ICustomJsonSerializer<T>
    {
        private const string TempFileExt = ".tmp";
        private const string BackupFileExt = ".bak";
        private const string ErrorRemoveBackupFile = "Failed to remove backup JSON file '{0}'.";
        private const string ErrorSaveFile = "Failed to save JSON file '{0}'.";
        private const string ErrorOpenFile = "Failed to open JSON file '{0}'.";

        private static readonly ILog Log = AlbaFrameworkTraceSources.Serialization.GetLog<CustomJsonSerializer<T>>();

        protected virtual JsonSerializer CreateJsonSerializer ()
        {
            var serializer = new JsonSerializer {
                DefaultValueHandling = DefaultValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.None,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                ContractResolver = GetContractResolver(),
                Binder = BindTypeNameInternal(new DictionarySerializationBinder()),
                Context = new StreamingContext(StreamingContextStates.All,
                    new JsonLinkedContext(GetLinkProviders())),
            };
            serializer.Converters.AddRange(GetConverters());
            SetOptions(serializer);
            return serializer;
        }

        protected virtual void SetOptions (JsonSerializer serializer)
        {}

        protected virtual void SetWriterOptions (JsonTextWriterEx writer)
        {}

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

        private DictionarySerializationBinder BindTypeNameInternal (DictionarySerializationBinder binder)
        {
            BindTypeNames(binder);
            return binder;
        }

        protected virtual void BindTypeNames (DictionarySerializationBinder binder)
        {}

        protected virtual void RememberLinks (T value, JsonLinkedContext context)
        {
            RememberLinks((object)value, context);
        }

        protected static void RememberLinks (object value, JsonLinkedContext context)
        {
            if (value == null)
                return;
            context.PushObject(value);
            context.RememberLink(value);
            var owner = value as IOwner;
            if (owner != null)
                owner.Owned.ForEach(owned => RememberLinks(owned, context));
            context.PopObject(value);
        }

        public bool SerializeToFile (T value, string fileName, bool createBackup = true, bool throwOnError = true)
        {
            string dirName = Path.GetDirectoryName(fileName);
            string tempFileName = fileName + TempFileExt;
            string backupFileName = fileName + BackupFileExt;

            try {
                if (dirName != null)
                    Directory.CreateDirectory(dirName);
                using (var textWriter = Streams.CreateTextFile(tempFileName))
                    SerializeToStream(value, textWriter);

                if (File.Exists(fileName))
                    File.Replace(tempFileName, fileName, backupFileName);
                else
                    File.Move(tempFileName, fileName);

                if (!createBackup) {
                    try {
                        File.Delete(backupFileName);
                    }
                    catch (Exception e) {
                        if (!e.IsIOException())
                            throw;
                        Log.Warning(ErrorRemoveBackupFile.Fmt(backupFileName), e);
                    }
                }
                return true;
            }
            catch (Exception e) {
                if (!e.IsIOException())
                    throw;
                ProcessIOException(e, throwOnError, ErrorSaveFile.Fmt(fileName));
                return false;
            }
        }

        public string SerializeToString (T value)
        {
            using (var stringWriter = Streams.WriteStringInv()) {
                SerializeToStream(value, stringWriter);
                return stringWriter.ToString();
            }
        }

        private void SerializeToStream (T value, TextWriter textWriter)
        {
            JsonSerializer serializer = CreateJsonSerializer();
            var context = JsonLinkedContext.Get(serializer.Context);
            RememberLinks(value, context);

            using (var jsonWriter = new JsonTextWriterEx(textWriter)) {
                jsonWriter.Formatting = Formatting.Indented;
                jsonWriter.Indentation = 2;
                jsonWriter.QuoteNameHandling = QuoteNameHandling.Auto;
                SetWriterOptions(jsonWriter);

                serializer.Serialize(jsonWriter, value);
                //await streamWriter.FlushAsync().ConfigureAwait(false);
                jsonWriter.Flush(); // Flush is the only truly async method, so there's no point in async hassle
            }
        }

        public bool PopulateFromFile (T value, string fileName, bool throwOnError = true)
        {
            try {
                using (var textReader = Streams.ReadTextFile(fileName))
                    PopulateFromStream(value, textReader);
                return true;
            }
            catch (Exception e) {
                if (!e.IsIOException())
                    throw;
                ProcessIOException(e, throwOnError, ErrorOpenFile.Fmt(fileName));
                return false;
            }
        }

        public void PopulateFromString (T value, string source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            using (var stringReader = Streams.ReadString(source))
                PopulateFromStream(value, stringReader);
        }

        private void PopulateFromStream (T value, TextReader textReader)
        {
            JsonSerializer serializer = CreateJsonSerializer();
            var context = JsonLinkedContext.Get(serializer.Context);

            using (var jsonReader = new JsonTextReader(textReader))
                serializer.Populate(jsonReader, value);

            context.ValidateLinksResolved();
        }

        public T DeserializeFromFile (string fileName, bool throwOnError = true)
        {
            try {
                using (var textReader = Streams.ReadTextFile(fileName))
                    return DeserializeFromStream(textReader);
            }
            catch (Exception e) {
                if (!e.IsIOException())
                    throw;
                ProcessIOException(e, throwOnError, ErrorOpenFile.Fmt(fileName));
                return default(T);
            }
        }

        public T DeserializeFromString (string source)
        {
            if (source == null)
                return default(T);
            using (var stringReader = Streams.ReadString(source))
                return DeserializeFromStream(stringReader);
        }

        private T DeserializeFromStream (TextReader textReader)
        {
            JsonSerializer serializer = CreateJsonSerializer();
            var context = JsonLinkedContext.Get(serializer.Context);
            T value;

            using (var jsonReader = new JsonTextReader(textReader))
                value = serializer.Deserialize<T>(jsonReader);

            context.ValidateLinksResolved();
            return value;
        }

        [UsedImplicitly] // throwOnError is not just for "argument check"
        private static void ProcessIOException (Exception e, bool throwOnError, string message)
        {
            if (throwOnError)
                throw new IOException(message, e);
            else
                Log.Error(message, e);
        }

        public Func<object, string> GetGlobalLinks (T value)
        {
            var context = new JsonLinkedContext(GetLinkProviders().Where(p => !p.IsScoped));
            RememberLinks((object)value, context);
            return v => {
                if (v == null)
                    return null;
                var idable = v as IIdentifiable<string>;
                if (idable != null && idable.Id == null)
                    return null;
                return context.Options.GetLinkProvider(v.GetType()).GetLink(v, context);
            };
        }
    }
}