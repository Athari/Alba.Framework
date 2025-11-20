using System.Net;

namespace Alba.Framework.Text;

public static partial class StringExts
{
    extension(string @this)
    {
        [Pure]
        public string HtmlEncode() => WebUtility.HtmlEncode(@this);

        [Pure]
        public string HtmlDecode() => WebUtility.HtmlDecode(@this);

        [Pure]
        public string UrlEncode() => WebUtility.UrlEncode(@this);

        [Pure]
        public string UrlDecode() => WebUtility.UrlDecode(@this);
    }
}