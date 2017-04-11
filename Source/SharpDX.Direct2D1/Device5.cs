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
    public partial class Device5
    {
        /// <summary>	
        /// Initializes a new instance of the <see cref="Device5"/> class.
        /// </summary>	
        /// <param name="factory"><para>The <see cref="Factory4"/> object used when creating  the <see cref="SharpDX.Direct2D1.Device5"/>. </para></param>	
        /// <param name="device"><para>The <see cref="SharpDX.DXGI.Device"/> object used when creating  the <see cref="SharpDX.Direct2D1.Device5"/>. </para></param>	
        /// <remarks>	
        /// Each call to CreateDevice returns a unique <see cref="SharpDX.Direct2D1.Device5"/> object.The <see cref="SharpDX.DXGI.Device"/> object is obtained by calling QueryInterface on an ID3D10Device or an ID3D11Device.	
        /// </remarks>	
        /// <unmanaged>HRESULT ID2D1Factory3::CreateDevice([In] IDXGIDevice* dxgiDevice,[Out] ID2D1Device2** d2dDevice2)</unmanaged>	
        public Device5(Factory6 factory, SharpDX.DXGI.Device device)
            : base(IntPtr.Zero)
        {
            factory.CreateDevice(device, this);
        }
    }
}
