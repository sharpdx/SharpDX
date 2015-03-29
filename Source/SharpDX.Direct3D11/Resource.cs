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
using System.IO;
using System.Runtime.InteropServices;
using SharpDX.Direct3D;
using SharpDX.DXGI;

namespace SharpDX.Direct3D11
{
    public partial class Resource
    {
        /// <summary>
        ///   Gets a swap chain back buffer.
        /// </summary>
        /// <typeparam name = "T">The type of the buffer.</typeparam>
        /// <param name = "swapChain">The swap chain to get the buffer from.</param>
        /// <param name = "index">The index of the desired buffer.</param>
        /// <returns>The buffer interface, or <c>null</c> on failure.</returns>
        public static T FromSwapChain<T>(SwapChain swapChain, int index) where T : Resource
        {
            return swapChain.GetBackBuffer<T>(index);
        }

        /// <summary>
        /// Calculates the sub resource index from a miplevel.
        /// </summary>
        /// <param name="mipSlice">A zero-based index for the mipmap level to address; 0 indicates the first, most detailed mipmap level.</param>
        /// <param name="arraySlice">The zero-based index for the array level to address; always use 0 for volume (3D) textures.</param>
        /// <param name="mipLevel">Number of mipmap levels in the resource.</param>
        /// <returns>
        /// The index which equals MipSlice + (ArraySlice * MipLevels).
        /// </returns>
        /// <unmanaged>D3D11CalcSubresource</unmanaged>
        public static int CalculateSubResourceIndex(int mipSlice, int arraySlice, int mipLevel)
        {
            return (mipLevel * arraySlice) + mipSlice;
        }

        /// <summary>
        /// Calculates the resulting size at a single level for an original size.
        /// </summary>
        /// <param name="mipLevel">The mip level to get the size.</param>
        /// <param name="baseSize">Size of the base.</param>
        /// <returns>
        /// Size of the mipLevel
        /// </returns>
        public static int CalculateMipSize(int mipLevel, int baseSize)
        {
            baseSize = baseSize >> mipLevel;
            return baseSize > 0 ? baseSize : 1;
        }

        /// <summary>
        /// Calculates the sub resource index for a particular mipSlice and arraySlice.
        /// </summary>
        /// <param name="mipSlice">The mip slice.</param>
        /// <param name="arraySlice">The array slice.</param>
        /// <param name="mipSize">The size of slice. This values is resource dependent. Texture1D -> mipSize of the Width. Texture2D -> mipSize of the Height. Texture3D -> mipsize of the Depth</param>
        /// <returns>The resulting miplevel calulated for this instance with this mipSlice and arraySlice.</returns>
        public virtual int CalculateSubResourceIndex(int mipSlice, int arraySlice, out int mipSize)
        {
            throw new NotImplementedException("This method is not implemented for this kind of resource");
        }
    }
}