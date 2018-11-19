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
using System.Runtime.InteropServices;
using SharpDX.Direct3D;
using SharpDX.DXGI;

namespace SharpDX.Direct3D11
{
    public partial class Device
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class. 
        /// </summary>
        /// <param name="driverType">
        /// Type of the driver.
        /// </param>
        /// <msdn-id>ff476082</msdn-id>	
        /// <unmanaged>HRESULT D3D11CreateDevice([In, Optional] IDXGIAdapter* pAdapter,[In] D3D_DRIVER_TYPE DriverType,[In] HINSTANCE Software,[In] D3D11_CREATE_DEVICE_FLAG Flags,[In, Buffer, Optional] const D3D_FEATURE_LEVEL* pFeatureLevels,[In] unsigned int FeatureLevels,[In] unsigned int SDKVersion,[Out, Fast] ID3D11Device** ppDevice,[Out, Optional] D3D_FEATURE_LEVEL* pFeatureLevel,[Out, Optional] ID3D11DeviceContext** ppImmediateContext)</unmanaged>	
        /// <unmanaged-short>D3D11CreateDevice</unmanaged-short>	
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
        /// <msdn-id>ff476082</msdn-id>	
        /// <unmanaged>HRESULT D3D11CreateDevice([In, Optional] IDXGIAdapter* pAdapter,[In] D3D_DRIVER_TYPE DriverType,[In] HINSTANCE Software,[In] D3D11_CREATE_DEVICE_FLAG Flags,[In, Buffer, Optional] const D3D_FEATURE_LEVEL* pFeatureLevels,[In] unsigned int FeatureLevels,[In] unsigned int SDKVersion,[Out, Fast] ID3D11Device** ppDevice,[Out, Optional] D3D_FEATURE_LEVEL* pFeatureLevel,[Out, Optional] ID3D11DeviceContext** ppImmediateContext)</unmanaged>	
        /// <unmanaged-short>D3D11CreateDevice</unmanaged-short>	
        public Device(Adapter adapter)
            : this(adapter, DeviceCreationFlags.None)
        {
        }

        /// <summary>
        /// Constructor for a D3D11 Device. See <see cref="SharpDX.Direct3D11.D3D11.CreateDevice"/> for more information.
        /// </summary>
        /// <param name="driverType">Type of the driver.</param>
        /// <param name="flags">The flags.</param>
        /// <msdn-id>ff476082</msdn-id>	
        /// <unmanaged>HRESULT D3D11CreateDevice([In, Optional] IDXGIAdapter* pAdapter,[In] D3D_DRIVER_TYPE DriverType,[In] HINSTANCE Software,[In] D3D11_CREATE_DEVICE_FLAG Flags,[In, Buffer, Optional] const D3D_FEATURE_LEVEL* pFeatureLevels,[In] unsigned int FeatureLevels,[In] unsigned int SDKVersion,[Out, Fast] ID3D11Device** ppDevice,[Out, Optional] D3D_FEATURE_LEVEL* pFeatureLevel,[Out, Optional] ID3D11DeviceContext** ppImmediateContext)</unmanaged>	
        /// <unmanaged-short>D3D11CreateDevice</unmanaged-short>	
        public Device(DriverType driverType, DeviceCreationFlags flags)
        {
            CreateDevice(null, driverType, flags, null);
        }

        /// <summary>
        ///   Constructor for a D3D11 Device. See <see cref = "SharpDX.Direct3D11.D3D11.CreateDevice" /> for more information.
        /// </summary>
        /// <param name = "adapter"></param>
        /// <param name = "flags"></param>
        /// <msdn-id>ff476082</msdn-id>	
        /// <unmanaged>HRESULT D3D11CreateDevice([In, Optional] IDXGIAdapter* pAdapter,[In] D3D_DRIVER_TYPE DriverType,[In] HINSTANCE Software,[In] D3D11_CREATE_DEVICE_FLAG Flags,[In, Buffer, Optional] const D3D_FEATURE_LEVEL* pFeatureLevels,[In] unsigned int FeatureLevels,[In] unsigned int SDKVersion,[Out, Fast] ID3D11Device** ppDevice,[Out, Optional] D3D_FEATURE_LEVEL* pFeatureLevel,[Out, Optional] ID3D11DeviceContext** ppImmediateContext)</unmanaged>	
        /// <unmanaged-short>D3D11CreateDevice</unmanaged-short>	
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
        /// <msdn-id>ff476082</msdn-id>	
        /// <unmanaged>HRESULT D3D11CreateDevice([In, Optional] IDXGIAdapter* pAdapter,[In] D3D_DRIVER_TYPE DriverType,[In] HINSTANCE Software,[In] D3D11_CREATE_DEVICE_FLAG Flags,[In, Buffer, Optional] const D3D_FEATURE_LEVEL* pFeatureLevels,[In] unsigned int FeatureLevels,[In] unsigned int SDKVersion,[Out, Fast] ID3D11Device** ppDevice,[Out, Optional] D3D_FEATURE_LEVEL* pFeatureLevel,[Out, Optional] ID3D11DeviceContext** ppImmediateContext)</unmanaged>	
        /// <unmanaged-short>D3D11CreateDevice</unmanaged-short>	
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
        /// <msdn-id>ff476082</msdn-id>	
        /// <unmanaged>HRESULT D3D11CreateDevice([In, Optional] IDXGIAdapter* pAdapter,[In] D3D_DRIVER_TYPE DriverType,[In] HINSTANCE Software,[In] D3D11_CREATE_DEVICE_FLAG Flags,[In, Buffer, Optional] const D3D_FEATURE_LEVEL* pFeatureLevels,[In] unsigned int FeatureLevels,[In] unsigned int SDKVersion,[Out, Fast] ID3D11Device** ppDevice,[Out, Optional] D3D_FEATURE_LEVEL* pFeatureLevel,[Out, Optional] ID3D11DeviceContext** ppImmediateContext)</unmanaged>	
        /// <unmanaged-short>D3D11CreateDevice</unmanaged-short>	
        public Device(Adapter adapter, DeviceCreationFlags flags, params FeatureLevel[] featureLevels)
        {
            CreateDevice(adapter, DriverType.Unknown, flags, featureLevels);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D11.Device" /> class along with a new <see cref = "T:SharpDX.DXGI.SwapChain" /> used for rendering.
        /// </summary>
        /// <param name = "driverType">The type of device to create.</param>
        /// <param name = "flags">A list of runtime layers to enable.</param>
        /// <param name = "swapChainDescription">Details used to create the swap chain.</param>
        /// <param name = "device">When the method completes, contains the created device instance.</param>
        /// <param name = "swapChain">When the method completes, contains the created swap chain instance.</param>
        /// <returns>A <see cref = "T:SharpDX.Result" /> object describing the result of the operation.</returns>
        public static void CreateWithSwapChain(DriverType driverType, DeviceCreationFlags flags,
                                                 SwapChainDescription swapChainDescription, out Device device,
                                                 out SwapChain swapChain)
        {
            CreateWithSwapChain(null, driverType, flags, null, swapChainDescription, out device, out swapChain);
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
        public static void CreateWithSwapChain(Adapter adapter, DeviceCreationFlags flags,
                                                 SwapChainDescription swapChainDescription, out Device device,
                                                 out SwapChain swapChain)
        {
            CreateWithSwapChain(adapter, DriverType.Unknown, flags, null, swapChainDescription, out device,
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
        public static void CreateWithSwapChain(DriverType driverType, DeviceCreationFlags flags,
                                                 FeatureLevel[] featureLevels, SwapChainDescription swapChainDescription,
                                                 out Device device, out SwapChain swapChain)
        {
            CreateWithSwapChain(null, driverType, flags, featureLevels, swapChainDescription, out device,
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
        public static void CreateWithSwapChain(Adapter adapter, DeviceCreationFlags flags,
                                                 FeatureLevel[] featureLevels, SwapChainDescription swapChainDescription,
                                                 out Device device, out SwapChain swapChain)
        {
            CreateWithSwapChain(adapter, DriverType.Unknown, flags, featureLevels, swapChainDescription,
                                       out device, out swapChain);
        }

        /// <summary>
        ///   This overload has been deprecated. Use one of the alternatives that does not take both an adapter and a driver type.
        /// </summary>
        private static void CreateWithSwapChain(Adapter adapter, DriverType driverType, DeviceCreationFlags flags,
                                                 FeatureLevel[] featureLevels, SwapChainDescription swapChainDescription,
                                                 out Device device, out SwapChain swapChain)
        {
            device = adapter == null ? new Device(driverType, flags, featureLevels) : new Device(adapter, flags, featureLevels);

            using (var factory = new Factory1())
            {
                swapChain = new SwapChain(factory, device, swapChainDescription);
            }
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
        /// <p>Gets information about the features <see cref="Feature.D3D11Options"/> that are supported by the current graphics driver.</p>	
        /// </summary>	
        /// <returns>Returns a structure <see cref="FeatureDataD3D11Options"/> </returns>	
        /// <msdn-id>ff476497</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CheckFeatureSupport([In] D3D11_FEATURE Feature,[Out, Buffer] void* pFeatureSupportData,[In] unsigned int FeatureSupportDataSize)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CheckFeatureSupport</unmanaged-short>	
        public FeatureDataD3D11Options CheckD3D11Feature()
        {
            unsafe
            {
                var support = default(FeatureDataD3D11Options);
                if (CheckFeatureSupport(Feature.D3D11Options, new IntPtr(&support), Utilities.SizeOf<FeatureDataD3D11Options>()).Failure)
                    return default(FeatureDataD3D11Options);
                return support;
            }
        }

        /// <summary>	
        /// <p>Gets information about the features <see cref="Feature.ShaderMinimumPrecisionSupport"/> that are supported by the current graphics driver.</p>	
        /// </summary>	
        /// <returns>Returns a structure <see cref="FeatureDataShaderMinimumPrecisionSupport"/> </returns>	
        /// <msdn-id>ff476497</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CheckFeatureSupport([In] D3D11_FEATURE Feature,[Out, Buffer] void* pFeatureSupportData,[In] unsigned int FeatureSupportDataSize)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CheckFeatureSupport</unmanaged-short>	
        public FeatureDataShaderMinimumPrecisionSupport CheckShaderMinimumPrecisionSupport()
        {
            unsafe
            {
                var support = default(FeatureDataShaderMinimumPrecisionSupport);
                if (CheckFeatureSupport(Feature.ShaderMinimumPrecisionSupport, new IntPtr(&support), Utilities.SizeOf<FeatureDataShaderMinimumPrecisionSupport>()).Failure)
                    return default(FeatureDataShaderMinimumPrecisionSupport);
                return support;
            }
        }

        /// <summary>	
        /// <p>Gets information about whether the driver supports the nonpowers-of-2-unconditionally feature. <strong>TRUE</strong> for hardware at Direct3D 10 and higher feature levels. </p>	
        /// </summary>	
        /// <returns>Returns <strong>true</strong> if this hardware supports non-powers-of-2 texture. This returns always true Direct3D 10 and higher feature levels.</returns>	
        /// <msdn-id>ff476497</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CheckFeatureSupport([In] D3D11_FEATURE Feature,[Out, Buffer] void* pFeatureSupportData,[In] unsigned int FeatureSupportDataSize)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CheckFeatureSupport</unmanaged-short>	
        public bool CheckFullNonPow2TextureSupport()
        {
            unsafe
            {
                var support = default(FeatureDataD3D9Options);
                var result = CheckFeatureSupport(Feature.D3D9Options, new IntPtr(&support), Utilities.SizeOf<FeatureDataD3D9Options>());
                if (FeatureLevel <= FeatureLevel.Level_9_3)
                {
                    return result.Failure;
                }
                if (result.Failure)
                    return false;
                return support.FullNonPow2TextureSupport;
            }
        }

        /// <summary>	
        /// <p>Gets information about whether a rendering device batches rendering commands and performs multipass rendering into tiles or bins over a render area. Certain API usage patterns that are fine TileBasedDefferredRenderers (TBDRs) can perform worse on non-TBDRs and vice versa.  Applications that are careful about rendering can be friendly to both TBDR and non-TBDR architectures.</p> 
        /// </summary>	
        /// <returns>Returns <strong>TRUE</strong> if the rendering device batches rendering commands and <strong><see cref="SharpDX.Result.False"/></strong> otherwise.</returns>	
        /// <msdn-id>ff476497</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CheckFeatureSupport([In] D3D11_FEATURE Feature,[Out, Buffer] void* pFeatureSupportData,[In] unsigned int FeatureSupportDataSize)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CheckFeatureSupport</unmanaged-short>	
        public bool CheckTileBasedDeferredRendererSupport()
        {
            unsafe
            {
                var support = default(FeatureDataArchitectureInformation);
                if (CheckFeatureSupport(Feature.ArchitectureInformation, new IntPtr(&support), Utilities.SizeOf<FeatureDataArchitectureInformation>()).Failure)
                    return false;
                return support.TileBasedDeferredRenderer;
            }
        }

        public FeatureDataD3D11Options1 CheckD3D112Feature()
        {
            unsafe
            {
                var support = default(FeatureDataD3D11Options1);
                if (CheckFeatureSupport(Feature.D3D11Options1, new IntPtr(&support), Utilities.SizeOf<FeatureDataD3D11Options1>()).Failure)
                    return default(FeatureDataD3D11Options1);
                return support;
            }
        }

        /// <summary>
        /// Retrieves information about Direct3D11.3 feature options in the current graphics driver
        /// </summary>
        /// <msdn-id>dn879499</msdn-id>
        /// <returns>Returns a structure <see cref="FeatureDataD3D11Options2"/> </returns>	
        public FeatureDataD3D11Options2 CheckD3D113Features2()
        {
            unsafe
            {
                var support = default(FeatureDataD3D11Options2);
                if (CheckFeatureSupport(Feature.D3D11Options2, new IntPtr(&support), Utilities.SizeOf<FeatureDataD3D11Options2>()).Failure)
                    return default(FeatureDataD3D11Options2);
                return support;
            }
        }

        /// <summary>
        /// Retrieves additional information about Direct3D11.3 feature options in the current graphics driver
        /// </summary>
        /// <msdn-id>dn933226</msdn-id>
        /// <returns>Returns a structure <see cref="FeatureDataD3D11Options3"/> </returns>	
        public FeatureDataD3D11Options3 CheckD3D113Features3()
        {
            unsafe
            {
                var support = default(FeatureDataD3D11Options3);
                if (CheckFeatureSupport(Feature.D3D11Options3, new IntPtr(&support), Utilities.SizeOf<FeatureDataD3D11Options3>()).Failure)
                    return default(FeatureDataD3D11Options3);
                return support;
            }
        }

        /// <summary>
        /// Retrieves additional information about Direct3D11.3 feature options in the current graphics driver
        /// </summary>
        /// <msdn-id>dn933226</msdn-id>
        /// <returns>Returns a structure <see cref="FeatureDataD3D11Options4"/> </returns>	
        public FeatureDataD3D11Options4 CheckD3D113Features4()
        {
            unsafe
            {
                var support = default(FeatureDataD3D11Options4);
                if (CheckFeatureSupport(Feature.D3D11Options4, new IntPtr(&support), Utilities.SizeOf<FeatureDataD3D11Options4>()).Failure)
                    return default(FeatureDataD3D11Options4);
                return support;
            }
        }

        /// <summary>
        /// Retrieves additional information about Direct3D11.4 feature options in the current graphics driver
        /// </summary>
        /// <msdn-id>dn933226</msdn-id>
        /// <returns>Returns a structure <see cref="FeatureDataD3D11Options5"/> </returns>	
        public FeatureDataD3D11Options5 CheckD3D113Feature5()
        {
            unsafe
            {
                var support = default(FeatureDataD3D11Options5);
                if (CheckFeatureSupport(Feature.D3D11Options5, new IntPtr(&support), Utilities.SizeOf<FeatureDataD3D11Options5>()).Failure)
                    return default(FeatureDataD3D11Options5);
                return support;
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
        /// Check if a feature level is supported by a primary adapter.
        /// </summary>
        /// <param name="featureLevel">The feature level.</param>
        /// <returns><c>true</c> if the primary adapter is supporting this feature level; otherwise, <c>false</c>.</returns>
        public static bool IsSupportedFeatureLevel(FeatureLevel featureLevel)
        {
            var device = new Device(IntPtr.Zero);
            DeviceContext context = null;
            try
            {
                FeatureLevel outputLevel;
                var result = D3D11.CreateDevice(null, DriverType.Hardware, IntPtr.Zero, DeviceCreationFlags.None,
                                                new[] {featureLevel}, 1, D3D11.SdkVersion, device, out outputLevel,
                                                out context);
                return result.Success && outputLevel == featureLevel;
            }
            finally
            {
                if (context != null)
                {
                    context.Dispose();
                }

                if (device.NativePointer != IntPtr.Zero)
                {
                    device.Dispose();
                }
            }
        }

        /// <summary>
        /// Check if a feature level is supported by a particular adapter.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        /// <param name="featureLevel">The feature level.</param>
        /// <returns><c>true</c> if the specified adapter is supporting this feature level; otherwise, <c>false</c>.</returns>
        public static bool IsSupportedFeatureLevel(Adapter adapter, FeatureLevel featureLevel)
        {
            var device = new Device(IntPtr.Zero);
            DeviceContext context = null;
            try
            {
                FeatureLevel outputLevel;
                var result = D3D11.CreateDevice(adapter, DriverType.Unknown, IntPtr.Zero, DeviceCreationFlags.None,
                                                new[] { featureLevel }, 1, D3D11.SdkVersion, device, out outputLevel,
                                                out context);
                return result.Success && outputLevel == featureLevel;
            }
            finally
            {
                if (context != null)
                {
                    context.Dispose();
                }

                if (device.NativePointer != IntPtr.Zero)
                {
                    device.Dispose();
                }
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
                               out context).CheckError();
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
                               out context).CheckError();
            context.Dispose();
            device.Dispose();
            return outputLevel;
        }

#if DESKTOP_APP
        /// <summary>
        /// Gets a value indicating whether the current device is using the reference rasterizer.
        /// </summary>
        public bool IsReferenceDevice
        {
            get
            {
                try
                {
                    using(var switchToRef = QueryInterface<SwitchToRef>())
                    {
                        return switchToRef.GetUseRef();
                    }
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
                    var namePtr = Utilities.StringToHGlobalAnsi(value);
                    SetPrivateData(CommonGuid.DebugObjectName, value.Length, namePtr);
                    // Warning, allocated string should not be released!
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (ImmediateContext__ != null)
                {
                    ImmediateContext__.Dispose();
                    ImmediateContext__ = null;
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
                                        out selectedLevel, out ImmediateContext__).CheckError();

            if (ImmediateContext__ != null)
            {
                // Add a reference when setting the device on the context
                ((IUnknown)this).AddReference();
                ImmediateContext__.Device__ = this;
            }
        }

        /// <summary>	
        /// <p> Creates a device that uses Direct3D 11 functionality in Direct3D 12, specifying a pre-existing D3D12 device to use for D3D11 interop. </p>	
        /// </summary>	
        /// <param name="d3D12Device"><dd>  <p> Specifies a pre-existing D3D12 device to use for D3D11 interop. May not be <c>null</c>. </p> </dd></param>	
        /// <param name="flags"><dd>  <p> Any of those documented for <strong>D3D11CreateDeviceAndSwapChain</strong>. Specifies which runtime layers to enable (see the <strong><see cref="SharpDX.Direct3D11.DeviceCreationFlags"/></strong> enumeration); values can be bitwise OR'd together. <em>Flags</em> must be compatible with device flags, and its <em>NodeMask</em> must be a subset of the <em>NodeMask</em> provided to the present API. </p> </dd></param>	
        /// <param name="featureLevels"><dd>  <p> An array of any of the following: </p> <ul> <li><see cref="SharpDX.Direct3D.FeatureLevel.Level_12_1"/></li> <li><see cref="SharpDX.Direct3D.FeatureLevel.Level_12_0"/></li> <li><see cref="SharpDX.Direct3D.FeatureLevel.Level_11_1"/></li> <li><see cref="SharpDX.Direct3D.FeatureLevel.Level_11_0"/></li> <li><see cref="SharpDX.Direct3D.FeatureLevel.Level_10_1"/></li> <li><see cref="SharpDX.Direct3D.FeatureLevel.Level_10_0"/></li> <li><see cref="SharpDX.Direct3D.FeatureLevel.Level_9_3"/></li> <li><see cref="SharpDX.Direct3D.FeatureLevel.Level_9_2"/></li> <li><see cref="SharpDX.Direct3D.FeatureLevel.Level_9_1"/></li> </ul> <p> The first feature level which is less than or equal to the D3D12 device's feature level will be used to perform D3D11 validation. Creation will fail if no acceptable feature levels are provided. Providing <c>null</c> will default to the D3D12 device's feature level. </p> </dd></param>	
        /// <param name="commandQueues"><dd>  <p> An array of unique queues for D3D11On12 to use. Valid queue types: 3D command queue. </p> </dd></param>	
        /// <returns>The Direct3D11 device created around the specified Direct3D12 device</returns>	
        /// <remarks>	
        /// <p> The function signature PFN_D3D11ON12_CREATE_DEVICE is provided as a typedef, so that you can use dynamic linking techniques (GetProcAddress) instead of statically linking. </p>	
        /// </remarks>	
        /// <msdn-id>dn933209</msdn-id>	
        /// <unmanaged>HRESULT D3D11On12CreateDevice([In] IUnknown* pDevice,[In] D3D11_CREATE_DEVICE_FLAG Flags,[In, Buffer, Optional] const D3D_FEATURE_LEVEL* pFeatureLevels,[In] unsigned int FeatureLevels,[In, Buffer, Optional] const IUnknown** ppCommandQueues,[In] unsigned int NumQueues,[In] unsigned int NodeMask,[Out] ID3D11Device** ppDevice,[Out, Optional] ID3D11DeviceContext** ppImmediateContext,[Out, Optional] D3D_FEATURE_LEVEL* pChosenFeatureLevel)</unmanaged>	
        /// <unmanaged-short>D3D11On12CreateDevice</unmanaged-short>	
        public static Device CreateFromDirect3D12(ComObject d3D12Device, Direct3D11.DeviceCreationFlags flags, Direct3D.FeatureLevel[] featureLevels, DXGI.Adapter adapter, params ComObject[] commandQueues)
        {
            if(d3D12Device == null) throw new ArgumentNullException("d3D12Device");
            Device devOut;
            DeviceContext contextOut;
            FeatureLevel featurelevelOut;

            D3D11.On12CreateDevice(d3D12Device, flags, featureLevels, featureLevels == null ? 0 : featureLevels.Length, commandQueues, commandQueues.Length, 0, out devOut, out contextOut, out featurelevelOut);
            contextOut.Dispose();
            return devOut;
        }
    }
}
