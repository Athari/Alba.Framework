using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Alba.Framework.IO
{
    /// <summary>
    /// Writing to multiple output streams. Position and length properties and methods return values for the first stream, but change both. Reading is not supported.
    /// </summary>
    public class TeeOutputStream : Stream
    {
        private readonly List<Stream> _streams;
        private readonly ReadOnlyCollection<Stream> _streamsReadOnly;

        public TeeOutputStream (params Stream[] streams)
        {
            if (streams.Length == 0)
                throw new ArgumentException("One or more streams must be provided.", "streams");
            _streams = new List<Stream>();
            _streamsReadOnly = new ReadOnlyCollection<Stream>(_streams);
            _streams.AddRange(streams);
        }

        public ICollection<Stream> Streams
        {
            get { return _streamsReadOnly; }
        }

        public override void Flush ()
        {
            _streams.ForEach(s => s.Flush());
        }

        public override long Seek (long offset, SeekOrigin origin)
        {
            return _streams.Select(s => s.Seek(offset, origin)).First();
        }

        public override void SetLength (long value)
        {
            _streams.ForEach(s => s.SetLength(value));
        }

        public override int Read (byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override void Write (byte[] buffer, int offset, int count)
        {
            _streams.ForEach(s => s.Write(buffer, offset, count));
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanSeek
        {
            get { return _streams.All(s => s.CanSeek); }
        }

        public override bool CanWrite
        {
            get { return _streams.All(s => s.CanWrite); }
        }

        public override long Length
        {
            get { return _streams[0].Length; }
        }

        public override long Position
        {
            get { return _streams[0].Position; }
            set { _streams.ForEach(s => s.Position = value); }
        }
    }
}