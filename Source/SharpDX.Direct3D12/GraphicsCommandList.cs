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

namespace SharpDX.Direct3D12
{
    public partial class GraphicsCommandList
    {
        /// <summary>	
        /// <p>Clears the depth-stencil resource.</p>	
        /// </summary>	
        /// <param name="depthStencilView"><dd>  <p> Describes the CPU descriptor handle that represents the start of the heap for the depth stencil to be cleared. </p> </dd></param>	
        /// <param name="clearFlags"><dd>  <p> A combination of <strong><see cref="SharpDX.Direct3D12.ClearFlags"/></strong> values that are combined by using a bitwise OR operation. The resulting value identifies the type of data to clear (depth buffer, stencil buffer, or both). </p> </dd></param>	
        /// <param name="depth"><dd>  <p> A value to clear the depth buffer with. This value will be clamped between 0 and 1. </p> </dd></param>	
        /// <param name="stencil"><dd>  <p> A value to clear the stencil buffer with. </p> </dd></param>	
        /// <param name="numRects"><dd>  <p> The number of rectangles in the array that the <em>pRects</em> parameter specifies. </p> </dd></param>	
        /// <param name="rectsRef"><dd>  <p> An array of <strong>D3D12_RECT</strong> structures for the rectangles in the resource view to clear. If <strong><c>null</c></strong>, <strong>ClearDepthStencilView</strong> clears the entire resource view. </p> </dd></param>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12GraphicsCommandList::ClearDepthStencilView']/*"/>	
        /// <msdn-id>dn903840</msdn-id>	
        /// <unmanaged>void ID3D12GraphicsCommandList::ClearDepthStencilView([In] D3D12_CPU_DESCRIPTOR_HANDLE DepthStencilView,[In] D3D12_CLEAR_FLAGS ClearFlags,[In] float Depth,[In] unsigned char Stencil,[In] unsigned int NumRects,[In, Buffer] const RECT* pRects)</unmanaged>	
        /// <unmanaged-short>ID3D12GraphicsCommandList::ClearDepthStencilView</unmanaged-short>	
        public void ClearDepthStencilView(SharpDX.Direct3D12.CpuDescriptorHandle depthStencilView, SharpDX.Direct3D12.ClearFlags clearFlags, float depth, byte stencil)
        {
            ClearDepthStencilView(depthStencilView, clearFlags, depth, stencil, 0, null);
        }

        /// <summary>	
        /// <p> Sets all the elements in a render target to one value. </p>	
        /// </summary>	
        /// <param name="renderTargetView"><dd>  <p> Specifies a <see cref="SharpDX.Direct3D12.CpuDescriptorHandle"/> structure that describes the CPU descriptor handle that represents the start of the heap for the render target to be cleared. </p> </dd></param>	
        /// <param name="colorRGBA"><dd>  <p> A 4-component array that represents the color to fill the render target with. </p> </dd></param>	
        /// <param name="numRects"><dd>  <p> The number of rectangles in the array that the <em>pRects</em> parameter specifies. </p> </dd></param>	
        /// <param name="rectsRef"><dd>  <p> An array of <strong>D3D12_RECT</strong> structures for the rectangles in the resource view to clear. If <strong><c>null</c></strong>, <strong>ClearRenderTargetView</strong> clears the entire resource view. </p> </dd></param>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12GraphicsCommandList::ClearRenderTargetView']/*"/>	
        /// <msdn-id>dn903842</msdn-id>	
        /// <unmanaged>void ID3D12GraphicsCommandList::ClearRenderTargetView([In] D3D12_CPU_DESCRIPTOR_HANDLE RenderTargetView,[In] const SHARPDX_COLOR4* ColorRGBA,[In] unsigned int NumRects,[In, Buffer] const RECT* pRects)</unmanaged>	
        /// <unmanaged-short>ID3D12GraphicsCommandList::ClearRenderTargetView</unmanaged-short>	
        public void ClearRenderTargetView(CpuDescriptorHandle renderTargetView, Mathematics.Interop.RawColor4 colorRGBA)
        {
            ClearRenderTargetView(renderTargetView, colorRGBA, 0, null);
        }

        /// <summary>	
        /// <p> Notifies the driver that it needs to synchronize multiple accesses to resources. </p>	
        /// </summary>	
        /// <param name="numBarriers"><dd>  <p> The number of submitted barrier descriptions. </p> </dd></param>	
        /// <param name="barriersRef"><dd>  <p> Pointer to an array of barrier descriptions. </p> </dd></param>	
        /// <remarks>	
        /// <p>There are three types of barrier descriptions:</p><ul> <li> <strong><see cref="SharpDX.Direct3D12.ResourceTransitionBarrier"/></strong> -  Transition barriers  indicate that a set of subresources transition between different usages.  The caller must specify the <em>before</em> and <em>after</em> usages of the subresources.  The D3D12_RESOURCE_BARRIER_ALL_SUBRESOURCES flag is used to transition all subresources in a resource at the same time. </li> <li> <strong><see cref="SharpDX.Direct3D12.ResourceAliasingBarrier"/></strong> - Aliasing barriers indicate a transition between usages of two different resources which have mappings into the same heap.  The application can specify both the before and the after resource.  Note that one or both resources can be <c>null</c> (indicating that any tiled resource could cause aliasing). </li> <li> <strong><see cref="SharpDX.Direct3D12.ResourceUnorderedAccessViewBarrier"/></strong> - Unordered access view barriers indicate all UAV accesses (read or writes) to a particular resource must complete before any future UAV accesses (read or write) can begin.  The specified resource cannot be <c>null</c>.  It is not necessary to insert a UAV barrier between two draw or dispatch calls which only read a UAV.  Additionally, it is not necessary to insert a UAV barrier between two draw or dispatch calls which write to the same UAV if the application knows that it is safe to execute the UAV accesses in any order.  The resource can be <c>null</c> (indicating that any UAV access could require the barrier). </li> </ul><p> When <strong><see cref="SharpDX.Direct3D12.GraphicsCommandList.ResourceBarrier"/></strong> is passed an array of resource barrier descriptions, the API behaves as if it was called N times (1 for each array element), in the specified order. </p><p> For descriptions of the usage states a subresource can be in, see the <strong><see cref="SharpDX.Direct3D12.ResourceStates"/></strong> enumeration and the Using Resource Barriers to Synchronize Resource States in Direct3D 12 section. </p><p> A subresource can be in any state when <strong><see cref="SharpDX.Direct3D12.GraphicsCommandList.DiscardResource"/></strong> is called. </p><p> When a back buffer is presented, it must be in the <see cref="SharpDX.Direct3D12.ResourceStates.Present"/> state.  If <strong>Present</strong> is called on a resource which is not in the PRESENT state, a debug layer warning will be emitted. </p><p>The resource usage bits are group into two categories, read-only and read/write.</p><p> The following usage bits are read-only: </p><ul> <li><see cref="SharpDX.Direct3D12.ResourceStates.VertexAndConstantBuffer"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.IndexBuffer"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.NonPixelShaderResource"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.PixelShaderResource"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.IndirectArgument"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.CopySource"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.DepthRead"/></li> </ul><p>The following usage bits are read/write:</p><ul> <li><see cref="SharpDX.Direct3D12.ResourceStates.CopyDestination"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.RenderTarget"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.UnorderedAccess"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.DepthWrite"/></li> <li>D3D12_RESOURCE_STATE_GENERATE_MIPS</li> <li><see cref="SharpDX.Direct3D12.ResourceStates.StreamOut"/></li> </ul><p> At most one write bit can be set. If any write bit is set, then no read bit may be set. If no write bit is set, then any number of read bits may be set.  </p><p> At any given time, a subresource is in exactly one  state (determined by a set of flags).  The application must ensure that the states are matched when making a sequence of <strong>ResourceBarrier</strong> calls. In other words, the before and after states in consecutive calls to <strong>ResourceBarrier</strong> must agree. </p><p>To transition all subresources within a resource, the application can set the subresource index to D3D12_RESOURCE_BARRIER_ALL_SUBRESOURCES, which implies that all subresources are changed.</p><p> For improved performance, applications should use split barriers (refer to Synchronization and Multi-Engine). Applications should also batch multiple transitions into a single call whenever possible. </p>	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12GraphicsCommandList::ResourceBarrier']/*"/>	
        /// <msdn-id>dn903898</msdn-id>	
        /// <unmanaged>void ID3D12GraphicsCommandList::ResourceBarrier([In] unsigned int NumBarriers,[In, Buffer] const void* pBarriers)</unmanaged>	
        /// <unmanaged-short>ID3D12GraphicsCommandList::ResourceBarrier</unmanaged-short>	
        public void ResourceBarrierTransition(Resource resource, ResourceStates stateBefore, ResourceStates stateAfter)
        {
            ResourceBarrierTransition(resource, -1, stateBefore, stateAfter);
        }

        /// <summary>	
        /// <p> Notifies the driver that it needs to synchronize multiple accesses to resources. </p>	
        /// </summary>	
        /// <param name="numBarriers"><dd>  <p> The number of submitted barrier descriptions. </p> </dd></param>	
        /// <param name="barriersRef"><dd>  <p> Pointer to an array of barrier descriptions. </p> </dd></param>	
        /// <remarks>	
        /// <p>There are three types of barrier descriptions:</p><ul> <li> <strong><see cref="SharpDX.Direct3D12.ResourceTransitionBarrier"/></strong> -  Transition barriers  indicate that a set of subresources transition between different usages.  The caller must specify the <em>before</em> and <em>after</em> usages of the subresources.  The D3D12_RESOURCE_BARRIER_ALL_SUBRESOURCES flag is used to transition all subresources in a resource at the same time. </li> <li> <strong><see cref="SharpDX.Direct3D12.ResourceAliasingBarrier"/></strong> - Aliasing barriers indicate a transition between usages of two different resources which have mappings into the same heap.  The application can specify both the before and the after resource.  Note that one or both resources can be <c>null</c> (indicating that any tiled resource could cause aliasing). </li> <li> <strong><see cref="SharpDX.Direct3D12.ResourceUnorderedAccessViewBarrier"/></strong> - Unordered access view barriers indicate all UAV accesses (read or writes) to a particular resource must complete before any future UAV accesses (read or write) can begin.  The specified resource cannot be <c>null</c>.  It is not necessary to insert a UAV barrier between two draw or dispatch calls which only read a UAV.  Additionally, it is not necessary to insert a UAV barrier between two draw or dispatch calls which write to the same UAV if the application knows that it is safe to execute the UAV accesses in any order.  The resource can be <c>null</c> (indicating that any UAV access could require the barrier). </li> </ul><p> When <strong><see cref="SharpDX.Direct3D12.GraphicsCommandList.ResourceBarrier"/></strong> is passed an array of resource barrier descriptions, the API behaves as if it was called N times (1 for each array element), in the specified order. </p><p> For descriptions of the usage states a subresource can be in, see the <strong><see cref="SharpDX.Direct3D12.ResourceStates"/></strong> enumeration and the Using Resource Barriers to Synchronize Resource States in Direct3D 12 section. </p><p> A subresource can be in any state when <strong><see cref="SharpDX.Direct3D12.GraphicsCommandList.DiscardResource"/></strong> is called. </p><p> When a back buffer is presented, it must be in the <see cref="SharpDX.Direct3D12.ResourceStates.Present"/> state.  If <strong>Present</strong> is called on a resource which is not in the PRESENT state, a debug layer warning will be emitted. </p><p>The resource usage bits are group into two categories, read-only and read/write.</p><p> The following usage bits are read-only: </p><ul> <li><see cref="SharpDX.Direct3D12.ResourceStates.VertexAndConstantBuffer"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.IndexBuffer"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.NonPixelShaderResource"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.PixelShaderResource"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.IndirectArgument"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.CopySource"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.DepthRead"/></li> </ul><p>The following usage bits are read/write:</p><ul> <li><see cref="SharpDX.Direct3D12.ResourceStates.CopyDestination"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.RenderTarget"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.UnorderedAccess"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.DepthWrite"/></li> <li>D3D12_RESOURCE_STATE_GENERATE_MIPS</li> <li><see cref="SharpDX.Direct3D12.ResourceStates.StreamOut"/></li> </ul><p> At most one write bit can be set. If any write bit is set, then no read bit may be set. If no write bit is set, then any number of read bits may be set.  </p><p> At any given time, a subresource is in exactly one  state (determined by a set of flags).  The application must ensure that the states are matched when making a sequence of <strong>ResourceBarrier</strong> calls. In other words, the before and after states in consecutive calls to <strong>ResourceBarrier</strong> must agree. </p><p>To transition all subresources within a resource, the application can set the subresource index to D3D12_RESOURCE_BARRIER_ALL_SUBRESOURCES, which implies that all subresources are changed.</p><p> For improved performance, applications should use split barriers (refer to Synchronization and Multi-Engine). Applications should also batch multiple transitions into a single call whenever possible. </p>	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12GraphicsCommandList::ResourceBarrier']/*"/>	
        /// <msdn-id>dn903898</msdn-id>	
        /// <unmanaged>void ID3D12GraphicsCommandList::ResourceBarrier([In] unsigned int NumBarriers,[In, Buffer] const void* pBarriers)</unmanaged>	
        /// <unmanaged-short>ID3D12GraphicsCommandList::ResourceBarrier</unmanaged-short>	
        public unsafe void ResourceBarrierTransition(Resource resource, int subresource, ResourceStates stateBefore, ResourceStates stateAfter)
        {
            var barrier = new ResourceBarrier(new ResourceTransitionBarrier(resource, subresource, stateBefore, stateAfter));
            ResourceBarrier(1, new IntPtr(&barrier));
        }

        /// <summary>	
        /// <p> Notifies the driver that it needs to synchronize multiple accesses to resources. </p>	
        /// </summary>	
        /// <param name="numBarriers"><dd>  <p> The number of submitted barrier descriptions. </p> </dd></param>	
        /// <param name="barriersRef"><dd>  <p> Pointer to an array of barrier descriptions. </p> </dd></param>	
        /// <remarks>	
        /// <p>There are three types of barrier descriptions:</p><ul> <li> <strong><see cref="SharpDX.Direct3D12.ResourceTransitionBarrier"/></strong> -  Transition barriers  indicate that a set of subresources transition between different usages.  The caller must specify the <em>before</em> and <em>after</em> usages of the subresources.  The D3D12_RESOURCE_BARRIER_ALL_SUBRESOURCES flag is used to transition all subresources in a resource at the same time. </li> <li> <strong><see cref="SharpDX.Direct3D12.ResourceAliasingBarrier"/></strong> - Aliasing barriers indicate a transition between usages of two different resources which have mappings into the same heap.  The application can specify both the before and the after resource.  Note that one or both resources can be <c>null</c> (indicating that any tiled resource could cause aliasing). </li> <li> <strong><see cref="SharpDX.Direct3D12.ResourceUnorderedAccessViewBarrier"/></strong> - Unordered access view barriers indicate all UAV accesses (read or writes) to a particular resource must complete before any future UAV accesses (read or write) can begin.  The specified resource cannot be <c>null</c>.  It is not necessary to insert a UAV barrier between two draw or dispatch calls which only read a UAV.  Additionally, it is not necessary to insert a UAV barrier between two draw or dispatch calls which write to the same UAV if the application knows that it is safe to execute the UAV accesses in any order.  The resource can be <c>null</c> (indicating that any UAV access could require the barrier). </li> </ul><p> When <strong><see cref="SharpDX.Direct3D12.GraphicsCommandList.ResourceBarrier"/></strong> is passed an array of resource barrier descriptions, the API behaves as if it was called N times (1 for each array element), in the specified order. </p><p> For descriptions of the usage states a subresource can be in, see the <strong><see cref="SharpDX.Direct3D12.ResourceStates"/></strong> enumeration and the Using Resource Barriers to Synchronize Resource States in Direct3D 12 section. </p><p> A subresource can be in any state when <strong><see cref="SharpDX.Direct3D12.GraphicsCommandList.DiscardResource"/></strong> is called. </p><p> When a back buffer is presented, it must be in the <see cref="SharpDX.Direct3D12.ResourceStates.Present"/> state.  If <strong>Present</strong> is called on a resource which is not in the PRESENT state, a debug layer warning will be emitted. </p><p>The resource usage bits are group into two categories, read-only and read/write.</p><p> The following usage bits are read-only: </p><ul> <li><see cref="SharpDX.Direct3D12.ResourceStates.VertexAndConstantBuffer"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.IndexBuffer"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.NonPixelShaderResource"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.PixelShaderResource"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.IndirectArgument"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.CopySource"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.DepthRead"/></li> </ul><p>The following usage bits are read/write:</p><ul> <li><see cref="SharpDX.Direct3D12.ResourceStates.CopyDestination"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.RenderTarget"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.UnorderedAccess"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.DepthWrite"/></li> <li>D3D12_RESOURCE_STATE_GENERATE_MIPS</li> <li><see cref="SharpDX.Direct3D12.ResourceStates.StreamOut"/></li> </ul><p> At most one write bit can be set. If any write bit is set, then no read bit may be set. If no write bit is set, then any number of read bits may be set.  </p><p> At any given time, a subresource is in exactly one  state (determined by a set of flags).  The application must ensure that the states are matched when making a sequence of <strong>ResourceBarrier</strong> calls. In other words, the before and after states in consecutive calls to <strong>ResourceBarrier</strong> must agree. </p><p>To transition all subresources within a resource, the application can set the subresource index to D3D12_RESOURCE_BARRIER_ALL_SUBRESOURCES, which implies that all subresources are changed.</p><p> For improved performance, applications should use split barriers (refer to Synchronization and Multi-Engine). Applications should also batch multiple transitions into a single call whenever possible. </p>	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12GraphicsCommandList::ResourceBarrier']/*"/>	
        /// <msdn-id>dn903898</msdn-id>	
        /// <unmanaged>void ID3D12GraphicsCommandList::ResourceBarrier([In] unsigned int NumBarriers,[In, Buffer] const void* pBarriers)</unmanaged>	
        /// <unmanaged-short>ID3D12GraphicsCommandList::ResourceBarrier</unmanaged-short>	
        public unsafe void ResourceBarrierAliasing(Resource resourceBefore, Resource resourceAfter)
        {
            var barrier = new ResourceBarrier(new ResourceAliasingBarrier(resourceBefore, resourceAfter));
            ResourceBarrier(1, new IntPtr(&barrier));
        }

        /// <summary>	
        /// <p> Notifies the driver that it needs to synchronize multiple accesses to resources. </p>	
        /// </summary>	
        /// <param name="numBarriers"><dd>  <p> The number of submitted barrier descriptions. </p> </dd></param>	
        /// <param name="barriersRef"><dd>  <p> Pointer to an array of barrier descriptions. </p> </dd></param>	
        /// <remarks>	
        /// <p>There are three types of barrier descriptions:</p><ul> <li> <strong><see cref="SharpDX.Direct3D12.ResourceTransitionBarrier"/></strong> -  Transition barriers  indicate that a set of subresources transition between different usages.  The caller must specify the <em>before</em> and <em>after</em> usages of the subresources.  The D3D12_RESOURCE_BARRIER_ALL_SUBRESOURCES flag is used to transition all subresources in a resource at the same time. </li> <li> <strong><see cref="SharpDX.Direct3D12.ResourceAliasingBarrier"/></strong> - Aliasing barriers indicate a transition between usages of two different resources which have mappings into the same heap.  The application can specify both the before and the after resource.  Note that one or both resources can be <c>null</c> (indicating that any tiled resource could cause aliasing). </li> <li> <strong><see cref="SharpDX.Direct3D12.ResourceUnorderedAccessViewBarrier"/></strong> - Unordered access view barriers indicate all UAV accesses (read or writes) to a particular resource must complete before any future UAV accesses (read or write) can begin.  The specified resource cannot be <c>null</c>.  It is not necessary to insert a UAV barrier between two draw or dispatch calls which only read a UAV.  Additionally, it is not necessary to insert a UAV barrier between two draw or dispatch calls which write to the same UAV if the application knows that it is safe to execute the UAV accesses in any order.  The resource can be <c>null</c> (indicating that any UAV access could require the barrier). </li> </ul><p> When <strong><see cref="SharpDX.Direct3D12.GraphicsCommandList.ResourceBarrier"/></strong> is passed an array of resource barrier descriptions, the API behaves as if it was called N times (1 for each array element), in the specified order. </p><p> For descriptions of the usage states a subresource can be in, see the <strong><see cref="SharpDX.Direct3D12.ResourceStates"/></strong> enumeration and the Using Resource Barriers to Synchronize Resource States in Direct3D 12 section. </p><p> A subresource can be in any state when <strong><see cref="SharpDX.Direct3D12.GraphicsCommandList.DiscardResource"/></strong> is called. </p><p> When a back buffer is presented, it must be in the <see cref="SharpDX.Direct3D12.ResourceStates.Present"/> state.  If <strong>Present</strong> is called on a resource which is not in the PRESENT state, a debug layer warning will be emitted. </p><p>The resource usage bits are group into two categories, read-only and read/write.</p><p> The following usage bits are read-only: </p><ul> <li><see cref="SharpDX.Direct3D12.ResourceStates.VertexAndConstantBuffer"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.IndexBuffer"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.NonPixelShaderResource"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.PixelShaderResource"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.IndirectArgument"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.CopySource"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.DepthRead"/></li> </ul><p>The following usage bits are read/write:</p><ul> <li><see cref="SharpDX.Direct3D12.ResourceStates.CopyDestination"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.RenderTarget"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.UnorderedAccess"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.DepthWrite"/></li> <li>D3D12_RESOURCE_STATE_GENERATE_MIPS</li> <li><see cref="SharpDX.Direct3D12.ResourceStates.StreamOut"/></li> </ul><p> At most one write bit can be set. If any write bit is set, then no read bit may be set. If no write bit is set, then any number of read bits may be set.  </p><p> At any given time, a subresource is in exactly one  state (determined by a set of flags).  The application must ensure that the states are matched when making a sequence of <strong>ResourceBarrier</strong> calls. In other words, the before and after states in consecutive calls to <strong>ResourceBarrier</strong> must agree. </p><p>To transition all subresources within a resource, the application can set the subresource index to D3D12_RESOURCE_BARRIER_ALL_SUBRESOURCES, which implies that all subresources are changed.</p><p> For improved performance, applications should use split barriers (refer to Synchronization and Multi-Engine). Applications should also batch multiple transitions into a single call whenever possible. </p>	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12GraphicsCommandList::ResourceBarrier']/*"/>	
        /// <msdn-id>dn903898</msdn-id>	
        /// <unmanaged>void ID3D12GraphicsCommandList::ResourceBarrier([In] unsigned int NumBarriers,[In, Buffer] const void* pBarriers)</unmanaged>	
        /// <unmanaged-short>ID3D12GraphicsCommandList::ResourceBarrier</unmanaged-short>	
        public unsafe void ResourceBarrier(SharpDX.Direct3D12.ResourceBarrier barrier)
        {
            ResourceBarrier(1, new IntPtr(&barrier));
        }

        /// <summary>	
        /// <p> Notifies the driver that it needs to synchronize multiple accesses to resources. </p>	
        /// </summary>	
        /// <param name="numBarriers"><dd>  <p> The number of submitted barrier descriptions. </p> </dd></param>	
        /// <param name="barriersRef"><dd>  <p> Pointer to an array of barrier descriptions. </p> </dd></param>	
        /// <remarks>	
        /// <p>There are three types of barrier descriptions:</p><ul> <li> <strong><see cref="SharpDX.Direct3D12.ResourceTransitionBarrier"/></strong> -  Transition barriers  indicate that a set of subresources transition between different usages.  The caller must specify the <em>before</em> and <em>after</em> usages of the subresources.  The D3D12_RESOURCE_BARRIER_ALL_SUBRESOURCES flag is used to transition all subresources in a resource at the same time. </li> <li> <strong><see cref="SharpDX.Direct3D12.ResourceAliasingBarrier"/></strong> - Aliasing barriers indicate a transition between usages of two different resources which have mappings into the same heap.  The application can specify both the before and the after resource.  Note that one or both resources can be <c>null</c> (indicating that any tiled resource could cause aliasing). </li> <li> <strong><see cref="SharpDX.Direct3D12.ResourceUnorderedAccessViewBarrier"/></strong> - Unordered access view barriers indicate all UAV accesses (read or writes) to a particular resource must complete before any future UAV accesses (read or write) can begin.  The specified resource cannot be <c>null</c>.  It is not necessary to insert a UAV barrier between two draw or dispatch calls which only read a UAV.  Additionally, it is not necessary to insert a UAV barrier between two draw or dispatch calls which write to the same UAV if the application knows that it is safe to execute the UAV accesses in any order.  The resource can be <c>null</c> (indicating that any UAV access could require the barrier). </li> </ul><p> When <strong><see cref="SharpDX.Direct3D12.GraphicsCommandList.ResourceBarrier"/></strong> is passed an array of resource barrier descriptions, the API behaves as if it was called N times (1 for each array element), in the specified order. </p><p> For descriptions of the usage states a subresource can be in, see the <strong><see cref="SharpDX.Direct3D12.ResourceStates"/></strong> enumeration and the Using Resource Barriers to Synchronize Resource States in Direct3D 12 section. </p><p> A subresource can be in any state when <strong><see cref="SharpDX.Direct3D12.GraphicsCommandList.DiscardResource"/></strong> is called. </p><p> When a back buffer is presented, it must be in the <see cref="SharpDX.Direct3D12.ResourceStates.Present"/> state.  If <strong>Present</strong> is called on a resource which is not in the PRESENT state, a debug layer warning will be emitted. </p><p>The resource usage bits are group into two categories, read-only and read/write.</p><p> The following usage bits are read-only: </p><ul> <li><see cref="SharpDX.Direct3D12.ResourceStates.VertexAndConstantBuffer"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.IndexBuffer"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.NonPixelShaderResource"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.PixelShaderResource"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.IndirectArgument"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.CopySource"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.DepthRead"/></li> </ul><p>The following usage bits are read/write:</p><ul> <li><see cref="SharpDX.Direct3D12.ResourceStates.CopyDestination"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.RenderTarget"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.UnorderedAccess"/></li> <li><see cref="SharpDX.Direct3D12.ResourceStates.DepthWrite"/></li> <li>D3D12_RESOURCE_STATE_GENERATE_MIPS</li> <li><see cref="SharpDX.Direct3D12.ResourceStates.StreamOut"/></li> </ul><p> At most one write bit can be set. If any write bit is set, then no read bit may be set. If no write bit is set, then any number of read bits may be set.  </p><p> At any given time, a subresource is in exactly one  state (determined by a set of flags).  The application must ensure that the states are matched when making a sequence of <strong>ResourceBarrier</strong> calls. In other words, the before and after states in consecutive calls to <strong>ResourceBarrier</strong> must agree. </p><p>To transition all subresources within a resource, the application can set the subresource index to D3D12_RESOURCE_BARRIER_ALL_SUBRESOURCES, which implies that all subresources are changed.</p><p> For improved performance, applications should use split barriers (refer to Synchronization and Multi-Engine). Applications should also batch multiple transitions into a single call whenever possible. </p>	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12GraphicsCommandList::ResourceBarrier']/*"/>	
        /// <msdn-id>dn903898</msdn-id>	
        /// <unmanaged>void ID3D12GraphicsCommandList::ResourceBarrier([In] unsigned int NumBarriers,[In, Buffer] const void* pBarriers)</unmanaged>	
        /// <unmanaged-short>ID3D12GraphicsCommandList::ResourceBarrier</unmanaged-short>	
        public unsafe void ResourceBarrier(params SharpDX.Direct3D12.ResourceBarrier[] barriers)
        {
            if (barriers == null) throw new ArgumentNullException("barriers");

            fixed (void* pBarriers = barriers)
                ResourceBarrier(barriers.Length, new IntPtr(pBarriers));
        }

        /// <summary>	
        /// <p> Changes the currently bound descriptor heaps that are associated with a command list. </p>	
        /// </summary>	
        /// <param name="descriptorHeaps"><dd>  <p> A reference to an array of <strong><see cref="SharpDX.Direct3D12.DescriptorHeap"/></strong> objects for the heaps to set on the command list. </p> </dd></param>	
        /// <remarks>	
        /// <p><strong>SetDescriptorHeaps</strong> can be called on a bundle, but the bundle descriptor heaps must match the calling command list descriptor heap. For more information on bundle restrictions, refer to Creating and Recording Command Lists and Bundles.</p>	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12GraphicsCommandList::SetDescriptorHeaps']/*"/>	
        /// <msdn-id>Dn903908</msdn-id>	
        /// <unmanaged>void ID3D12GraphicsCommandList::SetDescriptorHeaps([In] unsigned int NumDescriptorHeaps,[In, Buffer] const ID3D12DescriptorHeap** ppDescriptorHeaps)</unmanaged>	
        /// <unmanaged-short>ID3D12GraphicsCommandList::SetDescriptorHeaps</unmanaged-short>	
        public void SetDescriptorHeaps(params SharpDX.Direct3D12.DescriptorHeap[] descriptorHeaps)
        {
            SetDescriptorHeaps(descriptorHeaps.Length, descriptorHeaps);
        }

        /// <summary>	
        /// <p> Sets CPU descriptor handles for the render targets and depth stencil. </p>	
        /// </summary>	
        /// <param name="numRenderTargetDescriptors"><dd>  <p> The number of entries in the <em>pRenderTargetDescriptors</em> array. </p> </dd></param>	
        /// <param name="renderTargetDescriptorsRef"><dd>  <p> Specifies an array of <strong><see cref="SharpDX.Direct3D12.CpuDescriptorHandle"/></strong> structures that describe the CPU descriptor handles that represents the start of the heap of render target descriptors. </p> </dd></param>	
        /// <param name="rTsSingleHandleToDescriptorRange"><dd>  <p><strong>True</strong> means the handle passed in is the reference to a contiguous range of <em>NumRenderTargetDescriptors</em> descriptors.  This case is useful if the set of descriptors to bind already happens to be contiguous in memory (so all that?s needed is a handle to the first one).  For example, if  <em>NumRenderTargetDescriptors</em> is 3 then the memory layout is taken as follows:</p><p>In this case the driver dereferences the handle and then increments the memory being pointed to.</p> <p><strong>False</strong> means that the handle is the first of an array of <em>NumRenderTargetDescriptors</em> handles.  The false case allows an application to bind a set of descriptors from different locations at once. Again assuming that <em>NumRenderTargetDescriptors</em> is 3, the memory layout is taken as follows:</p><p>In this case the driver dereferences three handles that are expected to be adjacent to each other in memory.</p> </dd></param>	
        /// <param name="depthStencilDescriptorRef"><dd>  <p> A reference to a <strong><see cref="SharpDX.Direct3D12.CpuDescriptorHandle"/></strong> structure that describes the CPU descriptor handle that represents the start of the heap that holds the depth stencil descriptor. </p> </dd></param>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12GraphicsCommandList::OMSetRenderTargets']/*"/>	
        /// <msdn-id>dn986884</msdn-id>	
        /// <unmanaged>void ID3D12GraphicsCommandList::OMSetRenderTargets([In] unsigned int NumRenderTargetDescriptors,[In, Optional] const void* pRenderTargetDescriptors,[In] BOOL RTsSingleHandleToDescriptorRange,[In, Optional] const D3D12_CPU_DESCRIPTOR_HANDLE* pDepthStencilDescriptor)</unmanaged>	
        /// <unmanaged-short>ID3D12GraphicsCommandList::OMSetRenderTargets</unmanaged-short>	
        public unsafe void SetRenderTargets(int numRenderTargetDescriptors,
            CpuDescriptorHandle renderTargetDescriptors,
            SharpDX.Direct3D12.CpuDescriptorHandle? depthStencilDescriptorRef)
        {
            SetRenderTargets(numRenderTargetDescriptors, new IntPtr(&renderTargetDescriptors), true, depthStencilDescriptorRef);
        }

        /// <summary>	
        /// <p> Sets CPU descriptor handles for the render targets and depth stencil. </p>	
        /// </summary>	
        /// <param name="numRenderTargetDescriptors"><dd>  <p> The number of entries in the <em>pRenderTargetDescriptors</em> array. </p> </dd></param>	
        /// <param name="renderTargetDescriptorsRef"><dd>  <p> Specifies an array of <strong><see cref="SharpDX.Direct3D12.CpuDescriptorHandle"/></strong> structures that describe the CPU descriptor handles that represents the start of the heap of render target descriptors. </p> </dd></param>	
        /// <param name="rTsSingleHandleToDescriptorRange"><dd>  <p><strong>True</strong> means the handle passed in is the reference to a contiguous range of <em>NumRenderTargetDescriptors</em> descriptors.  This case is useful if the set of descriptors to bind already happens to be contiguous in memory (so all that?s needed is a handle to the first one).  For example, if  <em>NumRenderTargetDescriptors</em> is 3 then the memory layout is taken as follows:</p><p>In this case the driver dereferences the handle and then increments the memory being pointed to.</p> <p><strong>False</strong> means that the handle is the first of an array of <em>NumRenderTargetDescriptors</em> handles.  The false case allows an application to bind a set of descriptors from different locations at once. Again assuming that <em>NumRenderTargetDescriptors</em> is 3, the memory layout is taken as follows:</p><p>In this case the driver dereferences three handles that are expected to be adjacent to each other in memory.</p> </dd></param>	
        /// <param name="depthStencilDescriptorRef"><dd>  <p> A reference to a <strong><see cref="SharpDX.Direct3D12.CpuDescriptorHandle"/></strong> structure that describes the CPU descriptor handle that represents the start of the heap that holds the depth stencil descriptor. </p> </dd></param>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12GraphicsCommandList::OMSetRenderTargets']/*"/>	
        /// <msdn-id>dn986884</msdn-id>	
        /// <unmanaged>void ID3D12GraphicsCommandList::OMSetRenderTargets([In] unsigned int NumRenderTargetDescriptors,[In, Optional] const void* pRenderTargetDescriptors,[In] BOOL RTsSingleHandleToDescriptorRange,[In, Optional] const D3D12_CPU_DESCRIPTOR_HANDLE* pDepthStencilDescriptor)</unmanaged>	
        /// <unmanaged-short>ID3D12GraphicsCommandList::OMSetRenderTargets</unmanaged-short>	
        public unsafe void SetRenderTargets(CpuDescriptorHandle[] renderTargetDescriptors, SharpDX.Direct3D12.CpuDescriptorHandle? depthStencilDescriptorRef)
        {
            fixed (void* pRT = renderTargetDescriptors)
                SetRenderTargets(renderTargetDescriptors != null ? renderTargetDescriptors.Length : 0, new IntPtr(pRT), false, depthStencilDescriptorRef);
        }

        /// <summary>	
        /// <p> Sets CPU descriptor handles for the render targets and depth stencil. </p>	
        /// </summary>	
        /// <param name="numRenderTargetDescriptors"><dd>  <p> The number of entries in the <em>pRenderTargetDescriptors</em> array. </p> </dd></param>	
        /// <param name="renderTargetDescriptorsRef"><dd>  <p> Specifies an array of <strong><see cref="SharpDX.Direct3D12.CpuDescriptorHandle"/></strong> structures that describe the CPU descriptor handles that represents the start of the heap of render target descriptors. </p> </dd></param>	
        /// <param name="rTsSingleHandleToDescriptorRange"><dd>  <p><strong>True</strong> means the handle passed in is the reference to a contiguous range of <em>NumRenderTargetDescriptors</em> descriptors.  This case is useful if the set of descriptors to bind already happens to be contiguous in memory (so all that?s needed is a handle to the first one).  For example, if  <em>NumRenderTargetDescriptors</em> is 3 then the memory layout is taken as follows:</p><p>In this case the driver dereferences the handle and then increments the memory being pointed to.</p> <p><strong>False</strong> means that the handle is the first of an array of <em>NumRenderTargetDescriptors</em> handles.  The false case allows an application to bind a set of descriptors from different locations at once. Again assuming that <em>NumRenderTargetDescriptors</em> is 3, the memory layout is taken as follows:</p><p>In this case the driver dereferences three handles that are expected to be adjacent to each other in memory.</p> </dd></param>	
        /// <param name="depthStencilDescriptorRef"><dd>  <p> A reference to a <strong><see cref="SharpDX.Direct3D12.CpuDescriptorHandle"/></strong> structure that describes the CPU descriptor handle that represents the start of the heap that holds the depth stencil descriptor. </p> </dd></param>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12GraphicsCommandList::OMSetRenderTargets']/*"/>	
        /// <msdn-id>dn986884</msdn-id>	
        /// <unmanaged>void ID3D12GraphicsCommandList::OMSetRenderTargets([In] unsigned int NumRenderTargetDescriptors,[In, Optional] const void* pRenderTargetDescriptors,[In] BOOL RTsSingleHandleToDescriptorRange,[In, Optional] const D3D12_CPU_DESCRIPTOR_HANDLE* pDepthStencilDescriptor)</unmanaged>	
        /// <unmanaged-short>ID3D12GraphicsCommandList::OMSetRenderTargets</unmanaged-short>	
        public unsafe void SetRenderTargets(CpuDescriptorHandle? renderTargetDescriptor, SharpDX.Direct3D12.CpuDescriptorHandle? depthStencilDescriptorRef)
        {
            var renderTargetDesc = new CpuDescriptorHandle();
            if (renderTargetDescriptor.HasValue)
            {
                renderTargetDesc = renderTargetDescriptor.Value;
            }
            SetRenderTargets(renderTargetDesc.Ptr != PointerSize.Zero ? 1 : 0, renderTargetDescriptor.HasValue ? new IntPtr(&renderTargetDesc) : IntPtr.Zero, false, depthStencilDescriptorRef);
        }

        /// <summary>	
        /// <p>Sets a CPU descriptor handle for the vertex buffers.</p>	
        /// </summary>	
        /// <param name="startSlot"><dd>  <p> Index into the device's zero-based array to begin setting vertex buffers. </p> </dd></param>	
        /// <param name="vertexBufferViews"><dd>  <p> Specifies the vertex buffer views in an array of <strong><see cref="SharpDX.Direct3D12.VertexBufferView"/></strong> structures. </p> </dd></param>	
        /// <param name="numBuffers"><dd>  <p> The number of views in the <em>pViews</em> array. </p> </dd></param>	
        /// <msdn-id>dn986883</msdn-id>	
        /// <unmanaged>void ID3D12GraphicsCommandList::IASetVertexBuffers([In] unsigned int StartSlot,[In] unsigned int NumViews,[In] const void* pViews)</unmanaged>	
        /// <unmanaged-short>ID3D12GraphicsCommandList::IASetVertexBuffers</unmanaged-short>	
        public void SetVertexBuffers(int startSlot, SharpDX.Direct3D12.VertexBufferView[] vertexBufferViews, int numBuffers)
        {
            unsafe
            {
                fixed (void* descPtr = vertexBufferViews)
                    SetVertexBuffers(startSlot, numBuffers, new IntPtr(descPtr));
            }
        }

        /// <summary>	
        /// <p>Sets a CPU descriptor handle for the vertex buffers.</p>	
        /// </summary>	
        /// <param name="startSlot"><dd>  <p> Index into the device's zero-based array to begin setting vertex buffers. </p> </dd></param>	
        /// <param name="vertexBufferViews"><dd>  <p> Specifies the vertex buffer views in an array of <strong><see cref="SharpDX.Direct3D12.VertexBufferView"/></strong> structures. </p> </dd></param>
        /// <msdn-id>dn986883</msdn-id>	
        /// <unmanaged>void ID3D12GraphicsCommandList::IASetVertexBuffers([In] unsigned int StartSlot,[In] unsigned int NumViews,[In] const void* pViews)</unmanaged>	
        /// <unmanaged-short>ID3D12GraphicsCommandList::IASetVertexBuffers</unmanaged-short>	
        public void SetVertexBuffers(int startSlot, params SharpDX.Direct3D12.VertexBufferView[] vertexBufferViews)
        {
            SetVertexBuffers(startSlot, vertexBufferViews, vertexBufferViews.Length);
        }



        /// <summary>	
        /// <p>Sets a CPU descriptor handle for the vertex buffers.</p>	
        /// </summary>	
        /// <param name="startSlot"><dd>  <p> Index into the device's zero-based array to begin setting vertex buffers. </p> </dd></param>	
        /// <param name="vertexBufferView"><dd>  <p> Specifies the vertex buffer view of <strong><see cref="SharpDX.Direct3D12.VertexBufferView"/></strong> structures. </p> </dd></param>	
        /// <msdn-id>dn986883</msdn-id>	
        /// <unmanaged>void ID3D12GraphicsCommandList::IASetVertexBuffers([In] unsigned int StartSlot,[In] unsigned int NumViews,[In] const void* pViews)</unmanaged>	
        /// <unmanaged-short>ID3D12GraphicsCommandList::IASetVertexBuffers</unmanaged-short>	
        public void SetVertexBuffer(int startSlot, SharpDX.Direct3D12.VertexBufferView vertexBufferView)
        {
            unsafe
            {
                SetVertexBuffers(startSlot, 1, (IntPtr)(&vertexBufferView));
            }
        }

        /// <summary>	
        /// <p> Bind an array of viewports to the rasterizer stage of the pipeline. </p>	
        /// </summary>	
        /// <param name="viewports"><dd>  <p> Number of viewports to bind. The range of valid values is (0, D3D12_VIEWPORT_AND_SCISSORRECT_OBJECT_COUNT_PER_PIPELINE). </p> </dd></param>	
        /// <remarks>	
        /// <p> All viewports must be set atomically as one operation. Any viewports not defined by the call are disabled. </p><p> Which viewport to use is determined by the SV_ViewportArrayIndex semantic output by a geometry shader; if a geometry shader does not specify the semantic, Direct3D will use the first viewport in the array. </p><strong>Note</strong> Even though you specify float values to the members of the <strong><see cref="SharpDX.Direct3D12.Viewport"/></strong> structure for the <em>pViewports</em> array in a call to  <strong>RSSetViewports</strong> for feature levels 9_x, <strong>RSSetViewports</strong> uses DWORDs internally. Because of this behavior, when you use a negative top left corner for the viewport, the call to  <strong>RSSetViewports</strong> for feature levels 9_x fails. This failure occurs because <strong>RSSetViewports</strong> for 9_x casts the floating point values into unsigned integers without validation, which results in integer overflow.	
        /// </remarks>	
        /// <msdn-id>dn903900</msdn-id>	
        /// <unmanaged>void ID3D12GraphicsCommandList::RSSetViewports([In] unsigned int NumViewports,[In, Buffer] const D3D12_VIEWPORT* pViewports)</unmanaged>	
        /// <unmanaged-short>ID3D12GraphicsCommandList::RSSetViewports</unmanaged-short>	
        public void SetViewports(params SharpDX.Mathematics.Interop.RawViewportF[] viewports)
        {
            if (viewports == null) throw new ArgumentNullException("viewports");
            unsafe
            {
                fixed (void* pViewPorts = viewports)
                    SetViewports(viewports.Length, (IntPtr)pViewPorts);
            }
        }

        /// <summary>	
        /// <p> Bind an array of viewports to the rasterizer stage of the pipeline. </p>	
        /// </summary>	
        /// <param name="viewport"><dd>  <p> Number of viewports to bind. The range of valid values is (0, D3D12_VIEWPORT_AND_SCISSORRECT_OBJECT_COUNT_PER_PIPELINE). </p> </dd></param>	
        /// <remarks>	
        /// <p> All viewports must be set atomically as one operation. Any viewports not defined by the call are disabled. </p><p> Which viewport to use is determined by the SV_ViewportArrayIndex semantic output by a geometry shader; if a geometry shader does not specify the semantic, Direct3D will use the first viewport in the array. </p><strong>Note</strong> Even though you specify float values to the members of the <strong><see cref="SharpDX.Direct3D12.Viewport"/></strong> structure for the <em>pViewports</em> array in a call to  <strong>RSSetViewports</strong> for feature levels 9_x, <strong>RSSetViewports</strong> uses DWORDs internally. Because of this behavior, when you use a negative top left corner for the viewport, the call to  <strong>RSSetViewports</strong> for feature levels 9_x fails. This failure occurs because <strong>RSSetViewports</strong> for 9_x casts the floating point values into unsigned integers without validation, which results in integer overflow.	
        /// </remarks>	
        /// <msdn-id>dn903900</msdn-id>	
        /// <unmanaged>void ID3D12GraphicsCommandList::RSSetViewports([In] unsigned int NumViewports,[In, Buffer] const D3D12_VIEWPORT* pViewports)</unmanaged>	
        /// <unmanaged-short>ID3D12GraphicsCommandList::RSSetViewports</unmanaged-short>	
        public unsafe void SetViewport(SharpDX.Mathematics.Interop.RawViewportF viewport)
        {
            SetViewports(1, new IntPtr(&viewport));
        }

        /// <summary>	
        /// Binds an array of scissor rectangles to the rasterizer stage. 
        /// </summary>	
        /// <param name="rectangles">No documentation.</param>	
        /// <msdn-id>dn903899</msdn-id>	
        /// <unmanaged>void ID3D12GraphicsCommandList::RSSetScissorRects([In] unsigned int NumRects,[In, Buffer] const RECT* pRects)</unmanaged>	
        /// <unmanaged-short>ID3D12GraphicsCommandList::RSSetScissorRects</unmanaged-short>	
        public void SetScissorRectangles(params SharpDX.Mathematics.Interop.RawRectangle[] rectangles)
        {
            if (rectangles == null) throw new ArgumentNullException("rectangles");
            SetScissorRectangles(rectangles.Length, rectangles);
        }

        /// <summary>	
        /// Binds an array of scissor rectangles to the rasterizer stage. 
        /// </summary>	
        /// <param name="rectangle">No documentation.</param>	
        /// <msdn-id>dn903899</msdn-id>	
        /// <unmanaged>void ID3D12GraphicsCommandList::RSSetScissorRects([In] unsigned int NumRects,[In, Buffer] const RECT* pRects)</unmanaged>	
        /// <unmanaged-short>ID3D12GraphicsCommandList::RSSetScissorRects</unmanaged-short>	
        public unsafe void SetScissorRectangles(SharpDX.Mathematics.Interop.RawRectangle rectangle)
        {
            SetScissorRectangles(1, new IntPtr(&rectangle));
        }

        /// <summary>	
        /// <p> For internal use only. </p>	
        /// </summary>	
        /// <param name="name"><dd>  <p> Internal. </p> </dd></param>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12GraphicsCommandList::BeginEvent']/*"/>	
        /// <msdn-id>dn986879</msdn-id>	
        /// <unmanaged>void ID3D12GraphicsCommandList::BeginEvent([In] unsigned int Metadata,[In, Buffer, Optional] const void* pData,[In] unsigned int Size)</unmanaged>	
        /// <unmanaged-short>ID3D12GraphicsCommandList::BeginEvent</unmanaged-short>	
        public void BeginEvent(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            IntPtr hMessage = IntPtr.Zero;
            try
            {
                hMessage = Marshal.StringToHGlobalUni(name);
                BeginEvent(1, hMessage, name.Length);
            }
            finally
            {
                if (hMessage != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(hMessage);
                    hMessage = IntPtr.Zero;
                }
            }
        }

        /// <summary>	
        /// <p> For internal use only.</p>	
        /// </summary>	
        /// <param name="name"><dd>  <p> Internal. </p> </dd></param>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12GraphicsCommandList::SetMarker']/*"/>	
        /// <msdn-id>dn986885</msdn-id>	
        /// <unmanaged>void ID3D12GraphicsCommandList::SetMarker([In] unsigned int Metadata,[In, Buffer, Optional] const void* pData,[In] unsigned int Size)</unmanaged>	
        /// <unmanaged-short>ID3D12GraphicsCommandList::SetMarker</unmanaged-short>
        public void SetMarker(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            IntPtr hMessage = IntPtr.Zero;
            try
            {
                hMessage = Marshal.StringToHGlobalUni(name);
                SetMarker(1, hMessage, name.Length);
            }
            finally
            {
                if (hMessage != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(hMessage);
                    hMessage = IntPtr.Zero;
                }
            }
        }

        /// <summary>	
        /// <p> Apps perform indirect draws/dispatches using the <strong>ExecuteIndirect</strong> method. </p>	
        /// </summary>	
        /// <param name="commandSignature"><dd>  <p> Specifies a <strong><see cref="SharpDX.Direct3D12.CommandSignature"/></strong>. The data referenced by <em>pArgumentBuffer</em> will be interpreted depending on the contents of the command signature. Refer to Indirect Drawing for the APIs that are used to create a command signature. </p> </dd></param>	
        /// <param name="maxCommandCount"><dd>  <p>There are two ways that command counts can be specified:</p> <ul> <li> If <em>pCountBuffer</em> is not <c>null</c>, then <em>MaxCommandCount</em> specifies the maximum number of operations which will be performed.  The actual number of operations to be performed are defined by the minimum of this value, and a 32-bit unsigned integer contained in <em>pCountBuffer</em> (at the byte offset specified by <em>CountBufferOffset</em>). </li> <li> If <em>pCountBuffer</em> is <c>null</c>, the <em>MaxCommandCount</em> specifies the exact number of operations which will be performed. </li> </ul> </dd></param>	
        /// <param name="argumentBuffer"><dd>  <p> Specifies one or more <strong><see cref="SharpDX.Direct3D12.Resource"/></strong> objects, containing the command arguments. </p> </dd></param>	
        /// <param name="argumentBufferOffset"><dd>  <p> Specifies an offset into <em>pArgumentBuffer</em> to identify the first command argument. </p> </dd></param>	
        /// <remarks>	
        /// <p>The semantics of this API are defined with the following pseudo-code:</p><p>Non-<c>null</c> pCountBuffer:</p><code>// Read draw count out of count buffer	
        /// UINT CommandCount = pCountBuffer-&gt;ReadUINT32(CountBufferOffset); CommandCount = min(CommandCount, MaxCommandCount) // Get reference to first Commanding argument	
        /// BYTE* Arguments = pArgumentBuffer-&gt;GetBase() + ArgumentBufferOffset; for(UINT CommandIndex = 0; CommandIndex &lt; CommandCount; CommandIndex++)	
        /// { // Interpret the data contained in *Arguments // according to the command signature pCommandSignature-&gt;Interpret(Arguments); Arguments += pCommandSignature -&gt;GetByteStride();	
        /// }	
        /// </code><p><c>null</c> pCountBuffer:</p><code>// Get reference to first Commanding argument	
        /// BYTE* Arguments = pArgumentBuffer-&gt;GetBase() + ArgumentBufferOffset; for(UINT CommandIndex = 0; CommandIndex &lt; MaxCommandCount; CommandIndex++)	
        /// { // Interpret the data contained in *Arguments // according to the command signature pCommandSignature-&gt;Interpret(Arguments); Arguments += pCommandSignature -&gt;GetByteStride();	
        /// }	
        /// </code><p>The debug layer will issue an error if either the count buffer or the argument buffer are not in the <see cref="SharpDX.Direct3D12.ResourceStates.IndirectArgument"/> state. The core runtime will validate:</p><ul> <li><em>CountBufferOffset</em> and <em>ArgumentBufferOffset</em> are 4-byte aligned </li> <li><em>pCountBuffer</em> and <em>pArgumentBuffer</em> are buffer resources (any heap type) </li> <li> The offset implied by <em>MaxCommandCount</em>, <em>ArgumentBufferOffset</em>, and the drawing program stride do not exceed the bounds of <em>pArgumentBuffer</em> (similarly for count buffer) </li> <li>The command list is a direct command list or a compute command list (not a copy or JPEG decode command list)</li> <li>The root signature of the command list matches the root signature of the command signature</li> </ul><p> The functionality of two APIs from earlier versions of Direct3D, <code>DrawInstancedIndirect</code> and <code>DrawIndexedInstancedIndirect</code>, are encompassed by  <strong>ExecuteIndirect</strong>. </p>	
        /// </remarks>	
        public void ExecuteIndirect(
            SharpDX.Direct3D12.CommandSignature commandSignature, 
            int maxCommandCount, 
            SharpDX.Direct3D12.Resource argumentBuffer, 
            long argumentBufferOffset)
        {
            ExecuteIndirect(
                commandSignature, 
                maxCommandCount,
                argumentBuffer, 
                argumentBufferOffset, 
                null, 
                0);
        }
    }
}