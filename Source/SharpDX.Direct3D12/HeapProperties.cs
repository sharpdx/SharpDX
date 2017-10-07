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
        /// <param name="cpuPageProperty">The cpu page properties.</param>
        /// <param name="memoryPoolPreference">The memory pool preference.</param>
        /// <param name="creationNodeMask"></param>
        /// <param name="visibleNodeMask"></param>
        public HeapProperties(
            HeapType type,
            CpuPageProperty cpuPageProperty,
            MemoryPool memoryPoolPreference,
            int creationNodeMask = 1,
            int visibleNodeMask = 1)
        {
            Type = type;
            CPUPageProperty = cpuPageProperty;
            MemoryPoolPreference = memoryPoolPreference;
            CreationNodeMask = creationNodeMask;
            VisibleNodeMask = visibleNodeMask;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeapProperties"/> struct with <see cref="HeapType.Custom"/>
        /// </summary>
        /// <param name="cpuPageProperty">The cpu page properties.</param>
        /// <param name="memoryPoolPreference">The memory pool preference.</param>
        /// <param name="creationNodeMask"></param>
        /// <param name="visibleNodeMask"></param>
        public HeapProperties(
            CpuPageProperty cpuPageProperty,
            MemoryPool memoryPoolPreference,
            int creationNodeMask = 1,
            int visibleNodeMask = 1)
        {
            Type = HeapType.Custom;
            CPUPageProperty = cpuPageProperty;
            MemoryPoolPreference = memoryPoolPreference;
            CreationNodeMask = creationNodeMask;
            VisibleNodeMask = visibleNodeMask;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeapProperties"/> struct.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="creationNodeMask"></param>
        /// <param name="visibleNodeMask"></param>
        public HeapProperties(HeapType type,
            int creationNodeMask = 1,
            int visibleNodeMask = 1)
        {
            Type = type;
            CPUPageProperty = CpuPageProperty.Unknown;
            MemoryPoolPreference = MemoryPool.Unknown;
            CreationNodeMask = creationNodeMask;
            VisibleNodeMask = visibleNodeMask;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is cpu accessible.
        /// </summary>
        /// <value><c>true</c> if this instance is cpu accessible; otherwise, <c>false</c>.</value>
        public bool IsCpuAccessible
        {
            get
            {
                return
                    Type == HeapType.Upload ||
                    Type == HeapType.Readback ||
                    (Type == HeapType.Custom && (CPUPageProperty == CpuPageProperty.WriteCombine || CPUPageProperty == CpuPageProperty.WriteBack));
            }
        }
    }
}