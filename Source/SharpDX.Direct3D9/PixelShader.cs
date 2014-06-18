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

using SharpDX.Direct3D;

namespace SharpDX.Direct3D9
{
    public partial class PixelShader
    {
        private ShaderBytecode function;

        /// <summary>
        /// Initializes a new instance of the <see cref="PixelShader"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="function">The function.</param>
        /// <unmanaged>HRESULT IDirect3DDevice9::CreatePixelShader([In] const void* pFunction,[Out, Fast] IDirect3DPixelShader9** ppShader)</unmanaged>	
        public PixelShader(Device device, ShaderBytecode function)
        {
            device.CreatePixelShader(function.BufferPointer, this);
            this.function = function;
        }

        /// <summary>
        /// Gets the bytecode associated to this shader..
        /// </summary>
        public ShaderBytecode Function
        {
            get
            {
                if (function != null)
                    return function;

                int size = 0;
                GetFunction(IntPtr.Zero, ref size);

                Blob blob;
                D3DX9.CreateBuffer(size, out blob);

                GetFunction(blob.BufferPointer, ref size);

                function = new ShaderBytecode(blob);
                return function;
            }
        }
    }
}