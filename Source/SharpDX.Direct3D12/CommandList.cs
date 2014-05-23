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

namespace SharpDX.Direct3D12
{
    public partial class CommandList
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandList"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="type">The type.</param>
        /// <param name="commandAllocator">The command allocator.</param>
        /// <param name="initialState">The initial state.</param>
        /// <param name="descriptorHeap">The descriptor heap.</param>
        /// <exception cref="System.ArgumentNullException">device</exception>
        /// <unmanaged>HRESULT ID3D12Device::CreateCommandList([In] D3D12_COMMAND_LIST_TYPE type,[In] ID3D12CommandAllocator* pCommandAllocator,[In] ID3D12PipelineState* pInitialState,[In, Optional] ID3D12DescriptorHeap* pDescriptorHeap,[Out, Fast] ID3D12CommandList** ppCommandList)</unmanaged>
        ///   <unmanaged-short>ID3D12Device::CreateCommandList</unmanaged-short>
        public CommandList(Device device, SharpDX.Direct3D12.CommandListType type, SharpDX.Direct3D12.CommandAllocator commandAllocator, 
            SharpDX.Direct3D12.PipelineState initialState, SharpDX.Direct3D12.DescriptorHeap descriptorHeap) : base(IntPtr.Zero)
        {
            if(device == null) throw new ArgumentNullException("device");
            device.CreateCommandList(type, commandAllocator, initialState, descriptorHeap, this);
        }

        /// <unmanaged>void ID3D12CommandList::ResourceBarrier([In] unsigned int Count,[In, Buffer] const D3D12_RESOURCE_BARRIER_DESC* pDesc)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::ResourceBarrier</unmanaged-short>	
        public void ResourceBarrierTransition(Resource resource, ResourceUsage stateBefore, ResourceUsage stateAfter)
        {
            ResourceBarrierTransition(resource, -1, stateBefore, stateAfter);
        }

        /// <unmanaged>void ID3D12CommandList::ResourceBarrier([In] unsigned int Count,[In, Buffer] const D3D12_RESOURCE_BARRIER_DESC* pDesc)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::ResourceBarrier</unmanaged-short>	
        public unsafe void ResourceBarrierTransition(Resource resource, int subresource, ResourceUsage stateBefore, ResourceUsage stateAfter)
        {
            var barrier = new ResourceBarrierDescription(new ResourceTransitionBarrierDescription(resource, subresource, stateBefore, stateAfter));
            ResourceBarrier(1, new IntPtr(&barrier));
        }

        /// <unmanaged>void ID3D12CommandList::ResourceBarrier([In] unsigned int Count,[In, Buffer] const D3D12_RESOURCE_BARRIER_DESC* pDesc)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::ResourceBarrier</unmanaged-short>	
        public unsafe void ResourceBarrierAliasing(Resource resourceBefore, Resource resourceAfter)
        {
            var barrier = new ResourceBarrierDescription(new ResourceAliasingBarrierDescription(resourceBefore, resourceAfter));
            ResourceBarrier(1, new IntPtr(&barrier));
        }

        /// <unmanaged>void ID3D12CommandList::ResourceBarrier([In] unsigned int Count,[In, Buffer] const D3D12_RESOURCE_BARRIER_DESC* pDesc)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::ResourceBarrier</unmanaged-short>	
        public unsafe void ResourceBarrier(SharpDX.Direct3D12.ResourceBarrierDescription barrier)
        {
            ResourceBarrier(1, new IntPtr(&barrier));
        }

        /// <unmanaged>void ID3D12CommandList::ResourceBarrier([In] unsigned int Count,[In, Buffer] const D3D12_RESOURCE_BARRIER_DESC* pDesc)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::ResourceBarrier</unmanaged-short>	
        public unsafe void ResourceBarrier(params SharpDX.Direct3D12.ResourceBarrierDescription[] barriers)
        {
            if(barriers == null) throw new ArgumentNullException("barriers");

            fixed (void* pBarriers = barriers)
                ResourceBarrier(barriers.Length, new IntPtr(pBarriers));
        }

        /// <unmanaged>void ID3D12CommandList::RSSetViewports([In] unsigned int Count,[In, Buffer] const D3D11_VIEWPORT* pViewports)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::RSSetViewports</unmanaged-short>	
        public void SetViewports(params SharpDX.ViewportF[] viewports)
        {
            if(viewports == null) throw new ArgumentNullException("viewports");
            SetViewports(viewports.Length, viewports);
        }

        /// <unmanaged>void ID3D12CommandList::RSSetViewports([In] unsigned int Count,[In, Buffer] const D3D11_VIEWPORT* pViewports)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::RSSetViewports</unmanaged-short>	
        public unsafe void SetViewport(ViewportF viewport)
        {
            SetViewports(1, new IntPtr(&viewport));
        }

        /// <unmanaged>void ID3D12CommandList::RSSetScissorRects([In] unsigned int Count,[In, Buffer] const RECT* pRects)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::RSSetScissorRects</unmanaged-short>	
        public void SetScissorRectangles(params SharpDX.Rectangle[] rectangles)
        {
            if (rectangles == null) throw new ArgumentNullException("rectangles");
            SetScissorRectangles(rectangles.Length, rectangles);
        }

        /// <unmanaged>void ID3D12CommandList::RSSetScissorRects([In] unsigned int Count,[In, Buffer] const RECT* pRects)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::RSSetScissorRects</unmanaged-short>	
        public unsafe void SetScissorRectangles(Rectangle rectangle)
        {
            SetScissorRectangles(1, new IntPtr(&rectangle));
        }
    }
}