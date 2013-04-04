using System;
using Alba.Framework.Text;

namespace Alba.Framework.Logs
{
    public class LogEntry
    {
        public Exception Exception { get; set; }
        public string Message { get; set; }
        public string DetailedMessage { get; set; }
        public string TypeName { get; set; }
        public string FullTypeName { get; set; }

        public override string ToString ()
        {
            return "{{LogEntry Message='{0}'}}".Fmt(Message);
        }
    }
}