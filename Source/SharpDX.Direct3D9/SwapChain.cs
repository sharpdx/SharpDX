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

namespace SharpDX.Direct3D9
{
    public partial class SwapChain
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SwapChain"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="presentParameters">The present parameters.</param>
        /// <unmanaged>HRESULT IDirect3DDevice9::CreateAdditionalSwapChain([In] D3DPRESENT_PARAMETERS* pPresentationParameters,[In] IDirect3DSwapChain9** pSwapChain)</unmanaged>
        public SwapChain(Device device, PresentParameters presentParameters)
        {
            device.CreateAdditionalSwapChain(ref presentParameters, this);
        }

        /// <summary>
        /// Retrieves a back buffer from the swap chain of the device.
        /// </summary>
        /// <param name="iBackBuffer">The i back buffer.</param>
        /// <returns>The back buffer from the swap chain of the device.</returns>
        /// <unmanaged>HRESULT IDirect3DSwapChain9::GetBackBuffer([In] unsigned int iBackBuffer,[In] D3DBACKBUFFER_TYPE Type,[Out] IDirect3DSurface9** ppBackBuffer)</unmanaged>
        public SharpDX.Direct3D9.Surface GetBackBuffer(int iBackBuffer)
        {
            return GetBackBuffer(iBackBuffer, BackBufferType.Mono);
        }

        /// <summary>
        /// Presents the contents of the next buffer in the sequence of back buffers to the screen.
        /// </summary>
        /// <param name="presentFlags">The present flags.</param>
        /// <unmanaged>HRESULT IDirect3DSwapChain9::Present([In, Optional] const void* pSourceRect,[InOut, Optional] const void* pDestRect,[In] HWND hDestWindowOverride,[In] const RGNDATA* pDirtyRegion,[In] unsigned int dwFlags)</unmanaged>
        public void Present(Present presentFlags)
        {
            Present(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, (int)presentFlags);
        }

        /// <summary>
        /// Presents the contents of the next buffer in the sequence of back buffers to the screen.
        /// </summary>
        /// <param name="presentFlags">The present flags.</param>
        /// <param name="sourceRectangle">The area of the back buffer that should be presented.</param>
        /// <param name="destinationRectangle">The area of the front buffer that should receive the result of the presentation.</param>
        /// <unmanaged>HRESULT IDirect3DSwapChain9::Present([In, Optional] const void* pSourceRect,[InOut, Optional] const void* pDestRect,[In] HWND hDestWindowOverride,[In] const RGNDATA* pDirtyRegion,[In] unsigned int dwFlags)</unmanaged>
        public void Present(Present presentFlags, RawRectangle sourceRectangle, RawRectangle destinationRectangle)
        {
            Present(presentFlags, sourceRectangle, destinationRectangle, IntPtr.Zero);
        }

        /// <summary>
        /// Presents the contents of the next buffer in the sequence of back buffers to the screen.
        /// </summary>
        /// <param name="presentFlags">The present flags.</param>
        /// <param name="sourceRectangle">The area of the back buffer that should be presented.</param>
        /// <param name="destinationRectangle">The area of the front buffer that should receive the result of the presentation.</param>
        /// <param name="windowOverride">The destination window whose client area is taken as the target for this presentation.</param>
        /// <unmanaged>HRESULT IDirect3DSwapChain9::Present([In, Optional] const void* pSourceRect,[InOut, Optional] const void* pDestRect,[In] HWND hDestWindowOverride,[In] const RGNDATA* pDirtyRegion,[In] unsigned int dwFlags)</unmanaged>
        public void Present(Present presentFlags, RawRectangle sourceRectangle, RawRectangle destinationRectangle, IntPtr windowOverride)
        {
            unsafe
            {
                var srcPtr = IntPtr.Zero;
                if (!sourceRectangle.IsEmpty)
                    srcPtr = new IntPtr(&sourceRectangle);

                var destPtr = IntPtr.Zero;
                if (!destinationRectangle.IsEmpty)
                    destPtr = new IntPtr(&destinationRectangle);

                Present(srcPtr, destPtr, windowOverride, IntPtr.Zero, (int)presentFlags);
            }
        }


        /// <summary>
        /// Presents the contents of the next buffer in the sequence of back buffers to the screen.
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <param name="sourceRectangle">The area of the back buffer that should be presented.</param>
        /// <param name="destinationRectangle">The area of the front buffer that should receive the result of the presentation.</param>
        /// <param name="windowOverride">The destination window whose client area is taken as the target for this presentation.</param>
        /// <param name="dirtyRegionRGNData">Specifies a region on the back buffer that contains the minimal amount of pixels that need to be updated.</param>
        /// <unmanaged>HRESULT IDirect3DSwapChain9::Present([In, Optional] const void* pSourceRect,[InOut, Optional] const void* pDestRect,[In] HWND hDestWindowOverride,[In] const RGNDATA* pDirtyRegion,[In] unsigned int dwFlags)</unmanaged>
        public void Present(Present flags, RawRectangle sourceRectangle, RawRectangle destinationRectangle, IntPtr windowOverride, IntPtr dirtyRegionRGNData)
        {
            unsafe
            {
                var srcPtr = IntPtr.Zero;
                if (!sourceRectangle.IsEmpty)
                    srcPtr = new IntPtr(&sourceRectangle);

                var destPtr = IntPtr.Zero;
                if (!destinationRectangle.IsEmpty)
                    destPtr = new IntPtr(&destinationRectangle);

                Present(srcPtr, destPtr, windowOverride, dirtyRegionRGNData, (int)flags);
            }
        }
    }
}