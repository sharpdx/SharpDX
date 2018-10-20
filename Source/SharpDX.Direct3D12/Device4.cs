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
    public partial class Device4
    {
        /// <summary>
        /// <p>Creates a new graphics command list object.</p>	
        /// </summary>
        /// <param name="type">A <see cref="SharpDX.Direct3D12.CommandListType"/> value that specifies the type of command list to create.</param>
        /// <param name="flags">A <see cref="CommandListFlags"/> flags.</param>	
        /// <returns>A new instance of <see cref="GraphicsCommandList1"/>.</returns>	
        public GraphicsCommandList1 CreateCommandList1(SharpDX.Direct3D12.CommandListType type, SharpDX.Direct3D12.CommandListFlags flags)
        {
            return CreateCommandList1(0, type, flags);
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
        /// <param name="flags">A <see cref="CommandListFlags"/> flags.</param>	
        /// <returns>A new instance of <see cref="GraphicsCommandList1"/>.</returns>	
        public GraphicsCommandList1 CreateCommandList1(int nodeMask, SharpDX.Direct3D12.CommandListType type, SharpDX.Direct3D12.CommandListFlags flags)
        {
            var nativePointer = CreateCommandList1(nodeMask, type, flags, Utilities.GetGuidFromType(typeof(GraphicsCommandList1)));
            return new GraphicsCommandList1(nativePointer);
        }

        public ProtectedResourceSession CreateProtectedResourceSession(ProtectedResourceSessionDescription desc)
        {
            return CreateProtectedResourceSession(desc, Utilities.GetGuidFromType(typeof(ProtectedResourceSession)));
        }

        public Heap1 CreateHeap1(HeapDescription desc, ProtectedResourceSession protectedSession)
        {
            return CreateHeap1(ref desc, protectedSession, Utilities.GetGuidFromType(typeof(Heap1)));
        }
    }
}