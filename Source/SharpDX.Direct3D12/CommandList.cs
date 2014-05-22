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
using SharpDX.Direct3D;

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

        /// <unmanaged>HRESULT ID3D12CommandList::CreateIndexBufferViewsInHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12Resource** pBuffers,[In, Buffer] const D3D12_INDEX_BUFFER_VIEW_DESC* pDescs,[In] BOOL BindAsTable)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CreateIndexBufferViewsInHeap</unmanaged-short>	
        public unsafe void CreateIndexBufferViewsInHeap(int heapStartSlot, Resource buffer, IndexBufferViewDescription description, bool bindAsTable = false)
        {
            var nativePointer = buffer == null ? IntPtr.Zero : buffer.NativePointer;
            CreateIndexBufferViewsInHeap(heapStartSlot, 1, new IntPtr(&nativePointer), new IntPtr(&description), bindAsTable);
        }

        /// <unmanaged>void ID3D12CommandList::CopyIndexBufferViewsToHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12IndexBufferView** pViews,[In] BOOL BindAsTable)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CopyIndexBufferViewsToHeap</unmanaged-short>	
        public unsafe void CopyIndexBufferViewsToHeap(int heapStartSlot,
            IndexBufferView view,
            bool bindAsTable = false)
        {
            var nativePointer = view == null ? IntPtr.Zero : view.NativePointer;
            CopyIndexBufferViewsToHeap(heapStartSlot, 1, new IntPtr(&nativePointer), bindAsTable);
        }

        /// <unmanaged>HRESULT ID3D12CommandList::CreateVertexBufferViewsInHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12Resource** pBuffers,[In, Buffer] const D3D12_VERTEX_BUFFER_VIEW_DESC* pDescs,[In] BOOL BindAsTable)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CreateVertexBufferViewsInHeap</unmanaged-short>	
        public unsafe void CreateVertexBufferViewsInHeap(int heapStartSlot,
            Resource buffer,
            VertexBufferViewDescription description,
            bool bindAsTable = false)
        {
            var nativePointer = buffer == null ? IntPtr.Zero : buffer.NativePointer;
            CreateVertexBufferViewsInHeap(heapStartSlot, 1, new IntPtr(&nativePointer), new IntPtr(&description), bindAsTable);
        }

        /// <unmanaged>void ID3D12CommandList::CopyVertexBufferViewsToHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12VertexBufferView** pViews,[In] BOOL BindAsTable)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CopyVertexBufferViewsToHeap</unmanaged-short>	
        public unsafe void CopyVertexBufferViewsToHeap(int heapStartSlot, VertexBufferView view, bool bindAsTable = false)
        {
            var nativePointer = view == null ? IntPtr.Zero : view.NativePointer;
            CopyVertexBufferViewsToHeap(heapStartSlot, 1, new IntPtr(&nativePointer), bindAsTable);
        }

        /// <unmanaged>HRESULT ID3D12CommandList::CreateShaderResourceViewsInHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12Resource** pResources,[In, Buffer, Optional] const D3D12_SHADER_RESOURCE_VIEW_DESC* pDescs,[In] unsigned int DescriptorTableSlot)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CreateShaderResourceViewsInHeap</unmanaged-short>	
        public unsafe void CreateShaderResourceViewsInHeap(int heapStartSlot,
            SharpDX.Direct3D12.Resource resource,
            SharpDX.Direct3D12.ShaderResourceViewDescription description,
            int descriptorTableSlot = -1)
        {
            var nativePointer = resource == null ? IntPtr.Zero : resource.NativePointer;
            CreateShaderResourceViewsInHeap(heapStartSlot, 1, new IntPtr(&nativePointer), new IntPtr(&description), descriptorTableSlot);
        }

        /// <unmanaged>void ID3D12CommandList::CopyShaderResourceViewsToHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12ShaderResourceView** pViews,[In] unsigned int DescriptorTableSlot)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CopyShaderResourceViewsToHeap</unmanaged-short>	
        public unsafe void CopyShaderResourceViewsToHeap(int heapStartSlot, SharpDX.Direct3D12.ShaderResourceView view, int descriptorTableSlot = -1)
        {
            var nativePointer = view == null ? IntPtr.Zero : view.NativePointer;
            CopyShaderResourceViewsToHeap(heapStartSlot, 1, new IntPtr(&nativePointer), descriptorTableSlot);
        }

        /// <unmanaged>HRESULT ID3D12CommandList::CreateConstantBufferViewsInHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12Resource** pBuffers,[In, Buffer, Optional] const D3D12_CONSTANT_BUFFER_VIEW_DESC* pDescs,[In] unsigned int DescriptorTableSlot)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CreateConstantBufferViewsInHeap</unmanaged-short>	
        public unsafe void CreateConstantBufferViewsInHeap(int heapStartSlot,
            SharpDX.Direct3D12.Resource buffer,
            SharpDX.Direct3D12.ConstantBufferViewDescription description,
            int descriptorTableSlot = -1)
        {
            var nativePointer = buffer == null ? IntPtr.Zero : buffer.NativePointer;
            CreateConstantBufferViewsInHeap(heapStartSlot, 1, new IntPtr(&nativePointer), new IntPtr(&description), descriptorTableSlot);
        }

        /// <unmanaged>void ID3D12CommandList::CopyConstantBufferViewsToHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12ConstantBufferView** pViews,[In] unsigned int DescriptorTableSlot)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CopyConstantBufferViewsToHeap</unmanaged-short>	
        public unsafe void CopyConstantBufferViewsToHeap(int heapStartSlot, SharpDX.Direct3D12.ConstantBufferView view, int descriptorTableSlot = -1)
        {
            var nativePointer = view == null ? IntPtr.Zero : view.NativePointer;
            CopyConstantBufferViewsToHeap(heapStartSlot, 1, new IntPtr(&nativePointer), descriptorTableSlot);
        }

        /// <unmanaged>HRESULT ID3D12CommandList::CreateSamplersInHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const D3D12_SAMPLER_DESC* pDescs,[In] unsigned int DescriptorTableSlot)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CreateSamplersInHeap</unmanaged-short>	
        public void CreateSamplersInHeap(int heapStartSlot, SharpDX.Direct3D12.SamplerStateDescription description, int descriptorTableSlot = -1)
        {
            // TODO optimize
            CreateSamplersInHeap(heapStartSlot, 1, new [] { description }, descriptorTableSlot);
        }

        /// <unmanaged>void ID3D12CommandList::CopySamplersToHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12Sampler** pSamplers,[In] unsigned int DescriptorTableSlot)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CopySamplersToHeap</unmanaged-short>	
        public unsafe void CopySamplersToHeap(int heapStartSlot, SharpDX.Direct3D12.Sampler sampler, int descriptorTableSlot = -1)
        {
            var nativePointer = sampler == null ? IntPtr.Zero : sampler.NativePointer;
            CopySamplersToHeap(heapStartSlot, 1, new IntPtr(&nativePointer), descriptorTableSlot);
        }

        /// <unmanaged>HRESULT ID3D12CommandList::CreateUnorderedAccessViewsInHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12Resource** pResources,[In, Buffer, Optional] const D3D12_UNORDERED_ACCESS_VIEW_DESC* pDescs,[In] unsigned int DescriptorTableSlot)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CreateUnorderedAccessViewsInHeap</unmanaged-short>	
        public unsafe void CreateUnorderedAccessViewsInHeap(int heapStartSlot,
            SharpDX.Direct3D12.Resource resource,
            UnorderedAccessViewDescription description,
            int descriptorTableSlot = -1)
        {
            var nativePointer = resource == null ? IntPtr.Zero : resource.NativePointer;
            CreateUnorderedAccessViewsInHeap(heapStartSlot, 1, new IntPtr(&nativePointer), new IntPtr(&description), descriptorTableSlot);
        }

        /// <unmanaged>void ID3D12CommandList::CopyUnorderedAccessViewsToHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12UnorderedAccessView** pViews,[In] unsigned int DescriptorTableSlot)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CopyUnorderedAccessViewsToHeap</unmanaged-short>	
        public unsafe void CopyUnorderedAccessViewsToHeap(int heapStartSlot, SharpDX.Direct3D12.UnorderedAccessView view, int descriptorTableSlot = -1) {
            var nativePointer = view == null ? IntPtr.Zero : view.NativePointer;
            CopyUnorderedAccessViewsToHeap(heapStartSlot, 1, new IntPtr(&nativePointer), descriptorTableSlot);
        }

        /// <unmanaged>HRESULT ID3D12CommandList::CreateStreamOutputViewsInHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12Resource** pBuffers,[In] BOOL BindAsTable)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CreateStreamOutputViewsInHeap</unmanaged-short>	
        public unsafe void CreateStreamOutputViewsInHeap(int heapStartSlot, SharpDX.Direct3D12.Resource buffer, bool bindAsTable = false)
        {
            var nativePointer = buffer == null ? IntPtr.Zero : buffer.NativePointer;
            CreateStreamOutputViewsInHeap(heapStartSlot, 1, new IntPtr(&nativePointer), bindAsTable);
        }

        /// <unmanaged>void ID3D12CommandList::CopyStreamOutputViewsToHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12StreamOutputView** pViews,[In] BOOL BindAsTable)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CopyStreamOutputViewsToHeap</unmanaged-short>	
        public unsafe void CopyStreamOutputViewsToHeap(int heapStartSlot, SharpDX.Direct3D12.StreamOutputView view, bool bindAsTable = false)
        {
            var nativePointer = view == null ? IntPtr.Zero : view.NativePointer;
            CopyStreamOutputViewsToHeap(heapStartSlot, 1, new IntPtr(&nativePointer), bindAsTable);
        }

        /// <unmanaged>HRESULT ID3D12CommandList::CreateRenderTargetViewsInHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12Resource** pResources,[In, Buffer, Optional] const D3D12_RENDER_TARGET_VIEW_DESC* pDescs)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CreateRenderTargetViewsInHeap</unmanaged-short>	
        public unsafe void CreateRenderTargetViewsInHeap(int heapStartSlot, SharpDX.Direct3D12.Resource resource, SharpDX.Direct3D12.RenderTargetViewDescription description) 
        {
            var nativePointer = resource == null ? IntPtr.Zero : resource.NativePointer;
            CreateRenderTargetViewsInHeap(heapStartSlot, 1, new IntPtr(&nativePointer), new IntPtr(&description));
        }

        /// <unmanaged>void ID3D12CommandList::CopyRenderTargetViewsToHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12RenderTargetView** pViews)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CopyRenderTargetViewsToHeap</unmanaged-short>	
        public unsafe void CopyRenderTargetViewsToHeap(int heapStartSlot, SharpDX.Direct3D12.RenderTargetView view)
        {
            var nativePointer = view == null ? IntPtr.Zero : view.NativePointer;
            CopyRenderTargetViewsToHeap(heapStartSlot, 1, new IntPtr(&nativePointer));
        }

        /// <unmanaged>HRESULT ID3D12CommandList::CreateDepthStencilViewsInHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12Resource** pResources,[In, Buffer, Optional] const D3D12_DEPTH_STENCIL_VIEW_DESC* pDescs)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CreateDepthStencilViewsInHeap</unmanaged-short>	
        public unsafe void CreateDepthStencilViewsInHeap(int heapStartSlot, SharpDX.Direct3D12.Resource resource, SharpDX.Direct3D12.DepthStencilViewDescription description) {
            var nativePointer = resource == null ? IntPtr.Zero : resource.NativePointer;
            CreateDepthStencilViewsInHeap(heapStartSlot, 1, new IntPtr(&nativePointer), new IntPtr(&description));
        }

        /// <unmanaged>void ID3D12CommandList::CopyDepthStencilViewsToHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12DepthStencilView** pViews)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CopyDepthStencilViewsToHeap</unmanaged-short>	
        public unsafe void CopyDepthStencilViewsToHeap(int heapStartSlot, SharpDX.Direct3D12.DepthStencilView view)
        {
            var nativePointer = view == null ? IntPtr.Zero : view.NativePointer;
            CopyDepthStencilViewsToHeap(heapStartSlot, 1, new IntPtr(&nativePointer));
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
    }
}