using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Alba.Framework.Text
{
    public static class MatchCollectionExts
    {
        public static IEnumerable<string> Get (this MatchCollection @this, int groupNum = 1)
        {
            return @this.Cast<Match>().Select(m => m.Get(groupNum));
        }

        public static IEnumerable<string> Get (this MatchCollection @this, string groupName)
        {
            return @this.Cast<Match>().Select(m => m.Get(groupName));
        }
    }
}