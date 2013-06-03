using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Alba.Framework.Sys
{
    public static class WebUtils
    {
        public static string ConstructQuery (IEnumerable<KeyValuePair<string, string>> args)
        {
            string sep = "";
            var sb = new StringBuilder();
            foreach (KeyValuePair<string, string> arg in args) {
                sb.Append(sep);
                sb.Append(WebUtility.UrlEncode(arg.Key));
                sb.Append("=");
                sb.Append(WebUtility.UrlEncode(arg.Value));
                sep = "&";
            }
            return sb.ToString();
        }
    }
}