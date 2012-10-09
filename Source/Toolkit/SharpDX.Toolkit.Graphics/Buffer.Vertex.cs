// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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

using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    public partial class Buffer
    {
        /// <summary>
        /// Vertex buffer helper methods.
        /// </summary>
        public static class Vertex
        {
            /// <summary>
            /// Creates a new Vertex buffer with <see cref="ResourceUsage.Default"/> uasge by default.
            /// </summary>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="size">The size in bytes.</param>
            /// <param name="usage">The usage.</param>
            /// <returns>A Vertex buffer</returns>
            public static Buffer New(GraphicsDevice device, int size, ResourceUsage usage = ResourceUsage.Default)
            {
                return Buffer.New(device,size, BufferFlags.VertexBuffer, usage);
            }

            /// <summary>
            /// Creates a new Vertex buffer with <see cref="ResourceUsage.Default"/> uasge by default.
            /// </summary>
            /// <typeparam name="T">Type of the Vertex buffer to get the sizeof from</typeparam>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="vertexBufferCount">Number of vertex in this buffer with the sizeof(T).</param>
            /// <param name="usage">The usage.</param>
            /// <returns>A Vertex buffer</returns>
            public static Buffer<T> New<T>(GraphicsDevice device, int vertexBufferCount, ResourceUsage usage = ResourceUsage.Default) where T : struct
            {
                return Buffer.New<T>(device, vertexBufferCount, BufferFlags.VertexBuffer, usage);
            }

            /// <summary>
            /// Creates a new Vertex buffer with <see cref="ResourceUsage.Immutable"/> uasge by default.
            /// </summary>
            /// <typeparam name="T">Type of the Vertex buffer to get the sizeof from</typeparam>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="value">The value to initialize the Vertex buffer.</param>
            /// <param name="usage">The usage of this resource.</param>
            /// <returns>A Vertex buffer</returns>
            public static Buffer<T> New<T>(GraphicsDevice device, T[] value, ResourceUsage usage = ResourceUsage.Immutable) where T : struct
            {
                return Buffer.New(device,value, BufferFlags.VertexBuffer, usage);
            }

            /// <summary>
            /// Creates a new Vertex buffer with <see cref="ResourceUsage.Immutable"/> uasge by default.
            /// </summary>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="value">The value to initialize the Vertex buffer.</param>
            /// <param name="usage">The usage of this resource.</param>
            /// <returns>A Vertex buffer</returns>
            public static Buffer New(GraphicsDevice device, DataPointer value, ResourceUsage usage = ResourceUsage.Immutable)
            {
                return Buffer.New(device,value, 0, BufferFlags.VertexBuffer, usage);
            }
        }
    }
}