// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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

using System.Runtime.InteropServices;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>	
    /// Describes a swap chain. This class is the intersection of the fields found in <see cref="DXGI.SwapChain"/> and <see cref="DXGI.SwapChain1"/>.
    /// </summary>	
    /// <msdn-id>bb173075</msdn-id>	
    /// <unmanaged>DXGI_SWAP_CHAIN_DESC</unmanaged>	
    /// <unmanaged-short>DXGI_SWAP_CHAIN_DESC</unmanaged-short>	
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public partial struct PresentationParameters
    {
        /// <summary>	
        /// A value that describes the resolution width. 
        /// </summary>	
        /// <msdn-id>bb173075</msdn-id>	
        /// <unmanaged>DXGI_MODE_DESC BufferDesc</unmanaged>	
        /// <unmanaged-short>DXGI_MODE_DESC BufferDesc</unmanaged-short>	
        public int Width;

        /// <summary>	
        /// A value that describes the resolution height. 
        /// </summary>	
        /// <msdn-id>bb173075</msdn-id>	
        /// <unmanaged>DXGI_MODE_DESC BufferDesc</unmanaged>	
        /// <unmanaged-short>DXGI_MODE_DESC BufferDesc</unmanaged-short>	
        public int Height;

        /// <summary>	
        /// A <strong><see cref="SharpDX.DXGI.Format"/></strong> structure describing the display format.
        /// </summary>	
        /// <msdn-id>bb173075</msdn-id>	
        /// <unmanaged>DXGI_MODE_DESC BufferDesc</unmanaged>	
        /// <unmanaged-short>DXGI_MODE_DESC BufferDesc</unmanaged-short>	
        public PixelFormat Format;

        /// <summary>	
        /// <dd> <p>A <strong><see cref="SharpDX.DXGI.Rational"/></strong> structure describing the refresh rate in hertz</p> </dd>	
        /// </summary>	
        /// <msdn-id>bb173064</msdn-id>	
        /// <unmanaged>DXGI_RATIONAL RefreshRate</unmanaged>	
        /// <unmanaged-short>DXGI_RATIONAL RefreshRate</unmanaged-short>	
        public SharpDX.DXGI.Rational RefreshRate;

        /// <summary>	
        /// <dd> <p>A member of the DXGI_USAGE enumerated type that describes the surface usage and CPU access options for the back buffer. The back buffer can  be used for shader input or render-target output.</p> </dd>	
        /// </summary>	
        /// <msdn-id>bb173075</msdn-id>	
        /// <unmanaged>DXGI_USAGE_ENUM BufferUsage</unmanaged>	
        /// <unmanaged-short>DXGI_USAGE_ENUM BufferUsage</unmanaged-short>	
        public SharpDX.DXGI.Usage Usage;

        /// <summary>	
        /// <dd> <p>A member of the <strong><see cref="SharpDX.DXGI.SwapChainFlags"/></strong> enumerated type that describes options for swap-chain behavior.</p> </dd>	
        /// </summary>	
        /// <msdn-id>bb173075</msdn-id>	
        /// <unmanaged>DXGI_SWAP_CHAIN_FLAG Flags</unmanaged>	
        /// <unmanaged-short>DXGI_SWAP_CHAIN_FLAG Flags</unmanaged-short>	
        public SharpDX.DXGI.SwapChainFlags Flags;

        /// <summary>
        /// Performs an explicit conversion from <see cref="DXGI.SwapChainDescription"/> to <see cref="PresentationParameters"/>.
        /// </summary>
        /// <param name="description">The swapchain description.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator PresentationParameters(DXGI.SwapChainDescription description)
        {
            return new PresentationParameters()
            {
                Width = description.ModeDescription.Width,
                Height = description.ModeDescription.Height,
                Format = description.ModeDescription.Format,
                RefreshRate = description.ModeDescription.RefreshRate,
                Usage = description.Usage,
                Flags = description.Flags
            };
        }
    }
}