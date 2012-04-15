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
using System.Runtime.InteropServices;
using SharpDX.Direct3D;
using SharpDX.DXGI;

namespace SharpDX.Direct3D11
{
    public partial class Device
    {
        private DeviceContext _immediateContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class. 
        /// </summary>
        /// <param name="driverType">
        /// Type of the driver.
        /// </param>
        public Device(DriverType driverType)
            : this(driverType, DeviceCreationFlags.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class. 
        /// </summary>
        /// <param name="adapter">
        /// The adapter.
        /// </param>
        public Device(Adapter adapter)
            : this(adapter, DeviceCreationFlags.None)
        {
        }

        /// <summary>
        /// Constructor for a D3D11 Device. See <see cref="SharpDX.Direct3D11.D3D11.CreateDevice"/> for more information.
        /// </summary>
        /// <param name="driverType">Type of the driver.</param>
        /// <param name="flags">The flags.</param>
        public Device(DriverType driverType, DeviceCreationFlags flags)
        {
            CreateDevice(null, driverType, flags, null);
        }

        /// <summary>
        ///   Constructor for a D3D11 Device. See <see cref = "SharpDX.Direct3D11.D3D11.CreateDevice" /> for more information.
        /// </summary>
        /// <param name = "adapter"></param>
        /// <param name = "flags"></param>
        public Device(Adapter adapter, DeviceCreationFlags flags)
        {
            CreateDevice(adapter, DriverType.Unknown, flags, null);
        }

        /// <summary>
        ///   Constructor for a D3D11 Device. See <see cref = "SharpDX.Direct3D11.D3D11.CreateDevice" /> for more information.
        /// </summary>
        /// <param name = "driverType"></param>
        /// <param name = "flags"></param>
        /// <param name = "featureLevels"></param>
        public Device(DriverType driverType, DeviceCreationFlags flags, params FeatureLevel[] featureLevels)
        {
            CreateDevice(null, driverType, flags, featureLevels);
        }

        /// <summary>
        ///   Constructor for a D3D11 Device. See <see cref = "SharpDX.Direct3D11.D3D11.CreateDevice" /> for more information.
        /// </summary>
        /// <param name = "adapter"></param>
        /// <param name = "flags"></param>
        /// <param name = "featureLevels"></param>
        public Device(Adapter adapter, DeviceCreationFlags flags, params FeatureLevel[] featureLevels)
        {
            CreateDevice(adapter, DriverType.Unknown, flags, featureLevels);
        }

#if !WIN8METRO

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D11.Device" /> class along with a new <see cref = "T:SharpDX.DXGI.SwapChain" /> used for rendering.
        /// </summary>
        /// <param name = "driverType">The type of device to create.</param>
        /// <param name = "flags">A list of runtime layers to enable.</param>
        /// <param name = "swapChainDescription">Details used to create the swap chain.</param>
        /// <param name = "device">When the method completes, contains the created device instance.</param>
        /// <param name = "swapChain">When the method completes, contains the created swap chain instance.</param>
        /// <returns>A <see cref = "T:SharpDX.Result" /> object describing the result of the operation.</returns>
        public static Result CreateWithSwapChain(DriverType driverType, DeviceCreationFlags flags,
                                                 SwapChainDescription swapChainDescription, out Device device,
                                                 out SwapChain swapChain)
        {
            return CreateWithSwapChain(null, driverType, flags, null, swapChainDescription, out device, out swapChain);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D11.Device" /> class along with a new <see cref = "T:SharpDX.DXGI.SwapChain" /> used for rendering.
        /// </summary>
        /// <param name = "adapter">The video adapter on which the device should be created.</param>
        /// <param name = "flags">A list of runtime layers to enable.</param>
        /// <param name = "swapChainDescription">Details used to create the swap chain.</param>
        /// <param name = "device">When the method completes, contains the created device instance.</param>
        /// <param name = "swapChain">When the method completes, contains the created swap chain instance.</param>
        /// <returns>A <see cref = "T:SharpDX.Result" /> object describing the result of the operation.</returns>
        public static Result CreateWithSwapChain(Adapter adapter, DeviceCreationFlags flags,
                                                 SwapChainDescription swapChainDescription, out Device device,
                                                 out SwapChain swapChain)
        {
            return CreateWithSwapChain(adapter, DriverType.Unknown, flags, null, swapChainDescription, out device,
                                       out swapChain);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D11.Device" /> class along with a new <see cref = "T:SharpDX.DXGI.SwapChain" /> used for rendering.
        /// </summary>
        /// <param name = "driverType">The type of device to create.</param>
        /// <param name = "flags">A list of runtime layers to enable.</param>
        /// <param name = "featureLevels">A list of feature levels which determine the order of feature levels to attempt to create.</param>
        /// <param name = "swapChainDescription">Details used to create the swap chain.</param>
        /// <param name = "device">When the method completes, contains the created device instance.</param>
        /// <param name = "swapChain">When the method completes, contains the created swap chain instance.</param>
        /// <returns>A <see cref = "T:SharpDX.Result" /> object describing the result of the operation.</returns>
        public static Result CreateWithSwapChain(DriverType driverType, DeviceCreationFlags flags,
                                                 FeatureLevel[] featureLevels, SwapChainDescription swapChainDescription,
                                                 out Device device, out SwapChain swapChain)
        {
            return CreateWithSwapChain(null, driverType, flags, featureLevels, swapChainDescription, out device,
                                       out swapChain);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D11.Device" /> class along with a new <see cref = "T:SharpDX.DXGI.SwapChain" /> used for rendering.
        /// </summary>
        /// <param name = "adapter">The video adapter on which the device should be created.</param>
        /// <param name = "flags">A list of runtime layers to enable.</param>
        /// <param name = "featureLevels">A list of feature levels which determine the order of feature levels to attempt to create.</param>
        /// <param name = "swapChainDescription">Details used to create the swap chain.</param>
        /// <param name = "device">When the method completes, contains the created device instance.</param>
        /// <param name = "swapChain">When the method completes, contains the created swap chain instance.</param>
        /// <returns>A <see cref = "T:SharpDX.Result" /> object describing the result of the operation.</returns>
        public static Result CreateWithSwapChain(Adapter adapter, DeviceCreationFlags flags,
                                                 FeatureLevel[] featureLevels, SwapChainDescription swapChainDescription,
                                                 out Device device, out SwapChain swapChain)
        {
            return CreateWithSwapChain(adapter, DriverType.Unknown, flags, featureLevels, swapChainDescription,
                                       out device, out swapChain);
        }

        /// <summary>
        ///   This overload has been deprecated. Use one of the alternatives that does not take both an adapter and a driver type.
        /// </summary>
        private static Result CreateWithSwapChain(Adapter adapter, DriverType driverType, DeviceCreationFlags flags,
                                                 FeatureLevel[] featureLevels, SwapChainDescription swapChainDescription,
                                                 out Device device, out SwapChain swapChain)
        {
            FeatureLevel selectedLevel;
            DeviceContext context;

            Result result = D3D11.CreateDeviceAndSwapChain(adapter, driverType, IntPtr.Zero, flags, featureLevels,
                                                           featureLevels == null ? 0 : featureLevels.Length,
                                                           D3D11.SdkVersion,
                                                           ref swapChainDescription, out swapChain, out device,
                                                           out selectedLevel, out context);

            device.ImmediateContext = context;

            return result;
        }
#endif

        /// <summary>
        ///   Get the imediate <see cref = "SharpDX.Direct3D11.DeviceContext" /> attached to this Device.
        /// </summary>
        public DeviceContext ImmediateContext
        {
            get
            {
                if (_immediateContext == null)
                {
                    GetImmediateContext(out _immediateContext);
                }
                return _immediateContext;
            }

            private set { _immediateContext = value; }
        }

        /// <summary>
        /// Get the type, name, units of measure, and a description of an existing counter.	
        /// </summary>
        /// <param name="counterDescription">The counter description.</param>
        /// <returns>Description of the counter</returns>
        public CounterMetadata GetCounterMetadata(CounterDescription counterDescription)
        {
            unsafe
            {
                var data = new CounterMetadata();
                CounterType type;
                int hardwareCounterCount;
                int nameLength = 0;
                int unitsLength = 0;
                int descriptionLength = 0;

                // Get Length for strings
                CheckCounter(counterDescription, out type, out hardwareCounterCount, IntPtr.Zero, new IntPtr(&nameLength), IntPtr.Zero, new IntPtr(&unitsLength),
                             IntPtr.Zero, new IntPtr(&descriptionLength));

                sbyte* name = stackalloc sbyte[nameLength];
                sbyte* units = stackalloc sbyte[unitsLength];
                sbyte* description = stackalloc sbyte[descriptionLength];

                // Get strings
                CheckCounter(counterDescription, out type, out hardwareCounterCount, new IntPtr(name), new IntPtr(&nameLength), new IntPtr(units), new IntPtr(&unitsLength),
                             new IntPtr(description), new IntPtr(&descriptionLength));

                data.Type = type;
                data.HardwareCounterCount = hardwareCounterCount;
                data.Name = Marshal.PtrToStringAnsi((IntPtr)name, nameLength);
                data.Units = Marshal.PtrToStringAnsi((IntPtr)units, unitsLength);
                data.Description = Marshal.PtrToStringAnsi((IntPtr)description, descriptionLength);

                return data;
            }
        }

        /// <summary>
        /// Give a device access to a shared resource created on a different Direct3d device.
        /// </summary>
        /// <typeparam name="T">The type of the resource we are gaining access to.</typeparam>
        /// <param name="resourceHandle">A resource handle. See remarks.</param>
        /// <returns>
        /// This method returns a reference to the resource we are gaining access to.
        /// </returns>
        /// <remarks>
        /// To share a resource between two Direct3D 10 devices the resource must have been created with the  <see cref="SharpDX.Direct3D11.ResourceOptionFlags.Shared"/> flag, if it was created using the ID3D10Device interface.  If it was created using the IDXGIDevice interface, then the resource is always shared. The REFIID, or GUID, of the interface to the resource can be obtained by using the __uuidof() macro.  For example, __uuidof(ID3D10Buffer) will get the GUID of the interface to a buffer resource. When sharing a resource between two Direct3D 10 devices the unique handle of the resource can be obtained by querying the resource for the <see cref="SharpDX.DXGI.Resource"/> interface and then calling {{GetSharedHandle}}.
        /// <code> IDXGIResource* pOtherResource(NULL);
        /// hr = pOtherDeviceResource-&gt;QueryInterface( __uuidof(IDXGIResource), (void**)&amp;pOtherResource );
        /// HANDLE sharedHandle;
        /// pOtherResource-&gt;GetSharedHandle(&amp;sharedHandle); </code>
        /// The only resources that can be shared are 2D non-mipmapped textures. To share a resource between a Direct3D 9 device and a Direct3D 10 device the texture must have been created using  the pSharedHandle argument of {{CreateTexture}}.   The shared Direct3D 9 handle is then passed to OpenSharedResource in the hResource argument. The following code illustrates the method calls involved.
        /// <code> sharedHandle = NULL; // must be set to NULL to create, can use a valid handle here to open in D3D9
        /// pDevice9-&gt;CreateTexture(..., pTex2D_9, &amp;sharedHandle);
        /// ...
        /// pDevice10-&gt;OpenSharedResource(sharedHandle, __uuidof(ID3D10Resource), (void**)(&amp;tempResource10));
        /// tempResource10-&gt;QueryInterface(__uuidof(ID3D10Texture2D), (void**)(&amp;pTex2D_10));
        /// tempResource10-&gt;Release();
        /// // now use pTex2D_10 with pDevice10    </code>
        /// Textures being shared from D3D9 to D3D10 have the following restrictions.  Textures must be 2D Only 1 mip level is allowed Texture must have default usage Texture must be write only MSAA textures are not allowed Bind flags must have SHADER_RESOURCE and RENDER_TARGET set Only R10G10B10A2_UNORM, R16G16B16A16_FLOAT and R8G8B8A8_UNORM formats are allowed  If a shared texture is updated on one device <see cref="SharpDX.Direct3D11.DeviceContext.Flush"/> must be called on that device.
        /// </remarks>
        /// <unmanaged>HRESULT ID3D10Device::OpenSharedResource([In] void* hResource,[In] GUID* ReturnedInterface,[Out, Optional] void** ppResource)</unmanaged>
        public T OpenSharedResource<T>(IntPtr resourceHandle) where T : ComObject
        {
            IntPtr temp;
            OpenSharedResource(resourceHandle, Utilities.GetGuidFromType(typeof(T)), out temp);
            return FromPointer<T>(temp);
        }


        /// <summary>
        /// Check if this device is supporting compute shaders for the specified format.
        /// </summary>
        /// <param name="format">The format for which to check support.</param>
        /// <returns>Flags indicating usage contexts in which the specified format is supported.</returns>
	    public ComputeShaderFormatSupport CheckComputeShaderFormatSupport( Format format )
        {
            unsafe
            {
                FeatureDataFormatSupport2 support = default(FeatureDataFormatSupport2);
                support.InFormat = format;
                if (CheckFeatureSupport(Feature.ComputeShaders, new IntPtr(&support), Utilities.SizeOf<FeatureDataFormatSupport2>()).Failure)
                    return ComputeShaderFormatSupport.None;
                return support.OutFormatSupport2;
            }
        }

        /// <summary>
        /// Check if this device is supporting a feature.
        /// </summary>
        /// <param name="feature">The feature to check.</param>
        /// <returns>
        /// Returns true if this device supports this feature, otherwise false.
        /// </returns>
        public bool CheckFeatureSupport(Feature feature)
        {
            unsafe
            {
                switch (feature)
                {
                    case Feature.ShaderDoubles:
                        {
                            FeatureDataDoubles support;

                            if (CheckFeatureSupport(Feature.ShaderDoubles, new IntPtr(&support), Utilities.SizeOf<FeatureDataDoubles>()).Failure)
                                return false;
                            return support.DoublePrecisionFloatShaderOps;
                        }
                    case Feature.ComputeShaders:
                    case Feature.D3D10XHardwareOptions:
                        {
                            FeatureDataD3D10XHardwareOptions support;
                            if (CheckFeatureSupport(Feature.D3D10XHardwareOptions, new IntPtr(&support), Utilities.SizeOf<FeatureDataD3D10XHardwareOptions>()).Failure)
                                return false;
                            return support.ComputeShadersPlusRawAndStructuredBuffersViaShader4X;
                        }
                    default:
                        throw new SharpDXException("Unsupported Feature. Use specialized CheckXXX methods");
                }
            }
        }

        /// <summary>
        /// Check if this device is supporting threading.
        /// </summary>
        /// <param name="supportsConcurrentResources">Support concurrent resources.</param>
        /// <param name="supportsCommandLists">Support command lists.</param>
        /// <returns>
        /// A <see cref="T:SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        public Result CheckThreadingSupport( out bool supportsConcurrentResources, out bool supportsCommandLists )
        {
            unsafe
            {
                var support = default(FeatureDataThreading);
                var result = CheckFeatureSupport(Feature.Threading, new IntPtr(&support), Utilities.SizeOf<FeatureDataThreading>());

                if (result.Failure)
                {
                    supportsConcurrentResources = false;
                    supportsCommandLists = false;
                }
                else
                {
                    supportsConcurrentResources = support.DriverConcurrentCreates;
                    supportsCommandLists = support.DriverCommandLists;
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the highest supported hardware feature level of the primary adapter.
        /// </summary>
        /// <returns>The highest supported hardware feature level.</returns>
        public static FeatureLevel GetSupportedFeatureLevel()
        {
            FeatureLevel outputLevel;
            var device = new Device(IntPtr.Zero);
            DeviceContext context;
            D3D11.CreateDevice(null, DriverType.Hardware, IntPtr.Zero, DeviceCreationFlags.None, null, 0, D3D11.SdkVersion, device, out outputLevel,
                               out context);
            context.Dispose();
            device.Dispose();
            return outputLevel;
        }

        /// <summary>
        /// Gets the highest supported hardware feature level of the primary adapter.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        /// <returns>
        /// The highest supported hardware feature level.
        /// </returns>
        public static FeatureLevel GetSupportedFeatureLevel(Adapter adapter)
        {
            FeatureLevel outputLevel;
            var device = new Device(IntPtr.Zero);
            DeviceContext context;
            D3D11.CreateDevice(adapter, DriverType.Unknown, IntPtr.Zero, DeviceCreationFlags.None, null, 0, D3D11.SdkVersion, device, out outputLevel,
                               out context);
            context.Dispose();
            device.Dispose();
            return outputLevel;
        }
#if !DIRECT3D11_1
        /// <summary>
        /// Gets a value indicating whether the current device is using the reference rasterizer.
        /// </summary>
        public bool IsReferenceDevice
        {
            get
            {
                try
                {
                    var switchToRef = QueryInterface<SwitchToRef>();
                    return switchToRef.UseRef;
                }
                catch (SharpDXException)
                {
                    return false;
                }
            }
        }
#endif

        /// <summary>
        /// Gets or sets the debug-name for this object.
        /// </summary>
        /// <value>
        /// The debug name.
        /// </value>
        public string DebugName
        {
            get
            {
                unsafe
                {
                    byte* pname = stackalloc byte[1024];
                    int size = 1024 - 1;
                    if (GetPrivateData(CommonGuid.DebugObjectName, ref size, new IntPtr(pname)).Failure)
                        return string.Empty;
                    pname[size] = 0;
                    return Marshal.PtrToStringAnsi(new IntPtr(pname));
                }
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    SetPrivateData(CommonGuid.DebugObjectName, 0, IntPtr.Zero);
                }
                else
                {
                    var namePtr = Marshal.StringToHGlobalAnsi(value);
                    SetPrivateData(CommonGuid.DebugObjectName, value.Length, namePtr);
                    // Warning, allocated string should not be released!
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_immediateContext != null)
                {
                    _immediateContext.ClearState();
                    _immediateContext.Flush();
                    _immediateContext.Dispose();
                    _immediateContext = null;
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        ///   Internal CreateDevice
        /// </summary>
        /// <param name = "adapter"></param>
        /// <param name = "driverType"></param>
        /// <param name = "flags"></param>
        /// <param name = "featureLevels"></param>
        private void CreateDevice(Adapter adapter, DriverType driverType, DeviceCreationFlags flags,
                                  FeatureLevel[] featureLevels)
        {
            FeatureLevel selectedLevel;
            D3D11.CreateDevice(adapter, driverType, IntPtr.Zero, flags, featureLevels,
                               featureLevels == null ? 0 : featureLevels.Length, D3D11.SdkVersion, this,
                               out selectedLevel, out _immediateContext);
        }
    }
}