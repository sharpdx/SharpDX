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
using SharpDX.DXGI;

namespace SharpDX.Direct3D10
{
    public partial class Texture3D
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D10.Texture3D" /> class.
        /// </summary>
        /// <param name = "device">The device with which to associate the texture.</param>
        /// <param name = "description">The description of the texture.</param>
        public Texture3D(Device device, Texture3DDescription description)
            : base(IntPtr.Zero)
        {
            device.CreateTexture3D(ref description, null, this);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D10.Texture3D" /> class.
        /// </summary>
        /// <param name = "device">The device with which to associate the texture.</param>
        /// <param name = "description">The description of the texture.</param>
        /// <param name = "data">The initial texture data.</param>
        public Texture3D(Device device, Texture3DDescription description, DataBox data)
            : this(device, description, new[] {data})
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D10.Texture3D" /> class.
        /// </summary>
        /// <param name = "device">The device with which to associate the texture.</param>
        /// <param name = "description">The description of the texture.</param>
        /// <param name = "data">An array of initial texture data for each subresource.</param>
        public Texture3D(Device device, Texture3DDescription description, DataBox[] data) : base(IntPtr.Zero)
        {
            device.CreateTexture3D(ref description, data, this);
        }

        /// <summary>
        /// Maps the texture, providing CPU access to its contents.
        /// </summary>
        /// <param name="mipSlice">The mip slice to map.</param>
        /// <param name="mode">The IO operations to enable on the CPU.</param>
        /// <param name="flags">Flags indicating how the CPU should respond when the GPU is busy.</param>
        /// <returns>
        /// A databox containing the mapped data. This data stream is invalidated when the buffer is unmapped.
        /// </returns>
        public DataBox Map(int mipSlice, MapMode mode, MapFlags flags)
        {
            var desc = Description;
            int subresource = CalculateSubResourceIndex(mipSlice, 0, desc.MipLevels);

            DataBox mappedTexture3D;
            Map(subresource, mode, flags, out mappedTexture3D);

            return mappedTexture3D;
        }

        /// <summary>
        /// Maps the texture, providing CPU access to its contents.
        /// </summary>
        /// <param name="mipSlice">The mip slice to map.</param>
        /// <param name="mode">The IO operations to enable on the CPU.</param>
        /// <param name="flags">Flags indicating how the CPU should respond when the GPU is busy.</param>
        /// <param name="dataStream">The data stream.</param>
        /// <returns>
        /// A databox containing the mapped data. This data stream is invalidated when the buffer is unmapped.
        /// </returns>
        public DataBox Map(int mipSlice, MapMode mode, MapFlags flags, out DataStream dataStream)
        {
            var desc = Description;
            int subresource = CalculateSubResourceIndex(mipSlice, 0, desc.MipLevels);

            DataBox mappedTexture3D;
            Map(subresource, mode, flags, out mappedTexture3D);

            bool canRead = mode == MapMode.Read || mode == MapMode.ReadWrite;
            bool canWrite = mode != MapMode.Read;
            int mipDepth = GetMipSize(mipSlice, desc.Depth);
            dataStream = new DataStream(mappedTexture3D.DataPointer, mipDepth * mappedTexture3D.SlicePitch, canRead, canWrite);

            return mappedTexture3D;
        }
    }
}