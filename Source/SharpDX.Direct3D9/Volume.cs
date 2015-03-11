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

using SharpDX.Direct3D;
using SharpDX.Mathematics.Interop;

namespace SharpDX.Direct3D9
{
    public partial class Volume
    {
        /// <summary>	
        /// Loads a volume from memory.	
        /// </summary>	
        /// <param name="destPaletteRef"><para>Pointer to a  <see cref="SharpDX.Direct3D9.PaletteEntry"/> structure, the destination palette of 256 colors or <c>null</c>.</para></param>	
        /// <param name="destBox"><para>Pointer to a <see cref="SharpDX.Direct3D9.Box"/> structure. Specifies the destination box. Set this parameter to <c>null</c> to specify the entire volume.</para></param>	
        /// <param name="srcMemoryPointer"><para>Pointer to the top-left corner of the source volume in memory.</para></param>	
        /// <param name="srcFormat"><para>Member of the <see cref="SharpDX.Direct3D9.Format"/> enumerated type, the pixel format of the source volume.</para></param>	
        /// <param name="srcRowPitch"><para>Pitch of source image, in bytes. For DXT formats (compressed texture formats), this number should represent the size of one row of cells, in bytes.</para></param>	
        /// <param name="srcSlicePitch"><para>Pitch of source image, in bytes. For DXT formats (compressed texture formats), this number should represent the size of one slice of cells, in bytes.</para></param>	
        /// <param name="srcPaletteRef"><para>Pointer to a <see cref="SharpDX.Direct3D9.PaletteEntry"/> structure, the source palette of 256 colors or <c>null</c>.</para></param>	
        /// <param name="srcBox"><para>Pointer to a <see cref="SharpDX.Direct3D9.Box"/> structure. Specifies the source box. <c>null</c> is not a valid value for this parameter.</para></param>	
        /// <param name="filter"><para>A combination of one or more <see cref="SharpDX.Direct3D9.Filter"/> controlling how the image is filtered. Specifying D3DX_DEFAULT for this parameter is the equivalent of specifying <see cref="SharpDX.Direct3D9.Filter.Triangle"/> | <see cref="SharpDX.Direct3D9.Filter.Dither"/>.</para></param>	
        /// <param name="colorKey"><para> <see cref="RawColor4"/> value to replace with transparent black, or 0 to disable the color key. This is always a 32-bit ARGB color, independent of the source image format. Alpha is significant and should usually be set to FF for opaque color keys. Thus, for opaque black, the value would be equal to 0xFF000000.</para></param>	
        /// <returns>If the function succeeds, the return value is <see cref="SharpDX.Direct3D9.ResultCode.Success"/>. If the function fails, the return value can be one of the following values: <see cref="SharpDX.Direct3D9.ResultCode.InvalidCall"/>, D3DXERR_INVALIDDATA.</returns>	
        /// <remarks>	
        /// Writing to a non-level-zero surface of the volume texture will not cause the dirty rectangle to be updated. If <see cref="SharpDX.Direct3D9.D3DX9.LoadVolumeFromMemory"/> is called and the texture was not already dirty (this is unlikely under normal usage scenarios), the application needs to explicitly call <see cref="SharpDX.Direct3D9.VolumeTexture.AddDirtyBox"/> on the volume texture.	
        /// </remarks>	
        /// <unmanaged>HRESULT D3DXLoadVolumeFromMemory([In] IDirect3DVolume9* pDestVolume,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestBox,[In] const void* pSrcMemory,[In] D3DFORMAT SrcFormat,[In] unsigned int SrcRowPitch,[In] unsigned int SrcSlicePitch,[In, Buffer] const PALETTEENTRY* pSrcPalette,[In] const void* pSrcBox,[In] D3DX_FILTER Filter,[In] int ColorKey)</unmanaged>	
        public unsafe void LoadFromMemory(SharpDX.Direct3D9.PaletteEntry[] destPaletteRef, Box? destBox, System.IntPtr srcMemoryPointer, SharpDX.Direct3D9.Format srcFormat, int srcRowPitch, int srcSlicePitch, SharpDX.Direct3D9.PaletteEntry[] srcPaletteRef, Box srcBox, SharpDX.Direct3D9.Filter filter, RawColorBGRA colorKey)
        {
            Box localDestBox;
            if (destBox.HasValue)
                localDestBox = destBox.Value;

            D3DX9.LoadVolumeFromMemory(
                this, destPaletteRef, new IntPtr(&localDestBox), srcMemoryPointer, srcFormat, srcRowPitch, srcSlicePitch, srcPaletteRef, new IntPtr(&srcBox), filter, *(int*)&colorKey);
        }

        /// <summary>
        /// Loads a volume from a file on the disk.
        /// </summary>
        /// <param name="volume">The volume.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXLoadVolumeFromFileW([In] IDirect3DVolume9* pDestVolume,[In] const PALETTEENTRY* pDestPalette,[In] const D3DBOX* pDestBox,[In] const wchar_t* pSrcFile,[In] const D3DBOX* pSrcBox,[In] unsigned int Filter,[In] D3DCOLOR ColorKey,[In] D3DXIMAGE_INFO* pSrcInfo)</unmanaged>
        public static void FromFile(Volume volume, string fileName, Filter filter, int colorKey)
        {
            D3DX9.LoadVolumeFromFileW(volume, null, IntPtr.Zero, fileName, IntPtr.Zero, filter, colorKey, IntPtr.Zero);
        }

        /// <summary>
        /// Loads a volume from a file on the disk.
        /// </summary>
        /// <param name="volume">The volume.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="sourceBox">The source box.</param>
        /// <param name="destinationBox">The destination box.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXLoadVolumeFromFileW([In] IDirect3DVolume9* pDestVolume,[In] const PALETTEENTRY* pDestPalette,[In] const D3DBOX* pDestBox,[In] const wchar_t* pSrcFile,[In] const D3DBOX* pSrcBox,[In] unsigned int Filter,[In] D3DCOLOR ColorKey,[In] D3DXIMAGE_INFO* pSrcInfo)</unmanaged>
        public static void FromFile(Volume volume, string fileName, Filter filter, int colorKey, Box sourceBox, Box destinationBox)
        {
            unsafe
            {
                D3DX9.LoadVolumeFromFileW(volume, null, new IntPtr(&destinationBox), fileName, new IntPtr(&sourceBox), filter, colorKey, IntPtr.Zero);
            }
        }

        /// <summary>
        /// Loads a volume from a file on the disk.
        /// </summary>
        /// <param name="volume">The volume.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="sourceBox">The source box.</param>
        /// <param name="destinationBox">The destination box.</param>
        /// <param name="imageInformation">The image information.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXLoadVolumeFromFileW([In] IDirect3DVolume9* pDestVolume,[In] const PALETTEENTRY* pDestPalette,[In] const D3DBOX* pDestBox,[In] const wchar_t* pSrcFile,[In] const D3DBOX* pSrcBox,[In] unsigned int Filter,[In] D3DCOLOR ColorKey,[In] D3DXIMAGE_INFO* pSrcInfo)</unmanaged>
        public static void FromFile(Volume volume, string fileName, Filter filter, int colorKey, Box sourceBox, Box destinationBox, out ImageInformation imageInformation)
        {

            FromFile(volume, fileName, filter, colorKey, sourceBox, destinationBox, null, out imageInformation);
        }

        /// <summary>
        /// Loads a volume from a file on the disk.
        /// </summary>
        /// <param name="volume">The volume.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="sourceBox">The source box.</param>
        /// <param name="destinationBox">The destination box.</param>
        /// <param name="palette">The palette.</param>
        /// <param name="imageInformation">The image information.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXLoadVolumeFromFileW([In] IDirect3DVolume9* pDestVolume,[In] const PALETTEENTRY* pDestPalette,[In] const D3DBOX* pDestBox,[In] const wchar_t* pSrcFile,[In] const D3DBOX* pSrcBox,[In] unsigned int Filter,[In] D3DCOLOR ColorKey,[In] D3DXIMAGE_INFO* pSrcInfo)</unmanaged>
        public static void FromFile(Volume volume, string fileName, Filter filter, int colorKey, Box sourceBox, Box destinationBox, PaletteEntry[] palette, out ImageInformation imageInformation)
        {
            unsafe
            {
                fixed (void* pImageInformation = &imageInformation)
                    D3DX9.LoadVolumeFromFileW(volume, palette, new IntPtr(&destinationBox), fileName, new IntPtr(&sourceBox), filter, colorKey, (IntPtr)pImageInformation);
            }
        }

        /// <summary>
        /// Loads a volume from a file in memory.
        /// </summary>
        /// <param name="volume">The volume.</param>
        /// <param name="memory">The memory.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXLoadVolumeFromFileInMemory([In] IDirect3DVolume9* pDestVolume,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestBox,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] const void* pSrcBox,[In] D3DX_FILTER Filter,[In] int ColorKey,[In] void* pSrcInfo)</unmanaged>
        public static void FromFileInMemory(Volume volume, byte[] memory, Filter filter, int colorKey)
        {
            unsafe
            {
                fixed (void* pMemory = memory)
                    D3DX9.LoadVolumeFromFileInMemory(volume, null, IntPtr.Zero, (IntPtr)pMemory, memory.Length, IntPtr.Zero, filter, colorKey, IntPtr.Zero);
            }
        }

        /// <summary>
        /// Loads a volume from a file in memory.
        /// </summary>
        /// <param name="volume">The volume.</param>
        /// <param name="memory">The memory.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="sourceBox">The source box.</param>
        /// <param name="destinationBox">The destination box.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXLoadVolumeFromFileInMemory([In] IDirect3DVolume9* pDestVolume,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestBox,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] const void* pSrcBox,[In] D3DX_FILTER Filter,[In] int ColorKey,[In] void* pSrcInfo)</unmanaged>
        public static void FromFileInMemory(Volume volume, byte[] memory, Filter filter, int colorKey, Box sourceBox, Box destinationBox)
        {
            unsafe
            {
                fixed (void* pMemory = memory)
                    D3DX9.LoadVolumeFromFileInMemory(volume, null, new IntPtr(&destinationBox), (IntPtr)pMemory, memory.Length, new IntPtr(&sourceBox), filter, colorKey, IntPtr.Zero);
            }
        }

        /// <summary>
        /// Loads a volume from a file in memory.
        /// </summary>
        /// <param name="volume">The volume.</param>
        /// <param name="memory">The memory.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="sourceBox">The source box.</param>
        /// <param name="destinationBox">The destination box.</param>
        /// <param name="imageInformation">The image information.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXLoadVolumeFromFileInMemory([In] IDirect3DVolume9* pDestVolume,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestBox,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] const void* pSrcBox,[In] D3DX_FILTER Filter,[In] int ColorKey,[In] void* pSrcInfo)</unmanaged>
        public static void FromFileInMemory(Volume volume, byte[] memory, Filter filter, int colorKey, Box sourceBox, Box destinationBox, out ImageInformation imageInformation)
        {
            FromFileInMemory(volume, memory, filter, colorKey, sourceBox, destinationBox, null, out imageInformation);
        }

        /// <summary>
        /// Loads a volume from a file in memory.
        /// </summary>
        /// <param name="volume">The volume.</param>
        /// <param name="memory">The memory.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="sourceBox">The source box.</param>
        /// <param name="destinationBox">The destination box.</param>
        /// <param name="palette">The palette.</param>
        /// <param name="imageInformation">The image information.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXLoadVolumeFromFileInMemory([In] IDirect3DVolume9* pDestVolume,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestBox,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] const void* pSrcBox,[In] D3DX_FILTER Filter,[In] int ColorKey,[In] void* pSrcInfo)</unmanaged>
        public static void FromFileInMemory(Volume volume, byte[] memory, Filter filter, int colorKey, Box sourceBox, Box destinationBox, PaletteEntry[] palette, out ImageInformation imageInformation)
        {
            unsafe
            {
                fixed (void* pMemory = memory)
                    fixed (void* pImageInformation = &imageInformation)
                        D3DX9.LoadVolumeFromFileInMemory(volume, palette, new IntPtr(&destinationBox), (IntPtr)pMemory, memory.Length, new IntPtr(&sourceBox), filter, colorKey, (IntPtr)pImageInformation);
            }
        }

        /// <summary>
        /// Loads a volume from a file in a stream.
        /// </summary>
        /// <param name="volume">The volume.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXLoadVolumeFromFileInMemory([In] IDirect3DVolume9* pDestVolume,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestBox,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] const void* pSrcBox,[In] D3DX_FILTER Filter,[In] int ColorKey,[In] void* pSrcInfo)</unmanaged>
        public static void FromFileInStream(Volume volume, Stream stream, Filter filter, int colorKey)
        {

            CreateFromFileInStream(volume, stream, filter, colorKey, IntPtr.Zero, IntPtr.Zero, null, IntPtr.Zero);
        }

        /// <summary>
        /// Loads a volume from a file in a stream.
        /// </summary>
        /// <param name="volume">The volume.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="sourceBox">The source box.</param>
        /// <param name="destinationBox">The destination box.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXLoadVolumeFromFileInMemory([In] IDirect3DVolume9* pDestVolume,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestBox,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] const void* pSrcBox,[In] D3DX_FILTER Filter,[In] int ColorKey,[In] void* pSrcInfo)</unmanaged>
        public static void FromFileInStream(Volume volume, Stream stream, Filter filter, int colorKey, Box sourceBox, Box destinationBox)
        {
            unsafe
            {
                CreateFromFileInStream(volume, stream, filter, colorKey, new IntPtr(&sourceBox), new IntPtr(&destinationBox), null, IntPtr.Zero);
            }
        }

        /// <summary>
        /// Loads a volume from a file in a stream.
        /// </summary>
        /// <param name="volume">The volume.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="sourceBox">The source box.</param>
        /// <param name="destinationBox">The destination box.</param>
        /// <param name="imageInformation">The image information.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXLoadVolumeFromFileInMemory([In] IDirect3DVolume9* pDestVolume,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestBox,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] const void* pSrcBox,[In] D3DX_FILTER Filter,[In] int ColorKey,[In] void* pSrcInfo)</unmanaged>
        public static void FromFileInStream(Volume volume, Stream stream, Filter filter, int colorKey, Box sourceBox, Box destinationBox, out ImageInformation imageInformation)
        {

            FromFileInStream(volume, stream, filter, colorKey, sourceBox, destinationBox, null, out imageInformation);
        }

        /// <summary>
        /// Loads a volume from a file in a stream.
        /// </summary>
        /// <param name="volume">The volume.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="sourceBox">The source box.</param>
        /// <param name="destinationBox">The destination box.</param>
        /// <param name="palette">The palette.</param>
        /// <param name="imageInformation">The image information.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT D3DXLoadVolumeFromFileInMemory([In] IDirect3DVolume9* pDestVolume,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestBox,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] const void* pSrcBox,[In] D3DX_FILTER Filter,[In] int ColorKey,[In] void* pSrcInfo)</unmanaged>
        public static void FromFileInStream(Volume volume, Stream stream, Filter filter, int colorKey, Box sourceBox, Box destinationBox, PaletteEntry[] palette, out ImageInformation imageInformation)
        {
            unsafe
            {
                fixed (void* pImageInformation = &imageInformation)
                    CreateFromFileInStream(volume, stream, filter, colorKey, new IntPtr(&sourceBox), new IntPtr(&destinationBox), palette, (IntPtr)pImageInformation);
            }
        }

        private static void CreateFromFileInStream(Volume volume, Stream stream, Filter filter, int colorKey, IntPtr sourceBox, IntPtr destinationBox, PaletteEntry[] palette, IntPtr imageInformation)
        {
            if (stream is DataStream)
            {
                D3DX9.LoadVolumeFromFileInMemory(volume, palette, destinationBox, ((DataStream)stream).PositionPointer, (int)(stream.Length - stream.Position), sourceBox, filter, colorKey, imageInformation);
            }
            else
            {
                unsafe
                {
                    var data = Utilities.ReadStream(stream);
                    fixed (void* pData = data)
                        D3DX9.LoadVolumeFromFileInMemory(volume, palette, destinationBox, (IntPtr)pData, data.Length, sourceBox, filter, colorKey, imageInformation);
                }
            }
        }

        /// <summary>
        /// Loads a volume from a source volume.
        /// </summary>
        /// <param name="destinationVolume">The destination volume.</param>
        /// <param name="sourceVolume">The source volume.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXLoadVolumeFromVolume([In] IDirect3DVolume9* pDestVolume,[In] const PALETTEENTRY* pDestPalette,[In] const D3DBOX* pDestBox,[In] IDirect3DVolume9* pSrcVolume,[In] const PALETTEENTRY* pSrcPalette,[In] const D3DBOX* pSrcBox,[In] unsigned int Filter,[In] D3DCOLOR ColorKey)</unmanaged>
        public static void FromVolume(Volume destinationVolume, Volume sourceVolume, Filter filter, int colorKey)
        {
            D3DX9.LoadVolumeFromVolume(destinationVolume, null, IntPtr.Zero, sourceVolume, null, IntPtr.Zero, filter, colorKey);
        }

        /// <summary>
        /// Loads a volume from a source volume.
        /// </summary>
        /// <param name="destinationVolume">The destination volume.</param>
        /// <param name="sourceVolume">The source volume.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="sourceBox">The source box.</param>
        /// <param name="destinationBox">The destination box.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXLoadVolumeFromVolume([In] IDirect3DVolume9* pDestVolume,[In] const PALETTEENTRY* pDestPalette,[In] const D3DBOX* pDestBox,[In] IDirect3DVolume9* pSrcVolume,[In] const PALETTEENTRY* pSrcPalette,[In] const D3DBOX* pSrcBox,[In] unsigned int Filter,[In] D3DCOLOR ColorKey)</unmanaged>
        public static void FromVolume(Volume destinationVolume, Volume sourceVolume, Filter filter, int colorKey, Box sourceBox, Box destinationBox)
        {
            FromVolume(destinationVolume, sourceVolume, filter, colorKey, sourceBox, destinationBox, null, null);
        }

        /// <summary>
        /// Loads a volume from a source volume.
        /// </summary>
        /// <param name="destinationVolume">The destination volume.</param>
        /// <param name="sourceVolume">The source volume.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="sourceBox">The source box.</param>
        /// <param name="destinationBox">The destination box.</param>
        /// <param name="destinationPalette">The destination palette.</param>
        /// <param name="sourcePalette">The source palette.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT D3DXLoadVolumeFromVolume([In] IDirect3DVolume9* pDestVolume,[In] const PALETTEENTRY* pDestPalette,[In] const D3DBOX* pDestBox,[In] IDirect3DVolume9* pSrcVolume,[In] const PALETTEENTRY* pSrcPalette,[In] const D3DBOX* pSrcBox,[In] unsigned int Filter,[In] D3DCOLOR ColorKey)</unmanaged>
        public static void FromVolume(Volume destinationVolume, Volume sourceVolume, Filter filter, int colorKey, Box sourceBox, Box destinationBox, PaletteEntry[] destinationPalette, PaletteEntry[] sourcePalette)
        {
            unsafe
            {
                D3DX9.LoadVolumeFromVolume(destinationVolume, destinationPalette, new IntPtr(&destinationBox), sourceVolume, sourcePalette, new IntPtr(&sourceBox), filter, colorKey);
            }
        }

        /// <summary>
        /// Locks a box on a volume resource.
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <returns>
        /// The locked region of this resource
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DVolume9::LockBox([Out] D3DLOCKED_BOX* pLockedVolume,[In] const void* pBox,[In] D3DLOCK Flags)</unmanaged>
        public DataBox LockBox(LockFlags flags)
        {

            LockedBox lockedBox;
            LockBox(out lockedBox, IntPtr.Zero, flags);
            return new DataBox(lockedBox.PBits, lockedBox.RowPitch, lockedBox.SlicePitch);
        }

        /// <summary>
        /// Locks a box on a volume resource.
        /// </summary>
        /// <param name="box">The box.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>The locked region of this resource</returns>
        /// <unmanaged>HRESULT IDirect3DVolume9::LockBox([Out] D3DLOCKED_BOX* pLockedVolume,[In] const void* pBox,[In] D3DLOCK Flags)</unmanaged>
        public DataBox LockBox(Box box, LockFlags flags)
        {
            unsafe
            {
                LockedBox lockedBox;
                LockBox(out lockedBox, new IntPtr(&box), flags);
                return new DataBox(lockedBox.PBits, lockedBox.RowPitch, lockedBox.SlicePitch);
            }
        }

        /// <summary>
        /// Saves a volume to a file on disk.
        /// </summary>
        /// <param name="volume">The volume.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXSaveVolumeToFileW([In] const wchar_t* pDestFile,[In] D3DXIMAGE_FILEFORMAT DestFormat,[In] IDirect3DVolume9* pSrcVolume,[In] const PALETTEENTRY* pSrcPalette,[In] const D3DBOX* pSrcBox)</unmanaged>
        public static void ToFile(Volume volume, string fileName, ImageFileFormat format)
        {
            D3DX9.SaveVolumeToFileW(fileName, format, volume, null, IntPtr.Zero);
        }

        /// <summary>
        /// Saves a volume to a file on disk.
        /// </summary>
        /// <param name="volume">The volume.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="format">The format.</param>
        /// <param name="box">The box.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXSaveVolumeToFileW([In] const wchar_t* pDestFile,[In] D3DXIMAGE_FILEFORMAT DestFormat,[In] IDirect3DVolume9* pSrcVolume,[In] const PALETTEENTRY* pSrcPalette,[In] const D3DBOX* pSrcBox)</unmanaged>
        public static void ToFile(Volume volume, string fileName, ImageFileFormat format, Box box)
        {
            ToFile(volume, fileName, format, box, null);
        }

        /// <summary>
        /// Saves a volume to a file on disk.
        /// </summary>
        /// <param name="volume">The volume.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="format">The format.</param>
        /// <param name="box">The box.</param>
        /// <param name="palette">The palette.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT D3DXSaveVolumeToFileW([In] const wchar_t* pDestFile,[In] D3DXIMAGE_FILEFORMAT DestFormat,[In] IDirect3DVolume9* pSrcVolume,[In] const PALETTEENTRY* pSrcPalette,[In] const D3DBOX* pSrcBox)</unmanaged>
        public static void ToFile(Volume volume, string fileName, ImageFileFormat format, Box box, PaletteEntry[] palette)
        {
            unsafe
            {
                D3DX9.SaveVolumeToFileW(fileName, format, volume, palette, new IntPtr(&box));
            }
        }

        /// <summary>
        /// Saves a volume to a <see cref="DataStream"/>.
        /// </summary>
        /// <param name="volume">The volume.</param>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXSaveVolumeToFileInMemory([In] ID3DXBuffer** ppDestBuf,[In] D3DXIMAGE_FILEFORMAT DestFormat,[In] IDirect3DVolume9* pSrcVolume,[In, Buffer] const PALETTEENTRY* pSrcPalette,[In] const void* pSrcBox)</unmanaged>
        public static DataStream ToStream(Volume volume, ImageFileFormat format)
        {
            Blob blob;
            D3DX9.SaveVolumeToFileInMemory(out blob, format, volume, null, IntPtr.Zero);
            return new DataStream(blob);
        }

        /// <summary>
        /// Saves a volume to a <see cref="DataStream"/>.
        /// </summary>
        /// <param name="volume">The volume.</param>
        /// <param name="format">The format.</param>
        /// <param name="box">The box.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXSaveVolumeToFileInMemory([In] ID3DXBuffer** ppDestBuf,[In] D3DXIMAGE_FILEFORMAT DestFormat,[In] IDirect3DVolume9* pSrcVolume,[In, Buffer] const PALETTEENTRY* pSrcPalette,[In] const void* pSrcBox)</unmanaged>
        public static DataStream ToStream(Volume volume, ImageFileFormat format, Box box)
        {
            return ToStream(volume, format, box, null);
        }

        /// <summary>
        /// Saves a volume to a <see cref="DataStream"/>. 
        /// </summary>
        /// <param name="volume">The volume.</param>
        /// <param name="format">The format.</param>
        /// <param name="box">The box.</param>
        /// <param name="palette">The palette.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT D3DXSaveVolumeToFileInMemory([In] ID3DXBuffer** ppDestBuf,[In] D3DXIMAGE_FILEFORMAT DestFormat,[In] IDirect3DVolume9* pSrcVolume,[In, Buffer] const PALETTEENTRY* pSrcPalette,[In] const void* pSrcBox)</unmanaged>
        public static DataStream ToStream(Volume volume, ImageFileFormat format, Box box, PaletteEntry[] palette)
        {
            unsafe
            {
                Blob blob;
                D3DX9.SaveVolumeToFileInMemory(out blob, format, volume, palette, new IntPtr(&box));
                return new DataStream(blob);
            }
        }
    }
}