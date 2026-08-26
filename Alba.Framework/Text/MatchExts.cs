using System.Text.RegularExpressions;
using Alba.Framework.Collections;

namespace Alba.Framework.Text;

public static partial class MatchExts
{
    extension(Match @this)
    {
        public string Value =>
            @this.Groups[1].Value;

        public string Get(int groupIndex = 1) =>
            @this.Groups[groupIndex].Value;

        public string Get(string groupName) =>
            @this.Groups[groupName].Value;

        public ReadOnlySpan<char> GetSpan(int groupIndex = 1) =>
            @this.Groups[groupIndex].ValueSpan;

        public ReadOnlySpan<char> GetSpan(string groupName) =>
            @this.Groups[groupName].ValueSpan;

        public IEnumerable<string> GetAll(int groupNum = 1) =>
            @this.Groups[groupNum].Captures.Select(c => c.Value);

        public IEnumerable<string> GetAll(string groupName) =>
            @this.Groups[groupName].Captures.Select(c => c.Value);

        public string GetConcat(int groupNum = 1) =>
            @this.GetAll(groupNum).ConcatString();

        public string GetConcat(string groupName) =>
            @this.GetAll(groupName).ConcatString();

        public string? TryGet(int groupNum = 1)
        {
            var group = @this.Groups[groupNum];
            return group.Success ? group.Value : null;
        }

        public string? TryGet(string groupName)
        {
            var group = @this.Groups[groupName];
            return group.Success ? group.Value : null;
        }

        public int Count(int groupNum = 1) =>
            @this.Groups[groupNum].Captures.Count;

        public int Count(string groupName) =>
            @this.Groups[groupName].Captures.Count;
    }
}