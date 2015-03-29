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
using System.Text;

namespace SharpDX.Direct2D1
{
    public partial class VertexBufferProperties
    {
        private DataStream data;

        /// <summary>
        /// Initializes a new instance of <see cref="VertexBufferProperties"/> class.
        /// </summary>
        public VertexBufferProperties()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="VertexBufferProperties"/> class.
        /// </summary>
        /// <param name="inputCount">The number of inputs to the vertex shader.</param>
        /// <param name="usage">Indicates how frequently the vertex buffer is likely to be updated.</param>
        /// <param name="data">The initial contents of the vertex buffer</param>
        public VertexBufferProperties(int inputCount, VertexUsage usage, DataStream data)
        {
            InputCount = inputCount;
            Usage = usage;
            Data = data;
        }

        /// <summary>	
        /// The initial contents of the vertex buffer.
        /// </summary>	
        public DataStream Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
                if (data != null)
                {
                    DataPointer = data.DataPointer;
                    SizeInBytes = (int)data.Length;
                }
            }
        }
    }
}