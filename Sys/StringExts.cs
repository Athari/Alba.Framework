using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Alba.Framework.Sys
{
    public static class StringExts
    {
        private static readonly Regex _reNewlines = new Regex("@\r?\n", RegexOptions.Compiled);

        public static bool IsNullOrEmpty (this string @this)
        {
            return string.IsNullOrEmpty(@this);
        }

        public static bool IsNullOrWhitespace (this string @this)
        {
            return string.IsNullOrWhiteSpace(@this);
        }

        public static string RemovePostfix (this string @this, string postfix)
        {
            if (!@this.EndsWith(postfix))
                throw new ArgumentException("string does not contain postfix", "postfix");
            return @this.Remove(@this.Length - postfix.Length);
        }

        public static string AppendSentence (this string @this, string sentence)
        {
            var sb = new StringBuilder(@this, @this.Length + sentence.Length + 2);
            sb.AppendSentence(sentence);
            return sb.ToString();
        }

        public static string SingleLine (this string @this)
        {
            return _reNewlines.Replace(@this, " ");
        }

        public static void AppendSentence (this StringBuilder @this, string sentence)
        {
            @this.Append(@this[@this.Length - 1] == '.' ? " " : ". ");
            @this.Append(sentence);
        }
    }
}