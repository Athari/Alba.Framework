using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Alba.Framework.Collections;

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

        public static IEnumerable<string> GetAll (this Match @this, int groupNum = 1)
        {
            return @this.Groups[groupNum].Captures.Cast<Capture>().Select(c => c.Value);
        }

        public static IEnumerable<string> GetAll (this Match @this, string groupName)
        {
            return @this.Groups[groupName].Captures.Cast<Capture>().Select(c => c.Value);
        }

        public static string GetConcat (this Match @this, int groupNum = 1)
        {
            return @this.GetAll(groupNum).ConcatString();
        }

        public static string GetConcat (this Match @this, string groupName)
        {
            return @this.GetAll(groupName).ConcatString();
        }

        public static int Count (this Match @this, int groupNum = 1)
        {
            return @this.Groups[groupNum].Captures.Count;
        }

        public static int Count (this Match @this, string groupName)
        {
            return @this.Groups[groupName].Captures.Count;
        }
    }
}