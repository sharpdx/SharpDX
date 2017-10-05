// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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

namespace SharpDX.Multimedia
{
    /// <summary>
    /// Generic sound input stream supporting WAV (Pcm,Float), ADPCM, xWMA sound file formats.
    /// </summary>
    public class SoundStream : Stream
    {
        private Stream input;
        private long startPositionOfData;
        private long length;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundStream"/> class.
        /// </summary>
        /// <param name="stream">The sound stream.</param>
        public SoundStream(Stream stream)
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

            FileFormatName = "Unknown";

            // Parse Header
            if (!parser.MoveNext() || parser.Current == null)
            {
                ThrowInvalidFileFormat();
                return;
            }

            // Check that WAVE or XWMA header is present
            FileFormatName = parser.Current.Type;
            if (FileFormatName != "WAVE" && FileFormatName != "XWMA")
                throw new InvalidOperationException("Unsupported " + FileFormatName + " file format. Only WAVE or XWMA");

            // Parse inside the first chunk
            parser.Descend();

            // Get all the chunk
            var chunks = parser.GetAllChunks();

            // Get "fmt" chunk
            var fmtChunk = Chunk(chunks, "fmt ");
            if (fmtChunk.Size < sizeof(WaveFormat.__PcmNative))
                ThrowInvalidFileFormat();

            try
            {
                Format = WaveFormat.MarshalFrom(fmtChunk.GetData());
            }
            catch (InvalidOperationException ex)
            {
                ThrowInvalidFileFormat(ex);
            }

            // If XWMA
            if (FileFormatName == "XWMA")
            {
                // Check that format is Wma
                if (Format.Encoding != WaveFormatEncoding.Wmaudio2 && Format.Encoding != WaveFormatEncoding.Wmaudio3)
                    ThrowInvalidFileFormat();

                // Check for "dpds" chunk
                // Get the dpds decoded packed cumulative bytes
                var dpdsChunk = Chunk(chunks, "dpds");
                DecodedPacketsInfo = dpdsChunk.GetDataAsArray<uint>();                
            } else
            {
                switch (Format.Encoding)
                {
                    case WaveFormatEncoding.Pcm:
                    case WaveFormatEncoding.IeeeFloat:
                    case WaveFormatEncoding.Extensible:
                    case WaveFormatEncoding.Adpcm:
                        break;
                    default:
                        ThrowInvalidFileFormat();
                        break;
                }                
            }

            // Check for "data" chunk
            var dataChunk = Chunk(chunks, "data");
            startPositionOfData = dataChunk.DataPosition;
            length = dataChunk.Size;

            input.Position = startPositionOfData;
        }

        protected void ThrowInvalidFileFormat(Exception nestedException = null)
        {
            throw new InvalidOperationException("Invalid " + FileFormatName + " file format", nestedException);
        }

        /// <summary>
        /// Gets the decoded packets info.
        /// </summary>
        /// <remarks>
        /// This property is only valid for XWMA stream.</remarks>
        public uint[] DecodedPacketsInfo { get; private set; }

        /// <summary>
        /// Gets the wave format of this instance.
        /// </summary>
        public WaveFormat Format { get; protected set; }

        /// <summary>
        /// Converts this stream to a DataStream by loading all the data from the source stream.
        /// </summary>
        /// <returns></returns>
        public DataStream ToDataStream()
        {
            var buffer = new byte[Length];
            if ( Read(buffer, 0, (int)Length) != Length)
                throw new InvalidOperationException("Unable to get a valid DataStream");

            return DataStream.Create(buffer, true, true);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SharpDX.Multimedia.SoundStream"/> to <see cref="SharpDX.DataStream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator DataStream(SoundStream stream)
        {
            return stream.ToDataStream();
        }

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
                return input.Position - startPositionOfData;
            }
            set
            {
                Seek(value, SeekOrigin.Begin);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (input != null)
            {
                input.Dispose();
                input = null;
            }
            base.Dispose(disposing);
        }

        protected RiffChunk Chunk(IEnumerable<RiffChunk> chunks, string id)
        {
            RiffChunk chunk = null;
            foreach (var riffChunk in chunks)
            {
                if (riffChunk.Type == id)
                {
                    chunk = riffChunk;
                    break;
                }
            }
            if (chunk == null || chunk.Type != id)
                throw new InvalidOperationException("Invalid " + FileFormatName + " file format");
            return chunk;
        }

        private string FileFormatName { get; set; }

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
                    newPosition = startPositionOfData + offset;
                    break;
                case SeekOrigin.Current:
                    newPosition = input.Position + offset;
                    break;
                case SeekOrigin.End:
                    newPosition = startPositionOfData + length + offset;
                    break;
            }

            if (newPosition < startPositionOfData || newPosition > (startPositionOfData+length))
                throw new InvalidOperationException("Cannot seek outside the range of this stream");

            return input.Seek(newPosition, SeekOrigin.Begin);
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
            return input.Read(buffer, offset, Math.Min(count, (int)Math.Max(startPositionOfData + length - input.Position, 0)));
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