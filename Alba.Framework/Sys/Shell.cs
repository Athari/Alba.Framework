using System;
using System.IO;
using Alba.Framework.Interop;

namespace Alba.Framework.Sys
{
    public static class Shell
    {
        private const string ShellOpenCommand = "open";

        public static string DefaultBrowserName
        {
            get { return QueryHttpAssoc(ASSOCSTR.FRIENDLYAPPNAME); }
        }

        public static Browser DefaultBrowser
        {
            get
            {
                string exe = Path.GetFileNameWithoutExtension(QueryHttpAssoc(ASSOCSTR.EXECUTABLE));
                if (exe == null)
                    return Browser.Unknown;
                switch (exe.ToLower()) {
                    case "chrome":
                    case "sleipnir":
                    case "lunascape":
                    case "chromium":
                    case "iron":
                    case "dragon":
                    case "rockmelt":
                    case "konqueror":
                    case "nichrome": // rambler
                    case "browser": // yandex
                    case "internet": // mail.ru
                    case "maxthon": // ie+c
                        return Browser.Chrome;
                    case "iexplore":
                    case "360se":
                    case "sbframe":
                    case "greenbrowser":
                    case "theworld":
                    case "avant": // ie+f+c
                        return Browser.Explorer;
                    case "firefox":
                    case "seamonkey":
                    case "palemoon":
                        return Browser.Firefox;
                    case "opera":
                        return Browser.Opera;
                    case "safari":
                        return Browser.Safari;
                    default:
                        return Browser.Unknown;
                }
            }
        }

        private static string QueryHttpAssoc (ASSOCSTR assocStr)
        {
            return Native.AssocQueryString(ASSOCF.NONE, assocStr, Uri.UriSchemeHttp, ShellOpenCommand);
        }

        public enum Browser
        {
            Unknown,
            Chrome,
            Explorer,
            Firefox,
            Opera,
            Safari,
        }
    }
}