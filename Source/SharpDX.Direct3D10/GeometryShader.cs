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

namespace SharpDX.Direct3D10
{
    public partial class GeometryShader
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D10.GeometryShader" /> class.
        /// </summary>
        /// <param name = "device">The device used to create the shader.</param>
        /// <param name = "shaderBytecode">The compiled shader bytecode.</param>
        public unsafe GeometryShader(Device device, byte[] shaderBytecode)
            : base(IntPtr.Zero)
        {
            fixed (void* ptr = shaderBytecode)
                device.CreateGeometryShader((IntPtr)ptr, shaderBytecode.Length, this);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D10.GeometryShader" /> class.
        /// </summary>
        /// <param name = "device">The device used to create the shader.</param>
        /// <param name = "shaderBytecode">The compiled shader bytecode.</param>
        /// <param name = "elements">An array of <see cref = "T:SharpDX.Direct3D10.StreamOutputElement" /> instances describing the layout of the output buffers.</param>
        /// <param name = "outputStreamStride">The size, in bytes, of each element in the array pointed to by pSODeclaration. This parameter is only used when the output slot is 0 for all entries in pSODeclaration.</param>
        public unsafe GeometryShader(Device device, byte[] shaderBytecode, StreamOutputElement[] elements, int outputStreamStride)
            : base(IntPtr.Zero)
        {
            fixed (void* ptr = shaderBytecode)
                device.CreateGeometryShaderWithStreamOutput((IntPtr)ptr, shaderBytecode.Length, elements, elements.Length, outputStreamStride, this);
        }
    }
}