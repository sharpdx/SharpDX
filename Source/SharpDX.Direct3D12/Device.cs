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
            : this(null, FeatureLevel.Level_11_0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        public Device(Adapter adapter)
            : this(adapter, FeatureLevel.Level_11_0)
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
        /// <p> Gets information about the features that are supported by the current graphics driver.</p>	
        /// </summary>	
        /// <param name="feature"><dd>  <p> A <strong><see cref="SharpDX.Direct3D12.Feature"/></strong>-typed value that describes the feature to query for support. </p> </dd></param>	
        /// <param name="featureSupportDataRef"><dd>  <p> The passed structure is filled with data that describes the feature support. To see the structure types, see the Remarks section in <strong><see cref="SharpDX.Direct3D12.Feature"/> enumeration</strong>. </p> </dd></param>	
        /// <param name="featureSupportDataSize"><dd>  <p> The size of the structure passed to the <em>pFeatureSupportData</em> parameter. </p> </dd></param>	
        /// <returns><p> Returns <strong><see cref="SharpDX.Result.Ok"/></strong> if successful; otherwise, returns <strong>E_INVALIDARG</strong> if an unsupported data type is passed to the <em>pFeatureSupportData</em> parameter or a size mismatch is detected for the <em>FeatureSupportDataSize</em> parameter. </p></returns>	
        /// <remarks>	
        /// <p>Refer to Capability Querying.</p>	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12Device::CheckFeatureSupport']/*"/>	
        /// <msdn-id>dn788653</msdn-id>	
        /// <unmanaged>HRESULT ID3D12Device::CheckFeatureSupport([In] D3D12_FEATURE Feature,[Out, Buffer] void* pFeatureSupportData,[In] unsigned int FeatureSupportDataSize)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::CheckFeatureSupport</unmanaged-short>	
        public bool CheckFeatureSupport<T>(SharpDX.Direct3D12.Feature feature, ref T featureSupportData) where T : struct
        {
            unsafe
            {
                return CheckFeatureSupport(feature, new IntPtr(Interop.Fixed(ref featureSupportData)), Utilities.SizeOf<T>()).Success;
            }
        }

        /// <summary>	
        /// <p>Creates a command queue.</p>	
        /// </summary>	
        /// <param name="description"><dd>  <p> Specifies a <see cref="SharpDX.Direct3D12.CommandQueueDescription"/> that describes the command queue. </p> </dd></param>	
        /// <returns><dd>  <p> A reference to a memory block that receives a reference to the <strong><see cref="SharpDX.Direct3D12.CommandQueue"/></strong> interface for the command queue. </p> </dd></returns>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12Device::CreateCommandQueue']/*"/>	
        /// <msdn-id>dn788657</msdn-id>	
        /// <unmanaged>HRESULT ID3D12Device::CreateCommandQueue([In] const D3D12_COMMAND_QUEUE_DESC* pDesc,[In] const GUID&amp; riid,[Out] ID3D12CommandQueue** ppCommandQueue)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::CreateCommandQueue</unmanaged-short>	
        public SharpDX.Direct3D12.CommandQueue CreateCommandQueue(SharpDX.Direct3D12.CommandQueueDescription description)
        {
            return CreateCommandQueue(description, Utilities.GetGuidFromType(typeof(CommandQueue)));
        }

        /// <summary>	
        /// <p>Creates a command queue.</p>	
        /// </summary>	
        /// <param name="type">The <see cref="SharpDX.Direct3D12.CommandListType"/> that describes the command queue.</param>
        /// <returns><dd>  <p> A reference to a memory block that receives a reference to the <strong><see cref="SharpDX.Direct3D12.CommandQueue"/></strong> interface for the command queue. </p> </dd></returns>	
        public SharpDX.Direct3D12.CommandQueue CreateCommandQueue(SharpDX.Direct3D12.CommandListType type)
        {
            return CreateCommandQueue(new SharpDX.Direct3D12.CommandQueueDescription(type), Utilities.GetGuidFromType(typeof(CommandQueue)));
        }

        /// <summary>	
        /// <p>Creates a command queue.</p>	
        /// </summary>	
        /// <param name="type">The <see cref="SharpDX.Direct3D12.CommandListType"/> that describes the command queue.</param>
        /// <param name="nodeMask">Multi GPU node mask.</param>
        /// <returns><dd>  <p> A reference to a memory block that receives a reference to the <strong><see cref="SharpDX.Direct3D12.CommandQueue"/></strong> interface for the command queue. </p> </dd></returns>	
        public SharpDX.Direct3D12.CommandQueue CreateCommandQueue(SharpDX.Direct3D12.CommandListType type, int nodeMask)
        {
            return CreateCommandQueue(new SharpDX.Direct3D12.CommandQueueDescription(type, nodeMask), Utilities.GetGuidFromType(typeof(CommandQueue)));
        }

        /// <summary>	
        /// <p>Creates a command allocator object.</p>	
        /// </summary>	
        /// <param name="type"><dd>  <p> A <strong><see cref="SharpDX.Direct3D12.CommandListType"/></strong>-typed value that specifies the type of command allocator to create. The type of command allocator can be the type that records either direct command lists or bundles. </p> </dd></param>	
        /// <returns><dd>  <p> A reference to a memory block that receives a reference to the <strong><see cref="SharpDX.Direct3D12.CommandAllocator"/></strong> interface for the command allocator. </p> </dd></returns>	
        /// <remarks>	
        /// <p> The device creates command lists from the command allocator. </p>	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12Device::CreateCommandAllocator']/*"/>	
        /// <msdn-id>dn788655</msdn-id>	
        /// <unmanaged>HRESULT ID3D12Device::CreateCommandAllocator([In] D3D12_COMMAND_LIST_TYPE type,[In] const GUID&amp; riid,[Out] ID3D12CommandAllocator** ppCommandAllocator)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::CreateCommandAllocator</unmanaged-short>	
        public SharpDX.Direct3D12.CommandAllocator CreateCommandAllocator(SharpDX.Direct3D12.CommandListType type)
        {
            return CreateCommandAllocator(type, Utilities.GetGuidFromType(typeof(CommandAllocator)));
        }

        /// <summary>
        /// <p>Creates a new graphics command list object.</p>	
        /// </summary>
        /// <param name="type">A <see cref="SharpDX.Direct3D12.CommandListType"/> value that specifies the type of command list to create.</param>
        /// <param name="commandAllocator">A <see cref="CommandAllocator"/> object that the device creates command lists from.</param>	
        /// <param name="initialState">A <see cref="PipelineState"/> object that contains the initial pipeline state for the command list.</param>
        /// <returns>A new instance of <see cref="GraphicsCommandList"/>.</returns>	
        public GraphicsCommandList CreateCommandList(
            SharpDX.Direct3D12.CommandListType type,
            SharpDX.Direct3D12.CommandAllocator commandAllocator,
            SharpDX.Direct3D12.PipelineState initialState)
        {
            return CreateCommandList(0, type, commandAllocator, initialState);
        }

        /// <summary>	
        /// <p>Creates a new graphics command list object.</p>	
        /// </summary>	
        /// <param name="nodeMask">
        /// For single GPU operation, set this to zero. 
        /// If there are multiple GPU nodes, set a bit to identify the node (the device's physical adapter) for which to create the command list.
        /// Each bit in the mask corresponds to a single node. Only 1 bit must be set.
        /// </param>	
        /// <param name="type">A <see cref="SharpDX.Direct3D12.CommandListType"/> value that specifies the type of command list to create.</param>	
        /// <param name="commandAllocator">A <see cref="CommandAllocator"/> object that the device creates command lists from.</param>	
        /// <param name="initialState">A <see cref="PipelineState"/> object that contains the initial pipeline state for the command list.</param>
        /// <returns>A new instance of <see cref="GraphicsCommandList"/>.</returns>	
        /// <msdn-id>dn788656</msdn-id>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12Device::CreateCommandList']/*"/>	
        /// <unmanaged>HRESULT ID3D12Device::CreateCommandList([In] unsigned int nodeMask,[In] D3D12_COMMAND_LIST_TYPE type,[In] ID3D12CommandAllocator* pCommandAllocator,[In, Optional] ID3D12PipelineState* pInitialState,[In] const GUID&amp; riid,[Out] void** ppCommandList)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::CreateCommandList</unmanaged-short>	
        public GraphicsCommandList CreateCommandList(int nodeMask,
            SharpDX.Direct3D12.CommandListType type,
            SharpDX.Direct3D12.CommandAllocator commandAllocator,
            SharpDX.Direct3D12.PipelineState initialState)
        {
            var nativePointer = CreateCommandList(
                nodeMask,
                type,
                commandAllocator,
                initialState,
                Utilities.GetGuidFromType(typeof(GraphicsCommandList)));
            return new GraphicsCommandList(nativePointer);
        }

        /// <summary>	
        /// <p> This method creates a command signature. </p>	
        /// </summary>	
        /// <param name="descRef"><dd>  <p> Describes the command signature to be created with the <strong><see cref="SharpDX.Direct3D12.CommandSignatureDescription"/></strong> structure. </p> </dd></param>	
        /// <param name="rootSignatureRef"><dd>  <p> Specifies the  <strong><see cref="SharpDX.Direct3D12.RootSignature"/></strong> that the command signature applies to. </p> </dd></param>	
        /// <param name="riid"><dd>  <p> The globally unique identifier (<strong><see cref="System.Guid"/></strong>) for the command signature interface (<strong><see cref="SharpDX.Direct3D12.CommandSignature"/></strong>). The <strong>REFIID</strong>, or <strong><see cref="System.Guid"/></strong>, of the interface to the command signature can be obtained by using the __uuidof() macro. For example, __uuidof(<strong><see cref="SharpDX.Direct3D12.CommandSignature"/></strong>) will get the <strong><see cref="System.Guid"/></strong> of the interface to a command signature. </p> </dd></param>	
        /// <returns><dd>  <p> Specifies a reference, that on successful completion of the method will point to the created command signature (<strong><see cref="SharpDX.Direct3D12.CommandSignature"/></strong>). </p> </dd></returns>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12Device::CreateCommandSignature']/*"/>	
        /// <msdn-id>dn903827</msdn-id>	
        /// <unmanaged>HRESULT ID3D12Device::CreateCommandSignature([In] const void* pDesc,[In, Optional] ID3D12RootSignature* pRootSignature,[In] const GUID&amp; riid,[Out] ID3D12CommandSignature** ppvCommandSignature)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::CreateCommandSignature</unmanaged-short>	
        public unsafe CommandSignature CreateCommandSignature(SharpDX.Direct3D12.CommandSignatureDescription descRef, SharpDX.Direct3D12.RootSignature rootSignatureRef)
        {
            var nativeDesc = new CommandSignatureDescription.__Native();
            descRef.__MarshalTo(ref nativeDesc);
            fixed (void* pIndirectArguments = descRef.IndirectArguments)
            {
                if (descRef.IndirectArguments != null)
                {
                    nativeDesc.ArgumentDescCount = descRef.IndirectArguments.Length;
                    nativeDesc.ArgumentDescsPointer = new IntPtr(pIndirectArguments);
                }

                return CreateCommandSignature(new IntPtr(&nativeDesc), rootSignatureRef, Utilities.GetGuidFromType(typeof(CommandSignature)));
            }
        }

        /// <summary>	
        /// <p> Creates both a resource and an implicit heap, such that the heap is big enough to contain the entire resource and the resource is mapped to the heap. </p>	
        /// </summary>	
        /// <param name="heapPropertiesRef"><dd>  <p> A reference to a <strong><see cref="SharpDX.Direct3D12.HeapProperties"/></strong> structure that provides properties for the resource's heap. </p> </dd></param>	
        /// <param name="heapFlags"><dd>  <p> Heap options, as a bitwise-OR'd combination of <strong><see cref="SharpDX.Direct3D12.HeapFlags"/></strong> enumeration constants. </p> </dd></param>	
        /// <param name="resourceDescRef"><dd>  <p> A reference to a <strong><see cref="SharpDX.Direct3D12.ResourceDescription"/></strong> structure that describes the resource. </p> </dd></param>	
        /// <param name="initialResourceState"><dd>  <p> The initial state of the resource, as a bitwise-OR'd combination of <strong><see cref="SharpDX.Direct3D12.ResourceStates"/></strong> enumeration constants. </p> <p> When a resource is created together with a <strong><see cref="SharpDX.Direct3D12.HeapType"/></strong>_UPLOAD heap, <em>InitialResourceState</em> must be <strong>D3D12_RESOURCE_STATE</strong>_GENERIC_READ. When a resource is created together with a <see cref="SharpDX.Direct3D12.HeapType.Readback"/> heap, <em>InitialResourceState</em> must be <see cref="SharpDX.Direct3D12.ResourceStates.CopyDestination"/>. </p> </dd></param>	
        /// <param name="optimizedClearValueRef"><dd>  <p> Specifies a <strong><see cref="SharpDX.Direct3D12.ClearValue"/></strong> that describes the default value for a clear color. </p> <p><em>pOptimizedClearValue</em> specifies a value for which clear operations are most optimal. When the created resource is a texture with either the <strong>D3D12_RESOURCE_FLAG</strong>_ALLOW_RENDER_TARGET or <see cref="SharpDX.Direct3D12.ResourceFlags.AllowDepthStencil"/> flags, applications should choose the value that the clear operation will most commonly be called with. Clear operations can be called with other values, but those operations will not be as efficient as when the value matches the one passed into resource creation. <em>pOptimizedClearValue</em> must be <c>null</c> when used with <strong><see cref="SharpDX.Direct3D12.ResourceDimension"/></strong>_BUFFER. </p> </dd></param>	
        /// <param name="riidResource"><dd>  <p> The globally unique identifier (<strong><see cref="System.Guid"/></strong>) for the resource interface. This is an input parameter. The <strong>REFIID</strong>, or <strong><see cref="System.Guid"/></strong>, of the interface to the resource can be obtained by using the __uuidof() macro. For example, __uuidof(<strong><see cref="SharpDX.Direct3D12.Resource"/></strong>) will get the <strong><see cref="System.Guid"/></strong> of the interface to a resource. </p> <p> While riidResource is, most commonly, the <see cref="System.Guid"/> for <strong><see cref="SharpDX.Direct3D12.Resource"/></strong>, it may be any <see cref="System.Guid"/> for any interface. If the resource object doesn't support the interface for this <see cref="System.Guid"/>, creation will fail with E_NOINTERFACE. </p> </dd></param>	
        /// <returns><dd>  <p> A reference to memory that receives the requested interface reference to the created resource object. <em>ppvResource</em> can be <c>null</c>, to enable capability testing. When <em>ppvResource</em> is <c>null</c>, no object will be created and S_FALSE will be returned when <em>pResourceDesc</em> is valid. </p> </dd></returns>	
        /// <remarks>	
        /// <p> This method creates both a resource and a heap, such that the heap is big enough to contain the entire resource and the resource is mapped to the heap. The created heap is known as an implicit heap, because the heap object cannot be obtained by the application. The application must ensure the GPU will no longer read or write to this resource before releasing the final reference on the resource. </p><p> The implicit heap is made resident for GPU access before the method returns to the application. See Residency. </p><p> The resource GPU VA mapping cannot be changed. See <strong><see cref="SharpDX.Direct3D12.CommandQueue.UpdateTileMappings"/></strong> and Volume Tiled Resources. </p><p> This method may be called by multiple threads concurrently. </p>	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12Device::CreateCommittedResource']/*"/>	
        /// <msdn-id>dn899178</msdn-id>	
        /// <unmanaged>HRESULT ID3D12Device::CreateCommittedResource([In] const D3D12_HEAP_PROPERTIES* pHeapProperties,[In] D3D12_HEAP_FLAGS HeapFlags,[In, Value] const D3D12_RESOURCE_DESC* pResourceDesc,[In] D3D12_RESOURCE_STATES InitialResourceState,[In, Optional] const D3D12_CLEAR_VALUE* pOptimizedClearValue,[In] const GUID&amp; riidResource,[Out] ID3D12Resource** ppvResource)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::CreateCommittedResource</unmanaged-short>	
        public SharpDX.Direct3D12.Resource CreateCommittedResource(SharpDX.Direct3D12.HeapProperties heapPropertiesRef, SharpDX.Direct3D12.HeapFlags heapFlags, SharpDX.Direct3D12.ResourceDescription resourceDescRef, SharpDX.Direct3D12.ResourceStates initialResourceState, SharpDX.Direct3D12.ClearValue? optimizedClearValueRef = null)
        {
            return CreateCommittedResource(ref heapPropertiesRef, heapFlags, ref resourceDescRef, initialResourceState, optimizedClearValueRef, Utilities.GetGuidFromType(typeof(Resource)));
        }

        /// <summary>	
        /// <p>Creates a compute pipeline state object.</p>	
        /// </summary>	
        /// <param name="descRef"><dd>  <p> A reference to a <strong><see cref="SharpDX.Direct3D12.ComputePipelineStateDescription"/></strong> structure that describes compute pipeline state. </p> </dd></param>	
        /// <param name="riid"><dd>  <p> The globally unique identifier (<strong><see cref="System.Guid"/></strong>) for the pipeline state interface (<strong><see cref="SharpDX.Direct3D12.PipelineState"/></strong>). The <strong>REFIID</strong>, or <strong><see cref="System.Guid"/></strong>, of the interface to the pipeline state can be obtained by using the __uuidof() macro. For example, __uuidof(<see cref="SharpDX.Direct3D12.PipelineState"/>) will get the <strong><see cref="System.Guid"/></strong> of the interface to a pipeline state. </p> </dd></param>	
        /// <returns><dd>  <p> A reference to a memory block that receives a reference to the <strong><see cref="SharpDX.Direct3D12.PipelineState"/></strong> interface for the pipeline state object. The pipeline state object is an immutable state object.  It contains no methods. </p> </dd></returns>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12Device::CreateComputePipelineState']/*"/>	
        /// <msdn-id>dn788658</msdn-id>	
        /// <unmanaged>HRESULT ID3D12Device::CreateComputePipelineState([In] const void* pDesc,[In] const GUID&amp; riid,[Out] ID3D12PipelineState** ppPipelineState)</unmanaged>	
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

        /// <summary>	
        /// Creates a root signature layout.	
        /// </summary>	
        /// <param name="nodeMask">No documentation.</param>	
        /// <param name="blobWithRootSignatureRef">No documentation.</param>	
        /// <param name="blobLengthInBytes">No documentation.</param>	
        /// <param name="riid">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12Device::CreateRootSignature']/*"/>	
        /// <msdn-id>dn788658</msdn-id>	
        /// <unmanaged>HRESULT ID3D12Device::CreateRootSignature([In] unsigned int nodeMask,[In, Buffer] const void* pBlobWithRootSignature,[In] SIZE_T blobLengthInBytes,[In] const GUID&amp; riid,[Out] ID3D12RootSignature** ppvRootSignature)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::CreateRootSignature</unmanaged-short>	
        public SharpDX.Direct3D12.RootSignature CreateRootSignature(DataPointer rootSignaturePointer)
        {
            return CreateRootSignature(0, rootSignaturePointer);
        }

        /// <summary>	
        /// Creates a root signature layout.	
        /// </summary>	
        /// <param name="nodeMask">No documentation.</param>	
        /// <param name="blobWithRootSignatureRef">No documentation.</param>	
        /// <param name="blobLengthInBytes">No documentation.</param>	
        /// <param name="riid">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12Device::CreateRootSignature']/*"/>	
        /// <msdn-id>dn788658</msdn-id>	
        /// <unmanaged>HRESULT ID3D12Device::CreateRootSignature([In] unsigned int nodeMask,[In, Buffer] const void* pBlobWithRootSignature,[In] SIZE_T blobLengthInBytes,[In] const GUID&amp; riid,[Out] ID3D12RootSignature** ppvRootSignature)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::CreateRootSignature</unmanaged-short>	
        public SharpDX.Direct3D12.RootSignature CreateRootSignature(int nodeMask, DataPointer rootSignaturePointer)
        {
            return CreateRootSignature(nodeMask, rootSignaturePointer.Pointer, rootSignaturePointer.Size, Utilities.GetGuidFromType(typeof(RootSignature)));
        }

        /// <summary>	
        /// Creates a root signature layout.	
        /// </summary>	
        /// <param name="nodeMask">No documentation.</param>	
        /// <param name="blobWithRootSignatureRef">No documentation.</param>	
        /// <param name="blobLengthInBytes">No documentation.</param>	
        /// <param name="riid">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12Device::CreateRootSignature']/*"/>	
        /// <msdn-id>dn788658</msdn-id>	
        /// <unmanaged>HRESULT ID3D12Device::CreateRootSignature([In] unsigned int nodeMask,[In, Buffer] const void* pBlobWithRootSignature,[In] SIZE_T blobLengthInBytes,[In] const GUID&amp; riid,[Out] ID3D12RootSignature** ppvRootSignature)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::CreateRootSignature</unmanaged-short>	
        public unsafe SharpDX.Direct3D12.RootSignature CreateRootSignature(byte[] byteCode)
        {
            if (byteCode == null)
                throw new ArgumentNullException("byteCode");

            fixed (byte* ptr = &byteCode[0])
            {
                return CreateRootSignature(new DataPointer(new IntPtr(ptr), byteCode.Length));
            }
        }

        /// <summary>	
        /// Creates a descriptor heap object.
        /// </summary>	
        /// <param name="descriptorHeapDesc">No documentation.</param>	
        /// <returns>New instance of <see cref="SharpDX.Direct3D12.DescriptorHeap"/>.</returns>	
        /// <msdn-id>dn788662</msdn-id>
        /// <unmanaged>HRESULT ID3D12Device::CreateDescriptorHeap([In] const D3D12_DESCRIPTOR_HEAP_DESC* pDescriptorHeapDesc,[In] const GUID&amp; riid,[Out] ID3D12DescriptorHeap** ppvHeap)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::CreateDescriptorHeap</unmanaged-short>	
        public SharpDX.Direct3D12.DescriptorHeap CreateDescriptorHeap(SharpDX.Direct3D12.DescriptorHeapDescription descriptorHeapDesc)
        {
            return CreateDescriptorHeap(descriptorHeapDesc, Utilities.GetGuidFromType(typeof(DescriptorHeap)));
        }

        /// <summary>	
        /// Creates a descriptor heap object.
        /// </summary>	
        /// <param name="type">The heap type.</param>	
        /// <param name="descriptorCount">The descriptor count.</param>	
        /// <param name="flags">The optional heap flags.</param>	
        /// <param name="nodeMask">Multi GPU node mask.</param>	
        /// <returns>New instance of <see cref="SharpDX.Direct3D12.DescriptorHeap"/>.</returns>	
        /// <msdn-id>dn788662</msdn-id>
        /// <unmanaged>HRESULT ID3D12Device::CreateDescriptorHeap([In] const D3D12_DESCRIPTOR_HEAP_DESC* pDescriptorHeapDesc,[In] const GUID&amp; riid,[Out] ID3D12DescriptorHeap** ppvHeap)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::CreateDescriptorHeap</unmanaged-short>	
        public SharpDX.Direct3D12.DescriptorHeap CreateDescriptorHeap(DescriptorHeapType type, int descriptorCount, DescriptorHeapFlags flags = DescriptorHeapFlags.None, int nodeMask = 0)
        {
            DescriptorHeapDescription description = new DescriptorHeapDescription
            {
                Type = type,
                DescriptorCount = descriptorCount,
                Flags = flags,
                NodeMask = nodeMask,
            };
            return CreateDescriptorHeap(description, Utilities.GetGuidFromType(typeof(DescriptorHeap)));
        }

        /// <summary>	
        /// Creates a fence object. 	
        /// </summary>	
        /// <param name="initialValue">No documentation.</param>	
        /// <param name="flags">No documentation.</param>	
        /// <param name="riid">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12Device::CreateFence']/*"/>	
        /// <msdn-id>dn899179</msdn-id>
        /// <unmanaged>HRESULT ID3D12Device::CreateFence([In] unsigned longlong InitialValue,[In] D3D12_FENCE_FLAGS Flags,[In] const GUID&amp; riid,[Out] ID3D12Fence** ppFence)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::CreateFence</unmanaged-short>	
        public Fence CreateFence(long initialValue, SharpDX.Direct3D12.FenceFlags flags)
        {
            return CreateFence(initialValue, flags, Utilities.GetGuidFromType(typeof(Fence)));
        }

        /// <summary>	
        /// Creates a graphics pipeline state object.
        /// </summary>	
        /// <param name="descRef">No documentation.</param>	
        /// <param name="riid">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12Device::CreateGraphicsPipelineState']/*"/>	
        /// <msdn-id>dn788663</msdn-id>
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
                var elements = desc.InputLayout != null ? desc.InputLayout.Elements : null;
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
                var streamOutElements = desc.StreamOutput != null ? desc.StreamOutput.Elements : null;
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
                            elements[i].__MarshalFree(ref nativeElements[i]);
                        }
                    }

                    if (streamOutElements != null)
                    {
                        for (int i = 0; i < streamOutElements.Length; i++)
                        {
                            streamOutElements[i].__MarshalFree(ref nativeStreamOutElements[i]);
                        }
                    }
                }
            }
        }

        /// <summary>	
        /// <p> Creates a heap that can be used with placed resources and reserved resources. </p>	
        /// </summary>	
        /// <param name="descRef"><dd>  <p> A reference to a <strong><see cref="SharpDX.Direct3D12.HeapDescription"/></strong> structure that describes the heap. </p> </dd></param>	
        /// <param name="riid"><dd>  <p> The globally unique identifier (<strong><see cref="System.Guid"/></strong>) for the heap interface. This is an input parameter. The <strong>REFIID</strong>, or <strong><see cref="System.Guid"/></strong>, of the interface to the heap can be obtained by using the __uuidof() macro. For example, __uuidof(<strong><see cref="SharpDX.Direct3D12.Heap"/></strong>) will get the <strong><see cref="System.Guid"/></strong> of the interface to a heap. <em>riid</em> is, most commonly, the <see cref="System.Guid"/> for <strong><see cref="SharpDX.Direct3D12.Heap"/></strong>, but it may be any <see cref="System.Guid"/> for any interface. If the resource object does not support the interface for the specified <see cref="System.Guid"/>, creation will fail with E_NOINTERFACE. </p> </dd></param>	
        /// <returns><dd>  <p> A reference to a memory block that receives a reference to the heap. <em>ppvHeap</em> can be <c>null</c>, to enable capability testing. When <em>ppvHeap</em> is <c>null</c>, no object will be created and S_FALSE will be returned when <em>pDesc</em> is valid. </p> </dd></returns>	
        /// <remarks>	
        /// <p><strong>CreateHeap</strong> creates a heap that can be used with placed resources and reserved resources. Before releasing the final reference on the heap, the application must ensure that the GPU will no longer read or write to this heap. Placed resource objects will hold a reference on the heap they are created on, but reserved resources will not hold a reference for each mapping made to a heap. </p>	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12Device::CreateHeap']/*"/>	
        /// <msdn-id>dn788664</msdn-id>	
        /// <unmanaged>HRESULT ID3D12Device::CreateHeap([In] const D3D12_HEAP_DESC* pDesc,[In] const GUID&amp; riid,[Out] ID3D12Heap** ppvHeap)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::CreateHeap</unmanaged-short>	
        public SharpDX.Direct3D12.Heap CreateHeap(SharpDX.Direct3D12.HeapDescription descRef)
        {
            return CreateHeap(ref descRef, Utilities.GetGuidFromType(typeof(Heap)));
        }

        /// <summary>	
        /// <p> Creates a query heap. A query heap contains an array of queries. </p>	
        /// </summary>	
        /// <param name="descRef"><dd>  <p> Specifies the query heap in a <strong><see cref="SharpDX.Direct3D12.QueryHeapDescription"/></strong> structure. </p> </dd></param>	
        /// <param name="riid"><dd>  <p> Specifies a REFIID that uniquely identifies the heap. </p> </dd></param>	
        /// <returns><dd>  <p> Specifies a reference to the heap, that will be returned on successful completion of the method. <em>ppvHeap</em> can be <c>null</c>, to enable capability testing. When <em>ppvHeap</em> is <c>null</c>, no object will be created and S_FALSE will be returned when <em>pDesc</em> is valid. </p> </dd></returns>	
        /// <remarks>	
        /// <p> Refer to Queries for more information. </p>	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12Device::CreateQueryHeap']/*"/>	
        /// <msdn-id>dn903828</msdn-id>	
        /// <unmanaged>HRESULT ID3D12Device::CreateQueryHeap([In] const D3D12_QUERY_HEAP_DESC* pDesc,[In] const GUID&amp; riid,[Out] ID3D12QueryHeap** ppvHeap)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::CreateQueryHeap</unmanaged-short>	
        public SharpDX.Direct3D12.QueryHeap CreateQueryHeap(SharpDX.Direct3D12.QueryHeapDescription descRef)
        {
            return CreateQueryHeap(descRef, Utilities.GetGuidFromType(typeof(QueryHeap)));
        }

        /// <summary>	
        /// <p> Creates a resource that is placed in a specific heap. Placed resources are the lightest weight resource objects available, and are the fastest to create and destroy. </p>	
        /// </summary>	
        /// <param name="heapRef"><dd>  <p> A reference to the <strong><see cref="SharpDX.Direct3D12.Heap"/></strong> interface that represents the heap in which the resource is placed. </p> </dd></param>	
        /// <param name="heapOffset"><dd>  <p> The offset, in bytes, to the resource. The <em>HeapOffset</em> must be a multiple of the resource's alignment, and <em>HeapOffset</em> plus the resource size must be smaller than or equal to the heap size. <strong>GetResourceAllocationInfo</strong> must be used to understand the sizes of texture resources. </p> </dd></param>	
        /// <param name="descRef"><dd>  <p> A reference to a <strong><see cref="SharpDX.Direct3D12.ResourceDescription"/></strong> structure that describes the resource. </p> </dd></param>	
        /// <param name="initialState"><dd>  <p> The initial state of the resource, as a bitwise-OR'd combination of <strong><see cref="SharpDX.Direct3D12.ResourceStates"/></strong> enumeration constants. </p> <p> When a resource is created together with a <see cref="SharpDX.Direct3D12.HeapType.Upload"/> heap, <em>InitialState</em> must be <see cref="SharpDX.Direct3D12.ResourceStates.GenericRead"/>. When a resource is created together with a <see cref="SharpDX.Direct3D12.HeapType.Readback"/> heap, <em>InitialState</em> must be <see cref="SharpDX.Direct3D12.ResourceStates.CopyDestination"/>. </p> </dd></param>	
        /// <param name="optimizedClearValueRef"><dd>  <p> Specifies a <strong><see cref="SharpDX.Direct3D12.ClearValue"/></strong> that describes the default value for a clear color. </p> <p><em>pOptimizedClearValue</em> specifies a value for which clear operations are most optimal. When the created resource is a texture with either the <see cref="SharpDX.Direct3D12.ResourceFlags.AllowRenderTarget"/> or <see cref="SharpDX.Direct3D12.ResourceFlags.AllowDepthStencil"/> flags, applications should choose the value that the clear operation will most commonly be called with. Clear operations can be called with other values, but those operations will not be as efficient as when the value matches the one passed into resource creation. <em>pOptimizedClearValue</em> must be <c>null</c> when used with <see cref="SharpDX.Direct3D12.ResourceDimension.Buffer"/>. </p> </dd></param>	
        /// <param name="riid"><dd>  <p> The globally unique identifier (<strong><see cref="System.Guid"/></strong>) for the resource interface. This is an input parameter. </p> <p> The <strong>REFIID</strong>, or <strong><see cref="System.Guid"/></strong>, of the interface to the resource can be obtained by using the __uuidof() macro. For example, __uuidof(<strong><see cref="SharpDX.Direct3D12.Resource"/></strong>) will get the <strong><see cref="System.Guid"/></strong> of the interface to a resource. Although <strong>riid</strong> is, most commonly, the <see cref="System.Guid"/> for <strong><see cref="SharpDX.Direct3D12.Resource"/></strong>, it may be any <see cref="System.Guid"/> for any interface.  If the resource object doesn't support the interface for this <see cref="System.Guid"/>, creation will fail with E_NOINTERFACE. </p> </dd></param>	
        /// <returns><dd>  <p> A reference to a memory block that receives a reference to the resource. <em>ppvResource</em> can be <c>null</c>, to enable capability testing.  When <em>ppvResource</em> is <c>null</c>, no object will be created and S_FALSE will be returned when <em>pResourceDesc</em> and other parameters are valid. </p> </dd></returns>	
        /// <remarks>	
        /// <p><strong>CreatePlacedResource</strong> is similar to fully mapping a reserved resource to an offset within a heap; but the virtual address space associated with a heap may be reused as well. </p><p> Placed resources are lighter weight than committed resources to create and destroy, because no heap is created or destroyed during this operation.  However, placed resources enable an even lighter weight technique to reuse memory than resource creation and destruction: reuse through aliasing and aliasing barriers. Multiple placed resources may simultaneously overlap each other on the same heap, but only a single overlapping resource can be used at a time. </p><p> There are two placed resource usage semantics, a simple model and an advanced model. The simple model is recommended, and is the most likely model for tool support, until the advanced model is proven to be required by the app. </p>	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12Device::CreatePlacedResource']/*"/>	
        /// <msdn-id>dn899180</msdn-id>	
        /// <unmanaged>HRESULT ID3D12Device::CreatePlacedResource([In] ID3D12Heap* pHeap,[In] unsigned longlong HeapOffset,[In] const D3D12_RESOURCE_DESC* pDesc,[In] D3D12_RESOURCE_STATES InitialState,[In, Optional] const D3D12_CLEAR_VALUE* pOptimizedClearValue,[In] const GUID&amp; riid,[Out] ID3D12Resource** ppvResource)</unmanaged>	
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
        /// Creates a resource that is reserved, which is not yet mapped to any pages in a heap. 	
        /// </summary>	
        /// <param name="descRef">No documentation.</param>	
        /// <param name="initialState">No documentation.</param>	
        /// <param name="optimizedClearValueRef">No documentation.</param>	
        /// <param name="riid">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <msdn-id>dn899181</msdn-id>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12Device::CreateReservedResource']/*"/>	
        /// <unmanaged>HRESULT ID3D12Device::CreateReservedResource([In] const D3D12_RESOURCE_DESC* pDesc,[In] D3D12_RESOURCE_STATES InitialState,[In, Optional] const D3D12_CLEAR_VALUE* pOptimizedClearValue,[In] const GUID&amp; riid,[Out] ID3D12Resource** ppvResource)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::CreateReservedResource</unmanaged-short>	
        public SharpDX.Direct3D12.Resource CreateReservedResource(SharpDX.Direct3D12.ResourceDescription descRef,
            SharpDX.Direct3D12.ResourceStates initialState,
            SharpDX.Direct3D12.ClearValue? optimizedClearValueRef = null)
        {
            return CreateReservedResource(ref descRef, initialState, optimizedClearValueRef, Utilities.GetGuidFromType(typeof(Resource)));
        }

        /// <summary>	
        /// <p> Gets the size and alignment of memory required for a collection of resources on this adapter. </p>	
        /// </summary>	
        /// <param name="visibleMask"><dd>  <p> For single GPU operation, set this to zero.  If there are multiple GPU nodes, set bits to identify the nodes (the  device's physical adapters). Each bit in the mask corresponds to a single node. Refer to Multi-Adapter.</p> </dd></param>	
        /// <param name="numResourceDescs"><dd>  <p> The number of resource descriptors in the <em>pResourceDescs</em> array. </p> </dd></param>	
        /// <param name="resourceDescsRef"><dd>  <p> An array of <strong><see cref="SharpDX.Direct3D12.ResourceDescription"/></strong> structures that described the resources to get info about. </p> </dd></param>	
        /// <returns><p> Returns a <strong><see cref="SharpDX.Direct3D12.ResourceAllocationInformation"/></strong> structure that provides info about video memory allocated for the specified array of resources. </p></returns>	
        /// <remarks>	
        /// <p> When using <strong>CreatePlacedResource</strong>, the application must use this method to understand the size and alignment characteristics of texture resources.  The results of this method vary depending on the particular adapter, and must be treated as unique to this adapter and driver version. </p><p> Applications cannot use the output of <strong>GetResourceAllocationInfo</strong> to understand packed mip properties of textures. To understand packed mip properties of textures, applications must use <strong>GetResourceTiling</strong>.  Texture resource sizes significantly differ from the information returned by <strong>GetResourceTiling</strong>, because some adapter architectures allocate extra memory for textures to reduce the effective bandwidth during common rendering scenarios.  This even includes textures that have constraints on their texture layouts or have standardized texture layouts.  That extra memory cannot be sparsely mapped or remapped by an application using <strong>CreateReservedResource</strong> and  <strong>UpdateTileMappings</strong>, so it isn't reported in <strong>GetResourceTiling</strong>. </p><p> Applications can forgo using <strong>GetResourceAllocationInfo</strong> for buffer resources (<strong><see cref="SharpDX.Direct3D12.ResourceDimension"/></strong>_BUFFER).  Buffers have the same size on all adapters, which is merely the smallest multiple of 64KB which is greater or equal to <strong><see cref="SharpDX.Direct3D12.ResourceDescription"/></strong>::<strong>Width</strong>. </p><p> When multiple resource descriptions are passed in, the C++ algorithm for calculating a structure size and alignment are used.  For example, a three-element array with two tiny 64KB-aligned resources and a tiny 4MB-aligned resource reports differing sizes based on the order of the array.  If the 4MB aligned resource is in the middle, the resulting <strong>Size</strong> is 12MB.  Otherwise, the resulting <strong>Size</strong> is 8MB.  The <strong>Alignment</strong> returned would always be 4MB, as it is the superset of all alignments in the resource array. </p>	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12Device::GetResourceAllocationInfo']/*"/>	
        /// <msdn-id>dn788680</msdn-id>	
        /// <unmanaged>D3D12_RESOURCE_ALLOCATION_INFO ID3D12Device::GetResourceAllocationInfo([In] unsigned int visibleMask,[In] unsigned int numResourceDescs,[In, Buffer] const D3D12_RESOURCE_DESC* pResourceDescs)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::GetResourceAllocationInfo</unmanaged-short>	
        public SharpDX.Direct3D12.ResourceAllocationInformation GetResourceAllocationInfo(int visibleMask, SharpDX.Direct3D12.ResourceDescription resourceDesc)
        {
            return GetResourceAllocationInfo(visibleMask, 1, new ResourceDescription[] { resourceDesc });
        }

        /// <summary>	
        /// <p> Gets information about the features that are supported by the current graphics driver.</p>	
        /// </summary>	
        /// <param name="feature"><dd>  <p> A <strong><see cref="SharpDX.Direct3D12.Feature"/></strong>-typed value that describes the feature to query for support. </p> </dd></param>	
        /// <param name="featureSupportDataRef"><dd>  <p> The passed structure is filled with data that describes the feature support. To see the structure types, see the Remarks section in <strong><see cref="SharpDX.Direct3D12.Feature"/> enumeration</strong>. </p> </dd></param>	
        /// <param name="featureSupportDataSize"><dd>  <p> The size of the structure passed to the <em>pFeatureSupportData</em> parameter. </p> </dd></param>	
        /// <returns><p> Returns <strong><see cref="SharpDX.Result.Ok"/></strong> if successful; otherwise, returns <strong>E_INVALIDARG</strong> if an unsupported data type is passed to the <em>pFeatureSupportData</em> parameter or a size mismatch is detected for the <em>FeatureSupportDataSize</em> parameter. </p></returns>	
        /// <remarks>	
        /// <p>Refer to Capability Querying.</p>	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12Device::CheckFeatureSupport']/*"/>	
        /// <msdn-id>dn788653</msdn-id>	
        /// <unmanaged>HRESULT ID3D12Device::CheckFeatureSupport([In] D3D12_FEATURE Feature,[Out, Buffer] void* pFeatureSupportData,[In] unsigned int FeatureSupportDataSize)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::CheckFeatureSupport</unmanaged-short>	
        public unsafe FeatureDataD3D12Options D3D12Options
        {
            get
            {
                FeatureDataD3D12Options options = new FeatureDataD3D12Options();
                this.CheckFeatureSupport(Feature.D3D12Options, new IntPtr(&options), Utilities.SizeOf<FeatureDataD3D12Options>());
                return options;
            }
        }

        public unsafe FeatureDataD3D12Options1 D3D12Options1
        {
            get
            {
                FeatureDataD3D12Options1 options = new FeatureDataD3D12Options1();
                this.CheckFeatureSupport(Feature.D3D12Options1, new IntPtr(&options), Utilities.SizeOf<FeatureDataD3D12Options1>());
                return options;
            }
        }

        public unsafe FeatureDataD3D12Options2 D3D12Options2
        {
            get
            {
                FeatureDataD3D12Options2 options = new FeatureDataD3D12Options2();
                this.CheckFeatureSupport(Feature.D3D12Options2, new IntPtr(&options), Utilities.SizeOf<FeatureDataD3D12Options2>());
                return options;
            }
        }

        public unsafe FeatureDataD3D12Options3 D3D12Options3
        {
            get
            {
                FeatureDataD3D12Options3 options = new FeatureDataD3D12Options3();
                this.CheckFeatureSupport(Feature.D3D12Options3, new IntPtr(&options), Utilities.SizeOf<FeatureDataD3D12Options3>());
                return options;
            }
        }

        public unsafe FeatureDataD3D12Options4 D3D12Options4
        {
            get
            {
                FeatureDataD3D12Options4 options = new FeatureDataD3D12Options4();
                this.CheckFeatureSupport(Feature.D3D12Options4, new IntPtr(&options), Utilities.SizeOf<FeatureDataD3D12Options4>());
                return options;
            }
        }

        /// <summary>	
        /// <p> Gets information about the features that are supported by the current graphics driver.</p>	
        /// </summary>	
        /// <param name="feature"><dd>  <p> A <strong><see cref="SharpDX.Direct3D12.Feature"/></strong>-typed value that describes the feature to query for support. </p> </dd></param>	
        /// <param name="featureSupportDataRef"><dd>  <p> The passed structure is filled with data that describes the feature support. To see the structure types, see the Remarks section in <strong><see cref="SharpDX.Direct3D12.Feature"/> enumeration</strong>. </p> </dd></param>	
        /// <param name="featureSupportDataSize"><dd>  <p> The size of the structure passed to the <em>pFeatureSupportData</em> parameter. </p> </dd></param>	
        /// <returns><p> Returns <strong><see cref="SharpDX.Result.Ok"/></strong> if successful; otherwise, returns <strong>E_INVALIDARG</strong> if an unsupported data type is passed to the <em>pFeatureSupportData</em> parameter or a size mismatch is detected for the <em>FeatureSupportDataSize</em> parameter. </p></returns>	
        /// <remarks>	
        /// <p>Refer to Capability Querying.</p>	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D12Device::CheckFeatureSupport']/*"/>	
        /// <msdn-id>dn788653</msdn-id>	
        /// <unmanaged>HRESULT ID3D12Device::CheckFeatureSupport([In] D3D12_FEATURE Feature,[Out, Buffer] void* pFeatureSupportData,[In] unsigned int FeatureSupportDataSize)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::CheckFeatureSupport</unmanaged-short>	
        public unsafe FeatureDataArchitecture Architecture
        {
            get
            {
                FeatureDataArchitecture options = new FeatureDataArchitecture();
                this.CheckFeatureSupport(Feature.Architecture, new IntPtr(&options), Utilities.SizeOf<FeatureDataArchitecture>());
                return options;
            }
        }

        public unsafe FeatureDataGpuVirtualAddressSupport GpuVirtualAddressSupport
        {
            get
            {
                FeatureDataGpuVirtualAddressSupport options = new FeatureDataGpuVirtualAddressSupport();
                this.CheckFeatureSupport(Feature.GpuVirtualAddressSupport, new IntPtr(&options), Utilities.SizeOf<FeatureDataGpuVirtualAddressSupport>());
                return options;
            }
        }

        public unsafe FeatureDataShaderModel CheckShaderModel(ShaderModel highestShaderModel)
        {
            var options = new FeatureDataShaderModel
            {
                HighestShaderModel = highestShaderModel
            };
            this.CheckFeatureSupport(Feature.ShaderModel, new IntPtr(&options), Utilities.SizeOf<FeatureDataShaderModel>());
            return options;
        }

        public unsafe FeatureLevel CheckMaxSupportedFeatureLevel(params FeatureLevel[] levels)
        {
            fixed (FeatureLevel* levelsPtr = &levels[0])
            {
                var featureLevels = new FeatureDataFeatureLevels
                {
                    FeatureLevelCount = levels.Length,
                    FeatureLevelsRequestedPointer = new IntPtr(levelsPtr)
                };

                this.CheckFeatureSupport(Feature.FeatureLevels, new IntPtr(&featureLevels), Utilities.SizeOf<FeatureDataFeatureLevels>());
                return featureLevels.MaxSupportedFeatureLevel;
            }
        }

        private static void CreateDevice(Adapter adapter, FeatureLevel minFeatureLevel, Device instance)
        {
            D3D12.CreateDevice(adapter, minFeatureLevel, Utilities.GetGuidFromType(typeof(Device)), instance).CheckError();
        }
    }
}