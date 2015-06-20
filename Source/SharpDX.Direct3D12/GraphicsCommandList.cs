﻿// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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

namespace SharpDX.Direct3D12
{
    public partial class GraphicsCommandList
    {
        /// <summary>	
        /// No documentation for Direct3D12	
        /// </summary>	
        /// <param name="depthStencilView">No documentation.</param>	
        /// <param name="clearFlags">No documentation.</param>	
        /// <param name="depth">No documentation.</param>	
        /// <param name="stencil">No documentation.</param>	
        /// <param name="rectRef">No documentation.</param>	
        /// <param name="numRects">No documentation.</param>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12GraphicsCommandList::ClearDepthStencilView']/*"/>	
        /// <unmanaged>void ID3D12GraphicsCommandList::ClearDepthStencilView([In] D3D12_CPU_DESCRIPTOR_HANDLE DepthStencilView,[In] D3D12_CLEAR_FLAGS ClearFlags,[In] float Depth,[In] unsigned char Stencil,[In] unsigned int NumRects,[In, Buffer] const RECT* pRects)</unmanaged>	
        /// <unmanaged-short>ID3D12GraphicsCommandList::ClearDepthStencilView</unmanaged-short>	
        public void ClearDepthStencilView(SharpDX.Direct3D12.CpuDescriptorHandle depthStencilView, SharpDX.Direct3D12.ClearFlags clearFlags, float depth, byte stencil)
        {
            ClearDepthStencilView(depthStencilView, clearFlags, depth, stencil, 0, null);
        }

        /// <summary>	
        /// No documentation for Direct3D12	
        /// </summary>	
        /// <param name="renderTargetView">No documentation.</param>	
        /// <param name="colorRGBA">No documentation.</param>		
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12GraphicsCommandList::ClearRenderTargetView']/*"/>	
        /// <unmanaged>void ID3D12GraphicsCommandList::ClearRenderTargetView([In] D3D12_CPU_DESCRIPTOR_HANDLE RenderTargetView,[In] const SHARPDX_COLOR4* ColorRGBA,[In] unsigned int NumRects,[In, Buffer] const RECT* pRects)</unmanaged>	
        /// <unmanaged-short>ID3D12GraphicsCommandList::ClearRenderTargetView</unmanaged-short>	
        public void ClearRenderTargetView(CpuDescriptorHandle renderTargetView, Mathematics.Interop.RawColor4 colorRGBA)
        {
            ClearRenderTargetView(renderTargetView, colorRGBA, 0, null);
        }

        /// <unmanaged>void ID3D12CommandList::ResourceBarrier([In] unsigned int Count,[In, Buffer] const D3D12_RESOURCE_BARRIER_DESC* pDesc)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::ResourceBarrier</unmanaged-short>	
        public void ResourceBarrierTransition(Resource resource, ResourceStates stateBefore, ResourceStates stateAfter)
        {
            ResourceBarrierTransition(resource, -1, stateBefore, stateAfter);
        }

        /// <unmanaged>void ID3D12CommandList::ResourceBarrier([In] unsigned int Count,[In, Buffer] const D3D12_RESOURCE_BARRIER_DESC* pDesc)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::ResourceBarrier</unmanaged-short>	
        public unsafe void ResourceBarrierTransition(Resource resource, int subresource, ResourceStates stateBefore, ResourceStates stateAfter)
        {
            var barrier = new ResourceBarrier(new ResourceTransitionBarrier(resource, subresource, stateBefore, stateAfter));
            ResourceBarrier(1, new IntPtr(&barrier));
        }

        /// <unmanaged>void ID3D12CommandList::ResourceBarrier([In] unsigned int Count,[In, Buffer] const D3D12_RESOURCE_BARRIER_DESC* pDesc)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::ResourceBarrier</unmanaged-short>	
        public unsafe void ResourceBarrierAliasing(Resource resourceBefore, Resource resourceAfter)
        {
            var barrier = new ResourceBarrier(new ResourceAliasingBarrier(resourceBefore, resourceAfter));
            ResourceBarrier(1, new IntPtr(&barrier));
        }

        /// <unmanaged>void ID3D12CommandList::ResourceBarrier([In] unsigned int Count,[In, Buffer] const D3D12_RESOURCE_BARRIER_DESC* pDesc)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::ResourceBarrier</unmanaged-short>	
        public unsafe void ResourceBarrier(SharpDX.Direct3D12.ResourceBarrier barrier)
        {
            ResourceBarrier(1, new IntPtr(&barrier));
        }

        /// <unmanaged>void ID3D12CommandList::ResourceBarrier([In] unsigned int Count,[In, Buffer] const D3D12_RESOURCE_BARRIER_DESC* pDesc)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::ResourceBarrier</unmanaged-short>	
        public unsafe void ResourceBarrier(params SharpDX.Direct3D12.ResourceBarrier[] barriers)
        {
            if(barriers == null) throw new ArgumentNullException("barriers");

            fixed (void* pBarriers = barriers)
                ResourceBarrier(barriers.Length, new IntPtr(pBarriers));
        }

        /// <summary>	
        /// No documentation for Direct3D12	
        /// </summary>	
        /// <param name="startSlot">No documentation.</param>	
        /// <param name="descRef">No documentation.</param>	
        /// <param name="numBuffers">No documentation.</param>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12GraphicsCommandList::SetVertexBuffers']/*"/>	
        /// <unmanaged>void ID3D12GraphicsCommandList::SetVertexBuffers([In] unsigned int StartSlot,[In, Buffer, Optional] const D3D12_VERTEX_BUFFER_VIEW* pDesc,[In] unsigned int NumBuffers)</unmanaged>	
        /// <unmanaged-short>ID3D12GraphicsCommandList::SetVertexBuffers</unmanaged-short>	
        public void SetVertexBuffers(int startSlot, SharpDX.Direct3D12.VertexBufferView[] descRef, int numBuffers)
        {
            unsafe
            {
                fixed (void* descPtr = descRef)
                    SetVertexBuffers(startSlot, numBuffers, new IntPtr(descPtr));
            }
        }

        /// <summary>	
        /// No documentation for Direct3D12	
        /// </summary>	
        /// <param name="startSlot">No documentation.</param>	
        /// <param name="descRef">No documentation.</param>	
        /// <param name="numBuffers">No documentation.</param>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12GraphicsCommandList::SetVertexBuffers']/*"/>	
        /// <unmanaged>void ID3D12GraphicsCommandList::SetVertexBuffers([In] unsigned int StartSlot,[In, Buffer, Optional] const D3D12_VERTEX_BUFFER_VIEW* pDesc,[In] unsigned int NumBuffers)</unmanaged>	
        /// <unmanaged-short>ID3D12GraphicsCommandList::SetVertexBuffers</unmanaged-short>	
        public void SetVertexBuffer(int startSlot, SharpDX.Direct3D12.VertexBufferView descRef)
        {
            unsafe
            {
                SetVertexBuffers(startSlot, 1, (IntPtr)(&descRef));
            }
        }

        /// <unmanaged>void ID3D12CommandList::RSSetViewports([In] unsigned int Count,[In, Buffer] const D3D11_VIEWPORT* pViewports)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::RSSetViewports</unmanaged-short>	
        public void SetViewports(params SharpDX.Mathematics.Interop.RawViewportF[] viewports)
        {
            if(viewports == null) throw new ArgumentNullException("viewports");
            unsafe
            {
                fixed (void* pViewPorts = viewports)
                    SetViewports(viewports.Length, (IntPtr)pViewPorts);
            }
        }

        /// <unmanaged>void ID3D12CommandList::RSSetViewports([In] unsigned int Count,[In, Buffer] const D3D11_VIEWPORT* pViewports)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::RSSetViewports</unmanaged-short>	
        public unsafe void SetViewport(SharpDX.Mathematics.Interop.RawViewportF viewport)
        {
            SetViewports(1, new IntPtr(&viewport));
        }

        /// <unmanaged>void ID3D12CommandList::RSSetScissorRects([In] unsigned int Count,[In, Buffer] const RECT* pRects)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::RSSetScissorRects</unmanaged-short>	
        public void SetScissorRectangles(params SharpDX.Mathematics.Interop.RawRectangle[] rectangles)
        {
            if (rectangles == null) throw new ArgumentNullException("rectangles");
            SetScissorRectangles(rectangles.Length, rectangles);
        }

        /// <unmanaged>void ID3D12CommandList::RSSetScissorRects([In] unsigned int Count,[In, Buffer] const RECT* pRects)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::RSSetScissorRects</unmanaged-short>	
        public unsafe void SetScissorRectangles(SharpDX.Mathematics.Interop.RawRectangle rectangle)
        {
            SetScissorRectangles(1, new IntPtr(&rectangle));
        }
    }
}