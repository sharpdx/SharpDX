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
    public partial class DeviceContext4
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceContext4"/> class using an existing <see cref="Device4"/>.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="options">The options to be applied to the created device context.</param>
        /// <remarks>
        /// The new device context will not have a  selected target bitmap. The caller must create and select a bitmap as the target surface of the context.
        /// </remarks>
        /// <unmanaged>HRESULT ID2D1Device4::CreateDeviceContext([In] D2D1_DEVICE_CONTEXT_OPTIONS options,[Out] ID2D1DeviceContext4** DeviceContext4)</unmanaged>
        public DeviceContext4(Device4 device, DeviceContextOptions options)
            : base(IntPtr.Zero)
        {
            device.CreateDeviceContext(options, this);
        }
    }
}