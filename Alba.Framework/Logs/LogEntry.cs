using System;

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
            return string.Format("{{LogEntry Message='{0}'}}", Message);
        }
    }
}