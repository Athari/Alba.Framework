using System.Text.RegularExpressions;

namespace Alba.Framework.Text;

public static partial class StringExts
{
    extension(string @this)
    {
        [Pure]
        public string ReEscape() =>
            Regex.Escape(@this);

        [Pure]
        public string ReEscapeReplacement() =>
            @this.Replace("$", "$$");

        [Pure]
        public bool IsReMatch([RegexPattern] string pattern, RegexOptions options = RegexOptions.None) =>
            Regex.IsMatch(@this, pattern, options);

        [Pure]
        public bool IsReMatch(Regex regex) =>
            regex.IsMatch(@this);

        [Pure]
        public Match ReMatch([RegexPattern] string pattern, RegexOptions options = RegexOptions.None) =>
            Regex.Match(@this, pattern, options);

        [Pure]
        public Match ReMatch(Regex regex) =>
            regex.Match(@this);

        [Pure]
        public string ReMatchGet([RegexPattern] string pattern, int groupNum = 1, RegexOptions options = RegexOptions.None) =>
            Regex.Match(@this, pattern, options).Get(groupNum);

        [Pure]
        public string ReMatchGet([RegexPattern] string pattern, string groupName, RegexOptions options = RegexOptions.None) =>
            Regex.Match(@this, pattern, options).Get(groupName);

        [Pure]
        public string ReMatchGet(Regex regex, int groupNum = 1) =>
            regex.Match(@this).Get(groupNum);

        [Pure]
        public string ReMatchGet(Regex regex, string groupName) =>
            regex.Match(@this).Get(groupName);

        public string? ReMatchTryGet([RegexPattern] string pattern, int groupNum = 1, RegexOptions options = RegexOptions.None) =>
            Regex.Match(@this, pattern, options).TryGet(groupNum);

        [Pure]
        public string? ReMatchTryGet([RegexPattern] string pattern, string groupName, RegexOptions options = RegexOptions.None) =>
            Regex.Match(@this, pattern, options).TryGet(groupName);

        [Pure]
        public string? ReMatchTryGet(Regex regex, int groupNum = 1) =>
            regex.Match(@this).TryGet(groupNum);

        [Pure]
        public string? ReMatchTryGet(Regex regex, string groupName) =>
            regex.Match(@this).TryGet(groupName);

        [Pure]
        public MatchCollection ReMatches([RegexPattern] string pattern, RegexOptions options = RegexOptions.None) =>
            Regex.Matches(@this, pattern, options);

        [Pure]
        public MatchCollection ReMatches(Regex regex) =>
            regex.Matches(@this);

        [Pure]
        public IEnumerable<string> ReMatchesGet([RegexPattern] string pattern, int groupNum = 1, RegexOptions options = RegexOptions.None) =>
            Regex.Matches(@this, pattern, options).Get(groupNum);

        [Pure]
        public IEnumerable<string> ReMatchesGet([RegexPattern] string pattern, string groupName, RegexOptions options = RegexOptions.None) =>
            Regex.Matches(@this, pattern, options).Get(groupName);

        [Pure]
        public IEnumerable<string> ReMatchesGet(Regex regex, int groupNum = 1) =>
            regex.Matches(@this).Get(groupNum);

        [Pure]
        public IEnumerable<string> ReMatchesGet(Regex regex, string groupName) =>
            regex.Matches(@this).Get(groupName);

        [Pure]
        public string ReReplace([RegexPattern] string pattern, string replacement, RegexOptions options = RegexOptions.None) =>
            Regex.Replace(@this, pattern, replacement, options);

        [Pure]
        public string ReReplace(Regex regex, string replacement) =>
            regex.Replace(@this, replacement);

        [Pure]
        public string ReReplace([RegexPattern] string pattern, MatchEvaluator evaluator, RegexOptions options = RegexOptions.None) =>
            Regex.Replace(@this, pattern, evaluator, options);

        [Pure]
        public string ReReplace(Regex regex, MatchEvaluator evaluator) =>
            regex.Replace(@this, evaluator);

        [Pure]
        public string[] ReSplit([RegexPattern] string pattern, RegexOptions options = RegexOptions.None) =>
            Regex.Split(@this, pattern, options);

        [Pure]
        public string[] ReSplit(Regex regex, int count = 0) =>
            regex.Split(@this, count);

        [Pure]
        public string ReUnescape() =>
            Regex.Unescape(@this);
    }

    extension(ReadOnlySpan<char> @this)
    {
        [Pure]
        public Regex.ValueMatchEnumerator ReMatchesEnumerate(Regex regex) =>
            regex.EnumerateMatches(@this);

        [Pure]
        public Regex.ValueSplitEnumerator ReSplitEnumerate([RegexPattern] string pattern, RegexOptions options = RegexOptions.None) =>
            Regex.EnumerateSplits(@this, pattern, options);

        [Pure]
        public Regex.ValueSplitEnumerator ReSplitEnumerate(Regex regex, int count = 0) =>
            regex.EnumerateSplits(@this, count);
    }
}