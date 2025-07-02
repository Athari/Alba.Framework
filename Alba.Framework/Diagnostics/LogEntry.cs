namespace Alba.Framework.Diagnostics;

public class LogEntry
{
    public Exception? Exception { get; set; }
    public string Message { get; set; } = "";
    public string DetailedMessage { get; set; } = "";
    public string TypeName { get; set; } = "";
    public string MediumTypeName { get; set; } = "";
    public string FullTypeName { get; set; } = "";

    public override string ToString()
    {
        return $"Message='{Message}'\nType={MediumTypeName}";
    }
}