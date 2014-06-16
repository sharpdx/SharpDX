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

namespace SharpDX.DirectWrite
{
    public partial class RenderingParams
    {
        /// <summary>	
        /// Creates a rendering parameters object with default settings for the primary monitor. Different monitors may have different rendering parameters, for more information see the {{How to Add Support for Multiple Monitors}} topic.	
        /// </summary>	
        /// <param name="factory">A reference to a DirectWrite factory <see cref="Factory"/></param>
        /// <unmanaged>HRESULT IDWriteFactory::CreateRenderingParams([Out] IDWriteRenderingParams** renderingParams)</unmanaged>
        public RenderingParams(Factory factory)
        {
            factory.CreateRenderingParams(this);
        }

        /// <summary>	
        /// Creates a rendering parameters object with default settings for the specified monitor. In most cases, this is the preferred way to create a rendering parameters object.	
        /// </summary>	
        /// <param name="factory">A reference to a DirectWrite factory <see cref="Factory"/></param>
        /// <param name="monitorHandle">A handle for the specified monitor. </param>
        /// <unmanaged>HRESULT IDWriteFactory::CreateMonitorRenderingParams([None] void* monitor,[Out] IDWriteRenderingParams** renderingParams)</unmanaged>
        public RenderingParams(Factory factory, IntPtr monitorHandle)
        {
            factory.CreateMonitorRenderingParams(monitorHandle, this);
        }

        /// <summary>	
        /// Creates a rendering parameters object with the specified properties. 	
        /// </summary>	
        /// <param name="factory">A reference to a DirectWrite factory <see cref="Factory"/></param>
        /// <param name="gamma">The gamma level to be set for the new rendering parameters object. </param>
        /// <param name="enhancedContrast">The enhanced contrast level to be set for the new rendering parameters object. </param>
        /// <param name="clearTypeLevel">The ClearType level to be set for the new rendering parameters object. </param>
        /// <param name="pixelGeometry">Represents the internal structure of a device pixel (that is, the physical arrangement of red, green, and blue color components) that is assumed for purposes of rendering text. </param>
        /// <param name="renderingMode">A value that represents the method (for example, ClearType natural quality) for rendering glyphs. </param>
        /// <unmanaged>HRESULT IDWriteFactory::CreateCustomRenderingParams([None] float gamma,[None] float enhancedContrast,[None] float clearTypeLevel,[None] DWRITE_PIXEL_GEOMETRY pixelGeometry,[None] DWRITE_RENDERING_MODE renderingMode,[Out] IDWriteRenderingParams** renderingParams)</unmanaged>
        public RenderingParams(Factory factory, float gamma, float enhancedContrast, float clearTypeLevel, PixelGeometry pixelGeometry, RenderingMode renderingMode)
        {
            factory.CreateCustomRenderingParams(gamma, enhancedContrast, clearTypeLevel, pixelGeometry, renderingMode, this);
        }
    }
}