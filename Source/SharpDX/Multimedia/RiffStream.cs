// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SharpDX.Multimedia
{
    /// <summary>
    /// Base class for a RIFF stream (WAV, xWMA... etc.)
    /// </summary>
    public abstract class RiffStream : Stream
    {
        private readonly bool isOwnerOfInput;
        protected Stream input;
        protected long startPositionOfWXMA;
        protected long length;

        /// <summary>
        /// Initializes a new instance of the <see cref="XWMAStream"/> class.
        /// </summary>
        /// <param name="xwmaFile">The xwma file.</param>
        protected RiffStream(string xwmaFile)
        {
            input = new FileStream(xwmaFile, FileMode.Open, FileAccess.Read);
            isOwnerOfInput = true;
            Initialize(input);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XWMAStream"/> class.
        /// </summary>
        /// <param name="stream">The xwma stream.</param>
        protected RiffStream(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            input = stream;
            Initialize(stream);
        }

        /// <summary>
        /// Initializes the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        private unsafe void Initialize(Stream stream)
        {
            var parser = new RiffParser(stream);

            // Parse Header
            parser.MoveNext();
            if (parser.Current.Type != FileFormatName)
                ThrowInvalidFileFormat();

            // Parse inside the first chunk
            parser.Descend();

            // Get all the chunk
            var chunks = parser.GetAllChunks();
            Initialize(chunks);

            // Check for "data" chunk
            var dataChunk = Chunk(chunks, "data");
            startPositionOfWXMA = dataChunk.DataPosition;
            length = dataChunk.Size;

            input.Position = startPositionOfWXMA;
        }

        protected void ThrowInvalidFileFormat()
        {
            throw new InvalidOperationException("Invalid " + FileFormatName + " file format");
        }

        /// <summary>
        /// Initializes the specified stream.
        /// </summary>
        protected abstract unsafe void Initialize(IEnumerable<RiffChunk> chunks);

        /// <summary>
        /// Gets the wave format of this instance.
        /// </summary>
        public WaveFormat WaveFormat { get; protected set; }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports reading.
        /// </summary>
        /// <returns>true if the stream supports reading; otherwise, false.
        ///   </returns>
        public override bool CanRead
        {
            get { return true; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports seeking.
        /// </summary>
        /// <returns>true if the stream supports seeking; otherwise, false.
        ///   </returns>
        public override bool CanSeek
        {
            get { return true; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports writing.
        /// </summary>
        /// <returns>true if the stream supports writing; otherwise, false.
        ///   </returns>
        public override bool CanWrite
        {
            get { return false; }
        }

        /// <summary>
        /// When overridden in a derived class, gets or sets the position within the current stream.
        /// </summary>
        /// <returns>
        /// The current position within the stream.
        ///   </returns>
        ///   
        /// <exception cref="T:System.IO.IOException">
        /// An I/O error occurs.
        ///   </exception>
        ///   
        /// <exception cref="T:System.NotSupportedException">
        /// The stream does not support seeking.
        ///   </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">
        /// Methods were called after the stream was closed.
        ///   </exception>
        public override long Position
        {
            get
            {
                return input.Position - startPositionOfWXMA;
            }
            set
            {
                Seek(value, SeekOrigin.Begin);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && isOwnerOfInput && input != null)
            {
                input.Dispose();
                input = null;
            }
            base.Dispose(disposing);
        }

        protected RiffChunk Chunk(IEnumerable<RiffChunk> chunks, string id)
        {
            var chunk = chunks.FirstOrDefault(riff => riff.Type == id);
            if (chunk == null || chunk.Type != id)
                throw new InvalidOperationException("Invalid " + FileFormatName + " file format");
            return chunk;
        }

        protected abstract string FileFormatName { get; }

        /// <summary>
        /// When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        /// <exception cref="T:System.IO.IOException">
        /// An I/O error occurs.
        ///   </exception>
        public override void Flush()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// When overridden in a derived class, sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the <paramref name="origin"/> parameter.</param>
        /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin"/> indicating the reference point used to obtain the new position.</param>
        /// <returns>
        /// The new position within the current stream.
        /// </returns>
        /// <exception cref="T:System.IO.IOException">
        /// An I/O error occurs.
        ///   </exception>
        ///   
        /// <exception cref="T:System.NotSupportedException">
        /// The stream does not support seeking, such as if the stream is constructed from a pipe or console output.
        ///   </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">
        /// Methods were called after the stream was closed.
        ///   </exception>
        public override long Seek(long offset, SeekOrigin origin)
        {
            var newPosition = input.Position;
            switch (origin)
            {
                case SeekOrigin.Begin:
                    newPosition = startPositionOfWXMA + offset;
                    break;
                case SeekOrigin.Current:
                    newPosition = input.Position + offset;
                    break;
                case SeekOrigin.End:
                    newPosition = startPositionOfWXMA + length + offset;
                    break;
            }

            if (newPosition < startPositionOfWXMA || newPosition > (startPositionOfWXMA+length))
                throw new InvalidOperationException("Cannot seek outside the range of this stream");

            return input.Seek(offset, origin);
        }

        /// <summary>
        /// When overridden in a derived class, sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        /// <exception cref="T:System.IO.IOException">
        /// An I/O error occurs.
        ///   </exception>
        ///   
        /// <exception cref="T:System.NotSupportedException">
        /// The stream does not support both writing and seeking, such as if the stream is constructed from a pipe or console output.
        ///   </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">
        /// Methods were called after the stream was closed.
        ///   </exception>
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset"/> and (<paramref name="offset"/> + <paramref name="count"/> - 1) replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        /// The sum of <paramref name="offset"/> and <paramref name="count"/> is larger than the buffer length.
        ///   </exception>
        ///   
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="buffer"/> is null.
        ///   </exception>
        ///   
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="offset"/> or <paramref name="count"/> is negative.
        ///   </exception>
        ///   
        /// <exception cref="T:System.IO.IOException">
        /// An I/O error occurs.
        ///   </exception>
        ///   
        /// <exception cref="T:System.NotSupportedException">
        /// The stream does not support reading.
        ///   </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">
        /// Methods were called after the stream was closed.
        ///   </exception>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if ( (input.Position + count) > length)
                throw new InvalidOperationException("Cannot read more than the length of the stream");
            return input.Read(buffer, offset, count);
        }

        /// <summary>
        /// When overridden in a derived class, gets the length in bytes of the stream.
        /// </summary>
        /// <returns>
        /// A long value representing the length of the stream in bytes.
        ///   </returns>
        ///   
        /// <exception cref="T:System.NotSupportedException">
        /// A class derived from Stream does not support seeking.
        ///   </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">
        /// Methods were called after the stream was closed.
        ///   </exception>
        public override long Length
        {
            get { return length; }
        }

        /// <summary>
        /// When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies <paramref name="count"/> bytes from <paramref name="buffer"/> to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        /// <exception cref="T:System.ArgumentException">
        /// The sum of <paramref name="offset"/> and <paramref name="count"/> is greater than the buffer length.
        ///   </exception>
        ///   
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="buffer"/> is null.
        ///   </exception>
        ///   
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="offset"/> or <paramref name="count"/> is negative.
        ///   </exception>
        ///   
        /// <exception cref="T:System.IO.IOException">
        /// An I/O error occurs.
        ///   </exception>
        ///   
        /// <exception cref="T:System.NotSupportedException">
        /// The stream does not support writing.
        ///   </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">
        /// Methods were called after the stream was closed.
        ///   </exception>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
}