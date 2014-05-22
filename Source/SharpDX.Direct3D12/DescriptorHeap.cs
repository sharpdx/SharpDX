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
    public partial class DescriptorHeap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptorHeap"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="descriptorHeapDesc">The descriptor heap desc.</param>
        /// <exception cref="System.ArgumentNullException">device</exception>
        /// <unmanaged>HRESULT ID3D12Device::CreateDescriptorHeap([In] const D3D12_DESCRIPTOR_HEAP_DESC* pDescriptorHeapDesc,[Out, Fast] ID3D12DescriptorHeap** ppHeap)</unmanaged>
        ///   <unmanaged-short>ID3D12Device::CreateDescriptorHeap</unmanaged-short>
        public DescriptorHeap(Device device, SharpDX.Direct3D12.DescriptorHeapDescription descriptorHeapDesc) : base(IntPtr.Zero)
        {
            if(device == null) throw new ArgumentNullException("device");
            device.CreateDescriptorHeap(ref descriptorHeapDesc, this);
        }
    }
}