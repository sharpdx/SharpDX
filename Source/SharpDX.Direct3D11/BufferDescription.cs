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

namespace SharpDX.Direct3D11
{
    public partial struct BufferDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BufferDescription"/> struct.
        /// </summary>
        /// <param name="sizeInBytes">The size in bytes.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="bindFlags">The bind flags.</param>
        /// <param name="cpuAccessFlags">The CPU access flags.</param>
        /// <param name="optionFlags">The option flags.</param>
        /// <param name="structureByteStride">The structure byte stride.</param>
        public BufferDescription(int sizeInBytes, ResourceUsage usage, BindFlags bindFlags, CpuAccessFlags cpuAccessFlags, ResourceOptionFlags optionFlags, int structureByteStride)
        {
            SizeInBytes = sizeInBytes;
            Usage = usage;
            BindFlags = bindFlags;
            CpuAccessFlags = cpuAccessFlags;
            OptionFlags = optionFlags;
            StructureByteStride = structureByteStride;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferDescription"/> struct.
        /// </summary>
        /// <param name="sizeInBytes">The size in bytes.</param>
        /// <param name="bindFlags">The bind flags.</param>
        /// <param name="usage">The usage.</param>
        public BufferDescription(int sizeInBytes, BindFlags bindFlags, ResourceUsage usage) : this()
        {
            SizeInBytes = sizeInBytes;
            BindFlags = bindFlags;
            Usage = usage;
        }
    }
}