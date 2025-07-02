using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Alba.Framework.Diagnostics;

public static class AlbaFrameworkTraceSources
{
    public const string BaseTraceSourceName = "Alba.Framework.";
    public const string SerializationTraceSourceName = BaseTraceSourceName + "Serialization";

    [field: MaybeNull]
    public static TraceSource Serialization => field ??= new(SerializationTraceSourceName);
}