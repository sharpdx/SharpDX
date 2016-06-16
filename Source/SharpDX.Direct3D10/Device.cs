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
using SharpDX.Mathematics.Interop;

namespace SharpDX.Direct3D10
{
    public partial class Device
    {
        /// <summary>
        ///   Constructor for a D3D10 Device. See <see cref = "SharpDX.Direct3D10.D3D10.CreateDevice" /> for more information.
        /// </summary>
        /// <param name = "driverType"></param>
        public Device(DriverType driverType)
            : this(driverType, DeviceCreationFlags.None)
        {
        }

        /// <summary>
        ///   Constructor for a D3D10 Device. See <see cref = "SharpDX.Direct3D10.D3D10.CreateDevice" /> for more information.
        /// </summary>
        /// <param name = "adapter"></param>
        public Device(Adapter adapter)
            : this(adapter, DeviceCreationFlags.None)
        {
        }

        /// <summary>
        ///   Constructor for a D3D10 Device. See <see cref = "SharpDX.Direct3D10.D3D10.CreateDevice" /> for more information.
        /// </summary>
        /// <param name = "driverType"></param>
        /// <param name = "flags"></param>
        public Device(DriverType driverType, DeviceCreationFlags flags)
        {
            CreateDevice(null, driverType, flags);
        }

        /// <summary>
        ///   Constructor for a D3D10 Device. See <see cref = "SharpDX.Direct3D10.D3D10.CreateDevice" /> for more information.
        /// </summary>
        /// <param name = "adapter"></param>
        /// <param name = "flags"></param>
        public Device(Adapter adapter, DeviceCreationFlags flags)
        {
            CreateDevice(adapter, DriverType.Hardware, flags);
        }

        private void CreateDevice(Adapter adapter, DriverType driverType, DeviceCreationFlags flags)
        {
            D3D10.CreateDevice(adapter, driverType, IntPtr.Zero, flags, D3D10.SdkVersion, this);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D10.Device" /> class along with a new <see cref = "T:SharpDX.DXGI.SwapChain" /> used for rendering.
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
            CreateWithSwapChain(null, driverType, flags, swapChainDescription, out device, out swapChain);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D10.Device" /> class along with a new <see cref = "T:SharpDX.DXGI.SwapChain" /> used for rendering.
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
            CreateWithSwapChain(adapter, DriverType.Hardware, flags, swapChainDescription, out device, out swapChain);
        }


        /// <summary>
        ///   This overload has been deprecated. Use one of the alternatives that does not take both an adapter and a driver type.
        /// </summary>
        internal static void CreateWithSwapChain(Adapter adapter, DriverType driverType, DeviceCreationFlags flags, SwapChainDescription swapChainDescription, out Device device, out SwapChain swapChain)
        {
            D3D10.CreateDeviceAndSwapChain(adapter, driverType, IntPtr.Zero, flags, D3D10.SdkVersion,
                                                           ref swapChainDescription, out swapChain, out device);
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

                char* name = stackalloc char[nameLength];
                char* units = stackalloc char[unitsLength];
                char* description = stackalloc char[descriptionLength];

                // Get strings
                CheckCounter(counterDescription, out type, out hardwareCounterCount, new IntPtr(name), new IntPtr(&nameLength), new IntPtr(units), new IntPtr(&unitsLength),
                             new IntPtr(description), new IntPtr(&descriptionLength));

                data.Type = type;
                data.HardwareCounterCount = hardwareCounterCount;
                data.Name = new string(name, 0, nameLength);
                data.Units = new string(units, 0, unitsLength);
                data.Description = new string(description, 0, descriptionLength);

                return data;
            }
        }

        /// <summary>	
        /// Get the rendering predicate state.	
        /// </summary>	
        /// <remarks>	
        /// Any returned interfaces will have their reference count incremented by one. Applications should call {{IUnknown::Release}} on the returned interfaces when they are no longer needed to avoid memory leaks. 	
        /// </remarks>	
        /// <param name="predicateValue">a boolean to fill with the predicate comparison value. FALSE upon device creation. </param>
        /// <returns>a reference to a predicate (see <see cref="SharpDX.Direct3D10.Predicate"/>).</returns>
        /// <unmanaged>void ID3D10Device::GetPredication([Out, Optional] ID3D10Predicate** ppPredicate,[Out, Optional] BOOL* pPredicateValue)</unmanaged>
        public Predicate GetPredication(out bool predicateValue)
        {
            Predicate temp;
            RawBool tempPredicateValue;
            GetPredication(out temp, out tempPredicateValue);
            predicateValue = tempPredicateValue;
            return temp;
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
        /// To share a resource between two Direct3D 10 devices the resource must have been created with the  <see cref="SharpDX.Direct3D10.ResourceOptionFlags.Shared"/> flag, if it was created using the ID3D10Device interface.  If it was created using the IDXGIDevice interface, then the resource is always shared. The REFIID, or GUID, of the interface to the resource can be obtained by using the __uuidof() macro.  For example, __uuidof(ID3D10Buffer) will get the GUID of the interface to a buffer resource. When sharing a resource between two Direct3D 10 devices the unique handle of the resource can be obtained by querying the resource for the <see cref="SharpDX.DXGI.Resource"/> interface and then calling {{GetSharedHandle}}.
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
        /// Textures being shared from D3D9 to D3D10 have the following restrictions.  Textures must be 2D Only 1 mip level is allowed Texture must have default usage Texture must be write only MSAA textures are not allowed Bind flags must have SHADER_RESOURCE and RENDER_TARGET set Only R10G10B10A2_UNORM, R16G16B16A16_FLOAT and R8G8B8A8_UNORM formats are allowed  If a shared texture is updated on one device <see cref="SharpDX.Direct3D10.Device.Flush"/> must be called on that device.
        /// </remarks>
        /// <unmanaged>HRESULT ID3D10Device::OpenSharedResource([In] void* hResource,[In] GUID* ReturnedInterface,[Out, Optional] void** ppResource)</unmanaged>
        public T OpenSharedResource<T>(IntPtr resourceHandle) where T : ComObject
        {
            IntPtr temp;
            OpenSharedResource(resourceHandle, Utilities.GetGuidFromType(typeof(T)), out temp);
            return FromPointer<T>(temp);
        }

        /// <summary>	
        /// Copy the entire contents of the source resource to the destination resource using the GPU. 	
        /// </summary>	
        /// <remarks>	
        /// This method is unusual in that it causes the GPU to perform the copy operation (similar to a memcpy by the CPU). As a result, it has a few restrictions designed for improving performance. For instance, the source and destination resources:  Must be different resources. Must be the same {{type}}. Must have identical dimensions (including width, height, depth, and size as appropriate). Will only be copied. CopyResource does not support any stretch, color key, blend, or format conversions. Must have compatible {{formats}}, which means the formats must be identical or at least from the same type group. For example, a DXGI_FORMAT_R32G32B32_FLOAT texture can be copied to an DXGI_FORMAT_R32G32B32_UINT texture since both of these formats are in the DXGI_FORMAT_R32G32B32_TYPELESS group. May not be currently {{mapped}}.   {{Immutable}}, and {{depth-stencil}} resources cannot be used as a destination.  Resources created with {{multisampling capability}} cannot be used as either a source or destination. The method is an asynchronous call which may be added to the command-buffer queue. This attempts to remove pipeline stalls that may occur when copying data. See {{performance considerations}} for more details. An application that only needs to copy a portion of the data in a resource should use <see cref="SharpDX.Direct3D10.Device.CopySubresourceRegion_"/> instead.   Differences between Direct3D 10 and Direct3D 10.1: Direct3D 10.1 enables depth-stencil resources to be used as either a source or destination. Direct3D 10.1 enables multisampled resources to be used as source and destination only if both source and destination have identical multisampled count and quality. If source and destination differ in multisampled count and quality or if the source is multisampled and the destination is not multisampled (or vice versa), the call to ID3D10Device::CopyResource fails. It is possible to copy between prestructured+typed resources and block-compressed textures. See {{Format Conversion using Direct3D 10.1}}.   ? 	
        /// </remarks>	
        /// <param name="source">A reference to the source resource (see <see cref="SharpDX.Direct3D10.Resource"/>). </param>
        /// <param name="destination">A reference to the destination resource (see <see cref="SharpDX.Direct3D10.Resource"/>). </param>
        /// <unmanaged>void ID3D10Device::CopyResource([In] ID3D10Resource* pDstResource,[In] ID3D10Resource* pSrcResource)</unmanaged>
        public void CopyResource(Resource source, Resource destination)
        {
            CopyResource_(destination, source);
        }

        /// <summary>	
        /// Copy a region from a source resource to a destination resource.	
        /// </summary>	
        /// <remarks>	
        /// The source box must be within the size of the source resource. The destination location is an absolute value (not a relative value). The destination location can be offset from the source location; however, the size of the region to copy (including the destination location) must fit in the destination resource. If the resources are buffers, all coordinates are in bytes; if the resources are textures, all coordinates are in texels.   {{D3D10CalcSubresource}} is a helper function for calculating subresource indexes. CopySubresourceRegion performs the copy on the GPU (similar to a memcpy by the CPU). As a consequence, the source and destination resources must meet the following criteria:  Must be different subresources (although they can be from the same resource). Must be the same {{type}}. Must have compatible {{formats}} (the formats must either be identical or be from the same type group). For example, a DXGI_FORMAT_R32G32B32_FLOAT texture can be copied to an DXGI_FORMAT_R32G32B32_UINT texture because both of these formats are in the DXGI_FORMAT_R32G32B32_TYPELESS group. May not be currently {{mapped}}.  CopySubresourceRegion supports only copy; it does not support any stretch, color key, blend, or format conversions. An application that needs to copy an entire resource should use <see cref="SharpDX.Direct3D10.Device.CopyResource_"/> instead. CopySubresourceRegion is an asynchronous call that the runtime can add  to the command-buffer queue. This asynchronous behavior attempts to remove pipeline stalls that may occur when copying data. See {{performance considerations}} for more details.   Differences between Direct3D 10 and Direct3D 10.1: Direct3D 10 has the following limitations:  You cannot use a depth-stencil resource as a destination. You cannot use an immutable resource as a destination. You cannot use a multisampled texture as either a source or a destination  Direct3D 10.1 has added support for the following features:  You can use a depth-stencil buffer as a source or a destination. You can use multisampled resources as  source and destination only if both source and destination have identical multisampled count and quality. If source and destination differ in multisampled count and quality or if the source is multisampled and the destination is not multisampled (or vice versa), the call to ID3D10Device::CopySubresourceRegion fails. You can copy between uncompressed and compressed resources. During copy, the format conversions that are specified in  {{Format Conversion using Direct3D 10.1}} are supported automatically. The uncompressed resource must be at least prestructured, and typed. You must also account for the difference between the virtual and the physical size of the mipmap levels.    ? Note??If you use CopySubresourceRegion with a depth-stencil buffer or a multisampled resource, you must copy the whole subresource. You must also pass 0 to the DstX, DstY, and DstZ parameters and NULL to the pSrcBox parameter. In addition, source and destination resources, which are represented by the pSrcResource and pDstResource parameters respectively, must have identical sample count values. Example The following code snippet copies a box (located at (120,100),(200,220)) from a source texture into a region (130,120),(210,240) in a destination texture. 	
        /// <code> D3D10_BOX sourceRegion;	
        /// sourceRegion.left = 120;	
        /// sourceRegion.right = 200;	
        /// sourceRegion.top = 100;	
        /// sourceRegion.bottom = 220;	
        /// sourceRegion.front = 0;	
        /// sourceRegion.back = 1; pd3dDevice-&gt;CopySubresourceRegion( pDestTexture, 0, 130, 120, 0, pSourceTexture, 0, &amp;sourceRegion ); </code>	
        /// 	
        ///  Notice that, for a 2D texture, front and back are always set to 0 and 1 respectively. 	
        /// </remarks>	
        /// <param name="source">A reference to the source resource (see <see cref="SharpDX.Direct3D10.Resource"/>). </param>
        /// <param name="sourceSubresource">index of the source. </param>
        /// <param name="sourceRegion">A 3D box (see <see cref="SharpDX.Direct3D10.ResourceRegion"/>) that defines the source subresources that can be copied. If NULL, the entire source subresource is copied. The box must fit within the source resource. </param>
        /// <param name="destination">A reference to the destination resource (see <see cref="SharpDX.Direct3D10.Resource"/>). </param>
        /// <param name="destinationSubResource">index of the destination. </param>
        /// <param name="dstX">The x coordinate of the upper left corner of the destination region. </param>
        /// <param name="dstY">The y coordinate of the upper left corner of the destination region. </param>
        /// <param name="dstZ">The z coordinate of the upper left corner of the destination region. For a 1D or 2D subresource, this must be zero. </param>
        /// <unmanaged>void ID3D10Device::CopySubresourceRegion([In] ID3D10Resource* pDstResource,[In] int DstSubresource,[In] int DstX,[In] int DstY,[In] int DstZ,[In] ID3D10Resource* pSrcResource,[In] int SrcSubresource,[In, Optional] const D3D10_BOX* pSrcBox)</unmanaged>
        public void CopySubresourceRegion(SharpDX.Direct3D10.Resource source, int sourceSubresource, SharpDX.Direct3D10.ResourceRegion? sourceRegion, SharpDX.Direct3D10.Resource destination, int destinationSubResource, int dstX, int dstY, int dstZ)
        {
            CopySubresourceRegion_(destination, destinationSubResource, dstX, dstY, dstZ, source, sourceSubresource, sourceRegion);
        }

        /// <summary>	
        /// Copy a multisampled resource into a non-multisampled resource. This API is most useful when re-using the resulting render target of one render pass as an input to a second render pass.	
        /// </summary>	
        /// <remarks>	
        /// Both the source and destination resources must be the same {{resource type}} and have the same dimensions. The source and destination must have compatible formats. There are three scenarios for this:  ScenarioRequirements Source and destination are prestructured and typedBoth the source and destination must have identical formats and that format must be specified in the Format parameter. One resource is prestructured and typed and the other is prestructured and typelessThe typed resource must have a format that is compatible with the typeless resource (i.e. the typed resource is DXGI_FORMAT_R32_FLOAT and the typeless resource is DXGI_FORMAT_R32_TYPELESS). The format of the typed resource must be specified in the Format parameter. Source and destination are prestructured and typelessBoth the source and destination must have the same typeless format (i.e. both must have DXGI_FORMAT_R32_TYPELESS), and the Format parameter must specify a format that is compatible with the source and destination (i.e. if both are DXGI_FORMAT_R32_TYPELESS then DXGI_FORMAT_R32_FLOAT or DXGI_FORMAT_R32_UINT could be specified in the Format parameter).  ? 	
        /// </remarks>	
        /// <param name="source">Source resource. Must be multisampled. </param>
        /// <param name="sourceSubresource">The source subresource of the source resource. </param>
        /// <param name="destination">Destination resource. Must be a created with the <see cref="SharpDX.Direct3D10.ResourceUsage.Default"/> flag and be single-sampled. See <see cref="SharpDX.Direct3D10.Resource"/>. </param>
        /// <param name="destinationSubresource">A zero-based index, that identifies the destination subresource. See {{D3D10CalcSubresource}} for more details. </param>
        /// <param name="format">that indicates how the multisampled resource will be resolved to a single-sampled resource. See remarks. </param>
        /// <unmanaged>void ID3D10Device::ResolveSubresource([In] ID3D10Resource* pDstResource,[In] int DstSubresource,[In] ID3D10Resource* pSrcResource,[In] int SrcSubresource,[In] DXGI_FORMAT Format)</unmanaged>
        public void ResolveSubresource(SharpDX.Direct3D10.Resource source, int sourceSubresource, SharpDX.Direct3D10.Resource destination, int destinationSubresource, SharpDX.DXGI.Format format)
        {
            ResolveSubresource_(destination, destinationSubresource, source, sourceSubresource, format);
        }

        /// <summary>
        /// Copies data from the CPU to to a non-mappable subresource region.
        /// </summary>
        /// <typeparam name="T">Type of the data to upload</typeparam>
        /// <param name="data">A reference to the data to upload.</param>
        /// <param name="resource">The destination resource.</param>
        /// <param name="subresource">The destination subresource.</param>
        /// <param name="rowPitch">The row pitch.</param>
        /// <param name="depthPitch">The depth pitch.</param>
        public void UpdateSubresource<T>(ref T data, Resource resource, int subresource = 0, int rowPitch = 0, int depthPitch = 0) where T : struct
        {
            unsafe
            {
                UpdateSubresource(resource, subresource, null, (IntPtr)Interop.Fixed(ref data), rowPitch, depthPitch);
            }
        }

        /// <summary>
        /// Copies data from the CPU to to a non-mappable subresource region.
        /// </summary>
        /// <typeparam name="T">Type of the data to upload</typeparam>
        /// <param name="data">A reference to the data to upload.</param>
        /// <param name="resource">The destination resource.</param>
        /// <param name="subresource">The destination subresource.</param>
        /// <param name="rowPitch">The row pitch.</param>
        /// <param name="depthPitch">The depth pitch.</param>
        public void UpdateSubresource<T>(T[] data, Resource resource, int subresource = 0, int rowPitch = 0, int depthPitch = 0) where T : struct
        {
            unsafe
            {
                UpdateSubresource(resource, subresource, null, (IntPtr)Interop.Fixed(data), rowPitch, depthPitch);
            }
        }

        /// <summary>
        /// Copies data from the CPU to to a non-mappable subresource region.
        /// </summary>
        /// <param name="source">The source data.</param>
        /// <param name="resource">The destination resource.</param>
        /// <param name="subresource">The destination subresource.</param>
        public void UpdateSubresource(DataBox source, Resource resource, int subresource)
        {
            UpdateSubresource(resource, subresource, null, source.DataPointer, source.RowPitch, source.SlicePitch);
        }

        /// <summary>
        /// Copies data from the CPU to to a non-mappable subresource region.
        /// </summary>
        /// <param name="source">The source data.</param>
        /// <param name="resource">The destination resource.</param>
        /// <param name="subresource">The destination subresource.</param>
        /// <param name="region">The destination region within the resource.</param>
        public void UpdateSubresource(DataBox source, Resource resource, int subresource, ResourceRegion region)
        {
            UpdateSubresource(resource, subresource, region, source.DataPointer, source.RowPitch, source.SlicePitch);
        }

        /// <summary>	
        /// Get the flags used during the call to create the device with <see cref="SharpDX.Direct3D10.D3D10.CreateDevice"/>.	
        /// </summary>	
        /// <returns>A bitfield containing the flags used to create the device. See <see cref="SharpDX.Direct3D10.DeviceCreationFlags"/>. </returns>
        /// <unmanaged>int ID3D10Device::GetCreationFlags()</unmanaged>
        public DeviceCreationFlags CreationFlags
        {
            get
            {
                return (DeviceCreationFlags) GetCreationFlags();
            }
        }

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
                    Marshal.Release(namePtr);
                }
            }
        }
    }
}