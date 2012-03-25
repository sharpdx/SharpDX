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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.DXGI;
using MiniCube;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MiniCubeXaml
{
    /// <summary>
    /// CubeRenderer to a XAML panel.
    /// </summary>
    /// <remarks>
    /// This class only overrides the swap chain description and creation methods.
    /// </remarks>
    public class CubeRendererXaml : CubeRenderer
    {
        protected override SwapChainDescription1 CreateSwapChainDescription()
        {
            // Get the default descirption.
            var desc = base.CreateSwapChainDescription();

            // Can not use 0 to get the default on Composition SwapChain
            desc.Width = (int)(window.Bounds.Width * dpi / 96.0);    
            desc.Height = (int)(window.Bounds.Height * dpi / 96.0);

            // Required to be STRETCH for Composition 
            desc.Scaling = Scaling.Stretch; 

            return desc;
        }

        protected override SharpDX.DXGI.SwapChain1 CreateSwapChain(SharpDX.DXGI.Factory2 factory, SharpDX.Direct3D11.Device1 device, SharpDX.DXGI.SwapChainDescription1 desc)
        {
            // Creates the swap chain for XAML composition
            var swapChain = factory.CreateSwapChainForComposition(device, ref desc, null);

            // Associate the SwapChainBackgroundPanel with the swap chain
            using (var panelNative = ComObject.As<ISwapChainBackgroundPanelNative>(Window.Current.Content as SwapChainBackgroundPanel))
                panelNative.SwapChain = swapChain;

            // Returns the new swap chain
            return swapChain;
        }
    }
}
