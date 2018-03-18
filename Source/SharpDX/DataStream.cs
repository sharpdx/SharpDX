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
// -----------------------------------------------------------------------------
// Original code from SlimDX project. The difference in the implem is that 
// this class doesn't test limit, allowing slightly better performance.
// Greetings to SlimDX Group. Original code published with the following license:
// -----------------------------------------------------------------------------
/*
* Copyright (c) 2007-2011 SlimDX Group
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using System.IO;
using System.Runtime.InteropServices;
using SharpDX.Direct3D;

namespace SharpDX
{
    /// <summary>
    ///   Provides a stream interface to a buffer located in unmanaged memory.
    /// </summary>
    public class DataStream : Stream
    {
        private unsafe byte* _buffer;
        private readonly bool _canRead;
        private readonly bool _canWrite;
        private GCHandle _gCHandle;
        private Blob _blob;
        private readonly bool _ownsBuffer;
        private long _position;
        private readonly long _size;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataStream"/> class from a Blob buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        public DataStream(Blob buffer)
        {
            unsafe
            {
                System.Diagnostics.Debug.Assert(buffer.GetBufferSize() > 0);

                _buffer = (byte*) buffer.GetBufferPointer();
                _size = buffer.GetBufferSize();
                _canRead = true;
                _canWrite = true;
                _blob = buffer;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SharpDX.DataStream"/> class, using a managed buffer as a backing store.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userBuffer">A managed array to be used as a backing store.</param>
        /// <param name="canRead"><c>true</c> if reading from the buffer should be allowed; otherwise, <c>false</c>.</param>
        /// <param name="canWrite"><c>true</c> if writing to the buffer should be allowed; otherwise, <c>false</c>.</param>
        /// <param name="index">Index inside the buffer in terms of element count (not size in bytes).</param>
        /// <param name="pinBuffer">True to keep the managed buffer and pin it, false will allocate unmanaged memory and make a copy of it. Default is true.</param>
        /// <returns></returns>
        public static DataStream Create<T>(T[] userBuffer, bool canRead, bool canWrite, int index = 0, bool pinBuffer = true) where T : struct
        {
            unsafe
            {
                if (userBuffer == null)
                    throw new ArgumentNullException("userBuffer");

                if (index < 0 || index > userBuffer.Length)
                    throw new ArgumentException("Index is out of range [0, userBuffer.Length-1]", "index");

                DataStream stream;

                var sizeOfBuffer = Utilities.SizeOf(userBuffer);
                var indexOffset = index * Utilities.SizeOf<T>();

                if (pinBuffer)
                {
                    var handle = GCHandle.Alloc(userBuffer, GCHandleType.Pinned);
                    stream = new DataStream(indexOffset + (byte*)handle.AddrOfPinnedObject(), sizeOfBuffer - indexOffset, canRead, canWrite, handle);
                }
                else
                {
                    // The .NET Native compiler crashes if '(IntPtr)' is removed.
                    stream = new DataStream(indexOffset + (byte*)(IntPtr)Interop.Fixed(userBuffer), sizeOfBuffer - indexOffset, canRead, canWrite, true);
                }

                return stream;
            }
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "SharpDX.DataStream" /> class, and allocates a new buffer to use as a backing store.
        /// </summary>
        /// <param name = "sizeInBytes">The size of the buffer to be allocated, in bytes.</param>
        /// <param name = "canRead">
        ///   <c>true</c> if reading from the buffer should be allowed; otherwise, <c>false</c>.</param>
        /// <param name = "canWrite">
        ///   <c>true</c> if writing to the buffer should be allowed; otherwise, <c>false</c>.</param>
        public DataStream(int sizeInBytes, bool canRead, bool canWrite)
        {
            unsafe
            {
                System.Diagnostics.Debug.Assert(sizeInBytes > 0);

                _buffer = (byte*) Utilities.AllocateMemory(sizeInBytes);
                _size = sizeInBytes;
                _ownsBuffer = true;
                _canRead = canRead;
                _canWrite = canWrite;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataStream"/> class.
        /// </summary>
        /// <param name="dataPointer">The data pointer.</param>
        public DataStream(DataPointer dataPointer) : this(dataPointer.Pointer, dataPointer.Size, true, true)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "SharpDX.DataStream" /> class, using an unmanaged buffer as a backing store.
        /// </summary>
        /// <param name = "userBuffer">A pointer to the buffer to be used as a backing store.</param>
        /// <param name = "sizeInBytes">The size of the buffer provided, in bytes.</param>
        /// <param name = "canRead">
        ///   <c>true</c> if reading from the buffer should be allowed; otherwise, <c>false</c>.</param>
        /// <param name = "canWrite">
        ///   <c>true</c> if writing to the buffer should be allowed; otherwise, <c>false</c>.</param>
        public DataStream(IntPtr userBuffer, long sizeInBytes, bool canRead, bool canWrite)
        {
            unsafe
            {
                System.Diagnostics.Debug.Assert(userBuffer != IntPtr.Zero);
                System.Diagnostics.Debug.Assert(sizeInBytes > 0);
                _buffer = (byte*) userBuffer.ToPointer();
                _size = sizeInBytes;
                _canRead = canRead;
                _canWrite = canWrite;
            }
        }

        internal unsafe DataStream(void* dataPointer, int sizeInBytes, bool canRead, bool canWrite, GCHandle handle)
        {
            System.Diagnostics.Debug.Assert(sizeInBytes > 0);
            _gCHandle = handle;
            _buffer = (byte*)dataPointer;
            _size = sizeInBytes;
            _canRead = canRead;
            _canWrite = canWrite;
            _ownsBuffer = false;
        }

        internal unsafe DataStream(void* buffer, int sizeInBytes, bool canRead, bool canWrite, bool makeCopy)
        {
            System.Diagnostics.Debug.Assert(sizeInBytes > 0);
            if (makeCopy)
            {
                _buffer = (byte*) Utilities.AllocateMemory(sizeInBytes);
                Utilities.CopyMemory((IntPtr) _buffer, (IntPtr) buffer, sizeInBytes);
            }
            else
            {
                _buffer = (byte*) buffer;
            }
            _size = sizeInBytes;
            _canRead = canRead;
            _canWrite = canWrite;
            _ownsBuffer = makeCopy;
        }
        
        ~DataStream()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_blob != null)
                {
                    _blob.Dispose();
                    _blob = null;
                }
            }

            if (_gCHandle.IsAllocated)
                _gCHandle.Free();

            unsafe
            {
                if (_ownsBuffer && _buffer != (byte*)0)
                {
                    Utilities.FreeMemory((IntPtr)_buffer);
                    _buffer = (byte*)0;
                }
            }
        }

        /// <summary>
        ///   Not supported.
        /// </summary>
        /// <exception cref = "T:System.NotSupportedException">Always thrown.</exception>
        public override void Flush()
        {
            throw new NotSupportedException("DataStream objects cannot be flushed.");
        }

        /// <summary>
        ///   Reads a single value from the current stream and advances the current
        ///   position within this stream by the number of bytes read.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <typeparam name = "T">The type of the value to be read from the stream.</typeparam>
        /// <returns>The value that was read.</returns>
        /// <exception cref = "T:System.NotSupportedException">This stream does not support reading.</exception>
        public T Read<T>() where T : struct
        {
            unsafe
            {
                if (!_canRead)
                    throw new NotSupportedException();

                byte* from = _buffer + _position;
                T result = default(T);
                _position = (byte*) Utilities.ReadAndPosition((IntPtr)from, ref result) - _buffer;
                return result;
            }
        }

        /// <inheritdoc/>
        public unsafe override int ReadByte()
        {
            if (_position >= Length)
                return -1;

            return _buffer[_position++];
        }

        /// <summary>
        ///   Reads a sequence of bytes from the current stream and advances the position
        ///   within the stream by the number of bytes read.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <param name = "buffer">An array of values to be read from the stream.</param>
        /// <param name = "offset">The zero-based byte offset in buffer at which to begin storing
        ///   the data read from the current stream.</param>
        /// <param name = "count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>The number of bytes read from the stream.</returns>
        /// <exception cref = "T:System.NotSupportedException">This stream does not support reading.</exception>
        public override int Read(byte[] buffer, int offset, int count)
        {
            int minCount = (int)Math.Min(RemainingLength, count);
            return ReadRange(buffer, offset, minCount);
        }

        /// <summary>
        /// Reads a sequence of bytes from the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies <paramref name="count" /> bytes from <paramref name="buffer" /> to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        public void Read(IntPtr buffer, int offset, int count)
        {
            unsafe
            {
                Utilities.CopyMemory(new IntPtr((byte*)buffer + offset), (IntPtr)(_buffer + _position), count);
                _position += count;
            }
        }

        /// <summary>
        ///   Reads an array of values from the current stream, and advances the current position
        ///   within this stream by the number of bytes written.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <typeparam name = "T">The type of the values to be read from the stream.</typeparam>
        /// <returns>An array of values that was read from the current stream.</returns>
        public T[] ReadRange<T>(int count) where T : struct
        {
            unsafe
            {
                if (!_canRead)
                    throw new NotSupportedException();

                byte* from = _buffer + _position;
                var result = new T[count];
                _position = (byte*) Utilities.Read((IntPtr)from, result, 0, count) - _buffer;
                return result;
            }
        }

        /// <summary>
        ///   Reads a sequence of elements from the current stream into a target buffer and
        ///   advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <param name = "buffer">An array of values to be read from the stream.</param>
        /// <param name = "offset">The zero-based byte offset in buffer at which to begin storing
        ///   the data read from the current stream.</param>
        /// <param name = "count">The number of values to be read from the current stream.</param>
        /// <returns>The number of bytes read from the stream.</returns>
        /// <exception cref = "T:System.NotSupportedException">This stream does not support reading.</exception>
        public int ReadRange<T>(T[] buffer, int offset, int count) where T : struct
        {
            unsafe
            {
                if (!_canRead)
                    throw new NotSupportedException();

                var oldPosition = _position;
                _position = (byte*)Utilities.Read((IntPtr)(_buffer + _position), buffer, offset, count) - _buffer;
                return (int) (_position - oldPosition);
            }
        }

        /// <summary>
        ///   Sets the position within the current stream.
        /// </summary>
        /// <exception cref = "T:System.InvalidOperationException">Attempted to seek outside of the bounds of the stream.</exception>
        public override long Seek(long offset, SeekOrigin origin)
        {
            long targetPosition = 0;

            switch (origin)
            {
                case SeekOrigin.Begin:
                    targetPosition = offset;
                    break;

                case SeekOrigin.Current:
                    targetPosition = _position + offset;
                    break;

                case SeekOrigin.End:
                    targetPosition = _size - offset;
                    break;
            }

            if (targetPosition < 0)
                throw new InvalidOperationException("Cannot seek beyond the beginning of the stream.");
            if (targetPosition > _size)
                throw new InvalidOperationException("Cannot seek beyond the end of the stream.");

            _position = targetPosition;
            return _position;
        }

        /// <summary>
        ///   Not supported.
        /// </summary>
        /// <param name = "value">Always ignored.</param>
        /// <exception cref = "T:System.NotSupportedException">Always thrown.</exception>
        public override void SetLength(long value)
        {
            throw new NotSupportedException("DataStream objects cannot be resized.");
        }

        /// <summary>
        ///   Writes a single value to the stream, and advances the current position
        ///   within this stream by the number of bytes written.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <typeparam name = "T">The type of the value to be written to the stream.</typeparam>
        /// <param name = "value">The value to write to the stream.</param>
        /// <exception cref = "T:System.NotSupportedException">The stream does not support writing.</exception>
        public void Write<T>(T value) where T : struct
        {
            if (!_canWrite)
                throw new NotSupportedException();
            unsafe
            {
                _position = (byte*) Utilities.WriteAndPosition((IntPtr)(_buffer + _position), ref value) - _buffer;
            }
        }

        /// <summary>
        ///   Writes a sequence of bytes to the current stream and advances the current
        ///   position within this stream by the number of bytes written.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <param name = "buffer">An array of bytes. This method copies count bytes from buffer to the current stream.</param>
        /// <param name = "offset">The zero-based byte offset in buffer at which to begin copying bytes to the current stream.</param>
        /// <param name = "count">The number of bytes to be written to the current stream.</param>
        /// <exception cref = "T:System.NotSupportedException">This stream does not support writing.</exception>
        public override void Write(byte[] buffer, int offset, int count)
        {
            WriteRange(buffer, offset, count);
        }

        /// <summary>
        /// When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies <paramref name="count" /> bytes from <paramref name="buffer" /> to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        public void Write(IntPtr buffer, int offset, int count)
        {
            unsafe
            {
                Utilities.CopyMemory((IntPtr) (_buffer + _position), new IntPtr((byte*) buffer + offset), count);
                _position += count;
            }
        }
        
        /// <summary>
        ///   Writes an array of values to the current stream, and advances the current position
        ///   within this stream by the number of bytes written.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <param name = "data">An array of values to be written to the current stream.</param>
        /// <exception cref = "T:System.NotSupportedException">This stream does not support writing.</exception>
        public void WriteRange<T>(T[] data) where T : struct
        {
            WriteRange(data, 0, data.Length);
        }

        /// <summary>
        ///   Writes a range of bytes to the current stream, and advances the current position
        ///   within this stream by the number of bytes written.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <param name = "source">A pointer to the location to start copying from.</param>
        /// <param name = "count">The number of bytes to copy from source to the current stream.</param>
        /// <exception cref = "T:System.NotSupportedException">This stream does not support writing.</exception>
        public void WriteRange(IntPtr source, long count)
        {
            unsafe
            {
                if (!_canWrite)
                    throw new NotSupportedException();

                System.Diagnostics.Debug.Assert(_canWrite);
                System.Diagnostics.Debug.Assert(source != IntPtr.Zero);
                System.Diagnostics.Debug.Assert(count > 0);
                System.Diagnostics.Debug.Assert((_position + count) <= _size);

                // TODO: use Interop.memcpy
                Utilities.CopyMemory((IntPtr) (_buffer + _position), source, (int) count);
                _position += count;
            }
        }

        /// <summary>
        ///   Writes an array of values to the current stream, and advances the current position
        ///   within this stream by the number of bytes written.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <typeparam name = "T">The type of the values to be written to the stream.</typeparam>
        /// <param name = "data">An array of values to be written to the stream.</param>
        /// <param name = "offset">The zero-based offset in data at which to begin copying values to the current stream.</param>
        /// <param name = "count">The number of values to be written to the current stream. If this is zero,
        ///   all of the contents <paramref name = "data" /> will be written.</param>
        /// <exception cref = "T:System.NotSupportedException">This stream does not support writing.</exception>
        public void WriteRange<T>(T[] data, int offset, int count) where T : struct
        {
            unsafe
            {
                if (!_canWrite)
                    throw new NotSupportedException();

                _position = (byte*) Utilities.Write((IntPtr)(_buffer + _position), data, offset, count) - _buffer;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether the current stream supports reading.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the stream supports reading; otherwise, <c>false</c>.</value>
        public override bool CanRead
        {
            get { return _canRead; }
        }

        /// <summary>
        ///   Gets a value indicating whether the current stream supports seeking.
        /// </summary>
        /// <value>Always <c>true</c>.</value>
        public override bool CanSeek
        {
            get { return true; }
        }

        /// <summary>
        ///   Gets a value indicating whether the current stream supports writing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the stream supports writing; otherwise, <c>false</c>.</value>
        public override bool CanWrite
        {
            get { return _canWrite; }
        }

        /// <summary>
        ///   Gets the internal pointer to the current stream's backing store.
        /// </summary>
        /// <value>An IntPtr to the buffer being used as a backing store.</value>
        public IntPtr DataPointer
        {
            get
            {
                unsafe
                {
                    return new IntPtr(_buffer);
                }
            }
        }

        /// <summary>
        ///   Gets the length in bytes of the stream.
        /// </summary>
        /// <value>A long value representing the length of the stream in bytes.</value>
        public override long Length
        {
            get { return _size; }
        }

        /// <summary>
        ///   Gets or sets the position within the current stream.
        /// </summary>
        /// <value>The current position within the stream.</value>
        /// <seealso cref = "T:System.IO.Stream">Stream Class</seealso>
        public override long Position
        {
            get { return _position; }
            set { Seek(value, SeekOrigin.Begin); }
        }


        /// <summary>
        /// Gets the position pointer.
        /// </summary>
        /// <value>The position pointer.</value>
        public IntPtr PositionPointer
        {
            get
            {
                unsafe
                {
                    return (IntPtr) (_buffer + _position);
                }
            }
        }

        /// <summary>
        /// Gets the length of the remaining.
        /// </summary>
        /// <value>The length of the remaining.</value>
        public long RemainingLength
        {
            get { return (_size - _position); }
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SharpDX.DataStream"/> to <see cref="SharpDX.DataPointer"/>.
        /// </summary>
        /// <param name="from">The from value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator DataPointer(DataStream from)
        {
            return new DataPointer(from.PositionPointer, (int)from.RemainingLength);
        }
    }
}