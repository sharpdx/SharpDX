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
namespace SharpDX.Direct3D12
{
    public partial struct HeapProperties
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HeapProperties"/> struct.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="cpuPageProperties">The cpu page properties.</param>
        /// <param name="memoryPoolPreference">The memory pool preference.</param>
        public HeapProperties(HeapType type, CpuPageProperties cpuPageProperties, MemoryPool memoryPoolPreference)
        {
            Type = type;
            CPUPageProperties = cpuPageProperties;
            MemoryPoolPreference = memoryPoolPreference;
            this.CreationNodeMask = 1;
            this.VisibleNodeMask = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeapProperties"/> struct with <see cref="HeapType.Custom"/>
        /// </summary>
        /// <param name="cpuPageProperties">The cpu page properties.</param>
        /// <param name="memoryPoolPreference">The memory pool preference.</param>
        public HeapProperties(CpuPageProperties cpuPageProperties, MemoryPool memoryPoolPreference)
        {
            Type = HeapType.Custom;
            CPUPageProperties = cpuPageProperties;
            MemoryPoolPreference = memoryPoolPreference;
            this.CreationNodeMask = 1;
            this.VisibleNodeMask = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeapProperties"/> struct.
        /// </summary>
        /// <param name="type">The type.</param>
        public HeapProperties(HeapType type)
        {
            Type = type;
            CPUPageProperties = CpuPageProperties.Unknown;
            MemoryPoolPreference = MemoryPool.Unknown;
            this.CreationNodeMask = 1;
            this.VisibleNodeMask = 1;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is cpu accessible.
        /// </summary>
        /// <value><c>true</c> if this instance is cpu accessible; otherwise, <c>false</c>.</value>
        public bool IsCpuAccessible
        {
            get
            {
                return Type == HeapType.Upload || Type == HeapType.Upload || (Type == HeapType.Custom &&
                                                                              (CPUPageProperties == CpuPageProperties.WriteCombine
                                                                               || CPUPageProperties == CpuPageProperties.WriteBack));
            }
        }
    }
}