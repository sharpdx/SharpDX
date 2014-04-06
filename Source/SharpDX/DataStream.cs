﻿// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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

                if (pinBuffer)
                {
                    var handle = GCHandle.Alloc(userBuffer, GCHandleType.Pinned);
                    var indexOffset = index * Utilities.SizeOf<T>();
                    stream = new DataStream(indexOffset + (byte*)handle.AddrOfPinnedObject(), sizeOfBuffer - indexOffset, canRead, canWrite, handle);
                }
                else
                {
                    stream = new DataStream(Interop.Fixed(userBuffer), sizeOfBuffer, canRead, canWrite, true);
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

        /// <summary>
        /// Reads a float.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <returns>a float from the stream</returns>
        public float ReadFloat()
        {
            unsafe
            {
                if (!_canRead)
                    throw new NotSupportedException();

                float value = *((float*)(_buffer + _position));
                _position += 4;
                return value;
            }
        }

        /// <summary>
        /// Reads a int.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <returns>an int from the stream</returns>
        public int ReadInt()
        {
            unsafe
            {
                if (!_canRead)
                    throw new NotSupportedException();

                int value = *((int*)(_buffer + _position));
                _position += 4;
                return value;
            }
        }

        /// <summary>
        /// Reads a short.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <returns>an short from the stream</returns>
        public short ReadShort()
        {
            unsafe
            {
                if (!_canRead)
                    throw new NotSupportedException();

                short value = *((short*)(_buffer + _position));
                _position += 2;
                return value;
            }
        }

        /// <summary>
        /// Reads a bool.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <returns>an bool from the stream</returns>
        public bool ReadBoolean()
        {
            unsafe
            {
                if (!_canRead)
                    throw new NotSupportedException();

                bool value = *((int*)(_buffer + _position)) != 0;
                _position += 4;
                return value;
            }
        }

        /// <summary>
        /// Reads a Vector2.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <returns>an Vector2 from the stream</returns>
        public Vector2 ReadVector2()
        {
            unsafe
            {
                if (!_canRead)
                    throw new NotSupportedException();

                Vector2 value = *((Vector2*)(_buffer + _position));
                _position += 4 *2;
                return value;
            }
        }

        /// <summary>
        /// Reads a Vector3.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <returns>an Vector3 from the stream</returns>
        public Vector3 ReadVector3()
        {
            unsafe
            {
                if (!_canRead)
                    throw new NotSupportedException();

                Vector3 value = *((Vector3*)(_buffer + _position));
                _position += 4 * 3;
                return value;
            }
        }

        /// <summary>
        /// Reads a Vector4.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <returns>an Vector4 from the stream</returns>
        public Vector4 ReadVector4()
        {
            unsafe
            {
                if (!_canRead)
                    throw new NotSupportedException();

                Vector4 value = *((Vector4*)(_buffer + _position));
                _position += 4 * 4;
                return value;
            }
        }

        /// <summary>
        /// Reads a Color3.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <returns>an Color3 from the stream</returns>
        public Color3 ReadColor3()
        {
            unsafe
            {
                if (!_canRead)
                    throw new NotSupportedException();

                Color3 value = *((Color3*)(_buffer + _position));
                _position += 4 * 3;
                return value;
            }
        }

        /// <summary>
        /// Reads a Color4.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <returns>an Color4 from the stream</returns>
        public Color4 ReadColor4()
        {
            unsafe
            {
                if (!_canRead)
                    throw new NotSupportedException();

                Color4 value = *((Color4*)(_buffer + _position));
                _position += 4 * 4;
                return value;
            }
        }

        /// <summary>
        /// Reads a Half.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <returns>an Half from the stream</returns>
        public Half ReadHalf()
        {
            unsafe
            {
                if (!_canRead)
                    throw new NotSupportedException();

                Half value = *((Half*)(_buffer + _position));
                _position += 2;
                return value;
            }
        }

        /// <summary>
        /// Reads a Half2.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <returns>an Half2 from the stream</returns>
        public Half2 ReadHalf2()
        {
            unsafe
            {
                if (!_canRead)
                    throw new NotSupportedException();

                Half2 value = *((Half2*)(_buffer + _position));
                _position += 2 * 2;
                return value;
            }
        }

        /// <summary>
        /// Reads a Half3.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <returns>an Half3 from the stream</returns>
        public Half3 ReadHalf3()
        {
            unsafe
            {
                if (!_canRead)
                    throw new NotSupportedException();

                Half3 value = *((Half3*)(_buffer + _position));
                _position += 2 * 3;
                return value;
            }
        }

        /// <summary>
        /// Reads a Half4.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <returns>an Half4 from the stream</returns>
        public Half4 ReadHalf4()
        {
            unsafe
            {
                if (!_canRead)
                    throw new NotSupportedException();

                Half4 value = *((Half4*)(_buffer + _position));
                _position += 2 * 4;
                return value;
            }
        }

        /// <summary>
        /// Reads a Matrix.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <returns>a Matrix from the stream</returns>
        public Matrix ReadMatrix()
        {
            unsafe
            {
                if (!_canRead)
                    throw new NotSupportedException();

                Matrix value = *((Matrix*)(_buffer + _position));
                _position += 4 * 4 * 4;
                return value;
            }
        }

        /// <summary>
        /// Reads a Quaternion.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <returns>a Quaternion from the stream</returns>
        public Quaternion ReadQuaternion()
        {
            unsafe
            {
                if (!_canRead)
                    throw new NotSupportedException();

                Quaternion value = *((Quaternion*)(_buffer + _position));
                _position += 4 * 4;
                return value;
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
        /// Writes the specified value.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <param name="value">The value.</param>
        public void Write(float value)
        {
            if (!_canWrite)
                throw new NotSupportedException();
            unsafe
            {
                *((float*) (_buffer + _position)) = value;
                _position += 4;
            }
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <param name="value">The value.</param>
        public void Write(int value)
        {
            if (!_canWrite)
                throw new NotSupportedException();
            unsafe
            {
                *((int*)(_buffer + _position)) = value;
                _position += 4;
            }
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <param name="value">The value.</param>
        public void Write(short value)
        {
            if (!_canWrite)
                throw new NotSupportedException();
            unsafe
            {
                *((short*)(_buffer + _position)) = value;
                _position += 2;
            }
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <param name="value">The value.</param>
        public void Write(bool value)
        {
            if (!_canWrite)
                throw new NotSupportedException();
            unsafe
            {
                *((int*) (_buffer + _position)) = value ? 1 : 0;
                _position += 4;
            }
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <param name="value">The value.</param>
        public void Write(Vector2 value)
        {
            if (!_canWrite)
                throw new NotSupportedException();
            unsafe
            {
                *((Vector2*)(_buffer + _position)) = value;
                _position += 4 * 2;
            }
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <param name="value">The value.</param>
        public void Write(Vector3 value)
        {
            if (!_canWrite)
                throw new NotSupportedException();
            unsafe
            {
                *((Vector3*)(_buffer + _position)) = value;
                _position += 4 * 3;
            }
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <param name="value">The value.</param>
        public void Write(Vector4 value)
        {
            if (!_canWrite)
                throw new NotSupportedException();
            unsafe
            {
                *((Vector4*)(_buffer + _position)) = value;
                _position += 4 * 4;
            }
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <param name="value">The value.</param>
        public void Write(Color3 value)
        {
            if (!_canWrite)
                throw new NotSupportedException();
            unsafe
            {
                *((Color3*)(_buffer + _position)) = value;
                _position += 4 * 3;
            }
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <param name="value">The value.</param>
        public void Write(Color4 value)
        {
            if (!_canWrite)
                throw new NotSupportedException();
            unsafe
            {
                *((Color4*)(_buffer + _position)) = value;
                _position += 4 * 4;
            }
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <param name="value">The value.</param>
        public void Write(Half value)
        {
            if (!_canWrite)
                throw new NotSupportedException();
            unsafe
            {
                *((Half*)(_buffer + _position)) = value;
                _position += 2;
            }
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <param name="value">The value.</param>
        public void Write(Half2 value)
        {
            if (!_canWrite)
                throw new NotSupportedException();
            unsafe
            {
                *((Half2*)(_buffer + _position)) = value;
                _position += 2 * 2;
            }
        }
        
        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <param name="value">The value.</param>
        public void Write(Half3 value)
        {
            if (!_canWrite)
                throw new NotSupportedException();
            unsafe
            {
                *((Half3*)(_buffer + _position)) = value;
                _position += 2 * 3;
            }
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <param name="value">The value.</param>
        public void Write(Half4 value)
        {
            if (!_canWrite)
                throw new NotSupportedException();
            unsafe
            {
                *((Half4*)(_buffer + _position)) = value;
                _position += 2 * 4;
            }
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <param name="value">The value.</param>
        public void Write(Matrix value)
        {
            if (!_canWrite)
                throw new NotSupportedException();
            unsafe
            {
                *((Matrix*)(_buffer + _position)) = value;
                _position += 4 * 4 * 4;
            }
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <remarks>
        /// In order to provide faster read/write, this operation doesn't check stream bound. 
        /// A client must carefully not read/write above the size of this datastream.
        /// </remarks>
        /// <param name="value">The value.</param>
        public void Write(Quaternion value)
        {
            if (!_canWrite)
                throw new NotSupportedException();
            unsafe
            {
                *((Quaternion*)(_buffer + _position)) = value;
                _position += 4 * 4 ;
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