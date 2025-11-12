namespace Alba.Framework;

public class NeverException : InvalidOperationException
{
    private const string DefaultMessage = "Internal logic error. This error should have never happened.";

    public NeverException() : base(DefaultMessage) { }
    public NeverException(string message) : base(message) { }
    public NeverException(string message, Exception inner) : base(message, inner) { }
}