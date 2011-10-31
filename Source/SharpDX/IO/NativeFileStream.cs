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
using System.IO;
using System.Runtime.InteropServices;

namespace SharpDX.IO
{
    /// <summary>
    /// Windows File Helper.
    /// </summary>
    public class NativeFileStream : Stream
    {
        private bool canRead;
        private bool canWrite;
        private bool canSeek;
        private IntPtr handle;
        private long position;

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeFileStream"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileMode">The file mode.</param>
        /// <param name="access">The access mode.</param>
        /// <param name="share">The share mode.</param>
        public NativeFileStream(string fileName, NativeFileMode fileMode, NativeFileAccess access, NativeFileShare share = NativeFileShare.Read)
        {
#if WIN8
            handle = NativeFile.Create(fileName, access, share, fileMode, IntPtr.Zero);
#else
            handle = NativeFile.Create(fileName, access, share, IntPtr.Zero, fileMode, NativeFileOptions.None, IntPtr.Zero);
#endif
            if (handle == new IntPtr(-1))
                throw new IOException(string.Format("Unable to open file {0}", fileName), Marshal.GetLastWin32Error());

            // TODO setup correctly canRead, canWrite, canSeek flags
            canRead = true;
            canWrite = true;
            canSeek = true;
        }

        /// <inheritdoc/>
        public override void Flush()
        {
            // TODO implement flush
        }

        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin)
        {
            long newPosition;
            if (!NativeFile.SetFilePointerEx(handle, offset, out newPosition, origin))
                throw new IOException("Unable to seek to this position", Marshal.GetLastWin32Error());
            position = newPosition;
            return position;
        }

        /// <inheritdoc/>
        public override void SetLength(long value)
        {
            long newPosition;
            if (!NativeFile.SetFilePointerEx(handle, value, out newPosition, SeekOrigin.Begin))
                throw new IOException("Unable to seek to this position", Marshal.GetLastWin32Error());
            if (!NativeFile.SetEndOfFile(handle))
                throw new IOException("Unable to set the new length", Marshal.GetLastWin32Error());

            if (position < value)
            {
                Seek(position, SeekOrigin.Begin);
            } 
            else
            {
                Seek(0, SeekOrigin.End);
            }
        }

        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count)
        {
            int numberOfBytesRead;
            unsafe
            {
                fixed (void* pbuffer = &buffer[offset])
                {
                    if (!NativeFile.ReadFile(handle, (IntPtr)pbuffer, count, out numberOfBytesRead, IntPtr.Zero))
                        throw new IOException("Unable to read from file", Marshal.GetLastWin32Error());
                }
                position += numberOfBytesRead;
            }
            return numberOfBytesRead;
        }

        /// <inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count)
        {
            int numberOfBytesWritten;
            unsafe
            {
                fixed (void* pbuffer = &buffer[offset])
                {
                    if (!NativeFile.WriteFile(handle, (IntPtr)pbuffer, count, out numberOfBytesWritten, IntPtr.Zero))
                        throw new IOException("Unable to write to file", Marshal.GetLastWin32Error());
                }
                position += numberOfBytesWritten;
            }
        }

        /// <inheritdoc/>
        public override bool CanRead
        {
            get
            {
                return canRead;
            }
        }

        /// <inheritdoc/>
        public override bool CanSeek
        {
            get
            {
                return canSeek;
            }
        }

        /// <inheritdoc/>
        public override bool CanWrite
        {
            get
            {
                return canWrite;
            }
        }

        /// <inheritdoc/>
        public override long Length
        {
            get
            {
                long length;
                // TODO implement WIN8 replacement
                if (!NativeFile.GetFileSizeEx(handle, out length))
                    throw new IOException("Unable to get file length", Marshal.GetLastWin32Error());
                return length;
            }
        }

        /// <inheritdoc/>
        public override long Position
        {
            get
            {
                return position;
            }
            set
            {
                Seek(value, SeekOrigin.Begin);
                position = value;
            }
        }

        protected override void Dispose(bool disposing)
        {
            Utilities.CloseHandle(handle);
            handle = IntPtr.Zero;
            base.Dispose(disposing);
        }
    }
}
