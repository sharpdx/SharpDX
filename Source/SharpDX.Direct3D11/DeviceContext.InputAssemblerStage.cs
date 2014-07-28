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
    public partial class InputAssemblerStage
    {
        /// <summary>
        /// <p>Bind a single vertex buffer to the input-assembler stage.</p>	
        /// </summary>	
        /// <param name="slot"><dd>  <p>The first input slot for binding. The first vertex buffer is explicitly bound to the start slot; this causes each additional vertex buffer in the array to be implicitly bound to each subsequent input slot. The maximum of 16 or 32 input slots (ranges from 0 to <see cref="SharpDX.Direct3D11.InputAssemblerStage.VertexInputResourceSlotCount"/> - 1) are available; the maximum number of input slots depends on the feature level.</p> </dd></param>	
        /// <param name="vertexBufferBinding"><dd>  <p>A <see cref="SharpDX.Direct3D11.VertexBufferBinding"/>. The vertex buffer must have been created with the <strong><see cref="SharpDX.Direct3D11.BindFlags.VertexBuffer"/></strong> flag.</p> </dd></param>        /// <remarks>	
        /// <p>For information about creating vertex buffers, see Create a Vertex Buffer.</p><p>Calling this method using a buffer that is currently bound for writing (i.e. bound to the stream output pipeline stage) will effectively bind <strong><c>null</c></strong> instead because a buffer cannot be bound as both an input and an output at the same time.</p><p>The debug layer will generate a warning whenever a resource is prevented from being bound simultaneously as an input and an output, but this will not prevent invalid data from being used by the runtime.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p>	
        /// </remarks>	
        /// <msdn-id>ff476456</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::IASetVertexBuffers([In] unsigned int StartSlot,[In] unsigned int NumBuffers,[In, Buffer] const void* ppVertexBuffers,[In, Buffer] const void* pStrides,[In, Buffer] const void* pOffsets)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::IASetVertexBuffers</unmanaged-short>	
        public void SetVertexBuffers(int slot, VertexBufferBinding vertexBufferBinding)
        {
            unsafe
            {
                int stride = vertexBufferBinding.Stride;
                int offset = vertexBufferBinding.Offset;
                IntPtr pVertexBuffers = vertexBufferBinding.Buffer == null ? IntPtr.Zero : vertexBufferBinding.Buffer.NativePointer;
                SetVertexBuffers(slot, 1, new IntPtr(&pVertexBuffers), new IntPtr(&stride), new IntPtr(&offset));
            }
        }

        /// <summary>
        /// <p>Bind an array of vertex buffers to the input-assembler stage.</p>	
        /// </summary>	
        /// <param name="firstSlot"><dd>  <p>The first input slot for binding. The first vertex buffer is explicitly bound to the start slot; this causes each additional vertex buffer in the array to be implicitly bound to each subsequent input slot. The maximum of 16 or 32 input slots (ranges from 0 to <see cref="SharpDX.Direct3D11.InputAssemblerStage.VertexInputResourceSlotCount"/> - 1) are available; the maximum number of input slots depends on the feature level.</p> </dd></param>	
        /// <param name="vertexBufferBindings"><dd>  <p>A reference to an array of <see cref="SharpDX.Direct3D11.VertexBufferBinding"/>. The vertex buffers must have been created with the <strong><see cref="SharpDX.Direct3D11.BindFlags.VertexBuffer"/></strong> flag.</p> </dd></param>        /// <remarks>	
        /// <p>For information about creating vertex buffers, see Create a Vertex Buffer.</p><p>Calling this method using a buffer that is currently bound for writing (i.e. bound to the stream output pipeline stage) will effectively bind <strong><c>null</c></strong> instead because a buffer cannot be bound as both an input and an output at the same time.</p><p>The debug layer will generate a warning whenever a resource is prevented from being bound simultaneously as an input and an output, but this will not prevent invalid data from being used by the runtime.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p>	
        /// </remarks>	
        /// <msdn-id>ff476456</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::IASetVertexBuffers([In] unsigned int StartSlot,[In] unsigned int NumBuffers,[In, Buffer] const void* ppVertexBuffers,[In, Buffer] const void* pStrides,[In, Buffer] const void* pOffsets)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::IASetVertexBuffers</unmanaged-short>	
        public void SetVertexBuffers(int firstSlot, params VertexBufferBinding[] vertexBufferBindings)
        {
            unsafe
            {
                int length = vertexBufferBindings.Length;
                IntPtr* vertexBuffers = stackalloc IntPtr[length];
                var strides = stackalloc int[length];
                var offsets = stackalloc int[length];
                for (int i = 0; i < vertexBufferBindings.Length; i++)
                {
                    vertexBuffers[i] = (vertexBufferBindings[i].Buffer == null) ? IntPtr.Zero : vertexBufferBindings[i].Buffer.NativePointer;
                    strides[i] = vertexBufferBindings[i].Stride;
                    offsets[i] = vertexBufferBindings[i].Offset;
                }
                SetVertexBuffers(firstSlot, length, new IntPtr(vertexBuffers), new IntPtr(strides), new IntPtr(offsets));
            }
        }

        /// <summary>
        /// <p>Bind an array of vertex buffers to the input-assembler stage.</p>	
        /// </summary>	
        /// <param name="slot"><dd>  <p>The first input slot for binding. The first vertex buffer is explicitly bound to the start slot; this causes each additional vertex buffer in the array to be implicitly bound to each subsequent input slot. The maximum of 16 or 32 input slots (ranges from 0 to <see cref="SharpDX.Direct3D11.InputAssemblerStage.VertexInputResourceSlotCount"/> - 1) are available; the maximum number of input slots depends on the feature level.</p> </dd></param>	
        /// <param name="vertexBuffers"><dd>  <p>A reference to an array of vertex buffers (see <strong><see cref="SharpDX.Direct3D11.Buffer"/></strong>). The vertex buffers must have been created with the <strong><see cref="SharpDX.Direct3D11.BindFlags.VertexBuffer"/></strong> flag.</p> </dd></param>	
        /// <param name="stridesRef"><dd>  <p>Pointer to an array of stride values; one stride value for each buffer in the vertex-buffer array. Each stride is the size (in bytes) of the elements that are to be used from that vertex buffer.</p> </dd></param>	
        /// <param name="offsetsRef"><dd>  <p>Pointer to an array of offset values; one offset value for each buffer in the vertex-buffer array. Each offset is the number of bytes between the first element of a vertex buffer and the first element that will be used.</p> </dd></param>	
        /// <remarks>	
        /// <p>For information about creating vertex buffers, see Create a Vertex Buffer.</p><p>Calling this method using a buffer that is currently bound for writing (i.e. bound to the stream output pipeline stage) will effectively bind <strong><c>null</c></strong> instead because a buffer cannot be bound as both an input and an output at the same time.</p><p>The debug layer will generate a warning whenever a resource is prevented from being bound simultaneously as an input and an output, but this will not prevent invalid data from being used by the runtime.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p>	
        /// </remarks>	
        /// <msdn-id>ff476456</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::IASetVertexBuffers([In] unsigned int StartSlot,[In] unsigned int NumBuffers,[In, Buffer] const void* ppVertexBuffers,[In, Buffer] const void* pStrides,[In, Buffer] const void* pOffsets)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::IASetVertexBuffers</unmanaged-short>	
        public void SetVertexBuffers(int slot, SharpDX.Direct3D11.Buffer[] vertexBuffers, int[] stridesRef, int[] offsetsRef)
        {
            unsafe
            {
                IntPtr* pVertexBuffers = stackalloc IntPtr[vertexBuffers.Length];
                for (int i = 0; i < vertexBuffers.Length; i++)
                    pVertexBuffers[i] = (vertexBuffers[i] == null) ? IntPtr.Zero : vertexBuffers[i].NativePointer;
                fixed (void* stridesRef_ = stridesRef)
                fixed (void* offsetsRef_ = offsetsRef)
                    SetVertexBuffers(slot, vertexBuffers.Length, new IntPtr(pVertexBuffers), (IntPtr)stridesRef_, (IntPtr)offsetsRef_);
            }
        }
    }
}