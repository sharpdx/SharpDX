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
    public partial class Device
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <unmanaged>HRESULT D2D1CreateDevice([In] IDXGIDevice* dxgiDevice,[In, Optional] const D2D1_CREATION_PROPERTIES* creationProperties,[Out] ID2D1Device** d2dDevice)</unmanaged>
        public Device(SharpDX.DXGI.Device device)
            : base(IntPtr.Zero)
        {
            D2D1.CreateDevice(device, null, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="creationProperties">The creation properties.</param>
        /// <unmanaged>HRESULT D2D1CreateDevice([In] IDXGIDevice* dxgiDevice,[In, Optional] const D2D1_CREATION_PROPERTIES* creationProperties,[Out] ID2D1Device** d2dDevice)</unmanaged>
        public Device(SharpDX.DXGI.Device device, CreationProperties creationProperties)
            : base(IntPtr.Zero)
        {
            D2D1.CreateDevice(device, creationProperties, this);
        }        

        /// <summary>	
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>	
        /// <param name="factory"><para>The <see cref="Factory1"/> object used when creating  the <see cref="SharpDX.Direct2D1.Device"/>. </para></param>	
        /// <param name="device"><para>The <see cref="SharpDX.DXGI.Device"/> object used when creating  the <see cref="SharpDX.Direct2D1.Device"/>. </para></param>	
        /// <remarks>	
        /// Each call to CreateDevice returns a unique <see cref="SharpDX.Direct2D1.Device"/> object.The <see cref="SharpDX.DXGI.Device"/> object is obtained by calling QueryInterface on an ID3D10Device or an ID3D11Device.	
        /// </remarks>	
        /// <unmanaged>HRESULT ID2D1Factory1::CreateDevice([In] IDXGIDevice* dxgiDevice,[Out] ID2D1Device** d2dDevice)</unmanaged>	
        public Device(Factory1 factory, SharpDX.DXGI.Device device)
            : base(IntPtr.Zero)
        {
            factory.CreateDevice(device, this);
        }        
    }
}