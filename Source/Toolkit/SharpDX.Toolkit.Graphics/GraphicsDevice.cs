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
using System;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// This class is a frontend to <see cref="SharpDX.Direct3D11.Device"/>.
    /// </summary>
    public class GraphicsDevice : Component
    {
        internal Device Device;

        [ThreadStatic]
        private static GraphicsDevice current;

        public GraphicsDevice(DriverType type = DriverType.Hardware, DeviceCreationFlags flags = DeviceCreationFlags.None, params FeatureLevel[] featureLevels)
        {
            Device = ToDispose(featureLevels.Length > 0 ? new Device(type, flags, featureLevels) : new Device(type, flags));
            IsDebugMode = (flags & DeviceCreationFlags.Debug) != 0;
            ImmediateContext = new GraphicsDeviceContext(Device.ImmediateContext);
            FeatureLevel = Device.FeatureLevel;
            ImmediateContext.AttachToCurrentThread();
            AttachToCurrentThread();
        }

        public GraphicsDevice(Device device)
        {
            Device = ToDispose(device);
            IsDebugMode = (device.CreationFlags & (int)DeviceCreationFlags.Debug) != 0;
            ImmediateContext = new GraphicsDeviceContext(Device.ImmediateContext);
            FeatureLevel = Device.FeatureLevel;
            ImmediateContext.AttachToCurrentThread();
            AttachToCurrentThread();
        }

        /// <summary>
        /// Gets the <see cref="GraphicsDeviceContext"/> for immediate rendering.
        /// </summary>
        public readonly GraphicsDeviceContext ImmediateContext;

        /// <summary>
        /// Gets the <see cref="FeatureLevel"/> for this device.
        /// </summary>
        public readonly FeatureLevel FeatureLevel;

        /// <summary>
        /// Gets whether this <see cref="GraphicsDevice"/> is running in debug.
        /// </summary>
        public readonly bool IsDebugMode;

        /// <summary>
        /// Gets the <see cref="GraphicsDevice"/> attached to the current thread.
        /// </summary>
        public static GraphicsDevice Current
        {
            get { return current; }
        }

        /// <summary>
        /// Attach this <see cref="GraphicsDevice"/> to the current thread.
        /// </summary>
        public void AttachToCurrentThread()
        {
            current = this;
        }

        /// <summary>
        /// Creates a new deferred <see cref="GraphicsDeviceContext"/>.
        /// </summary>
        /// <returns>A deferred <see cref="GraphicsDeviceContext"/></returns>
        public GraphicsDeviceContext NewDeferredContext()
        {
            return new GraphicsDeviceContext(new DeviceContext(Device));
        }

        public static implicit operator Device(GraphicsDevice from)
        {
            return from.Device;
        }
    }
}
