using System;
using System.Diagnostics;
using System.Linq;
using Alba.Framework.Collections;
using Alba.Framework.Sys;
using Alba.Framework.Text;

// ReSharper disable StaticFieldInGenericType
namespace Alba.Framework.Logs
{
    public class Log<T> : ILog
    {
        private static readonly string FullTypeName = typeof(T).GetFullSharpName();
        private static readonly string MediumTypeName = typeof(T).GetMediumName();
        private static readonly string TypeName = typeof(T).Name;
        private readonly TraceSource _source;
        private readonly SimpleMonitor _reentrancyMonitor = new SimpleMonitor();

        static Log ()
        {
            int genericGrave = FullTypeName.IndexOf('`');
            if (genericGrave != -1) {
                FullTypeName = "{0}<{1}>".Fmt(
                    FullTypeName.Sub(0, genericGrave),
                    typeof(T).GenericTypeArguments.Select(t => t.Name).JoinString(","));
            }
        }

        public Log (TraceSource source)
        {
            _source = source;
        }

        public void Trace (string message, string detailedMessage)
        {
            AddEntry(TraceEventType.Verbose, message, detailedMessage, null);
        }

        public void Info (string message, string detailedMessage)
        {
            AddEntry(TraceEventType.Information, message, detailedMessage, null);
        }

        public void Warning (string message, string detailedMessage)
        {
            AddEntry(TraceEventType.Warning, message, detailedMessage, null);
        }

        public void Error (string message, string detailedMessage)
        {
            AddEntry(TraceEventType.Error, message, detailedMessage, null);
        }

        public void Trace (string message, Exception exception = null)
        {
            AddEntry(TraceEventType.Verbose, message, null, exception);
        }

        public void Info (string message, Exception exception = null)
        {
            AddEntry(TraceEventType.Information, message, null, exception);
        }

        public void Warning (string message, Exception exception = null)
        {
            AddEntry(TraceEventType.Warning, message, null, exception);
        }

        public void Error (string message, Exception exception = null)
        {
            AddEntry(TraceEventType.Error, message, null, exception);
        }

        public void Trace (Exception exception)
        {
            AddEntry(TraceEventType.Verbose, null, null, exception);
        }

        public void Info (Exception exception)
        {
            AddEntry(TraceEventType.Information, null, null, exception);
        }

        public void Warning (Exception exception)
        {
            AddEntry(TraceEventType.Warning, null, null, exception);
        }

        public void Error (Exception exception)
        {
            AddEntry(TraceEventType.Error, null, null, exception);
        }

        private void AddEntry (TraceEventType eventType, string message, string detailedMessage, Exception exception)
        {
            using (_reentrancyMonitor.Enter()) {
                if (_reentrancyMonitor.BusyCount > 1)
                    return;

                bool hasMessage = !message.IsNullOrEmpty();
                bool hasDetailedMessage = !detailedMessage.IsNullOrEmpty();
                bool hasException = exception != null;
                var entry = new LogEntry {
                    Exception = exception,
                    TypeName = TypeName,
                    MediumTypeName = MediumTypeName,
                    FullTypeName = FullTypeName,
                };
                if (hasMessage && hasException) {
                    entry.Message = message.AppendSentence(exception.GetFullMessage());
                    entry.DetailedMessage = "Exception: {0}".Fmt(exception);
                }
                else if (hasMessage && hasDetailedMessage) {
                    entry.Message = message;
                    entry.DetailedMessage = detailedMessage;
                }
                else if (hasMessage) {
                    entry.Message = message;
                }
                else if (hasException) {
                    entry.Message = exception.GetFullMessage();
                    entry.DetailedMessage = "Exception: {0}".Fmt(exception);
                }
                else if (hasDetailedMessage) {
                    entry.Message = detailedMessage;
                }
                else {
                    entry.Message = "{0} message in {1}".Fmt(eventType, TypeName);
                }
                _source.TraceData(eventType, entry);
            }
        }

        private class SimpleMonitor : IDisposable
        {
            public int BusyCount { get; private set; }

            public IDisposable Enter ()
            {
                ++BusyCount;
                return this;
            }

            public void Dispose ()
            {
                --BusyCount;
            }
        }
    }
}