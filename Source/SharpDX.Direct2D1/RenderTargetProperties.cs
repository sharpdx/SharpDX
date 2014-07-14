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

namespace SharpDX.Direct2D1
{
    public partial struct RenderTargetProperties
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderTargetProperties"/> struct.
        /// </summary>
        /// <param name="pixelFormat">The pixel format and alpha mode of the render target. You can use the {{D2D1::PixelFormat}} function to create a pixel format that specifies that Direct2D should select the pixel format and alpha mode for you. For a list of pixel formats and alpha modes supported by each render target, see {{Supported Pixel Formats and Alpha Modes}}.</param>
        public RenderTargetProperties(PixelFormat pixelFormat) : this(RenderTargetType.Default, pixelFormat, 96, 96, RenderTargetUsage.None, FeatureLevel.Level_DEFAULT)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderTargetProperties"/> struct.
        /// </summary>
        /// <param name="type">A value that specifies whether the render target should force hardware or software rendering. A value of <see cref="SharpDX.Direct2D1.RenderTargetType.Default"/> specifies that the render target should use hardware rendering if it is available; otherwise, it uses software rendering. Note that WIC bitmap render targets do not support hardware rendering.</param>
        /// <param name="pixelFormat">The pixel format and alpha mode of the render target. You can use the {{D2D1::PixelFormat}} function to create a pixel format that specifies that Direct2D should select the pixel format and alpha mode for you. For a list of pixel formats and alpha modes supported by each render target, see {{Supported Pixel Formats and Alpha Modes}}.</param>
        /// <param name="dpiX">The horizontal DPI of the render target.  To use the default DPI, set dpiX and dpiY to 0. For more information, see the Remarks section. 	</param>
        /// <param name="dpiY">The vertical DPI of the render target. To use the default DPI, set dpiX and dpiY to 0.  For more information, see the Remarks section. 	</param>
        /// <param name="usage">A value that specifies how the render target is remoted and whether it should be GDI-compatible.  Set to <see cref="SharpDX.Direct2D1.RenderTargetUsage.None"/> to create a render target that is not compatible with GDI and uses Direct3D command-stream remoting if it  is available.</param>
        /// <param name="minLevel">A value that specifies the minimum Direct3D feature level required for hardware rendering. If the specified minimum level is not available, the render target uses software rendering if the type  member is set to <see cref="SharpDX.Direct2D1.RenderTargetType.Default"/>; if  type  is set to to D2D1_RENDER_TARGET_TYPE_HARDWARE, render target creation fails. A value of <see cref="SharpDX.Direct2D1.FeatureLevel.Level_DEFAULT"/> indicates that Direct2D should determine whether the Direct3D feature level of the device is adequate. This field is used only when creating <see cref="WindowRenderTarget"/> and <see cref="DeviceContextRenderTarget"/> objects.	</param>  
        public RenderTargetProperties(RenderTargetType type, PixelFormat pixelFormat, float dpiX, float dpiY, RenderTargetUsage usage, FeatureLevel minLevel)
        {
            Type = type;
            PixelFormat = pixelFormat;
            DpiX = dpiX;
            DpiY = dpiY;
            Usage = usage;
            MinLevel = minLevel;
        }
    }
}
