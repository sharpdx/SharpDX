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

namespace SharpDX.Direct3D9
{
    public partial struct PresentParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PresentParameters"/> struct.
        /// </summary>
        /// <param name="backBufferWidth">Width of the back buffer.</param>
        /// <param name="backBufferHeight">Height of the back buffer.</param>
        public PresentParameters(int backBufferWidth, int backBufferHeight) : this(backBufferWidth, backBufferHeight, Format.X8R8G8B8, 1, MultisampleType.None, 0, Direct3D9.SwapEffect.Discard, IntPtr.Zero, true, true, Format.D24X8, Direct3D9.PresentFlags.None, 0, PresentInterval.Default|PresentInterval.Immediate)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PresentParameters"/> struct.
        /// </summary>
        /// <param name="backBufferWidth">Width of the back buffer.</param>
        /// <param name="backBufferHeight">Height of the back buffer.</param>
        /// <param name="backBufferFormat">The back buffer format.</param>
        /// <param name="backBufferCount">The back buffer count.</param>
        /// <param name="multiSampleType">Type of the multi sample.</param>
        /// <param name="multiSampleQuality">The multi sample quality.</param>
        /// <param name="swapEffect">The swap effect.</param>
        /// <param name="deviceWindowHandle">The device window handle.</param>
        /// <param name="windowed">if set to <c>true</c> [windowed].</param>
        /// <param name="enableAutoDepthStencil">if set to <c>true</c> [enable auto depth stencil].</param>
        /// <param name="autoDepthStencilFormat">The auto depth stencil format.</param>
        /// <param name="presentFlags">The present flags.</param>
        /// <param name="fullScreenRefreshRateInHz">The full screen refresh rate in Hz.</param>
        /// <param name="presentationInterval">The presentation interval.</param>
        public PresentParameters(int backBufferWidth, int backBufferHeight, Format backBufferFormat, int backBufferCount, MultisampleType multiSampleType, int multiSampleQuality, SwapEffect swapEffect, IntPtr deviceWindowHandle, bool windowed, bool enableAutoDepthStencil, Format autoDepthStencilFormat, PresentFlags presentFlags, int fullScreenRefreshRateInHz, PresentInterval presentationInterval)
        {
            BackBufferWidth = backBufferWidth;
            BackBufferHeight = backBufferHeight;
            BackBufferFormat = backBufferFormat;
            BackBufferCount = backBufferCount;
            MultiSampleType = multiSampleType;
            MultiSampleQuality = multiSampleQuality;
            SwapEffect = swapEffect;
            DeviceWindowHandle = deviceWindowHandle;
            Windowed = windowed;
            EnableAutoDepthStencil = enableAutoDepthStencil;
            AutoDepthStencilFormat = autoDepthStencilFormat;
            PresentFlags = presentFlags;
            FullScreenRefreshRateInHz = fullScreenRefreshRateInHz;
            PresentationInterval = presentationInterval;
        }

        /// <summary>
        /// Init this structure to defaults
        /// </summary>
        public void InitDefaults()
        {
            this.BackBufferWidth = 800;
            this.BackBufferHeight = 600;
            this.BackBufferFormat = Format.X8R8G8B8;
            this.BackBufferCount = 1;
            this.MultiSampleType = MultisampleType.None;
            this.SwapEffect = SwapEffect.Discard;
            this.DeviceWindowHandle = IntPtr.Zero;
            this.Windowed = true;
            this.EnableAutoDepthStencil = true;
            this.AutoDepthStencilFormat = Format.D24X8;
            this.PresentFlags = PresentFlags.None;
            this.PresentationInterval = PresentInterval.Default | PresentInterval.Immediate;            
        }
    }
}
