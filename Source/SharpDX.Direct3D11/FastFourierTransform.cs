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

namespace SharpDX.Direct3D11
{
    public partial class FastFourierTransform
    {
        private FastFourierTransformBufferRequirements _bufferRequirements;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpDX.Direct3D11.FastFourierTransform" /> class.
        /// </summary>
        /// <param name="context">The device context used to create the FFT.</param>
        /// <param name="description">Information that describes the shape of the FFT data as well as the scaling factors that should be used for forward and inverse transforms.</param>
        public FastFourierTransform(DeviceContext context, FastFourierTransformDescription description) : this(context, description, FastFourierTransformCreationFlags.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpDX.Direct3D11.FastFourierTransform" /> class.
        /// </summary>
        /// <param name="context">The device context used to create the FFT.</param>
        /// <param name="description">Information that describes the shape of the FFT data as well as the scaling factors that should be used for forward and inverse transforms.</param>
        /// <param name="flags">Flag affecting the behavior of the FFT.</param>
        public FastFourierTransform(DeviceContext context, FastFourierTransformDescription description, FastFourierTransformCreationFlags flags) : base(IntPtr.Zero)
        {
            D3DCSX.CreateFFT(context, ref description, (int)flags, out _bufferRequirements, this);
        }

        /// <summary>	
        /// Attaches buffers to an FFT context and performs any required precomputations.	
        /// </summary>	
        /// <remarks>	
        /// The buffers must be no smaller than the corresponding buffer sizes returned by D3DX11CreateFFT*(). Temporary buffers can be shared between multiple contexts, though care should be taken not  to concurrently execute multiple FFTs which share temp buffers. 	
        /// </remarks>	
        /// <param name="temporaryBuffers">Temporary buffers to attach. </param>
        /// <param name="precomputeBuffers">Buffers to hold precomputed data. </param>
        /// <returns>Returns one of the return codes described in the topic {{Direct3D 11 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3DX11FFT::AttachBuffersAndPrecompute([In] int NumTempBuffers,[In, Buffer] const ID3D11UnorderedAccessView** ppTempBuffers,[In] int NumPrecomputeBuffers,[In, Buffer] const ID3D11UnorderedAccessView** ppPrecomputeBufferSizes)</unmanaged>
        public Result AttachBuffersAndPrecompute(UnorderedAccessView[] temporaryBuffers, UnorderedAccessView[] precomputeBuffers)
        {
            return AttachBuffersAndPrecompute(temporaryBuffers.Length, temporaryBuffers, precomputeBuffers.Length, precomputeBuffers);
        }

        /// <summary>	
        /// Creates a new one-dimensional complex FFT.	
        /// </summary>	
        /// <param name="context">Pointer to the <see cref="SharpDX.Direct3D11.DeviceContext"/> interface to use for the FFT. </param>
        /// <param name="x">Length of the first dimension of the FFT. </param>
        /// <returns>an <see cref="SharpDX.Direct3D11.FastFourierTransform"/> interface reference.</returns>
        /// <unmanaged>HRESULT D3DX11CreateFFT1DComplex([None] ID3D11DeviceContext* pDeviceContext,[None] int X,[None] int Flags,[Out] D3DX11_FFT_BUFFER_INFO* pBufferInfo,[Out] ID3DX11FFT** ppFFT)</unmanaged>
        public static FastFourierTransform Create1DComplex(DeviceContext context, int x)
        {
            return Create1DComplex(context, x, FastFourierTransformCreationFlags.None);
        }

        /// <summary>	
        /// Creates a new one-dimensional complex FFT.	
        /// </summary>	
        /// <param name="context">Pointer to the <see cref="SharpDX.Direct3D11.DeviceContext"/> interface to use for the FFT. </param>
        /// <param name="x">Length of the first dimension of the FFT. </param>
        /// <param name="flags">Flag affecting the behavior of the FFT, can be 0 or a combination of flags from <see cref="SharpDX.Direct3D11.FastFourierTransformCreationFlags"/>. </param>
        /// <returns>an <see cref="SharpDX.Direct3D11.FastFourierTransform"/> interface reference.</returns>
        /// <unmanaged>HRESULT D3DX11CreateFFT1DComplex([None] ID3D11DeviceContext* pDeviceContext,[None] int X,[None] int Flags,[Out] D3DX11_FFT_BUFFER_INFO* pBufferInfo,[Out] ID3DX11FFT** ppFFT)</unmanaged>
        public static FastFourierTransform Create1DComplex(DeviceContext context, int x, FastFourierTransformCreationFlags flags)
        {
            FastFourierTransformBufferRequirements info;
            FastFourierTransform temp;
            D3DCSX.CreateFFT1DComplex(context, x, (int)flags, out info, out temp);
            temp.BufferRequirements = info;
            return temp;
        }

        /// <summary>	
        /// Creates a new one-dimensional real FFT.	
        /// </summary>	
        /// <param name="context">Pointer to the <see cref="SharpDX.Direct3D11.DeviceContext"/> interface to use for the FFT. </param>
        /// <param name="x">Length of the first dimension of the FFT. </param>
        /// <returns>an <see cref="SharpDX.Direct3D11.FastFourierTransform"/> interface reference.</returns>
        /// <unmanaged>HRESULT D3DX11CreateFFT1DReal([None] ID3D11DeviceContext* pDeviceContext,[None] int X,[None] int Flags,[Out] D3DX11_FFT_BUFFER_INFO* pBufferInfo,[Out] ID3DX11FFT** ppFFT)</unmanaged>
        public static FastFourierTransform Create1DReal(DeviceContext context, int x)
        {
            return Create1DReal(context, x, FastFourierTransformCreationFlags.None);
        }

        /// <summary>	
        /// Creates a new one-dimensional real FFT.	
        /// </summary>	
        /// <param name="context">Pointer to the <see cref="SharpDX.Direct3D11.DeviceContext"/> interface to use for the FFT. </param>
        /// <param name="x">Length of the first dimension of the FFT. </param>
        /// <param name="flags">Flag affecting the behavior of the FFT, can be 0 or a combination of flags from <see cref="SharpDX.Direct3D11.FastFourierTransformCreationFlags"/>. </param>
        /// <returns>an <see cref="SharpDX.Direct3D11.FastFourierTransform"/> interface reference.</returns>
        /// <unmanaged>HRESULT D3DX11CreateFFT1DReal([None] ID3D11DeviceContext* pDeviceContext,[None] int X,[None] int Flags,[Out] D3DX11_FFT_BUFFER_INFO* pBufferInfo,[Out] ID3DX11FFT** ppFFT)</unmanaged>
        public static FastFourierTransform Create1DReal(DeviceContext context, int x, FastFourierTransformCreationFlags flags)
        {
            FastFourierTransformBufferRequirements info;
            FastFourierTransform temp;
            D3DCSX.CreateFFT1DReal(context, x, (int)flags, out info, out temp);
            temp.BufferRequirements = info;
            return temp;
        }

        /// <summary>	
        /// Creates a new two-dimensional complex FFT.	
        /// </summary>	
        /// <param name="context">Pointer to the <see cref="SharpDX.Direct3D11.DeviceContext"/> interface to use for the FFT. </param>
        /// <param name="x">Length of the first dimension of the FFT.</param>
        /// <param name="y">Length of the second dimension of the FFT.</param>
        /// <returns>an <see cref="SharpDX.Direct3D11.FastFourierTransform"/> interface reference.</returns>
        /// <unmanaged>HRESULT D3DX11CreateFFT1DReal([None] ID3D11DeviceContext* pDeviceContext,[None] int X,[None] int Flags,[Out] D3DX11_FFT_BUFFER_INFO* pBufferInfo,[Out] ID3DX11FFT** ppFFT)</unmanaged>
        public static FastFourierTransform Create2DComplex(DeviceContext context, int x, int y)
        {
            return Create2DComplex(context, x, y, FastFourierTransformCreationFlags.None);
        }

        /// <summary>	
        /// Creates a new two-dimensional complex FFT.	
        /// </summary>	
        /// <param name="context">Pointer to the <see cref="SharpDX.Direct3D11.DeviceContext"/> interface to use for the FFT. </param>
        /// <param name="x">Length of the first dimension of the FFT.</param>
        /// <param name="y">Length of the second dimension of the FFT.</param>
        /// <param name="flags">Flag affecting the behavior of the FFT, can be 0 or a combination of flags from <see cref="SharpDX.Direct3D11.FastFourierTransformCreationFlags"/>. </param>
        /// <returns>an <see cref="SharpDX.Direct3D11.FastFourierTransform"/> interface reference.</returns>
        /// <unmanaged>HRESULT D3DX11CreateFFT1DReal([None] ID3D11DeviceContext* pDeviceContext,[None] int X,[None] int Flags,[Out] D3DX11_FFT_BUFFER_INFO* pBufferInfo,[Out] ID3DX11FFT** ppFFT)</unmanaged>
        public static FastFourierTransform Create2DComplex(DeviceContext context, int x, int y, FastFourierTransformCreationFlags flags)
        {
            FastFourierTransformBufferRequirements info;
            FastFourierTransform temp;
            D3DCSX.CreateFFT2DComplex(context, x, y, (int)flags, out info, out temp);
            temp.BufferRequirements = info;
            return temp;
        }

        /// <summary>	
        /// Creates a new two-dimensional real FFT.	
        /// </summary>	
        /// <param name="context">Pointer to the <see cref="SharpDX.Direct3D11.DeviceContext"/> interface to use for the FFT. </param>
        /// <param name="x">Length of the first dimension of the FFT.</param>
        /// <param name="y">Length of the second dimension of the FFT.</param>
        /// <returns>an <see cref="SharpDX.Direct3D11.FastFourierTransform"/> interface reference.</returns>
        /// <unmanaged>HRESULT D3DX11CreateFFT1DReal([None] ID3D11DeviceContext* pDeviceContext,[None] int X,[None] int Flags,[Out] D3DX11_FFT_BUFFER_INFO* pBufferInfo,[Out] ID3DX11FFT** ppFFT)</unmanaged>
        public static FastFourierTransform Create2DReal(DeviceContext context, int x, int y)
        {
            return Create2DReal(context, x, y, FastFourierTransformCreationFlags.None);
        }

        /// <summary>	
        /// Creates a new two-dimensional real FFT.	
        /// </summary>	
        /// <param name="context">Pointer to the <see cref="SharpDX.Direct3D11.DeviceContext"/> interface to use for the FFT. </param>
        /// <param name="x">Length of the first dimension of the FFT.</param>
        /// <param name="y">Length of the second dimension of the FFT.</param>
        /// <param name="flags">Flag affecting the behavior of the FFT, can be 0 or a combination of flags from <see cref="SharpDX.Direct3D11.FastFourierTransformCreationFlags"/>. </param>
        /// <returns>an <see cref="SharpDX.Direct3D11.FastFourierTransform"/> interface reference.</returns>
        /// <unmanaged>HRESULT D3DX11CreateFFT1DReal([None] ID3D11DeviceContext* pDeviceContext,[None] int X,[None] int Flags,[Out] D3DX11_FFT_BUFFER_INFO* pBufferInfo,[Out] ID3DX11FFT** ppFFT)</unmanaged>
        public static FastFourierTransform Create2DReal(DeviceContext context, int x, int y, FastFourierTransformCreationFlags flags)
        {
            FastFourierTransformBufferRequirements info;
            FastFourierTransform temp;
            D3DCSX.CreateFFT2DReal(context, x, y, (int)flags, out info, out temp);
            temp.BufferRequirements = info;
            return temp;
        }

        /// <summary>	
        /// Creates a new three-dimensional complex FFT.	
        /// </summary>	
        /// <param name="context">Pointer to the <see cref="SharpDX.Direct3D11.DeviceContext"/> interface to use for the FFT. </param>
        /// <param name="x">Length of the first dimension of the FFT.</param>
        /// <param name="y">Length of the second dimension of the FFT.</param>
        /// <param name="z">Length of the third dimension of the FFT.</param>
        /// <returns>an <see cref="SharpDX.Direct3D11.FastFourierTransform"/> interface reference.</returns>
        /// <unmanaged>HRESULT D3DX11CreateFFT1DReal([None] ID3D11DeviceContext* pDeviceContext,[None] int X,[None] int Flags,[Out] D3DX11_FFT_BUFFER_INFO* pBufferInfo,[Out] ID3DX11FFT** ppFFT)</unmanaged>
        public static FastFourierTransform Create3DComplex(DeviceContext context, int x, int y, int z)
        {
            return Create3DComplex(context, x, y, z, FastFourierTransformCreationFlags.None);
        }

        /// <summary>	
        /// Creates a new three-dimensional complex FFT.	
        /// </summary>	
        /// <param name="context">Pointer to the <see cref="SharpDX.Direct3D11.DeviceContext"/> interface to use for the FFT. </param>
        /// <param name="x">Length of the first dimension of the FFT.</param>
        /// <param name="y">Length of the second dimension of the FFT.</param>
        /// <param name="z">Length of the third dimension of the FFT.</param>
        /// <param name="flags">Flag affecting the behavior of the FFT, can be 0 or a combination of flags from <see cref="SharpDX.Direct3D11.FastFourierTransformCreationFlags"/>. </param>
        /// <returns>an <see cref="SharpDX.Direct3D11.FastFourierTransform"/> interface reference.</returns>
        /// <unmanaged>HRESULT D3DX11CreateFFT1DReal([None] ID3D11DeviceContext* pDeviceContext,[None] int X,[None] int Flags,[Out] D3DX11_FFT_BUFFER_INFO* pBufferInfo,[Out] ID3DX11FFT** ppFFT)</unmanaged>
        public static FastFourierTransform Create3DComplex(DeviceContext context, int x, int y, int z, FastFourierTransformCreationFlags flags)
        {
            FastFourierTransformBufferRequirements info;
            FastFourierTransform temp;
            D3DCSX.CreateFFT3DComplex(context, x, y, z, (int)flags, out info, out temp);
            temp.BufferRequirements = info;
            return temp;
        }

        /// <summary>	
        /// Creates a new three-dimensional real FFT.	
        /// </summary>	
        /// <param name="context">Pointer to the <see cref="SharpDX.Direct3D11.DeviceContext"/> interface to use for the FFT. </param>
        /// <param name="x">Length of the first dimension of the FFT.</param>
        /// <param name="y">Length of the second dimension of the FFT.</param>
        /// <param name="z">Length of the third dimension of the FFT.</param>
        /// <returns>an <see cref="SharpDX.Direct3D11.FastFourierTransform"/> interface reference.</returns>
        /// <unmanaged>HRESULT D3DX11CreateFFT1DReal([None] ID3D11DeviceContext* pDeviceContext,[None] int X,[None] int Flags,[Out] D3DX11_FFT_BUFFER_INFO* pBufferInfo,[Out] ID3DX11FFT** ppFFT)</unmanaged>
        public static FastFourierTransform Create3DReal(DeviceContext context, int x, int y, int z)
        {
            return Create3DReal(context, x, y, z, FastFourierTransformCreationFlags.None);
        }

        /// <summary>	
        /// Creates a new three-dimensional real FFT.	
        /// </summary>	
        /// <param name="context">Pointer to the <see cref="SharpDX.Direct3D11.DeviceContext"/> interface to use for the FFT. </param>
        /// <param name="x">Length of the first dimension of the FFT.</param>
        /// <param name="y">Length of the second dimension of the FFT.</param>
        /// <param name="z">Length of the third dimension of the FFT.</param>
        /// <param name="flags">Flag affecting the behavior of the FFT, can be 0 or a combination of flags from <see cref="SharpDX.Direct3D11.FastFourierTransformCreationFlags"/>. </param>
        /// <returns>an <see cref="SharpDX.Direct3D11.FastFourierTransform"/> interface reference.</returns>
        /// <unmanaged>HRESULT D3DX11CreateFFT1DReal([None] ID3D11DeviceContext* pDeviceContext,[None] int X,[None] int Flags,[Out] D3DX11_FFT_BUFFER_INFO* pBufferInfo,[Out] ID3DX11FFT** ppFFT)</unmanaged>
        public static FastFourierTransform Create3DReal(DeviceContext context, int x, int y, int z, FastFourierTransformCreationFlags flags)
        {
            FastFourierTransformBufferRequirements info;
            FastFourierTransform temp;
            D3DCSX.CreateFFT3DReal(context, x, y, z, (int)flags, out info, out temp);
            temp.BufferRequirements = info;
            return temp;
        }

        /// <summary>
        /// Gets the buffer requirements.
        /// </summary>
        /// <value>The buffer requirements.</value>
        public FastFourierTransformBufferRequirements BufferRequirements
        {
            get { return _bufferRequirements; }
            private set { _bufferRequirements = value; }
        }
    }
}