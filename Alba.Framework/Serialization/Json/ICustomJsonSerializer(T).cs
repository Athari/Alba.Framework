namespace Alba.Framework.Serialization.Json
{
    public interface ICustomJsonSerializer<T>
    {
        bool SerializeToFile (T value, string fileName, bool createBackup = true, bool throwOnError = true);
        string SerializeToString (T value);
        bool PopulateFromFile (T value, string fileName, bool throwOnError = true);
        void PopulateFromString (T value, string source);
        T DeserializeFromFile (string fileName, bool throwOnError = true);
        T DeserializeFromString (string source);
    }
}