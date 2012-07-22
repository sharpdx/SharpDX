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
// -----------------------------------------------------------------------------
// Part of te following code is a port of http://directxtex.codeplex.com
// -----------------------------------------------------------------------------
// Microsoft Public License (Ms-PL)
//
// This license governs use of the accompanying software. If you use the 
// software, you accept this license. If you do not accept the license, do not
// use the software.
//
// 1. Definitions
// The terms "reproduce," "reproduction," "derivative works," and 
// "distribution" have the same meaning here as under U.S. copyright law.
// A "contribution" is the original software, or any additions or changes to 
// the software.
// A "contributor" is any person that distributes its contribution under this 
// license.
// "Licensed patents" are a contributor's patent claims that read directly on 
// its contribution.
//
// 2. Grant of Rights
// (A) Copyright Grant- Subject to the terms of this license, including the 
// license conditions and limitations in section 3, each contributor grants 
// you a non-exclusive, worldwide, royalty-free copyright license to reproduce
// its contribution, prepare derivative works of its contribution, and 
// distribute its contribution or any derivative works that you create.
// (B) Patent Grant- Subject to the terms of this license, including the license
// conditions and limitations in section 3, each contributor grants you a 
// non-exclusive, worldwide, royalty-free license under its licensed patents to
// make, have made, use, sell, offer for sale, import, and/or otherwise dispose
// of its contribution in the software or derivative works of the contribution 
// in the software.
//
// 3. Conditions and Limitations
// (A) No Trademark License- This license does not grant you rights to use any 
// contributors' name, logo, or trademarks.
// (B) If you bring a patent claim against any contributor over patents that 
// you claim are infringed by the software, your patent license from such 
// contributor to the software ends automatically.
// (C) If you distribute any portion of the software, you must retain all 
// copyright, patent, trademark, and attribution notices that are present in the
// software.
// (D) If you distribute any portion of the software in source code form, you 
// may do so only under this license by including a complete copy of this 
// license with your distribution. If you distribute any portion of the software
// in compiled or object code form, you may only do so under a license that 
// complies with this license.
// (E) The software is licensed "as-is." You bear the risk of using it. The
// contributors give no express warranties, guarantees or conditions. You may
// have additional consumer rights under your local laws which this license 
// cannot change. To the extent permitted under your local laws, the 
// contributors exclude the implied warranties of merchantability, fitness for a
// particular purpose and non-infringement.
using System;
using System.IO;
using System.Runtime.InteropServices;
using SharpDX.DXGI;
using SharpDX.Direct3D11;
using SharpDX.IO;

namespace SharpDX.Toolkit.Graphics
{
    public class Image : Component
    {
        /// <summary>
        /// Description of this image.
        /// </summary>
        public readonly TextureDescription Description;

        /// <summary>
        /// Gets all pixel buffers.
        /// </summary>
        public readonly PixelBuffer[] PixelBuffers;

        private readonly IntPtr mainPixelBuffer;
        private readonly IntPtr externalDataBuffer;
        private readonly bool releaseExternalBufferOnDispose;
        private readonly bool isAllocatedInternally;

        internal unsafe Image(TextureDescription mdata, Texture.PitchFlags pitchFlags, IntPtr dataBuffer, int offset, bool releaseExternalBufferOnDispose = true)
        {
            if (!FormatHelper.IsValid(mdata.Format) || FormatHelper.IsVideo(mdata.Format))
                throw new InvalidOperationException("Unsupported DXGI Format");

            int mipLevels = mdata.MipLevels;

            switch (mdata.Dimension)
            {
                case TextureDimension.Texture1D:
                    if (mdata.Width <= 0 || mdata.Height != 1 || mdata.Depth != 1 || mdata.ArraySize == 0)
                        throw new InvalidOperationException("Invalid Width/Height/Depth/ArraySize for Image 1D");

                    // Check that miplevels are fine
                    Texture.CalculateMipLevels(mdata.Width, 1, mipLevels);
                    break;

                case TextureDimension.Texture2D:
                case TextureDimension.TextureCube:
                    if (mdata.Width <= 0 || mdata.Height <= 0 || mdata.Depth != 1 || mdata.ArraySize == 0)
                        throw new InvalidOperationException("Invalid Width/Height/Depth/ArraySize for Image 2D");

                    if (mdata.Dimension == TextureDimension.TextureCube)
                        mdata.OptionFlags |= ResourceOptionFlags.TextureCube;


                    if ((mdata.OptionFlags & ResourceOptionFlags.TextureCube) != 0)
                    {
                        if ((mdata.ArraySize % 6) != 0)
                            throw new InvalidOperationException("TextureCube must have an arraysize = 6");
                    }

                    // Check that miplevels are fine
                    Texture.CalculateMipLevels(mdata.Width, mdata.Height, mipLevels);
                    break;

                case TextureDimension.Texture3D:
                    if (mdata.Width <= 0 || mdata.Height <= 0 || mdata.Depth <= 0 || mdata.ArraySize != 1)
                        throw new InvalidOperationException("Invalid Width/Height/Depth/ArraySize for Image 3D");

                    // Check that miplevels are fine
                    Texture.CalculateMipLevels(mdata.Width, mdata.Height, mdata.Depth, mipLevels);
                    break;
            }

            // Calculate mipmaps
            int pixelBufferCount;
            int pixelSizeInBytes;
            CalculateImageArray(mdata, pitchFlags, out pixelBufferCount, out pixelSizeInBytes);

            // Allocate all pixel buffers
            PixelBuffers = new PixelBuffer[pixelBufferCount];

            // Setup all pointers
            this.releaseExternalBufferOnDispose = releaseExternalBufferOnDispose;
            externalDataBuffer = dataBuffer;
            mainPixelBuffer = (IntPtr)((byte*)dataBuffer + offset);

            if (mainPixelBuffer == IntPtr.Zero)
            {
                mainPixelBuffer = Utilities.AllocateMemory(pixelSizeInBytes);
                isAllocatedInternally = true;
            }

            SetupImageArray(mainPixelBuffer, pixelSizeInBytes, mdata, pitchFlags, PixelBuffers);


            Description = mdata;
        }


        protected override void Dispose(bool disposeManagedResources)
        {

            if (releaseExternalBufferOnDispose)
            {
                Marshal.FreeHGlobal(externalDataBuffer);
            } 
            else if (isAllocatedInternally)
            {
                Utilities.FreeMemory(mainPixelBuffer);
            }


            base.Dispose(disposeManagedResources);
        }

        public static Image New(TextureDescription description)
        {
            description.BindFlags = BindFlags.None;
            description.OptionFlags = ResourceOptionFlags.None;
            description.SampleDescription = new SampleDescription(1, 0);
            description.Usage = ResourceUsage.Default;

            return new Image(description, Texture.PitchFlags.None, IntPtr.Zero, 0);
        }

        public static Image New1D(int width, MipMapCount mipMapCount, PixelFormat format, int arraySize = 1)
        {
            return new Image(CreateDescription(TextureDimension.Texture1D, width, 1, 1, mipMapCount, format, arraySize), Texture.PitchFlags.None, IntPtr.Zero, 0);
        }

        public static Image New2D(int width, int height, MipMapCount mipMapCount, PixelFormat format, int arraySize = 1)
        {
            return new Image(CreateDescription(TextureDimension.Texture2D, width, height, 1, mipMapCount, format, arraySize), Texture.PitchFlags.None, IntPtr.Zero, 0);
        }

        public static Image NewCube(int width, MipMapCount mipMapCount, PixelFormat format)
        {
            return new Image(CreateDescription(TextureDimension.TextureCube, width, width, 1, mipMapCount, format, 6), Texture.PitchFlags.None, IntPtr.Zero, 0);
        }

        public static Image New3D(int width, int height, int depth, MipMapCount mipMapCount, PixelFormat format)
        {
            return new Image(CreateDescription(TextureDimension.Texture3D, width, width, depth, mipMapCount, format, 1), Texture.PitchFlags.None, IntPtr.Zero, 0);
        }

        public static Image Load(DataPointer dataBuffer, bool makeACopy = false)
        {
            return Load(dataBuffer.Pointer, dataBuffer.Size, makeACopy);
        }

        public static Image Load(IntPtr memory, int size, bool makeACopy = false)
        {
            return DDSHelper.LoadFromDDSMemory(memory, size, makeACopy ? DDSFlags.CopyMemory : DDSFlags.None);
        }

        public unsafe static Image Load(Stream imageStream)
        {
            // Read the whole stream into memory.
            var size = (int)(imageStream.Length - imageStream.Position);
            var buffer = Utilities.ReadStream(imageStream);
            fixed (void* pbuffer = buffer)
            {
                return Load((IntPtr)pbuffer, size, true);
            }
        }

        public static Image Load(string fileName)
        {
            var stream = new NativeFileStream(fileName, NativeFileMode.Open, NativeFileAccess.Read);
            int size = (int) stream.Length;
            var memoryPtr = Utilities.AllocateMemory(size);
            try
            {
                stream.Read(memoryPtr, 0, size);
            }
            catch (Exception ex)
            {
                Utilities.FreeMemory(memoryPtr);
                throw;
            }
            finally
            {
                stream.Close();
            }

            return Load(memoryPtr, size);
        }

        private static TextureDescription CreateDescription(TextureDimension dimension, int width, int height, int depth, MipMapCount mipMapCount, PixelFormat format, int arraySize)
        {
            return new TextureDescription()
                       {
                           Width = width,
                           Height = height,
                           Depth = depth,
                           ArraySize = arraySize,
                           BindFlags = BindFlags.None,
                           CpuAccessFlags = CpuAccessFlags.Read | CpuAccessFlags.Write,
                           Dimension = dimension,
                           Format = format,
                           MipLevels = mipMapCount,
                           OptionFlags = ResourceOptionFlags.None,
                           SampleDescription = new SampleDescription(1, 0),
                           Usage = ResourceUsage.Default
                       };
        }

        /// <summary>
        /// Determines number of image array entries and pixel size.
        /// </summary>
        /// <param name="metadata">Description of the image to create.</param>
        /// <param name="cpFlags">Pitch flags.</param>
        /// <param name="mipMapCount">Output number of mipmap.</param>
        /// <param name="pixelSizeInBytes">Output total size to allocate pixel buffers for all images.</param>
        private static void CalculateImageArray( TextureDescription metadata, Texture.PitchFlags cpFlags, out int mipMapCount, out int pixelSizeInBytes )
        {
            pixelSizeInBytes = 0;
            mipMapCount = 0;

            for (int j = 0; j < metadata.ArraySize; j++)
            {
                int w = metadata.Width;
                int h = metadata.Height;
                int d = metadata.Depth; 
                
                for (int i = 0; i < metadata.MipLevels; i++)
                {
                    int rowPitch, slicePitch;
                    Texture.ComputePitch(metadata.Format, w, h, out rowPitch, out slicePitch, cpFlags);

                    for (int slice = 0; slice < d; ++slice)
                    {
                        pixelSizeInBytes += slicePitch;
                        ++mipMapCount;
                    }

                    if (h > 1)
                        h >>= 1;

                    if (w > 1)
                        w >>= 1;

                    if (d > 1)
                        d >>= 1;
                }
            }
        }


//-------------------------------------------------------------------------------------
// Fills in the image array entries
//-------------------------------------------------------------------------------------
        private static unsafe void SetupImageArray(IntPtr pMemory, int pixelSize, TextureDescription metadata, Texture.PitchFlags cpFlags, PixelBuffer[] mipmaps)
        {
            int index = 0;
            var pixels = (byte*)pMemory;
            for (uint item = 0; item < metadata.ArraySize; ++item)
            {
                int w = metadata.Width;
                int h = metadata.Height;
                int d = metadata.Depth;

                for (uint level = 0; level < metadata.MipLevels; ++level)
                {
                    int rowPitch, slicePitch;
                    Texture.ComputePitch(metadata.Format, w, h, out rowPitch, out slicePitch, cpFlags);

                    for (uint slice = 0; slice < d; ++slice)
                    {
                        // We use the same memory organization that Direct3D 11 needs for D3D11_SUBRESOURCE_DATA
                        // with all slices of a given miplevel being continuous in memory
                        mipmaps[index] = new PixelBuffer(w, h, d, metadata.Format, rowPitch, slicePitch, (IntPtr)pixels);
                        ++index;

                        pixels += slicePitch;
                    }

                    if (h > 1)
                        h >>= 1;

                    if (w > 1)
                        w >>= 1;

                    if (d > 1)
                        d >>= 1;
                }
            }
        }

   }
}