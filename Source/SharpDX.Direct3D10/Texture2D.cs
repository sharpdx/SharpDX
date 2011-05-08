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
using SharpDX.DXGI;

namespace SharpDX.Direct3D10
{
    public partial class Texture2D
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D10.Texture2D" /> class.
        /// </summary>
        /// <param name = "device">The device with which to associate the texture.</param>
        /// <param name = "description">The description of the texture.</param>
        public Texture2D(Device device, Texture2DDescription description)
            : base(IntPtr.Zero)
        {
            Texture2D temp;
            device.CreateTexture2D(ref description, null, out temp);
            NativePointer = temp.NativePointer;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D10.Texture2D" /> class.
        /// </summary>
        /// <param name = "device">The device with which to associate the texture.</param>
        /// <param name = "description">The description of the texture.</param>
        /// <param name = "data">The initial texture data.</param>
        public Texture2D(Device device, Texture2DDescription description, DataRectangle data)
            : this(device, description, new[] {data})
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D10.Texture2D" /> class.
        /// </summary>
        /// <param name = "device">The device with which to associate the texture.</param>
        /// <param name = "description">The description of the texture.</param>
        /// <param name = "data">An array of initial texture data for each subresource.</param>
        public Texture2D(Device device, Texture2DDescription description, DataRectangle[] data)
            : base(IntPtr.Zero)
        {
            Texture2D temp;

            SubResourceData[] subResourceDatas = null;

            if (data != null)
            {
                subResourceDatas = new SubResourceData[data.Length];
                for (int i = 0; i < subResourceDatas.Length; i++)
                {
                    subResourceDatas[i].DataPointer = data[i].Data.DataPointer;
                    subResourceDatas[i].Pitch = data[i].Pitch;
                }
            }

            device.CreateTexture2D(ref description, subResourceDatas, out temp);
            NativePointer = temp.NativePointer;
        }

        /// <summary>
        ///   Converts a height map into a normal map. The (x,y,z) components of each normal are mapped to the (r,g,b) channels of the output texture.
        /// </summary>
        /// <param name = "source">The source height map texture.</param>
        /// <param name = "destination">The destination texture.</param>
        /// <param name = "flags">One or more flags that control generation of normal maps.</param>
        /// <param name = "channel">One or more flag specifying the source of height information.</param>
        /// <param name = "amplitude">Constant value multiplier that increases (or decreases) the values in the normal map. Higher values usually make bumps more visible, lower values usually make bumps less visible.</param>
        /// <returns>A <see cref = "T:SharpDX.Result" /> object describing the result of the operation.</returns>
        public static Result ComputeNormalMap(Texture2D source, Texture2D destination,
                                              NormalMapFlags flags, Channel channel, float amplitude)
        {
            return D3DX10.ComputeNormalMap(source, flags, channel, amplitude, destination);
        }

        /// <summary>
        /// Maps the texture, providing CPU access to its contents.
        /// </summary>
        /// <param name="mipSlice">The mip slice to map.</param>
        /// <param name="mode">The IO operations to enable on the CPU.</param>
        /// <param name="flags">Flags indicating how the CPU should respond when the GPU is busy.</param>
        /// <returns>A data rectangle containing the mapped data. This data stream is invalidated when the buffer is unmapped.</returns>
        public DataRectangle Map(int mipSlice, MapMode mode, MapFlags flags)
        {
            var desc = Description;
            int subresource = CalculateSubresourceIndex(mipSlice, 0, desc.MipLevels);
            int mipHeight = GetMipSize(mipSlice, desc.Height);

            MappedTexture2D mappedTexture2D;
            Map(subresource, mode, flags, out mappedTexture2D);

            bool canRead = mode == MapMode.Read || mode == MapMode.ReadWrite;
            bool canWrite = mode != MapMode.Read;

            return new DataRectangle(mappedTexture2D.RowPitch, new DataStream(mappedTexture2D.PData, mipHeight * mappedTexture2D.RowPitch, canRead, canWrite));
        }
    }
}