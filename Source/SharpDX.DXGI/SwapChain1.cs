// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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
#if WIN8
using System;

namespace SharpDX.DXGI
{
    public partial class SwapChain1
    {
        /// <unmanaged>HRESULT IDXGIFactory2::CreateSwapChainForComposition([In] IUnknown* pDevice,[In] const DXGI_SWAP_CHAIN_DESC1* pDesc,[In, Optional] IDXGIOutput* pRestrictToOutput,[Out] IDXGISwapChain1** ppSwapChain)</unmanaged>	
        public SwapChain1(Factory2 factory, ComObject device, SwapChainDescription1 description, Output restrictToOutput = null)
            : base(IntPtr.Zero)
        {
            factory.CreateSwapChainForComposition(device, ref description, restrictToOutput, this);
        }

        /// <unmanaged>HRESULT IDXGIFactory2::CreateSwapChainForCoreWindow([In] IUnknown* pDevice,[In] IUnknown* pWindow,[In] const DXGI_SWAP_CHAIN_DESC1* pDesc,[In, Optional] IDXGIOutput* pRestrictToOutput,[Out, Fast] IDXGISwapChain1** ppSwapChain)</unmanaged>	
        public SwapChain1(Factory2 factory, ComObject device, ComObject coreWindow, SwapChainDescription1 description, Output restrictToOutput = null)
            : base(IntPtr.Zero)
        {
            factory.CreateSwapChainForCoreWindow(device, coreWindow, ref description, restrictToOutput, this);
        }

        /// <unmanaged>HRESULT IDXGIFactory2::CreateSwapChainForHwnd([In] IUnknown* pDevice,[In] HWND hWnd,[In] const DXGI_SWAP_CHAIN_DESC1* pDesc,[In, Optional] const DXGI_SWAP_CHAIN_FULLSCREEN_DESC* pFullscreenDesc,[In, Optional] IDXGIOutput* pRestrictToOutput,[Out, Fast] IDXGISwapChain1** ppSwapChain)</unmanaged>	
        public SwapChain1(Factory2 factory, ComObject device, IntPtr hwndHandle, SwapChainDescription1 description, SwapChainFullscreenDescription? fullscreenDescription = null, Output restrictToOutput = null)
            : base(IntPtr.Zero)
        {
            factory.CreateSwapChainForHwnd(device, hwndHandle, ref description, fullscreenDescription, restrictToOutput, this);
        }
   }
}
#endif