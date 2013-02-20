using System.Diagnostics;

// ReSharper disable MethodOverloadWithOptionalParameter
namespace Alba.Framework.Logs
{
    public static class TraceSourceExts
    {
        public static void TraceEvent (this TraceSource @this, TraceEventType eventType, string message)
        {
            @this.TraceEvent(eventType, 0, message);
        }

        public static void TraceEvent (this TraceSource @this, TraceEventType eventType, string format, params object[] args)
        {
            @this.TraceEvent(eventType, 0, format, args);
        }

        public static void TraceVerboseEvent (this TraceSource @this, string message)
        {
            @this.TraceEvent(TraceEventType.Verbose, 0, message);
        }

        public static void TraceVerboseEvent (this TraceSource @this, string format, params object[] args)
        {
            @this.TraceEvent(TraceEventType.Verbose, 0, format, args);
        }

        public static void TraceInfoEvent (this TraceSource @this, string message)
        {
            @this.TraceEvent(TraceEventType.Information, 0, message);
        }

        public static void TraceInfoEvent (this TraceSource @this, string format, params object[] args)
        {
            @this.TraceEvent(TraceEventType.Information, 0, format, args);
        }

        public static void TraceWarningEvent (this TraceSource @this, string message)
        {
            @this.TraceEvent(TraceEventType.Warning, 0, message);
        }

        public static void TraceWarningEvent (this TraceSource @this, string format, params object[] args)
        {
            @this.TraceEvent(TraceEventType.Warning, 0, format, args);
        }

        public static void TraceErrorEvent (this TraceSource @this, string message)
        {
            @this.TraceEvent(TraceEventType.Error, 0, message);
        }

        public static void TraceErrorEvent (this TraceSource @this, string format, params object[] args)
        {
            @this.TraceEvent(TraceEventType.Error, 0, format, args);
        }

        public static void TraceData (this TraceSource @this, TraceEventType eventType, object data)
        {
            @this.TraceData(eventType, 0, data);
        }

        public static void TraceData (this TraceSource @this, TraceEventType eventType, params object[] data)
        {
            @this.TraceData(eventType, 0, data);
        }

        public static void TraceVerboseData (this TraceSource @this, object data)
        {
            @this.TraceData(TraceEventType.Verbose, 0, data);
        }

        public static void TraceVerboseData (this TraceSource @this, params object[] data)
        {
            @this.TraceData(TraceEventType.Verbose, 0, data);
        }

        public static void TraceInfoData (this TraceSource @this, object data)
        {
            @this.TraceData(TraceEventType.Information, 0, data);
        }

        public static void TraceInfoData (this TraceSource @this, params object[] data)
        {
            @this.TraceData(TraceEventType.Information, 0, data);
        }

        public static void TraceWarningData (this TraceSource @this, object data)
        {
            @this.TraceData(TraceEventType.Warning, 0, data);
        }

        public static void TraceWarningData (this TraceSource @this, params object[] data)
        {
            @this.TraceData(TraceEventType.Warning, 0, data);
        }

        public static void TraceErrorData (this TraceSource @this, object data)
        {
            @this.TraceData(TraceEventType.Error, 0, data);
        }

        public static void TraceErrorData (this TraceSource @this, params object[] data)
        {
            @this.TraceData(TraceEventType.Error, 0, data);
        }
    }
}