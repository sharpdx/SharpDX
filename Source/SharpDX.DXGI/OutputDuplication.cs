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
namespace SharpDX.DXGI
{
    public partial class OutputDuplication
    {
        /// <unmanaged>HRESULT IDXGIOutputDuplication::MapDesktopSurface([Out] DXGI_MAPPED_RECT* pLockedRect)</unmanaged>	
#if DESKTOP_APP
        public DataRectangle MapDesktopSurface()
        {
            MappedRectangle mappedRect;
            MapDesktopSurface(out mappedRect);
            return new DataRectangle(mappedRect.PBits, mappedRect.Pitch);
        }

        /// <summary>
        /// <p>Indicates that the application is ready to process the next desktop image.</p>	
        /// </summary>
        /// <param name="timeoutInMilliseconds"><dd> <p>The time-out interval, in milliseconds. This interval specifies the amount of time that this method waits for a new frame before it returns to the caller.  This method returns if the interval elapses, and a new desktop image is not available.</p> <p>For more information about the time-out interval, see Remarks.</p> </dd></param>	
        /// <param name="frameInfoRef"><dd> <p>A reference to a memory location that receives the <strong><see cref="SharpDX.DXGI.OutputDuplicateFrameInformation"/></strong> structure that describes timing and presentation statistics for a frame.</p> </dd></param>	
        /// <param name="desktopResourceOut"><dd> <p>A reference to a variable that receives the <strong><see cref="SharpDX.DXGI.Resource"/></strong> interface of the surface that contains the desktop bitmap.</p> </dd></param>	
        /// <remarks>
        /// <p>When <strong>AcquireNextFrame</strong> returns successfully, the calling application can access the desktop image that <strong>AcquireNextFrame</strong> returns in the variable at <em>ppDesktopResource</em>.	
        /// If the caller specifies a zero time-out interval in the <em>TimeoutInMilliseconds</em> parameter, <strong>AcquireNextFrame</strong> verifies whether there is a new desktop image available, returns immediately, and indicates its outcome with the return value.  If the caller specifies an <strong>INFINITE</strong> time-out interval in the <em>TimeoutInMilliseconds</em> parameter, the time-out interval never elapses.</p><strong>Note</strong>??You cannot cancel the wait that you specified in the <em>TimeoutInMilliseconds</em> parameter. Therefore, if you must periodically check for other conditions (for example, a terminate signal), you should specify a non-<strong>INFINITE</strong> time-out interval. After the time-out interval elapses, you can check for these other conditions and then call <strong>AcquireNextFrame</strong> again to wait for the next frame.?<p><strong>AcquireNextFrame</strong> acquires a new desktop frame when the operating system either updates the desktop bitmap image or changes the shape or position of a hardware reference.  The new frame that <strong>AcquireNextFrame</strong> acquires might have only the desktop image updated, only the reference shape or position updated, or both.</p>	
        /// </remarks>
        /// <include file='.\..\..\Documentation\CodeComments.xml' path="/comments/comment[@id='IDXGIOutputDuplication::AcquireNextFrame']/*"/>	
        /// <msdn-id>hh404615</msdn-id>
        /// <unmanaged>HRESULT IDXGIOutputDuplication::AcquireNextFrame([In] unsigned int TimeoutInMilliseconds,[Out] DXGI_OUTDUPL_FRAME_INFO* pFrameInfo,[Out] IDXGIResource** ppDesktopResource)</unmanaged>	
        /// <unmanaged-short>IDXGIOutputDuplication::AcquireNextFrame</unmanaged-short>	
        public void AcquireNextFrame(int timeoutInMilliseconds, out SharpDX.DXGI.OutputDuplicateFrameInformation frameInfoRef, out SharpDX.DXGI.Resource desktopResourceOut)
        {
            var result = this.TryAcquireNextFrame(timeoutInMilliseconds, out frameInfoRef, out desktopResourceOut);
            result.CheckError();
        }
#endif
    }
}