// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Service providing method to access GraphicsDevice life-cycle.
    /// </summary>
    public interface IGraphicsDeviceService
    {
        /// <summary>
        /// Occurs when a device is created.
        /// </summary>
        event EventHandler<EventArgs> DeviceCreated;

        /// <summary>
        /// Occurs when a device is disposing.
        /// </summary>
        event EventHandler<EventArgs> DeviceDisposing;

        /// <summary>
        /// Occurs when a device is lost.
        /// </summary>
        event EventHandler<EventArgs> DeviceLost;

        /// <summary>
        /// Occurs right before device is about to change (recreate or resize)
        /// </summary>
        event EventHandler<EventArgs> DeviceChangeBegin;

        /// <summary>
        /// Occurs when device is changed (recreated or resized)
        /// </summary>
        event EventHandler<EventArgs> DeviceChangeEnd;
            
        /// <summary>
        /// Gets the current graphics device.
        /// </summary>
        /// <value>The graphics device.</value>
        GraphicsDevice GraphicsDevice { get; }
    }
}