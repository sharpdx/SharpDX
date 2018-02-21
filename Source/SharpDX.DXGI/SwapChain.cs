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
    public partial class SwapChain
    {
        /// <summary>	
        /// Creates a swap chain.	
        /// </summary>	
        /// <remarks>	
        /// If you attempt to create a swap chain in full-screen mode, and full-screen mode is unavailable, the swap chain will be created in windowed mode and DXGI_STATUS_OCCLUDED will be returned. If the buffer width or the buffer height are zero, the sizes will be inferred from the output window size in the swap-chain description. Since the target output cannot be chosen explicitly when the swap-chain is created, you should not create a full-screen swap chain. This can reduce presentation performance if the swap chain size and the output window size do not match. Here are two ways to ensure the sizes match:  Create a windowed swap chain and then set it full-screen using <see cref="SharpDX.DXGI.SwapChain.SetFullscreenState"/>. Save a reference to the swap-chain immediately after creation, and use it to get the output window size during a WM_SIZE event. Then resize the swap chain buffers (with <see cref="SharpDX.DXGI.SwapChain.ResizeBuffers"/>) during the transition from windowed to full-screen.  If the swap chain is in full-screen mode, before you release it, you must use {{SetFullscreenState}} to switch it to windowed mode. For more information about releasing a swap chain, see the Destroying a Swap Chain section of {{DXGI Overview}}. 	
        /// </remarks>	
        /// <param name="factory">a reference to a <see cref="Factory"/>.</param>
        /// <param name="device">A reference to the device that will write 2D images to the swap chain. </param>
        /// <param name="description">A reference to the swap-chain description (see <see cref="SharpDX.DXGI.SwapChainDescription"/>).</param>
        /// <unmanaged>HRESULT IDXGIFactory::CreateSwapChain([In] IUnknown* pDevice,[In] DXGI_SWAP_CHAIN_DESC* pDesc,[Out] IDXGISwapChain** ppSwapChain)</unmanaged>
        /// <msdn-id>bb174537</msdn-id>	
        /// <unmanaged>HRESULT IDXGIFactory::CreateSwapChain([In] IUnknown* pDevice,[In] DXGI_SWAP_CHAIN_DESC* pDesc,[Out, Fast] IDXGISwapChain** ppSwapChain)</unmanaged>	
        /// <unmanaged-short>IDXGIFactory::CreateSwapChain</unmanaged-short>	
        public SwapChain(Factory factory, ComObject device, SwapChainDescription description)
            : base(IntPtr.Zero)
        {
            factory.CreateSwapChain(device, ref description, this);
        }

        /// <summary>
        /// Access one of the swap-chain back buffers.
        /// </summary>
        /// <typeparam name="T">The interface of the surface to resolve from the back buffer</typeparam>
        /// <param name="index">A zero-based buffer index. If the swap effect is not DXGI_SWAP_EFFECT_SEQUENTIAL, this method only has access to the first buffer; for this case, set the index to zero.</param>
        /// <returns>
        /// Returns a reference to a back-buffer interface.
        /// </returns>
        /// <msdn-id>bb174570</msdn-id>	
        /// <unmanaged>HRESULT IDXGISwapChain::GetBuffer([In] unsigned int Buffer,[In] const GUID&amp; riid,[Out] void** ppSurface)</unmanaged>	
        /// <unmanaged-short>IDXGISwapChain::GetBuffer</unmanaged-short>	
        public T GetBackBuffer<T>(int index) where T : ComObject
        {
            IntPtr temp;
            GetBuffer(index, Utilities.GetGuidFromType(typeof (T)), out temp);
            return FromPointer<T>(temp);
        }

        /// <summary>	
        /// <p>Gets performance statistics about the last render frame.</p>	
        /// </summary>	
        /// <remarks>	
        /// <p>You cannot use <strong>GetFrameStatistics</strong> for swap chains that both use the bit-block transfer (bitblt) presentation model and draw in windowed mode.</p><p>You can only use <strong>GetFrameStatistics</strong> for swap chains that either use the flip presentation model or draw in full-screen mode. You set the <strong><see cref="SharpDX.DXGI.SwapEffect.FlipSequential"/></strong> value in the <strong>SwapEffect</strong> member of the <strong><see cref="SharpDX.DXGI.SwapChainDescription1"/></strong> structure to specify that the swap chain uses the flip presentation model.</p>	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IDXGISwapChain::GetFrameStatistics']/*"/>	
        /// <msdn-id>bb174573</msdn-id>	
        /// <unmanaged>GetFrameStatistics</unmanaged>	
        /// <unmanaged-short>GetFrameStatistics</unmanaged-short>	
        /// <unmanaged>HRESULT IDXGISwapChain::GetFrameStatistics([Out] DXGI_FRAME_STATISTICS* pStats)</unmanaged>
        public SharpDX.DXGI.FrameStatistics FrameStatistics
        {
            get
            {
                SharpDX.DXGI.FrameStatistics output;
                SharpDX.Result result = TryGetFrameStatistics(out output);
                result.CheckError();
                return output;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the swapchain is in fullscreen.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this swapchain is in fullscreen; otherwise, <c>false</c>.
        /// </value>
        /// <msdn-id>bb174574</msdn-id>	
        /// <unmanaged>HRESULT IDXGISwapChain::GetFullscreenState([Out] BOOL* pFullscreen,[Out] IDXGIOutput** ppTarget)</unmanaged>	
        /// <unmanaged-short>IDXGISwapChain::GetFullscreenState</unmanaged-short>	
        public bool IsFullScreen
        {
            get
            {
                RawBool isFullScreen;
                Output output;
                GetFullscreenState(out isFullScreen, out output);
                if (output != null)
                    output.Dispose();
                return isFullScreen;
            }

            set
            {
                SetFullscreenState(value, null);
            }
        }

        /// <summary>	
        /// <p>[Starting with Direct3D 11.1, we recommend not to use <strong>Present</strong> anymore to present a rendered image. Instead, use <strong><see cref="SharpDX.DXGI.SwapChain1.Present1"/></strong>. For more info, see Remarks.]</p><p>Presents a rendered image to the user.</p>	
        /// </summary>	
        /// <param name="syncInterval">No documentation.</param>	
        /// <param name="flags">No documentation.</param>	
        /// <returns><p>Possible return values include: <see cref="SharpDX.Result.Ok"/>, <see cref="SharpDX.DXGI.ResultCode.DeviceReset"/> or <see cref="SharpDX.DXGI.ResultCode.DeviceRemoved"/> (see DXGI_ERROR), <see cref="SharpDX.DXGI.DXGIStatus.Occluded"/> (see <see cref="SharpDX.DXGI.DXGIStatus"/>), or D3DDDIERR_DEVICEREMOVED.  </p><p><strong>Note</strong>??The <strong>Present</strong> method can return either <see cref="SharpDX.DXGI.ResultCode.DeviceRemoved"/> or D3DDDIERR_DEVICEREMOVED if a video card has been physically removed from the computer, or a driver upgrade for the video card has occurred.</p></returns>	
        /// <remarks>	
        /// <p>Starting with Direct3D 11.1, we recommend to instead use <strong><see cref="SharpDX.DXGI.SwapChain1.Present1"/></strong> because you can then use dirty rectangles and the scroll rectangle in the swap chain presentation and as such use less memory bandwidth and as a result less system power. For more info about using dirty rectangles and the scroll rectangle in swap chain presentation, see Using dirty rectangles and the scroll rectangle in swap chain presentation.</p><p>For the best performance when flipping swap-chain buffers in a full-screen application, see Full-Screen Application Performance Hints.</p><p>Because calling <strong>Present</strong> might cause the render thread to wait on the message-pump thread, be careful when calling this method in an application that uses multiple threads. For more details, see Multithreading Considerations.</p><table> <tr><td> <p>Differences between Direct3D 9 and Direct3D 10:</p> <p>Specifying <strong><see cref="SharpDX.DXGI.PresentFlags.Test"/></strong> in the <em>Flags</em> parameter is analogous to <strong>IDirect3DDevice9::TestCooperativeLevel</strong> in Direct3D 9.</p> </td></tr> </table><p>?</p><p>For flip presentation model swap chains that you create with the <strong><see cref="SharpDX.DXGI.SwapEffect.FlipSequential"/></strong> value set, a successful presentation unbinds back buffer 0 from the graphics pipeline, except for when you pass the <strong><see cref="SharpDX.DXGI.PresentFlags.DoNotSequence"/></strong> flag in the <em>Flags</em> parameter.</p><p>For info about how data values change when you present content to the screen, see Converting data for the color space.</p>	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IDXGISwapChain::Present']/*"/>	
        /// <msdn-id>bb174576</msdn-id>	
        /// <unmanaged>HRESULT IDXGISwapChain::Present([In] unsigned int SyncInterval,[In] DXGI_PRESENT_FLAGS Flags)</unmanaged>	
        /// <unmanaged-short>IDXGISwapChain::Present</unmanaged-short>	
        public SharpDX.Result Present(int syncInterval, SharpDX.DXGI.PresentFlags flags)
        {
            unsafe
            {
                SharpDX.Result result;
                result = TryPresent(syncInterval, flags);
                result.CheckError();
                return result;
            }
        }
    }
}