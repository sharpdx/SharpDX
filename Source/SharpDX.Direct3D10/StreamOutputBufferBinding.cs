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
using System.Runtime.InteropServices;

namespace SharpDX.Direct3D10
{
    /// <summary>
    ///   Properties defining the way a buffer is bound to the pipeline as a target for stream output operations.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct StreamOutputBufferBinding
    {
        private Buffer _buffer;
        private int _offset;

        /// <summary>
        ///   Gets or sets the buffer being bound.
        /// </summary>
        public Buffer Buffer
        {
            get { return this._buffer; }
            set { this._buffer = value; }
        }

        /// <summary>
        ///   Gets or sets the offset from the start of the buffer of the first vertex to use (in bytes).
        /// </summary>
        public int Offset
        {
            get { return this._offset; }
            set { this._offset = value; }
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D10.StreamOutputBufferBinding" /> struct.
        /// </summary>
        /// <param name = "buffer">The buffer being bound.</param>
        /// <param name = "offset">The offset to the first vertex (in bytes).</param>
        public StreamOutputBufferBinding(Buffer buffer, int offset)
        {
            this._buffer = buffer;
            this._offset = offset;
        }
    }
}