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
using System.Diagnostics;
using SharpDX.DXGI;


namespace SharpDX.Direct3D10
{
    public partial class Texture1D
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D10.Texture1D" /> class.
        /// </summary>
        /// <param name = "device">The device with which to associate the texture.</param>
        /// <param name = "description">The description of the texture.</param>
        public Texture1D(Device device, Texture1DDescription description)
            : base(IntPtr.Zero)
        {
            device.CreateTexture1D(ref description, null, this);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D10.Texture1D" /> class.
        /// </summary>
        /// <param name = "device">The device with which to associate the texture.</param>
        /// <param name = "description">The description of the texture.</param>
        /// <param name = "data">The initial texture data.</param>
        public Texture1D(Device device, Texture1DDescription description, DataStream data)
            : this(device, description, new[] {data})
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D10.Texture1D" /> class.
        /// </summary>
        /// <param name = "device">The device with which to associate the texture.</param>
        /// <param name = "description">The description of the texture.</param>
        /// <param name = "data">An array of initial texture data for each subresource.</param>
        public Texture1D(Device device, Texture1DDescription description, DataStream[] data)
            : base(IntPtr.Zero)
        {
            var subResourceDatas = new DataBox[data.Length];
            for (int i = 0; i < subResourceDatas.Length; i++)
                subResourceDatas[i].DataPointer = data[i].DataPointer;

            device.CreateTexture1D(ref description, subResourceDatas, this);
        }

        /// <summary>
        /// Maps the texture, providing CPU access to its contents.
        /// </summary>
        /// <param name="mipSlice">The mip slice to map.</param>
        /// <param name="mode">The IO operations to enable on the CPU.</param>
        /// <param name="flags">Flags indicating how the CPU should respond when the GPU is busy.</param>
        /// <returns>A data stream containing the mapped data. This data stream is invalidated
        /// when the buffer is unmapped.</returns>
        public DataStream Map(int mipSlice, MapMode mode, MapFlags flags)
        {
            var desc = Description;
		    int subresource = CalculateSubResourceIndex( mipSlice, 0, desc.MipLevels );
		    int mipWidth = GetMipSize( mipSlice, desc.Width );
		    int bufferSize = (int)(mipWidth * FormatHelper.SizeOfInBytes(desc.Format));

            IntPtr dataPointer;
            Map(subresource, mode, flags, out dataPointer);

            bool canRead = mode == MapMode.Read || mode == MapMode.ReadWrite;
            bool canWrite = mode != MapMode.Read;

            return new DataStream(dataPointer, bufferSize, canRead, canWrite);
        }
    }
}