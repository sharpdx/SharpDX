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

namespace SharpDX.Direct3D11
{
    public partial class GeometryShader
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D11.GeometryShader" /> class.
        /// </summary>
        /// <param name = "device">The device used to create the shader.</param>
        /// <param name = "shaderBytecode">The compiled shader bytecode.</param>
        /// <param name = "linkage">A dynamic class linkage interface.</param>
        public GeometryShader(Device device, byte[] shaderBytecode, ClassLinkage linkage = null)
            : base(IntPtr.Zero)
        {
            if (shaderBytecode == null) throw new ArgumentNullException("shaderBytecode", "ShaderBytecode cannot be null");

            unsafe
            {
                fixed (void* pBuffer = shaderBytecode)
                    device.CreateGeometryShader((IntPtr)pBuffer, shaderBytecode.Length, linkage, this);
            }
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D11.GeometryShader" /> class.
        /// </summary>
        /// <param name = "device">The device used to create the shader.</param>
        /// <param name = "shaderBytecode">The compiled shader bytecode.</param>
        /// <param name = "elements">An array of <see cref = "T:SharpDX.Direct3D11.StreamOutputElement" /> instances describing the layout of the output buffers.</param>
        /// <param name = "bufferedStrides">An array of buffer strides; each stride is the size of an element for that buffer.</param>
        /// <param name = "rasterizedStream">The index number of the stream to be sent to the rasterizer stage. Set to NoRasterizedStream if no stream is to be rasterized.</param>
        /// <param name = "linkage">A dynamic class linkage interface.</param>
        public GeometryShader(Device device, byte[] shaderBytecode, StreamOutputElement[] elements,
                              int[] bufferedStrides, int rasterizedStream, ClassLinkage linkage = null)
            : base(IntPtr.Zero)
        {
            unsafe
            {
                fixed (void* pBuffer = shaderBytecode)
                    device.CreateGeometryShaderWithStreamOutput((IntPtr)pBuffer, shaderBytecode.Length, elements, elements.Length,
                                                                bufferedStrides, bufferedStrides.Length, rasterizedStream,
                                                                linkage, this);
            }
        }
    }
}