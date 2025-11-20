using System.Globalization;
using System.Text;
using Alba.Framework.Text;

namespace Alba.Framework.IO;

public class Streams
{
    private const FileShare DefaultFileShare = FileShare.Read;
    //private const int DefaultBufferSize = 1024;

    public static FileStream CreateFile(string path) =>
        new(path, FileMode.Create, FileAccess.Write, DefaultFileShare);

    public static FileStream AppendFile(string path) =>
        new(path, FileMode.Append, FileAccess.Write, DefaultFileShare);

    public static FileStream ReadFile(string path) =>
        new(path, FileMode.Open, FileAccess.Read, DefaultFileShare);

    public static StreamWriter CreateTextFile(string path, Encoding? encoding = null) =>
        new(path, false, encoding ?? Encodings.Utf8);

    public static StreamWriter AppendTextFile(string path, Encoding? encoding = null) =>
        new(path, true, encoding ?? Encodings.Utf8);

    public static StreamReader ReadTextFile(string path, Encoding? encoding = null) =>
        new(path, encoding ?? Encodings.Utf8);

    public static BinaryWriter CreateBinaryFile(string path, Encoding? encoding = null) =>
        new(CreateFile(path), encoding ?? Encodings.Utf8);

    public static BinaryWriter AppendBinaryFile(string path, Encoding? encoding = null) =>
        new(AppendFile(path), encoding ?? Encodings.Utf8);

    public static BinaryReader ReadBinaryFile(string path, Encoding? encoding = null) =>
        new(ReadFile(path), encoding ?? Encodings.Utf8);

    public static StringWriter WriteString(IFormatProvider? formatProvider = null) =>
        new(formatProvider ?? CultureInfo.CurrentCulture);

    public static StringWriter WriteStringInv() =>
        new(CultureInfo.InvariantCulture);

    public static StringReader ReadString(string value) =>
        new(value);

    public static StringWriter WriteString(StringBuilder sb, IFormatProvider? formatProvider = null) =>
        new(sb, formatProvider ?? CultureInfo.CurrentCulture);

    public static StringWriter WriteStringInv(StringBuilder sb) =>
        new(sb, CultureInfo.InvariantCulture);

    public static MemoryStream WriteMemory(int capacity = 0) =>
        new(capacity);

    public static MemoryStream WriteMemory(byte[] buffer, int index = 0, int count = int.MaxValue) =>
        new(buffer, index, count != int.MaxValue ? count : buffer.Length - index, true);

    public static MemoryStream ReadMemory(byte[] buffer, int index = 0, int count = int.MaxValue) =>
        new(buffer, index, count != int.MaxValue ? count : buffer.Length - index, false);
}