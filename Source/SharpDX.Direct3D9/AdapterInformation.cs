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
    /// <summary>
    /// Adapter information.
    /// </summary>
    public class AdapterInformation
    {
        private readonly Direct3D direct3d;

        internal AdapterInformation(Direct3D direct3D, int adapter)
        {
            this.direct3d = direct3D;
            Adapter = adapter;
            Details = direct3D.GetAdapterIdentifier(adapter, 0);
        }

        /// <summary>
        /// Gets the capabilities of this adapter.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The capabilities</returns>
        public Capabilities GetCaps(DeviceType type)
        {
            return direct3d.GetDeviceCaps(Adapter, type);
        }

        /// <summary>
        /// Gets the display modes supported by this adapter.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>The display modes supported by this adapter.</returns>
        public DisplayModeCollection GetDisplayModes(Format format)
        {
            return new DisplayModeCollection(direct3d, Adapter, format);
        }

        /// <summary>
        /// Gets the adapter ordinal.
        /// </summary>
        public int Adapter { get; private set; }

        /// <summary>
        /// Gets the current display mode.
        /// </summary>
        public DisplayMode CurrentDisplayMode
        {
            get
            {
                return direct3d.GetAdapterDisplayMode(Adapter);
            }
        }

        /// <summary>
        /// Gets the details.
        /// </summary>
        public AdapterDetails Details { get; private set; }

        /// <summary>
        /// Gets the monitor.
        /// </summary>
        public IntPtr Monitor
        {
            get
            {
                return direct3d.GetAdapterMonitor(Adapter);
            }
        }
    }
}