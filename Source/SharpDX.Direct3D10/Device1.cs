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
using SharpDX.Direct3D;
using SharpDX.DXGI;

namespace SharpDX.Direct3D10
{
    public partial class Device1
    {
        /// <summary>
        ///   Constructor for a D3D10.1 Device. See <see cref = "SharpDX.Direct3D10.D3D10.CreateDevice1" /> for more information.
        /// </summary>
        /// <param name = "driverType"></param>
        public Device1(DriverType driverType)
            : this(driverType, DeviceCreationFlags.None)
        {
        }

        /// <summary>
        ///   Constructor for a D3D10.1 Device. See <see cref = "SharpDX.Direct3D10.D3D10.CreateDevice1" /> for more information.
        /// </summary>
        /// <param name = "adapter"></param>
        public Device1(Adapter adapter)
            : this(adapter, DeviceCreationFlags.None)
        {
        }

        /// <summary>
        ///   Constructor for a D3D10.1 Device. See <see cref = "SharpDX.Direct3D10.D3D10.CreateDevice1" /> for more information.
        /// </summary>
        /// <param name = "driverType"></param>
        /// <param name = "flags"></param>
        public Device1(DriverType driverType, DeviceCreationFlags flags) : this(driverType, flags, FeatureLevel.Level_10_1)
        {
        }

        /// <summary>
        ///   Constructor for a D3D10.1 Device. See <see cref = "SharpDX.Direct3D10.D3D10.CreateDevice1" /> for more information.
        /// </summary>
        /// <param name = "adapter"></param>
        /// <param name = "flags"></param>
        public Device1(Adapter adapter, DeviceCreationFlags flags)
            : this(adapter, flags, FeatureLevel.Level_10_1)
        {
        }

        /// <summary>
        ///   Constructor for a D3D10.1 Device. See <see cref = "SharpDX.Direct3D10.D3D10.CreateDevice1" /> for more information.
        /// </summary>
        /// <param name = "driverType"></param>
        /// <param name = "flags"></param>
        /// <param name="featureLevel"></param>
        public Device1(DriverType driverType, DeviceCreationFlags flags, FeatureLevel featureLevel)
            : base(IntPtr.Zero)
        {
            CreateDevice(null, driverType, flags, featureLevel);
        }

        /// <summary>
        ///   Constructor for a D3D10.1 Device. See <see cref = "SharpDX.Direct3D10.D3D10.CreateDevice1" /> for more information.
        /// </summary>
        /// <param name = "adapter"></param>
        /// <param name = "flags"></param>
        /// <param name="featureLevel"></param>
        public Device1(Adapter adapter, DeviceCreationFlags flags, FeatureLevel featureLevel) : base(IntPtr.Zero)
        {
            CreateDevice(adapter, DriverType.Hardware, flags, featureLevel);
        }

        private void CreateDevice(Adapter adapter, DriverType driverType, DeviceCreationFlags flags, FeatureLevel featureLevel)
        {
            D3D10.CreateDevice1(adapter, driverType, IntPtr.Zero, flags, featureLevel, D3D10.SdkVersion1, this);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D10.Device1" /> class along with a new <see cref = "T:SharpDX.DXGI.SwapChain" /> used for rendering.
        /// </summary>
        /// <param name = "driverType">The type of device to create.</param>
        /// <param name = "flags">A list of runtime layers to enable.</param>
        /// <param name = "swapChainDescription">Details used to create the swap chain.</param>
        /// <param name = "device">When the method completes, contains the created device instance.</param>
        /// <param name = "swapChain">When the method completes, contains the created swap chain instance.</param>
        /// <returns>A <see cref = "T:SharpDX.Result" /> object describing the result of the operation.</returns>
        public static void CreateWithSwapChain(DriverType driverType, DeviceCreationFlags flags,
                                                 SwapChainDescription swapChainDescription, out Device1 device,
                                                 out SwapChain swapChain)
        {
            CreateWithSwapChain(null, driverType, flags, swapChainDescription, FeatureLevel.Level_10_1, out device, out swapChain);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D10.Device1" /> class along with a new <see cref = "T:SharpDX.DXGI.SwapChain" /> used for rendering.
        /// </summary>
        /// <param name = "adapter">The video adapter on which the device should be created.</param>
        /// <param name = "flags">A list of runtime layers to enable.</param>
        /// <param name = "swapChainDescription">Details used to create the swap chain.</param>
        /// <param name = "device">When the method completes, contains the created device instance.</param>
        /// <param name = "swapChain">When the method completes, contains the created swap chain instance.</param>
        /// <returns>A <see cref = "T:SharpDX.Result" /> object describing the result of the operation.</returns>
        public static void CreateWithSwapChain(Adapter adapter, DeviceCreationFlags flags,
                                                 SwapChainDescription swapChainDescription, out Device1 device,
                                                 out SwapChain swapChain)
        {
            CreateWithSwapChain(adapter, DriverType.Hardware, flags, swapChainDescription, FeatureLevel.Level_10_1, out device, out swapChain);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D10.Device1" /> class along with a new <see cref = "T:SharpDX.DXGI.SwapChain" /> used for rendering.
        /// </summary>
        /// <param name = "driverType">The type of device to create.</param>
        /// <param name = "flags">A list of runtime layers to enable.</param>
        /// <param name = "swapChainDescription">Details used to create the swap chain.</param>
        /// <param name="featureLevel">Desired feature level</param>
        /// <param name = "device">When the method completes, contains the created device instance.</param>
        /// <param name = "swapChain">When the method completes, contains the created swap chain instance.</param>
        /// <returns>A <see cref = "T:SharpDX.Result" /> object describing the result of the operation.</returns>
        public static void CreateWithSwapChain(DriverType driverType, DeviceCreationFlags flags,
                                                 SwapChainDescription swapChainDescription, FeatureLevel featureLevel, out Device1 device,
                                                 out SwapChain swapChain)
        {
            CreateWithSwapChain(null, driverType, flags, swapChainDescription, featureLevel, out device, out swapChain);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D10.Device1" /> class along with a new <see cref = "T:SharpDX.DXGI.SwapChain" /> used for rendering.
        /// </summary>
        /// <param name = "adapter">The video adapter on which the device should be created.</param>
        /// <param name = "flags">A list of runtime layers to enable.</param>
        /// <param name = "swapChainDescription">Details used to create the swap chain.</param>
        /// <param name="featureLevel">Desired feature level</param>
        /// <param name = "device">When the method completes, contains the created device instance.</param>
        /// <param name = "swapChain">When the method completes, contains the created swap chain instance.</param>
        /// <returns>A <see cref = "T:SharpDX.Result" /> object describing the result of the operation.</returns>
        public static void CreateWithSwapChain(Adapter adapter, DeviceCreationFlags flags,
                                                 SwapChainDescription swapChainDescription, FeatureLevel featureLevel,out Device1 device,
                                                 out SwapChain swapChain)
        {
            CreateWithSwapChain(adapter, DriverType.Hardware, flags, swapChainDescription, featureLevel, out device, out swapChain);
        }


        /// <summary>
        ///   This overload has been deprecated. Use one of the alternatives that does not take both an adapter and a driver type.
        /// </summary>
        internal static void CreateWithSwapChain(Adapter adapter, DriverType driverType, DeviceCreationFlags flags, SwapChainDescription swapChainDescription, FeatureLevel featureLevel, out Device1 device, out SwapChain swapChain)
        {
            D3D10.CreateDeviceAndSwapChain1(adapter, driverType, IntPtr.Zero, flags, featureLevel, D3D10.SdkVersion1,
                                                           ref swapChainDescription, out swapChain, out device);
        }

    }
}