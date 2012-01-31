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

#if !WIN8
        /// <summary>
        ///   Loads a texture from an image file.
        /// </summary>
        /// <param name = "device">The device used to load the texture.</param>
        /// <param name = "fileName">Path to the file on disk.</param>
        /// <returns>The loaded texture object.</returns>
        public static T FromFile<T>(Device device, string fileName) where T : Resource
        {
            System.Diagnostics.Debug.Assert(typeof(T) == typeof(Texture1D) || typeof(T) == typeof(Texture2D) ||
                         typeof(T) == typeof(Texture3D));

            IntPtr temp;
            Result resultOut;
            D3DX11.CreateTextureFromFile(device, fileName, null, IntPtr.Zero, out temp, out resultOut);
            // TODO test resultOut?
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
            System.Diagnostics.Debug.Assert(typeof(T) == typeof(Texture1D) || typeof(T) == typeof(Texture2D) ||
                         typeof(T) == typeof(Texture3D));

            IntPtr temp;
            Result resultOut;
            D3DX11.CreateTextureFromFile(device, fileName, loadInfo, IntPtr.Zero, out temp, out resultOut);
            return FromPointer<T>(temp);
        }

        /// <summary>	
        /// Load a texture from a texture.	
        /// </summary>
        /// <param name="context">A reference to a valid <see cref="DeviceContext"/></param>
        /// <param name="source">Pointer to the source texture. See <see cref="SharpDX.Direct3D11.Resource"/>. </param>
        /// <param name="destination">Pointer to the destination texture. See <see cref="SharpDX.Direct3D11.Resource"/>. </param>
        /// <param name="loadInformation">Pointer to texture loading parameters. See <see cref="SharpDX.Direct3D11.TextureLoadInformation"/>. </param>
        /// <returns>The return value is one of the values listed in {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT D3DX10LoadTextureFromTexture([None] ID3D10Resource* pSrcTexture,[None] D3DX10_TEXTURE_LOAD_INFO* pLoadInfo,[None] ID3D10Resource* pDstTexture)</unmanaged>
        public static Result LoadTextureFromTexture(DeviceContext context, Resource source, Resource destination, TextureLoadInformation loadInformation)
        {
            return D3DX11.LoadTextureFromTexture(context, source, loadInformation, destination);
        }

        /// <summary>	
        /// Generates mipmap chain using a particular texture filter for this texture instance.
        /// </summary>	
        /// <param name="deviceContext"><para>A reference to an <see cref="SharpDX.Direct3D11.DeviceContext"/> object.</para></param>	
        /// <param name="sourceLevel"><para>The mipmap level whose data is used to generate the rest of the mipmap chain.</para></param>	
        /// <param name="mipFilter"><para>Flags controlling how each miplevel is filtered (or D3DX11_DEFAULT for <see cref="SharpDX.Direct3D11.FilterFlags.Linear"/>). See <see cref="SharpDX.Direct3D11.FilterFlags"/>.</para></param>	
        /// <returns>The return value is one of the values listed in Direct3D 11 Return Codes.</returns>	
        /// <unmanaged>HRESULT D3DX11FilterTexture([In] ID3D11DeviceContext* pContext,[In] ID3D11Resource* pTexture,[In] unsigned int SrcLevel,[In] unsigned int MipFilter)</unmanaged>	
        public Result FilterTexture(DeviceContext deviceContext, int sourceLevel, FilterFlags mipFilter)
        {
            return D3DX11.FilterTexture(deviceContext, this, sourceLevel, (int)mipFilter);
        }

        /// <summary>
        ///   Saves a texture to file.
        /// </summary>
        /// <param name = "context">The device used to save the texture.</param>
        /// <param name = "texture">The texture to save.</param>
        /// <param name = "format">The format the texture will be saved as.</param>
        /// <param name = "fileName">Name of the destination output file where the texture will be saved.</param>
        /// <returns>A <see cref = "T:SharpDX.Result" /> object describing the result of the operation.</returns>
        public static Result ToFile<T>(DeviceContext context, T texture, ImageFileFormat format, string fileName)
            where T : Resource
        {
            System.Diagnostics.Debug.Assert(typeof(T) == typeof(Texture1D) || typeof(T) == typeof(Texture2D) ||
                         typeof(T) == typeof(Texture3D));

            return D3DX11.SaveTextureToFile(context, texture, format, fileName);
        }

        /// <summary>
        ///   Loads a texture from an image in memory.
        /// </summary>
        /// <param name = "device">The device used to load the texture.</param>
        /// <param name = "memory">Array of memory containing the image data to load.</param>
        /// <returns>The loaded texture object.</returns>
        public static T FromMemory<T>(Device device, byte[] memory) where T : Resource
        {
            System.Diagnostics.Debug.Assert(typeof(T) == typeof(Texture1D) || typeof(T) == typeof(Texture2D) ||
                         typeof(T) == typeof(Texture3D));

            unsafe
            {
                System.Diagnostics.Debug.Assert(memory != null);
                System.Diagnostics.Debug.Assert(memory.Length > 0);
                IntPtr temp;
                Result resultOut;
                fixed (void* pBuffer = &memory[0])
                    D3DX11.CreateTextureFromMemory(device, (IntPtr)pBuffer, memory.Length, null, IntPtr.Zero,
                                                   out temp, out resultOut);
                // TODO test resultOut?
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
            System.Diagnostics.Debug.Assert(typeof(T) == typeof(Texture1D) || typeof(T) == typeof(Texture2D) ||
                         typeof(T) == typeof(Texture3D));

            unsafe
            {
                System.Diagnostics.Debug.Assert(memory != null);
                System.Diagnostics.Debug.Assert(memory.Length > 0);
                IntPtr temp;
                Result resultOut;
                fixed (void* pBuffer = &memory[0])
                    D3DX11.CreateTextureFromMemory(device, (IntPtr)pBuffer, memory.Length, loadInfo, IntPtr.Zero,
                                                   out temp, out resultOut);
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
        ///   Saves a texture to a stream.
        /// </summary>
        /// <param name = "context">The device used to save the texture.</param>
        /// <param name = "texture">The texture to save.</param>
        /// <param name = "format">The format the texture will be saved as.</param>
        /// <param name = "stream">Destination memory stream where the image will be saved.</param>
        /// <returns>A <see cref = "T:SharpDX.Result" /> object describing the result of the operation.</returns>
        public static Result ToStream<T>(DeviceContext context, T texture, ImageFileFormat format, Stream stream)
            where T : Resource
        {
            System.Diagnostics.Debug.Assert(typeof (T) == typeof (Texture1D) || typeof (T) == typeof (Texture2D) ||
                         typeof (T) == typeof (Texture3D));

            Blob blob;
            Result result = D3DX11.SaveTextureToMemory(context, texture, format, out blob, 0);

            IntPtr bufferPtr = blob.BufferPointer;
            int blobSize = blob.BufferSize;

            // Write byte-by-byte to avoid allocating a managed byte[] that will wastefully persist.
            for (int byteIndex = 0; byteIndex < blobSize; ++byteIndex)
                stream.WriteByte(Marshal.ReadByte(bufferPtr, byteIndex));

            blob.Dispose();

            return result;
        }
#endif

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