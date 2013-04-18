using System.Diagnostics;

namespace Alba.Framework.Testing
{
    public class TraceRedirectTraceListener : TraceListener
    {
        public override void Write (string message)
        {
            Trace.Write(message);
        }

        public override void WriteLine (string message)
        {
            Trace.WriteLine(message);
        }
    }
}