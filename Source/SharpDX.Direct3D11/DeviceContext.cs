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
using System.Globalization;
using System.Runtime.InteropServices;

namespace SharpDX.Direct3D11
{
    public partial class DeviceContext
    {
        /// <summary>
        ///   Constructs a new deferred context <see cref = "T:SharpDX.Direct3D11.DeviceContext" />.
        /// </summary>
        /// <param name = "device">The device with which to associate the state object.</param>
        /// <returns>The newly created object.</returns>
        public DeviceContext(Device device)
            : base(IntPtr.Zero)
        {
            device.CreateDeferredContext(0, this);

            // Add a reference to the device
            ((IUnknown)device).AddReference();
            Device__ = device;
        }

        /// <summary>
        ///   Create a command list and record graphics commands into it.
        /// </summary>
        /// <param name = "restoreState">A flag indicating whether the immediate context state is saved (prior) and restored (after) the execution of a command list.</param>
        /// <returns>The created command list containing the queued rendering commands.</returns>
        public CommandList FinishCommandList(bool restoreState)
        {
            CommandList temp;
            FinishCommandListInternal(restoreState, out temp);
            return temp;
        }

        /// <summary>
        /// Determines whether asynchronous query data is available.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>
        ///   <c>true</c> if asynchronous query data is available; otherwise, <c>false</c>.
        /// </returns>
        /// <unmanaged>HRESULT ID3D11DeviceContext::GetData([In] ID3D11Asynchronous* pAsync,[Out, Buffer, Optional] void* pData,[In] unsigned int DataSize,[In] D3D11_ASYNC_GETDATA_FLAG GetDataFlags)</unmanaged>
        public bool IsDataAvailable(Asynchronous data)
        {
            return IsDataAvailable( data, AsynchronousFlags.None );
        }

        /// <summary>
        /// Determines whether asynchronous query data is available.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="flags">Optional flags</param>
        /// <returns>
        ///   <c>true</c> if asynchronous query data is available; otherwise, <c>false</c>.
        /// </returns>
        /// <unmanaged>HRESULT ID3D11DeviceContext::GetData([In] ID3D11Asynchronous* pAsync,[Out, Buffer, Optional] void* pData,[In] unsigned int DataSize,[In] D3D11_ASYNC_GETDATA_FLAG GetDataFlags)</unmanaged>
        public bool IsDataAvailable(Asynchronous data, AsynchronousFlags flags)
        {
            return GetDataInternal(data, IntPtr.Zero, 0, flags) == Result.Ok;
        }

        /// <summary>
        ///   Gets data from the GPU asynchronously.
        /// </summary>
        /// <param name = "data">The asynchronous data provider.</param>
        /// <returns>The data retrieved from the GPU.</returns>
        public DataStream GetData(Asynchronous data)
        {
            return GetData(data, AsynchronousFlags.None);
        }

        /// <summary>
        ///   Gets data from the GPU asynchronously.
        /// </summary>
        /// <param name = "data">The asynchronous data provider.</param>
        /// <returns>The data retrieved from the GPU.</returns>
        public T GetData<T>(Asynchronous data) where T : struct
        {
            return GetData<T>(data, AsynchronousFlags.None);
        }

        /// <summary>
        /// Gets data from the GPU asynchronously.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The asynchronous data provider.</param>
        /// <param name="result">The data retrieved from the GPU.</param>
        /// <returns>
        /// True if result contains valid data, false otherwise.
        /// </returns>
        public bool GetData<T>(Asynchronous data, out T result) where T : struct
        {
            return GetData(data, AsynchronousFlags.None, out result);
        }

        /// <summary>
        ///   Gets data from the GPU asynchronously.
        /// </summary>
        /// <param name = "data">The asynchronous data provider.</param>
        /// <param name = "flags">Flags specifying how the command should operate.</param>
        /// <returns>The data retrieved from the GPU.</returns>
        public DataStream GetData(Asynchronous data, AsynchronousFlags flags)
        {
            var result = new DataStream(data.DataSize, true, true);
            GetDataInternal(data, result.DataPointer, (int) result.Length, flags);
            return result;
        }

        /// <summary>
        ///   Gets data from the GPU asynchronously.
        /// </summary>
        /// <param name = "data">The asynchronous data provider.</param>
        /// <param name = "flags">Flags specifying how the command should operate.</param>
        /// <returns>The data retrieved from the GPU.</returns>
        public T GetData<T>(Asynchronous data, AsynchronousFlags flags) where T : struct
        {
            T result;
            GetData<T>(data, flags, out result);
            return result;
        }

        /// <summary>
        /// Gets data from the GPU asynchronously.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The asynchronous data provider.</param>
        /// <param name="flags">Flags specifying how the command should operate.</param>
        /// <param name="result">The data retrieved from the GPU.</param>
        /// <returns>
        /// True if result contains valid data, false otherwise.
        /// </returns>
        public bool GetData<T>(Asynchronous data, AsynchronousFlags flags, out T result) where T : struct
        {
            unsafe
            {
                result = default(T);
                return GetDataInternal(data, (IntPtr)Interop.Fixed(ref result), Utilities.SizeOf<T>(), flags) == Result.Ok;
            }
        }


        /// <summary>	
        /// Copy the entire contents of the source resource to the destination resource using the GPU. 	
        /// </summary>	
        /// <remarks>	
        /// This method is unusual in that it causes the GPU to perform the copy operation (similar to a memcpy by the CPU). As a result, it has a few restrictions designed for improving performance. For instance, the source and destination resources:  Must be different resources. Must be the same type. Must have identical dimensions (including width, height, depth, and size as appropriate). Will only be copied. CopyResource does not support any stretch, color key, blend, or format conversions. Must have compatible DXGI formats, which means the formats must be identical or at least from the same type group. For example, a DXGI_FORMAT_R32G32B32_FLOAT texture can be copied to an DXGI_FORMAT_R32G32B32_UINT texture since both of these formats are in the DXGI_FORMAT_R32G32B32_TYPELESS group. Might not be currently mapped.  You cannot use an {{Immutable}} resource as a destination. You can use a   {{depth-stencil}} resource as either a source or a destination.  Resources created with multisampling capability (see <see cref="SharpDX.DXGI.SampleDescription"/>) can be used as source and destination only if both source and destination have identical multisampled count and quality. If source and destination differ in multisampled count and quality or if one is multisampled and the other is not multisampled the call to ID3D11DeviceContext::CopyResource fails. The method is an asynchronous call which may be added to the command-buffer queue. This attempts to remove pipeline stalls that may occur when copying data.  An application that only needs to copy a portion of the data in a resource should use <see cref="SharpDX.Direct3D11.DeviceContext.CopySubresourceRegion_"/> instead. 	
        /// </remarks>	
        /// <param name="source">A reference to the source resource (see <see cref="SharpDX.Direct3D11.Resource"/>). </param>
        /// <param name="destination">A reference to the destination resource (see <see cref="SharpDX.Direct3D11.Resource"/>). </param>
        /// <unmanaged>void ID3D11DeviceContext::CopyResource([In] ID3D11Resource* pDstResource,[In] ID3D11Resource* pSrcResource)</unmanaged>
        /// <msdn-id>ff476392</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::CopyResource([In] ID3D11Resource* pDstResource,[In] ID3D11Resource* pSrcResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::CopyResource</unmanaged-short>	
        public void CopyResource(Resource source, Resource destination)
        {
            CopyResource_(destination, source);
        }

        /// <summary>	
        /// Copy a region from a source resource to a destination resource.	
        /// </summary>	
        /// <remarks>	
        /// The source box must be within the size of the source resource. The destination offsets, (x, y, and z) allow the source box to be offset when writing into the destination resource; however, the dimensions of the source box and the offsets must be within the size of the resource. If the resources are buffers, all coordinates are in bytes; if the resources are textures, all coordinates are in texels. {{D3D11CalcSubresource}} is a helper function for calculating subresource indexes. CopySubresourceRegion performs the copy on the GPU (similar to a memcpy by the CPU). As a consequence, the source and destination resources:  Must be different subresources (although they can be from the same resource). Must be the same type. Must have compatible DXGI formats (identical or from the same type group). For example, a DXGI_FORMAT_R32G32B32_FLOAT texture can be copied to an DXGI_FORMAT_R32G32B32_UINT texture since both of these formats are in the DXGI_FORMAT_R32G32B32_TYPELESS group. May not be currently mapped.  CopySubresourceRegion only supports copy; it does not support any stretch, color key, blend, or format conversions. An application that needs to copy an entire resource should use <see cref="SharpDX.Direct3D11.DeviceContext.CopyResource_"/> instead. CopySubresourceRegion is an asynchronous call which may be added to the command-buffer queue, this attempts to remove pipeline stalls that may occur when copying data. See performance considerations for more details. Note??If you use CopySubresourceRegion with a depth-stencil buffer or a multisampled resource, you must copy the whole subresource. In this situation, you must pass 0 to the DstX, DstY, and DstZ parameters and NULL to the pSrcBox parameter. In addition, source and destination resources, which are represented by the pSrcResource and pDstResource parameters, should have identical sample count values. Example The following code snippet copies a box (located at (120,100),(200,220)) from a source texture into a region (10,20),(90,140) in a destination texture. 	
        /// <code> D3D11_BOX sourceRegion;	
        /// sourceRegion.left = 120;	
        /// sourceRegion.right = 200;	
        /// sourceRegion.top = 100;	
        /// sourceRegion.bottom = 220;	
        /// sourceRegion.front = 0;	
        /// sourceRegion.back = 1; pd3dDeviceContext-&gt;CopySubresourceRegion( pDestTexture, 0, 10, 20, 0, pSourceTexture, 0, &amp;sourceRegion ); </code>	
        /// 	
        ///  Notice, that for a 2D texture, front and back are set to 0 and 1 respectively. 	
        /// </remarks>	
        /// <param name="source">A reference to the source resource (see <see cref="SharpDX.Direct3D11.Resource"/>). </param>
        /// <param name="sourceSubresource">Source subresource index. </param>
        /// <param name="sourceRegion">A reference to a 3D box (see <see cref="SharpDX.Direct3D11.ResourceRegion"/>) that defines the source subresources that can be copied. If NULL, the entire source subresource is copied. The box must fit within the source resource. </param>
        /// <param name="destination">A reference to the destination resource (see <see cref="SharpDX.Direct3D11.Resource"/>). </param>
        /// <param name="destinationSubResource">Destination subresource index. </param>
        /// <param name="dstX">The x-coordinate of the upper left corner of the destination region. </param>
        /// <param name="dstY">The y-coordinate of the upper left corner of the destination region. For a 1D subresource, this must be zero. </param>
        /// <param name="dstZ">The z-coordinate of the upper left corner of the destination region. For a 1D or 2D subresource, this must be zero. </param>
        /// <msdn-id>ff476394</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::CopySubresourceRegion([In] ID3D11Resource* pDstResource,[In] unsigned int DstSubresource,[In] unsigned int DstX,[In] unsigned int DstY,[In] unsigned int DstZ,[In] ID3D11Resource* pSrcResource,[In] unsigned int SrcSubresource,[In, Optional] const D3D11_BOX* pSrcBox)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::CopySubresourceRegion</unmanaged-short>	
        public void CopySubresourceRegion(SharpDX.Direct3D11.Resource source, int sourceSubresource, SharpDX.Direct3D11.ResourceRegion? sourceRegion, SharpDX.Direct3D11.Resource destination, int destinationSubResource, int dstX = 0, int dstY = 0, int dstZ = 0)
        {
            CopySubresourceRegion_(destination, destinationSubResource, dstX, dstY, dstZ, source, sourceSubresource, sourceRegion);
        }

        /// <summary>	
        /// Copy a multisampled resource into a non-multisampled resource.	
        /// </summary>	
        /// <remarks>	
        /// This API is most useful when re-using the resulting render target of one render pass as an input to a second render pass. The source and destination resources must be the same resource type and have the same dimensions. In addition, they must have compatible formats. There are three scenarios for this:  ScenarioRequirements Source and destination are prestructured and typedBoth the source and destination must have identical formats and that format must be specified in the Format parameter. One resource is prestructured and typed and the other is prestructured and typelessThe typed resource must have a format that is compatible with the typeless resource (i.e. the typed resource is DXGI_FORMAT_R32_FLOAT and the typeless resource is DXGI_FORMAT_R32_TYPELESS). The format of the typed resource must be specified in the Format parameter. Source and destination are prestructured and typelessBoth the source and destination must have the same typeless format (i.e. both must have DXGI_FORMAT_R32_TYPELESS), and the Format parameter must specify a format that is compatible with the source and destination (i.e. if both are DXGI_FORMAT_R32_TYPELESS then DXGI_FORMAT_R32_FLOAT could be specified in the Format parameter). For example, given the DXGI_FORMAT_R16G16B16A16_TYPELESS format:  The source (or dest) format could be DXGI_FORMAT_R16G16B16A16_UNORM The dest (or source) format could be DXGI_FORMAT_R16G16B16A16_FLOAT    ? 	
        /// </remarks>	
        /// <param name="source">Source resource. Must be multisampled. </param>
        /// <param name="sourceSubresource">&gt;The source subresource of the source resource. </param>
        /// <param name="destination">Destination resource. Must be a created with the <see cref="SharpDX.Direct3D11.ResourceUsage.Default"/> flag and be single-sampled. See <see cref="SharpDX.Direct3D11.Resource"/>. </param>
        /// <param name="destinationSubresource">A zero-based index, that identifies the destination subresource. Use {{D3D11CalcSubresource}} to calculate the index. </param>
        /// <param name="format">A <see cref="SharpDX.DXGI.Format"/> that indicates how the multisampled resource will be resolved to a single-sampled resource.  See remarks. </param>
        /// <unmanaged>void ID3D11DeviceContext::ResolveSubresource([In] ID3D11Resource* pDstResource,[In] int DstSubresource,[In] ID3D11Resource* pSrcResource,[In] int SrcSubresource,[In] DXGI_FORMAT Format)</unmanaged>
        public void ResolveSubresource(SharpDX.Direct3D11.Resource source, int sourceSubresource, SharpDX.Direct3D11.Resource destination, int destinationSubresource, SharpDX.DXGI.Format format)
        {
            ResolveSubresource_(destination, destinationSubresource, source, sourceSubresource, format);
        }

        /// <summary>
        /// Maps the data contained in a subresource to a memory pointer, and denies the GPU access to that subresource.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="mipSlice">The mip slice.</param>
        /// <param name="arraySlice">The array slice.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="stream">The output stream containing the pointer.</param>
        /// <returns>
        /// The locked <see cref="SharpDX.DataBox"/>
        /// </returns>
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        public DataBox MapSubresource(Texture1D resource, int mipSlice, int arraySlice, MapMode mode, MapFlags flags, out DataStream stream)
        {
            int mipSize;
            var box = MapSubresource(resource, mipSlice, arraySlice, mode, flags, out mipSize);
            stream = new DataStream(box.DataPointer, mipSize * (int)DXGI.FormatHelper.SizeOfInBytes(resource.Description.Format), true, true);
            return box;
        }

        /// <summary>
        /// Maps the data contained in a subresource to a memory pointer, and denies the GPU access to that subresource.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="mipSlice">The mip slice.</param>
        /// <param name="arraySlice">The array slice.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="stream">The output stream containing the pointer.</param>
        /// <returns>
        /// The locked <see cref="SharpDX.DataBox"/>
        /// </returns>
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        public DataBox MapSubresource(Texture2D resource, int mipSlice, int arraySlice, MapMode mode, MapFlags flags, out DataStream stream)
        {
            int mipSize;
            var box = MapSubresource(resource, mipSlice, arraySlice, mode, flags, out mipSize);
            stream = new DataStream(box.DataPointer, mipSize * box.RowPitch, true, true);
            return box;
        }

        /// <summary>
        /// Maps the data contained in a subresource to a memory pointer, and denies the GPU access to that subresource.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="mipSlice">The mip slice.</param>
        /// <param name="arraySlice">The array slice.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="stream">The output stream containing the pointer.</param>
        /// <returns>
        /// The locked <see cref="SharpDX.DataBox"/>
        /// </returns>
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        public DataBox MapSubresource(Texture3D resource, int mipSlice, int arraySlice, MapMode mode, MapFlags flags, out DataStream stream)
        {
            int mipSize;
            var box = MapSubresource(resource, mipSlice, arraySlice, mode, flags, out mipSize);
            stream = new DataStream(box.DataPointer, mipSize * box.SlicePitch, true, true);
            return box;
        }

        /// <summary>
        /// Maps the data contained in a subresource to a memory pointer, and denies the GPU access to that subresource.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="stream">The output stream containing the pointer.</param>
        /// <returns>
        /// The locked <see cref="SharpDX.DataBox"/>
        /// </returns>
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        public DataBox MapSubresource(Buffer resource, MapMode mode, MapFlags flags, out DataStream stream)
        {
            var box = MapSubresource(resource, 0, mode, flags);
            stream = new DataStream(box.DataPointer, resource.Description.SizeInBytes, true, true);
            return box;
        }

        /// <summary>
        /// Maps the data contained in a subresource to a memory pointer, and denies the GPU access to that subresource.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="mipSlice">The mip slice.</param>
        /// <param name="arraySlice">The array slice.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="mipSize">Size of the selected miplevel.</param>
        /// <returns>
        /// The locked <see cref="SharpDX.DataBox"/>
        /// </returns>
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        public DataBox MapSubresource(Resource resource, int mipSlice, int arraySlice, MapMode mode, MapFlags flags, out int mipSize)
        {
            int subresource = resource.CalculateSubResourceIndex(mipSlice, arraySlice, out mipSize);
            var box = MapSubresource(resource, subresource, mode, flags);
            return box;
        }
        
        /// <summary>
        /// Maps the data contained in a subresource to a memory pointer, and denies the GPU access to that subresource.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="subresource">The subresource.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="stream">The output stream containing the pointer.</param>
        /// <returns>The locked <see cref="SharpDX.DataBox"/></returns>
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        public SharpDX.DataBox MapSubresource(SharpDX.Direct3D11.Resource resource, int subresource, SharpDX.Direct3D11.MapMode mode, SharpDX.Direct3D11.MapFlags flags, out DataStream stream)
        {
            int mipLevels;
            switch (resource.Dimension)
            {
                case ResourceDimension.Buffer:
                    return MapSubresource((Buffer)resource, mode, flags, out stream);
                case ResourceDimension.Texture1D:
                    var texture1D = (Texture1D)resource;
                    mipLevels = texture1D.Description.MipLevels;
                    return MapSubresource(texture1D, subresource % mipLevels, subresource / mipLevels, mode, flags, out stream);
                case ResourceDimension.Texture2D:
                    var texture2D = (Texture2D)resource;
                    mipLevels = texture2D.Description.MipLevels;
                    return MapSubresource(texture2D, subresource % mipLevels, subresource / mipLevels, mode, flags, out stream);
                case ResourceDimension.Texture3D:
                    var texture3D = (Texture3D)resource;
                    mipLevels = texture3D.Description.MipLevels;
                    return MapSubresource(texture3D, subresource % mipLevels, subresource / mipLevels, mode, flags, out stream);
                default:
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "MapSubresource is not supported for Resource [{0}]", resource.Dimension));
            }
        }

        /// <summary>	
        /// <p>Gets a reference to the data contained in a subresource, and denies the GPU access to that subresource.</p>	
        /// </summary>	
        /// <param name="resourceRef"><dd>  <p>A reference to a <strong><see cref="SharpDX.Direct3D11.Resource"/></strong> interface.</p> </dd></param>	
        /// <param name="subresource"><dd>  <p>Index number of the subresource.</p> </dd></param>	
        /// <param name="mapType"><dd>  <p>Specifies the CPU's read and write permissions for a resource. For possible values, see <strong><see cref="SharpDX.Direct3D11.MapMode"/></strong>.</p> </dd></param>	
        /// <param name="mapFlags"><dd>  <p> <strong>Flag</strong> that specifies what the CPU should do when the GPU is busy. This flag is optional.</p> </dd></param>	
        /// <param name="mappedResourceRef"><dd>  <p>A reference to the mapped subresource (see <strong><see cref="SharpDX.DataBox"/></strong>).</p> </dd></param>	
        /// <returns>The mapped subresource (see <strong><see cref="SharpDX.DataBox"/></strong>). If <see cref="MapFlags.DoNotWait"/> is used and the resource is still being used by the GPU, this method return an empty DataBox whose property <see cref="DataBox.IsEmpty"/> returns <c>true</c>.<p>This method also throws an exception with the code <strong><see cref="SharpDX.DXGI.ResultCode.DeviceRemoved"/></strong> if <em>MapType</em> allows any CPU read access and the video card has been removed.</p><p>For more information about these error codes, see DXGI_ERROR.</p></returns>	
        /// <remarks>	
        /// <p>If you call <strong>Map</strong> on a deferred context, you can only pass <strong><see cref="SharpDX.Direct3D11.MapMode.WriteDiscard"/></strong>, <strong><see cref="SharpDX.Direct3D11.MapMode.WriteNoOverwrite"/></strong>, or both to the <em>MapType</em> parameter. Other <strong><see cref="SharpDX.Direct3D11.MapMode"/></strong>-typed values are not supported for a deferred context.</p><p>The Direct3D 11.1 runtime, which is available starting with Windows Developer Preview, can  map shader resource views (SRVs) of dynamic buffers with <strong><see cref="SharpDX.Direct3D11.MapMode.WriteNoOverwrite"/></strong>.  The Direct3D 11 and earlier runtimes limited mapping to vertex or index buffers.</p>	
        /// If <see cref="MapFlags.DoNotWait"/> is used and the resource is still being used by the GPU, this method return an empty DataBox whose property <see cref="DataBox.IsEmpty"/> returns <c>true</c>.
        /// </remarks>	
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        public SharpDX.DataBox MapSubresource(SharpDX.Direct3D11.Resource resourceRef, int subresource, SharpDX.Direct3D11.MapMode mapType, SharpDX.Direct3D11.MapFlags mapFlags)
        {
            var box = default(DataBox);
            var result = MapSubresource(resourceRef, subresource, mapType, mapFlags, out box);
            if((mapFlags & MapFlags.DoNotWait) != 0 && result == DXGI.ResultCode.WasStillDrawing)
            {
                return box;
            }
            result.CheckError();
            return box;
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
        /// <param name="region">The region</param>
        /// <remarks>
        /// This method is implementing the <a href="http://blogs.msdn.com/b/chuckw/archive/2010/07/28/known-issue-direct3d-11-updatesubresource-and-deferred-contexts.aspx">workaround for deferred context</a>.
        /// </remarks>
        /// <msdn-id>ff476486</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::UpdateSubresource([In] ID3D11Resource* pDstResource,[In] unsigned int DstSubresource,[In, Optional] const D3D11_BOX* pDstBox,[In] const void* pSrcData,[In] unsigned int SrcRowPitch,[In] unsigned int SrcDepthPitch)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::UpdateSubresource</unmanaged-short>	
        public void UpdateSubresource<T>(ref T data, Resource resource, int subresource = 0, int rowPitch = 0, int depthPitch = 0, ResourceRegion? region = null) where T : struct
        {
            unsafe
            {
                UpdateSubresource(resource, subresource, region, (IntPtr)Interop.Fixed(ref data), rowPitch, depthPitch);
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
        /// <param name="region">A region that defines the portion of the destination subresource to copy the resource data into. Coordinates are in bytes for buffers and in texels for textures.</param>
        /// <msdn-id>ff476486</msdn-id>
        ///   <unmanaged>void ID3D11DeviceContext::UpdateSubresource([In] ID3D11Resource* pDstResource,[In] unsigned int DstSubresource,[In, Optional] const D3D11_BOX* pDstBox,[In] const void* pSrcData,[In] unsigned int SrcRowPitch,[In] unsigned int SrcDepthPitch)</unmanaged>
        ///   <unmanaged-short>ID3D11DeviceContext::UpdateSubresource</unmanaged-short>
        /// <remarks>This method is implementing the <a href="http://blogs.msdn.com/b/chuckw/archive/2010/07/28/known-issue-direct3d-11-updatesubresource-and-deferred-contexts.aspx">workaround for deferred context</a>.</remarks>
        public void UpdateSubresource<T>(T[] data, Resource resource, int subresource = 0, int rowPitch = 0, int depthPitch = 0, ResourceRegion? region = null) where T : struct
        {
            unsafe
            {
                UpdateSubresource(resource, subresource, region, (IntPtr)Interop.Fixed(data), rowPitch, depthPitch);
            }
        }
        
        /// <summary>
        ///   Copies data from the CPU to to a non-mappable subresource region.
        /// </summary>
        /// <param name = "source">The source data.</param>
        /// <param name = "resource">The destination resource.</param>
        /// <param name = "subresource">The destination subresource.</param>
        /// <remarks>
        /// This method is implementing the <a href="http://blogs.msdn.com/b/chuckw/archive/2010/07/28/known-issue-direct3d-11-updatesubresource-and-deferred-contexts.aspx">workaround for deferred context</a>.
        /// </remarks>
        /// <unmanaged>void ID3D11DeviceContext::UpdateSubresource([In] ID3D11Resource* pDstResource,[In] unsigned int DstSubresource,[In, Optional] const D3D11_BOX* pDstBox,[In] const void* pSrcData,[In] unsigned int SrcRowPitch,[In] unsigned int SrcDepthPitch)</unmanaged>	
        public void UpdateSubresource(DataBox source, Resource resource, int subresource = 0)
        {
            UpdateSubresource(resource, subresource, null, source.DataPointer, source.RowPitch, source.SlicePitch);
        }

        /// <summary>
        ///   Copies data from the CPU to to a non-mappable subresource region.
        /// </summary>
        /// <param name = "source">The source data.</param>
        /// <param name = "resource">The destination resource.</param>
        /// <param name = "subresource">The destination subresource.</param>
        /// <param name = "region">The destination region within the resource.</param>
        /// <remarks>
        /// This method is implementing the <a href="http://blogs.msdn.com/b/chuckw/archive/2010/07/28/known-issue-direct3d-11-updatesubresource-and-deferred-contexts.aspx">workaround for deferred context</a>.
        /// </remarks>
        /// <unmanaged>void ID3D11DeviceContext::UpdateSubresource([In] ID3D11Resource* pDstResource,[In] unsigned int DstSubresource,[In, Optional] const D3D11_BOX* pDstBox,[In] const void* pSrcData,[In] unsigned int SrcRowPitch,[In] unsigned int SrcDepthPitch)</unmanaged>	
        public void UpdateSubresource(DataBox source, Resource resource, int subresource, ResourceRegion region)
        {
            UpdateSubresource(resource, subresource, region, source.DataPointer, source.RowPitch, source.SlicePitch);
        }

        /// <summary>
        /// Copies data from the CPU to to a non-mappable subresource region.
        /// </summary>
        /// <typeparam name="T">Type of the data to upload</typeparam>
        /// <param name="data">A reference to the data to upload.</param>
        /// <param name="resource">The destination resource.</param>
        /// <param name="srcBytesPerElement">The size in bytes per pixel/block element.</param>
        /// <param name="subresource">The destination subresource.</param>
        /// <param name="rowPitch">The row pitch.</param>
        /// <param name="depthPitch">The depth pitch.</param>
        /// <param name="isCompressedResource">if set to <c>true</c> the resource is a block/compressed resource</param>
        /// <unmanaged>void ID3D11DeviceContext::UpdateSubresource([In] ID3D11Resource* pDstResource,[In] unsigned int DstSubresource,[In, Optional] const D3D11_BOX* pDstBox,[In] const void* pSrcData,[In] unsigned int SrcRowPitch,[In] unsigned int SrcDepthPitch)</unmanaged>
        /// <remarks>
        /// This method is implementing the <a href="http://blogs.msdn.com/b/chuckw/archive/2010/07/28/known-issue-direct3d-11-updatesubresource-and-deferred-contexts.aspx">workaround for deferred context</a>.
        /// </remarks>
        public void UpdateSubresourceSafe<T>(ref T data, Resource resource, int srcBytesPerElement, int subresource = 0, int rowPitch = 0, int depthPitch = 0, bool isCompressedResource = false) where T : struct
        {
            unsafe
            {
                UpdateSubresourceSafe(resource, subresource, null, (IntPtr)Interop.Fixed(ref data), rowPitch, depthPitch, srcBytesPerElement, isCompressedResource);
            }
        }

        /// <summary>
        /// Copies data from the CPU to to a non-mappable subresource region.
        /// </summary>
        /// <typeparam name="T">Type of the data to upload</typeparam>
        /// <param name="data">A reference to the data to upload.</param>
        /// <param name="resource">The destination resource.</param>
        /// <param name="srcBytesPerElement">The size in bytes per pixel/block element.</param>
        /// <param name="subresource">The destination subresource.</param>
        /// <param name="rowPitch">The row pitch.</param>
        /// <param name="depthPitch">The depth pitch.</param>
        /// <param name="isCompressedResource">if set to <c>true</c> the resource is a block/compressed resource</param>
        /// <remarks>
        /// This method is implementing the <a href="http://blogs.msdn.com/b/chuckw/archive/2010/07/28/known-issue-direct3d-11-updatesubresource-and-deferred-contexts.aspx">workaround for deferred context</a>.
        /// </remarks>
        /// <unmanaged>void ID3D11DeviceContext::UpdateSubresource([In] ID3D11Resource* pDstResource,[In] unsigned int DstSubresource,[In, Optional] const D3D11_BOX* pDstBox,[In] const void* pSrcData,[In] unsigned int SrcRowPitch,[In] unsigned int SrcDepthPitch)</unmanaged>	
        public void UpdateSubresourceSafe<T>(T[] data, Resource resource, int srcBytesPerElement, int subresource = 0, int rowPitch = 0, int depthPitch = 0, bool isCompressedResource = false) where T : struct
        {
            unsafe
            {
                UpdateSubresourceSafe(resource, subresource, null, (IntPtr)Interop.Fixed(data), rowPitch, depthPitch, srcBytesPerElement, isCompressedResource);
            }
        }

        /// <summary>
        ///   Copies data from the CPU to to a non-mappable subresource region.
        /// </summary>
        /// <param name = "source">The source data.</param>
        /// <param name = "resource">The destination resource.</param>
        /// <param name="srcBytesPerElement">The size in bytes per pixel/block element.</param>
        /// <param name = "subresource">The destination subresource.</param>
        /// <param name="isCompressedResource">if set to <c>true</c> the resource is a block/compressed resource</param>
        /// <remarks>
        /// This method is implementing the <a href="http://blogs.msdn.com/b/chuckw/archive/2010/07/28/known-issue-direct3d-11-updatesubresource-and-deferred-contexts.aspx">workaround for deferred context</a>.
        /// </remarks>
        /// <unmanaged>void ID3D11DeviceContext::UpdateSubresource([In] ID3D11Resource* pDstResource,[In] unsigned int DstSubresource,[In, Optional] const D3D11_BOX* pDstBox,[In] const void* pSrcData,[In] unsigned int SrcRowPitch,[In] unsigned int SrcDepthPitch)</unmanaged>	
        public void UpdateSubresourceSafe(DataBox source, Resource resource, int srcBytesPerElement, int subresource = 0, bool isCompressedResource = false)
        {
            UpdateSubresourceSafe(resource, subresource, null, source.DataPointer, source.RowPitch, source.SlicePitch, srcBytesPerElement, isCompressedResource);
        }

        /// <summary>
        ///   Copies data from the CPU to to a non-mappable subresource region.
        /// </summary>
        /// <param name = "source">The source data.</param>
        /// <param name = "resource">The destination resource.</param>
        /// <param name="srcBytesPerElement">The size in bytes per pixel/block element.</param>
        /// <param name = "subresource">The destination subresource.</param>
        /// <param name = "region">The destination region within the resource.</param>
        /// <param name="isCompressedResource">if set to <c>true</c> the resource is a block/compressed resource</param>
        /// <remarks>
        /// This method is implementing the <a href="http://blogs.msdn.com/b/chuckw/archive/2010/07/28/known-issue-direct3d-11-updatesubresource-and-deferred-contexts.aspx">workaround for deferred context</a>.
        /// </remarks>
        /// <unmanaged>void ID3D11DeviceContext::UpdateSubresource([In] ID3D11Resource* pDstResource,[In] unsigned int DstSubresource,[In, Optional] const D3D11_BOX* pDstBox,[In] const void* pSrcData,[In] unsigned int SrcRowPitch,[In] unsigned int SrcDepthPitch)</unmanaged>	
        public void UpdateSubresourceSafe(DataBox source, Resource resource, int srcBytesPerElement, int subresource, ResourceRegion region, bool isCompressedResource = false)
        {
            UpdateSubresourceSafe(resource, subresource, region, source.DataPointer, source.RowPitch, source.SlicePitch, srcBytesPerElement, isCompressedResource);
        }

        /// <summary>
        /// Updates the subresource safe method.
        /// </summary>
        /// <param name="dstResourceRef">The DST resource ref.</param>
        /// <param name="dstSubresource">The DST subresource.</param>
        /// <param name="dstBoxRef">The DST box ref.</param>
        /// <param name="pSrcData">The p SRC data.</param>
        /// <param name="srcRowPitch">The SRC row pitch.</param>
        /// <param name="srcDepthPitch">The SRC depth pitch.</param>
        /// <param name="srcBytesPerElement">The size in bytes per pixel/block element.</param>
        /// <param name="isCompressedResource">if set to <c>true</c> the resource is a block/compressed resource</param>
        /// <returns></returns>
        /// <remarks>
        /// This method is implementing the <a href="http://blogs.msdn.com/b/chuckw/archive/2010/07/28/known-issue-direct3d-11-updatesubresource-and-deferred-contexts.aspx">workaround for deferred context</a>.
        /// </remarks>
        internal unsafe bool UpdateSubresourceSafe(SharpDX.Direct3D11.Resource dstResourceRef, int dstSubresource, SharpDX.Direct3D11.ResourceRegion? dstBoxRef, System.IntPtr pSrcData, int srcRowPitch, int srcDepthPitch, int srcBytesPerElement, bool isCompressedResource)
        {

            bool needWorkaround = false;

            // Check thread support just once as it won't change during the life of this instance.
            if (!isCheckThreadingSupport)
            {
                bool supportsConcurrentResources;
                Device.CheckThreadingSupport(out supportsConcurrentResources, out supportsCommandLists);
                isCheckThreadingSupport = true;
            }

            if ( dstBoxRef.HasValue)
            {
                if (TypeInfo == DeviceContextType.Deferred)
                {
                    // If this deferred context doesn't support command list, we need to perform the workaround
                    needWorkaround = !supportsCommandLists;
                }
            }

            // Adjust the pSrcData pointer if needed
            IntPtr pAdjustedSrcData = pSrcData;
            if( needWorkaround )
            {
                var alignedBox = dstBoxRef.Value;
		
                // convert from pixels to blocks
                if (isCompressedResource)
                {
                    alignedBox.Left     /= 4;
                    alignedBox.Right    /= 4;
                    alignedBox.Top      /= 4;
                    alignedBox.Bottom   /= 4;
                }

                pAdjustedSrcData = (IntPtr)(((byte*)pSrcData) - (alignedBox.Front * srcDepthPitch) - (alignedBox.Top * srcRowPitch) - (alignedBox.Left * srcBytesPerElement));
            }

            UpdateSubresource( dstResourceRef, dstSubresource, dstBoxRef, pAdjustedSrcData, srcRowPitch, srcDepthPitch );

            return needWorkaround;
        }

        private bool isCheckThreadingSupport;
        private bool supportsCommandLists;
    }
}