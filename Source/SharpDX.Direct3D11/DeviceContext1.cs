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
    public partial class DeviceContext1
    {
        /// <summary>
        /// Initializes a new deferred context instance of <see cref="SharpDX.Direct3D11.DeviceContext1"/> class.
        /// </summary>
        /// <param name="device"></param>
        public DeviceContext1(Device1 device) : base(IntPtr.Zero)
        {
            device.CreateDeferredContext1(0, this);
        }

        /// <summary>
        /// Partially clears a view using an array of rectangles
        /// </summary>
        /// <param name="viewRef">View to clear</param>
        /// <param name="color">Clear color</param>
        /// <param name="rectangles">Rectangle areas</param>
        public void ClearView(SharpDX.Direct3D11.ResourceView viewRef, SharpDX.Mathematics.Interop.RawColor4 color, params SharpDX.Mathematics.Interop.RawRectangle[] rectangles)
        {
            ClearView(viewRef, color, rectangles, rectangles.Length);
        }
    }
}