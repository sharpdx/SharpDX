﻿// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
// Part of the following code is a port of http://directxtex.codeplex.com
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
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using SharpDX.DXGI;
using SharpDX.IO;
using SharpDX.Serialization;
using SharpDX.Toolkit.Content;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Provides method to instantiate an image 1D/2D/3D supporting TextureArray and mipmaps on the CPU or to load/save an image from the disk.
    /// </summary>
    [ContentReader(typeof(ImageContentReader))]
    public sealed class Image : Component
    {
        public delegate Image ImageLoadDelegate(IntPtr dataPointer, int dataSize, bool makeACopy, GCHandle? handle);
        public delegate void ImageSaveDelegate(PixelBuffer[] pixelBuffers, int count, ImageDescription description, Stream imageStream);

        private const string MagicCodeTKTX = "TKTX";

        /// <summary>
        /// Pixel buffers.
        /// </summary>
        internal PixelBuffer[] pixelBuffers;
        private DataBox[] dataBoxArray;
        private List<int> mipMapToZIndex;
        private int zBufferCountPerArraySlice;
        private MipMapDescription[] mipmapDescriptions;
        private static List<LoadSaveDelegate> loadSaveDelegates = new List<LoadSaveDelegate>();
        
        /// <summary>
        /// Provides access to all pixel buffers.
        /// </summary>
        /// <remarks>
        /// For Texture3D, each z slice of the Texture3D has a pixelBufferArray * by the number of mipmaps.
        /// For other textures, there is Description.MipLevels * Description.ArraySize pixel buffers.
        /// </remarks>
        private PixelBufferArray pixelBufferArray;

        /// <summary>
        /// Gets the total number of bytes occupied by this image in memory.
        /// </summary>
        private int totalSizeInBytes;

        /// <summary>
        /// Pointer to the buffer.
        /// </summary>
        private IntPtr buffer;

        /// <summary>
        /// True if the buffer must be disposed.
        /// </summary>
        private bool bufferIsDisposable;

        /// <summary>
        /// Handle != null if the buffer is a pinned managed object on the LOH (Large Object Heap).
        /// </summary>
        private GCHandle? handle;

        /// <summary>
        /// Description of this image.
        /// </summary>
        public ImageDescription Description;

        private Image()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image" /> class.
        /// </summary>
        /// <param name="description">The image description.</param>
        /// <param name="dataPointer">The pointer to the data buffer.</param>
        /// <param name="offset">The offset from the beginning of the data buffer.</param>
        /// <param name="handle">The handle (optional).</param>
        /// <param name="bufferIsDisposable">if set to <c>true</c> [buffer is disposable].</param>
        /// <exception cref="System.InvalidOperationException">If the format is invalid, or width/height/depth/arraysize is invalid with respect to the dimension.</exception>
        internal unsafe Image(ImageDescription description, IntPtr dataPointer, int offset, GCHandle? handle, bool bufferIsDisposable, PitchFlags pitchFlags = PitchFlags.None)
        {
            Initialize(description, dataPointer, offset, handle, bufferIsDisposable, pitchFlags);
        }

        protected override void Dispose(bool disposeManagedResources)
        {
            if (handle.HasValue)
            {
                handle.Value.Free();
            }

            if (bufferIsDisposable)
            {
                Utilities.FreeMemory(buffer);
            } 
            
            base.Dispose(disposeManagedResources);
        }

        /// <summary>
        /// Gets the mipmap description of this instance for the specified mipmap level.
        /// </summary>
        /// <param name="mipmap">The mipmap.</param>
        /// <returns>A description of a particular mipmap for this texture.</returns>
        public MipMapDescription GetMipMapDescription(int mipmap)
        {
            return mipmapDescriptions[mipmap];
        }

        /// <summary>
        /// Gets the pixel buffer for the specified array/z slice and mipmap level.
        /// </summary>
        /// <param name="arrayOrZSliceIndex">For 3D image, the parameter is the Z slice, otherwise it is an index into the texture array.</param>
        /// <param name="mipmap">The mipmap.</param>
        /// <returns>A <see cref="pixelBufferArray"/>.</returns>
        /// <exception cref="System.ArgumentException">If arrayOrZSliceIndex or mipmap are out of range.</exception>
        public PixelBuffer GetPixelBuffer(int arrayOrZSliceIndex, int mipmap)
        {
            // Check for parameters, as it is easy to mess up things...
            if (mipmap > Description.MipLevels)
                throw new ArgumentException("Invalid mipmap level", "mipmap");

            if (Description.Dimension == TextureDimension.Texture3D)
            {
                if (arrayOrZSliceIndex > Description.Depth)
                    throw new ArgumentException("Invalid z slice index", "arrayOrZSliceIndex");

                // For 3D textures
                return GetPixelBufferUnsafe(0, arrayOrZSliceIndex, mipmap);
            }
            
            if (arrayOrZSliceIndex > Description.ArraySize)
            {
                throw new ArgumentException("Invalid array slice index", "arrayOrZSliceIndex");
            }

            // For 1D, 2D textures
            return GetPixelBufferUnsafe(arrayOrZSliceIndex, 0, mipmap);
        }

        /// <summary>
        /// Gets the pixel buffer for the specified array/z slice and mipmap level.
        /// </summary>
        /// <param name="arrayIndex">Index into the texture array. Must be set to 0 for 3D images.</param>
        /// <param name="zIndex">Z index for 3D image. Must be set to 0 for all 1D/2D images.</param>
        /// <param name="mipmap">The mipmap.</param>
        /// <returns>A <see cref="pixelBufferArray"/>.</returns>
        /// <exception cref="System.ArgumentException">If arrayIndex, zIndex or mipmap are out of range.</exception>
        public PixelBuffer GetPixelBuffer(int arrayIndex, int zIndex, int mipmap)
        {
            // Check for parameters, as it is easy to mess up things...
            if (mipmap > Description.MipLevels)
                throw new ArgumentException("Invalid mipmap level", "mipmap");

            if (arrayIndex > Description.ArraySize)
                throw new ArgumentException("Invalid array slice index", "arrayIndex");

            if (zIndex > Description.Depth)
                throw new ArgumentException("Invalid z slice index", "zIndex");

            return this.GetPixelBufferUnsafe(arrayIndex, zIndex, mipmap);
        }


        /// <summary>
        /// Registers a loader/saver for a specified image file type.
        /// </summary>
        /// <param name="type">The file type (use integer and explicit casting to <see cref="ImageFileType"/> to register other file format.</param>
        /// <param name="loader">The loader delegate (can be null).</param>
        /// <param name="saver">The saver delegate (can be null).</param>
        /// <exception cref="System.ArgumentException"></exception>
        public static void Register(ImageFileType type, ImageLoadDelegate loader, ImageSaveDelegate saver)
        {
            // If reference equals, then it is null
            if (ReferenceEquals(loader, saver))
                throw new ArgumentNullException("Can set both loader and saver to null", "loader/saver");

            var newDelegate = new LoadSaveDelegate(type, loader, saver);
            for (int i = 0; i < loadSaveDelegates.Count; i++)
            {
                var loadSaveDelegate = loadSaveDelegates[i];
                if (loadSaveDelegate.FileType == type)
                {
                    loadSaveDelegates[i] = newDelegate;
                    return;
                }
            }
            loadSaveDelegates.Add(newDelegate);
        }


        /// <summary>
        /// Gets a pointer to the image buffer in memory.
        /// </summary>
        /// <value>A pointer to the image buffer in memory.</value>
        public IntPtr DataPointer
        {
            get { return this.buffer; }
        }

        /// <summary>
        /// Provides access to all pixel buffers.
        /// </summary>
        /// <remarks>
        /// For Texture3D, each z slice of the Texture3D has a pixelBufferArray * by the number of mipmaps.
        /// For other textures, there is Description.MipLevels * Description.ArraySize pixel buffers.
        /// </remarks>
        public PixelBufferArray PixelBuffer
        {
            get { return pixelBufferArray; }
        }

        /// <summary>
        /// Gets the total number of bytes occupied by this image in memory.
        /// </summary>
        public int TotalSizeInBytes
        {
            get { return totalSizeInBytes; }
        }

        /// <summary>
        /// Gets the databox from this image.
        /// </summary>
        /// <returns>The databox of this image.</returns>
        public DataBox[] ToDataBox()
        {
            return (DataBox[])dataBoxArray.Clone();
        }

        /// <summary>
        /// Gets the databox from this image.
        /// </summary>
        /// <returns>The databox of this image.</returns>
        private DataBox[] ComputeDataBox()
        {
            dataBoxArray = new DataBox[Description.ArraySize * Description.MipLevels];
            int i = 0;
            for (int arrayIndex = 0; arrayIndex < Description.ArraySize; arrayIndex++)
            {
                for (int mipIndex = 0; mipIndex < Description.MipLevels; mipIndex++)
                {
                    // Get the first z-slice (A DataBox for a Texture3D is pointing to the whole texture).
                    var pixelBuffer = this.GetPixelBufferUnsafe(arrayIndex, 0, mipIndex);

                    dataBoxArray[i].DataPointer = pixelBuffer.DataPointer;
                    dataBoxArray[i].RowPitch = pixelBuffer.RowStride;
                    dataBoxArray[i].SlicePitch = pixelBuffer.BufferStride;
                    i++;
                }
            }
            return dataBoxArray;
        }

        /// <summary>
        /// Creates a new instance of <see cref="Image"/> from an image description.
        /// </summary>
        /// <param name="description">The image description.</param>
        /// <returns>A new image.</returns>
        public static Image New(ImageDescription description)
        {
            return New(description, IntPtr.Zero);
        }

        /// <summary>
        /// Creates a new instance of a 1D <see cref="Image"/>.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="mipMapCount">The mip map count.</param>
        /// <param name="format">The format.</param>
        /// <param name="arraySize">Size of the array.</param>
        /// <returns>A new image.</returns>
        public static Image New1D(int width, MipMapCount mipMapCount, PixelFormat format, int arraySize = 1)
        {
            return New1D(width, mipMapCount, format, arraySize, IntPtr.Zero);
        }

        /// <summary>
        /// Creates a new instance of a 2D <see cref="Image"/>.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="mipMapCount">The mip map count.</param>
        /// <param name="format">The format.</param>
        /// <param name="arraySize">Size of the array.</param>
        /// <returns>A new image.</returns>
        public static Image New2D(int width, int height, MipMapCount mipMapCount, PixelFormat format, int arraySize = 1)
        {
            return New2D(width, height, mipMapCount, format, arraySize, IntPtr.Zero);
        }

        /// <summary>
        /// Creates a new instance of a Cube <see cref="Image"/>.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="mipMapCount">The mip map count.</param>
        /// <param name="format">The format.</param>
        /// <returns>A new image.</returns>
        public static Image NewCube(int width, MipMapCount mipMapCount, PixelFormat format)
        {
            return NewCube(width, mipMapCount, format, IntPtr.Zero);
        }

        /// <summary>
        /// Creates a new instance of a 3D <see cref="Image"/>.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="mipMapCount">The mip map count.</param>
        /// <param name="format">The format.</param>
        /// <returns>A new image.</returns>
        public static Image New3D(int width, int height, int depth, MipMapCount mipMapCount, PixelFormat format)
        {
            return New3D(width, height, depth, mipMapCount, format, IntPtr.Zero);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Image"/> from an image description.
        /// </summary>
        /// <param name="description">The image description.</param>
        /// <param name="dataPointer">Pointer to an existing buffer.</param>
        /// <returns>A new image.</returns>
        public static Image New(ImageDescription description, IntPtr dataPointer)
        {
            return new Image(description, dataPointer, 0, null, false);
        }

        /// <summary>
        /// Creates a new instance of a 1D <see cref="Image"/>.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="mipMapCount">The mip map count.</param>
        /// <param name="format">The format.</param>
        /// <param name="arraySize">Size of the array.</param>
        /// <param name="dataPointer">Pointer to an existing buffer.</param>
        /// <returns>A new image.</returns>
        public static Image New1D(int width, MipMapCount mipMapCount, PixelFormat format, int arraySize, IntPtr dataPointer)
        {
            return new Image(CreateDescription(TextureDimension.Texture1D, width, 1, 1, mipMapCount, format, arraySize), dataPointer, 0, null, false);
        }

        /// <summary>
        /// Creates a new instance of a 2D <see cref="Image"/>.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="mipMapCount">The mip map count.</param>
        /// <param name="format">The format.</param>
        /// <param name="arraySize">Size of the array.</param>
        /// <param name="dataPointer">Pointer to an existing buffer.</param>
        /// <returns>A new image.</returns>
        public static Image New2D(int width, int height, MipMapCount mipMapCount, PixelFormat format, int arraySize, IntPtr dataPointer)
        {
            return new Image(CreateDescription(TextureDimension.Texture2D, width, height, 1, mipMapCount, format, arraySize), dataPointer, 0, null, false);
        }

        /// <summary>
        /// Creates a new instance of a Cube <see cref="Image"/>.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="mipMapCount">The mip map count.</param>
        /// <param name="format">The format.</param>
        /// <param name="dataPointer">Pointer to an existing buffer.</param>
        /// <returns>A new image.</returns>
        public static Image NewCube(int width, MipMapCount mipMapCount, PixelFormat format, IntPtr dataPointer)
        {
            return new Image(CreateDescription(TextureDimension.TextureCube, width, width, 1, mipMapCount, format, 6), dataPointer, 0, null, false);
        }

        /// <summary>
        /// Creates a new instance of a 3D <see cref="Image"/>.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="mipMapCount">The mip map count.</param>
        /// <param name="format">The format.</param>
        /// <param name="dataPointer">Pointer to an existing buffer.</param>
        /// <returns>A new image.</returns>
        public static Image New3D(int width, int height, int depth, MipMapCount mipMapCount, PixelFormat format, IntPtr dataPointer)
        {
            return new Image(CreateDescription(TextureDimension.Texture3D, width, width, depth, mipMapCount, format, 1), dataPointer, 0, null, false);
        }

        /// <summary>
        /// Loads an image from an unmanaged memory pointer.
        /// </summary>
        /// <param name="dataBuffer">Pointer to an unmanaged memory. If <see cref="makeACopy"/> is false, this buffer must be allocated with <see cref="Utilities.AllocateMemory"/>.</param>
        /// <param name="makeACopy">True to copy the content of the buffer to a new allocated buffer, false otherwhise.</param>
        /// <returns>An new image.</returns>
        /// <remarks>If <see cref="makeACopy"/> is set to false, the returned image is now the holder of the unmanaged pointer and will release it on Dispose. </remarks>
        public static Image Load(DataPointer dataBuffer, bool makeACopy = false)
        {
            return Load(dataBuffer.Pointer, dataBuffer.Size, makeACopy);
        }

        /// <summary>
        /// Loads an image from an unmanaged memory pointer.
        /// </summary>
        /// <param name="dataPointer">Pointer to an unmanaged memory. If <see cref="makeACopy"/> is false, this buffer must be allocated with <see cref="Utilities.AllocateMemory"/>.</param>
        /// <param name="dataSize">Size of the unmanaged buffer.</param>
        /// <param name="makeACopy">True to copy the content of the buffer to a new allocated buffer, false otherwise.</param>
        /// <returns>An new image.</returns>
        /// <remarks>If <see cref="makeACopy"/> is set to false, the returned image is now the holder of the unmanaged pointer and will release it on Dispose. </remarks>
        public static Image Load(IntPtr dataPointer, int dataSize, bool makeACopy = false)
        {
            return Load(dataPointer, dataSize, makeACopy, null);
        }

        /// <summary>
        /// Loads an image from a managed buffer.
        /// </summary>
        /// <param name="buffer">Reference to a managed buffer.</param>
        /// <returns>An new image.</returns>
        /// <remarks>This method support the following format: <c>dds, bmp, jpg, png, gif, tiff, wmp, tga</c>.</remarks>
        public unsafe static Image Load(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            int size = buffer.Length;

            // If buffer is allocated on Large Object Heap, then we are going to pin it instead of making a copy.
            if (size > (85 * 1024))
            {
                var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                return Load(handle.AddrOfPinnedObject(), size, false, handle);
            }

            fixed (void* pbuffer = buffer)
            {
                return Load((IntPtr) pbuffer, size, true);
            }
        }

        /// <summary>
        /// Loads the specified image from a stream.
        /// </summary>
        /// <param name="imageStream">The image stream.</param>
        /// <returns>An new image.</returns>
        /// <remarks>This method support the following format: <c>dds, bmp, jpg, png, gif, tiff, wmp, tga</c>.</remarks>
        public static Image Load(Stream imageStream)
        {
            // Use fast path using NativeFileStream
            // TODO: THIS IS NOT OPTIMIZED IN THE CASE THE STREAM IS NOT AN IMAGE. FIND A WAY TO OPTIMIZE THIS CASE.
            var nativeImageStream = imageStream as NativeFileStream;
            if (nativeImageStream != null)
            {
                var imageBuffer = IntPtr.Zero;
                Image image = null;
                try
                {
                    var imageSize = (int)nativeImageStream.Length;
                    imageBuffer = Utilities.AllocateMemory(imageSize);
                    nativeImageStream.Read(imageBuffer, 0, imageSize);
                    image = Load(imageBuffer, imageSize);
                }
                finally
                {
                    if (image == null)
                    {
                        Utilities.FreeMemory(imageBuffer);
                    }
                }
                return image;
            }

            // Else Read the whole stream into memory.
            return Load(Utilities.ReadStream(imageStream));
        }

        /// <summary>
        /// Loads the specified image from a file.
        /// </summary>
        /// <param name="fileName">The filename.</param>
        /// <returns>An new image.</returns>
        /// <remarks>This method support the following format: <c>dds, bmp, jpg, png, gif, tiff, wmp, tga</c>.</remarks>
        public static Image Load(string fileName)
        {
            NativeFileStream stream = null;
            IntPtr memoryPtr = IntPtr.Zero;
            int size;
            try
            {
                stream = new NativeFileStream(fileName, NativeFileMode.Open, NativeFileAccess.Read);
                size = (int)stream.Length;
                memoryPtr = Utilities.AllocateMemory(size);
                stream.Read(memoryPtr, 0, size);
            }
            catch (Exception)
            {
                if (memoryPtr != IntPtr.Zero)
                    Utilities.FreeMemory(memoryPtr);
                throw;
            }
            finally
            {
                try
                {
                    if (stream != null)
                        stream.Dispose();
                } catch {}
            }

            // If everything was fine, load the image from memory
            return Load(memoryPtr, size, false);
        }

        /// <summary>
        /// Saves this instance to a file.
        /// </summary>
        /// <param name="fileName">The destination file. Filename must end with a known extension (dds, bmp, jpg, png, gif, tiff, wmp, tga)</param>
        public void Save(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            extension = extension ?? string.Empty;

            ImageFileType fileType;
            extension = extension.TrimStart('.').ToLower();
            switch (extension)
            {
                case "jpg":
                    fileType = ImageFileType.Jpg;
                    break;
                case "dds":
                    fileType = ImageFileType.Dds;
                    break;
                case "gif":
                    fileType = ImageFileType.Gif;
                    break;
                case "bmp":
                    fileType = ImageFileType.Bmp;
                    break;
                case "png":
                    fileType = ImageFileType.Png;
                    break;
                case "tga":
                    fileType = ImageFileType.Tga;
                    break;
                case "tiff":
                    fileType = ImageFileType.Tiff;
                    break;
                case "tktx":
                    fileType = ImageFileType.Tktx;
                    break;
                case "wmp":
                    fileType = ImageFileType.Wmp;
                    break;
                default:
                    throw new ArgumentException("Filename must have a supported image extension: dds, bmp, jpg, png, gif, tiff, wmp, tga");
            }

            Save(fileName, fileType);
        }

        /// <summary>
        /// Saves this instance to a file.
        /// </summary>
        /// <param name="fileName">The destination file.</param>
        /// <param name="fileType">Specify the output format.</param>
        /// <remarks>This method support the following format: <c>dds, bmp, jpg, png, gif, tiff, wmp, tga</c>.</remarks>
        public void Save(string fileName, ImageFileType fileType)
        {
            using (var imageStream = new NativeFileStream(fileName, NativeFileMode.Create, NativeFileAccess.Write))
            {
                Save(imageStream, fileType);
            }
        }

        /// <summary>
        /// Saves this instance to a stream.
        /// </summary>
        /// <param name="imageStream">The destination stream.</param>
        /// <param name="fileType">Specify the output format.</param>
        /// <remarks>This method support the following format: <c>dds, bmp, jpg, png, gif, tiff, wmp, tga</c>.</remarks>
        public void Save(Stream imageStream, ImageFileType fileType)
        {
            Save(pixelBuffers, this.pixelBuffers.Length, Description, imageStream, fileType);
        }


        /// <summary>
        /// Loads an image from the specified pointer.
        /// </summary>
        /// <param name="dataPointer">The data pointer.</param>
        /// <param name="dataSize">Size of the data.</param>
        /// <param name="makeACopy">if set to <c>true</c> [make A copy].</param>
        /// <param name="handle">The handle.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        private static Image Load(IntPtr dataPointer, int dataSize, bool makeACopy, GCHandle? handle)
        {
            foreach (var loadSaveDelegate in loadSaveDelegates)
            {
                if (loadSaveDelegate.Load != null)
                {
                    var image = loadSaveDelegate.Load(dataPointer, dataSize, makeACopy, handle);
                    if (image != null)
                    {
                        return image;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Saves this instance to a stream.
        /// </summary>
        /// <param name="pixelBuffers">The buffers to save.</param>
        /// <param name="count">The number of buffers to save.</param>
        /// <param name="description">Global description of the buffer.</param>
        /// <param name="imageStream">The destination stream.</param>
        /// <param name="fileType">Specify the output format.</param>
        /// <remarks>This method support the following format: <c>dds, bmp, jpg, png, gif, tiff, wmp, tga</c>.</remarks>
        internal static void Save(PixelBuffer[] pixelBuffers, int count, ImageDescription description, Stream imageStream, ImageFileType fileType)
        {
            foreach (var loadSaveDelegate in loadSaveDelegates)
            {
                if (loadSaveDelegate.FileType == fileType)
                {
                    loadSaveDelegate.Save(pixelBuffers, count, description, imageStream);
                    return;
                }

            }
            throw new NotSupportedException("This file format is not yet implemented.");
        }

        static Image()
        {
            Register(ImageFileType.Dds,  DDSHelper.LoadFromDDSMemory, DDSHelper.SaveToDDSStream);
#if !WP8
            Register(ImageFileType.Gif,  WICHelper.LoadFromWICMemory, WICHelper.SaveGifToWICMemory);
            Register(ImageFileType.Tiff, WICHelper.LoadFromWICMemory, WICHelper.SaveTiffToWICMemory);
            Register(ImageFileType.Bmp,  WICHelper.LoadFromWICMemory, WICHelper.SaveBmpToWICMemory);
            Register(ImageFileType.Jpg,  WICHelper.LoadFromWICMemory, WICHelper.SaveJpgToWICMemory);
            Register(ImageFileType.Png,  WICHelper.LoadFromWICMemory, WICHelper.SavePngToWICMemory);
            Register(ImageFileType.Wmp,  WICHelper.LoadFromWICMemory, WICHelper.SaveWmpToWICMemory);
#endif
            Register(ImageFileType.Tktx, LoadTKTX, SaveTKTX);
        }


        internal unsafe void Initialize(ImageDescription description, IntPtr dataPointer, int offset, GCHandle? handle, bool bufferIsDisposable, PitchFlags pitchFlags = PitchFlags.None)
        {
            if (!FormatHelper.IsValid(description.Format) || FormatHelper.IsVideo(description.Format))
                throw new InvalidOperationException("Unsupported DXGI Format");

            this.handle = handle;

            switch (description.Dimension)
            {
                case TextureDimension.Texture1D:
                    if (description.Width <= 0 || description.Height != 1 || description.Depth != 1 || description.ArraySize == 0)
                        throw new InvalidOperationException("Invalid Width/Height/Depth/ArraySize for Image 1D");

                    // Check that miplevels are fine
                    description.MipLevels = Texture.CalculateMipLevels(description.Width, 1, description.MipLevels);
                    break;

                case TextureDimension.Texture2D:
                case TextureDimension.TextureCube:
                    if (description.Width <= 0 || description.Height <= 0 || description.Depth != 1 || description.ArraySize == 0)
                        throw new InvalidOperationException("Invalid Width/Height/Depth/ArraySize for Image 2D");

                    if (description.Dimension == TextureDimension.TextureCube)
                    {
                        if ((description.ArraySize % 6) != 0)
                            throw new InvalidOperationException("TextureCube must have an arraysize = 6");
                    }

                    // Check that miplevels are fine
                    description.MipLevels = Texture.CalculateMipLevels(description.Width, description.Height, description.MipLevels);
                    break;

                case TextureDimension.Texture3D:
                    if (description.Width <= 0 || description.Height <= 0 || description.Depth <= 0 || description.ArraySize != 1)
                        throw new InvalidOperationException("Invalid Width/Height/Depth/ArraySize for Image 3D");

                    // Check that miplevels are fine
                    description.MipLevels = Texture.CalculateMipLevels(description.Width, description.Height, description.Depth, description.MipLevels);
                    break;
            }

            // Calculate mipmaps
            int pixelBufferCount;
            this.mipMapToZIndex = CalculateImageArray(description, pitchFlags, out pixelBufferCount, out totalSizeInBytes);
            this.mipmapDescriptions = CalculateMipMapDescription(description, pitchFlags);
            zBufferCountPerArraySlice = this.mipMapToZIndex[this.mipMapToZIndex.Count - 1];

            // Allocate all pixel buffers
            pixelBuffers = new PixelBuffer[pixelBufferCount];
            pixelBufferArray = new PixelBufferArray(this);

            // Setup all pointers
            // only release buffer that is not pinned and is asked to be disposed.
            this.bufferIsDisposable = !handle.HasValue && bufferIsDisposable;
            this.buffer = dataPointer;

            if (dataPointer == IntPtr.Zero)
            {
                buffer = Utilities.AllocateMemory(totalSizeInBytes);
                offset = 0;
                this.bufferIsDisposable = true;
            }

            SetupImageArray((IntPtr)((byte*)buffer + offset), totalSizeInBytes, description, pitchFlags, pixelBuffers);

            Description = description;

            // PreCompute databoxes
            dataBoxArray = ComputeDataBox();
        }

        private PixelBuffer GetPixelBufferUnsafe(int arrayIndex, int zIndex, int mipmap)
        {
            var depthIndex = this.mipMapToZIndex[mipmap];
            var pixelBufferIndex = arrayIndex * this.zBufferCountPerArraySlice + depthIndex + zIndex;
            return pixelBuffers[pixelBufferIndex];
        }

        private static ImageDescription CreateDescription(TextureDimension dimension, int width, int height, int depth, MipMapCount mipMapCount, PixelFormat format, int arraySize)
        {
            return new ImageDescription()
                       {
                           Width = width,
                           Height = height,
                           Depth = depth,
                           ArraySize = arraySize,
                           Dimension = dimension,
                           Format = format,
                           MipLevels = mipMapCount,
                       };
        }

        /// <summary>
        /// Offset from the beginning of the buffer where pixel buffers are stored.
        /// This offset is used to keep data aligned on 16 bytes (if the original buffer is aligned on 16 bytes as well).
        /// </summary>
        private const int OffsetBufferTKTX = 48;

        /// <summary>
        /// Saves the specified pixel buffers in TKTX format.
        /// </summary>
        internal static Image LoadTKTX(IntPtr dataPointer, int dataSize, bool makeACopy, GCHandle? handle)
        {
            // Make a copy?
            if (makeACopy)
            {
                var temp = Utilities.AllocateMemory(dataSize);
                Utilities.CopyMemory(temp, dataPointer, dataSize);
                dataPointer = temp;
            }

            // Use DataStream to load from memory pointer
            var imageStream = new DataStream(dataPointer, dataSize, true, true);

            var beginPosition = imageStream.Position;

            var serializer = new BinarySerializer(imageStream, SerializerMode.Read);
            var description = default(ImageDescription);

            // Load MagicCode TKTX
            try
            {
                serializer.BeginChunk(MagicCodeTKTX);
            } catch (InvalidChunkException ex)
            {
                // If magic code not found, return null
                if (ex.ExpectedChunkId == MagicCodeTKTX)
                    return null;
                throw;
            }

            // Read description
            serializer.Serialize(ref description);

            // Read size of pixel buffer
            int size = 0;
            serializer.Serialize(ref size);

            // Pad to align pixel buffer on 16 bytes (fixed offset at 48 bytes from the beginning of the file).
            var padBuffer = new byte[OffsetBufferTKTX - (int)(imageStream.Position - beginPosition)];
            if (padBuffer.Length > 0)
            {
                if (imageStream.Read(padBuffer, 0, padBuffer.Length) != padBuffer.Length)
                    throw new EndOfStreamException();
            }

            // Check that current offset is exactly our fixed offset.
            int pixelBufferOffsets = (int)serializer.Stream.Position;
            if (pixelBufferOffsets != OffsetBufferTKTX)
                throw new InvalidOperationException(string.Format("Unexpected offset [{0}] for pixel buffers. Must be {1}", pixelBufferOffsets, OffsetBufferTKTX));

            // Seek to the end of the stream to the number of pixel buffers
            imageStream.Seek(size, SeekOrigin.Current);

            // Close the chunk to verify that we did read the whole chunk
            serializer.EndChunk();

            return new Image(description, dataPointer, pixelBufferOffsets, handle, makeACopy);
        }

        /// <summary>
        /// Saves the specified pixel buffers in TKTX format.
        /// </summary>
        /// <param name="pixelBuffers">The pixel buffers.</param>
        /// <param name="count">The count.</param>
        /// <param name="description">The description.</param>
        /// <param name="imageStream">The image stream.</param>
        internal static void SaveTKTX(PixelBuffer[] pixelBuffers, int count, ImageDescription description, Stream imageStream)
        {
            var serializer = new BinarySerializer(imageStream, SerializerMode.Write);

            var beginPosition = imageStream.Position;

            // Write MagicCode for TKTX
            serializer.BeginChunk(MagicCodeTKTX);

            // Serialize Description
            serializer.Serialize(ref description);

            // Serialize Size
            int size = 0;
            for (int i = 0; i < count; i++)
                size += pixelBuffers[i].BufferStride;
            serializer.Serialize(ref size);

            // Pad to align pixel buffer on 16 bytes (fixed offset at 48 bytes from the beginning of the file).
            var padBuffer = new byte[OffsetBufferTKTX - (int) (imageStream.Position - beginPosition)];
            if (padBuffer.Length > 0)
                imageStream.Write(padBuffer, 0, padBuffer.Length);

            // Write the whole pixel buffer
            for (int i = 0; i < count; i++)
                serializer.SerializeMemoryRegion(pixelBuffers[i].DataPointer, pixelBuffers[i].BufferStride);

            serializer.EndChunk();
        }

        [Flags]
        internal enum PitchFlags
        {
            None = 0x0,      // Normal operation
            LegacyDword = 0x1,      // Assume pitch is DWORD aligned instead of BYTE aligned
            Bpp24 = 0x10000,  // Override with a legacy 24 bits-per-pixel format size
            Bpp16 = 0x20000,  // Override with a legacy 16 bits-per-pixel format size
            Bpp8 = 0x40000,  // Override with a legacy 8 bits-per-pixel format size
        };

        internal static void ComputePitch(Format fmt, int width, int height, out int rowPitch, out int slicePitch, out int widthCount, out int heightCount, PitchFlags flags = PitchFlags.None)
        {
            widthCount = width;
            heightCount = height;

            if (FormatHelper.IsCompressed(fmt))
            {
                int bpb = (fmt == Format.BC1_Typeless
                             || fmt == Format.BC1_UNorm
                             || fmt == Format.BC1_UNorm_SRgb
                             || fmt == Format.BC4_Typeless
                             || fmt == Format.BC4_UNorm
                             || fmt == Format.BC4_SNorm) ? 8 : 16;
                widthCount = Math.Max(1, (width + 3) / 4);
                heightCount = Math.Max(1, (height + 3) / 4);
                rowPitch = widthCount * bpb;

                slicePitch = rowPitch * heightCount;
            }
            else if (FormatHelper.IsPacked(fmt))
            {
                rowPitch = ((width + 1) >> 1) * 4;

                slicePitch = rowPitch * height;
            }
            else
            {
                int bpp;

                if ((flags & PitchFlags.Bpp24) != 0)
                    bpp = 24;
                else if ((flags & PitchFlags.Bpp16) != 0)
                    bpp = 16;
                else if ((flags & PitchFlags.Bpp8) != 0)
                    bpp = 8;
                else
                    bpp = FormatHelper.SizeOfInBits(fmt);

                if ((flags & PitchFlags.LegacyDword) != 0)
                {
                    // Special computation for some incorrectly created DDS files based on
                    // legacy DirectDraw assumptions about pitch alignment
                    rowPitch = ((width * bpp + 31) / 32) * sizeof(int);
                    slicePitch = rowPitch * height;
                }
                else
                {
                    rowPitch = (width * bpp + 7) / 8;
                    slicePitch = rowPitch * height;
                }
            }
        }

        internal static MipMapDescription[] CalculateMipMapDescription(ImageDescription metadata, PitchFlags cpFlags = PitchFlags.None)
        {
            int nImages;
            int pixelSize;
            return CalculateMipMapDescription(metadata, cpFlags, out nImages, out pixelSize);
        }

        internal static MipMapDescription[] CalculateMipMapDescription(ImageDescription metadata, PitchFlags cpFlags, out int nImages, out int pixelSize)
        {
            pixelSize = 0;
            nImages = 0;

            int w = metadata.Width;
            int h = metadata.Height;
            int d = metadata.Depth;

            var mipmaps = new MipMapDescription[metadata.MipLevels];

            for (int level = 0; level < metadata.MipLevels; ++level)
            {
                int rowPitch, slicePitch;
                int widthPacked;
                int heightPacked;
                ComputePitch(metadata.Format, w, h, out rowPitch, out slicePitch, out widthPacked, out heightPacked, PitchFlags.None);

                mipmaps[level] = new MipMapDescription(
                    w,
                    h,
                    d,
                    rowPitch,
                    slicePitch,
                    widthPacked,
                    heightPacked
                    );

                pixelSize += d * slicePitch;
                nImages += d;

                if (h > 1)
                    h >>= 1;

                if (w > 1)
                    w >>= 1;

                if (d > 1)
                    d >>= 1;
            }
            return mipmaps;
        }

        /// <summary>
        /// Determines number of image array entries and pixel size.
        /// </summary>
        /// <param name="imageDesc">Description of the image to create.</param>
        /// <param name="pitchFlags">Pitch flags.</param>
        /// <param name="bufferCount">Output number of mipmap.</param>
        /// <param name="pixelSizeInBytes">Output total size to allocate pixel buffers for all images.</param>
        private static List<int> CalculateImageArray( ImageDescription imageDesc, PitchFlags pitchFlags, out int bufferCount, out int pixelSizeInBytes)
        {
            pixelSizeInBytes = 0;
            bufferCount = 0;

            var mipmapToZIndex = new List<int>();

            for (int j = 0; j < imageDesc.ArraySize; j++)
            {
                int w = imageDesc.Width;
                int h = imageDesc.Height;
                int d = imageDesc.Depth; 
                
                for (int i = 0; i < imageDesc.MipLevels; i++)
                {
                    int rowPitch, slicePitch;
                    int widthPacked;
                    int heightPacked;
                    ComputePitch(imageDesc.Format, w, h, out rowPitch, out slicePitch, out widthPacked, out heightPacked, pitchFlags);

                    // Store the number of z-slices per miplevel
                    if ( j == 0)
                        mipmapToZIndex.Add(bufferCount);

                    // Keep a trace of indices for the 1st array size, for each mip levels
                    pixelSizeInBytes += d * slicePitch;
                    bufferCount += d;

                    if (h > 1)
                        h >>= 1;

                    if (w > 1)
                        w >>= 1;

                    if (d > 1)
                        d >>= 1;
                }

                // For the last mipmaps, store just the number of zbuffers in total
                if (j == 0)
                    mipmapToZIndex.Add(bufferCount);
            }
            return mipmapToZIndex;
        }

        /// <summary>
        /// Allocates PixelBuffers 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="pixelSize"></param>
        /// <param name="imageDesc"></param>
        /// <param name="pitchFlags"></param>
        /// <param name="output"></param>
        private static unsafe void SetupImageArray(IntPtr buffer, int pixelSize, ImageDescription imageDesc, PitchFlags pitchFlags, PixelBuffer[] output)
        {
            int index = 0;
            var pixels = (byte*)buffer;
            for (uint item = 0; item < imageDesc.ArraySize; ++item)
            {
                int w = imageDesc.Width;
                int h = imageDesc.Height;
                int d = imageDesc.Depth;

                for (uint level = 0; level < imageDesc.MipLevels; ++level)
                {
                    int rowPitch, slicePitch;
                    int widthPacked;
                    int heightPacked;
                    ComputePitch(imageDesc.Format, w, h, out rowPitch, out slicePitch, out widthPacked, out heightPacked, pitchFlags);

                    for (uint zSlice = 0; zSlice < d; ++zSlice)
                    {
                        // We use the same memory organization that Direct3D 11 needs for D3D11_SUBRESOURCE_DATA
                        // with all slices of a given miplevel being continuous in memory
                        output[index] = new PixelBuffer(w, h, imageDesc.Format, rowPitch, slicePitch, (IntPtr)pixels);
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

        private class LoadSaveDelegate
        {
            public LoadSaveDelegate(ImageFileType fileType, ImageLoadDelegate load, ImageSaveDelegate save)
            {
                FileType = fileType;
                Load = load;
                Save = save;
            }

            public ImageFileType FileType;

            public ImageLoadDelegate Load;

            public ImageSaveDelegate Save;
        }
   }
}