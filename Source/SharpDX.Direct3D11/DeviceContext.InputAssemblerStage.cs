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
    public partial class InputAssemblerStage
    {
        /// <summary>
        ///   Binds a single vertex buffer to the input assembler.
        /// </summary>
        /// <param name = "slot">Index of the slot to which to bind the vertex buffer.</param>
        /// <param name = "vertexBufferBinding">A binding for the input vertex buffer.</param>
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
        ///   Binds an array of vertex buffers to the input assembler.
        /// </summary>
        /// <param name = "firstSlot">Index of the first input slot to use for binding. The first vertex buffer is explicitly bound to the start slot; this causes each additional vertex buffer in the array to be implicitly bound to each subsequent input slot. There are 16 input slots.</param>
        /// <param name = "vertexBufferBindings">An array of bindings for input vertex buffers.</param>
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
        /// Binds an array of vertex buffers to the input assembler.
        /// </summary>
        /// <param name="slot">Index of the first input slot to use for binding. The first vertex buffer is explicitly bound to the start slot; this causes each additional vertex buffer in the array to be implicitly bound to each subsequent input slot. There are 16 input slots.</param>
        /// <param name="vertexBuffers">The vertex buffers.</param>
        /// <param name="stridesRef">The strides.</param>
        /// <param name="offsetsRef">The offsets.</param>
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