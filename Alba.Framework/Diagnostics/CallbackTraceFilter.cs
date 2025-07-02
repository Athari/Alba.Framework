using System.Diagnostics;

namespace Alba.Framework.Diagnostics;

public delegate bool ShouldTraceCallback(TraceEventCache? cache, string source, TraceEventType eventType, int id);

public delegate bool ShouldTraceDetailedCallback(TraceEventCache? cache, string source,
    TraceEventType eventType, int id, string? formatOrMessage, object?[]? args, object? data1, object?[]? data);

[PublicAPI]
public class CallbackTraceFilter : TraceFilter
{
    private readonly ShouldTraceCallback? _shouldTraceCallback;
    private readonly ShouldTraceDetailedCallback? _shouldTraceDetailedCallback;

    public CallbackTraceFilter(ShouldTraceCallback shouldTraceCallback)
    {
        Guard.IsNotNull(shouldTraceCallback, nameof(shouldTraceCallback));
        _shouldTraceCallback = shouldTraceCallback;
    }

    public CallbackTraceFilter(ShouldTraceDetailedCallback shouldTraceDetailedCallback)
    {
        Guard.IsNotNull(shouldTraceDetailedCallback, nameof(shouldTraceDetailedCallback));
        _shouldTraceDetailedCallback = shouldTraceDetailedCallback;
    }

    public override bool ShouldTrace(TraceEventCache? cache, string source,
        TraceEventType eventType, int id, string? formatOrMessage, object?[]? args, object? data1, object?[]? data)
    {
        if (_shouldTraceCallback != null)
            return _shouldTraceCallback(cache, source, eventType, id);
        else if (_shouldTraceDetailedCallback != null)
            return _shouldTraceDetailedCallback(cache, source, eventType, id, formatOrMessage, args, data1, data);
        return false; // should never happen
    }
}