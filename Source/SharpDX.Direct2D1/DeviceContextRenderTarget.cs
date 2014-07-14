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
    public partial class DeviceContextRenderTarget
    {
        /// <summary>	
        /// Creates a render target that draws to a Windows Graphics Device Interface (GDI) device context.	
        /// </summary>	
        /// <remarks>	
        /// Before you can render with a DC render target, you must use the render target's {{BindDC}} method to associate it with a GDI DC.  Do this for each different DC and whenever there is a change in the size of the area you want to draw to.To enable the DC render target to work with GDI, set the render target's DXGI format to <see cref="SharpDX.DXGI.Format.B8G8R8A8_UNorm"/> and alpha mode to <see cref="SharpDX.Direct2D1.AlphaMode.Premultiplied"/> or D2D1_ALPHA_MODE_IGNORE.Your application should create render targets once and hold on to them for the life of the application or until the render target's  {{EndDraw}} method returns the {{D2DERR_RECREATE_TARGET}} error. When you receive this error, recreate the render target (and any resources it created).	
        /// </remarks>	
        /// <param name="factory">an instance of <see cref = "SharpDX.Direct2D1.Factory" /></param>
        /// <param name="properties">The rendering mode, pixel format, remoting options, DPI information, and the minimum DirectX support required for hardware rendering.  To enable the device context (DC) render target to work with GDI, set the DXGI format to <see cref="SharpDX.DXGI.Format.B8G8R8A8_UNorm"/> and the alpha mode to <see cref="SharpDX.Direct2D1.AlphaMode.Premultiplied"/> or D2D1_ALPHA_MODE_IGNORE. For more information about pixel formats, see  {{Supported Pixel  Formats and Alpha Modes}}.</param>
        public DeviceContextRenderTarget(Factory factory, RenderTargetProperties properties)
            : base(IntPtr.Zero)
        {
            factory.CreateDCRenderTarget(ref properties, this);
        }
    }
}
