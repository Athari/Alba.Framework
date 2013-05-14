using System;
using System.IO;
using Alba.Framework.Logs;
using Alba.Framework.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Alba.Framework.Serialization.Json
{
    public class Json
    {
        /// <summary>Value for <see cref="JsonPropertyAttribute.Order"/> of identifier property.</summary>
        public const int IdPropOrder = -1000;

        private static readonly ILog Log = AlbaFrameworkTraceSources.Serialization.GetLog<Json>();

        public static bool PopulateFromFile (object obj, string fileName)
        {
            if (!File.Exists(fileName)) {
                Log.Info("File '{0}' does not exist, loading skipped.".Fmt(fileName));
                return false;
            }

            try {
                using (var fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var streamReader = new StreamReader(fileStream))
                using (var jsonReader = new JsonTextReader(streamReader)) {
                    GetJsonSerializer().Populate(jsonReader, obj);
                }
            }
            catch (Exception e) {
                Log.Error("Failed to load file '{0}'.".Fmt(fileName), e);
            }
            return true;
        }

        public static void SerializeToFile (object obj, string fileName)
        {
            string tempFileName = fileName + ".tmp";
            string backupFileName = fileName + ".bak";

            try {
                using (var fileStream = File.Open(tempFileName, FileMode.Create, FileAccess.Write, FileShare.Read))
                using (var streamWriter = new StreamWriter(fileStream))
                using (var jsonWriter = new JsonTextWriter(streamWriter)) {
                    jsonWriter.Formatting = Formatting.Indented;
                    jsonWriter.Indentation = 2;
                    jsonWriter.QuoteNameHandling = QuoteNameHandling.Auto;

                    GetJsonSerializer().Serialize(jsonWriter, obj);
                    jsonWriter.Flush();
                }

                if (File.Exists(fileName))
                    File.Replace(tempFileName, fileName, backupFileName);
                else
                    File.Move(tempFileName, fileName);
            }
            catch (Exception e) {
                Log.Error("Failed to save file '{0}'.".Fmt(fileName), e);
            }
        }

        private static JsonSerializer GetJsonSerializer ()
        {
            return new JsonSerializer {
                DefaultValueHandling = DefaultValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto,
                Converters = {
                    new EnumFlagsConverter(),
                },
                ContractResolver = new DefaultContractResolver {
                    IgnoreSerializableAttribute = false,
                }
            };
        }
    }
}