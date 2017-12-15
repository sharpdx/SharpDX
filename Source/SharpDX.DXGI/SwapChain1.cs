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
    public partial class SwapChain1
    {
        /// <summary>
        /// Creates a swapchain associated to the specified HWND. This is applicable only for Desktop platform.
        /// </summary>
        /// <param name="factory">The DXGI Factory used to create the swapchain.</param>
        /// <param name="device">The associated device instance.</param>
        /// <param name="hwnd">The HWND of the window to which this swapchain is associated.</param>
        /// <param name="description">The swap chain description.</param>
        /// <param name="fullScreenDescription">The fullscreen description of the swap chain. Default is null.</param>
        /// <param name="restrictToOutput">The output to which this swap chain should be restricted. Default is null, meaning that there is no restriction.</param>
        public SwapChain1(Factory2 factory, ComObject device, IntPtr hwnd, ref SwapChainDescription1 description, SwapChainFullScreenDescription? fullScreenDescription = null, Output restrictToOutput = null)
            : base(IntPtr.Zero)
        {
            factory.CreateSwapChainForHwnd(device, hwnd, ref description, fullScreenDescription, restrictToOutput, this);
        }

        /// <summary>
        /// Creates a swapchain associated to the specified CoreWindow. This is applicable only for WinRT platform.
        /// </summary>
        /// <param name="factory">The DXGI Factory used to create the swapchain.</param>
        /// <param name="device">The associated device instance.</param>
        /// <param name="coreWindow">The HWND of the window to which this swapchain is associated.</param>
        /// <param name="description">The swap chain description.</param>
        /// <param name="restrictToOutput">The output to which this swap chain should be restricted. Default is null, meaning that there is no restriction.</param>
        public SwapChain1(Factory2 factory, ComObject device, ComObject coreWindow, ref SwapChainDescription1 description, Output restrictToOutput = null)
            : base(IntPtr.Zero)
        {
            factory.CreateSwapChainForCoreWindow(device, coreWindow, ref description, restrictToOutput, this);
        }

        /// <summary>
        /// Creates a swapchain for DirectComposition API or WinRT XAML framework. This is applicable only for WinRT platform.
        /// </summary>
        /// <param name="factory">The DXGI Factory used to create the swapchain.</param>
        /// <param name="device">The associated device instance.</param>
        /// <param name="description">The swap chain description.</param>
        /// <param name="restrictToOutput">The output to which this swap chain should be restricted. Default is null, meaning that there is no restriction.</param>
        public SwapChain1(Factory2 factory, ComObject device, ref SwapChainDescription1 description, Output restrictToOutput = null)
            : base(IntPtr.Zero)
        {
            factory.CreateSwapChainForComposition(device, ref description, restrictToOutput, this);
        }

        /// <summary>	
        /// Presents a frame on the display screen, internally using the Present1 method.
        /// </summary>	
        /// <param name="syncInterval"><para>An integer that specifies how to synchronize presentation of a frame with the vertical blank.</para> <para>For the bit-block transfer (bitblt) model, values are:</para>  0 - The presentation occurs immediately, there is no synchronization. 1,2,3,4 - Synchronize presentation after the nth vertical blank.  <para>For the flip model, values are:</para>  0 - Discard this frame if you submitted a more recent presentation. n &gt; 0 - Synchronize presentation for at least n vertical blanks.  <para>For an example that shows how sync-interval values affect a flip presentation queue, see Remarks.</para> <para>If the update region straddles more than one output (each represented by <see cref="SharpDX.DXGI.Output1"/>), Present1 performs the synchronization to the output that contains the largest subrectangle of the target window's client area.</para></param>	
        /// <param name="presentFlags"><para>An integer value that contains swap-chain presentation options. These options are defined by the DXGI_PRESENT constants.</para></param>	
        /// <param name="presentParameters"><para>A reference to a <see cref="SharpDX.DXGI.PresentParameters"/> structure that describes updated rectangles and scroll information of the frame to present.</para></param>	
        /// <returns>Possible return values include: <see cref="SharpDX.Result.Ok"/>, <see cref="SharpDX.DXGI.ResultCode.DeviceRemoved"/> , <see cref="SharpDX.DXGI.DXGIStatus.Occluded"/>, <see cref="SharpDX.DXGI.ResultCode.InvalidCall"/>, or E_OUTOFMEMORY.</returns>	
        /// <remarks>
        /// An application can use Present1 to optimize presentation by specifying scroll and dirty rectangles. When the runtime has information about these rectangles, the runtime can then perform necessary bitblts during presentation more efficiently and pass this metadata to the Desktop Window Manager (DWM). The DWM can then use the metadata to optimize presentation and pass the metadata to indirect displays and terminal servers to optimize traffic over the wire. An application must confine its modifications to only the dirty regions that it passes to Present1, as well as modify the entire dirty region to avoid undefined resource contents from being exposed.For flip presentation model swap chains that you create with the <see cref="SharpDX.DXGI.SwapEffect.FlipSequential"/> value set, a successful presentation results in an unbind of back buffer 0 from the graphics pipeline, except for when you pass the <see cref="SharpDX.DXGI.PresentFlags.DoNotSequence"/> flag in the Flags parameter.Flip presentation model queueSuppose the following frames with sync-interval values are queued from oldest (A) to newest (E) before you call Present1.A: 3, B: 0, C: 0, D: 1, E: 0When you call Present1, the runtime shows frame A for 3 vertical blank intervals, then frame D for 1 vertical blank interval, and then frame E until you submit a new presentation. The runtime discards frames C and D.	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IDXGISwapChain1::Present1']/*"/>	
        /// <unmanaged>HRESULT IDXGISwapChain1::Present1([In] unsigned int SyncInterval,[In] unsigned int PresentFlags,[In] const void* pPresentParameters)</unmanaged>	
        public unsafe Result Present(int syncInterval, PresentFlags presentFlags, PresentParameters presentParameters)
        {
            bool hasScrollRectangle = presentParameters.ScrollRectangle.HasValue;
            bool hasScrollOffset = presentParameters.ScrollOffset.HasValue;

            var scrollRectangle = hasScrollRectangle ? presentParameters.ScrollRectangle.Value : new RawRectangle();
            var scrollOffset = hasScrollOffset ? presentParameters.ScrollOffset.Value : default(RawPoint);

            fixed (void* pDirtyRects = presentParameters.DirtyRectangles)
            {
                var native = default(PresentParameters.__Native);
                native.DirtyRectsCount = presentParameters.DirtyRectangles != null ? presentParameters.DirtyRectangles.Length : 0;
                native.PDirtyRects = (IntPtr)pDirtyRects;
                native.PScrollRect = hasScrollRectangle ? new IntPtr(&scrollRectangle) : IntPtr.Zero;
                native.PScrollOffset = hasScrollOffset ? new IntPtr(&scrollOffset) : IntPtr.Zero;

                return Present1(syncInterval, presentFlags, new IntPtr(&native));
            }
        }
    }
}
