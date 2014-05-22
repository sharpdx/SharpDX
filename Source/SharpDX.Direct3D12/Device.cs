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
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace SharpDX.Direct3D12
{
    public partial class Device
    {
        private FeatureLevel selectedLevel;
        private Direct3D11.Device device11;
        private CommandQueue defaultCommandQueue;

        public Device(DriverType driverType, int maxRecordingCommandLists = 32)
            : this(driverType, DeviceCreationFlags.None, maxRecordingCommandLists)
        {
        }

        public Device(Adapter adapter, int maxRecordingCommandLists = 32)
            : this(adapter, DeviceCreationFlags.None, maxRecordingCommandLists)
        {
        }

        public Device(DriverType driverType, DeviceCreationFlags flags, int maxRecordingCommandLists = 32)
        {
            CreateDevice(null, driverType, flags, null, maxRecordingCommandLists);
        }

        public Device(Adapter adapter, DeviceCreationFlags flags, int maxRecordingCommandLists = 32)
        {
            CreateDevice(adapter, DriverType.Unknown, flags, null, maxRecordingCommandLists);
        }

        public Device(DriverType driverType, DeviceCreationFlags flags, int maxRecordingCommandLists, params FeatureLevel[] featureLevels)
        {
            CreateDevice(null, driverType, flags, featureLevels, maxRecordingCommandLists);
        }

        public Device(Adapter adapter, DeviceCreationFlags flags, int maxRecordingCommandLists, params FeatureLevel[] featureLevels)
        {
            CreateDevice(adapter, DriverType.Unknown, flags, featureLevels, maxRecordingCommandLists);
        }

        protected override void NativePointerUpdated(IntPtr oldNativePointer)
        {
            base.NativePointerUpdated(oldNativePointer);

            Utilities.Dispose(ref device11);
            Utilities.Dispose(ref defaultCommandQueue);

            if(NativePointer != IntPtr.Zero)
            {
                device11 = this.QueryInterface<Direct3D11.Device>();
                GetDefaultCommandQueue(out defaultCommandQueue);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                Utilities.Dispose(ref device11);
                Utilities.Dispose(ref defaultCommandQueue);
            }

            base.Dispose(disposing);
        }

        private void CreateDevice(Adapter adapter, DriverType driverType, DeviceCreationFlags flags,
                                  FeatureLevel[] featureLevels, int maxRecordingCommandLists)
        {
            D3D12.CreateDevice(adapter, driverType, IntPtr.Zero, flags, featureLevels,
                                        featureLevels == null ? 0 : featureLevels.Length, D3D12.SdkVersion, maxRecordingCommandLists, this,
                                        out selectedLevel).CheckError();
        }

        public static implicit operator Direct3D11.Device(Device device)
        {
            return device == null ? null : device.device11;
        }
    }
}