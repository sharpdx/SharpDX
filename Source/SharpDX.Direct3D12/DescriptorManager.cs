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
    public partial class DescriptorManager
    {
        /// <unmanaged>HRESULT ID3D12CommandList::CreateIndexBufferViewsInHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12Resource** pBuffers,[In, Buffer] const D3D12_INDEX_BUFFER_VIEW_DESC* pDescs,[In] BOOL BindAsTable)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CreateIndexBufferViewsInHeap</unmanaged-short>	
        public unsafe void CreateIndexBufferViews(int heapStartSlot, Resource buffer, IndexBufferViewDescription description, bool bindAsTable = false)
        {
            var nativePointer = buffer == null ? IntPtr.Zero : buffer.NativePointer;
            CreateIndexBufferViews(heapStartSlot, 1, new IntPtr(&nativePointer), new IntPtr(&description), bindAsTable);
        }

        /// <unmanaged>void ID3D12CommandList::CopyIndexBufferViewsToHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12IndexBufferView** pViews,[In] BOOL BindAsTable)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CopyIndexBufferViewsToHeap</unmanaged-short>	
        public unsafe void CopyIndexBufferViewsTo(int heapStartSlot,
            IndexBufferView view,
            bool bindAsTable = false)
        {
            var nativePointer = view == null ? IntPtr.Zero : view.NativePointer;
            CopyIndexBufferViewsTo(heapStartSlot, 1, new IntPtr(&nativePointer), bindAsTable);
        }

        /// <unmanaged>HRESULT ID3D12CommandList::CreateVertexBufferViewsInHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12Resource** pBuffers,[In, Buffer] const D3D12_VERTEX_BUFFER_VIEW_DESC* pDescs,[In] BOOL BindAsTable)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CreateVertexBufferViewsInHeap</unmanaged-short>	
        public unsafe void CreateVertexBufferViews(int heapStartSlot,
            Resource buffer,
            VertexBufferViewDescription description,
            bool bindAsTable = false)
        {
            var nativePointer = buffer == null ? IntPtr.Zero : buffer.NativePointer;
            CreateVertexBufferViews(heapStartSlot, 1, new IntPtr(&nativePointer), new IntPtr(&description), bindAsTable);
        }

        /// <unmanaged>void ID3D12CommandList::CopyVertexBufferViewsToHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12VertexBufferView** pViews,[In] BOOL BindAsTable)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CopyVertexBufferViewsToHeap</unmanaged-short>	
        public unsafe void CopyVertexBufferViewsTo(int heapStartSlot, VertexBufferView view, bool bindAsTable = false)
        {
            var nativePointer = view == null ? IntPtr.Zero : view.NativePointer;
            CopyVertexBufferViewsTo(heapStartSlot, 1, new IntPtr(&nativePointer), bindAsTable);
        }

        /// <unmanaged>HRESULT ID3D12CommandList::CreateShaderResourceViewsInHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12Resource** pResources,[In, Buffer, Optional] const D3D12_SHADER_RESOURCE_VIEW_DESC* pDescs,[In] unsigned int DescriptorTableSlot)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CreateShaderResourceViewsInHeap</unmanaged-short>	
        public unsafe void CreateShaderResourceViews(int heapStartSlot,
            SharpDX.Direct3D12.Resource resource,
            SharpDX.Direct3D12.ShaderResourceViewDescription description,
            int descriptorTableSlot = -1)
        {
            var nativePointer = resource == null ? IntPtr.Zero : resource.NativePointer;
            CreateShaderResourceViews(heapStartSlot, 1, new IntPtr(&nativePointer), new IntPtr(&description), descriptorTableSlot);
        }

        /// <unmanaged>void ID3D12CommandList::CopyShaderResourceViewsToHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12ShaderResourceView** pViews,[In] unsigned int DescriptorTableSlot)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CopyShaderResourceViewsToHeap</unmanaged-short>	
        public unsafe void CopyShaderResourceViewsTo(int heapStartSlot, SharpDX.Direct3D12.ShaderResourceView view, int descriptorTableSlot = -1)
        {
            var nativePointer = view == null ? IntPtr.Zero : view.NativePointer;
            CopyShaderResourceViewsTo(heapStartSlot, 1, new IntPtr(&nativePointer), descriptorTableSlot);
        }

        /// <unmanaged>HRESULT ID3D12CommandList::CreateConstantBufferViewsInHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12Resource** pBuffers,[In, Buffer, Optional] const D3D12_CONSTANT_BUFFER_VIEW_DESC* pDescs,[In] unsigned int DescriptorTableSlot)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CreateConstantBufferViewsInHeap</unmanaged-short>	
        public unsafe void CreateConstantBufferViews(int heapStartSlot,
            SharpDX.Direct3D12.Resource buffer,
            SharpDX.Direct3D12.ConstantBufferViewDescription description,
            int descriptorTableSlot = -1)
        {
            var nativePointer = buffer == null ? IntPtr.Zero : buffer.NativePointer;
            CreateConstantBufferViews(heapStartSlot, 1, new IntPtr(&nativePointer), new IntPtr(&description), descriptorTableSlot);
        }

        /// <unmanaged>void ID3D12CommandList::CopyConstantBufferViewsToHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12ConstantBufferView** pViews,[In] unsigned int DescriptorTableSlot)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CopyConstantBufferViewsToHeap</unmanaged-short>	
        public unsafe void CopyConstantBufferViewsTo(int heapStartSlot, SharpDX.Direct3D12.ConstantBufferView view, int descriptorTableSlot = -1)
        {
            var nativePointer = view == null ? IntPtr.Zero : view.NativePointer;
            CopyConstantBufferViewsTo(heapStartSlot, 1, new IntPtr(&nativePointer), descriptorTableSlot);
        }

        /// <unmanaged>HRESULT ID3D12CommandList::CreateSamplersInHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const D3D12_SAMPLER_DESC* pDescs,[In] unsigned int DescriptorTableSlot)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CreateSamplersInHeap</unmanaged-short>	
        public void CreateSamplers(int heapStartSlot, SharpDX.Direct3D12.SamplerStateDescription description, int descriptorTableSlot = -1)
        {
            // TODO optimize
            CreateSamplers(heapStartSlot, 1, new[] { description }, descriptorTableSlot);
        }

        /// <unmanaged>void ID3D12CommandList::CopySamplersToHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12Sampler** pSamplers,[In] unsigned int DescriptorTableSlot)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CopySamplersToHeap</unmanaged-short>	
        public unsafe void CopySamplersTo(int heapStartSlot, SharpDX.Direct3D12.Sampler sampler, int descriptorTableSlot = -1)
        {
            var nativePointer = sampler == null ? IntPtr.Zero : sampler.NativePointer;
            CopySamplersTo(heapStartSlot, 1, new IntPtr(&nativePointer), descriptorTableSlot);
        }

        /// <unmanaged>HRESULT ID3D12CommandList::CreateUnorderedAccessViewsInHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12Resource** pResources,[In, Buffer, Optional] const D3D12_UNORDERED_ACCESS_VIEW_DESC* pDescs,[In] unsigned int DescriptorTableSlot)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CreateUnorderedAccessViewsInHeap</unmanaged-short>	
        public unsafe void CreateUnorderedAccessViews(int heapStartSlot,
            SharpDX.Direct3D12.Resource resource,
            UnorderedAccessViewDescription description,
            int descriptorTableSlot = -1)
        {
            var nativePointer = resource == null ? IntPtr.Zero : resource.NativePointer;
            CreateUnorderedAccessViews(heapStartSlot, 1, new IntPtr(&nativePointer), new IntPtr(&description), descriptorTableSlot);
        }

        /// <unmanaged>void ID3D12CommandList::CopyUnorderedAccessViewsToHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12UnorderedAccessView** pViews,[In] unsigned int DescriptorTableSlot)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CopyUnorderedAccessViewsToHeap</unmanaged-short>	
        public unsafe void CopyUnorderedAccessViewsTo(int heapStartSlot, SharpDX.Direct3D12.UnorderedAccessView view, int descriptorTableSlot = -1)
        {
            var nativePointer = view == null ? IntPtr.Zero : view.NativePointer;
            CopyUnorderedAccessViewsTo(heapStartSlot, 1, new IntPtr(&nativePointer), descriptorTableSlot);
        }

        /// <unmanaged>HRESULT ID3D12CommandList::CreateStreamOutputViewsInHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12Resource** pBuffers,[In] BOOL BindAsTable)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CreateStreamOutputViewsInHeap</unmanaged-short>	
        public unsafe void CreateStreamOutputViews(int heapStartSlot, SharpDX.Direct3D12.Resource buffer, bool bindAsTable = false)
        {
            var nativePointer = buffer == null ? IntPtr.Zero : buffer.NativePointer;
            CreateStreamOutputViews(heapStartSlot, 1, new IntPtr(&nativePointer), bindAsTable);
        }

        /// <unmanaged>void ID3D12CommandList::CopyStreamOutputViewsToHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12StreamOutputView** pViews,[In] BOOL BindAsTable)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CopyStreamOutputViewsToHeap</unmanaged-short>	
        public unsafe void CopyStreamOutputViewsTo(int heapStartSlot, SharpDX.Direct3D12.StreamOutputView view, bool bindAsTable = false)
        {
            var nativePointer = view == null ? IntPtr.Zero : view.NativePointer;
            CopyStreamOutputViewsTo(heapStartSlot, 1, new IntPtr(&nativePointer), bindAsTable);
        }

        /// <unmanaged>HRESULT ID3D12CommandList::CreateRenderTargetViewsInHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12Resource** pResources,[In, Buffer, Optional] const D3D12_RENDER_TARGET_VIEW_DESC* pDescs)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CreateRenderTargetViewsInHeap</unmanaged-short>	
        public unsafe void CreateRenderTargetViews(int heapStartSlot, SharpDX.Direct3D12.Resource resource, SharpDX.Direct3D12.RenderTargetViewDescription description)
        {
            var nativePointer = resource == null ? IntPtr.Zero : resource.NativePointer;
            CreateRenderTargetViews(heapStartSlot, 1, new IntPtr(&nativePointer), new IntPtr(&description));
        }

        /// <unmanaged>void ID3D12CommandList::CopyRenderTargetViewsToHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12RenderTargetView** pViews)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CopyRenderTargetViewsToHeap</unmanaged-short>	
        public unsafe void CopyRenderTargetViewsTo(int heapStartSlot, SharpDX.Direct3D12.RenderTargetView view)
        {
            var nativePointer = view == null ? IntPtr.Zero : view.NativePointer;
            CopyRenderTargetViewsTo(heapStartSlot, 1, new IntPtr(&nativePointer));
        }

        /// <unmanaged>HRESULT ID3D12CommandList::CreateDepthStencilViewsInHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12Resource** pResources,[In, Buffer, Optional] const D3D12_DEPTH_STENCIL_VIEW_DESC* pDescs)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CreateDepthStencilViewsInHeap</unmanaged-short>	
        public unsafe void CreateDepthStencilViews(int heapStartSlot, SharpDX.Direct3D12.Resource resource, SharpDX.Direct3D12.DepthStencilViewDescription description)
        {
            var nativePointer = resource == null ? IntPtr.Zero : resource.NativePointer;
            CreateDepthStencilViews(heapStartSlot, 1, new IntPtr(&nativePointer), new IntPtr(&description));
        }

        /// <unmanaged>void ID3D12CommandList::CopyDepthStencilViewsToHeap([In] unsigned int HeapStartSlot,[In] unsigned int NumHeapSlots,[In, Buffer] const ID3D12DepthStencilView** pViews)</unmanaged>	
        /// <unmanaged-short>ID3D12CommandList::CopyDepthStencilViewsToHeap</unmanaged-short>	
        public unsafe void CopyDepthStencilViewsTo(int heapStartSlot, SharpDX.Direct3D12.DepthStencilView view)
        {
            var nativePointer = view == null ? IntPtr.Zero : view.NativePointer;
            CopyDepthStencilViewsTo(heapStartSlot, 1, new IntPtr(&nativePointer));
        }
    }
}