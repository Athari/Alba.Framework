using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Alba.Framework.Collections;
using Alba.Framework.Sys;

namespace Alba.Framework.Diagnostics
{
    public class UnhandledExceptionsEventArgs : EventArgs
    {
        public IReadOnlyCollection<Exception> Exceptions { get; private set; }
        public UnhandledExceptionSource Source { get; private set; }
        public bool CanHandle { get; private set; }
        public bool Handled { get; set; }

        public UnhandledExceptionsEventArgs (ReadOnlyCollection<Exception> exceptions, UnhandledExceptionSource source, bool canHandle = true)
        {
            Exceptions = exceptions;
            Source = source;
            CanHandle = canHandle;
        }

        public UnhandledExceptionsEventArgs (Exception exception, UnhandledExceptionSource source, bool canHandle = true)
            : this(new ReadOnlyCollection<Exception>(new[] { exception }), source, canHandle)
        {}

        public string FullMessage
        {
            get { return Exceptions.Select(e => e.GetFullMessage()).JoinString(Environment.NewLine); }
        }
    }
}