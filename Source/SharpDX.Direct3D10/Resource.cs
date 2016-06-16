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
using System.IO;
using System.Runtime.InteropServices;
using SharpDX.Direct3D;
using SharpDX.DXGI;

namespace SharpDX.Direct3D10
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
        ///   Loads a texture from an image file.
        /// </summary>
        /// <param name = "device">The device used to load the texture.</param>
        /// <param name = "fileName">Path to the file on disk.</param>
        /// <returns>The loaded texture object.</returns>
        public static T FromFile<T>(Device device, string fileName) where T : Resource
        {
            IntPtr temp;
            Result resultOut;
            D3DX10.CreateTextureFromFile(device, fileName, null, IntPtr.Zero, out temp, out resultOut);
            // TODO Test resultOut
            return FromPointer<T>(temp);
        }

        /// <summary>
        ///   Loads a texture from an image file.
        /// </summary>
        /// <param name = "device">The device used to load the texture.</param>
        /// <param name = "fileName">Path to the file on disk.</param>
        /// <param name = "loadInfo">Specifies information used to load the texture.</param>
        /// <returns>The loaded texture object.</returns>
        public static T FromFile<T>(Device device, string fileName, ImageLoadInformation loadInfo) where T : Resource
        {
            IntPtr temp;
            Result resultOut;
            D3DX10.CreateTextureFromFile(device, fileName, loadInfo, IntPtr.Zero, out temp, out resultOut);
            // TODO test resultOut?
            return FromPointer<T>(temp);
        }

        /// <summary>
        ///   Loads a texture from an image in memory.
        /// </summary>
        /// <param name = "device">The device used to load the texture.</param>
        /// <param name = "memory">Array of memory containing the image data to load.</param>
        /// <returns>The loaded texture object.</returns>
        public static T FromMemory<T>(Device device, byte[] memory) where T : Resource
        {
            unsafe
            {
                IntPtr temp;
                Result resultOut;
                fixed (void* pBuffer = &memory[0])
                    D3DX10.CreateTextureFromMemory(device, (IntPtr)pBuffer, memory.Length, null, IntPtr.Zero,
                                                   out temp, out resultOut);
                // TODO test resultOut
                return FromPointer<T>(temp);
            }
        }

        /// <summary>
        ///   Loads a texture from an image in memory.
        /// </summary>
        /// <param name = "device">The device used to load the texture.</param>
        /// <param name = "memory">Array of memory containing the image data to load.</param>
        /// <param name = "loadInfo">Specifies information used to load the texture.</param>
        /// <returns>The loaded texture object.</returns>
        public static T FromMemory<T>(Device device, byte[] memory, ImageLoadInformation loadInfo) where T : Resource
        {
            unsafe
            {
                IntPtr temp;
                Result resultOut;
                fixed (void* pBuffer = &memory[0])
                    D3DX10.CreateTextureFromMemory(device, (IntPtr)pBuffer, memory.Length, loadInfo, IntPtr.Zero,
                                                   out temp, out resultOut);
                // TODO test resultOut?
                return FromPointer<T>(temp);
            }
        }

        /// <summary>
        ///   Loads a texture from a stream of data.
        /// </summary>
        /// <param name = "device">The device used to load the texture.</param>
        /// <param name = "stream">A stream containing the image data to load.</param>
        /// <param name = "sizeInBytes">Size of the image to load.</param>
        /// <returns>The loaded texture object.</returns>
        public static T FromStream<T>(Device device, Stream stream, int sizeInBytes) where T : Resource
        {
            byte[] buffer = Utilities.ReadStream(stream, ref sizeInBytes);
            return FromMemory<T>(device, buffer);
        }

        /// <summary>
        ///   Loads a texture from a stream of data.
        /// </summary>
        /// <param name = "device">The device used to load the texture.</param>
        /// <param name = "stream">A stream containing the image data to load.</param>
        /// <param name = "sizeInBytes">Size of the image to load.</param>
        /// <param name = "loadInfo">Specifies information used to load the texture.</param>
        /// <returns>The loaded texture object.</returns>
        public static T FromStream<T>(Device device, Stream stream, int sizeInBytes, ImageLoadInformation loadInfo)
            where T : Resource
        {
            byte[] buffer = Utilities.ReadStream(stream, ref sizeInBytes);
            return FromMemory<T>(device, buffer, loadInfo);
        }

        /// <summary>
        ///   Saves a texture to file.
        /// </summary>
        /// <param name = "texture">The texture to save.</param>
        /// <param name = "format">The format the texture will be saved as.</param>
        /// <param name = "fileName">Name of the destination output file where the texture will be saved.</param>
        /// <returns>A <see cref = "T:SharpDX.Result" /> object describing the result of the operation.</returns>
        public static void ToFile<T>(T texture, ImageFileFormat format, string fileName)
            where T : Resource
        {
            D3DX10.SaveTextureToFile(texture, format, fileName);
        }

        /// <summary>
        ///   Saves a texture to a stream.
        /// </summary>
        /// <param name = "texture">The texture to save.</param>
        /// <param name = "format">The format the texture will be saved as.</param>
        /// <param name = "stream">Destination memory stream where the image will be saved.</param>
        /// <returns>A <see cref = "T:SharpDX.Result" /> object describing the result of the operation.</returns>
        public static void ToStream<T>(T texture, ImageFileFormat format, Stream stream)
            where T : Resource
        {
            Blob blob;
            D3DX10.SaveTextureToMemory(texture, format, out blob, 0);

            IntPtr bufferPtr = blob.BufferPointer;
            int blobSize = blob.BufferSize;

            // Write byte-by-byte to avoid allocating a managed byte[] that will wastefully persist.
            for (int byteIndex = 0; byteIndex < blobSize; ++byteIndex)
                stream.WriteByte(Marshal.ReadByte(bufferPtr, byteIndex));

            blob.Dispose();
        }


        /// <summary>	
        /// Load a texture from a texture.	
        /// </summary>	
        /// <param name="source">Pointer to the source texture. See <see cref="SharpDX.Direct3D10.Resource"/>. </param>
        /// <param name="destination">Pointer to the destination texture. See <see cref="SharpDX.Direct3D10.Resource"/>. </param>
        /// <param name="loadInformation">Pointer to texture loading parameters. See <see cref="SharpDX.Direct3D10.TextureLoadInformation"/>. </param>
        /// <returns>The return value is one of the values listed in {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT D3DX10LoadTextureFromTexture([None] ID3D10Resource* pSrcTexture,[None] D3DX10_TEXTURE_LOAD_INFO* pLoadInfo,[None] ID3D10Resource* pDstTexture)</unmanaged>
        public static void LoadTextureFromTexture(Resource source, Resource destination, TextureLoadInformation loadInformation)
        {
            D3DX10.LoadTextureFromTexture(source, loadInformation, destination);
        }

        /// <summary>	
        /// Generates mipmap chain using a particular texture filter.	
        /// </summary>	
        /// <param name="sourceLevel">The mipmap level whose data is used to generate the rest of the mipmap chain. </param>
        /// <param name="mipFilter">Flags controlling how each miplevel is filtered (or D3DX10_DEFAULT for D3DX10_FILTER_BOX). See <see cref="SharpDX.Direct3D10.FilterFlags"/>. </param>
        /// <returns>The return value is one of the values listed in {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT D3DX10FilterTexture([None] ID3D10Resource* pTexture,[None] int SrcLevel,[None] int MipFilter)</unmanaged>
        public void FilterTexture(int sourceLevel, FilterFlags mipFilter)
        {
            D3DX10.FilterTexture(this, sourceLevel, (int) mipFilter);
        }

        /// <summary>
        /// Returns a DXGI Surface for this resource.
        /// </summary>
        /// <returns>The buffer interface, or <c>null</c> on failure.</returns>
        public Surface AsSurface()
        {
            return QueryInterface<Surface>();
        }

        /// <summary>
        /// Calculates a subresource index.
        /// </summary>
        /// <param name="mipSlice">The index of the desired mip slice.</param>
        /// <param name="arraySlice">The index of the desired array slice.</param>
        /// <param name="mipLevels">The total number of mip levels.</param>
        /// <returns>The subresource index (equivalent to mipSlice + (arraySlice * mipLevels)).</returns>
        public static int CalculateSubResourceIndex(int mipSlice, int arraySlice, int mipLevels)
        {
            return ((arraySlice * mipLevels) + mipSlice);
        }

        /// <summary>
        /// Calculate the MipSize
        /// </summary>
        /// <param name="mipSlice"></param>
        /// <param name="baseSliceSize"></param>
        /// <returns></returns>
        internal static int GetMipSize(int mipSlice, int baseSliceSize)
        {
            float size = baseSliceSize;
            if (mipSlice > 0)
            {
                do
                {
                    size = (float)Math.Floor(size * 0.5f);
                    mipSlice -= 1;
                }
                while (mipSlice > 0);
            }
            return (int)size;
        }
    }
}