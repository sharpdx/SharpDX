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
using SharpDX.Direct3D;
using SharpDX.DXGI;

namespace SharpDX.Direct3D12
{
    public partial class Device
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        public Device()
            : this(null, FeatureLevel.Level_9_1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        public Device(Adapter adapter)
            : this(adapter, FeatureLevel.Level_9_1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        /// <param name="minFeatureLevel">The minimum feature level.</param>
        public Device(Adapter adapter, FeatureLevel minFeatureLevel) : base(IntPtr.Zero)
        {
            CreateDevice(adapter, minFeatureLevel, this);
        }

        /// <summary>	
        /// No documentation for Direct3D12	
        /// </summary>	
        /// <param name="descRef">No documentation.</param>	
        /// <param name="riid">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12Device::CreateCommandQueue']/*"/>	
        /// <unmanaged>HRESULT ID3D12Device::CreateCommandQueue([In] const D3D12_COMMAND_QUEUE_DESC* pDesc,[In] const GUID&amp; riid,[Out] ID3D12CommandQueue** ppCommandQueue)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::CreateCommandQueue</unmanaged-short>	
        public SharpDX.Direct3D12.CommandQueue CreateCommandQueue(SharpDX.Direct3D12.CommandQueueDescription description)
        {
            return CreateCommandQueue(description, Utilities.GetGuidFromType(typeof(CommandQueue)));
        }

        /// <summary>	
        /// No documentation for Direct3D12	
        /// </summary>	
        /// <param name="type">No documentation.</param>	
        /// <param name="riid">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12Device::CreateCommandAllocator']/*"/>	
        /// <unmanaged>HRESULT ID3D12Device::CreateCommandAllocator([In] D3D12_COMMAND_LIST_TYPE type,[In] const GUID&amp; riid,[Out] ID3D12CommandAllocator** ppCommandAllocator)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::CreateCommandAllocator</unmanaged-short>	
        public SharpDX.Direct3D12.CommandAllocator CreateCommandAllocator(SharpDX.Direct3D12.CommandListType type)
        {
            return CreateCommandAllocator(type, Utilities.GetGuidFromType(typeof(CommandAllocator)));
        }

        public GraphicsCommandList CreateCommandList(
    SharpDX.Direct3D12.CommandListType type,
    SharpDX.Direct3D12.CommandAllocator commandAllocatorRef,
    SharpDX.Direct3D12.PipelineState initialStateRef)
        {
            return CreateCommandList(0, type, commandAllocatorRef, initialStateRef);
        }

        /// <summary>	
        /// No documentation for Direct3D12	
        /// </summary>	
        /// <param name="nodeMask">No documentation.</param>	
        /// <param name="type">No documentation.</param>	
        /// <param name="commandAllocatorRef">No documentation.</param>	
        /// <param name="initialStateRef">No documentation.</param>	
        /// <param name="riid">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12Device::CreateCommandList']/*"/>	
        /// <unmanaged>HRESULT ID3D12Device::CreateCommandList([In] unsigned int nodeMask,[In] D3D12_COMMAND_LIST_TYPE type,[In] ID3D12CommandAllocator* pCommandAllocator,[In, Optional] ID3D12PipelineState* pInitialState,[In] const GUID&amp; riid,[Out] void** ppCommandList)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::CreateCommandList</unmanaged-short>	
        public GraphicsCommandList CreateCommandList(int nodeMask,
            SharpDX.Direct3D12.CommandListType type,
            SharpDX.Direct3D12.CommandAllocator commandAllocatorRef,
            SharpDX.Direct3D12.PipelineState initialStateRef)
        {
            var nativePointer = CreateCommandList(nodeMask, type, commandAllocatorRef, initialStateRef, Utilities.GetGuidFromType(typeof(GraphicsCommandList)));
            return new GraphicsCommandList(nativePointer);
        }

        /// <summary>	
        /// No documentation for Direct3D12	
        /// </summary>	
        /// <param name="descRef">No documentation.</param>	
        /// <param name="rootSignatureRef">No documentation.</param>	
        /// <param name="riid">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12Device::CreateCommandSignature']/*"/>	
        /// <unmanaged>HRESULT ID3D12Device::CreateCommandSignature([In] const D3D12_COMMAND_SIGNATURE* pDesc,[In, Optional] ID3D12RootSignature* pRootSignature,[In] const GUID&amp; riid,[Out] ID3D12CommandSignature** ppvCommandSignature)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::CreateCommandSignature</unmanaged-short>	
        public CommandSignature CreateCommandSignature(SharpDX.Direct3D12.CommandSignatureDescription descRef, SharpDX.Direct3D12.RootSignature rootSignatureRef)
        {
            return CreateCommandSignature(descRef, rootSignatureRef, Utilities.GetGuidFromType(typeof(CommandSignature)));
        }


        /// <summary>
        /// No documentation for Direct3D12
        /// </summary>
        /// <param name="heapPropertiesRef">No documentation.</param>
        /// <param name="heapFlags">No documentation.</param>
        /// <param name="resourceDescRef">No documentation.</param>
        /// <param name="initialResourceState">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ID3D12Device::CreateCommittedResource([In] const D3D12_HEAP_PROPERTIES* pHeapProperties,[In] D3D12_HEAP_MISC_FLAG HeapMiscFlags,[In, Value] const D3D12_RESOURCE_DESC* pResourceDesc,[In] D3D12_RESOURCE_USAGE InitialResourceState,[In] const GUID&amp; riidResource,[Out] ID3D12Resource** ppvResource)</unmanaged>
        ///   <unmanaged-short>ID3D12Device::CreateCommittedResource</unmanaged-short>
        public SharpDX.Direct3D12.Resource CreateCommittedResource(SharpDX.Direct3D12.HeapProperties heapPropertiesRef, SharpDX.Direct3D12.HeapFlags heapFlags, SharpDX.Direct3D12.ResourceDescription resourceDescRef, SharpDX.Direct3D12.ResourceStates initialResourceState, SharpDX.Direct3D12.ClearValue? optimizedClearValueRef = null)
        {
            return CreateCommittedResource(ref heapPropertiesRef, heapFlags, resourceDescRef, initialResourceState, optimizedClearValueRef, Utilities.GetGuidFromType(typeof(Resource)));
        }

        /// <summary>	
        /// No documentation for Direct3D12	
        /// </summary>	
        /// <param name="descRef">No documentation.</param>	
        /// <param name="riid">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12Device::CreateComputePipelineState']/*"/>	
        /// <unmanaged>HRESULT ID3D12Device::CreateComputePipelineState([In] const D3D12_COMPUTE_PIPELINE_STATE_DESC* pDesc,[In] const GUID&amp; riid,[Out] ID3D12PipelineState** ppPipelineState)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::CreateComputePipelineState</unmanaged-short>	
        public unsafe SharpDX.Direct3D12.PipelineState CreateComputePipelineState(SharpDX.Direct3D12.ComputePipelineStateDescription descRef)
        {
            // Use a custom marshalling routine for this class
            var nativeDesc = new ComputePipelineStateDescription.__Native();
            descRef.__MarshalTo(ref nativeDesc);
            fixed (void* pComputeShader = descRef.ComputeShader.Buffer)
            {
                descRef.ComputeShader.UpdateNative(ref nativeDesc.ComputeShader, (IntPtr)pComputeShader);
                return CreateComputePipelineState(new IntPtr(&nativeDesc), Utilities.GetGuidFromType(typeof(PipelineState)));
            }       
        }

        public SharpDX.Direct3D12.RootSignature CreateRootSignature(DataPointer rootSignaturePointer)
        {
            return CreateRootSignature(0, rootSignaturePointer);
        }

        /// <summary>	
        /// No documentation for Direct3D12	
        /// </summary>	
        /// <param name="nodeMask">No documentation.</param>	
        /// <param name="blobWithRootSignatureRef">No documentation.</param>	
        /// <param name="blobLengthInBytes">No documentation.</param>	
        /// <param name="riid">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12Device::CreateRootSignature']/*"/>	
        /// <unmanaged>HRESULT ID3D12Device::CreateRootSignature([In] unsigned int nodeMask,[In, Buffer] const void* pBlobWithRootSignature,[In] SIZE_T blobLengthInBytes,[In] const GUID&amp; riid,[Out] ID3D12RootSignature** ppvRootSignature)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::CreateRootSignature</unmanaged-short>	
        public SharpDX.Direct3D12.RootSignature CreateRootSignature(int nodeMask, DataPointer rootSignaturePointer)
        {
            return CreateRootSignature(nodeMask, rootSignaturePointer.Pointer, rootSignaturePointer.Size, Utilities.GetGuidFromType(typeof(RootSignature)));
        }

        /// <summary>	
        /// No documentation for Direct3D12	
        /// </summary>	
        /// <param name="descriptorHeapDesc">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <unmanaged>HRESULT ID3D12Device::CreateDescriptorHeap([In] const D3D12_DESCRIPTOR_HEAP_DESC* pDescriptorHeapDesc,[In] const GUID&amp; riid,[Out] ID3D12DescriptorHeap** ppvHeap)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::CreateDescriptorHeap</unmanaged-short>	
        public SharpDX.Direct3D12.DescriptorHeap CreateDescriptorHeap(SharpDX.Direct3D12.DescriptorHeapDescription descriptorHeapDesc)
        {
            return CreateDescriptorHeap(descriptorHeapDesc, Utilities.GetGuidFromType(typeof(DescriptorHeap)));
        }

        /// <summary>	
        /// No documentation for Direct3D12	
        /// </summary>	
        /// <param name="initialValue">No documentation.</param>	
        /// <param name="flags">No documentation.</param>	
        /// <param name="riid">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12Device::CreateFence']/*"/>	
        /// <unmanaged>HRESULT ID3D12Device::CreateFence([In] unsigned longlong InitialValue,[In] D3D12_FENCE_MISC_FLAG Flags,[In] const GUID&amp; riid,[Out] void** ppFence)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::CreateFence</unmanaged-short>	
        public Fence CreateFence(long initialValue, SharpDX.Direct3D12.FenceFlags flags)
        {
            return CreateFence(initialValue, flags, Utilities.GetGuidFromType(typeof(Fence)));
        }

        /// <summary>	
        /// No documentation for Direct3D12	
        /// </summary>	
        /// <param name="descRef">No documentation.</param>	
        /// <param name="riid">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12Device::CreateGraphicsPipelineState']/*"/>	
        /// <unmanaged>HRESULT ID3D12Device::CreateGraphicsPipelineState([In] const void* pDesc,[In] const GUID&amp; riid,[Out] ID3D12PipelineState** ppPipelineState)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::CreateGraphicsPipelineState</unmanaged-short>	
        public unsafe SharpDX.Direct3D12.PipelineState CreateGraphicsPipelineState(SharpDX.Direct3D12.GraphicsPipelineStateDescription desc)
        {
            // Use a custom marshalling routine for this class
            var nativeDesc = new GraphicsPipelineStateDescription.__Native();
            desc.__MarshalTo(ref nativeDesc);

            // Pin buffers if necessary
            fixed (void* pVertexShader = desc.VertexShader.Buffer)
            fixed (void* pGeometryShader = desc.GeometryShader.Buffer)
            fixed (void* pDomainShader = desc.DomainShader.Buffer)
            fixed (void* pHullShader = desc.HullShader.Buffer)
            fixed (void* pPixelShader = desc.PixelShader.Buffer)
            {
                // Transfer pin buffer address to marshal
                desc.VertexShader.UpdateNative(ref nativeDesc.VertexShader, (IntPtr)pVertexShader);
                desc.GeometryShader.UpdateNative(ref nativeDesc.GeometryShader, (IntPtr)pGeometryShader);
                desc.DomainShader.UpdateNative(ref nativeDesc.DomainShader, (IntPtr)pDomainShader);
                desc.HullShader.UpdateNative(ref nativeDesc.HullShader, (IntPtr)pHullShader);
                desc.PixelShader.UpdateNative(ref nativeDesc.PixelShader, (IntPtr)pPixelShader);

                // Marshal input elements
                var elements = desc.InputLayout.Elements;
                var nativeElements = (InputElement.__Native*)0;
                if (elements != null && elements.Length > 0)
                {
                    var ptr = stackalloc InputElement.__Native[elements.Length];
                    nativeElements = ptr;
                    for (int i = 0; i < elements.Length; i++)
                    {
                        elements[i].__MarshalTo(ref nativeElements[i]);
                    }

                    nativeDesc.InputLayout.InputElementsPointer = new IntPtr(nativeElements);
                    nativeDesc.InputLayout.ElementCount = elements.Length;
                }

                //Marshal stream output elements
                var streamOutElements = desc.StreamOutput.Elements;
                var nativeStreamOutElements = (StreamOutputElement.__Native*)0;
                if (streamOutElements != null && streamOutElements.Length > 0)
                {
                    var ptr = stackalloc StreamOutputElement.__Native[streamOutElements.Length];
                    nativeStreamOutElements = ptr;
                    for (int i = 0; i < streamOutElements.Length; i++)
                    {
                        streamOutElements[i].__MarshalTo(ref nativeStreamOutElements[i]);
                    }

                    nativeDesc.StreamOutput.StreamOutputEntriesPointer = new IntPtr(nativeStreamOutElements);
                    nativeDesc.StreamOutput.EntrieCount = streamOutElements.Length;
                }

                try
                {
                    // Create the pipeline state
                    return CreateGraphicsPipelineState(new IntPtr(&nativeDesc), Utilities.GetGuidFromType(typeof(PipelineState)));
                }
                finally
                {
                    if (elements != null)
                    {
                        for (int i = 0; i < elements.Length; i++)
                        {
                            nativeElements[i].__MarshalFree();
                        }
                    }

                    if (streamOutElements != null)
                    {
                        for (int i = 0; i < streamOutElements.Length; i++)
                        {
                            nativeStreamOutElements[i].__MarshalFree();
                        }
                    }
                }
            }
        }

        /// <summary>	
        /// No documentation for Direct3D12	
        /// </summary>	
        /// <param name="descRef">No documentation.</param>	
        /// <param name="riid">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12Device::CreateHeap']/*"/>	
        /// <unmanaged>HRESULT ID3D12Device::CreateHeap([In] const D3D12_HEAP_DESC* pDesc,[In] const GUID&amp; riid,[Out] ID3D12Heap** ppvHeap)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::CreateHeap</unmanaged-short>	
        public SharpDX.Direct3D12.Heap CreateHeap(SharpDX.Direct3D12.HeapDescription descRef)
        {
            return CreateHeap(ref descRef, Utilities.GetGuidFromType(typeof(Heap)));
        }

        /// <summary>	
        /// No documentation for Direct3D12	
        /// </summary>	
        /// <param name="descRef">No documentation.</param>	
        /// <param name="riid">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12Device::CreateQueryHeap']/*"/>	
        /// <unmanaged>HRESULT ID3D12Device::CreateQueryHeap([In] const D3D12_QUERY_HEAP_DESC* pDesc,[In] const GUID&amp; riid,[Out] ID3D12QueryHeap** ppvHeap)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::CreateQueryHeap</unmanaged-short>	
        public SharpDX.Direct3D12.QueryHeap CreateQueryHeap(SharpDX.Direct3D12.QueryHeapDescription descRef)
        {
            return CreateQueryHeap(descRef, Utilities.GetGuidFromType(typeof(QueryHeap)));
        }


        /// <summary>	
        /// No documentation for Direct3D12	
        /// </summary>	
        /// <param name="heapRef">No documentation.</param>	
        /// <param name="heapOffset">No documentation.</param>	
        /// <param name="descRef">No documentation.</param>	
        /// <param name="initialState">No documentation.</param>	
        /// <param name="optimizedClearValueRef">No documentation.</param>	
        /// <param name="riid">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12Device::CreatePlacedResource']/*"/>	
        /// <unmanaged>HRESULT ID3D12Device::CreatePlacedResource([In] ID3D12Heap* pHeap,[In] unsigned longlong HeapOffset,[In] const D3D12_RESOURCE_DESC* pDesc,[In] D3D12_RESOURCE_USAGE InitialState,[In, Optional] const D3D12_CLEAR_VALUE* pOptimizedClearValue,[In] const GUID&amp; riid,[Out] ID3D12Resource** ppvResource)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::CreatePlacedResource</unmanaged-short>	
        public SharpDX.Direct3D12.Resource CreatePlacedResource(SharpDX.Direct3D12.Heap heapRef,
            long heapOffset,
            SharpDX.Direct3D12.ResourceDescription descRef,
            SharpDX.Direct3D12.ResourceStates initialState,
            SharpDX.Direct3D12.ClearValue? optimizedClearValueRef = null)
        {
            return CreatePlacedResource(heapRef, heapOffset, ref descRef, initialState, optimizedClearValueRef, Utilities.GetGuidFromType(typeof(Resource)));
        }

        /// <summary>	
        /// No documentation for Direct3D12	
        /// </summary>	
        /// <param name="descRef">No documentation.</param>	
        /// <param name="initialState">No documentation.</param>	
        /// <param name="optimizedClearValueRef">No documentation.</param>	
        /// <param name="riid">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12Device::CreateReservedResource']/*"/>	
        /// <unmanaged>HRESULT ID3D12Device::CreateReservedResource([In] const D3D12_RESOURCE_DESC* pDesc,[In] D3D12_RESOURCE_USAGE InitialState,[In, Optional] const D3D12_CLEAR_VALUE* pOptimizedClearValue,[In] const GUID&amp; riid,[Out] ID3D12Resource** ppvResource)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::CreateReservedResource</unmanaged-short>	
        public SharpDX.Direct3D12.Resource CreateReservedResource(SharpDX.Direct3D12.ResourceDescription descRef,
            SharpDX.Direct3D12.ResourceStates initialState,
            SharpDX.Direct3D12.ClearValue? optimizedClearValueRef = null)
        {
            return CreateReservedResource(ref descRef, initialState, optimizedClearValueRef, Utilities.GetGuidFromType(typeof(Resource)));
        }

        /// <summary>
        /// Retrieves Direct3D12 Options"/>
        /// </summary>
        public unsafe FeatureDataD3D12Options D3D12Options
        {
            get
            {
                FeatureDataD3D12Options options = new FeatureDataD3D12Options();
                this.CheckFeatureSupport(Feature.D3D12Options, new IntPtr(&options), Utilities.SizeOf<FeatureDataD3D12Options>());
                return options;
            }
        }

        /// <summary>
        /// Retrieves Direct3D12 Architecture information
        /// </summary>
        public unsafe FeatureDataArchitecture Architecture
        {
            get
            {
                FeatureDataArchitecture options = new FeatureDataArchitecture();
                this.CheckFeatureSupport(Feature.Architecture, new IntPtr(&options), Utilities.SizeOf<FeatureDataArchitecture>());
                return options;
            }
        }

        private static void CreateDevice(Adapter adapter, FeatureLevel minFeatureLevel, Device instance)
        {
            D3D12.CreateDevice(adapter, minFeatureLevel, Utilities.GetGuidFromType(typeof(Device)), instance).CheckError();
        }
    }
}