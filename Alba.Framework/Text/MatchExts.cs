using System.Text.RegularExpressions;
using Alba.Framework.Collections;

namespace Alba.Framework.Text;

[PublicAPI]
public static class MatchExts
{
    public static string Get(this Match @this, int groupNum = 1) =>
        @this.Groups[groupNum].Value;

    public static string Get(this Match @this, string groupName) =>
        @this.Groups[groupName].Value;

    public static IEnumerable<string> GetAll(this Match @this, int groupNum = 1) =>
        @this.Groups[groupNum].Captures.Select(c => c.Value);

    public static IEnumerable<string> GetAll(this Match @this, string groupName) =>
        @this.Groups[groupName].Captures.Select(c => c.Value);

    public static string GetConcat(this Match @this, int groupNum = 1) =>
        @this.GetAll(groupNum).ConcatString();

    public static string GetConcat(this Match @this, string groupName) =>
        @this.GetAll(groupName).ConcatString();

    public static int Count(this Match @this, int groupNum = 1) =>
        @this.Groups[groupNum].Captures.Count;

    public static int Count(this Match @this, string groupName) =>
        @this.Groups[groupName].Captures.Count;
}