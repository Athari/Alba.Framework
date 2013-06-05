using System;
using System.Globalization;
using System.IO;
using System.Text;
using Alba.Framework.Text;

namespace Alba.Framework.IO
{
    public class Streams
    {
        private const FileShare DefaultFileShare = FileShare.Read;
        //private const int DefaultBufferSize = 1024;

        public static FileStream CreateFile (string path)
        {
            return new FileStream(path, FileMode.Create, FileAccess.Write, DefaultFileShare);
        }

        public static FileStream AppendFile (string path)
        {
            return new FileStream(path, FileMode.Append, FileAccess.Write, DefaultFileShare);
        }

        public static FileStream ReadFile (string path)
        {
            return new FileStream(path, FileMode.Open, FileAccess.Read, DefaultFileShare);
        }

        public static StreamWriter CreateTextFile (string path, Encoding encoding = null)
        {
            return new StreamWriter(path, false, encoding ?? Encodings.Utf8NoBom);
        }

        public static StreamWriter AppendTextFile (string path, Encoding encoding = null)
        {
            return new StreamWriter(path, true, encoding ?? Encodings.Utf8NoBom);
        }

        public static StreamReader ReadTextFile (string path, Encoding encoding = null)
        {
            return new StreamReader(path, encoding ?? Encodings.Utf8);
        }

        public static BinaryWriter CreateBinaryFile (string path, Encoding encoding = null)
        {
            return new BinaryWriter(CreateFile(path), encoding ?? Encodings.Utf8NoBom);
        }

        public static BinaryWriter AppendBinaryFile (string path, Encoding encoding = null)
        {
            return new BinaryWriter(AppendFile(path), encoding ?? Encodings.Utf8NoBom);
        }

        public static BinaryReader ReadBinaryFile (string path, Encoding encoding = null)
        {
            return new BinaryReader(ReadFile(path), encoding ?? Encodings.Utf8NoBom);
        }

        public static StringWriter WriteString (IFormatProvider formatProvider = null)
        {
            return new StringWriter(formatProvider ?? CultureInfo.CurrentCulture);
        }

        public static StringWriter WriteStringInv ()
        {
            return new StringWriter(CultureInfo.InvariantCulture);
        }

        public static StringReader ReadString (string value, Encoding encoding = null)
        {
            return new StringReader(value);
        }

        public static StringWriter WriteString (StringBuilder sb, IFormatProvider formatProvider = null)
        {
            return new StringWriter(sb, formatProvider ?? CultureInfo.CurrentCulture);
        }

        public static StringWriter WriteStringInv (StringBuilder sb)
        {
            return new StringWriter(sb, CultureInfo.InvariantCulture);
        }

        public static MemoryStream WriteMemory (int capacity = 0)
        {
            return new MemoryStream(capacity);
        }

        public static MemoryStream WriteMemory (byte[] buffer, int index = 0, int count = int.MaxValue)
        {
            return new MemoryStream(buffer, index, count != int.MaxValue ? count : buffer.Length - index, true);
        }

        public static MemoryStream ReadMemory (byte[] buffer, int index = 0, int count = int.MaxValue)
        {
            return new MemoryStream(buffer, index, count != int.MaxValue ? count : buffer.Length - index, false);
        }
    }
}