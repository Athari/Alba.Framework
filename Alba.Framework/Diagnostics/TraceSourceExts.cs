using System.Diagnostics;

namespace Alba.Framework.Diagnostics;

public static class TraceSourceExts
{
    extension(TraceSource @this)
    {
        public void TraceEvent(TraceEventType eventType, string message) =>
            @this.TraceEvent(eventType, 0, message);

        public void TraceEvent(TraceEventType eventType, string format, params object[] args) =>
            @this.TraceEvent(eventType, 0, format, args);

        public void TraceVerboseEvent(string message) =>
            @this.TraceEvent(TraceEventType.Verbose, 0, message);

        public void TraceVerboseEvent(string format, params object[] args) =>
            @this.TraceEvent(TraceEventType.Verbose, 0, format, args);

        public void TraceInfoEvent(string message) =>
            @this.TraceEvent(TraceEventType.Information, 0, message);

        public void TraceInfoEvent(string format, params object[] args) =>
            @this.TraceEvent(TraceEventType.Information, 0, format, args);

        public void TraceWarningEvent(string message) =>
            @this.TraceEvent(TraceEventType.Warning, 0, message);

        public void TraceWarningEvent(string format, params object[] args) =>
            @this.TraceEvent(TraceEventType.Warning, 0, format, args);

        public void TraceErrorEvent(string message) =>
            @this.TraceEvent(TraceEventType.Error, 0, message);

        public void TraceErrorEvent(string format, params object[] args) =>
            @this.TraceEvent(TraceEventType.Error, 0, format, args);

        public void TraceData(TraceEventType eventType, object data) =>
            @this.TraceData(eventType, 0, data);

        public void TraceData(TraceEventType eventType, params object[] data) =>
            @this.TraceData(eventType, 0, data);

        public void TraceVerboseData(object data) =>
            @this.TraceData(TraceEventType.Verbose, 0, data);

        public void TraceVerboseData(params object[] data) =>
            @this.TraceData(TraceEventType.Verbose, 0, data);

        public void TraceInfoData(object data) =>
            @this.TraceData(TraceEventType.Information, 0, data);

        public void TraceInfoData(params object[] data) =>
            @this.TraceData(TraceEventType.Information, 0, data);

        public void TraceWarningData(object data) =>
            @this.TraceData(TraceEventType.Warning, 0, data);

        public void TraceWarningData(params object[] data) =>
            @this.TraceData(TraceEventType.Warning, 0, data);

        public void TraceErrorData(object data) =>
            @this.TraceData(TraceEventType.Error, 0, data);

        public void TraceErrorData(params object[] data) =>
            @this.TraceData(TraceEventType.Error, 0, data);
    }
}