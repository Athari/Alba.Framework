using System;
using System.IO;
using System.Text;
using Alba.Framework.Text;

namespace Alba.Framework.Sys
{
    public static class ExceptionExts
    {
        public static string GetFullMessage (this Exception @this)
        {
            var sb = new StringBuilder(@this.Message);
            for (Exception e = @this.InnerException; e != null; e = e.InnerException)
                sb.AppendSentence(e.Message);
            return sb.ToString().SingleLine();
        }

        public static bool IsIOException (this Exception @this)
        {
            return @this is IOException || @this is UnauthorizedAccessException;
        }
    }
}