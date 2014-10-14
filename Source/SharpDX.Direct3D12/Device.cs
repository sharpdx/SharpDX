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
using SharpDX.DXGI;

namespace SharpDX.Direct3D12
{
    public partial class Device
    {
        private FeatureLevel selectedLevel;
        private CommandQueue defaultCommandQueue;

        public Device(DriverType driverType)
            : this(driverType, DeviceCreationFlags.None)
        {
        }

        public Device(Adapter adapter)
            : this(adapter, DeviceCreationFlags.None)
        {
        }

        public Device(DriverType driverType, DeviceCreationFlags flags)
            :this(driverType, flags, FeatureLevel.Level_9_1)
        {
        }

        public Device(Adapter adapter, DeviceCreationFlags flags)
            : this(adapter, flags, FeatureLevel.Level_9_1)
        {
        }

        public Device(DriverType driverType, DeviceCreationFlags flags, FeatureLevel minFeatureLevel)
            : base(IntPtr.Zero)
        {
            CreateDevice(null, driverType, flags, minFeatureLevel);
        }

        public Device(Adapter adapter, DeviceCreationFlags flags, FeatureLevel minFeatureLevel) : base(IntPtr.Zero)
        {
            CreateDevice(adapter, DriverType.Unknown, flags, minFeatureLevel);
        }

        public CommandQueue DefaultCommandQueue
        {
            get { return defaultCommandQueue; }
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

        private void CreateDevice(Adapter adapter, DriverType driverType, DeviceCreationFlags flags,
                                  FeatureLevel minFeatureLevel)
        {

            D3D12.CreateDevice(adapter, driverType, flags, minFeatureLevel, D3D12.SdkVersion, Utilities.GetGuidFromType(typeof(Device)), this).CheckError();
        }
    }
}