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
        private CommandQueue defaultCommandQueue;

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        /// <param name="driverType">Type of the driver.</param>
        public Device(DriverType driverType)
            : this(driverType, DeviceCreationFlags.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        public Device(Adapter adapter)
            : this(adapter, DeviceCreationFlags.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        /// <param name="driverType">Type of the driver.</param>
        /// <param name="flags">The flags.</param>
        public Device(DriverType driverType, DeviceCreationFlags flags)
            :this(driverType, flags, FeatureLevel.Level_9_1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        /// <param name="flags">The flags.</param>
        public Device(Adapter adapter, DeviceCreationFlags flags)
            : this(adapter, flags, FeatureLevel.Level_9_1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        /// <param name="driverType">Type of the driver.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="minFeatureLevel">The minimum feature level.</param>
        public Device(DriverType driverType, DeviceCreationFlags flags, FeatureLevel minFeatureLevel)
            : base(IntPtr.Zero)
        {
            CreateDevice(null, driverType, flags, minFeatureLevel, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="minFeatureLevel">The minimum feature level.</param>
        public Device(Adapter adapter, DeviceCreationFlags flags, FeatureLevel minFeatureLevel) : base(IntPtr.Zero)
        {
            CreateDevice(adapter, DriverType.Unknown, flags, minFeatureLevel, this);
        }

        /// <summary>
        /// Gets the default command queue.
        /// </summary>
        /// <value>The default command queue.</value>
        public CommandQueue DefaultCommandQueue
        {
            get { return defaultCommandQueue; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Library"/> class.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <exception cref="System.ArgumentNullException">
        /// device
        /// or
        /// buffer
        /// </exception>
        /// <unmanaged>HRESULT ID3D12Device::CreateLibrary([In, Buffer] const void* pLibraryBlob,[In] SIZE_T BlobLength,[Out, Fast] ID3D12Library** ppLibrary)</unmanaged>
        ///   <unmanaged-short>ID3D12Device::CreateLibrary</unmanaged-short>
        public unsafe Library CreateLibrary(byte[] buffer)
        {
            if(buffer == null) throw new ArgumentNullException("buffer");
            fixed(void* ptr = buffer)
            {
                return CreateLibrary((IntPtr)ptr, buffer.Length);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Library"/> class.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <exception cref="System.ArgumentNullException">
        /// device
        /// or
        /// buffer
        /// </exception>
        public Library CreateLibrary(DataPointer buffer)
        {
            if (buffer.Pointer == IntPtr.Zero) throw new ArgumentNullException("buffer");
            return CreateLibrary(buffer.Pointer, buffer.Size);
        }


        /// <summary>	
        /// No documentation for Direct3D12	
        /// </summary>	
        /// <param name="type">No documentation.</param>	
        /// <param name="priority">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <unmanaged>HRESULT ID3D12Device::CreateCommandQueue([In] D3D12_COMMAND_LIST_TYPE type,[In] int priority,[In] const GUID&amp; riid,[Out] ID3D12CommandQueue** ppCommandQueue)</unmanaged>	
        /// <unmanaged-short>ID3D12Device::CreateCommandQueue</unmanaged-short>	
        public SharpDX.Direct3D12.CommandQueue CreateCommandQueue(SharpDX.Direct3D12.CommandListType type, int priority)
        {
            return CreateCommandQueue(type, priority, Utilities.GetGuidFromType(typeof(CommandQueue)));
        }

        /// <summary>
        /// No documentation for Direct3D12
        /// </summary>
        /// <param name="heapPropertiesRef">No documentation.</param>
        /// <param name="heapMiscFlags">No documentation.</param>
        /// <param name="resourceDescRef">No documentation.</param>
        /// <param name="initialResourceState">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ID3D12Device::CreateCommittedResource([In] const D3D12_HEAP_PROPERTIES* pHeapProperties,[In] D3D12_HEAP_MISC_FLAG HeapMiscFlags,[In, Value] const D3D12_RESOURCE_DESC* pResourceDesc,[In] D3D12_RESOURCE_USAGE InitialResourceState,[In] const GUID&amp; riidResource,[Out] ID3D12Resource** ppvResource)</unmanaged>
        ///   <unmanaged-short>ID3D12Device::CreateCommittedResource</unmanaged-short>
        public SharpDX.Direct3D12.Resource CreateCommittedResource(SharpDX.Direct3D12.HeapProperties heapPropertiesRef,
            SharpDX.Direct3D12.HeapMiscFlags heapMiscFlags,
            SharpDX.Direct3D12.ResourceDescription resourceDescRef,
            SharpDX.Direct3D12.ResourceUsage initialResourceState)
        {
            return CreateCommittedResource(heapPropertiesRef, heapMiscFlags, resourceDescRef, initialResourceState, Utilities.GetGuidFromType(typeof(Resource)));
        }

        /// <summary>
        /// No documentation for Direct3D12
        /// </summary>
        /// <param name="heapType">No documentation.</param>
        /// <param name="byteSize">No documentation.</param>
        /// <param name="miscFlags">No documentation.</param>
        /// <returns>No documentation.</returns>
        /// <unmanaged>HRESULT ID3D12Device::CreateBuffer([In] D3D12_HEAP_TYPE HeapType,[In] unsigned longlong ByteSize,[In] D3D12_RESOURCE_MISC_FLAG MiscFlags,[In] const GUID&amp; riid,[Out] ID3D12Resource** ppvBuffer)</unmanaged>
        ///   <unmanaged-short>ID3D12Device::CreateBuffer</unmanaged-short>
        public SharpDX.Direct3D12.Resource CreateBuffer(SharpDX.Direct3D12.HeapType heapType, long byteSize, SharpDX.Direct3D12.ResourceOptionFlags miscFlags)
        {
            return CreateBuffer(heapType, byteSize, miscFlags, Utilities.GetGuidFromType(typeof(Resource)));
        }

        public SharpDX.Direct3D12.RootSignature CreateRootSignature(DataPointer rootSignaturePointer)
        {
            return CreateRootSignature(rootSignaturePointer.Pointer, rootSignaturePointer.Size, Utilities.GetGuidFromType(typeof(RootSignature)));
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
                desc.VertexShader.UpdateNative(ref nativeDesc.VertexShader, pVertexShader);
                desc.GeometryShader.UpdateNative(ref nativeDesc.GeometryShader, pGeometryShader);
                desc.DomainShader.UpdateNative(ref nativeDesc.DomainShader, pDomainShader);
                desc.HullShader.UpdateNative(ref nativeDesc.HullShader, pHullShader);
                desc.PixelShader.UpdateNative(ref nativeDesc.PixelShader, pPixelShader);

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

                try
                {
                    // Create the pipeline state
                    return CreateGraphicsPipelineState(new IntPtr(&nativeDesc));
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
                }
            }
        }

        protected override
            void NativePointerUpdated(IntPtr oldNativePointer)
        {
            base.NativePointerUpdated(oldNativePointer);

            Utilities.Dispose(ref defaultCommandQueue);

            if(NativePointer != IntPtr.Zero)
            {
                GetDefaultCommandQueue(out defaultCommandQueue);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                Utilities.Dispose(ref defaultCommandQueue);
            }

            base.Dispose(disposing);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpDX.Direct3D12.Device" /> class along with a new <see cref="T:SharpDX.DXGI.SwapChain" /> used for rendering.
        /// </summary>
        /// <param name="driverType">Type of the driver.</param>
        /// <param name="flags">A list of runtime layers to enable.</param>
        /// <param name="minFeatureLevel">The minimum feature level.</param>
        /// <param name="swapChainDescription">Details used to create the swap chain.</param>
        /// <param name="swapChain">When the method completes, contains the created swap chain instance.</param>
        /// <returns>The created device instance.</returns>
        public static Device CreateWithSwapChain(DriverType driverType, DeviceCreationFlags flags,
                                                 FeatureLevel minFeatureLevel, SwapChainDescription swapChainDescription,
                                                 out SwapChain swapChain)
        {
            Device device;
            CreateWithSwapChain(null, driverType, flags, minFeatureLevel, swapChainDescription, out device, out swapChain);
            return device;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpDX.Direct3D12.Device" /> class along with a new <see cref="T:SharpDX.DXGI.SwapChain" /> used for rendering.
        /// </summary>
        /// <param name="adapter">The video adapter on which the device should be created.</param>
        /// <param name="flags">A list of runtime layers to enable.</param>
        /// <param name="minFeatureLevel">The minimum feature level.</param>
        /// <param name="swapChainDescription">Details used to create the swap chain.</param>
        /// <param name="swapChain">When the method completes, contains the created swap chain instance.</param>
        /// <returns>The created device instance.</returns>
        public static Device CreateWithSwapChain(Adapter adapter, DeviceCreationFlags flags,
                                                 FeatureLevel minFeatureLevel, SwapChainDescription swapChainDescription,
                                                 out SwapChain swapChain)
        {
            Device device;
            CreateWithSwapChain(adapter, DriverType.Unknown, flags, minFeatureLevel, swapChainDescription,
                                       out device, out swapChain);
            return device;
        }

        private static void CreateWithSwapChain(Adapter adapter, DriverType driverType, DeviceCreationFlags flags,
                                                 FeatureLevel minFeatureLevel, SwapChainDescription swapChainDescription,
                                                 out Device device, out SwapChain swapChain)
        {
            D3D12.CreateDeviceAndSwapChain(adapter,
                driverType,
                flags,
                minFeatureLevel,
                D3D12.SdkVersion,
                ref swapChainDescription,
                Utilities.GetGuidFromType(typeof(SwapChain)),
                out swapChain,
                Utilities.GetGuidFromType(typeof(Device)),
                out device);
        }

        private static void CreateDevice(Adapter adapter, DriverType driverType, DeviceCreationFlags flags,
                                  FeatureLevel minFeatureLevel, Device instance)
        {
            D3D12.CreateDevice(adapter, driverType, flags, minFeatureLevel, D3D12.SdkVersion, Utilities.GetGuidFromType(typeof(Device)), instance).CheckError();
        }
    }
}