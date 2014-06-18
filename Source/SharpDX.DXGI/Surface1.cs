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
using SharpDX.Mathematics.Interop;

namespace SharpDX.DXGI
{
    public partial class Surface1
    {
        /// <summary>	
        /// Releases the GDI device context (DC) associated with the current surface and allows rendering using Direct3D. The whole surface to be considered dirty.
        /// </summary>	
        /// <remarks>	
        /// Use the ReleaseDC method to release the DC and indicate that your application has finished all GDI rendering to this surface.   You must call the ReleaseDC method before you perform addition rendering using Direct3D. Prior to resizing buffers all outstanding DCs must be released. 	
        /// </remarks>	
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code. </returns>
        /// <unmanaged>HRESULT IDXGISurface1::ReleaseDC([In, Optional] RECT* pDirtyRect)</unmanaged>
        public void ReleaseDC()
        {
            ReleaseDC_(null);
        }

        /// <summary>	
        /// Releases the GDI device context (DC) associated with the current surface and allows rendering using Direct3D.	
        /// </summary>	
        /// <remarks>	
        /// Use the ReleaseDC method to release the DC and indicate that your application has finished all GDI rendering to this surface.   You must call the ReleaseDC method before you perform addition rendering using Direct3D. Prior to resizing buffers all outstanding DCs must be released. 	
        /// </remarks>	
        /// <param name="dirtyRect">A reference to a <see cref="RawRectangle"/> structure that identifies the dirty region of the surface.   A dirty region is any part of the surface that you have used for GDI rendering and that you want to preserve.  This is used as a performance hint to graphics subsystem in certain scenarios.  Do not use this parameter to restrict rendering to the specified rectangular region. The area specified by the <see cref="RawRectangle"/> will be used as a performance hint to indicate what areas have been manipulated by GDI rendering. </param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code. </returns>
        /// <unmanaged>HRESULT IDXGISurface1::ReleaseDC([In, Optional] RECT* pDirtyRect)</unmanaged>
        public void ReleaseDC(RawRectangle dirtyRect)
        {
            ReleaseDC_(dirtyRect);
        }
    }
}