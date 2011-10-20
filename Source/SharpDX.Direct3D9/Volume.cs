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
using System.IO;

namespace SharpDX.Direct3D9
{
    public partial class Volume
    {

        public static Result FromFile(Volume volume, string fileName, Filter filter, int colorKey)
        {

            return Result.Ok;
        }

        public static Result FromFile(Volume volume, string fileName, Filter filter, int colorKey, Box sourceBox, Box destinationBox)
        {

            return Result.Ok;
        }

        public static Result FromFile(Volume volume, string fileName, Filter filter, int colorKey, Box sourceBox, Box destinationBox, out ImageInformation imageInformation)
        {

            return Result.Ok;
        }

        public static Result FromFile(Volume volume, string fileName, Filter filter, int colorKey, Box sourceBox, Box destinationBox, PaletteEntry[] palette, out ImageInformation imageInformation)
        {
            return Result.Ok;
        }

        public static Result FromFileInMemory(Volume volume, byte[] memory, Filter filter, int colorKey)
        {
            unsafe
            {
                fixed (void* pMemory = &memory)
                    return D3DX9.LoadVolumeFromFileInMemory(volume, null, IntPtr.Zero, (IntPtr)pMemory, memory.Length, IntPtr.Zero, filter, colorKey, IntPtr.Zero);
            }
        }

        public static Result FromFileInMemory(Volume volume, byte[] memory, Filter filter, int colorKey, Box sourceBox, Box destinationBox)
        {
            unsafe
            {
                fixed (void* pMemory = &memory)
                    return D3DX9.LoadVolumeFromFileInMemory(volume, null, new IntPtr(&destinationBox), (IntPtr)pMemory, memory.Length, new IntPtr(&sourceBox), filter, colorKey, IntPtr.Zero);
            }
        }

        public static Result FromFileInMemory(Volume volume, byte[] memory, Filter filter, int colorKey, Box sourceBox, Box destinationBox, out ImageInformation imageInformation)
        {
            return FromFileInMemory(volume, memory, filter, colorKey, sourceBox, destinationBox, null, out imageInformation);
        }

        public static Result FromFileInMemory(Volume volume, byte[] memory, Filter filter, int colorKey, Box sourceBox, Box destinationBox, PaletteEntry[] palette, out ImageInformation imageInformation)
        {
            unsafe
            {
                fixed (void* pMemory = &memory)
                fixed (void* pImageInformation = &imageInformation)
                    return D3DX9.LoadVolumeFromFileInMemory(volume, palette, new IntPtr(&destinationBox), (IntPtr)pMemory, memory.Length, new IntPtr(&sourceBox), filter, colorKey, (IntPtr)pImageInformation);
            }
        }

        public static Result FromFileInStream(Volume volume, Stream stream, Filter filter, int colorKey)
        {

            return CreateFromFileInStream(volume, stream, filter, colorKey, IntPtr.Zero, IntPtr.Zero, null, IntPtr.Zero);
        }

        public static Result FromFileInStream(Volume volume, Stream stream, Filter filter, int colorKey, Box sourceBox, Box destinationBox)
        {
            unsafe
            {
                return CreateFromFileInStream(volume, stream, filter, colorKey, new IntPtr(&sourceBox), new IntPtr(&destinationBox), null, IntPtr.Zero);
            }
        }

        public static Result FromFileInStream(Volume volume, Stream stream, Filter filter, int colorKey, Box sourceBox, Box destinationBox, out ImageInformation imageInformation)
        {

            return FromFileInStream(volume, stream, filter, colorKey, sourceBox, destinationBox, null, out imageInformation);
        }

        public static Result FromFileInStream(Volume volume, Stream stream, Filter filter, int colorKey, Box sourceBox, Box destinationBox, PaletteEntry[] palette, out ImageInformation imageInformation)
        {
            unsafe
            {
                fixed (void* pImageInformation = &imageInformation)
                    return CreateFromFileInStream(volume, stream, filter, colorKey, new IntPtr(&sourceBox), new IntPtr(&destinationBox), palette, (IntPtr)pImageInformation);
            }
        }

        private static Result CreateFromFileInStream(Volume volume, Stream stream, Filter filter, int colorKey, IntPtr sourceBox, IntPtr destinationBox, PaletteEntry[] palette, IntPtr imageInformation)
        {

            unsafe
            {
                if (stream is DataStream)
                    return D3DX9.LoadVolumeFromFileInMemory(volume, palette, destinationBox, ((DataStream)stream).DataPointer, (int)stream.Length, sourceBox, filter, colorKey, (IntPtr)imageInformation);
                var data = Utilities.ReadStream(stream);
                fixed (void* pData = data)
                    D3DX9.LoadVolumeFromFileInMemory(volume, palette, destinationBox, (IntPtr)pData, data.Length, sourceBox, filter, colorKey, (IntPtr)imageInformation);
            }
        }

        public static Result FromVolume(Volume destinationVolume, Volume sourceVolume, Filter filter, int colorKey)
        {

            return Result.Ok;
        }

        public static Result FromVolume(Volume destinationVolume, Volume sourceVolume, Filter filter, int colorKey, Box sourceBox, Box destinationBox)
        {

            return Result.Ok;
        }

        public static Result FromVolume(Volume destinationVolume, Volume sourceVolume, Filter filter, int colorKey, Box sourceBox, Box destinationBox, PaletteEntry[] destinationPalette, PaletteEntry[] sourcePalette)
        {

            return Result.Ok;
        }

        public DataBox LockBox(LockFlags flags)
        {

            return Result.Ok;
        }

        public DataBox LockBox(Box box, LockFlags flags)
        {

            return Result.Ok;
        }

        public static Result ToFile(Volume volume, string fileName, ImageFileFormat format)
        {

            return Result.Ok;
        }

        public static Result ToFile(Volume volume, string fileName, ImageFileFormat format, Box box)
        {

            return Result.Ok;
        }

        public static Result ToFile(Volume volume, string fileName, ImageFileFormat format, Box box, PaletteEntry[] palette)
        {

            return Result.Ok;
        }

        public static DataStream ToStream(Volume volume, ImageFileFormat format)
        {

            return Result.Ok;
        }

        public static DataStream ToStream(Volume volume, ImageFileFormat format, Box box)
        {

            return Result.Ok;
        }

        public static DataStream ToStream(Volume volume, ImageFileFormat format, Box box, PaletteEntry[] palette)
        {

            return Result.Ok;
        }
    }
}