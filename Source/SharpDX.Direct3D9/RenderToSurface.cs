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

namespace SharpDX.Direct3D9
{
    public partial class RenderToSurface
    {
        /// <summary>	
        /// Creates a render surface.
        /// </summary>	
        /// <param name="device"><dd>  <p>Pointer to an <strong><see cref="SharpDX.Direct3D9.Device"/></strong> interface, the device to be associated with the render surface.</p> </dd></param>	
        /// <param name="width"><dd>  <p>Width of the render surface, in pixels.</p> </dd></param>	
        /// <param name="height"><dd>  <p>Height of the render surface, in pixels.</p> </dd></param>	
        /// <param name="format"><dd>  <p>Member of the <see cref="SharpDX.Direct3D9.Format"/> enumerated type, describing the pixel format of the render surface.</p> </dd></param>	
        /// <param name="depthStencil"><dd>  <p>If <strong>TRUE</strong>, the render surface supports a depth-stencil surface. Otherwise, this member is set to <strong><see cref="SharpDX.Result.False"/></strong>. This function will create a new depth buffer.</p> </dd></param>	
        /// <param name="depthStencilFormat"><dd>  <p>If  <em>DepthStencil</em> is set to <strong>TRUE</strong>, this parameter is a member of the <see cref="SharpDX.Direct3D9.Format"/> enumerated type, describing the depth-stencil format of the render surface.</p> </dd></param>	
        /// <msdn-id>bb172791</msdn-id>	
        /// <unmanaged>HRESULT D3DXCreateRenderToSurface([In] IDirect3DDevice9* pDevice,[In] unsigned int Width,[In] unsigned int Height,[In] D3DFORMAT Format,[In] BOOL DepthStencil,[In] D3DFORMAT DepthStencilFormat,[In] ID3DXRenderToSurface** ppRenderToSurface)</unmanaged>	
        /// <unmanaged-short>D3DXCreateRenderToSurface</unmanaged-short>	
        public RenderToSurface(SharpDX.Direct3D9.Device device, int width, int height, SharpDX.Direct3D9.Format format, bool depthStencil = false, SharpDX.Direct3D9.Format depthStencilFormat = Format.Unknown) : base(IntPtr.Zero)
        {
            D3DX9.CreateRenderToSurface(device, width, height, format, depthStencil, depthStencilFormat, this);
        }
    }
}