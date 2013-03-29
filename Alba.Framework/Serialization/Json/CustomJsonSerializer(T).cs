using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Alba.Framework.Attributes;
using Alba.Framework.Collections;
using Alba.Framework.IO;
using Alba.Framework.Logs;
using Alba.Framework.Sys;
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

        private static readonly Lazy<ILog> _log = new Lazy<ILog>(() => new Log<CustomJsonSerializer<T>>(AlbaFrameworkTraceSources.Serialization));

        private static ILog Log
        {
            get { return _log.Value; }
        }

        protected virtual JsonSerializer CreateJsonSerializer ()
        {
            var serializer = new JsonSerializer {
                DefaultValueHandling = DefaultValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = GetContractResolver(),
                Binder = BindTypeNameInternal(new DictionarySerializationBinder()),
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

        private DictionarySerializationBinder BindTypeNameInternal (DictionarySerializationBinder binder)
        {
            BindTypeName(binder);
            return binder;
        }

        protected virtual void BindTypeName (DictionarySerializationBinder binder)
        {}

        protected virtual void RememberLinks (T value, JsonLinkedContext context)
        {}

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
                        Log.Warning(string.Format(ErrorRemoveBackupFile, backupFileName), e);
                    }
                }
                return true;
            }
            catch (Exception e) {
                if (!e.IsIOException())
                    throw;
                ProcessIOException(e, throwOnError, string.Format(ErrorSaveFile, fileName));
                return false;
            }
        }

        public string SerializeToString (T value)
        {
            using (var stringWriter = Streams.WriteInvariantString()) {
                SerializeToStream(value, stringWriter);
                return stringWriter.ToString();
            }
        }

        private void SerializeToStream (T value, TextWriter textWriter)
        {
            JsonSerializer serializer = CreateJsonSerializer();
            var context = JsonLinkedContext.Get(serializer.Context);
            RememberLinks(value, context);

            using (var jsonWriter = new JsonTextWriter(textWriter)) {
                jsonWriter.Formatting = Formatting.Indented;
                jsonWriter.Indentation = 2;
                jsonWriter.QuoteName = false;

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
                ProcessIOException(e, throwOnError, string.Format(ErrorOpenFile, fileName));
                return false;
            }
        }

        public void PopulateFromString (T value, string source)
        {
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
                ProcessIOException(e, throwOnError, string.Format(ErrorOpenFile, fileName));
                return default(T);
            }
        }

        public T DeserializeFromString (string source)
        {
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
    }
}