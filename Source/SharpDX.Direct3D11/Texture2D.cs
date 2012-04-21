// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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
    public partial class Texture2D
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D11.Texture2D" /> class.
        /// </summary>
        /// <param name = "device">The device with which to associate the texture.</param>
        /// <param name = "description">The description of the texture.</param>
        public Texture2D(Device device, Texture2DDescription description)
            : base(IntPtr.Zero)
        {
            device.CreateTexture2D(ref description, null, this);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D11.Texture2D" /> class.
        /// </summary>
        /// <param name = "device">The device with which to associate the texture.</param>
        /// <param name = "description">The description of the texture.</param>
        /// <param name = "data">An array of initial texture data for each subresource.</param>
        public Texture2D(Device device, Texture2DDescription description, params DataRectangle[] data)
            : base(IntPtr.Zero)
        {
            DataBox[] subResourceDatas = null;

            if (data != null && data.Length > 0)
            {
                subResourceDatas = new DataBox[data.Length];
                for (int i = 0; i < subResourceDatas.Length; i++)
                {
                    subResourceDatas[i].DataPointer = data[i].DataPointer;
                    subResourceDatas[i].RowPitch = data[i].Pitch;
                }
            }

            device.CreateTexture2D(ref description, subResourceDatas, this);
        }

        /// <inheritdoc/>
        public override int CalculateSubResourceIndex(int mipSlice, int arraySlice, out int mipSize)
        {
            var desc = Description;
            mipSize = CalculateMipSize(mipSlice, desc.Height);
            return CalculateSubResourceIndex(mipSlice, arraySlice, desc.MipLevels);
        }

#if !DIRECTX11_1
        /// <summary>
        ///   Converts a height map into a normal map. The (x,y,z) components of each normal are mapped to the (r,g,b) channels of the output texture.
        /// </summary>
        /// <param name = "context">The device used to create the normal map.</param>
        /// <param name = "source">The source height map texture.</param>
        /// <param name = "destination">The destination texture.</param>
        /// <param name = "flags">One or more flags that control generation of normal maps.</param>
        /// <param name = "channel">One or more flag specifying the source of height information.</param>
        /// <param name = "amplitude">Constant value multiplier that increases (or decreases) the values in the normal map. Higher values usually make bumps more visible, lower values usually make bumps less visible.</param>
        /// <returns>A <see cref = "T:SharpDX.Result" /> object describing the result of the operation.</returns>
        public static Result ComputeNormalMap(DeviceContext context, Texture2D source, Texture2D destination,
                                              NormalMapFlags flags, Channel channel, float amplitude)
        {
            return D3DX11.ComputeNormalMap(context, source, flags, channel, amplitude, destination);
        }

        /// <summary>	
        /// Projects a function represented in a cube map into spherical harmonics.	
        /// </summary>	
        /// <param name="context"><para>A reference to an <see cref="SharpDX.Direct3D11.DeviceContext"/> object.</para></param>	
        /// <param name="cubeMap"><para>A reference to an <see cref="SharpDX.Direct3D11.Texture2D"/> that represents a cubemap that is going to be projected into spherical harmonics.</para></param>	
        /// <param name="order"><para>Order of the SH evaluation, generates Order^2 coefficients whose degree is Order-1. Valid range is between 2 and 6.</para></param>	
        /// <returns>An array of SH Vector for red, green and blue components with a length Order^2.</returns>	
        /// <unmanaged>HRESULT D3DX11SHProjectCubeMap([In] ID3D11DeviceContext* pContext,[In] unsigned int Order,[In] ID3D11Texture2D* pCubeMap,[Out, Buffer] float* pROut,[Out, Buffer, Optional] float* pGOut,[Out, Buffer, Optional] float* pBOut)</unmanaged>	
        public static Color3[] SHProjectCubeMap(DeviceContext context, Texture2D cubeMap, int order)
        {
            if (order < 2 || order > 6)
                throw new ArgumentException("Invalid range for SH order. Must be in the range [2,6]");

            int length = order * order;
            var redSH = new float[length];
            var greenSH = new float[length];
            var blueSH = new float[length];

            D3DX11.SHProjectCubeMap(context, order, cubeMap, redSH, greenSH, blueSH);
            var result = new Color3[length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i].Red = redSH[i];
                result[i].Green = greenSH[i];
                result[i].Blue = blueSH[i];
            }
            return result;
        }
#endif
  }
}