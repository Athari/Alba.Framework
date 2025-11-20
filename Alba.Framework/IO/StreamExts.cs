namespace Alba.Framework.IO;

public static class StreamExts
{
    extension(Stream @this)
    {
        public long SeekBegin(long offset = 0) => @this.Seek(offset, SeekOrigin.Begin);
        public long SeekCurrent(long offset) => @this.Seek(offset, SeekOrigin.Current);
        public long SeekEnd(long offset = 0) => @this.Seek(offset, SeekOrigin.End);

        public Stream ToUndisposable() => new UndisposableStream(@this);
    }
}