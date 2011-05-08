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

namespace SharpDX.Direct3D10
{
    public partial class Buffer
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D10.Buffer" /> class.
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
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D10.Buffer" /> class.
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
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D10.Buffer" /> class.
        /// </summary>
        /// <param name = "device">The device with which to associate the buffer.</param>
        /// <param name = "sizeInBytes">The size, in bytes, of the buffer.</param>
        /// <param name = "usage">The usage pattern for the buffer.</param>
        /// <param name = "bindFlags">Flags specifying how the buffer will be bound to the pipeline.</param>
        /// <param name = "accessFlags">Flags specifying how the buffer will be accessible from the CPU.</param>
        /// <param name = "optionFlags">Miscellaneous resource options.</param>
        public Buffer(Device device, int sizeInBytes, ResourceUsage usage, BindFlags bindFlags,
                      CpuAccessFlags accessFlags, ResourceOptionFlags optionFlags)
            : base(IntPtr.Zero)
        {
            var description = new BufferDescription()
                                  {
                                      BindFlags = bindFlags,
                                      CpuAccessFlags = accessFlags,
                                      OptionFlags = optionFlags,
                                      SizeInBytes = sizeInBytes,
                                      Usage = usage,                                      
                                  };

            Buffer buffer;
            device.CreateBuffer(ref description, null, out buffer);
            NativePointer = buffer.NativePointer;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D10.Buffer" /> class.
        /// </summary>
        /// <param name = "device">The device with which to associate the buffer.</param>
        /// <param name = "data">Initial data used to initialize the buffer.</param>
        /// <param name = "sizeInBytes">The size, in bytes, of the buffer.</param>
        /// <param name = "usage">The usage pattern for the buffer.</param>
        /// <param name = "bindFlags">Flags specifying how the buffer will be bound to the pipeline.</param>
        /// <param name = "accessFlags">Flags specifying how the buffer will be accessible from the CPU.</param>
        /// <param name = "optionFlags">Miscellaneous resource options.</param>
        public Buffer(Device device, DataStream data, int sizeInBytes, ResourceUsage usage, BindFlags bindFlags,
                      CpuAccessFlags accessFlags, ResourceOptionFlags optionFlags)
            : base(IntPtr.Zero)
        {
            var description = new BufferDescription()
                                  {
                                      BindFlags = bindFlags,
                                      CpuAccessFlags = accessFlags,
                                      OptionFlags = optionFlags,
                                      SizeInBytes = sizeInBytes,
                                      Usage = usage,
                                  };

            var subResourceData = new SubResourceData();
            subResourceData.DataPointer = data.DataPointer;
            subResourceData.Pitch = 0;
            subResourceData.SlicePitch = 0;

            Buffer buffer;
            device.CreateBuffer(ref description, subResourceData, out buffer);
            NativePointer = buffer.NativePointer;
        }

        /// <summary>	
        /// Get a reference to the data contained in the resource and deny GPU access to the resource.	
        /// </summary>	
        /// <remarks>	
        /// For the CPU to write the contents of a resource, the resource must be created with the dynamic usage flag, D3D10_USAGE_DYNAMIC.  To both read and write those contents, the resource must be created with the staging usage flag, D3D10_USAGE_STAGING. (For more information about  these flags, see <see cref="SharpDX.Direct3D10.ResourceUsage"/>.) ID3D10Buffer::Map will retrieve a reference to the resource data.  For a discussion on how to access resources efficiently, see {{Copying and Accessing Resource Data (Direct3D 10)}}. Call <see cref="SharpDX.Direct3D10.Buffer.Unmap"/> to signify that the application has finished accessing the resource. ID3D10Buffer::Map has a few other restrictions. For example:  The same buffer cannot be mapped multiple times; in other words, do not call ID3D10Buffer::Map on a buffer that is already mapped. Any buffer that is bound to the pipeline must be unmapped before any rendering operation (that is, <see cref="SharpDX.Direct3D10.Device.Draw"/>)  can be executed.    Differences between Direct3D 9 and Direct3D 10: ID3D10Buffer::Map in Direct3D 10 is analogous to resource {{Lock}} in Direct3D 9.   ? 	
        /// </remarks>	
        /// <param name="mapType">Flag that specifies the CPU's permissions for the reading and writing of a resource. For possible values, see <see cref="SharpDX.Direct3D10.MapMode"/>. </param>
        /// <returns>If this function succeeds returns a <see cref="SharpDX.DataStream"/> with the size this buffer.</returns>
        /// <unmanaged>HRESULT ID3D10Buffer::Map([In] D3D10_MAP MapType,[In] int MapFlags,[Out] void** ppData)</unmanaged>
        public DataStream Map(SharpDX.Direct3D10.MapMode mapType)
        {
            return Map(mapType, MapFlags.None);
        }

        /// <summary>	
        /// Get a reference to the data contained in the resource and deny GPU access to the resource.	
        /// </summary>	
        /// <remarks>	
        /// For the CPU to write the contents of a resource, the resource must be created with the dynamic usage flag, D3D10_USAGE_DYNAMIC.  To both read and write those contents, the resource must be created with the staging usage flag, D3D10_USAGE_STAGING. (For more information about  these flags, see <see cref="SharpDX.Direct3D10.ResourceUsage"/>.) ID3D10Buffer::Map will retrieve a reference to the resource data.  For a discussion on how to access resources efficiently, see {{Copying and Accessing Resource Data (Direct3D 10)}}. Call <see cref="SharpDX.Direct3D10.Buffer.Unmap"/> to signify that the application has finished accessing the resource. ID3D10Buffer::Map has a few other restrictions. For example:  The same buffer cannot be mapped multiple times; in other words, do not call ID3D10Buffer::Map on a buffer that is already mapped. Any buffer that is bound to the pipeline must be unmapped before any rendering operation (that is, <see cref="SharpDX.Direct3D10.Device.Draw"/>)  can be executed.    Differences between Direct3D 9 and Direct3D 10: ID3D10Buffer::Map in Direct3D 10 is analogous to resource {{Lock}} in Direct3D 9.   ? 	
        /// </remarks>	
        /// <param name="mode">Flag that specifies the CPU's permissions for the reading and writing of a resource. For possible values, see <see cref="SharpDX.Direct3D10.MapMode"/>. </param>
        /// <param name="mapFlags">Flag that specifies what the CPU should do when the GPU is busy (see <see cref="SharpDX.Direct3D10.MapFlags"/>). This flag is optional. </param>
        /// <returns>If this function succeeds returns a <see cref="SharpDX.DataStream"/> with the size this buffer.</returns>
        /// <unmanaged>HRESULT ID3D10Buffer::Map([In] D3D10_MAP MapType,[In] int MapFlags,[Out] void** ppData)</unmanaged>
        public DataStream Map(SharpDX.Direct3D10.MapMode mode, SharpDX.Direct3D10.MapFlags mapFlags)
        {
            int sizeInBytes = Description.SizeInBytes;
            IntPtr data;
            Map(mode, mapFlags, out data);
            bool canRead = mode == MapMode.Read || mode == MapMode.ReadWrite;
            bool canWrite = mode != MapMode.Read;
            return new DataStream(data, sizeInBytes, canRead, canWrite);
        }
    }
}