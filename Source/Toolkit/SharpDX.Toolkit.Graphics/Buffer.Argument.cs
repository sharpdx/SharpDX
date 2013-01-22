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

using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    public partial class Buffer
    {
        /// <summary>
        /// Argument buffer helper methods.
        /// </summary>
        public static class Argument
        {
            /// <summary>
            /// Creates a new Argument buffer <see cref="ResourceUsage.Default"/> usage.
            /// </summary>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="size">The size in bytes.</param>
            /// <param name="usage">The usage.</param>
            /// <returns>A Argument buffer</returns>
            public static Buffer New(GraphicsDevice device, int size, ResourceUsage usage = ResourceUsage.Default)
            {
                return Buffer.New(device, size, BufferFlags.ArgumentBuffer, usage);
            }

            /// <summary>
            /// Creates a new Argument buffer <see cref="ResourceUsage.Default"/> usage.
            /// </summary>
            /// <typeparam name="T">Type of the Argument buffer to get the sizeof from</typeparam>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="usage">The usage.</param>
            /// <returns>A Argument buffer</returns>
            public static Buffer<T> New<T>(GraphicsDevice device, ResourceUsage usage = ResourceUsage.Default) where T : struct
            {
                return Buffer.New<T>(device, 1, BufferFlags.ArgumentBuffer, usage);
            }

            /// <summary>
            /// Creates a new Argument buffer <see cref="ResourceUsage.Default"/> usage.
            /// </summary>
            /// <typeparam name="T">Type of the Argument buffer to get the sizeof from</typeparam>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="value">The value to initialize the Argument buffer.</param>
            /// <param name="usage">The usage of this resource.</param>
            /// <returns>A Argument buffer</returns>
            public static Buffer<T> New<T>(GraphicsDevice device, ref T value, ResourceUsage usage = ResourceUsage.Default) where T : struct
            {
                return Buffer.New(device, ref value, BufferFlags.ArgumentBuffer, usage);
            }
        }
    }
}
