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

#if WIN8
namespace SharpDX.Direct2D1
{
    public partial class DeviceContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceContext"/> class.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <unmanaged>HRESULT D2D1CreateDeviceContext([In] IDXGISurface* dxgiSurface,[In, Optional] const D2D1_CREATION_PROPERTIES* creationProperties,[Out] ID2D1DeviceContext** d2dDeviceContext)</unmanaged>	
        public DeviceContext(SharpDX.DXGI.Surface surface)
            : base(IntPtr.Zero)
        {
            D2D1.CreateDeviceContext(surface, null, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="creationProperties">The creation properties.</param>
        /// <unmanaged>HRESULT D2D1CreateDeviceContext([In] IDXGISurface* dxgiSurface,[In, Optional] const D2D1_CREATION_PROPERTIES* creationProperties,[Out] ID2D1DeviceContext** d2dDeviceContext)</unmanaged>	
        public DeviceContext(SharpDX.DXGI.Surface surface, CreationProperties creationProperties)
            : base(IntPtr.Zero)
        {
            D2D1.CreateDeviceContext(surface, creationProperties, this);
        }
    }
}
#endif