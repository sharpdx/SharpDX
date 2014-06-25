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

#if DIRECTX11_1 && !WP8

namespace SharpDX.Toolkit.Graphics
{
    using System;
    using Direct2D1;
    using Factory1 = DirectWrite.Factory1;

    /// <summary>
    /// Provides Direct2DService support for drawing on D3D11.1 SwapChain
    /// </summary>
    public interface IDirect2DService : IDisposable
    {
        /// <summary>
        /// Gets a reference to the Direct2DService device. Can be used to create additional <see cref="Direct2D1.DeviceContext" />.
        /// </summary>
        Device Device { get; }

        /// <summary>
        /// Gets a reference to the default <see cref="Direct2D1.DeviceContext" /> which will draw directly over SwapChain.
        /// The developer is responsible to restore default render target states.
        /// </summary>
        DeviceContext DeviceContext { get; }

        /// <summary>
        /// Gets a reference to the default <see cref="SharpDX.DirectWrite.Factory1" /> used to create all DirectWrite objects.
        /// </summary>
        Factory1 DirectWriteFactory { get; }
    }
}

#endif