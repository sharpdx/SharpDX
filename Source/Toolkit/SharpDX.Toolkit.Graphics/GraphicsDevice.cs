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
using System.IO;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// This class is a frontend to <see cref="SharpDX.Direct3D11.Device"/> and <see cref="SharpDX.Direct3D11.DeviceContext"/>
    /// </summary>
    public class GraphicsDevice : Component
    {
        internal Device Device;
        internal DeviceContext Context;
        private byte[] copyTempBuffer;

        [ThreadStatic]
        private static GraphicsDevice current;

        internal readonly StageStatus CurrentStage;
        internal struct StageStatus
        {
            internal VertexShader VertexShader;
            internal DomainShader DomainShader;
            internal HullShader HullShader;
            internal GeometryShader GeometryShader;
            internal PixelShader PixelShader;
            internal ComputeShader ComputeShader;
        };

        protected GraphicsDevice(DriverType type = DriverType.Hardware, DeviceCreationFlags flags = DeviceCreationFlags.None, params FeatureLevel[] featureLevels)
        {
            Device = ToDispose(featureLevels.Length > 0 ? new Device(type, flags, featureLevels) : new Device(type, flags));
            IsDebugMode = (Device.CreationFlags & (int)DeviceCreationFlags.Debug) != 0;
            MainDevice = this;
            Context = Device.ImmediateContext;
            FeatureLevel = Device.FeatureLevel;
            AttachToCurrentThread();
        }

        protected GraphicsDevice(Device device)
        {
            Device = ToDispose(device);
            IsDebugMode = (Device.CreationFlags & (int)DeviceCreationFlags.Debug) != 0;
            MainDevice = this;
            Context = Device.ImmediateContext;
            FeatureLevel = Device.FeatureLevel;
            AttachToCurrentThread();
        }

        protected GraphicsDevice(GraphicsDevice mainDevice, DeviceContext deferredContext)
        {
            Device = mainDevice.Device;
            IsDebugMode = (Device.CreationFlags & (int)DeviceCreationFlags.Debug) != 0;
            MainDevice = mainDevice;
            Context = deferredContext;
            FeatureLevel = Device.FeatureLevel;
        }

        public static GraphicsDevice New(Device device)
        {
            return new GraphicsDevice(device);
        }

        public static GraphicsDevice New(DriverType type = DriverType.Hardware, DeviceCreationFlags flags = DeviceCreationFlags.None, params FeatureLevel[] featureLevels)
        {
            return new GraphicsDevice(type, flags, featureLevels);
        }

        /// <summary>
        /// Creates a new deferred <see cref="GraphicsDeviceContext"/>.
        /// </summary>
        /// <returns>A deferred <see cref="GraphicsDeviceContext"/></returns>
        public GraphicsDevice NewDeferred()
        {
            return new GraphicsDevice(this, new DeviceContext(Device));
        }

        /// <summary>
        /// Gets the <see cref="GraphicsDeviceContext"/> for immediate rendering.
        /// </summary>
        public readonly GraphicsDevice MainDevice;

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

        public static implicit operator Device(GraphicsDevice from)
        {
            return from.Device;
        }

        public static implicit operator DeviceContext(GraphicsDevice from)
        {
            return from.Context;
        }
    }
}
