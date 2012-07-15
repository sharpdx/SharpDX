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
using SharpDX.DXGI;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using Device = SharpDX.Direct3D11.Device;
using MapFlags = SharpDX.Direct3D11.MapFlags;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// This class is a frontend to <see cref="SharpDX.Direct3D11.Device" /> and <see cref="SharpDX.Direct3D11.DeviceContext" />
    /// </summary>
    public class GraphicsDevice : Component
    {
        internal Device Device;
        internal DeviceContext Context;

        [ThreadStatic]
        private static GraphicsDevice current;

        /// <summary>
        /// Gets the features supported by this <see cref="GraphicsDevice"/>.
        /// </summary>
        public readonly GraphicsDeviceFeatures Features;

        /// <summary>
        /// Gets the <see cref="GraphicsDevice"/> for immediate rendering.
        /// </summary>
        public readonly GraphicsDevice MainDevice;

        /// <summary>
        /// Gets whether this <see cref="GraphicsDevice"/> is running in debug.
        /// </summary>
        public readonly bool IsDebugMode;

        /// <summary>
        /// Gets whether this <see cref="GraphicsDevice"/> is a deferred context.
        /// </summary>
        public readonly bool IsDeferred;

        // Current states

        private BlendState currentBlendState;
        private Color4 currentBlendFactor = Color.White;
        private int currentMultiSampleMask = -1;
        private DepthStencilState currentDepthStencilState;
        private int currentDepthStencilReference = 0;
        private RasterizerState currentRasterizerState;
        private PrimitiveTopology currentPrimitiveTopology;

        private readonly bool needWorkAroundForUpdateSubResource;

        protected GraphicsDevice(DriverType type, DeviceCreationFlags flags = DeviceCreationFlags.None, params FeatureLevel[] featureLevels)
        {
            Device = ToDispose(featureLevels.Length > 0 ? new Device(type, flags, featureLevels) : new Device(type, flags));
            IsDebugMode = (Device.CreationFlags & (int)DeviceCreationFlags.Debug) != 0;
            MainDevice = this;
            Context = Device.ImmediateContext;
            IsDeferred = false;
            Features = new GraphicsDeviceFeatures(Device);
            AttachToCurrentThread();
        }

        protected GraphicsDevice(GraphicsAdapter adapter, DeviceCreationFlags flags = DeviceCreationFlags.None, params FeatureLevel[] featureLevels)
        {
            Device = ToDispose(featureLevels.Length > 0 ? new Device(adapter, flags, featureLevels) : new Device(adapter, flags));
            IsDebugMode = (Device.CreationFlags & (int)DeviceCreationFlags.Debug) != 0;
            Adapter = adapter;
            MainDevice = this;
            Context = Device.ImmediateContext;
            IsDeferred = false;
            Features = new GraphicsDeviceFeatures(Device);
            AttachToCurrentThread();
        }

        protected GraphicsDevice(GraphicsDevice mainDevice, DeviceContext deferredContext)
        {
            Device = mainDevice.Device;
            IsDebugMode = (Device.CreationFlags & (int)DeviceCreationFlags.Debug) != 0;
            MainDevice = mainDevice;
            Context = deferredContext;
            IsDeferred = true;
            Features = mainDevice.Features;
            // Setup the workaround flag
            needWorkAroundForUpdateSubResource = IsDeferred && !Features.HasDriverCommandLists;
        }

        /// <summary>
        /// Gets the adapter associated with this device.
        /// </summary>
        public readonly GraphicsAdapter Adapter;

        /// <summary>
        /// Gets the <see cref="GraphicsDevice"/> attached to the current thread.
        /// </summary>
        public static GraphicsDevice Current
        {
            get { return current; }
        }

        /// <summary>
        /// Gets the <see cref="GraphicsDevice"/> attached to the current thread.
        /// </summary>
        internal static GraphicsDevice CurrentSafe
        {
            get
            {
                if (current == null) throw new InvalidOperationException("A GraphicsDevice is not initialized or not attached to the current thread.");
                return current;
            }
        }

        /// <summary>
        /// Gets or sets the current presenter use by the <see cref="Present"/> method.
        /// </summary>
        /// <value>The current presenter.</value>
        public GraphicsPresenter CurrentPresenter { get; set; }

        /// <summary>
        /// Clears a render target view by setting all the elements in a render target to one value.
        /// </summary>
        /// <param name="renderTargetView">The render target view.</param>
        /// <param name="colorRGBA">A 4-component array that represents the color to fill the render target with.</param>
        /// <remarks><p>Applications that wish to clear a render target to a specific integer value bit pattern should render a screen-aligned quad instead of using this method.  The reason for this is because this method accepts as input a floating point value, which may not have the same bit pattern as the original integer.</p><table> <tr><td> <p>Differences between Direct3D 9 and Direct3D 11/10:</p> <p>Unlike Direct3D 9, the full extent of the resource view is always cleared. Viewport and scissor settings are not applied.</p> </td></tr> </table><p>?</p></remarks>
        /// <msdn-id>ff476388</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::ClearRenderTargetView([In] ID3D11RenderTargetView* pRenderTargetView,[In] const SHARPDX_COLOR4* ColorRGBA)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::ClearRenderTargetView</unmanaged-short>
        public void Clear(SharpDX.Direct3D11.RenderTargetView renderTargetView, Color4 colorRGBA)
        {
            Context.ClearRenderTargetView(renderTargetView, colorRGBA);
        }

        /// <summary>	
        /// Clears the depth-stencil resource.
        /// </summary>	
        /// <param name="depthStencilView"><dd>  <p>Pointer to the depth stencil to be cleared.</p> </dd></param>	
        /// <param name="clearFlags"><dd>  <p>Identify the type of data to clear (see <strong><see cref="SharpDX.Direct3D11.DepthStencilClearFlags"/></strong>).</p> </dd></param>	
        /// <param name="depth"><dd>  <p>Clear the depth buffer with this value. This value will be clamped between 0 and 1.</p> </dd></param>	
        /// <param name="stencil"><dd>  <p>Clear the stencil buffer with this value.</p> </dd></param>	
        /// <remarks>	
        /// <table> <tr><td> <p>Differences between Direct3D 9 and Direct3D 11/10:</p> <p>Unlike Direct3D 9, the full extent of the resource view is always cleared. Viewport and scissor settings are not applied.</p> </td></tr> </table><p>?</p>	
        /// </remarks>	
        /// <msdn-id>ff476387</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::ClearDepthStencilView([In] ID3D11DepthStencilView* pDepthStencilView,[In] D3D11_CLEAR_FLAG ClearFlags,[In] float Depth,[In] unsigned char Stencil)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::ClearDepthStencilView</unmanaged-short>	
        public void Clear(SharpDX.Direct3D11.DepthStencilView depthStencilView, SharpDX.Direct3D11.DepthStencilClearFlags clearFlags, float depth, byte stencil)
        {
            Context.ClearDepthStencilView(depthStencilView, clearFlags, depth, stencil);
        }

        /// <summary>	
        /// Clears an unordered access resource with bit-precise values.	
        /// </summary>	
        /// <param name="view">The buffer to clear.</param>	
        /// <param name="value">The value used to clear.</param>	
        /// <remarks>	
        /// <p>This API copies the lower ni bits from each array element i to the corresponding channel, where ni is the number of bits in  the ith channel of the resource format (for example, R8G8B8_FLOAT has 8 bits for the first 3 channels). This works on any UAV with no format conversion.  For a raw or structured buffer view, only the first array element value is used.</p>	
        /// </remarks>	
        /// <msdn-id>ff476391</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::ClearUnorderedAccessViewUint([In] ID3D11UnorderedAccessView* pUnorderedAccessView,[In] const unsigned int* Values)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::ClearUnorderedAccessViewUint</unmanaged-short>	
        public void Clear(UnorderedAccessView view, int value)
        {
            Context.ClearUnorderedAccessView(view, value);
        }

        /// <summary>	
        /// Clears an unordered access resource with a float value.
        /// </summary>	
        /// <param name="view">The buffer to clear.</param>	
        /// <param name="value">The value used to clear.</param>	
        /// <remarks>	
        /// <p>This API works on FLOAT, UNORM, and SNORM unordered access views (UAVs), with format conversion from FLOAT to *NORM where appropriate. On other UAVs, the operation is invalid and the call will not reach the driver.</p>	
        /// </remarks>	
        /// <msdn-id>ff476390</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::ClearUnorderedAccessViewFloat([In] ID3D11UnorderedAccessView* pUnorderedAccessView,[In] const float* Values)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::ClearUnorderedAccessViewFloat</unmanaged-short>	
        public void Clear(UnorderedAccessView view, float value)
        {
            Context.ClearUnorderedAccessView(view, value);
        }

        /// <summary>
        /// Copies the content of this resource to another <see cref="GraphicsResource" />.
        /// </summary>
        /// <param name="fromResource">The resource to copy from.</param>
        /// <param name="toResource">The resource to copy to.</param>
        /// <remarks>See the unmanaged documentation for usage and restrictions.</remarks>
        /// <msdn-id>ff476392</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::CopyResource([In] ID3D11Resource* pDstResource,[In] ID3D11Resource* pSrcResource)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::CopyResource</unmanaged-short>
        public void Copy(Direct3D11.Resource fromResource, Direct3D11.Resource toResource)
        {
            Context.CopyResource(fromResource, toResource);
        }

        /// <summary>	
        /// Copy a region from a source resource to a destination resource.	
        /// </summary>	
        /// <remarks>	
        /// The source box must be within the size of the source resource. The destination offsets, (x, y, and z) allow the source box to be offset when writing into the destination resource; however, the dimensions of the source box and the offsets must be within the size of the resource. If the resources are buffers, all coordinates are in bytes; if the resources are textures, all coordinates are in texels. {{D3D11CalcSubresource}} is a helper function for calculating subresource indexes. CopySubresourceRegion performs the copy on the GPU (similar to a memcpy by the CPU). As a consequence, the source and destination resources:  Must be different subresources (although they can be from the same resource). Must be the same type. Must have compatible DXGI formats (identical or from the same type group). For example, a DXGI_FORMAT_R32G32B32_FLOAT texture can be copied to an DXGI_FORMAT_R32G32B32_UINT texture since both of these formats are in the DXGI_FORMAT_R32G32B32_TYPELESS group. May not be currently mapped.  CopySubresourceRegion only supports copy; it does not support any stretch, color key, blend, or format conversions. An application that needs to copy an entire resource should use <see cref="SharpDX.Direct3D11.DeviceContext.CopyResource_"/> instead. CopySubresourceRegion is an asynchronous call which may be added to the command-buffer queue, this attempts to remove pipeline stalls that may occur when copying data. See performance considerations for more details. Note??If you use CopySubresourceRegion with a depth-stencil buffer or a multisampled resource, you must copy the whole subresource. In this situation, you must pass 0 to the DstX, DstY, and DstZ parameters and NULL to the pSrcBox parameter. In addition, source and destination resources, which are represented by the pSrcResource and pDstResource parameters, should have identical sample count values. Example The following code snippet copies a box (located at (120,100),(200,220)) from a source texture into a reqion (10,20),(90,140) in a destination texture. 	
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
        /// <param name="destination">A reference to the destination resource (see <see cref="SharpDX.Direct3D11.Resource"/>). </param>
        /// <param name="destinationSubResource">Destination subresource index. </param>
        /// <param name="dstX">The x-coordinate of the upper left corner of the destination region. </param>
        /// <param name="dstY">The y-coordinate of the upper left corner of the destination region. For a 1D subresource, this must be zero. </param>
        /// <param name="dstZ">The z-coordinate of the upper left corner of the destination region. For a 1D or 2D subresource, this must be zero. </param>
        /// <msdn-id>ff476394</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::CopySubresourceRegion([In] ID3D11Resource* pDstResource,[In] unsigned int DstSubresource,[In] unsigned int DstX,[In] unsigned int DstY,[In] unsigned int DstZ,[In] ID3D11Resource* pSrcResource,[In] unsigned int SrcSubresource,[In, Optional] const D3D11_BOX* pSrcBox)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::CopySubresourceRegion</unmanaged-short>	
        public void Copy(SharpDX.Direct3D11.Resource source, int sourceSubresource, SharpDX.Direct3D11.Resource destination, int destinationSubResource, int dstX = 0, int dstY = 0, int dstZ = 0)
        {
            Context.CopySubresourceRegion(source, sourceSubresource, null, destination, destinationSubResource, dstX, dstY, dstZ);
        }

        /// <summary>	
        /// Copy a region from a source resource to a destination resource.	
        /// </summary>	
        /// <remarks>	
        /// The source box must be within the size of the source resource. The destination offsets, (x, y, and z) allow the source box to be offset when writing into the destination resource; however, the dimensions of the source box and the offsets must be within the size of the resource. If the resources are buffers, all coordinates are in bytes; if the resources are textures, all coordinates are in texels. {{D3D11CalcSubresource}} is a helper function for calculating subresource indexes. CopySubresourceRegion performs the copy on the GPU (similar to a memcpy by the CPU). As a consequence, the source and destination resources:  Must be different subresources (although they can be from the same resource). Must be the same type. Must have compatible DXGI formats (identical or from the same type group). For example, a DXGI_FORMAT_R32G32B32_FLOAT texture can be copied to an DXGI_FORMAT_R32G32B32_UINT texture since both of these formats are in the DXGI_FORMAT_R32G32B32_TYPELESS group. May not be currently mapped.  CopySubresourceRegion only supports copy; it does not support any stretch, color key, blend, or format conversions. An application that needs to copy an entire resource should use <see cref="SharpDX.Direct3D11.DeviceContext.CopyResource_"/> instead. CopySubresourceRegion is an asynchronous call which may be added to the command-buffer queue, this attempts to remove pipeline stalls that may occur when copying data. See performance considerations for more details. Note??If you use CopySubresourceRegion with a depth-stencil buffer or a multisampled resource, you must copy the whole subresource. In this situation, you must pass 0 to the DstX, DstY, and DstZ parameters and NULL to the pSrcBox parameter. In addition, source and destination resources, which are represented by the pSrcResource and pDstResource parameters, should have identical sample count values. Example The following code snippet copies a box (located at (120,100),(200,220)) from a source texture into a reqion (10,20),(90,140) in a destination texture. 	
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
        public void Copy(SharpDX.Direct3D11.Resource source, int sourceSubresource, SharpDX.Direct3D11.ResourceRegion sourceRegion, SharpDX.Direct3D11.Resource destination, int destinationSubResource, int dstX = 0, int dstY = 0, int dstZ = 0)
        {
            Context.CopySubresourceRegion(source, sourceSubresource, sourceRegion, destination, destinationSubResource, dstX, dstY, dstZ);
        }

        /// <summary>	
        /// Copy a multisampled resource into a non-multisampled resource.	
        /// </summary>	
        /// <remarks>	
        /// This API is most useful when re-using the resulting rendertarget of one render pass as an input to a second render pass. The source and destination resources must be the same resource type and have the same dimensions. In addition, they must have compatible formats. There are three scenarios for this:  ScenarioRequirements Source and destination are prestructured and typedBoth the source and destination must have identical formats and that format must be specified in the Format parameter. One resource is prestructured and typed and the other is prestructured and typelessThe typed resource must have a format that is compatible with the typeless resource (i.e. the typed resource is DXGI_FORMAT_R32_FLOAT and the typeless resource is DXGI_FORMAT_R32_TYPELESS). The format of the typed resource must be specified in the Format parameter. Source and destination are prestructured and typelessBoth the source and desintation must have the same typeless format (i.e. both must have DXGI_FORMAT_R32_TYPELESS), and the Format parameter must specify a format that is compatible with the source and destination (i.e. if both are DXGI_FORMAT_R32_TYPELESS then DXGI_FORMAT_R32_FLOAT could be specified in the Format parameter). For example, given the DXGI_FORMAT_R16G16B16A16_TYPELESS format:  The source (or dest) format could be DXGI_FORMAT_R16G16B16A16_UNORM The dest (or source) format could be DXGI_FORMAT_R16G16B16A16_FLOAT    ? 	
        /// </remarks>	
        /// <param name="source">Source resource. Must be multisampled. </param>
        /// <param name="sourceSubresource">&gt;The source subresource of the source resource. </param>
        /// <param name="destination">Destination resource. Must be a created with the <see cref="SharpDX.Direct3D11.ResourceUsage.Default"/> flag and be single-sampled. See <see cref="SharpDX.Direct3D11.Resource"/>. </param>
        /// <param name="destinationSubresource">A zero-based index, that identifies the destination subresource. Use {{D3D11CalcSubresource}} to calculate the index. </param>
        /// <param name="format">A <see cref="SharpDX.DXGI.Format"/> that indicates how the multisampled resource will be resolved to a single-sampled resource.  See remarks. </param>
        /// <unmanaged>void ID3D11DeviceContext::ResolveSubresource([In] ID3D11Resource* pDstResource,[In] int DstSubresource,[In] ID3D11Resource* pSrcResource,[In] int SrcSubresource,[In] DXGI_FORMAT Format)</unmanaged>
        public void Copy(SharpDX.Direct3D11.Resource source, int sourceSubresource, SharpDX.Direct3D11.Resource destination, int destinationSubresource, SharpDX.DXGI.Format format)
        {
            Context.ResolveSubresource(source, sourceSubresource, destination, destinationSubresource, format);
        }

        /// <summary>	
        /// <p>Copies data from a buffer holding variable length data.</p>	
        /// </summary>	
        /// <param name="sourceView"><dd>  <p>Pointer to an <strong><see cref="SharpDX.Direct3D11.UnorderedAccessView"/></strong> of a Structured Buffer resource created with either  <strong><see cref="SharpDX.Direct3D11.UnorderedAccessViewBufferFlags.Append"/></strong> or <strong><see cref="SharpDX.Direct3D11.UnorderedAccessViewBufferFlags.Counter"/></strong> specified  when the UAV was created.   These types of resources have hidden counters tracking "how many" records have  been written.</p> </dd></param>	
        /// <param name="destinationBuffer"><dd>  <p>Pointer to <strong><see cref="SharpDX.Direct3D11.Buffer"/></strong>.  This can be any buffer resource that other copy commands,  such as <strong><see cref="SharpDX.Direct3D11.DeviceContext.CopyResource_"/></strong> or <strong><see cref="SharpDX.Direct3D11.DeviceContext.CopySubresourceRegion_"/></strong>, are able to write to.</p> </dd></param>	
        /// <param name="offsetInBytes"><dd>  <p>Offset from the start of <em>pDstBuffer</em> to write 32-bit UINT structure (vertex) count from <em>pSrcView</em>.</p> </dd></param>	
        /// <msdn-id>ff476393</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::CopyStructureCount([In] ID3D11Buffer* pDstBuffer,[In] unsigned int DstAlignedByteOffset,[In] ID3D11UnorderedAccessView* pSrcView)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::CopyStructureCount</unmanaged-short>	
        public void CopyCount(SharpDX.Direct3D11.UnorderedAccessView sourceView, SharpDX.Direct3D11.Buffer destinationBuffer, int offsetInBytes = 0)
        {
            Context.CopyStructureCount(destinationBuffer, offsetInBytes, sourceView);
        }

        /// <summary>	
        /// <p>Restore all default settings.</p>	
        /// </summary>	
        /// <remarks>	
        /// <p>This method resets any device context to the default settings. This sets all input/output resource slots, shaders, input layouts, predications, scissor rectangles, depth-stencil state, rasterizer state, blend state, sampler state, and viewports to <strong><c>null</c></strong>. The primitive topology is set to UNDEFINED.</p><p>For a scenario where you would like to clear a list of commands recorded so far, call <strong><see cref="SharpDX.Direct3D11.DeviceContext.FinishCommandListInternal"/></strong> and throw away the resulting <strong><see cref="SharpDX.Direct3D11.CommandList"/></strong>.</p>	
        /// </remarks>	
        /// <msdn-id>ff476389</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::ClearState()</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::ClearState</unmanaged-short>	
        public void ClearState()
        {
            Context.ClearState();
        }

        private PrimitiveTopology PrimitiveType
        {
            set
            {
                if (currentPrimitiveTopology != value)
                {
                    Context.InputAssembler.PrimitiveTopology = value;
                    currentPrimitiveTopology = value;
                }
            }
        }

        /// <summary>	
        /// <p>Draw indexed, non-instanced primitives.</p>	
        /// </summary>
        /// <param name="primitiveType">Type of the primitive to draw.</param>
        /// <param name="indexCount"><dd>  <p>Number of indices to draw.</p> </dd></param>	
        /// <param name="startIndexLocation"><dd>  <p>The location of the first index read by the GPU from the index buffer.</p> </dd></param>	
        /// <param name="baseVertexLocation"><dd>  <p>A value added to each index before reading a vertex from the vertex buffer.</p> </dd></param>	
        /// <remarks>	
        /// <p>A draw API submits work to the rendering pipeline.</p><p>If the sum of both indices is negative, the result of the function call is undefined.</p>	
        /// </remarks>	
        /// <msdn-id>ff476409</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::DrawIndexed([In] unsigned int IndexCount,[In] unsigned int StartIndexLocation,[In] int BaseVertexLocation)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::DrawIndexed</unmanaged-short>	
        public void DrawIndexed(PrimitiveType primitiveType, int indexCount, int startIndexLocation = 0, int baseVertexLocation = 0)
        {
            PrimitiveType = primitiveType;
            Context.DrawIndexed(indexCount, startIndexLocation, baseVertexLocation);
        }

        /// <summary>	
        /// <p>Draw non-indexed, non-instanced primitives.</p>	
        /// </summary>
        /// <param name="primitiveType">Type of the primitive to draw.</param>
        /// <param name="vertexCount"><dd>  <p>Number of vertices to draw.</p> </dd></param>	
        /// <param name="startVertexLocation"><dd>  <p>Index of the first vertex, which is usually an offset in a vertex buffer; it could also be used as the first vertex id generated for a shader parameter marked with the <strong>SV_TargetId</strong> system-value semantic.</p> </dd></param>	
        /// <remarks>	
        /// <p>A draw API submits work to the rendering pipeline.</p><p>The vertex data for a draw call normally comes from a vertex buffer that is bound to the pipeline. However, you could also provide the vertex data from a shader that has vertex data marked with the <strong>SV_VertexId</strong> system-value semantic.</p>	
        /// </remarks>	
        /// <msdn-id>ff476407</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::Draw([In] unsigned int VertexCount,[In] unsigned int StartVertexLocation)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Draw</unmanaged-short>	
        public void Draw(PrimitiveType primitiveType, int vertexCount, int startVertexLocation = 0)
        {
            PrimitiveType = primitiveType;
            Context.Draw(vertexCount, startVertexLocation);
        }

        /// <summary>	
        /// <p>Draw indexed, instanced primitives.</p>	
        /// </summary>	
        /// <param name="primitiveType">Type of the primitive to draw.</param>
        /// <param name="indexCountPerInstance"><dd>  <p>Number of indices read from the index buffer for each instance.</p> </dd></param>	
        /// <param name="instanceCount"><dd>  <p>Number of instances to draw.</p> </dd></param>	
        /// <param name="startIndexLocation"><dd>  <p>The location of the first index read by the GPU from the index buffer.</p> </dd></param>	
        /// <param name="baseVertexLocation"><dd>  <p>A value added to each index before reading a vertex from the vertex buffer.</p> </dd></param>	
        /// <param name="startInstanceLocation"><dd>  <p>A value added to each index before reading per-instance data from a vertex buffer.</p> </dd></param>	
        /// <remarks>	
        /// <p>A draw API submits work to the rendering pipeline.</p><p>Instancing may extend performance by reusing the same geometry to draw multiple objects in a scene. One example of instancing could be  to draw the same object with different positions and colors. Indexing requires multiple vertex buffers: at least one for per-vertex data  and a second buffer for per-instance data.</p>	
        /// </remarks>	
        /// <msdn-id>ff476410</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::DrawIndexedInstanced([In] unsigned int IndexCountPerInstance,[In] unsigned int InstanceCount,[In] unsigned int StartIndexLocation,[In] int BaseVertexLocation,[In] unsigned int StartInstanceLocation)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::DrawIndexedInstanced</unmanaged-short>	
        public void DrawIndexedInstanced(PrimitiveType primitiveType, int indexCountPerInstance, int instanceCount, int startIndexLocation = 0, int baseVertexLocation = 0, int startInstanceLocation = 0)
        {
            PrimitiveType = primitiveType;
            Context.DrawIndexedInstanced(indexCountPerInstance, instanceCount, startIndexLocation, baseVertexLocation, startInstanceLocation);
        }

        /// <summary>	
        /// <p>Draw non-indexed, instanced primitives.</p>	
        /// </summary>	
        /// <param name="primitiveType">Type of the primitive to draw.</param>
        /// <param name="vertexCountPerInstance"><dd>  <p>Number of vertices to draw.</p> </dd></param>	
        /// <param name="instanceCount"><dd>  <p>Number of instances to draw.</p> </dd></param>	
        /// <param name="startVertexLocation"><dd>  <p>Index of the first vertex.</p> </dd></param>	
        /// <param name="startInstanceLocation"><dd>  <p>A value added to each index before reading per-instance data from a vertex buffer.</p> </dd></param>	
        /// <remarks>	
        /// <p>A draw API submits work to the rendering pipeline.</p><p>Instancing may extend performance by reusing the same geometry to draw multiple objects in a scene. One example of instancing could be  to draw the same object with different positions and colors.</p><p>The vertex data for an instanced draw call normally comes from a vertex buffer that is bound to the pipeline.  However, you could also provide the vertex data from a shader that has instanced data identified with a system-value semantic (SV_InstanceID).</p>	
        /// </remarks>	
        /// <msdn-id>ff476412</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::DrawInstanced([In] unsigned int VertexCountPerInstance,[In] unsigned int InstanceCount,[In] unsigned int StartVertexLocation,[In] unsigned int StartInstanceLocation)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::DrawInstanced</unmanaged-short>	
        public void DrawInstanced(PrimitiveType primitiveType, int vertexCountPerInstance, int instanceCount, int startVertexLocation = 0, int startInstanceLocation = 0)
        {
            PrimitiveType = primitiveType;
            Context.DrawInstanced(vertexCountPerInstance, instanceCount, startVertexLocation, startInstanceLocation);
        }

        /// <summary>	
        /// <p>Draw geometry of an unknown size.</p>	
        /// </summary>	
        /// <param name="primitiveType">Type of the primitive to draw.</param>
        /// <remarks>	
        /// <p>A draw API submits work to the rendering pipeline. This API submits work of an unknown size that was processed by the input assembler, vertex shader, and stream-output stages;  the work may or may not have gone through the geometry-shader stage.</p><p>After data has been streamed out to stream-output stage buffers, those buffers can be again bound to the Input Assembler stage at input slot 0 and DrawAuto will draw them without the application needing to know the amount of data that was written to the buffers. A measurement of the amount of data written to the SO stage buffers is maintained internally when the data is streamed out. This means that the CPU does not need to fetch the measurement before re-binding the data that was streamed as input data. Although this amount is tracked internally, it is still the responsibility of applications to use input layouts to describe the format of the data in the SO stage buffers so that the layouts are available when the buffers are again bound to the input assembler.</p><p>The following diagram shows the DrawAuto process.</p><p></p><p>Calling DrawAuto does not change the state of the streaming-output buffers that were bound again as inputs.</p><p>DrawAuto only works when drawing with one input buffer bound as an input to the IA stage at slot 0. Applications must create the SO buffer resource with both binding flags, <strong><see cref="SharpDX.Direct3D11.BindFlags.VertexBuffer"/></strong> and <strong><see cref="SharpDX.Direct3D11.BindFlags.StreamOutput"/></strong>.</p><p>This API does not support indexing or instancing.</p><p>If an application needs to retrieve the size of the streaming-output buffer, it can query for statistics on streaming output by using <strong><see cref="SharpDX.Direct3D11.QueryType.StreamOutputStatistics"/></strong>.</p>	
        /// </remarks>	
        /// <msdn-id>ff476408</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::DrawAuto()</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::DrawAuto</unmanaged-short>	
        public void DrawAuto(PrimitiveType primitiveType)
        {
            PrimitiveType = primitiveType;
            Context.DrawAuto();
        }

        /// <summary>	
        /// <p>Draw indexed, instanced, GPU-generated primitives.</p>	
        /// </summary>	
        /// <param name="primitiveType">Type of the primitive to draw.</param>
        /// <param name="argumentsBuffer"><dd>  <p>A reference to an <strong><see cref="SharpDX.Direct3D11.Buffer"/></strong>, which is a buffer containing the GPU generated primitives.</p> </dd></param>	
        /// <param name="alignedByteOffsetForArgs"><dd>  <p>Offset in <em>pBufferForArgs</em> to the start of the GPU generated primitives.</p> </dd></param>	
        /// <remarks>	
        /// <p>When an application creates a buffer that is associated with the <strong><see cref="SharpDX.Direct3D11.Buffer"/></strong> interface that  <em>pBufferForArgs</em> points to, the application must set the <strong><see cref="SharpDX.Direct3D11.ResourceOptionFlags.DrawindirectArgs"/></strong> flag in the <strong>MiscFlags</strong> member of the <strong><see cref="SharpDX.Direct3D11.BufferDescription"/></strong> structure that describes the buffer. To create the buffer, the application calls the <strong><see cref="SharpDX.Direct3D11.Device.CreateBuffer"/></strong> method and in this call passes a reference to <strong><see cref="SharpDX.Direct3D11.BufferDescription"/></strong> in the <em>pDesc</em> parameter.</p>	
        /// </remarks>	
        /// <msdn-id>ff476411</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::DrawIndexedInstancedIndirect([In] ID3D11Buffer* pBufferForArgs,[In] unsigned int AlignedByteOffsetForArgs)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::DrawIndexedInstancedIndirect</unmanaged-short>	
        public void DrawIndexedInstanced(PrimitiveType primitiveType, SharpDX.Direct3D11.Buffer argumentsBuffer, int alignedByteOffsetForArgs = 0)
        {
            PrimitiveType = primitiveType;
            Context.DrawIndexedInstancedIndirect(argumentsBuffer, alignedByteOffsetForArgs);
        }

        /// <summary>	
        /// <p>Draw instanced, GPU-generated primitives.</p>	
        /// </summary>	
        /// <param name="primitiveType">Type of the primitive to draw.</param>
        /// <param name="argumentsBuffer"><dd>  <p>A reference to an <strong><see cref="SharpDX.Direct3D11.Buffer"/></strong>, which is a buffer containing the GPU generated primitives.</p> </dd></param>	
        /// <param name="alignedByteOffsetForArgs"><dd>  <p>Offset in <em>pBufferForArgs</em> to the start of the GPU generated primitives.</p> </dd></param>	
        /// <remarks>	
        /// <p>When an application creates a buffer that is associated with the <strong><see cref="SharpDX.Direct3D11.Buffer"/></strong> interface that  <em>pBufferForArgs</em> points to, the application must set the <strong><see cref="SharpDX.Direct3D11.ResourceOptionFlags.DrawindirectArgs"/></strong> flag in the <strong>MiscFlags</strong> member of the <strong><see cref="SharpDX.Direct3D11.BufferDescription"/></strong> structure that describes the buffer. To create the buffer, the application calls the <strong><see cref="SharpDX.Direct3D11.Device.CreateBuffer"/></strong> method and in this call passes a reference to <strong><see cref="SharpDX.Direct3D11.BufferDescription"/></strong> in the <em>pDesc</em> parameter.</p>	
        /// </remarks>	
        /// <msdn-id>ff476413</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::DrawInstancedIndirect([In] ID3D11Buffer* pBufferForArgs,[In] unsigned int AlignedByteOffsetForArgs)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::DrawInstancedIndirect</unmanaged-short>	
        public void DrawInstanced(PrimitiveType primitiveType, SharpDX.Direct3D11.Buffer argumentsBuffer, int alignedByteOffsetForArgs = 0)
        {
            PrimitiveType = primitiveType;
            Context.DrawIndexedInstancedIndirect(argumentsBuffer, alignedByteOffsetForArgs);
        }

        /// <summary>	
        /// <p>Execute a command list from a thread group.</p>	
        /// </summary>	
        /// <param name="threadGroupCountX"><dd>  <p>The number of groups dispatched in the x direction. <em>ThreadGroupCountX</em> must be less than <see cref="SharpDX.Direct3D11.ComputeShaderStage.DispatchMaximumThreadGroupsPerDimension"/> (65535).</p> </dd></param>	
        /// <param name="threadGroupCountY"><dd>  <p>The number of groups dispatched in the y direction. <em>ThreadGroupCountY</em> must be less than <see cref="SharpDX.Direct3D11.ComputeShaderStage.DispatchMaximumThreadGroupsPerDimension"/> (65535).</p> </dd></param>	
        /// <param name="threadGroupCountZ"><dd>  <p>The number of groups dispatched in the z direction.  <em>ThreadGroupCountZ</em> must be less than <see cref="SharpDX.Direct3D11.ComputeShaderStage.DispatchMaximumThreadGroupsPerDimension"/> (65535).  In feature level 10 the value for <em>ThreadGroupCountZ</em> must be 1.</p> </dd></param>	
        /// <remarks>	
        /// <p>You call the <strong>Dispatch</strong> method to execute commands in a compute shader. A compute shader can be run on many threads in parallel, within a thread group. Index a particular thread, within a thread group using a 3D vector  given by (x,y,z).</p><p>In the following illustration, assume a thread group with 50 threads where the size of the group is given by (5,5,2). A single thread is identified from a  thread group with 50 threads in it, using the vector (4,1,1).</p><p></p><p>The following illustration shows the relationship between the parameters passed to <strong><see cref="SharpDX.Direct3D11.DeviceContext.Dispatch"/></strong>, Dispatch(5,3,2), the values specified in the numthreads attribute, numthreads(10,8,3), and values that will passed to the compute shader for the thread-related system values 	
        /// (SV_GroupIndex,SV_DispatchThreadID,SV_GroupThreadID,SV_GroupID).</p><p></p>	
        /// </remarks>	
        /// <msdn-id>ff476405</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::Dispatch([In] unsigned int ThreadGroupCountX,[In] unsigned int ThreadGroupCountY,[In] unsigned int ThreadGroupCountZ)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Dispatch</unmanaged-short>	
        public void Dispatch(int threadGroupCountX, int threadGroupCountY, int threadGroupCountZ)
        {
            Context.Dispatch(threadGroupCountX, threadGroupCountY, threadGroupCountZ);
        }

        /// <summary>	
        /// <p>Execute a command list over one or more thread groups.</p>	
        /// </summary>	
        /// <param name="argumentsBuffer"><dd>  <p>A reference to an <strong><see cref="SharpDX.Direct3D11.Buffer"/></strong>, which must be loaded with data that matches the argument list for <strong><see cref="SharpDX.Direct3D11.DeviceContext.Dispatch"/></strong>.</p> </dd></param>	
        /// <param name="alignedByteOffsetForArgs"><dd>  <p>A byte-aligned offset between the start of the buffer and the arguments.</p> </dd></param>	
        /// <remarks>	
        /// <p>You call the <strong>DispatchIndirect</strong> method to execute commands in a compute shader.</p><p>When an application creates a buffer that is associated with the <strong><see cref="SharpDX.Direct3D11.Buffer"/></strong> interface that  <em>pBufferForArgs</em> points to, the application must set the <strong><see cref="SharpDX.Direct3D11.ResourceOptionFlags.DrawindirectArgs"/></strong> flag in the <strong>MiscFlags</strong> member of the <strong><see cref="SharpDX.Direct3D11.BufferDescription"/></strong> structure that describes the buffer. To create the buffer, the application calls the <strong><see cref="SharpDX.Direct3D11.Device.CreateBuffer"/></strong> method and in this call passes a reference to <strong><see cref="SharpDX.Direct3D11.BufferDescription"/></strong> in the <em>pDesc</em> parameter.</p>	
        /// </remarks>	
        /// <msdn-id>ff476406</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::DispatchIndirect([In] ID3D11Buffer* pBufferForArgs,[In] unsigned int AlignedByteOffsetForArgs)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::DispatchIndirect</unmanaged-short>	
        public void Dispatch(SharpDX.Direct3D11.Buffer argumentsBuffer, int alignedByteOffsetForArgs = 0)
        {
            Context.DispatchIndirect(argumentsBuffer, alignedByteOffsetForArgs);
        }

        /// <summary>	
        /// <p>Sends queued-up commands in the command buffer to the graphics processing unit (GPU).</p>	
        /// </summary>	
        /// <remarks>	
        /// <p>Most applications don't need to call this method. If an application calls this method when not necessary, it incurs a performance penalty.  Each call to <strong>Flush</strong> incurs a significant amount of overhead.</p><p>When Microsoft Direct3D state-setting, present, or draw commands are called by an application, those commands are queued into an internal command buffer.  <strong>Flush</strong> sends those commands to the GPU for processing. Typically, the Direct3D runtime sends these commands to the GPU automatically whenever the runtime determines that  they need to be sent, such as when the command buffer is full or when an application maps a resource. <strong>Flush</strong> sends the commands manually.</p><p>We recommend that you use <strong>Flush</strong> when the CPU waits for an arbitrary amount of time (such as when  you call the <strong>Sleep</strong> function).</p><p>Because <strong>Flush</strong> operates asynchronously,  it can return either before or after the GPU finishes executing the queued graphics commands. However, the graphics commands eventually always complete. You can call the <strong><see cref="SharpDX.Direct3D11.Device.CreateQuery"/></strong> method with the <strong><see cref="SharpDX.Direct3D11.QueryType.Event"/></strong> value to create an event query; you can then use that event query in a call to the <strong><see cref="SharpDX.Direct3D11.DeviceContext.GetDataInternal"/></strong> method to determine when the GPU is finished processing the graphics commands.	
        /// </p><p>Microsoft Direct3D?11 defers the destruction of objects. Therefore, an application can't rely upon objects immediately being destroyed. By calling <strong>Flush</strong>, you destroy any  objects whose destruction was deferred.  If an application requires synchronous destruction of an object, we recommend that the application release all its references, call <strong><see cref="SharpDX.Direct3D11.DeviceContext.ClearState"/></strong>, and then call <strong>Flush</strong>.</p>Deferred Destruction Issues with Flip Presentation Swap Chains<p>Direct3D?11 defers the destruction of objects like views and resources until it can efficiently destroy them. This deferred destruction can cause problems with flip presentation model swap chains. Flip presentation model swap chains have the <strong>DXGI_SWAP_EFFECT_FLIP_SEQUENTIAL</strong> flag set. When you create a flip presentation model swap chain, you can associate only one swap chain at a time with an <strong><see cref="System.IntPtr"/></strong>, <strong>IWindow</strong>, or composition surface. If an application attempts to destroy a flip presentation model swap chain and replace it with another swap chain, the original swap chain is not destroyed when the application immediately frees all of the original swap chain's references.</p><p>Most applications typically use the <strong><see cref="SharpDX.DXGI.SwapChain.ResizeBuffers"/></strong> method for the majority of scenarios where they replace new swap chain buffers for old swap chain buffers. However, if an application must actually destroy an old swap chain and create a new swap chain, the application must force the destruction of all objects that the application freed. To force the destruction, call <strong><see cref="SharpDX.Direct3D11.DeviceContext.ClearState"/></strong> (or otherwise ensure no views are bound to pipeline state), and then call <strong>Flush</strong> on the immediate context. You must force destruction before you call <strong>IDXGIFactory2::CreateSwapChainForHwnd</strong>, <strong>IDXGIFactory2::CreateSwapChainForImmersiveWindow</strong>, or <strong>IDXGIFactory2::CreateSwapChainForCompositionSurface</strong> again to create a new swap chain.</p>	
        /// </remarks>	
        /// <msdn-id>ff476425</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::Flush()</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Flush</unmanaged-short>	
        public void Flush()
        {
            Context.Flush();
        }

        /// <summary>
        /// Creates a new <see cref="GraphicsDevice"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="featureLevels">The feature levels.</param>
        /// <returns>A new instance of <see cref="GraphicsDevice"/></returns>
        public static GraphicsDevice New(DriverType type = DriverType.Hardware, DeviceCreationFlags flags = DeviceCreationFlags.None, params FeatureLevel[] featureLevels)
        {
            if (type == DriverType.Hardware)
                return new GraphicsDevice(GraphicsAdapter.Default, flags, featureLevels);

            return new GraphicsDevice(type, flags, featureLevels);
        }

        /// <summary>
        /// Creates a new <see cref="GraphicsDevice"/>.
        /// </summary>
        /// <param name="adapter">The graphics adapter to use.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="featureLevels">The feature levels.</param>
        /// <returns>A new instance of <see cref="GraphicsDevice"/></returns>
        public static GraphicsDevice New(GraphicsAdapter adapter, DeviceCreationFlags flags = DeviceCreationFlags.None, params FeatureLevel[] featureLevels)
        {
            return new GraphicsDevice(adapter, flags, featureLevels);
        }

        /// <summary>
        /// Creates a new deferred <see cref="GraphicsDevice"/>.
        /// </summary>
        /// <returns>A deferred <see cref="GraphicsDevice"/></returns>
        public GraphicsDevice NewDeferred()
        {
            return new GraphicsDevice(this, new DeviceContext(Device));
        }

        /// <summary>
        /// Attach this <see cref="GraphicsDevice"/> to the current thread.
        /// </summary>
        public void AttachToCurrentThread()
        {
            current = this;
        }

        /// <summary>
        /// Gets the content of this texture to an array of data.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="buffer">The buffer to get the data from.</param>
        /// <remarks>
        /// This method creates internally a stagging resource, copies to it and map it to memory. Use method with explicit staging resource
        /// for optimal performances.
        /// </remarks>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        public TData[] GetContent<TData>(Buffer buffer) where TData : struct
        {
            var toData = new TData[buffer.Description.SizeInBytes / Utilities.SizeOf<TData>()];
            GetContent(buffer, toData);
            return toData;
        }

        /// <summary>
        /// Copies the content of this texture to an array of data.
        /// </summary>
        /// <param name="buffer">The buffer to get the data from.</param>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="toData">The destination buffer to receive a copy of the texture datas.</param>
        /// <remarks>
        /// This method creates internally a stagging resource if this texture is not already a stagging resouce, copies to it and map it to memory. Use method with explicit staging resource
        /// for optimal performances.
        /// </remarks>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        public void GetContent<TData>(Buffer buffer, TData[] toData) where TData : struct
        {
            // Get data from this resource
            if (buffer.Description.Usage == ResourceUsage.Staging)
            {
                // Directly if this is a staging resource
                GetContent(buffer, buffer, toData);
            }
            else
            {
                // Unefficient way to use the Copy method using dynamic staging texture
                using (var throughStaging = buffer.ToStaging())
                    GetContent(buffer, throughStaging, toData);
            }
        }

        /// <summary>
        /// Copies the content of this texture to an array of data.
        /// </summary>
        /// <param name="buffer">The buffer to get the data from.</param>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="toData">The destination buffer to receive a copy of the texture datas.</param>
        /// <remarks>
        /// This method creates internally a stagging resource if this texture is not already a stagging resouce, copies to it and map it to memory. Use method with explicit staging resource
        /// for optimal performances.
        /// </remarks>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        public void GetContent<TData>(Buffer buffer, ref TData toData) where TData : struct
        {
            // Get data from this resource
            if (buffer.Description.Usage == ResourceUsage.Staging)
            {
                // Directly if this is a staging resource
                GetContent(buffer, buffer, ref toData);
            }
            else
            {
                // Unefficient way to use the Copy method using dynamic staging texture
                using (var throughStaging = buffer.ToStaging())
                    GetContent(buffer, throughStaging, ref toData);
            }
        }

        /// <summary>
        /// Copies the content of this texture from GPU memory to an array of data on CPU memory using a specific staging resource.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="buffer">The buffer to get the data from.</param>
        /// <param name="stagingTexture">The staging buffer used to transfer the buffer.</param>
        /// <param name="toData">To data.</param>
        /// <exception cref="System.ArgumentException">When strides is different from optimal strides, and TData is not the same size as the pixel format, or Width * Height != toData.Length</exception>
        /// <remarks>
        /// See the unmanaged documentation for usage and restrictions.
        /// </remarks>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        public unsafe void GetContent<TData>(Buffer buffer, Buffer stagingTexture, ref TData toData) where TData : struct
        {
            GetContent(buffer, stagingTexture, new DataPointer(Interop.Fixed(ref toData), Utilities.SizeOf<TData>()));
        }

        /// <summary>
        /// Copies the content of this texture from GPU memory to an array of data on CPU memory using a specific staging resource.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="buffer">The buffer to get the data from.</param>
        /// <param name="stagingTexture">The staging buffer used to transfer the buffer.</param>
        /// <param name="toData">To data.</param>
        /// <exception cref="System.ArgumentException">When strides is different from optimal strides, and TData is not the same size as the pixel format, or Width * Height != toData.Length</exception>
        /// <remarks>
        /// See the unmanaged documentation for usage and restrictions.
        /// </remarks>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        public unsafe void GetContent<TData>(Buffer buffer, Buffer stagingTexture, TData[] toData) where TData : struct
        {
            GetContent(buffer, stagingTexture, new DataPointer(Interop.Fixed(toData), toData.Length * Utilities.SizeOf<TData>()));
        }

        /// <summary>
        /// Copies the content of this texture from GPU memory to a CPU memory using a specific staging resource.
        /// </summary>
        /// <param name="buffer">The buffer to get the data from.</param>
        /// <param name="stagingTexture">The staging buffer used to transfer the buffer.</param>
        /// <param name="toData">To data pointer.</param>
        /// <exception cref="System.ArgumentException">When strides is different from optimal strides, and TData is not the same size as the pixel format, or Width * Height != toData.Length</exception>
        /// <remarks>
        /// See the unmanaged documentation for usage and restrictions.
        /// </remarks>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        public void GetContent(Buffer buffer, Buffer stagingTexture, DataPointer toData)
        {
            // Check size validity of data to copy to
            if (toData.Size > buffer.Description.SizeInBytes)
                throw new ArgumentException("Length of TData is larger than size of buffer");

            // Copy the texture to a staging resource
            Context.CopyResource(buffer, stagingTexture);

            // Map the staging resource to a CPU accessible memory
            var box = Context.MapSubresource(stagingTexture, 0, MapMode.Read, Direct3D11.MapFlags.None);
            Utilities.CopyMemory(box.DataPointer, toData.Pointer, toData.Size);
            // Make sure that we unmap the resource in case of an exception
            Context.UnmapSubresource(stagingTexture, 0);
        }

        /// <summary>
        /// Copies the content an array of data on CPU memory to this texture into GPU memory.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="buffer">The buffer to set the data to.</param>
        /// <param name="fromData">The data to copy from.</param>
        /// <param name="offsetInBytes">The offset in bytes to write to.</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <remarks>See the unmanaged documentation for usage and restrictions.</remarks>
        /// <msdn-id>ff476457</msdn-id>
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>
        public unsafe void SetContent<TData>(Buffer buffer, ref TData fromData, int offsetInBytes = 0) where TData : struct
        {
            SetContent(buffer, new DataPointer(Interop.Fixed(ref fromData), Utilities.SizeOf<TData>()), offsetInBytes);
        }

        /// <summary>
        /// Copies the content an array of data on CPU memory to this texture into GPU memory.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="buffer">The buffer to set the data to.</param>
        /// <param name="fromData">The data to copy from.</param>
        /// <param name="offsetInBytes">The offset in bytes to write to.</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <remarks>See the unmanaged documentation for usage and restrictions.</remarks>
        /// <msdn-id>ff476457</msdn-id>
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>
        public unsafe void SetContent<TData>(Buffer buffer, TData[] fromData, int offsetInBytes = 0) where TData : struct
        {
            SetContent(buffer, new DataPointer(Interop.Fixed(fromData), (fromData.Length * Utilities.SizeOf<TData>())), offsetInBytes);
        }

        /// <summary>
        /// Copies the content an array of data on CPU memory to this texture into GPU memory.
        /// </summary>
        /// <param name="buffer">The buffer to set the data to.</param>
        /// <param name="fromData">A data pointer.</param>
        /// <param name="offsetInBytes">The offset in bytes to write to.</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <msdn-id>ff476457</msdn-id>
        ///   <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        ///   <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>
        /// <remarks>See the unmanaged documentation for usage and restrictions.</remarks>
        public void SetContent(Buffer buffer, DataPointer fromData, int offsetInBytes = 0)
        {
            // Check size validity of data to copy to
            if (fromData.Size > buffer.Description.SizeInBytes)
                throw new ArgumentException("Size of data to upload larger than size of buffer");

            // If this texture is declared as default usage, we can only use UpdateSubresource, which is not optimal but better than nothing
            if (buffer.Description.Usage == ResourceUsage.Default)
            {
                // Setup the dest region inside the buffer
                if ((buffer.Description.BindFlags & BindFlags.ConstantBuffer) != 0)
                {
                    Context.UpdateSubresource(new DataBox(fromData.Pointer, 0, 0), buffer);                                                        
                }
                else
                {
                    var destRegion = new ResourceRegion(offsetInBytes, 0, 0, offsetInBytes + fromData.Size, 1, 1);
                    Context.UpdateSubresource(new DataBox(fromData.Pointer, 0, 0), buffer, 0, destRegion);                                    
                }

            }
            else
            {
                if (offsetInBytes > 0)
                    throw new ArgumentException("offset is only supported for textured declared with ResourceUsage.Default", "offsetInBytes");

                try
                {
                    var box = Context.MapSubresource(buffer, 0, MapMode.WriteDiscard, Direct3D11.MapFlags.None);
                    Utilities.CopyMemory(box.DataPointer, fromData.Pointer, fromData.Size);
                }
                finally
                {
                    Context.UnmapSubresource(buffer, 0);
                }
            }
        }

        /// <summary>
        /// Gets the content of this texture to an array of data.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="texture">The texture to get the data from.</param>
        /// <param name="arrayOrDepthSlice">The array slice index. This value must be set to 0 for Texture 3D.</param>
        /// <param name="mipSlice">The mip slice index.</param>
        /// <returns>The texture data.</returns>
        /// <msdn-id>ff476457</msdn-id>
        ///   <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        ///   <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>
        /// <remarks>This method creates internally a stagging resource, copies to it and map it to memory. Use method with explicit staging resource
        /// for optimal performances.</remarks>
        public TData[] GetContent<TData>(Texture texture, int arrayOrDepthSlice = 0, int mipSlice = 0) where TData : struct
        {
            var toData = new TData[texture.CalculatePixelDataCount<TData>(mipSlice)];
            GetContent(texture, toData, arrayOrDepthSlice, mipSlice);
            return toData;
        }

        /// <summary>
        /// Copies the content of this texture to an array of data.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="texture">The texture to get the data from.</param>
        /// <param name="toData">The destination buffer to receive a copy of the texture datas.</param>
        /// <param name="arraySlice">The array slice index. This value must be set to 0 for Texture 3D.</param>
        /// <param name="mipSlice">The mip slice index.</param>
        /// <msdn-id>ff476457</msdn-id>
        ///   <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        ///   <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>
        /// <remarks>This method creates internally a stagging resource if this texture is not already a stagging resouce, copies to it and map it to memory. Use method with explicit staging resource
        /// for optimal performances.</remarks>
        public void GetContent<TData>(Texture texture, TData[] toData, int arraySlice = 0, int mipSlice = 0) where TData : struct
        {
            // Get data from this resource
            if (texture.Description.Usage == ResourceUsage.Staging)
            {
                // Directly if this is a staging resource
                GetContent(texture, texture, toData, arraySlice, mipSlice);
            }
            else
            {
                // Unefficient way to use the Copy method using dynamic staging texture
                using (var throughStaging = texture.ToStaging())
                    GetContent(texture, throughStaging, toData, arraySlice, mipSlice);
            }
        }

        /// <summary>
        /// Copies the content of this texture from GPU memory to an array of data on CPU memory using a specific staging resource.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="texture">The texture to get the data from.</param>
        /// <param name="stagingTexture">The staging texture used to transfer the texture to.</param>
        /// <param name="toData">To data.</param>
        /// <param name="arraySlice">The array slice index. This value must be set to 0 for Texture 3D.</param>
        /// <param name="mipSlice">The mip slice index.</param>
        /// <exception cref="System.ArgumentException">When strides is different from optimal strides, and TData is not the same size as the pixel format, or Width * Height != toData.Length</exception>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        /// <remarks>
        /// See unmanaged documentation for usage and restrictions.
        /// </remarks>
        public unsafe void GetContent<TData>(Texture texture, Texture stagingTexture, TData[] toData, int arraySlice = 0, int mipSlice = 0) where TData : struct
        {
            GetContent(texture, stagingTexture, new DataPointer((IntPtr)Interop.Fixed(toData), toData.Length * Utilities.SizeOf<TData>()), arraySlice, mipSlice);
        }

        /// <summary>
        /// Copies the content of this texture from GPU memory to a pointer on CPU memory using a specific staging resource.
        /// </summary>
        /// <param name="texture">The texture to get the data from.</param>
        /// <param name="stagingTexture">The staging texture used to transfer the texture to.</param>
        /// <param name="toData">The pointer to data in CPU memory.</param>
        /// <param name="arraySlice">The array slice index. This value must be set to 0 for Texture 3D.</param>
        /// <param name="mipSlice">The mip slice index.</param>
        /// <exception cref="System.ArgumentException">When strides is different from optimal strides, and TData is not the same size as the pixel format, or Width * Height != toData.Length</exception>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        /// <remarks>
        /// See unmanaged documentation for usage and restrictions.
        /// </remarks>
        public unsafe void GetContent(Texture texture, Texture stagingTexture, DataPointer toData, int arraySlice = 0, int mipSlice = 0)
        {
            // Actual width for this particular mipSlice
            int width = Texture.CalculateMipSize(texture.Description.Width, mipSlice);
            int height = Texture.CalculateMipSize(texture.Description.Height, mipSlice);
            int depth = Texture.CalculateMipSize(texture.Description.Depth, mipSlice);

            // Calculate depth stride based on mipmap level
            var rowStride = (int)(width * FormatHelper.SizeOfInBytes(texture.Description.Format));

            // Depth Stride
            var textureDepthStride = rowStride * height;

            // Size Of actual texture data
            int sizeOfTextureData = textureDepthStride * depth;

            // Check size validity of data to copy to
            if (toData.Size != sizeOfTextureData)
                throw new ArgumentException(string.Format("Size of toData ({0} bytes) is not compatible expected size ({1} bytes) : Width * Height * Depth * sizeof(PixelFormat) size in bytes", toData.Size, sizeOfTextureData));

            // Copy the actual content of the texture to the staging resource
            if (!ReferenceEquals(texture, stagingTexture))
                Copy(texture, stagingTexture);

            // Calculate the subResourceIndex for a Texture2D
            int subResourceIndex = texture.GetSubResourceIndex(arraySlice, mipSlice);

            try
            {
                // Map the staging resource to a CPU accessible memory
                var box = Context.MapSubresource(stagingTexture, subResourceIndex, MapMode.Read, MapFlags.None);

                // If depth == 1 (Texture1D, Texture2D or TextureCube), then depthStride is not used
                var boxDepthStride = texture.Description.Depth == 1 ? box.SlicePitch : textureDepthStride;

                // The fast way: If same stride, we can directly copy the whole texture in one shot
                if (box.RowPitch == rowStride && boxDepthStride == textureDepthStride)
                {
                    Utilities.CopyMemory(toData.Pointer, box.DataPointer, sizeOfTextureData);
                }
                else
                {
                    // Otherwise, the long way by copying each scanline
                    var sourcePerDepthPtr = (byte*)box.DataPointer;
                    var destPtr = (byte*)toData.Pointer;

                    // Iterate on all depths
                    for (int j = 0; j < depth; j++)
                    {
                        var sourcePtr = sourcePerDepthPtr;
                        // Iterate on each line
                        for (int i = 0; i < height; i++)
                        {
                            // Copy a single row
                            Utilities.CopyMemory(new IntPtr(destPtr), new IntPtr(sourcePtr), rowStride);
                            sourcePtr += box.RowPitch;
                            destPtr += rowStride;
                        }
                        sourcePerDepthPtr += box.SlicePitch;
                    }
                }
            }
            finally
            {
                // Make sure that we unmap the resource in case of an exception
                Context.UnmapSubresource(stagingTexture, subResourceIndex);
            }
        }

        /// <summary>
        /// Copies the content an array of data on CPU memory to this texture into GPU memory.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="texture">The texture to set the data to.</param>
        /// <param name="fromData">The data to copy from.</param>
        /// <param name="arraySlice">The array slice index. This value must be set to 0 for Texture 3D.</param>
        /// <param name="mipSlice">The mip slice index.</param>
        /// <param name="region">Destination region</param>
        /// <exception cref="System.ArgumentException">When strides is different from optimal strides, and TData is not the same size as the pixel format, or Width * Height != toData.Length</exception>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        /// <remarks>
        /// See unmanaged documentation for usage and restrictions.
        /// </remarks>
        public unsafe void SetContent<TData>(Texture texture, TData[] fromData, int arraySlice = 0, int mipSlice = 0, ResourceRegion? region = null) where TData : struct
        {
            SetContent(texture, new DataPointer((IntPtr)Interop.Fixed(fromData), fromData.Length * Utilities.SizeOf<TData>()), arraySlice, mipSlice, region);
        }

        /// <summary>
        /// Copies the content an data on CPU memory to this texture into GPU memory.
        /// </summary>
        /// <param name="texture">The texture to set the data to.</param>
        /// <param name="fromData">The data to copy from.</param>
        /// <param name="arraySlice">The array slice index. This value must be set to 0 for Texture 3D.</param>
        /// <param name="mipSlice">The mip slice index.</param>
        /// <param name="region">Destination region</param>
        /// <exception cref="System.ArgumentException">When strides is different from optimal strides, and TData is not the same size as the pixel format, or Width * Height != toData.Length</exception>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        /// <remarks>
        /// See unmanaged documentation for usage and restrictions.
        /// </remarks>
        public unsafe void SetContent(Texture texture, DataPointer fromData, int arraySlice = 0, int mipSlice = 0, ResourceRegion? region = null)
        {
            if (region.HasValue && texture.Description.Usage != ResourceUsage.Default)
                throw new ArgumentException("Region is only supported for textures with ResourceUsage.Default");

            int width = Texture.CalculateMipSize(texture.Description.Width, mipSlice);
            int height = Texture.CalculateMipSize(texture.Description.Height, mipSlice);
            int depth = Texture.CalculateMipSize(texture.Description.Depth, mipSlice);

            // If we are using a region, then check that parameters are fine
            if (region.HasValue)
            {
                int newWidth = region.Value.Right - region.Value.Left;
                int newHeight = region.Value.Bottom - region.Value.Top;
                int newDepth = region.Value.Back - region.Value.Front;
                if (newWidth > width)
                    throw new ArgumentException(string.Format("Region width [{0}] cannot be greater than mipmap width [{1}]", newWidth, width), "region");
                if (newHeight > height)
                    throw new ArgumentException(string.Format("Region height [{0}] cannot be greater than mipmap height [{1}]", newHeight, height), "region");
                if (newDepth > depth)
                    throw new ArgumentException(string.Format("Region depth [{0}] cannot be greater than mipmap depth [{1}]", newDepth, depth), "region");

                width = newWidth;
                height = newHeight;
                depth = newDepth;
            }
            
            // Size per pixel
            var sizePerElement = (int)FormatHelper.SizeOfInBytes(texture.Description.Format);

            // Calculate depth stride based on mipmap level
            var rowStride = width * sizePerElement;

            // Depth Stride
            var textureDepthStride = rowStride * height;

            // Size Of actual texture data
            int sizeOfTextureData = textureDepthStride * depth;

            // Check size validity of data to copy to
            if (fromData.Size != sizeOfTextureData)
                throw new ArgumentException(string.Format("Size of toData ({0} bytes) is not compatible expected size ({1} bytes) : Width * Height * Depth * sizeof(PixelFormat) size in bytes", fromData.Size, sizeOfTextureData));

            // Calculate the subResourceIndex for a Texture
            int subResourceIndex = texture.GetSubResourceIndex(arraySlice, mipSlice);

            // If this texture is declared as default usage, we use UpdateSubresource that supports sub resource region.
            if (texture.Description.Usage == ResourceUsage.Default)
            {
                // If using a specific region, we need to handle this case
                if (region.HasValue)
                {
                    var regionValue = region.Value;
                    var sourceDataPtr = fromData.Pointer;

                    // Workaround when using region with a deferred context and a device that does not support CommandList natively
                    // see http://blogs.msdn.com/b/chuckw/archive/2010/07/28/known-issue-direct3d-11-updatesubresource-and-deferred-contexts.aspx
                    if (needWorkAroundForUpdateSubResource)
                    {
                        if (texture.IsBlockCompressed)
                        {
                            regionValue.Left /= 4;
                            regionValue.Right /= 4;
                            regionValue.Top /= 4;
                            regionValue.Bottom /= 4;
                        }
                        sourceDataPtr = new IntPtr((byte*)sourceDataPtr - (regionValue.Front * textureDepthStride) - (regionValue.Top * rowStride) - (regionValue.Left * sizePerElement));
                    }
                    Context.UpdateSubresource(new DataBox(sourceDataPtr, rowStride, textureDepthStride), texture, subResourceIndex, regionValue);
                }
                else
                {
                    Context.UpdateSubresource(new DataBox(fromData.Pointer, rowStride, textureDepthStride), texture, subResourceIndex);
                }
            }
            else
            {
                try
                {
                    var box = Context.MapSubresource(texture, subResourceIndex, texture.Description.Usage == ResourceUsage.Dynamic ? MapMode.WriteDiscard : MapMode.Write, MapFlags.None);

                    // If depth == 1 (Texture1D, Texture2D or TextureCube), then depthStride is not used
                    var boxDepthStride = texture.Description.Depth == 1 ? box.SlicePitch : textureDepthStride;

                    // The fast way: If same stride, we can directly copy the whole texture in one shot
                    if (box.RowPitch == rowStride && boxDepthStride == textureDepthStride)
                    {
                        Utilities.CopyMemory(box.DataPointer, fromData.Pointer, sizeOfTextureData);
                    }
                    else
                    {
                        // Otherwise, the long way by copying each scanline
                        var destPerDepthPtr = (byte*)box.DataPointer;
                        var sourcePtr = (byte*)fromData.Pointer;

                        // Iterate on all depths
                        for (int j = 0; j < depth; j++)
                        {
                            var destPtr = destPerDepthPtr;
                            // Iterate on each line
                            for (int i = 0; i < height; i++)
                            {
                                Utilities.CopyMemory((IntPtr)destPtr, (IntPtr)sourcePtr, rowStride);
                                destPtr += box.RowPitch;
                                sourcePtr += rowStride;
                            }
                            destPerDepthPtr += box.SlicePitch;
                        }

                    }
                }
                finally
                {
                    Context.UnmapSubresource(texture, subResourceIndex);
                }
            }
        }



        /// <summary>	
        /// <p>Set the blend state of the output-merger stage.</p>	
        /// </summary>	
        /// <param name="blendState"><dd>  <p>Pointer to a blend-state interface (see <strong><see cref="SharpDX.Direct3D11.BlendState"/></strong>). Passing in <strong><c>null</c></strong> implies a default blend state. See remarks for further details.</p> </dd></param>	
        /// <remarks>	
        /// <p>Blend state is used by the output-merger stage to determine how to blend together two pixel values. The two values are commonly the current pixel value and the pixel value already in the output render target. Use the <strong>blend operation</strong> to control where the two pixel values come from and how they are mathematically combined.</p><p>To create a blend-state interface, call <strong><see cref="SharpDX.Direct3D11.Device.CreateBlendState"/></strong>.</p><p>Passing in <strong><c>null</c></strong> for the blend-state interface indicates to the runtime to set a default blending state.  The following table indicates the default blending parameters.</p><table> <tr><th>State</th><th>Default Value</th></tr> <tr><td>AlphaToCoverageEnable</td><td><strong><see cref="SharpDX.Result.False"/></strong></td></tr> <tr><td>BlendEnable</td><td><strong><see cref="SharpDX.Result.False"/></strong>[8]</td></tr> <tr><td>SrcBlend</td><td><see cref="SharpDX.Direct3D11.BlendOption.One"/></td></tr> <tr><td>DstBlend</td><td><see cref="SharpDX.Direct3D11.BlendOption.Zero"/></td></tr> <tr><td>BlendOp</td><td><see cref="SharpDX.Direct3D11.BlendOperation.Add"/></td></tr> <tr><td>SrcBlendAlpha</td><td><see cref="SharpDX.Direct3D11.BlendOption.One"/></td></tr> <tr><td>DstBlendAlpha</td><td><see cref="SharpDX.Direct3D11.BlendOption.Zero"/></td></tr> <tr><td>BlendOpAlpha</td><td><see cref="SharpDX.Direct3D11.BlendOperation.Add"/></td></tr> <tr><td>RenderTargetWriteMask[8]</td><td><see cref="SharpDX.Direct3D11.ColorWriteMaskFlags.All"/>[8]</td></tr> </table><p>?</p><p>A sample mask determines which samples get updated in all the active render targets. The mapping of bits in a sample mask to samples in a multisample render target is the responsibility of an individual application. A sample mask is always applied; it is independent of whether multisampling is enabled, and does not depend on whether an application uses multisample render targets.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p>	
        /// </remarks>	
        /// <msdn-id>ff476462</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetBlendState([In, Optional] ID3D11BlendState* pBlendState,[In, Optional] const SHARPDX_COLOR4* BlendFactor,[In] unsigned int SampleMask)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetBlendState</unmanaged-short>	
        public void SetBlendState(BlendState blendState)
        {
            // If same instance, avoid a unmanaged to managed transition.
            if (ReferenceEquals(currentBlendState, blendState) && currentBlendFactor == blendState.BlendFactor && currentMultiSampleMask == blendState.MultiSampleMask)
                return;

            if (blendState == null)
            {
                Context.OutputMerger.SetBlendState(null, Color.White, -1);
                currentBlendState = null;
                currentBlendFactor = Color.White;
                currentMultiSampleMask = -1;
            }
            else
            {
                Context.OutputMerger.SetBlendState(blendState, blendState.BlendFactor, blendState.MultiSampleMask);
                currentBlendState = blendState;
                currentBlendFactor = blendState.BlendFactor;
                currentMultiSampleMask = blendState.MultiSampleMask;
            }
        }

        /// <summary>	
        /// <p>Set the blend state of the output-merger stage.</p>	
        /// </summary>	
        /// <param name="blendState"><dd>  <p>Pointer to a blend-state interface (see <strong><see cref="SharpDX.Direct3D11.BlendState"/></strong>). Passing in <strong><c>null</c></strong> implies a default blend state. See remarks for further details.</p> </dd></param>
        /// <param name="blendFactor"><dd>  <p>Array of blend factors, one for each RGBA component. This requires a blend state object that specifies the <strong><see cref="SharpDX.Direct3D11.BlendOption.BlendFactor"/></strong> option.</p> </dd></param>	
        /// <param name="multiSampleMask"><dd>  <p>32-bit sample coverage. The default value is 0xffffffff. See remarks.</p> </dd></param>	
        /// <remarks>	
        /// <p>Blend state is used by the output-merger stage to determine how to blend together two pixel values. The two values are commonly the current pixel value and the pixel value already in the output render target. Use the <strong>blend operation</strong> to control where the two pixel values come from and how they are mathematically combined.</p><p>To create a blend-state interface, call <strong><see cref="SharpDX.Direct3D11.Device.CreateBlendState"/></strong>.</p><p>Passing in <strong><c>null</c></strong> for the blend-state interface indicates to the runtime to set a default blending state.  The following table indicates the default blending parameters.</p><table> <tr><th>State</th><th>Default Value</th></tr> <tr><td>AlphaToCoverageEnable</td><td><strong><see cref="SharpDX.Result.False"/></strong></td></tr> <tr><td>BlendEnable</td><td><strong><see cref="SharpDX.Result.False"/></strong>[8]</td></tr> <tr><td>SrcBlend</td><td><see cref="SharpDX.Direct3D11.BlendOption.One"/></td></tr> <tr><td>DstBlend</td><td><see cref="SharpDX.Direct3D11.BlendOption.Zero"/></td></tr> <tr><td>BlendOp</td><td><see cref="SharpDX.Direct3D11.BlendOperation.Add"/></td></tr> <tr><td>SrcBlendAlpha</td><td><see cref="SharpDX.Direct3D11.BlendOption.One"/></td></tr> <tr><td>DstBlendAlpha</td><td><see cref="SharpDX.Direct3D11.BlendOption.Zero"/></td></tr> <tr><td>BlendOpAlpha</td><td><see cref="SharpDX.Direct3D11.BlendOperation.Add"/></td></tr> <tr><td>RenderTargetWriteMask[8]</td><td><see cref="SharpDX.Direct3D11.ColorWriteMaskFlags.All"/>[8]</td></tr> </table><p>?</p><p>A sample mask determines which samples get updated in all the active render targets. The mapping of bits in a sample mask to samples in a multisample render target is the responsibility of an individual application. A sample mask is always applied; it is independent of whether multisampling is enabled, and does not depend on whether an application uses multisample render targets.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p>	
        /// </remarks>	
        /// <msdn-id>ff476462</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetBlendState([In, Optional] ID3D11BlendState* pBlendState,[In, Optional] const SHARPDX_COLOR4* BlendFactor,[In] unsigned int SampleMask)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetBlendState</unmanaged-short>	
        public void SetBlendState(BlendState blendState, Color4 blendFactor, int multiSampleMask = -1)
        {
            // If same instance, avoid a unmanaged to managed transition.
            if (ReferenceEquals(currentBlendState, blendState) && currentBlendFactor == blendFactor && currentMultiSampleMask == multiSampleMask)
                return;

            if (blendState == null)
            {
                Context.OutputMerger.SetBlendState(null, blendFactor, multiSampleMask);
                currentBlendState = null;
            }
            else
            {
                Context.OutputMerger.SetBlendState(blendState, blendFactor, multiSampleMask);
                currentBlendState = blendState;
            }

            currentBlendFactor = blendFactor;
            currentMultiSampleMask = multiSampleMask;
        }

        /// <summary>	
        /// <p>Set the blend state of the output-merger stage.</p>	
        /// </summary>	
        /// <param name="blendState"><dd>  <p>Pointer to a blend-state interface (see <strong><see cref="SharpDX.Direct3D11.BlendState"/></strong>). Passing in <strong><c>null</c></strong> implies a default blend state. See remarks for further details.</p> </dd></param>
        /// <param name="blendFactor"><dd>  <p>Array of blend factors, one for each RGBA component. This requires a blend state object that specifies the <strong><see cref="SharpDX.Direct3D11.BlendOption.BlendFactor"/></strong> option.</p> </dd></param>	
        /// <param name="multiSampleMask"><dd>  <p>32-bit sample coverage. The default value is 0xffffffff. See remarks.</p> </dd></param>	
        /// <remarks>	
        /// <p>Blend state is used by the output-merger stage to determine how to blend together two pixel values. The two values are commonly the current pixel value and the pixel value already in the output render target. Use the <strong>blend operation</strong> to control where the two pixel values come from and how they are mathematically combined.</p><p>To create a blend-state interface, call <strong><see cref="SharpDX.Direct3D11.Device.CreateBlendState"/></strong>.</p><p>Passing in <strong><c>null</c></strong> for the blend-state interface indicates to the runtime to set a default blending state.  The following table indicates the default blending parameters.</p><table> <tr><th>State</th><th>Default Value</th></tr> <tr><td>AlphaToCoverageEnable</td><td><strong><see cref="SharpDX.Result.False"/></strong></td></tr> <tr><td>BlendEnable</td><td><strong><see cref="SharpDX.Result.False"/></strong>[8]</td></tr> <tr><td>SrcBlend</td><td><see cref="SharpDX.Direct3D11.BlendOption.One"/></td></tr> <tr><td>DstBlend</td><td><see cref="SharpDX.Direct3D11.BlendOption.Zero"/></td></tr> <tr><td>BlendOp</td><td><see cref="SharpDX.Direct3D11.BlendOperation.Add"/></td></tr> <tr><td>SrcBlendAlpha</td><td><see cref="SharpDX.Direct3D11.BlendOption.One"/></td></tr> <tr><td>DstBlendAlpha</td><td><see cref="SharpDX.Direct3D11.BlendOption.Zero"/></td></tr> <tr><td>BlendOpAlpha</td><td><see cref="SharpDX.Direct3D11.BlendOperation.Add"/></td></tr> <tr><td>RenderTargetWriteMask[8]</td><td><see cref="SharpDX.Direct3D11.ColorWriteMaskFlags.All"/>[8]</td></tr> </table><p>?</p><p>A sample mask determines which samples get updated in all the active render targets. The mapping of bits in a sample mask to samples in a multisample render target is the responsibility of an individual application. A sample mask is always applied; it is independent of whether multisampling is enabled, and does not depend on whether an application uses multisample render targets.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p>	
        /// </remarks>	
        /// <msdn-id>ff476462</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetBlendState([In, Optional] ID3D11BlendState* pBlendState,[In, Optional] const SHARPDX_COLOR4* BlendFactor,[In] unsigned int SampleMask)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetBlendState</unmanaged-short>	
        public void SetBlendState(BlendState blendState, SharpDX.Color4 blendFactor, uint multiSampleMask = 0xFFFFFFFF)
        {
            SetBlendState(blendState, blendFactor, checked((int)multiSampleMask));
        }

        /// <summary>	
        /// Sets the depth-stencil state of the output-merger stage.
        /// </summary>	
        /// <param name="depthStencilState"><dd>  <p>Pointer to a depth-stencil state interface (see <strong><see cref="SharpDX.Direct3D11.DepthStencilState"/></strong>) to bind to the device. Set this to <strong><c>null</c></strong> to use the default state listed in <strong><see cref="SharpDX.Direct3D11.DepthStencilStateDescription"/></strong>.</p> </dd></param>	
        /// <param name="stencilReference"><dd>  <p>Reference value to perform against when doing a depth-stencil test. See remarks.</p> </dd></param>	
        /// <remarks>	
        /// <p>To create a depth-stencil state interface, call <strong><see cref="SharpDX.Direct3D11.Device.CreateDepthStencilState"/></strong>.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p>	
        /// </remarks>	
        /// <msdn-id>ff476463</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetDepthStencilState([In, Optional] ID3D11DepthStencilState* pDepthStencilState,[In] unsigned int StencilRef)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetDepthStencilState</unmanaged-short>	
        public void SetDepthStencilState(DepthStencilState depthStencilState, int stencilReference = 0)
        {
            // If same instance, avoid a unmanaged to managed transition.
            if (ReferenceEquals(currentDepthStencilState, depthStencilState) && currentDepthStencilReference == stencilReference)
                return;

            Context.OutputMerger.SetDepthStencilState(depthStencilState, stencilReference);

            // Set new current state
            currentDepthStencilState = depthStencilState;
            currentDepthStencilReference = stencilReference;
        }

        /// <summary>	
        /// <p>Set the <strong>rasterizer state</strong> for the rasterizer stage of the pipeline.</p>	
        /// </summary>	
        /// <param name="rasterizerState">The rasterizser state to set on this device.</param>	
        /// <msdn-id>ff476479</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::RSSetState([In, Optional] ID3D11RasterizerState* pRasterizerState)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::RSSetState</unmanaged-short>	
        public void SetRasterizerState(RasterizerState rasterizerState)
        {
            // If same instance, avoid a unmanaged to managed transition.
            if (ReferenceEquals(currentRasterizerState, rasterizerState))
                return;

            Context.Rasterizer.State = rasterizerState;

            // Set new current state
            currentRasterizerState = rasterizerState;
        }

        /// <summary>
        /// Binds a single scissor rectangle to the rasterizer stage.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="top">The top.</param>
        /// <param name="right">The right.</param>
        /// <param name="bottom">The bottom.</param>
        /// <remarks>	
        /// <p>All scissor rects must be set atomically as one operation. Any scissor rects not defined by the call are disabled.</p><p>The scissor rectangles will only be used if ScissorEnable is set to true in the rasterizer state (see <strong><see cref="SharpDX.Direct3D11.RasterizerStateDescription"/></strong>).</p><p>Which scissor rectangle to use is determined by the SV_ViewportArrayIndex semantic output by a geometry shader (see shader semantic syntax). If a geometry shader does not make use of the SV_ViewportArrayIndex semantic then Direct3D will use the first scissor rectangle in the array.</p><p>Each scissor rectangle in the array corresponds to a viewport in an array of viewports (see <strong><see cref="SharpDX.Direct3D11.RasterizerStage.SetViewports"/></strong>).</p>	
        /// </remarks>	
        /// <msdn-id>ff476478</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::RSSetScissorRects([In] unsigned int NumRects,[In, Buffer, Optional] const void* pRects)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::RSSetScissorRects</unmanaged-short>	
        public void SetScissorRectangles(int left, int top, int right, int bottom)
        {
            Context.Rasterizer.SetScissorRectangle(left, top, right, bottom);
        }

        /// <summary>
        ///   Binds a set of scissor rectangles to the rasterizer stage.
        /// </summary>
        /// <param name = "scissorRectangles">The set of scissor rectangles to bind.</param>
        /// <remarks>	
        /// <p>All scissor rects must be set atomically as one operation. Any scissor rects not defined by the call are disabled.</p><p>The scissor rectangles will only be used if ScissorEnable is set to true in the rasterizer state (see <strong><see cref="SharpDX.Direct3D11.RasterizerStateDescription"/></strong>).</p><p>Which scissor rectangle to use is determined by the SV_ViewportArrayIndex semantic output by a geometry shader (see shader semantic syntax). If a geometry shader does not make use of the SV_ViewportArrayIndex semantic then Direct3D will use the first scissor rectangle in the array.</p><p>Each scissor rectangle in the array corresponds to a viewport in an array of viewports (see <strong><see cref="SharpDX.Direct3D11.RasterizerStage.SetViewports"/></strong>).</p>	
        /// </remarks>	
        /// <msdn-id>ff476478</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::RSSetScissorRects([In] unsigned int NumRects,[In, Buffer, Optional] const void* pRects)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::RSSetScissorRects</unmanaged-short>	
        public void SetScissorRectangles(params Rectangle[] scissorRectangles)
        {
            Context.Rasterizer.SetScissorRectangles(scissorRectangles);
        }

        /// <summary>
        /// Binds a single viewport to the rasterizer stage.
        /// </summary>
        /// <param name="x">The x coord of the viewport.</param>
        /// <param name="y">The x coord of the viewport.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="minZ">The min Z.</param>
        /// <param name="maxZ">The max Z.</param>
        /// <remarks>	
        /// <p></p><p>All viewports must be set atomically as one operation. Any viewports not defined by the call are disabled.</p><p>Which viewport to use is determined by the SV_ViewportArrayIndex semantic output by a geometry shader; if a geometry shader does not specify the semantic, Direct3D will use the first viewport in the array.</p>	
        /// </remarks>	
        /// <msdn-id>ff476480</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::RSSetViewports([In] unsigned int NumViewports,[In, Buffer, Optional] const void* pViewports)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::RSSetViewports</unmanaged-short>	
        public void SetViewports(float x, float y, float width, float height, float minZ = 0.0f, float maxZ = 1.0f)
        {
            Context.Rasterizer.SetViewport(x, y, width, height, minZ, maxZ);
        }

        /// <summary>
        /// Binds a single viewport to the rasterizer stage.
        /// </summary>
        /// <param name="viewport">The viewport.</param>
        /// <remarks>	
        /// <p></p><p>All viewports must be set atomically as one operation. Any viewports not defined by the call are disabled.</p><p>Which viewport to use is determined by the SV_ViewportArrayIndex semantic output by a geometry shader; if a geometry shader does not specify the semantic, Direct3D will use the first viewport in the array.</p>	
        /// </remarks>	
        /// <msdn-id>ff476480</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::RSSetViewports([In] unsigned int NumViewports,[In, Buffer, Optional] const void* pViewports)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::RSSetViewports</unmanaged-short>	
        public void SetViewports(Viewport viewport)
        {
            Context.Rasterizer.SetViewports(viewport);
        }

        /// <summary>
        ///   Binds a set of viewports to the rasterizer stage.
        /// </summary>
        /// <param name = "viewports">The set of viewports to bind.</param>
        /// <remarks>	
        /// <p></p><p>All viewports must be set atomically as one operation. Any viewports not defined by the call are disabled.</p><p>Which viewport to use is determined by the SV_ViewportArrayIndex semantic output by a geometry shader; if a geometry shader does not specify the semantic, Direct3D will use the first viewport in the array.</p>	
        /// </remarks>	
        /// <msdn-id>ff476480</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::RSSetViewports([In] unsigned int NumViewports,[In, Buffer, Optional] const void* pViewports)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::RSSetViewports</unmanaged-short>	
        public void SetViewports(params Viewport[] viewports)
        {
            Context.Rasterizer.SetViewports(viewports);
        }

        /// <summary>
        ///   Unbinds all depth-stencil buffer and render targets from the output-merger stage.
        /// </summary>
        /// <msdn-id>ff476464</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargets([In] unsigned int NumViews,[In] const void** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargets</unmanaged-short>	
        public void ResetTargets()
        {
            Context.OutputMerger.ResetTargets();
        }

        /// <summary>	
        /// <p>Bind one or more render targets atomically and the depth-stencil buffer to the output-merger stage.</p>	
        /// </summary>	
        /// <param name = "renderTargetViews">A set of render target views to bind.</param>
        /// <remarks>	
        /// <p>The maximum number of active render targets a device can have active at any given time is set by a #define in D3D11.h called  D3D11_SIMULTANEOUS_RENDER_TARGET_COUNT. It is invalid to try to set the same subresource to multiple render target slots.  Any render targets not defined by this call are set to <strong><c>null</c></strong>.</p><p>If any subresources are also currently bound for reading in a different stage or writing (perhaps in a different part of the pipeline),  those bind points will be set to <strong><c>null</c></strong>, in order to prevent the same subresource from being read and written simultaneously in a single rendering operation.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p><p>If the render-target views were created from an array resource type, then all of the render-target views must have the same array size.   This restriction also applies to the depth-stencil view, its array size must match that of the render-target views being bound.</p><p>The pixel shader must be able to simultaneously render to at least eight separate render targets. All of these render targets must access the same type of resource: Buffer, Texture1D, Texture1DArray, Texture2D, Texture2DArray, Texture3D, or TextureCube. All render targets must have the same size in all dimensions (width and height, and depth for 3D or array size for *Array types). If render targets use multisample anti-aliasing, all bound render targets and depth buffer must be the same form of multisample resource (that is, the sample counts must be the same). Each render target can have a different data format. These render target formats are not required to have identical bit-per-element counts.</p><p>Any combination of the eight slots for render targets can have a render target set or not set.</p><p>The same resource view cannot be bound to multiple render target slots simultaneously. However, you can set multiple non-overlapping resource views of a single resource as simultaneous multiple render targets.</p>	
        /// </remarks>	
        /// <msdn-id>ff476464</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargets([In] unsigned int NumViews,[In] const void** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargets</unmanaged-short>	
        public void SetRenderTargets(params RenderTargetView[] renderTargetViews)
        {
            Context.OutputMerger.SetTargets(renderTargetViews);
        }

        /// <summary>	
        ///   Binds a single render target to the output-merger stage.
        /// </summary>	
        /// <param name = "renderTargetView">A view of the render target to bind.</param>
        /// <remarks>	
        /// <p>The maximum number of active render targets a device can have active at any given time is set by a #define in D3D11.h called  D3D11_SIMULTANEOUS_RENDER_TARGET_COUNT. It is invalid to try to set the same subresource to multiple render target slots.  Any render targets not defined by this call are set to <strong><c>null</c></strong>.</p><p>If any subresources are also currently bound for reading in a different stage or writing (perhaps in a different part of the pipeline),  those bind points will be set to <strong><c>null</c></strong>, in order to prevent the same subresource from being read and written simultaneously in a single rendering operation.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p><p>If the render-target views were created from an array resource type, then all of the render-target views must have the same array size.   This restriction also applies to the depth-stencil view, its array size must match that of the render-target views being bound.</p><p>The pixel shader must be able to simultaneously render to at least eight separate render targets. All of these render targets must access the same type of resource: Buffer, Texture1D, Texture1DArray, Texture2D, Texture2DArray, Texture3D, or TextureCube. All render targets must have the same size in all dimensions (width and height, and depth for 3D or array size for *Array types). If render targets use multisample anti-aliasing, all bound render targets and depth buffer must be the same form of multisample resource (that is, the sample counts must be the same). Each render target can have a different data format. These render target formats are not required to have identical bit-per-element counts.</p><p>Any combination of the eight slots for render targets can have a render target set or not set.</p><p>The same resource view cannot be bound to multiple render target slots simultaneously. However, you can set multiple non-overlapping resource views of a single resource as simultaneous multiple render targets.</p>	
        /// </remarks>	
        /// <msdn-id>ff476464</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargets([In] unsigned int NumViews,[In] const void** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargets</unmanaged-short>	
        public void SetRenderTargets(RenderTargetView renderTargetView)
        {
            Context.OutputMerger.SetTargets(renderTargetView);
        }

        /// <summary>
        ///   Binds a depth-stencil buffer and a set of render targets to the output-merger stage.
        /// </summary>
        /// <param name = "depthStencilView">A view of the depth-stencil buffer to bind.</param>
        /// <param name = "renderTargetViews">A set of render target views to bind.</param>
        /// <remarks>	
        /// <p>The maximum number of active render targets a device can have active at any given time is set by a #define in D3D11.h called  D3D11_SIMULTANEOUS_RENDER_TARGET_COUNT. It is invalid to try to set the same subresource to multiple render target slots.  Any render targets not defined by this call are set to <strong><c>null</c></strong>.</p><p>If any subresources are also currently bound for reading in a different stage or writing (perhaps in a different part of the pipeline),  those bind points will be set to <strong><c>null</c></strong>, in order to prevent the same subresource from being read and written simultaneously in a single rendering operation.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p><p>If the render-target views were created from an array resource type, then all of the render-target views must have the same array size.   This restriction also applies to the depth-stencil view, its array size must match that of the render-target views being bound.</p><p>The pixel shader must be able to simultaneously render to at least eight separate render targets. All of these render targets must access the same type of resource: Buffer, Texture1D, Texture1DArray, Texture2D, Texture2DArray, Texture3D, or TextureCube. All render targets must have the same size in all dimensions (width and height, and depth for 3D or array size for *Array types). If render targets use multisample anti-aliasing, all bound render targets and depth buffer must be the same form of multisample resource (that is, the sample counts must be the same). Each render target can have a different data format. These render target formats are not required to have identical bit-per-element counts.</p><p>Any combination of the eight slots for render targets can have a render target set or not set.</p><p>The same resource view cannot be bound to multiple render target slots simultaneously. However, you can set multiple non-overlapping resource views of a single resource as simultaneous multiple render targets.</p>	
        /// </remarks>	
        /// <msdn-id>ff476464</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargets([In] unsigned int NumViews,[In] const void** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargets</unmanaged-short>	
        public void SetRenderTargets(DepthStencilView depthStencilView, params RenderTargetView[] renderTargetViews)
        {
            Context.OutputMerger.SetTargets(depthStencilView, renderTargetViews);
        }

        /// <summary>
        ///   Binds a depth-stencil buffer and a single render target to the output-merger stage.
        /// </summary>
        /// <param name = "depthStencilView">A view of the depth-stencil buffer to bind.</param>
        /// <param name = "renderTargetView">A view of the render target to bind.</param>
        /// <remarks>	
        /// <p>The maximum number of active render targets a device can have active at any given time is set by a #define in D3D11.h called  D3D11_SIMULTANEOUS_RENDER_TARGET_COUNT. It is invalid to try to set the same subresource to multiple render target slots.  Any render targets not defined by this call are set to <strong><c>null</c></strong>.</p><p>If any subresources are also currently bound for reading in a different stage or writing (perhaps in a different part of the pipeline),  those bind points will be set to <strong><c>null</c></strong>, in order to prevent the same subresource from being read and written simultaneously in a single rendering operation.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p><p>If the render-target views were created from an array resource type, then all of the render-target views must have the same array size.   This restriction also applies to the depth-stencil view, its array size must match that of the render-target views being bound.</p><p>The pixel shader must be able to simultaneously render to at least eight separate render targets. All of these render targets must access the same type of resource: Buffer, Texture1D, Texture1DArray, Texture2D, Texture2DArray, Texture3D, or TextureCube. All render targets must have the same size in all dimensions (width and height, and depth for 3D or array size for *Array types). If render targets use multisample anti-aliasing, all bound render targets and depth buffer must be the same form of multisample resource (that is, the sample counts must be the same). Each render target can have a different data format. These render target formats are not required to have identical bit-per-element counts.</p><p>Any combination of the eight slots for render targets can have a render target set or not set.</p><p>The same resource view cannot be bound to multiple render target slots simultaneously. However, you can set multiple non-overlapping resource views of a single resource as simultaneous multiple render targets.</p>	
        /// </remarks>	
        /// <msdn-id>ff476464</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargets([In] unsigned int NumViews,[In] const void** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargets</unmanaged-short>	
        public void SetRenderTargets(DepthStencilView depthStencilView, RenderTargetView renderTargetView)
        {
            Context.OutputMerger.SetTargets(depthStencilView, renderTargetView);
        }

        /// <summary>
        ///   Binds a set of unordered access views and a single render target to the output-merger stage.
        /// </summary>
        /// <param name = "startSlot">Index into a zero-based array to begin setting unordered access views.</param>
        /// <param name = "unorderedAccessViews">A set of unordered access views to bind.</param>
        /// <param name = "renderTargetView">A view of the render target to bind.</param>
        /// <msdn-id>ff476465</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews([In] unsigned int NumRTVs,[In, Buffer, Optional] const ID3D11RenderTargetView** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView,[In] unsigned int UAVStartSlot,[In] unsigned int NumUAVs,[In, Buffer, Optional] const ID3D11UnorderedAccessView** ppUnorderedAccessViews,[In, Buffer, Optional] const unsigned int* pUAVInitialCounts)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews</unmanaged-short>	
        public void SetRenderTargetsAndUnorderedAccess(
            RenderTargetView renderTargetView,
            int startSlot,
            UnorderedAccessView[] unorderedAccessViews)
        {
            Context.OutputMerger.SetTargets(renderTargetView, startSlot, unorderedAccessViews);
        }

        /// <summary>
        ///   Binds a set of unordered access views and a set of render targets to the output-merger stage.
        /// </summary>
        /// <param name = "startSlot">Index into a zero-based array to begin setting unordered access views.</param>
        /// <param name = "unorderedAccessViews">A set of unordered access views to bind.</param>
        /// <param name = "renderTargetViews">A set of render target views to bind.</param>
        /// <msdn-id>ff476465</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews([In] unsigned int NumRTVs,[In, Buffer, Optional] const ID3D11RenderTargetView** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView,[In] unsigned int UAVStartSlot,[In] unsigned int NumUAVs,[In, Buffer, Optional] const ID3D11UnorderedAccessView** ppUnorderedAccessViews,[In, Buffer, Optional] const unsigned int* pUAVInitialCounts)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews</unmanaged-short>	
        public void SetRenderTargetsAndUnorderedAccess(
            int startSlot,
            UnorderedAccessView[] unorderedAccessViews,
            params RenderTargetView[] renderTargetViews)
        {
            Context.OutputMerger.SetTargets(startSlot, unorderedAccessViews, renderTargetViews);
        }

        /// <summary>
        ///   Binds a depth-stencil buffer, a set of unordered access views, and a single render target to the output-merger stage.
        /// </summary>
        /// <param name = "depthStencilView">A view of the depth-stencil buffer to bind.</param>
        /// <param name = "startSlot">Index into a zero-based array to begin setting unordered access views.</param>
        /// <param name = "unorderedAccessViews">A set of unordered access views to bind.</param>
        /// <param name = "renderTargetView">A view of the render target to bind.</param>
        /// <msdn-id>ff476465</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews([In] unsigned int NumRTVs,[In, Buffer, Optional] const ID3D11RenderTargetView** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView,[In] unsigned int UAVStartSlot,[In] unsigned int NumUAVs,[In, Buffer, Optional] const ID3D11UnorderedAccessView** ppUnorderedAccessViews,[In, Buffer, Optional] const unsigned int* pUAVInitialCounts)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews</unmanaged-short>	
        public void SetRenderTargetsAndUnorderedAccess(
            DepthStencilView depthStencilView,
            RenderTargetView renderTargetView,
            int startSlot,
            UnorderedAccessView[] unorderedAccessViews)
        {
            Context.OutputMerger.SetTargets(depthStencilView, renderTargetView, startSlot, unorderedAccessViews);
        }

        /// <summary>
        ///   Binds a depth-stencil buffer, a set of unordered access views, and a set of render targets to the output-merger stage.
        /// </summary>
        /// <param name = "depthStencilView">A view of the depth-stencil buffer to bind.</param>
        /// <param name = "startSlot">Index into a zero-based array to begin setting unordered access views.</param>
        /// <param name = "unorderedAccessViews">A set of unordered access views to bind.</param>
        /// <param name = "renderTargetViews">A set of render target views to bind.</param>
        /// <msdn-id>ff476465</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews([In] unsigned int NumRTVs,[In, Buffer, Optional] const ID3D11RenderTargetView** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView,[In] unsigned int UAVStartSlot,[In] unsigned int NumUAVs,[In, Buffer, Optional] const ID3D11UnorderedAccessView** ppUnorderedAccessViews,[In, Buffer, Optional] const unsigned int* pUAVInitialCounts)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews</unmanaged-short>	
        public void SetRenderTargetsAndUnorderedAccess(
            DepthStencilView depthStencilView,
            int startSlot,
            UnorderedAccessView[] unorderedAccessViews,
            params RenderTargetView[] renderTargetViews)
        {
            Context.OutputMerger.SetTargets(depthStencilView, startSlot, unorderedAccessViews, renderTargetViews);
        }

        /// <summary>
        ///   Binds a set of unordered access views and a single render target to the output-merger stage.
        /// </summary>
        /// <param name = "startSlot">Index into a zero-based array to begin setting unordered access views.</param>
        /// <param name = "unorderedAccessViews">A set of unordered access views to bind.</param>
        /// <param name = "renderTargetView">A view of the render target to bind.</param>
        /// <param name = "initialLengths">An array of Append/Consume buffer offsets. A value of -1 indicates the current offset should be kept. Any other values set the hidden counter for that Appendable/Consumeable UAV.</param>
        /// <msdn-id>ff476465</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews([In] unsigned int NumRTVs,[In, Buffer, Optional] const ID3D11RenderTargetView** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView,[In] unsigned int UAVStartSlot,[In] unsigned int NumUAVs,[In, Buffer, Optional] const ID3D11UnorderedAccessView** ppUnorderedAccessViews,[In, Buffer, Optional] const unsigned int* pUAVInitialCounts)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews</unmanaged-short>	
        public void SetRenderTargetsAndUnorderedAccess(
            RenderTargetView renderTargetView,
            int startSlot,
            UnorderedAccessView[] unorderedAccessViews,
            int[] initialLengths)
        {
            Context.OutputMerger.SetTargets(renderTargetView, startSlot, unorderedAccessViews, initialLengths);
        }

        /// <summary>
        /// Binds a set of unordered access views and a set of render targets to the output-merger stage.
        /// </summary>
        /// <param name = "startSlot">Index into a zero-based array to begin setting unordered access views.</param>
        /// <param name = "unorderedAccessViews">A set of unordered access views to bind.</param>
        /// <param name = "renderTargetViews">A set of render target views to bind.</param>
        /// <param name = "initialLengths">An array of Append/Consume buffer offsets. A value of -1 indicates the current offset should be kept. Any other values set the hidden counter for that Appendable/Consumeable UAV.</param>
        /// <msdn-id>ff476465</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews([In] unsigned int NumRTVs,[In, Buffer, Optional] const ID3D11RenderTargetView** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView,[In] unsigned int UAVStartSlot,[In] unsigned int NumUAVs,[In, Buffer, Optional] const ID3D11UnorderedAccessView** ppUnorderedAccessViews,[In, Buffer, Optional] const unsigned int* pUAVInitialCounts)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews</unmanaged-short>	
        public void SetRenderTargetsAndUnorderedAccess(
            int startSlot,
            UnorderedAccessView[] unorderedAccessViews,
            int[] initialLengths,
            params RenderTargetView[] renderTargetViews)
        {
            Context.OutputMerger.SetTargets(startSlot, unorderedAccessViews, initialLengths, renderTargetViews);
        }

        /// <summary>
        /// Binds a depth-stencil buffer, a set of unordered access views, and a single render target to the output-merger stage.
        /// </summary>
        /// <param name = "depthStencilView">A view of the depth-stencil buffer to bind.</param>
        /// <param name = "startSlot">Index into a zero-based array to begin setting unordered access views.</param>
        /// <param name = "unorderedAccessViews">A set of unordered access views to bind.</param>
        /// <param name = "renderTargetView">A view of the render target to bind.</param>
        /// <param name = "initialLengths">An array of Append/Consume buffer offsets. A value of -1 indicates the current offset should be kept. Any other values set the hidden counter for that Appendable/Consumeable UAV.</param>
        /// <msdn-id>ff476465</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews([In] unsigned int NumRTVs,[In, Buffer, Optional] const ID3D11RenderTargetView** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView,[In] unsigned int UAVStartSlot,[In] unsigned int NumUAVs,[In, Buffer, Optional] const ID3D11UnorderedAccessView** ppUnorderedAccessViews,[In, Buffer, Optional] const unsigned int* pUAVInitialCounts)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews</unmanaged-short>	
        public void SetRenderTargetsAndUnorderedAccess(
            DepthStencilView depthStencilView,
            RenderTargetView renderTargetView,
            int startSlot,
            UnorderedAccessView[] unorderedAccessViews,
            int[] initialLengths)
        {
            Context.OutputMerger.SetTargets(depthStencilView, renderTargetView, startSlot, unorderedAccessViews, initialLengths);
        }

        /// <summary>
        /// Binds a depth-stencil buffer, a set of unordered access views, and a set of render targets to the output-merger stage.
        /// </summary>
        /// <param name = "depthStencilView">A view of the depth-stencil buffer to bind.</param>
        /// <param name = "startSlot">Index into a zero-based array to begin setting unordered access views.</param>
        /// <param name = "unorderedAccessViews">A set of unordered access views to bind.</param>
        /// <param name = "renderTargetViews">A set of render target views to bind.</param>
        /// <param name = "initialLengths">An array of Append/Consume buffer offsets. A value of -1 indicates the current offset should be kept. Any other values set the hidden counter for that Appendable/Consumeable UAV.</param>
        /// <msdn-id>ff476465</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews([In] unsigned int NumRTVs,[In, Buffer, Optional] const ID3D11RenderTargetView** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView,[In] unsigned int UAVStartSlot,[In] unsigned int NumUAVs,[In, Buffer, Optional] const ID3D11UnorderedAccessView** ppUnorderedAccessViews,[In, Buffer, Optional] const unsigned int* pUAVInitialCounts)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews</unmanaged-short>	
        public void SetRenderTargetsAndUnorderedAccess(
            DepthStencilView depthStencilView,
            int startSlot,
            UnorderedAccessView[] unorderedAccessViews,
            int[] initialLengths,
            params RenderTargetView[] renderTargetViews)
        {
            Context.OutputMerger.SetTargets(depthStencilView, startSlot, unorderedAccessViews, initialLengths, renderTargetViews);
        }

        /// <summary>	
        /// <p>Bind an index buffer to the input-assembler stage.</p>	
        /// </summary>	
        /// <param name="indexBuffer"><dd>  <p>A reference to an <strong><see cref="SharpDX.Direct3D11.Buffer"/></strong> object, that contains indices. The index buffer must have been created with  the <strong><see cref="SharpDX.Direct3D11.BindFlags.IndexBuffer"/></strong> flag.</p> </dd></param>	
        /// <param name="is32Bit">Set to true if indices are 32-bit values (integer size) or false if they are 16-bit values (short size)</param>	
        /// <param name="offset">Offset (in bytes) from the start of the index buffer to the first index to use. Default to 0</param>	
        /// <remarks>	
        /// <p>For information about creating index buffers, see How to: Create an Index Buffer.</p><p>Calling this method using a buffer that is currently bound for writing (i.e. bound to the stream output pipeline stage) will effectively bind  <strong><c>null</c></strong> instead because a buffer cannot be bound as both an input and an output at the same time.</p><p>The debug layer will generate a warning whenever a resource is prevented from being bound simultaneously as an input and an output, but this will  not prevent invalid data from being used by the runtime.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p>	
        /// </remarks>	
        /// <msdn-id>ff476453</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::IASetIndexBuffer([In, Optional] ID3D11Buffer* pIndexBuffer,[In] DXGI_FORMAT Format,[In] unsigned int Offset)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::IASetIndexBuffer</unmanaged-short>	
        public void SetIndexBuffer(Buffer indexBuffer, bool is32Bit, int offset = 0)
        {
            Context.InputAssembler.SetIndexBuffer(indexBuffer, is32Bit ? DXGI.Format.R32_UInt : DXGI.Format.R16_UInt, offset);
        }

        public void SetInputLayout(InputLayout inputLayout)
        {
            Context.InputAssembler.InputLayout = inputLayout;
        }

        /// <summary>
        /// <p>Bind a single vertex buffer to the input-assembler stage.</p>	
        /// </summary>	
        /// <param name="slot"><dd>  <p>The first input slot for binding. The first vertex buffer is explicitly bound to the start slot; this causes each additional vertex buffer in the array to be implicitly bound to each subsequent input slot. The maximum of 16 or 32 input slots (ranges from 0 to <see cref="SharpDX.Direct3D11.InputAssemblerStage.VertexInputResourceSlotCount"/> - 1) are available; the maximum number of input slots depends on the feature level.</p> </dd></param>	
        /// <param name="vertexBufferBinding"><dd>  <p>A <see cref="SharpDX.Direct3D11.VertexBufferBinding"/>. The vertex buffer must have been created with the <strong><see cref="SharpDX.Direct3D11.BindFlags.VertexBuffer"/></strong> flag.</p> </dd></param>        /// <remarks>	
        /// <p>For information about creating vertex buffers, see Create a Vertex Buffer.</p><p>Calling this method using a buffer that is currently bound for writing (i.e. bound to the stream output pipeline stage) will effectively bind <strong><c>null</c></strong> instead because a buffer cannot be bound as both an input and an output at the same time.</p><p>The debug layer will generate a warning whenever a resource is prevented from being bound simultaneously as an input and an output, but this will not prevent invalid data from being used by the runtime.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p>	
        /// </remarks>	
        /// <msdn-id>ff476456</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::IASetVertexBuffers([In] unsigned int StartSlot,[In] unsigned int NumBuffers,[In, Buffer] const void* ppVertexBuffers,[In, Buffer] const void* pStrides,[In, Buffer] const void* pOffsets)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::IASetVertexBuffers</unmanaged-short>	
        public void SetVertexBuffers(int slot, VertexBufferBinding vertexBufferBinding)
        {
            Context.InputAssembler.SetVertexBuffers(slot, vertexBufferBinding);
        }

        /// <summary>
        /// <p>Bind an array of vertex buffers to the input-assembler stage.</p>	
        /// </summary>	
        /// <param name="firstSlot"><dd>  <p>The first input slot for binding. The first vertex buffer is explicitly bound to the start slot; this causes each additional vertex buffer in the array to be implicitly bound to each subsequent input slot. The maximum of 16 or 32 input slots (ranges from 0 to <see cref="SharpDX.Direct3D11.InputAssemblerStage.VertexInputResourceSlotCount"/> - 1) are available; the maximum number of input slots depends on the feature level.</p> </dd></param>	
        /// <param name="vertexBufferBindings"><dd>  <p>A reference to an array of <see cref="SharpDX.Direct3D11.VertexBufferBinding"/>. The vertex buffers must have been created with the <strong><see cref="SharpDX.Direct3D11.BindFlags.VertexBuffer"/></strong> flag.</p> </dd></param>        /// <remarks>	
        /// <p>For information about creating vertex buffers, see Create a Vertex Buffer.</p><p>Calling this method using a buffer that is currently bound for writing (i.e. bound to the stream output pipeline stage) will effectively bind <strong><c>null</c></strong> instead because a buffer cannot be bound as both an input and an output at the same time.</p><p>The debug layer will generate a warning whenever a resource is prevented from being bound simultaneously as an input and an output, but this will not prevent invalid data from being used by the runtime.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p>	
        /// </remarks>	
        /// <msdn-id>ff476456</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::IASetVertexBuffers([In] unsigned int StartSlot,[In] unsigned int NumBuffers,[In, Buffer] const void* ppVertexBuffers,[In, Buffer] const void* pStrides,[In, Buffer] const void* pOffsets)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::IASetVertexBuffers</unmanaged-short>	
        public void SetVertexBuffers(int firstSlot, params VertexBufferBinding[] vertexBufferBindings)
        {
            Context.InputAssembler.SetVertexBuffers(firstSlot, vertexBufferBindings);
        }

        /// <summary>
        /// <p>Bind an array of vertex buffers to the input-assembler stage.</p>	
        /// </summary>	
        /// <param name="slot"><dd>  <p>The first input slot for binding. The first vertex buffer is explicitly bound to the start slot; this causes each additional vertex buffer in the array to be implicitly bound to each subsequent input slot. The maximum of 16 or 32 input slots (ranges from 0 to <see cref="SharpDX.Direct3D11.InputAssemblerStage.VertexInputResourceSlotCount"/> - 1) are available; the maximum number of input slots depends on the feature level.</p> </dd></param>	
        /// <param name="vertexBuffers"><dd>  <p>A reference to an array of vertex buffers (see <strong><see cref="SharpDX.Direct3D11.Buffer"/></strong>). The vertex buffers must have been created with the <strong><see cref="SharpDX.Direct3D11.BindFlags.VertexBuffer"/></strong> flag.</p> </dd></param>	
        /// <param name="stridesRef"><dd>  <p>Pointer to an array of stride values; one stride value for each buffer in the vertex-buffer array. Each stride is the size (in bytes) of the elements that are to be used from that vertex buffer.</p> </dd></param>	
        /// <param name="offsetsRef"><dd>  <p>Pointer to an array of offset values; one offset value for each buffer in the vertex-buffer array. Each offset is the number of bytes between the first element of a vertex buffer and the first element that will be used.</p> </dd></param>	
        /// <remarks>	
        /// <p>For information about creating vertex buffers, see Create a Vertex Buffer.</p><p>Calling this method using a buffer that is currently bound for writing (i.e. bound to the stream output pipeline stage) will effectively bind <strong><c>null</c></strong> instead because a buffer cannot be bound as both an input and an output at the same time.</p><p>The debug layer will generate a warning whenever a resource is prevented from being bound simultaneously as an input and an output, but this will not prevent invalid data from being used by the runtime.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p>	
        /// </remarks>	
        /// <msdn-id>ff476456</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::IASetVertexBuffers([In] unsigned int StartSlot,[In] unsigned int NumBuffers,[In, Buffer] const void* ppVertexBuffers,[In, Buffer] const void* pStrides,[In, Buffer] const void* pOffsets)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::IASetVertexBuffers</unmanaged-short>	
        public void SetVertexBuffers(int slot, SharpDX.Direct3D11.Buffer[] vertexBuffers, int[] stridesRef, int[] offsetsRef)
        {
            Context.InputAssembler.SetVertexBuffers(slot, vertexBuffers, stridesRef, offsetsRef);
        }

        /// <summary>
        /// Presents the Backbuffer to the screen.
        /// </summary>
        /// <remarks>
        /// This method is only working if a <see cref="GraphicsPresenter"/> is set on this device using <see cref="CurrentPresenter"/> property.
        /// </remarks>
        /// <msdn-id>bb174576</msdn-id>	
        /// <unmanaged>HRESULT IDXGISwapChain::Present([In] unsigned int SyncInterval,[In] DXGI_PRESENT_FLAGS Flags)</unmanaged>	
        /// <unmanaged-short>IDXGISwapChain::Present</unmanaged-short>	
        public void Present()
        {
            if (CurrentPresenter == null)
                throw new InvalidOperationException("No presenter currently setup for this instance. CurrentPresenter is null");

            CurrentPresenter.Present();
        }

        public static implicit operator Device(GraphicsDevice from)
        {
            return from == null ? null : from.Device;
        }

        public static implicit operator DeviceContext(GraphicsDevice from)
        {
            return from == null ? null : from.Context;
        }
    }
}
