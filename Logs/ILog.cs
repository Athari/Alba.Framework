using System;

namespace Alba.Framework.Logs
{
    public interface ILog
    {
        void Trace (string message, string detailedMessage);
        void Trace (string message, Exception exception = null);
        void Trace (Exception exception);
        void Info (string message, string detailedMessage);
        void Info (string message, Exception exception = null);
        void Info (Exception exception);
        void Warning (string message, string detailedMessage);
        void Warning (string message, Exception exception = null);
        void Warning (Exception exception);
        void Error (string message, string detailedMessage);
        void Error (string message, Exception exception = null);
        void Error (Exception exception);
    }
}