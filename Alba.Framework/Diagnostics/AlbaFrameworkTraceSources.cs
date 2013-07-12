using System.Diagnostics;

namespace Alba.Framework.Diagnostics
{
    public static class AlbaFrameworkTraceSources
    {
        public const string BaseTraceSourceName = "Alba.Framework.";
        public const string SerializationTraceSourceName = BaseTraceSourceName + "Serialization";

        private static TraceSource _serialization;

        public static TraceSource Serialization
        {
            get { return _serialization ?? (_serialization = new TraceSource(SerializationTraceSourceName)); }
        }
    }
}