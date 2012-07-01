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
    /// This class is a frontend to <see cref="SharpDX.Direct3D11.Device" /> and <see cref="SharpDX.Direct3D11.DeviceContext" />
    /// </summary>
    public class GraphicsDevice : Component
    {
        internal Device Device;
        internal DeviceContext Context;

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

        protected GraphicsDevice(DriverType type = DriverType.Hardware, DeviceCreationFlags flags = DeviceCreationFlags.None, params FeatureLevel[] featureLevels)
        {
            Device = ToDispose(featureLevels.Length > 0 ? new Device(type, flags, featureLevels) : new Device(type, flags));
            IsDebugMode = (Device.CreationFlags & (int)DeviceCreationFlags.Debug) != 0;
            MainDevice = this;
            Context = Device.ImmediateContext;
            Features = new GraphicsDeviceFeatures(Device);
            AttachToCurrentThread();
        }

        protected GraphicsDevice(Device device)
        {
            Device = ToDispose(device);
            IsDebugMode = (Device.CreationFlags & (int)DeviceCreationFlags.Debug) != 0;
            MainDevice = this;
            Context = Device.ImmediateContext;
            Features = new GraphicsDeviceFeatures(Device);
            AttachToCurrentThread();
        }

        protected GraphicsDevice(GraphicsDevice mainDevice, DeviceContext deferredContext)
        {
            Device = mainDevice.Device;
            IsDebugMode = (Device.CreationFlags & (int)DeviceCreationFlags.Debug) != 0;
            MainDevice = mainDevice;
            Context = deferredContext;
            Features = mainDevice.Features;
        }

        /// <summary>
        /// Gets the <see cref="GraphicsDevice"/> attached to the current thread.
        /// </summary>
        public static GraphicsDevice Current
        {
            get { return current; }
        }

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
        /// <p>Copies data from a buffer holding variable length data.</p>	
        /// </summary>	
        /// <param name="sourceView"><dd>  <p>Pointer to an <strong><see cref="SharpDX.Direct3D11.UnorderedAccessView"/></strong> of a Structured Buffer resource created with either  <strong><see cref="SharpDX.Direct3D11.UnorderedAccessViewBufferFlags.Append"/></strong> or <strong><see cref="SharpDX.Direct3D11.UnorderedAccessViewBufferFlags.Counter"/></strong> specified  when the UAV was created.   These types of resources have hidden counters tracking "how many" records have  been written.</p> </dd></param>	
        /// <param name="destinationBuffer"><dd>  <p>Pointer to <strong><see cref="SharpDX.Direct3D11.Buffer"/></strong>.  This can be any buffer resource that other copy commands,  such as <strong><see cref="SharpDX.Direct3D11.DeviceContext.CopyResource_"/></strong> or <strong><see cref="SharpDX.Direct3D11.DeviceContext.CopySubresourceRegion_"/></strong>, are able to write to.</p> </dd></param>	
        /// <param name="offsetInBytes"><dd>  <p>Offset from the start of <em>pDstBuffer</em> to write 32-bit UINT structure (vertex) count from <em>pSrcView</em>.</p> </dd></param>	
        /// <msdn-id>ff476393</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::CopyStructureCount([In] ID3D11Buffer* pDstBuffer,[In] unsigned int DstAlignedByteOffset,[In] ID3D11UnorderedAccessView* pSrcView)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::CopyStructureCount</unmanaged-short>	
        public void CopyStructureCount(SharpDX.Direct3D11.UnorderedAccessView sourceView, SharpDX.Direct3D11.Buffer destinationBuffer, int offsetInBytes = 0)
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

        /// <summary>	
        /// <p>Draw indexed, non-instanced primitives.</p>	
        /// </summary>	
        /// <param name="indexCount"><dd>  <p>Number of indices to draw.</p> </dd></param>	
        /// <param name="startIndexLocation"><dd>  <p>The location of the first index read by the GPU from the index buffer.</p> </dd></param>	
        /// <param name="baseVertexLocation"><dd>  <p>A value added to each index before reading a vertex from the vertex buffer.</p> </dd></param>	
        /// <remarks>	
        /// <p>A draw API submits work to the rendering pipeline.</p><p>If the sum of both indices is negative, the result of the function call is undefined.</p>	
        /// </remarks>	
        /// <msdn-id>ff476409</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::DrawIndexed([In] unsigned int IndexCount,[In] unsigned int StartIndexLocation,[In] int BaseVertexLocation)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::DrawIndexed</unmanaged-short>	
        public void DrawIndexed(int indexCount, int startIndexLocation = 0, int baseVertexLocation = 0)
        {
            Context.DrawIndexed(indexCount, startIndexLocation, baseVertexLocation);
        }

        /// <summary>	
        /// <p>Draw non-indexed, non-instanced primitives.</p>	
        /// </summary>	
        /// <param name="vertexCount"><dd>  <p>Number of vertices to draw.</p> </dd></param>	
        /// <param name="startVertexLocation"><dd>  <p>Index of the first vertex, which is usually an offset in a vertex buffer; it could also be used as the first vertex id generated for a shader parameter marked with the <strong>SV_TargetId</strong> system-value semantic.</p> </dd></param>	
        /// <remarks>	
        /// <p>A draw API submits work to the rendering pipeline.</p><p>The vertex data for a draw call normally comes from a vertex buffer that is bound to the pipeline. However, you could also provide the vertex data from a shader that has vertex data marked with the <strong>SV_VertexId</strong> system-value semantic.</p>	
        /// </remarks>	
        /// <msdn-id>ff476407</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::Draw([In] unsigned int VertexCount,[In] unsigned int StartVertexLocation)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Draw</unmanaged-short>	
        public void Draw(int vertexCount, int startVertexLocation = 0)
        {
            Context.Draw(vertexCount, startVertexLocation);
        }

        /// <summary>	
        /// <p>Draw indexed, instanced primitives.</p>	
        /// </summary>	
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
        public void DrawIndexedInstanced(int indexCountPerInstance, int instanceCount, int startIndexLocation = 0, int baseVertexLocation = 0, int startInstanceLocation = 0)
        {
            Context.DrawIndexedInstanced(indexCountPerInstance, instanceCount, startIndexLocation, baseVertexLocation, startInstanceLocation);
        }

        /// <summary>	
        /// <p>Draw non-indexed, instanced primitives.</p>	
        /// </summary>	
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
        public void DrawInstanced(int vertexCountPerInstance, int instanceCount, int startVertexLocation = 0, int startInstanceLocation = 0)
        {
            Context.DrawInstanced(vertexCountPerInstance, instanceCount, startVertexLocation, startInstanceLocation);
        }

        /// <summary>	
        /// <p>Draw geometry of an unknown size.</p>	
        /// </summary>	
        /// <remarks>	
        /// <p>A draw API submits work to the rendering pipeline. This API submits work of an unknown size that was processed by the input assembler, vertex shader, and stream-output stages;  the work may or may not have gone through the geometry-shader stage.</p><p>After data has been streamed out to stream-output stage buffers, those buffers can be again bound to the Input Assembler stage at input slot 0 and DrawAuto will draw them without the application needing to know the amount of data that was written to the buffers. A measurement of the amount of data written to the SO stage buffers is maintained internally when the data is streamed out. This means that the CPU does not need to fetch the measurement before re-binding the data that was streamed as input data. Although this amount is tracked internally, it is still the responsibility of applications to use input layouts to describe the format of the data in the SO stage buffers so that the layouts are available when the buffers are again bound to the input assembler.</p><p>The following diagram shows the DrawAuto process.</p><p></p><p>Calling DrawAuto does not change the state of the streaming-output buffers that were bound again as inputs.</p><p>DrawAuto only works when drawing with one input buffer bound as an input to the IA stage at slot 0. Applications must create the SO buffer resource with both binding flags, <strong><see cref="SharpDX.Direct3D11.BindFlags.VertexBuffer"/></strong> and <strong><see cref="SharpDX.Direct3D11.BindFlags.StreamOutput"/></strong>.</p><p>This API does not support indexing or instancing.</p><p>If an application needs to retrieve the size of the streaming-output buffer, it can query for statistics on streaming output by using <strong><see cref="SharpDX.Direct3D11.QueryType.StreamOutputStatistics"/></strong>.</p>	
        /// </remarks>	
        /// <msdn-id>ff476408</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::DrawAuto()</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::DrawAuto</unmanaged-short>	
        public void DrawAuto()
        {
            Context.DrawAuto();
        }

        /// <summary>	
        /// <p>Draw indexed, instanced, GPU-generated primitives.</p>	
        /// </summary>	
        /// <param name="argumentsBuffer"><dd>  <p>A reference to an <strong><see cref="SharpDX.Direct3D11.Buffer"/></strong>, which is a buffer containing the GPU generated primitives.</p> </dd></param>	
        /// <param name="alignedByteOffsetForArgs"><dd>  <p>Offset in <em>pBufferForArgs</em> to the start of the GPU generated primitives.</p> </dd></param>	
        /// <remarks>	
        /// <p>When an application creates a buffer that is associated with the <strong><see cref="SharpDX.Direct3D11.Buffer"/></strong> interface that  <em>pBufferForArgs</em> points to, the application must set the <strong><see cref="SharpDX.Direct3D11.ResourceOptionFlags.DrawindirectArgs"/></strong> flag in the <strong>MiscFlags</strong> member of the <strong><see cref="SharpDX.Direct3D11.BufferDescription"/></strong> structure that describes the buffer. To create the buffer, the application calls the <strong><see cref="SharpDX.Direct3D11.Device.CreateBuffer"/></strong> method and in this call passes a reference to <strong><see cref="SharpDX.Direct3D11.BufferDescription"/></strong> in the <em>pDesc</em> parameter.</p>	
        /// </remarks>	
        /// <msdn-id>ff476411</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::DrawIndexedInstancedIndirect([In] ID3D11Buffer* pBufferForArgs,[In] unsigned int AlignedByteOffsetForArgs)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::DrawIndexedInstancedIndirect</unmanaged-short>	
        public void DrawIndexedInstanced(SharpDX.Direct3D11.Buffer argumentsBuffer, int alignedByteOffsetForArgs = 0)
        {
            Context.DrawIndexedInstancedIndirect(argumentsBuffer, alignedByteOffsetForArgs);
        }

        /// <summary>	
        /// <p>Draw instanced, GPU-generated primitives.</p>	
        /// </summary>	
        /// <param name="argumentsBuffer"><dd>  <p>A reference to an <strong><see cref="SharpDX.Direct3D11.Buffer"/></strong>, which is a buffer containing the GPU generated primitives.</p> </dd></param>	
        /// <param name="alignedByteOffsetForArgs"><dd>  <p>Offset in <em>pBufferForArgs</em> to the start of the GPU generated primitives.</p> </dd></param>	
        /// <remarks>	
        /// <p>When an application creates a buffer that is associated with the <strong><see cref="SharpDX.Direct3D11.Buffer"/></strong> interface that  <em>pBufferForArgs</em> points to, the application must set the <strong><see cref="SharpDX.Direct3D11.ResourceOptionFlags.DrawindirectArgs"/></strong> flag in the <strong>MiscFlags</strong> member of the <strong><see cref="SharpDX.Direct3D11.BufferDescription"/></strong> structure that describes the buffer. To create the buffer, the application calls the <strong><see cref="SharpDX.Direct3D11.Device.CreateBuffer"/></strong> method and in this call passes a reference to <strong><see cref="SharpDX.Direct3D11.BufferDescription"/></strong> in the <em>pDesc</em> parameter.</p>	
        /// </remarks>	
        /// <msdn-id>ff476413</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::DrawInstancedIndirect([In] ID3D11Buffer* pBufferForArgs,[In] unsigned int AlignedByteOffsetForArgs)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::DrawInstancedIndirect</unmanaged-short>	
        public void DrawInstanced(SharpDX.Direct3D11.Buffer argumentsBuffer, int alignedByteOffsetForArgs = 0)
        {
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
        /// Creates a new device from a <see cref="SharpDX.Direct3D11.Device"/>.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>A new instance of <see cref="GraphicsDevice"/></returns>
        public static GraphicsDevice New(Device device)
        {
            return new GraphicsDevice(device);
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
            return new GraphicsDevice(type, flags, featureLevels);
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
        /// <param name="subResourceIndex">Index of the subresource to copy from.</param>
        /// <remarks>
        /// This method creates internally a stagging resource, copies to it and map it to memory. Use method with explicit staging resource
        /// for optimal performances.
        /// </remarks>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        public TData[] GetData<TData>(Buffer buffer, int subResourceIndex = 0) where TData : struct
        {
            var toData = new TData[buffer.Description.SizeInBytes / Utilities.SizeOf<TData>()];
            GetData(buffer, toData, subResourceIndex);
            return toData;
        }

        /// <summary>
        /// Copies the content of this texture to an array of data.
        /// </summary>
        /// <param name="buffer">The buffer to get the data from.</param>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="toData">The destination buffer to receive a copy of the texture datas.</param>
        /// <param name="subResourceIndex">Index of the subresource to copy from.</param>
        /// <remarks>
        /// This method creates internally a stagging resource if this texture is not already a stagging resouce, copies to it and map it to memory. Use method with explicit staging resource
        /// for optimal performances.
        /// </remarks>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        public void GetData<TData>(Buffer buffer, TData[] toData, int subResourceIndex = 0) where TData : struct
        {
            // Get data from this resource
            if (buffer.Description.Usage == ResourceUsage.Staging)
            {
                // Directly if this is a staging resource
                GetData(buffer, buffer, toData, subResourceIndex);
            }
            else
            {
                // Unefficient way to use the Copy method using dynamic staging texture
                using (var throughStaging = buffer.ToStaging())
                    GetData(buffer, throughStaging, toData, subResourceIndex);
            }
        }

        /// <summary>
        /// Copies the content of this texture from GPU memory to an array of data on CPU memory using a specific staging resource.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="buffer">The buffer to get the data from.</param>
        /// <param name="stagingTexture">The staging buffer used to transfer the buffer.</param>
        /// <param name="toData">To data.</param>
        /// <param name="subResourceIndex">Index of the sub resource.</param>
        /// <exception cref="System.ArgumentException">When strides is different from optimal strides, and TData is not the same size as the pixel format, or Width * Height != toData.Length</exception>
        /// <remarks>
        /// See the unmanaged documentation for usage and restrictions.
        /// </remarks>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        public void GetData<TData>(Buffer buffer, Buffer stagingTexture, TData[] toData, int subResourceIndex = 0) where TData : struct
        {
            // Check size validity of data to copy to
            if ((toData.Length * Utilities.SizeOf<TData>()) > buffer.Description.SizeInBytes)
                throw new ArgumentException("Length of TData is larger than size of buffer");

            // Copy the texture to a staging resource
            Context.CopyResource(buffer, stagingTexture);

            // Map the staging resource to a CPU accessible memory
            var box = Context.MapSubresource(stagingTexture, subResourceIndex, MapMode.Read, Direct3D11.MapFlags.None);
            Utilities.Read(box.DataPointer, toData, 0, toData.Length);
            // Make sure that we unmap the resource in case of an exception
            Context.UnmapSubresource(stagingTexture, subResourceIndex);
        }

        /// <summary>
        /// Copies the content an array of data on CPU memory to this texture into GPU memory.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="buffer">The buffer to set the data to.</param>
        /// <param name="fromData">The data to copy from.</param>
        /// <param name="offsetInBytes">The offset in bytes to write to.</param>
        /// <param name="subResourceIndex">Index of the sub resource.</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <remarks>See the unmanaged documentation for usage and restrictions.</remarks>
        /// <msdn-id>ff476457</msdn-id>
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>
        public unsafe void SetData<TData>(Buffer buffer, ref TData fromData, int offsetInBytes = 0, int subResourceIndex = 0) where TData : struct
        {
            // Check size validity of data to copy to
            if (Utilities.SizeOf<TData>() > buffer.Description.SizeInBytes)
                throw new ArgumentException("Length of TData is larger than size of buffer");

            // Offset in bytes is set to 0 for constant buffers
            offsetInBytes = (buffer.Description.BindFlags & BindFlags.ConstantBuffer) != 0 ? 0 : offsetInBytes;

            // If this texture is declared as default usage, we can only use UpdateSubresource, which is not optimal but better than nothing
            if (buffer.Description.Usage == ResourceUsage.Default)
            {
                // Setup the dest region inside the buffer
                var destRegion = new ResourceRegion(offsetInBytes, 0, 0, offsetInBytes + Utilities.SizeOf<TData>(), 1, 1);
                Context.UpdateSubresource(ref fromData, buffer, subResourceIndex, 0, 0, (buffer.Description.BindFlags & BindFlags.ConstantBuffer) != 0 ? (ResourceRegion?)null : destRegion);
            }
            else
            {
                try
                {
                    var box = Context.MapSubresource(buffer, subResourceIndex, MapMode.WriteDiscard, Direct3D11.MapFlags.None);
                    Utilities.Write((IntPtr)((byte*)box.DataPointer + offsetInBytes), ref fromData);
                }
                finally
                {
                    Context.UnmapSubresource(buffer, subResourceIndex);
                }
            }
        }

        /// <summary>
        /// Copies the content an array of data on CPU memory to this texture into GPU memory.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="buffer">The buffer to set the data to.</param>
        /// <param name="fromData">The data to copy from.</param>
        /// <param name="offsetInBytes">The offset in bytes to write to.</param>
        /// <param name="subResourceIndex">Index of the sub resource.</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <remarks>See the unmanaged documentation for usage and restrictions.</remarks>
        /// <msdn-id>ff476457</msdn-id>
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>
        public unsafe void SetData<TData>(Buffer buffer, TData[] fromData, int offsetInBytes = 0, int subResourceIndex = 0) where TData : struct
        {
            // Check size validity of data to copy to
            if ((fromData.Length * Utilities.SizeOf<TData>()) > buffer.Description.SizeInBytes)
                throw new ArgumentException("Length of TData is larger than size of buffer");

            // If this texture is declared as default usage, we can only use UpdateSubresource, which is not optimal but better than nothing
            if (buffer.Description.Usage == ResourceUsage.Default)
            {
                // Setup the dest region inside the buffer
                var destRegion = new ResourceRegion(offsetInBytes, 0, 0, offsetInBytes + fromData.Length * Utilities.SizeOf<TData>(), 1, 1);
                Context.UpdateSubresource(fromData, buffer, subResourceIndex, 0, 0, (buffer.Description.BindFlags & BindFlags.ConstantBuffer) != 0 ? (ResourceRegion?)null : destRegion);
            }
            else
            {
                try
                {
                    var box = Context.MapSubresource(buffer, subResourceIndex, MapMode.WriteDiscard, Direct3D11.MapFlags.None);
                    Utilities.Write((IntPtr)((byte*)box.DataPointer + offsetInBytes), fromData, 0, fromData.Length);
                }
                finally
                {
                    Context.UnmapSubresource(buffer, subResourceIndex);
                }
            }
        }

        /// <summary>
        /// Gets the content of this texture to an array of data.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="texture">The texture.</param>
        /// <param name="subResourceIndex">Index of the subresource to copy from.</param>
        /// <returns></returns>
        /// <msdn-id>ff476457</msdn-id>
        ///   <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>
        ///   <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>
        /// <remarks>This method creates internally a stagging resource, copies to it and map it to memory. Use method with explicit staging resource
        /// for optimal performances.</remarks>
        public TData[] GetData<TData>(Texture2DBase texture, int subResourceIndex = 0) where TData : struct
        {
            var toData = new TData[CalculateElementWidth<TData>(texture) * texture.Description.Height];
            GetData(texture, toData, subResourceIndex);
            return toData;
        }

        /// <summary>
        /// Copies the content of this texture to an array of data.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="toData">The destination buffer to receive a copy of the texture datas.</param>
        /// <param name="subResourceIndex">Index of the subresource to copy from.</param>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        /// <remarks>
        /// This method creates internally a stagging resource if this texture is not already a stagging resouce, copies to it and map it to memory. Use method with explicit staging resource
        /// for optimal performances.
        /// </remarks>
        public void GetData<TData>(Texture2DBase texture, TData[] toData, int subResourceIndex = 0) where TData : struct
        {
            // Get data from this resource
            if (texture.Description.Usage == ResourceUsage.Staging)
            {
                // Directly if this is a staging resource
                GetData(texture, texture, toData, subResourceIndex);
            }
            else
            {
                // Unefficient way to use the Copy method using dynamic staging texture
                using (var throughStaging = texture.ToStaging())
                    GetData(texture, throughStaging, toData, subResourceIndex);
            }
        }

        /// <summary>
        /// Copies the content of this texture from GPU memory to an array of data on CPU memory using a specific staging resource.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <param name="texture">The texture to get the data from.</param>
        /// <param name="stagingTexture">The staging texture used to transfer the texture to.</param>
        /// <param name="toData">To data.</param>
        /// <param name="subResourceIndex">Index of the sub resource.</param>
        /// <exception cref="System.ArgumentException">When strides is different from optimal strides, and TData is not the same size as the pixel format, or Width * Height != toData.Length</exception>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        /// <remarks>
        /// See unmanaged documentation for usage and restrictions.
        /// </remarks>
        public unsafe void GetData<TData>(Texture2DBase texture, Texture2DBase stagingTexture, TData[] toData, int subResourceIndex = 0) where TData : struct
        {
            // Check size validity of data to copy to
            if ((toData.Length * Utilities.SizeOf<TData>()) != (texture.RowStride * texture.Description.Height))
                throw new ArgumentException("Length of TData is not compatible with Width * Height * Pixel size in bytes");

            // Copy the texture to a staging resource
            Context.CopyResource(texture, stagingTexture);

            try
            {
                int width = CalculateElementWidth<TData>(texture);

                // Map the staging resource to a CPU accessible memory
                var box = Context.MapSubresource(stagingTexture, subResourceIndex, MapMode.Read, MapFlags.None);

                // The fast way: If same stride, we can directly copy the whole texture in one shot
                if (box.RowPitch == texture.RowStride)
                {
                    Utilities.Read(box.DataPointer, toData, 0, toData.Length);
                }
                else
                {
                    // Otherwise, the long way by copying each scanline
                    int offsetStride = 0;
                    var sourcePtr = (byte*)box.DataPointer;

                    for (int i = 0; i < texture.Description.Height; i++)
                    {
                        Utilities.Read((IntPtr)sourcePtr, toData, offsetStride, width);
                        sourcePtr += box.RowPitch;
                        offsetStride += width;
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
        /// <param name="subResourceIndex">Index of the sub resource.</param>
        /// <exception cref="System.ArgumentException">When strides is different from optimal strides, and TData is not the same size as the pixel format, or Width * Height != toData.Length</exception>
        /// <msdn-id>ff476457</msdn-id>	
        /// <unmanaged>HRESULT ID3D11DeviceContext::Map([In] ID3D11Resource* pResource,[In] unsigned int Subresource,[In] D3D11_MAP MapType,[In] D3D11_MAP_FLAG MapFlags,[Out] D3D11_MAPPED_SUBRESOURCE* pMappedResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Map</unmanaged-short>	
        /// <remarks>
        /// See unmanaged documentation for usage and restrictions.
        /// </remarks>
        public unsafe void SetData<TData>(Texture2DBase texture, TData[] fromData, int subResourceIndex = 0) where TData : struct
        {
            // Check size validity of data to copy to
            if ((fromData.Length * Utilities.SizeOf<TData>()) != (texture.RowStride * texture.Description.Height))
                throw new ArgumentException("Length of TData is not compatible with Width * Height * Pixel size in bytes");

            // If this texture is declared as default usage, we can only use UpdateSubresource, which is not optimal but better than nothing
            if (texture.Description.Usage == ResourceUsage.Default)
            {
                Context.UpdateSubresource(fromData, texture, subResourceIndex, texture.RowStride);
            }
            else
            {
                try
                {
                    int width = CalculateElementWidth<TData>(texture);
                    var box = Context.MapSubresource(texture, subResourceIndex, MapMode.WriteDiscard,
                                                     MapFlags.None);
                    // The fast way: If same stride, we can directly copy the whole texture in one shot
                    if (box.RowPitch == texture.RowStride)
                    {
                        Utilities.Write(box.DataPointer, fromData, 0, fromData.Length);
                    }
                    else
                    {
                        // Otherwise, the long way by copying each scanline
                        int offsetStride = 0;
                        var destPtr = (byte*)box.DataPointer;

                        for (int i = 0; i < texture.Description.Height; i++)
                        {
                            Utilities.Write((IntPtr)destPtr, fromData, offsetStride, width);
                            destPtr += box.RowPitch;
                            offsetStride += width;
                        }

                    }
                }
                finally
                {
                    Context.UnmapSubresource(texture, subResourceIndex);
                }
            }
        }

        public static implicit operator Device(GraphicsDevice from)
        {
            return from.Device;
        }

        public static implicit operator DeviceContext(GraphicsDevice from)
        {
            return from.Context;
        }

        /// <summary>
        /// Calculates the width of the element.
        /// </summary>
        /// <typeparam name="TData">The type of the T data.</typeparam>
        /// <returns>The width</returns>
        /// <exception cref="System.ArgumentException">If the size is invalid</exception>
        private int CalculateElementWidth<TData>(Texture2DBase texture) where TData : struct
        {
            var dataStrideInBytes = Utilities.SizeOf<TData>() * texture.Description.Width;
            var width = ((double)texture.RowStride / dataStrideInBytes) * texture.Description.Width;
            if (Math.Abs(width - (int)width) > double.Epsilon)
                throw new ArgumentException("sizeof(TData) / sizeof(Format) * Width is not an integer");
            return (int)width;
        }
    }
}
