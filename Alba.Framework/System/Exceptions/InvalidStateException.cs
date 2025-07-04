namespace Alba.Framework;

public class InvalidStateException : Exception
{
    private const string InvalidObjectStateError = "Invalid object state.";

    public InvalidStateException() { }

    public InvalidStateException(string message = InvalidObjectStateError) : base(message) { }

    public InvalidStateException(Exception inner) : base(InvalidObjectStateError, inner) { }

    public InvalidStateException(string message, Exception inner) : base(message, inner) { }
}