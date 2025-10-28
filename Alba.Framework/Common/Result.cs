namespace Alba.Framework.Common;

public readonly record struct Result<T>(T Value, Exception? Error, bool IsSuccess)
{
    public bool IsCancel => !IsSuccess && Error == null;

    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsError => !IsSuccess && Error != null;

    public T GetValueOrThrow()
    {
        if (IsError)
            Error.Rethrow();
        return Value;
    }
}

public static class Result
{
    public static Result<T> Success<T>(T value) => new(value, null, true);
    public static Result<T> Cancel<T>() => new(default!, null, false);
    public static Result<T> Error<T>(Exception error) => new(default!, error, false);
}