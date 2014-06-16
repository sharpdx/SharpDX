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

namespace SharpDX.Win32
{
    public partial class ComStream
    {
        /// <summary>
        /// Copies a specified number of bytes from the current seek pointer in the stream to the current seek pointer in another stream.
        /// </summary>
        /// <param name="streamDest">The stream destination.</param>
        /// <param name="numberOfBytesToCopy">The number of bytes to copy.</param>
        /// <param name="bytesWritten">The bytes written.</param>
        /// <returns>The number of bytes read from this instance</returns>
        public long CopyTo(IStream streamDest, long numberOfBytesToCopy, out long bytesWritten)
        {
            CopyTo_(ToIntPtr(streamDest), numberOfBytesToCopy, out bytesWritten);
            return bytesWritten;
        }

        /// <summary>
        /// Gets a com pointer to the underlying <see cref="IStream"/> object.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>A Com pointer</returns>
        public static IntPtr ToIntPtr(IStream stream)
        {
            return ComStreamShadow.ToIntPtr(stream);
        }
    }
}

