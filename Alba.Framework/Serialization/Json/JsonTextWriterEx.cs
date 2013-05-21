using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Alba.Framework.Collections;
using Newtonsoft.Json;

namespace Alba.Framework.Serialization.Json
{
    /// <summary>
    /// JSON text writer with proper support for unquoted properties (they can be quoted only if necessary).
    /// </summary>
    public class JsonTextWriterEx : JsonTextWriter
    {
        private readonly TextWriter _writer;

        public QuoteNameHandling QuoteNameHandling { get; set; }

        public JsonTextWriterEx (TextWriter textWriter) : base(textWriter)
        {
            _writer = textWriter;
            QuoteNameHandling = QuoteNameHandling.Quoted;
        }

        private bool[] CharEscapeFlags
        {
            get
            {
                if (StringEscapeHandling == StringEscapeHandling.EscapeHtml)
                    return JavaScriptUtils.HtmlCharEscapeFlags;
                else if (QuoteChar == '"')
                    return JavaScriptUtils.DoubleQuoteCharEscapeFlags;
                else
                    return JavaScriptUtils.SingleQuoteCharEscapeFlags;
            }
        }

        public override void WritePropertyName (string name)
        {
            WritePropertyName(name, true);
        }

        public override void WritePropertyName (string name, bool escape)
        {
            SetWriteState(JsonToken.PropertyName, name);
            if (escape) {
                JavaScriptUtils.WriteEscapedJavaScriptString(_writer, name, QuoteChar, QuoteNameHandling, CharEscapeFlags, StringEscapeHandling);
            }
            else {
                bool quoteName = QuoteNameHandling == QuoteNameHandling.Quoted
                    || QuoteNameHandling == QuoteNameHandling.Auto && !JavaScriptUtils.IsValidIdentifier(name);
                if (quoteName)
                    _writer.Write(QuoteChar);
                _writer.Write(name);
                if (quoteName)
                    _writer.Write(QuoteChar);
            }
            _writer.Write(':');
        }

        private static class JavaScriptUtils
        {
            private const string EscapedUnicodeText = "!";

            public static readonly bool[] SingleQuoteCharEscapeFlags = new bool[128];
            public static readonly bool[] DoubleQuoteCharEscapeFlags = new bool[128];
            public static readonly bool[] HtmlCharEscapeFlags = new bool[128];

            static JavaScriptUtils ()
            {
                var escapeChars = new List<char> { '\n', '\r', '\t', '\\', '\f', '\b' };
                for (int i = 0; i < ' '; i++)
                    escapeChars.Add((char)i);
                foreach (var escapeChar in escapeChars.Concat('\''))
                    SingleQuoteCharEscapeFlags[escapeChar] = true;
                foreach (var escapeChar in escapeChars.Concat('"'))
                    DoubleQuoteCharEscapeFlags[escapeChar] = true;
                foreach (var escapeChar in escapeChars.Concat('"', '\'', '<', '>', '&'))
                    HtmlCharEscapeFlags[escapeChar] = true;
            }

            public static void WriteEscapedJavaScriptString (TextWriter writer, string s, char delimiter, QuoteNameHandling appendDelimitersHandling, bool[] charEscapeFlags, StringEscapeHandling stringEscapeHandling)
            {
                // leading delimiter
                bool appendDelimiters = appendDelimitersHandling == QuoteNameHandling.Quoted
                    || appendDelimitersHandling == QuoteNameHandling.Auto && !IsValidIdentifier(s);
                if (appendDelimiters)
                    writer.Write(delimiter);

                if (s != null) {
                    char[] chars = null;
                    char[] unicodeBuffer = null;
                    int lastWritePosition = 0;

                    for (int i = 0; i < s.Length; i++) {
                        var c = s[i];

                        if (c < charEscapeFlags.Length && !charEscapeFlags[c])
                            continue;

                        string escapedValue;

                        switch (c) {
                            case '\t':
                                escapedValue = "\\t";
                                break;
                            case '\n':
                                escapedValue = "\\n";
                                break;
                            case '\r':
                                escapedValue = "\\r";
                                break;
                            case '\f':
                                escapedValue = "\\f";
                                break;
                            case '\b':
                                escapedValue = "\\b";
                                break;
                            case '\\':
                                escapedValue = "\\\\";
                                break;
                            case '\u0085': // Next Line
                                escapedValue = "\\u0085";
                                break;
                            case '\u2028': // Line Separator
                                escapedValue = "\\u2028";
                                break;
                            case '\u2029': // Paragraph Separator
                                escapedValue = "\\u2029";
                                break;
                            default:
                                if (c >= charEscapeFlags.Length && stringEscapeHandling != StringEscapeHandling.EscapeNonAscii)
                                    escapedValue = null;
                                else if (c == '\'' && stringEscapeHandling != StringEscapeHandling.EscapeHtml)
                                    escapedValue = "\\'";
                                else if (c == '"' && stringEscapeHandling != StringEscapeHandling.EscapeHtml)
                                    escapedValue = "\\\"";
                                else {
                                    if (unicodeBuffer == null)
                                        unicodeBuffer = new char[6];
                                    StringUtils.ToCharAsUnicode(c, unicodeBuffer);
                                    // slightly hacky but it saves multiple conditions in if test
                                    escapedValue = EscapedUnicodeText;
                                }
                                break;
                        }

                        if (escapedValue == null)
                            continue;

                        if (i > lastWritePosition) {
                            if (chars == null)
                                chars = s.ToCharArray();
                            // write unchanged chars before writing escaped text
                            writer.Write(chars, lastWritePosition, i - lastWritePosition);
                        }

                        lastWritePosition = i + 1;
                        if (string.Equals(escapedValue, EscapedUnicodeText))
                            writer.Write(unicodeBuffer);
                        else
                            writer.Write(escapedValue);
                    }

                    if (lastWritePosition == 0) // no escaped text, write entire string
                        writer.Write(s);
                    else {
                        if (chars == null)
                            chars = s.ToCharArray();
                        // write remaining text
                        writer.Write(chars, lastWritePosition, s.Length - lastWritePosition);
                    }
                }

                // trailing delimiter
                if (appendDelimiters)
                    writer.Write(delimiter);
            }

            private static bool IsValidIdentifierChar (char value)
            {
                return (Char.IsLetterOrDigit(value) || value == '_' || value == '$');
            }

            public static bool IsValidIdentifier (string value)
            {
                return value.All(IsValidIdentifierChar);
            }
        }

        private static class StringUtils
        {
            public static void ToCharAsUnicode (char c, char[] buffer)
            {
                buffer[0] = '\\';
                buffer[1] = 'u';
                buffer[2] = IntToHex((c >> 12) & '\x000f');
                buffer[3] = IntToHex((c >> 8) & '\x000f');
                buffer[4] = IntToHex((c >> 4) & '\x000f');
                buffer[5] = IntToHex((c >> 0) & '\x000f');
            }

            private static char IntToHex (int n)
            {
                return n <= 9 ? (char)(n + 48) : (char)((n - 10) + 97);
            }
        }
    }
}