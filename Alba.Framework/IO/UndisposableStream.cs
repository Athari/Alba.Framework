namespace Alba.Framework.IO;

internal class UndisposableStream(Stream stream) : Stream
{
    public override bool CanRead => stream.CanRead;
    public override bool CanSeek => stream.CanSeek;
    public override bool CanTimeout => stream.CanTimeout;
    public override bool CanWrite => stream.CanWrite;
    public override long Length => stream.Length;

    public override int ReadTimeout
    {
        get => stream.ReadTimeout;
        set => stream.ReadTimeout = value;
    }

    public override long Position
    {
        get => stream.Position;
        set => stream.Position = value;
    }

    public override int WriteTimeout
    {
        get => stream.WriteTimeout;
        set => stream.WriteTimeout = value;
    }

    public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken ct) =>
        stream.CopyToAsync(destination, bufferSize, ct);

    public override void Flush() =>
        stream.Flush();

    public override Task FlushAsync(CancellationToken ct) =>
        stream.FlushAsync(ct);

    public override int Read(byte[] buffer, int offset, int count) =>
        stream.Read(buffer, offset, count);

    public override int Read(Span<byte> buffer) =>
        stream.Read(buffer);

    public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken ct = new()) =>
        stream.ReadAsync(buffer, ct);

    public override int ReadByte() =>
        stream.ReadByte();

    public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback? callback, object? state) =>
        stream.BeginRead(buffer, offset, count, callback, state);

    public override int EndRead(IAsyncResult asyncResult) =>
        stream.EndRead(asyncResult);

    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken ct) =>
        stream.ReadAsync(buffer, offset, count, ct);

    public override long Seek(long offset, SeekOrigin origin) =>
        stream.Seek(offset, origin);

    public override void SetLength(long value) =>
        stream.SetLength(value);

    public override void Write(byte[] buffer, int offset, int count) =>
        stream.Write(buffer, offset, count);

    public override void Write(ReadOnlySpan<byte> buffer) =>
        stream.Write(buffer);

    public override void WriteByte(byte value) =>
        stream.WriteByte(value);

    public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback? callback, object? state) =>
        stream.BeginWrite(buffer, offset, count, callback, state);

    public override void EndWrite(IAsyncResult asyncResult) =>
        stream.EndWrite(asyncResult);

    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken ct) =>
        stream.WriteAsync(buffer, offset, count, ct);

    public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken ct = new()) =>
        stream.WriteAsync(buffer, ct);

    public override void Close() { }
}