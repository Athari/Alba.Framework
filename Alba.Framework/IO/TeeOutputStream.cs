using System.Collections.ObjectModel;

namespace Alba.Framework.IO;

/// <summary>
/// Writing to multiple output streams. Position and length properties and methods return values for the first stream, but change both. Reading is not supported.
/// </summary>
public class TeeOutputStream : Stream
{
    private readonly List<Stream> _streams;
    private readonly ReadOnlyCollection<Stream> _streamsReadOnly;

    public TeeOutputStream(params IList<Stream> streams)
    {
        Guard.HasSizeGreaterThan(streams, 0);
        _streams = [ ];
        _streamsReadOnly = new(_streams);
        _streams.AddRange(streams);
    }

    public ICollection<Stream> Streams => _streamsReadOnly;

    public override bool CanRead => false;

    public override bool CanSeek => _streams.All(s => s.CanSeek);

    public override bool CanWrite => _streams.All(s => s.CanWrite);

    public override long Length => _streams[0].Length;

    public override long Position {
        get => _streams[0].Position;
        set => _streams.ForEach(s => s.Position = value);
    }

    public override void Flush() =>
        _streams.ForEach(s => s.Flush());

    public override long Seek(long offset, SeekOrigin origin) =>
        _streams.Select(s => s.Seek(offset, origin)).First();

    public override void SetLength(long value) =>
        _streams.ForEach(s => s.SetLength(value));

    public override int Read(byte[] buffer, int offset, int count) =>
        throw new NotSupportedException();

    public override void Write(byte[] buffer, int offset, int count) =>
        _streams.ForEach(s => s.Write(buffer, offset, count));
}