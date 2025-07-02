namespace Alba.Framework.IO;

[PublicAPI]
public static class StreamExts
{
    public static Stream ToUndisposable(this Stream @this) =>
        new UndisposableStream(@this);
}