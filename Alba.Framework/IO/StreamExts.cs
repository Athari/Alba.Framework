namespace Alba.Framework.IO;

[PublicAPI]
public static class StreamExts
{
    public static long SeekBegin(this Stream @this, long offset = 0) => @this.Seek(offset, SeekOrigin.Begin);
    public static long SeekCurrent(this Stream @this, long offset) => @this.Seek(offset, SeekOrigin.Current);
    public static long SeekEnd(this Stream @this, long offset = 0) => @this.Seek(offset, SeekOrigin.End);

    public static Stream ToUndisposable(this Stream @this) => new UndisposableStream(@this);
}