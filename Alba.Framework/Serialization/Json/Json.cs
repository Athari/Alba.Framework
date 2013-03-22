using System;
using System.IO;
using System.Threading.Tasks;
using Alba.Framework.Logs;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Alba.Framework.Serialization.Json
{
    public class Json
    {
        private static readonly Lazy<ILog> _log = new Lazy<ILog>(() => new Log<Json>(AlbaFrameworkTraceSources.Serialization));

        private static ILog Log
        {
            get { return _log.Value; }
        }

        public static bool PopulateFromFile (object obj, string fileName)
        {
            if (!File.Exists(fileName)) {
                Log.Info(string.Format("File '{0}' does not exist, loading skipped.", fileName));
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
                Log.Error(string.Format("Failed to load file '{0}'.", fileName), e);
            }
            return true;
        }

        public static async Task SerializeToFile (object obj, string fileName)
        {
            string tempFileName = fileName + ".tmp";
            string backupFileName = fileName + ".bak";

            try {
                using (var fileStream = File.Open(tempFileName, FileMode.Create, FileAccess.Write, FileShare.Read))
                using (var streamWriter = new StreamWriter(fileStream))
                using (var jsonWriter = new JsonTextWriter(streamWriter)) {
                    jsonWriter.Formatting = Formatting.Indented;
                    jsonWriter.Indentation = 2;
                    jsonWriter.QuoteName = false;

                    GetJsonSerializer().Serialize(jsonWriter, obj);
                    await streamWriter.FlushAsync().ConfigureAwait(false);
                }

                if (File.Exists(fileName))
                    File.Replace(tempFileName, fileName, backupFileName);
                else
                    File.Move(tempFileName, fileName);
            }
            catch (Exception e) {
                Log.Error(string.Format("Failed to save file '{0}'.", fileName), e);
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