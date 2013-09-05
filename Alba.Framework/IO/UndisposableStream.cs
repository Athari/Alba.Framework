﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Alba.Framework.IO
{
    internal class UndisposableStream : Stream
    {
        private readonly Stream _stream;

        public UndisposableStream (Stream stream)
        {
            _stream = stream;
        }

        public override bool CanRead
        {
            get { return _stream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return _stream.CanSeek; }
        }

        public override bool CanTimeout
        {
            get { return _stream.CanTimeout; }
        }

        public override bool CanWrite
        {
            get { return _stream.CanWrite; }
        }

        public override long Length
        {
            get { return _stream.Length; }
        }

        public override int ReadTimeout
        {
            get { return _stream.ReadTimeout; }
            set { _stream.ReadTimeout = value; }
        }

        public override long Position
        {
            get { return _stream.Position; }
            set { _stream.Position = value; }
        }

        public override int WriteTimeout
        {
            get { return _stream.WriteTimeout; }
            set { _stream.WriteTimeout = value; }
        }

        public override Task CopyToAsync (Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            return _stream.CopyToAsync(destination, bufferSize, cancellationToken);
        }

        public override void Flush ()
        {
            _stream.Flush();
        }

        public override Task FlushAsync (CancellationToken cancellationToken)
        {
            return _stream.FlushAsync(cancellationToken);
        }

        public override int Read (byte[] buffer, int offset, int count)
        {
            return _stream.Read(buffer, offset, count);
        }

        public override int ReadByte ()
        {
            return _stream.ReadByte();
        }

        public override IAsyncResult BeginRead (byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return _stream.BeginRead(buffer, offset, count, callback, state);
        }

        public override int EndRead (IAsyncResult asyncResult)
        {
            return _stream.EndRead(asyncResult);
        }

        public override Task<int> ReadAsync (byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return _stream.ReadAsync(buffer, offset, count, cancellationToken);
        }

        public override long Seek (long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset, origin);
        }

        public override void SetLength (long value)
        {
            _stream.SetLength(value);
        }

        public override void Write (byte[] buffer, int offset, int count)
        {
            _stream.Write(buffer, offset, count);
        }

        public override void WriteByte (byte value)
        {
            _stream.WriteByte(value);
        }

        public override IAsyncResult BeginWrite (byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return _stream.BeginWrite(buffer, offset, count, callback, state);
        }

        public override void EndWrite (IAsyncResult asyncResult)
        {
            _stream.EndWrite(asyncResult);
        }

        public override Task WriteAsync (byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return _stream.WriteAsync(buffer, offset, count, cancellationToken);
        }

        public override void Close ()
        {
            //_stream.Close();
        }
    }
}