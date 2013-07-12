using System;
using System.Diagnostics;

namespace Alba.Framework.Diagnostics
{
    public delegate bool ShouldTraceCallback (TraceEventCache cache, string source, TraceEventType eventType, int id);

    public delegate bool ShouldTraceDetailedCallback (TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args, object data1, object[] data);

    public class CallbackTraceFilter : TraceFilter
    {
        private readonly ShouldTraceCallback _shouldTraceCallback;
        private readonly ShouldTraceDetailedCallback _shouldTraceDetailedCallback;

        public CallbackTraceFilter (ShouldTraceCallback shouldTraceCallback)
        {
            if (shouldTraceCallback == null)
                throw new ArgumentNullException("shouldTraceCallback");
            _shouldTraceCallback = shouldTraceCallback;
        }

        public CallbackTraceFilter (ShouldTraceDetailedCallback shouldTraceDetailedCallback)
        {
            if (shouldTraceDetailedCallback == null)
                throw new ArgumentNullException("shouldTraceDetailedCallback");
            _shouldTraceDetailedCallback = shouldTraceDetailedCallback;
        }

        public override bool ShouldTrace (TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args, object data1, object[] data)
        {
            if (_shouldTraceCallback != null)
                return _shouldTraceCallback(cache, source, eventType, id);
            else if (_shouldTraceDetailedCallback != null)
                return _shouldTraceDetailedCallback(cache, source, eventType, id, formatOrMessage, args, data1, data);
            return false; // should never happen
        }
    }
}