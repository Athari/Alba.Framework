using System.Text.RegularExpressions;

namespace Alba.Framework.Text;

public static class MatchCollectionExts
{
    extension(MatchCollection @this)
    {
        public IEnumerable<string> Get(int groupNum = 1) =>
            @this.Select(m => m.Get(groupNum));

        public IEnumerable<string> Get(string groupName) =>
            @this.Select(m => m.Get(groupName));
    }
}