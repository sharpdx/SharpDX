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
    ///   Properties defining the way a buffer (containing vertex data) is bound
    ///   to the pipeline for rendering.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct VertexBufferBinding
    {
        private Buffer m_Buffer;
        private int m_Stride;
        private int m_Offset;

        /// <summary>
        ///   Gets or sets the buffer being bound.
        /// </summary>
        public Buffer Buffer
        {
            get { return this.m_Buffer; }
            set { this.m_Buffer = value; }
        }

        /// <summary>
        ///   Gets or sets the stride between vertex elements in the buffer (in bytes).
        /// </summary>
        public int Stride
        {
            get { return this.m_Stride; }
            set { this.m_Stride = value; }
        }

        /// <summary>
        ///   Gets or sets the offset from the start of the buffer of the first vertex to use (in bytes).
        /// </summary>
        public int Offset
        {
            get { return this.m_Offset; }
            set { this.m_Offset = value; }
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D10.VertexBufferBinding" /> struct.
        /// </summary>
        /// <param name = "buffer">The buffer being bound.</param>
        /// <param name = "stride">The stride between vertex element (in bytes).</param>
        /// <param name = "offset">The offset to the first vertex (in bytes).</param>
        public VertexBufferBinding(Buffer buffer, int stride, int offset)
        {
            this.m_Buffer = buffer;
            this.m_Stride = stride;
            this.m_Offset = offset;
        }
    }
}