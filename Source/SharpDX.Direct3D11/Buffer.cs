// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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
    public partial class Buffer
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D11.Buffer" /> class.
        /// </summary>
        /// <param name = "device">The device with which to associate the buffer.</param>
        /// <param name = "description">The description of the buffer.</param>
        public Buffer(Device device, BufferDescription description)
            : base(IntPtr.Zero)
        {
            Buffer buffer;
            device.CreateBuffer(ref description, null, out buffer);
            NativePointer = buffer.NativePointer;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D11.Buffer" /> class.
        /// </summary>
        /// <param name = "device">The device with which to associate the buffer.</param>
        /// <param name = "data">Initial data used to initialize the buffer.</param>
        /// <param name = "description">The description of the buffer.</param>
        public Buffer(Device device, DataStream data, BufferDescription description)
            : base(IntPtr.Zero)
        {
            var subResourceData = new SubResourceData();
            subResourceData.DataPointer = data.DataPointer;
            subResourceData.Pitch = 0;
            subResourceData.SlicePitch = 0;

            Buffer buffer;
            device.CreateBuffer(ref description, subResourceData, out buffer);
            NativePointer = buffer.NativePointer;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D11.Buffer" /> class.
        /// </summary>
        /// <param name = "device">The device with which to associate the buffer.</param>
        /// <param name = "sizeInBytes">The size, in bytes, of the buffer.</param>
        /// <param name = "usage">The usage pattern for the buffer.</param>
        /// <param name = "bindFlags">Flags specifying how the buffer will be bound to the pipeline.</param>
        /// <param name = "accessFlags">Flags specifying how the buffer will be accessible from the CPU.</param>
        /// <param name = "optionFlags">Miscellaneous resource options.</param>
        /// <param name = "structureByteStride">The size (in bytes) of the structure element for structured buffers.</param>
        public Buffer(Device device, int sizeInBytes, ResourceUsage usage, BindFlags bindFlags,
                      CpuAccessFlags accessFlags, ResourceOptionFlags optionFlags, int structureByteStride)
            : base(IntPtr.Zero)
        {
            var description = new BufferDescription()
                                  {
                                      BindFlags = bindFlags,
                                      CpuAccessFlags = accessFlags,
                                      OptionFlags = optionFlags,
                                      SizeInBytes = sizeInBytes,
                                      Usage = usage,
                                      StructureByteStride = structureByteStride
                                  };

            Buffer buffer;
            device.CreateBuffer(ref description, null, out buffer);
            NativePointer = buffer.NativePointer;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D11.Buffer" /> class.
        /// </summary>
        /// <param name = "device">The device with which to associate the buffer.</param>
        /// <param name = "data">Initial data used to initialize the buffer.</param>
        /// <param name = "sizeInBytes">The size, in bytes, of the buffer.</param>
        /// <param name = "usage">The usage pattern for the buffer.</param>
        /// <param name = "bindFlags">Flags specifying how the buffer will be bound to the pipeline.</param>
        /// <param name = "accessFlags">Flags specifying how the buffer will be accessible from the CPU.</param>
        /// <param name = "optionFlags">Miscellaneous resource options.</param>
        /// <param name = "structureByteStride">The size (in bytes) of the structure element for structured buffers.</param>
        public Buffer(Device device, DataStream data, int sizeInBytes, ResourceUsage usage, BindFlags bindFlags,
                      CpuAccessFlags accessFlags, ResourceOptionFlags optionFlags, int structureByteStride)
            : base(IntPtr.Zero)
        {
            var description = new BufferDescription()
                                  {
                                      BindFlags = bindFlags,
                                      CpuAccessFlags = accessFlags,
                                      OptionFlags = optionFlags,
                                      SizeInBytes = sizeInBytes,
                                      Usage = usage,
                                      StructureByteStride = structureByteStride
                                  };

            var subResourceData = new SubResourceData();
            subResourceData.DataPointer = data.DataPointer;
            subResourceData.Pitch = 0;
            subResourceData.SlicePitch = 0;

            Buffer buffer;
            device.CreateBuffer(ref description, subResourceData, out buffer);
            NativePointer = buffer.NativePointer;
        }
    }
}