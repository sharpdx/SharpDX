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

namespace SharpDX.Direct2D1
{
    public partial class DrawInformation
    {

        /// <summary>
        /// Sets the constant buffer data from a <see cref="SharpDX.DataStream"/> for the Vertex stage.
        /// </summary>
        /// <param name="dataStream">The DataStream that contains the constant buffer data</param>
        // <unmanaged>HRESULT ID2D1ComputeInfo::SetComputeShaderConstantBuffer([In, Buffer] const void* buffer,[In] unsigned int bufferCount)</unmanaged>	
        public void SetVertexConstantBuffer(DataStream dataStream)
        {
            SetVertexShaderConstantBuffer(dataStream.DataPointer, (int)dataStream.Length);
        }

        /// <summary>
        /// Sets the constant buffer data from a struct value for the Vertex stage.
        /// </summary>
        /// <typeparam name="T">Type of the constant buffer</typeparam>
        /// <param name="value">Value of the constant buffer</param>
        /// <unmanaged>HRESULT ID2D1ComputeInfo::SetComputeShaderConstantBuffer([In, Buffer] const void* buffer,[In] unsigned int bufferCount)</unmanaged>	
        public void SetVertexConstantBuffer<T>(T value) where T : struct
        {
            unsafe
            {
                SetVertexShaderConstantBuffer((IntPtr)Interop.Fixed<T>(ref value), Utilities.SizeOf<T>());
            }
        }

        /// <summary>
        /// Sets the constant buffer data from a struct value for the Vertex Stage.
        /// </summary>
        /// <typeparam name="T">Type of the constant buffer</typeparam>
        /// <param name="value">Value of the constant buffer</param>
        /// <unmanaged>HRESULT ID2D1ComputeInfo::SetComputeShaderConstantBuffer([In, Buffer] const void* buffer,[In] unsigned int bufferCount)</unmanaged>	
        public void SetVertexConstantBuffer<T>(ref T value) where T : struct
        {
            unsafe
            {
                SetVertexShaderConstantBuffer((IntPtr)Interop.Fixed<T>(ref value), Utilities.SizeOf<T>());
            }
        }

        /// <summary>
        /// Sets the constant buffer data from a <see cref="SharpDX.DataStream"/> for the Pixel stage.
        /// </summary>
        /// <param name="dataStream">The DataStream that contains the constant buffer data</param>
        // <unmanaged>HRESULT ID2D1ComputeInfo::SetComputeShaderConstantBuffer([In, Buffer] const void* buffer,[In] unsigned int bufferCount)</unmanaged>	
        public void SetPixelConstantBuffer(DataStream dataStream)
        {
            SetPixelShaderConstantBuffer(dataStream.DataPointer, (int)dataStream.Length);
        }

        /// <summary>
        /// Sets the constant buffer data from a struct value for the Pixel stage.
        /// </summary>
        /// <typeparam name="T">Type of the constant buffer</typeparam>
        /// <param name="value">Value of the constant buffer</param>
        /// <unmanaged>HRESULT ID2D1ComputeInfo::SetComputeShaderConstantBuffer([In, Buffer] const void* buffer,[In] unsigned int bufferCount)</unmanaged>	
        public void SetPixelConstantBuffer<T>(T value) where T : struct
        {
            unsafe
            {
                SetPixelShaderConstantBuffer((IntPtr)Interop.Fixed<T>(ref value), Utilities.SizeOf<T>());
            }
        }

        /// <summary>
        /// Sets the constant buffer data from a struct value for the Pixel Stage.
        /// </summary>
        /// <typeparam name="T">Type of the constant buffer</typeparam>
        /// <param name="value">Value of the constant buffer</param>
        /// <unmanaged>HRESULT ID2D1ComputeInfo::SetComputeShaderConstantBuffer([In, Buffer] const void* buffer,[In] unsigned int bufferCount)</unmanaged>	
        public void SetPixelConstantBuffer<T>(ref T value) where T : struct
        {
            unsafe
            {
                SetPixelShaderConstantBuffer((IntPtr)Interop.Fixed<T>(ref value), Utilities.SizeOf<T>());
            }
        }
    }
}