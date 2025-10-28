using System.Text.RegularExpressions;
using Alba.Framework.Collections;

namespace Alba.Framework.Text;

[PublicAPI]
public static class MatchExts
{
    extension(Match @this)
    {
        public string Get(int groupNum = 1) =>
            @this.Groups[groupNum].Value;

        public string Get(string groupName) =>
            @this.Groups[groupName].Value;

        public IEnumerable<string> GetAll(int groupNum = 1) =>
            @this.Groups[groupNum].Captures.Select(c => c.Value);

        public IEnumerable<string> GetAll(string groupName) =>
            @this.Groups[groupName].Captures.Select(c => c.Value);

        public string GetConcat(int groupNum = 1) =>
            @this.GetAll(groupNum).ConcatString();

        public string GetConcat(string groupName) =>
            @this.GetAll(groupName).ConcatString();

        public int Count(int groupNum = 1) =>
            @this.Groups[groupNum].Captures.Count;

        public int Count(string groupName) =>
            @this.Groups[groupName].Captures.Count;
    }
}