using System.Collections.ObjectModel;
using Alba.Framework.Collections;

namespace Alba.Framework.Diagnostics;

public class UnhandledExceptionsEventArgs(
    ReadOnlyCollection<Exception> exceptions, UnhandledExceptionSource source, bool canHandle = true)
    : EventArgs
{
    public IReadOnlyCollection<Exception> Exceptions { get; } = exceptions;
    public UnhandledExceptionSource Source { get; } = source;
    public bool CanHandle { get; } = canHandle;
    public bool Handled { get; set; }

    public UnhandledExceptionsEventArgs(Exception exception, UnhandledExceptionSource source, bool canHandle = true)
        : this(new ReadOnlyCollection<Exception>([ exception ]), source, canHandle) { }

    public string FullMessage => Exceptions.Select(e => e.GetFullMessage()).JoinString(Environment.NewLine);
}