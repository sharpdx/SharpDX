// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Describes whether existing vertex or index buffer data will be overwritten or discarded during a SetData operation.
    /// </summary>
    [Flags]
    public enum SetDataOptions
    {
        /// <summary>
        /// Portions of existing data in the buffer may be overwritten during this operation.
        /// </summary>
        None,

        /// <summary>
        /// The SetData operation will discard the entire buffer. A pointer to a new memory area is returned so that the direct memory access (DMA) and rendering from the previous area do not stall.
        /// </summary>
        Discard,

        /// <summary>
        /// The SetData operation will not overwrite existing data in the vertex and index buffers. Specifying this option allows the driver to return immediately from a SetData operation and continue rendering.
        /// </summary>
        NoOverwrite
    }

    class SetDataOptionsHelper
    {
        public static MapMode ConvertToMapMode(SetDataOptions options)
        {
            switch (options)
            {
                case SetDataOptions.None:
                    return MapMode.Write;
                case SetDataOptions.Discard:
                    return MapMode.WriteDiscard;
                case SetDataOptions.NoOverwrite:
                    return MapMode.WriteNoOverwrite;
            }
            return MapMode.Write;
        }
    }
}