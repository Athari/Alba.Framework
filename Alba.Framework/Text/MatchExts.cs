using System.Text.RegularExpressions;

namespace Alba.Framework.Text
{
    public static class MatchExts
    {
        public static string Get (this Match @this, int groupNum = 1)
        {
            return @this.Groups[groupNum].Value;
        }

        public static string Get (this Match @this, string groupName)
        {
            return @this.Groups[groupName].Value;
        }
    }
}