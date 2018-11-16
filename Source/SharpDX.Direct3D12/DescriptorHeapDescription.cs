// Copyright (c) 2010-2015 SharpDX - Alexandre Mutel
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

namespace SharpDX.Direct3D12
{
    public partial struct DescriptorHeapDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptorHeapDescription"/> struct.
        /// </summary>
        /// <param name="type">The heap type.</param>
        /// <param name="descriptorCount">The descriptor count.</param>
        /// <param name="flags">The optional heap flags.</param>
        /// <param name="nodeMask">Multi GPU node mask.</param>
        public DescriptorHeapDescription(DescriptorHeapType type, int descriptorCount, DescriptorHeapFlags flags = DescriptorHeapFlags.None, int nodeMask = 0)
        {
            Type = type;
            DescriptorCount = descriptorCount;
            Flags = flags;
            NodeMask = nodeMask;
        }
    }
}
