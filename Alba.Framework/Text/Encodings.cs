using System;
using System.Text;

namespace Alba.Framework.Text
{
    public static class Encodings
    {
        private static readonly Lazy<Encoding> _utf8 = new Lazy<Encoding>(() => new UTF8Encoding(false, true));
        private static readonly Lazy<Encoding> _utf8Safe = new Lazy<Encoding>(() => new UTF8Encoding(false, false));
        private static readonly Lazy<Encoding> _utf8NoBom = new Lazy<Encoding>(() => new UTF8Encoding(false, true));
        private static readonly Lazy<Encoding> _utf8NoBomSafe = new Lazy<Encoding>(() => new UTF8Encoding(false, false));
        private static readonly Lazy<Encoding> _utf16 = new Lazy<Encoding>(() => new UnicodeEncoding(false, false, true));
        private static readonly Lazy<Encoding> _utf16Safe = new Lazy<Encoding>(() => new UnicodeEncoding(false, false, false));
        private static readonly Lazy<Encoding> _utf16NoBom = new Lazy<Encoding>(() => new UnicodeEncoding(false, false, true));
        private static readonly Lazy<Encoding> _utf16NoBomSafe = new Lazy<Encoding>(() => new UnicodeEncoding(false, false, true));

        public static Encoding Utf8
        {
            get { return _utf8.Value; }
        }

        public static Encoding Utf8Safe
        {
            get { return _utf8Safe.Value; }
        }

        public static Encoding Utf8NoBom
        {
            get { return _utf8NoBom.Value; }
        }

        public static Encoding Utf8NoBomSafe
        {
            get { return _utf8NoBomSafe.Value; }
        }

        public static Encoding Utf16
        {
            get { return _utf16.Value; }
        }

        public static Encoding Utf16Safe
        {
            get { return _utf16Safe.Value; }
        }

        public static Encoding Utf16NoBom
        {
            get { return _utf16NoBom.Value; }
        }

        public static Encoding Utf16NoBomSafe
        {
            get { return _utf16NoBomSafe.Value; }
        }
    }
}