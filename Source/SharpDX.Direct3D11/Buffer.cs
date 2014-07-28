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
    public partial class Buffer
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D11.Buffer" /> class.
        /// </summary>
        /// <param name = "device">The device with which to associate the buffer.</param>
        /// <param name = "description">The description of the buffer.</param>
        /// <msdn-id>ff476501</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>	
        public Buffer(Device device, BufferDescription description)
            : base(IntPtr.Zero)
        {
            device.CreateBuffer(ref description, null, this);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D11.Buffer" /> class.
        /// </summary>
        /// <param name = "device">The device with which to associate the buffer.</param>
        /// <param name = "data">Initial data used to initialize the buffer.</param>
        /// <param name = "description">The description of the buffer.</param>
        /// <msdn-id>ff476501</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>	
        public Buffer(Device device, DataStream data, BufferDescription description)
            : base(IntPtr.Zero)
        {
            device.CreateBuffer(ref description, new DataBox(data.PositionPointer, 0, 0), this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpDX.Direct3D11.Buffer" /> class.
        /// </summary>
        /// <param name="device">The device with which to associate the buffer.</param>
        /// <param name="dataPointer">The data pointer.</param>
        /// <param name="description">The description of the buffer.</param>
        /// <msdn-id>ff476501</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>	
        public Buffer(Device device, IntPtr dataPointer, BufferDescription description)
            : base(IntPtr.Zero)
        {
            device.CreateBuffer(ref description, dataPointer != IntPtr.Zero ? new DataBox(dataPointer, 0, 0) : (DataBox?)null, this);
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
        /// <msdn-id>ff476501</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>	
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

            device.CreateBuffer(ref description, null, this);
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
        /// <msdn-id>ff476501</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>	
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
            device.CreateBuffer(ref description, new DataBox(data.PositionPointer, 0, 0), this);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="T:SharpDX.Direct3D11.Buffer"/> class.
        /// </summary>
        /// <typeparam name="T">Type of the data to upload</typeparam>
        /// <param name="device">The device with which to associate the buffer.</param>
        /// <param name="bindFlags">Flags specifying how the buffer will be bound to the pipeline.</param>
        /// <param name="data">Initial data used to initialize the buffer.</param>
        /// <param name="sizeInBytes">The size, in bytes, of the buffer. If 0 is specified, sizeof(T) is used. </param>
        /// <param name="usage">The usage pattern for the buffer.</param>
        /// <param name="accessFlags">Flags specifying how the buffer will be accessible from the CPU.</param>
        /// <param name="optionFlags">Miscellaneous resource options.</param>
        /// <param name="structureByteStride">The size (in bytes) of the structure element for structured buffers.</param>
        /// <returns>An initialized buffer</returns>
        /// <msdn-id>ff476501</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>	
        public static Buffer Create<T>(
            Device device,
            BindFlags bindFlags,
            ref T data,
            int sizeInBytes = 0,
            ResourceUsage usage = ResourceUsage.Default,
            CpuAccessFlags accessFlags = CpuAccessFlags.None,
            ResourceOptionFlags optionFlags = ResourceOptionFlags.None,
            int structureByteStride = 0) 
            where T : struct
        {
            var buffer = new Buffer(IntPtr.Zero);

            var description = new BufferDescription()
            {
                BindFlags = bindFlags,
                CpuAccessFlags = accessFlags,
                OptionFlags = optionFlags,
                SizeInBytes = sizeInBytes == 0 ? Utilities.SizeOf<T>() : sizeInBytes,
                Usage = usage,
                StructureByteStride = structureByteStride
            };

            unsafe
            {
                device.CreateBuffer(ref description, new DataBox((IntPtr)Interop.Fixed(ref data)), buffer);
            }
            return buffer;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="T:SharpDX.Direct3D11.Buffer"/> class.
        /// </summary>
        /// <typeparam name="T">Type of the data to upload</typeparam>
        /// <param name="device">The device with which to associate the buffer.</param>
        /// <param name="bindFlags">Flags specifying how the buffer will be bound to the pipeline.</param>
        /// <param name="data">Initial data used to initialize the buffer.</param>
        /// <param name="sizeInBytes">The size, in bytes, of the buffer. If 0 is specified, sizeof(T) * data.Length is used.</param>
        /// <param name="usage">The usage pattern for the buffer.</param>
        /// <param name="accessFlags">Flags specifying how the buffer will be accessible from the CPU.</param>
        /// <param name="optionFlags">Miscellaneous resource options.</param>
        /// <param name="structureByteStride">The size (in bytes) of the structure element for structured buffers.</param>
        /// <returns>An initialized buffer</returns>
        /// <msdn-id>ff476501</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>	
        public static Buffer Create<T>(Device device, BindFlags bindFlags, T[] data, int sizeInBytes = 0, ResourceUsage usage = ResourceUsage.Default, CpuAccessFlags accessFlags = CpuAccessFlags.None, ResourceOptionFlags optionFlags = ResourceOptionFlags.None, int structureByteStride = 0) where T : struct 
        {
            var buffer = new Buffer(IntPtr.Zero);

            var description = new BufferDescription()
            {
                BindFlags = bindFlags,
                CpuAccessFlags = accessFlags,
                OptionFlags = optionFlags,
                SizeInBytes = sizeInBytes == 0 ? Utilities.SizeOf<T>() * data.Length : sizeInBytes,
                Usage = usage,
                StructureByteStride = structureByteStride
            };

            unsafe
            {
                device.CreateBuffer(ref description, new DataBox((IntPtr)Interop.Fixed(data)), buffer);
            }
            return buffer;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="T:SharpDX.Direct3D11.Buffer"/> class.
        /// </summary>
        /// <typeparam name="T">Type of the data to upload</typeparam>
        /// <param name="device">The device with which to associate the buffer.</param>
        /// <param name="data">Initial data used to initialize the buffer.</param>
        /// <param name="description">The description.</param>
        /// <returns>
        /// An initialized buffer
        /// </returns>
        /// <remarks>
        /// If the <see cref="BufferDescription.SizeInBytes"/> is at 0, sizeof(T) is used.
        /// </remarks>
        /// <msdn-id>ff476501</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>	
        public static Buffer Create<T>(Device device, ref T data, BufferDescription description) where T : struct
        {
            var buffer = new Buffer(IntPtr.Zero);
            unsafe
            {
                if (description.SizeInBytes == 0)
                    description.SizeInBytes = Utilities.SizeOf<T>();

                device.CreateBuffer(ref description, new DataBox((IntPtr)Interop.Fixed(ref data)), buffer);
            }
            return buffer;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="T:SharpDX.Direct3D11.Buffer"/> class.
        /// </summary>
        /// <typeparam name="T">Type of the data to upload</typeparam>
        /// <param name="device">The device with which to associate the buffer.</param>
        /// <param name="data">Initial data used to initialize the buffer.</param>
        /// <param name="description">The description.</param>
        /// <returns>
        /// An initialized buffer
        /// </returns>
        /// <remarks>
        /// If the <see cref="BufferDescription.SizeInBytes"/> is at 0, sizeof(T) * data.Length is used.
        /// </remarks>
        /// <msdn-id>ff476501</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>	
        public static Buffer Create<T>(Device device, T[] data, BufferDescription description) where T : struct
        {
            var buffer = new Buffer(IntPtr.Zero);
            unsafe
            {
                if (description.SizeInBytes == 0)
                    description.SizeInBytes = Utilities.SizeOf<T>() * data.Length;
                device.CreateBuffer(ref description, new DataBox((IntPtr)Interop.Fixed(data)), buffer);
            }
            return buffer;
        }
    }
}