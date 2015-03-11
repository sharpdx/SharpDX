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
using SharpDX.Mathematics.Interop;

namespace SharpDX.Direct3D9
{
    public partial class CubeTexture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CubeTexture"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="edgeLength">Length of the edge.</param>
        /// <param name="levelCount">The level count.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="format">The format.</param>
        /// <param name="pool">The pool.</param>
        public CubeTexture(Device device, int edgeLength, int levelCount, Usage usage, Format format, Pool pool = Pool.Managed) : base(IntPtr.Zero)
        {
            device.CreateCubeTexture(edgeLength, levelCount, (int)usage, format, pool, this, IntPtr.Zero);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CubeTexture"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="edgeLength">Length of the edge.</param>
        /// <param name="levelCount">The level count.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="format">The format.</param>
        /// <param name="pool">The pool.</param>
        /// <param name="sharedHandle">The shared handle.</param>
        public CubeTexture(Device device, int edgeLength, int levelCount, Usage usage, Format format, Pool pool, ref IntPtr sharedHandle) : base(IntPtr.Zero)
        {
            unsafe
            {
                fixed (void* pSharedHandle = &sharedHandle)
                    device.CreateCubeTexture(edgeLength, levelCount, (int)usage, format, pool, this, (IntPtr)pSharedHandle);
            }
        }

        /// <summary>
        /// Checks texture-creation parameters.
        /// </summary>
        /// <param name="device">Device associated with the texture.</param>
        /// <param name="size">Requested size of the texture. Null if </param>
        /// <param name="mipLevelCount">Requested number of mipmap levels for the texture.</param>
        /// <param name="usage">The requested usage for the texture.</param>
        /// <param name="format">Requested format for the texture.</param>
        /// <param name="pool">Memory class where the resource will be placed.</param>
        /// <returns>A value type containing the proposed values to pass to the texture creation functions.</returns>
        /// <unmanaged>HRESULT D3DXCheckCubeTextureRequirements([In] IDirect3DDevice9* pDevice,[InOut] unsigned int* pSize,[InOut] unsigned int* pNumMipLevels,[In] unsigned int Usage,[InOut] D3DFORMAT* pFormat,[In] D3DPOOL Pool)</unmanaged>
        public static CubeTextureRequirements CheckRequirements(Device device, int size, int mipLevelCount, Usage usage, Format format, Pool pool)
        {
            var result = new CubeTextureRequirements
                {
                    Size = size,
                    MipLevelCount = mipLevelCount,
                    Format = format
                };
            D3DX9.CheckCubeTextureRequirements(device, ref result.Size, ref result.MipLevelCount, (int)usage, ref result.Format, pool);
            return result;
        }

        /// <summary>
        /// Uses a user-provided function to fill each texel of each mip level of a given cube texture.
        /// </summary>
        /// <param name="callback">A function that is used to fill the texture.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        public void Fill(Fill3DCallback callback)
        {
            var handle = GCHandle.Alloc(callback);
            try
            {
                D3DX9.FillCubeTexture(this, FillCallbackHelper.Native3DCallbackPtr, GCHandle.ToIntPtr(handle));
            } 
            finally
            {
                handle.Free();
            }
        }

        /// <summary>
        /// Uses a compiled high-level shader language (HLSL) function to fill each texel of each mipmap level of a texture.
        /// </summary>
        /// <param name="shader">A texture shader object that is used to fill the texture.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        public void Fill(TextureShader shader)
        {
            D3DX9.FillCubeTextureTX(this, shader);
        }

        /// <summary>
        /// Locks a rectangle on a cube texture resource.
        /// </summary>
        /// <param name="faceType">Type of the face.</param>
        /// <param name="level">The level.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>
        /// A <see cref="DataRectangle"/> describing the region locked.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DCubeTexture9::LockRect([In] D3DCUBEMAP_FACES FaceType,[In] unsigned int Level,[In] D3DLOCKED_RECT* pLockedRect,[In] const void* pRect,[In] D3DLOCK Flags)</unmanaged>
        public DataRectangle LockRectangle(SharpDX.Direct3D9.CubeMapFace faceType, int level, SharpDX.Direct3D9.LockFlags flags)
        {
            LockedRectangle lockedRect;
            LockRectangle(faceType, level, out lockedRect, IntPtr.Zero, flags);
            return new DataRectangle(lockedRect.PBits, lockedRect.Pitch);
        }

        /// <summary>
        /// Locks a rectangle on a cube texture resource.
        /// </summary>
        /// <param name="faceType">Type of the face.</param>
        /// <param name="level">The level.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="stream">The stream pointing to the locked region.</param>
        /// <returns>
        /// A <see cref="DataRectangle"/> describing the region locked.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DCubeTexture9::LockRect([In] D3DCUBEMAP_FACES FaceType,[In] unsigned int Level,[In] D3DLOCKED_RECT* pLockedRect,[In] const void* pRect,[In] D3DLOCK Flags)</unmanaged>
        public DataRectangle LockRectangle(SharpDX.Direct3D9.CubeMapFace faceType, int level, SharpDX.Direct3D9.LockFlags flags, out DataStream stream)
        {
            LockedRectangle lockedRect;
            LockRectangle(faceType, level, out lockedRect, IntPtr.Zero, flags);
            stream = new DataStream(lockedRect.PBits, lockedRect.Pitch * GetLevelDescription(level).Height, true, (flags & LockFlags.ReadOnly) == 0);
            return new DataRectangle(lockedRect.PBits, lockedRect.Pitch);
        }

        /// <summary>
        /// Locks a rectangle on a cube texture resource.
        /// </summary>
        /// <param name="faceType">Type of the face.</param>
        /// <param name="level">The level.</param>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>
        /// A <see cref="DataRectangle"/> describing the region locked.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DCubeTexture9::LockRect([In] D3DCUBEMAP_FACES FaceType,[In] unsigned int Level,[In] D3DLOCKED_RECT* pLockedRect,[In] const void* pRect,[In] D3DLOCK Flags)</unmanaged>
        public DataRectangle LockRectangle(SharpDX.Direct3D9.CubeMapFace faceType, int level, RawRectangle rectangle, SharpDX.Direct3D9.LockFlags flags) {
            unsafe
            {
                LockedRectangle lockedRect;
                LockRectangle(faceType, level, out lockedRect, new IntPtr(&rectangle), flags);
                return new DataRectangle(lockedRect.PBits, lockedRect.Pitch);
            }
        }

        /// <summary>
        /// Locks a rectangle on a cube texture resource.
        /// </summary>
        /// <param name="faceType">Type of the face.</param>
        /// <param name="level">The level.</param>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="stream">The stream pointing to the locked region.</param>
        /// <returns>
        /// A <see cref="DataRectangle"/> describing the region locked.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DCubeTexture9::LockRect([In] D3DCUBEMAP_FACES FaceType,[In] unsigned int Level,[In] D3DLOCKED_RECT* pLockedRect,[In] const void* pRect,[In] D3DLOCK Flags)</unmanaged>
        public DataRectangle LockRectangle(SharpDX.Direct3D9.CubeMapFace faceType, int level, RawRectangle rectangle, SharpDX.Direct3D9.LockFlags flags, out DataStream stream)
        {
            unsafe
            {
                LockedRectangle lockedRect;
                LockRectangle(faceType, level, out lockedRect, new IntPtr(&rectangle), flags);
                stream = new DataStream(lockedRect.PBits, lockedRect.Pitch * GetLevelDescription(level).Height, true, (flags & LockFlags.ReadOnly) == 0);
                return new DataRectangle(lockedRect.PBits, lockedRect.Pitch);
            }
        }

        /// <summary>
        /// Adds a dirty region to a cube texture resource.
        /// </summary>
        /// <param name="faceType">Type of the face.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT IDirect3DCubeTexture9::AddDirtyRect([In] D3DCUBEMAP_FACES FaceType,[In] const void* pDirtyRect)</unmanaged>
        public void AddDirtyRectangle(SharpDX.Direct3D9.CubeMapFace faceType)
        {
            AddDirtyRectangle(faceType, IntPtr.Zero);
        }

        /// <summary>
        /// Adds a dirty region to a cube texture resource.
        /// </summary>
        /// <param name="faceType">Type of the face.</param>
        /// <param name="dirtyRectRef">The dirty rect ref.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT IDirect3DCubeTexture9::AddDirtyRect([In] D3DCUBEMAP_FACES FaceType,[In] const void* pDirtyRect)</unmanaged>
        public void AddDirtyRectangle(SharpDX.Direct3D9.CubeMapFace faceType, RawRectangle dirtyRectRef)
        {
            unsafe
            {
                AddDirtyRectangle(faceType, new IntPtr(&dirtyRectRef));
            }
        }

        /// <summary>
        /// Creates a <see cref="CubeTexture"/> from a file
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="filename">The filename.</param>
        /// <returns>
        /// A <see cref="CubeTexture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateCubeTextureFromFileExW([In] IDirect3DDevice9* pDevice,[In] const wchar_t* pSrcFile,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[In] void* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DCubeTexture9** ppCubeTexture)</unmanaged>
        public static CubeTexture FromFile(Device device, string filename)
        {
            CubeTexture cubeTexture;
            D3DX9.CreateCubeTextureFromFileW(device, filename, out cubeTexture);
            return cubeTexture;
        }

        /// <summary>
        /// Creates a <see cref="CubeTexture"/> from a file
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="pool">The pool.</param>
        /// <returns>
        /// A <see cref="CubeTexture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateCubeTextureFromFileExW([In] IDirect3DDevice9* pDevice,[In] const wchar_t* pSrcFile,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[In] void* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DCubeTexture9** ppCubeTexture)</unmanaged>
        public static CubeTexture FromFile(Device device, string filename, Usage usage, Pool pool)
        {
            return FromFile(device, filename, -1, -1, usage, Format.Unknown, pool, Filter.Default, Filter.Default, 0);
        }

        /// <summary>
        /// Creates a <see cref="CubeTexture"/> from a file
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="size">The size.</param>
        /// <param name="levelCount">The level count.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="format">The format.</param>
        /// <param name="pool">The pool.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="mipFilter">The mip filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <returns>
        /// A <see cref="CubeTexture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateCubeTextureFromFileExW([In] IDirect3DDevice9* pDevice,[In] const wchar_t* pSrcFile,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[In] void* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DCubeTexture9** ppCubeTexture)</unmanaged>
        public static CubeTexture FromFile(Device device, string filename, int size, int levelCount, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey)
        {
            return CreateFromFile(device, filename, size, levelCount, usage, format, pool, filter, mipFilter, colorKey, IntPtr.Zero, null);
        }

        /// <summary>
        /// Creates a <see cref="CubeTexture"/> from a file
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="size">The size.</param>
        /// <param name="levelCount">The level count.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="format">The format.</param>
        /// <param name="pool">The pool.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="mipFilter">The mip filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="imageInformation">The image information.</param>
        /// <returns>
        /// A <see cref="CubeTexture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateCubeTextureFromFileExW([In] IDirect3DDevice9* pDevice,[In] const wchar_t* pSrcFile,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[In] void* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DCubeTexture9** ppCubeTexture)</unmanaged>
        public static unsafe CubeTexture FromFile(Device device, string filename, int size, int levelCount, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey, out ImageInformation imageInformation)
        {
            fixed (void* pImageInfo = &imageInformation)
                return CreateFromFile(device, filename, size, levelCount, usage, format, pool, filter, mipFilter, colorKey, (IntPtr)pImageInfo, null);
        }

        /// <summary>
        /// Creates a <see cref="CubeTexture"/> from a file
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="size">The size.</param>
        /// <param name="levelCount">The level count.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="format">The format.</param>
        /// <param name="pool">The pool.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="mipFilter">The mip filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="imageInformation">The image information.</param>
        /// <param name="palette">The palette.</param>
        /// <returns>
        /// A <see cref="CubeTexture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateCubeTextureFromFileExW([In] IDirect3DDevice9* pDevice,[In] const wchar_t* pSrcFile,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[In] void* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DCubeTexture9** ppCubeTexture)</unmanaged>
        public static unsafe CubeTexture FromFile(Device device, string filename, int size, int levelCount, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey, out ImageInformation imageInformation, out PaletteEntry[] palette)
        {
            palette = new PaletteEntry[256];
            fixed (void* pImageInfo = &imageInformation)
                return CreateFromFile(device, filename, size, levelCount, usage, format, pool, filter, mipFilter, colorKey, (IntPtr)pImageInfo, palette);
        }

        /// <summary>
        /// Creates a <see cref="CubeTexture"/> from a memory buffer.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="buffer">The buffer.</param>
        /// <returns>
        /// A <see cref="CubeTexture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateCubeTextureFromFileInMemory([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] IDirect3DCubeTexture9** ppCubeTexture)</unmanaged>
        public static CubeTexture FromMemory(Device device, byte[] buffer)
        {
            CubeTexture cubeTexture;
            unsafe
            {
                fixed (void* pData = buffer)
                    D3DX9.CreateCubeTextureFromFileInMemory(device, (IntPtr)pData, buffer.Length, out cubeTexture);
            }
            return cubeTexture;
        }

        /// <summary>
        /// Creates a <see cref="CubeTexture"/> from a memory buffer.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="pool">The pool.</param>
        /// <returns>
        /// A <see cref="CubeTexture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateCubeTextureFromFileInMemoryEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[Out] D3DXIMAGE_INFO* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DCubeTexture9** ppCubeTexture)</unmanaged>
        public static CubeTexture FromMemory(Device device, byte[] buffer, Usage usage, Pool pool)
        {
            return FromMemory(device, buffer, -1, -1, usage, Format.Unknown, pool, Filter.Default, Filter.Default, 0);
        }

        /// <summary>
        /// Creates a <see cref="CubeTexture"/> from a memory buffer.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="size">The size.</param>
        /// <param name="levelCount">The level count.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="format">The format.</param>
        /// <param name="pool">The pool.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="mipFilter">The mip filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <returns>
        /// A <see cref="CubeTexture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateCubeTextureFromFileInMemoryEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[Out] D3DXIMAGE_INFO* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DCubeTexture9** ppCubeTexture)</unmanaged>
        public static CubeTexture FromMemory(Device device, byte[] buffer, int size, int levelCount, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey)
        {
            return CreateFromMemory(device, buffer, size, levelCount, usage, format, pool, filter, mipFilter, colorKey, IntPtr.Zero, null);
        }

        /// <summary>
        /// Creates a <see cref="CubeTexture"/> from a memory buffer.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="size">The size.</param>
        /// <param name="levelCount">The level count.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="format">The format.</param>
        /// <param name="pool">The pool.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="mipFilter">The mip filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="imageInformation">The image information.</param>
        /// <returns>
        /// A <see cref="CubeTexture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateCubeTextureFromFileInMemoryEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[Out] D3DXIMAGE_INFO* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DCubeTexture9** ppCubeTexture)</unmanaged>
        public static unsafe CubeTexture FromMemory(Device device, byte[] buffer, int size, int levelCount, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey, out ImageInformation imageInformation)
        {
            fixed (void* pImageInfo = &imageInformation)
                return CreateFromMemory(device, buffer, size, levelCount, usage, format, pool, filter, mipFilter, colorKey, (IntPtr)pImageInfo, null);
        }

        /// <summary>
        /// Creates a <see cref="CubeTexture"/> from a memory buffer.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="size">The size.</param>
        /// <param name="levelCount">The level count.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="format">The format.</param>
        /// <param name="pool">The pool.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="mipFilter">The mip filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="imageInformation">The image information.</param>
        /// <param name="palette">The palette.</param>
        /// <returns>
        /// A <see cref="CubeTexture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateCubeTextureFromFileInMemoryEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[Out] D3DXIMAGE_INFO* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DCubeTexture9** ppCubeTexture)</unmanaged>
        public static unsafe CubeTexture FromMemory(Device device, byte[] buffer, int size, int levelCount, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey, out ImageInformation imageInformation, out PaletteEntry[] palette)
        {
            palette = new PaletteEntry[256];
            fixed (void* pImageInfo = &imageInformation)
                return CreateFromMemory(device, buffer, size, levelCount, usage, format, pool, filter, mipFilter, colorKey, (IntPtr)pImageInfo, palette);
        }

        /// <summary>
        /// Creates a <see cref="CubeTexture"/> from a stream.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="stream">The stream.</param>
        /// <returns>A <see cref="CubeTexture"/></returns>
        /// <unmanaged>HRESULT D3DXCreateCubeTextureFromFileInMemory([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] IDirect3DCubeTexture9** ppCubeTexture)</unmanaged>
        public static CubeTexture FromStream(Device device, Stream stream)
        {
            CubeTexture cubeTexture;
            if (stream is DataStream)
            {
                D3DX9.CreateCubeTextureFromFileInMemory(device, ((DataStream)stream).PositionPointer, (int)(stream.Length - stream.Position), out cubeTexture);
            } 
            else
            {
                unsafe
                {
                    var data = Utilities.ReadStream(stream);
                    fixed (void* pData = data)
                        D3DX9.CreateCubeTextureFromFileInMemory(device, (IntPtr)pData, data.Length, out cubeTexture);
                }
            }
            return cubeTexture;
        }

        /// <summary>
        /// Creates a <see cref="CubeTexture"/> from a stream.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="pool">The pool.</param>
        /// <returns>
        /// A <see cref="CubeTexture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateCubeTextureFromFileInMemoryEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[Out] D3DXIMAGE_INFO* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DCubeTexture9** ppCubeTexture)</unmanaged>
        public static CubeTexture FromStream(Device device, Stream stream, Usage usage, Pool pool)
        {
            return FromStream(device, stream, 0, -1, -1, usage, Format.Unknown, pool, Filter.Default, Filter.Default, 0);
        }

        /// <summary>
        /// Creates a <see cref="CubeTexture"/> from a stream.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="size">The size.</param>
        /// <param name="levelCount">The level count.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="format">The format.</param>
        /// <param name="pool">The pool.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="mipFilter">The mip filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <returns>
        /// A <see cref="CubeTexture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateCubeTextureFromFileInMemoryEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[Out] D3DXIMAGE_INFO* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DCubeTexture9** ppCubeTexture)</unmanaged>
        public static CubeTexture FromStream(Device device, Stream stream, int size, int levelCount, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey)
        {
            return FromStream(device, stream, 0, size, levelCount, usage, format, pool, filter, mipFilter, colorKey);
        }

        /// <summary>
        /// Creates a <see cref="CubeTexture"/> from a stream.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="sizeBytes">The size bytes.</param>
        /// <param name="size">The size.</param>
        /// <param name="levelCount">The level count.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="format">The format.</param>
        /// <param name="pool">The pool.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="mipFilter">The mip filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <returns>
        /// A <see cref="CubeTexture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateCubeTextureFromFileInMemoryEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[Out] D3DXIMAGE_INFO* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DCubeTexture9** ppCubeTexture)</unmanaged>
        public static CubeTexture FromStream(Device device, Stream stream, int sizeBytes, int size, int levelCount, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey)
        {
            return CreateFromStream(device, stream, sizeBytes, size, levelCount, usage, format, pool, filter, mipFilter, colorKey, IntPtr.Zero, null);
        }

        /// <summary>
        /// Creates a <see cref="CubeTexture"/> from a stream.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="sizeBytes">The size bytes.</param>
        /// <param name="size">The size.</param>
        /// <param name="levelCount">The level count.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="format">The format.</param>
        /// <param name="pool">The pool.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="mipFilter">The mip filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="imageInformation">The image information.</param>
        /// <returns>
        /// A <see cref="CubeTexture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateCubeTextureFromFileInMemoryEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[Out] D3DXIMAGE_INFO* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DCubeTexture9** ppCubeTexture)</unmanaged>
        public static unsafe CubeTexture FromStream(Device device, Stream stream, int sizeBytes, int size, int levelCount, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey, out ImageInformation imageInformation)
        {
            fixed (void* pImageInfo = &imageInformation)
                return CreateFromStream(device, stream, sizeBytes, size, levelCount, usage, format, pool, filter, mipFilter, colorKey, (IntPtr)pImageInfo, null);
        }

        /// <summary>
        /// Creates a <see cref="CubeTexture"/> from a stream.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="sizeBytes">The size bytes.</param>
        /// <param name="size">The size.</param>
        /// <param name="levelCount">The level count.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="format">The format.</param>
        /// <param name="pool">The pool.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="mipFilter">The mip filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="imageInformation">The image information.</param>
        /// <param name="palette">The palette.</param>
        /// <returns>
        /// A <see cref="CubeTexture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateCubeTextureFromFileInMemoryEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[Out] D3DXIMAGE_INFO* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DCubeTexture9** ppCubeTexture)</unmanaged>
        public static unsafe CubeTexture FromStream(Device device, Stream stream, int sizeBytes, int size, int levelCount, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey, out ImageInformation imageInformation, out PaletteEntry[] palette)
        {
            palette = new PaletteEntry[256];
            fixed (void* pImageInfo = &imageInformation)
                return CreateFromStream(device, stream, sizeBytes, size, levelCount, usage, format, pool, filter, mipFilter, colorKey, (IntPtr)pImageInfo, palette);
        }

        /// <summary>
        /// Creates a <see cref="CubeTexture"/> from a stream.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="size">The size.</param>
        /// <param name="levelCount">The level count.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="format">The format.</param>
        /// <param name="pool">The pool.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="mipFilter">The mip filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="imageInformation">The image information.</param>
        /// <param name="palette">The palette.</param>
        /// <returns>
        /// A <see cref="CubeTexture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateCubeTextureFromFileInMemoryEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[Out] D3DXIMAGE_INFO* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DCubeTexture9** ppCubeTexture)</unmanaged>
        private static unsafe CubeTexture CreateFromMemory(Device device, byte[] buffer, int size, int levelCount, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey, IntPtr imageInformation, PaletteEntry[] palette)
        {
            CubeTexture cubeTexture;
            fixed (void* pBuffer = buffer)
                cubeTexture = CreateFromPointer(
                    device,
                    (IntPtr)pBuffer,
                    buffer.Length,
                    size,
                    levelCount,
                    usage,
                    format,
                    pool,
                    filter,
                    mipFilter,
                    colorKey,
                    imageInformation,
                    palette
                    );
            return cubeTexture;
        }

        /// <summary>
        /// Creates a <see cref="CubeTexture"/> from a stream.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="sizeBytes">The size bytes.</param>
        /// <param name="size">The size.</param>
        /// <param name="levelCount">The level count.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="format">The format.</param>
        /// <param name="pool">The pool.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="mipFilter">The mip filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="imageInformation">The image information.</param>
        /// <param name="palette">The palette.</param>
        /// <returns>A <see cref="CubeTexture"/></returns>
        /// <unmanaged>HRESULT D3DXCreateCubeTextureFromFileInMemoryEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[Out] D3DXIMAGE_INFO* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DCubeTexture9** ppCubeTexture)</unmanaged>
        private static unsafe CubeTexture CreateFromStream(Device device, Stream stream, int sizeBytes, int size, int levelCount, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey, IntPtr imageInformation, PaletteEntry[] palette)
        {
            CubeTexture cubeTexture;
            sizeBytes = sizeBytes == 0 ? (int)(stream.Length - stream.Position) : sizeBytes;
            if (stream is DataStream)
            {
                cubeTexture = CreateFromPointer(
                    device,
                    ((DataStream)stream).PositionPointer,
                    sizeBytes,
                    size,
                    levelCount,
                    usage,
                    format,
                    pool,
                    filter,
                    mipFilter,
                    colorKey,
                    imageInformation,
                    palette
                    );
            }
            else
            {
                var data = Utilities.ReadStream(stream);
                fixed (void* pData = data)
                    cubeTexture = CreateFromPointer(
                        device,
                        (IntPtr)pData,
                        data.Length,
                        size,
                        levelCount,
                        usage,
                        format,
                        pool,
                        filter,
                        mipFilter,
                        colorKey,
                        imageInformation,
                        palette
                        );
            }
            stream.Position = sizeBytes;
            return cubeTexture;
        }

        /// <summary>
        /// Creates a <see cref="CubeTexture"/> from a stream.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="pointer">The pointer.</param>
        /// <param name="sizeInBytes">The size in bytes.</param>
        /// <param name="size">The size.</param>
        /// <param name="levelCount">The level count.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="format">The format.</param>
        /// <param name="pool">The pool.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="mipFilter">The mip filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="imageInformation">The image information.</param>
        /// <param name="palette">The palette.</param>
        /// <returns>
        /// A <see cref="CubeTexture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateCubeTextureFromFileInMemoryEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[Out] D3DXIMAGE_INFO* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DCubeTexture9** ppCubeTexture)</unmanaged>
        private static unsafe CubeTexture CreateFromPointer(Device device, IntPtr pointer, int sizeInBytes, int size, int levelCount, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey, IntPtr imageInformation, PaletteEntry[] palette)
        {
            CubeTexture cubeTexture;
            D3DX9.CreateCubeTextureFromFileInMemoryEx(
                device,
                pointer,
                sizeInBytes,
                size,
                levelCount,
                (int)usage,
                format,
                pool,
                (int)filter,
                (int)mipFilter,
                *(RawColorBGRA*)&colorKey,
                imageInformation,
                palette,
                out cubeTexture);
            return cubeTexture;
        }


        /// <summary>
        /// Creates a <see cref="CubeTexture"/> from a stream.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="size">The size.</param>
        /// <param name="levelCount">The level count.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="format">The format.</param>
        /// <param name="pool">The pool.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="mipFilter">The mip filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="imageInformation">The image information.</param>
        /// <param name="palette">The palette.</param>
        /// <returns>
        /// A <see cref="CubeTexture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateCubeTextureFromFileInMemoryEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[Out] D3DXIMAGE_INFO* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DCubeTexture9** ppCubeTexture)</unmanaged>
        private unsafe static CubeTexture CreateFromFile(Device device, string fileName, int size, int levelCount, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey, IntPtr imageInformation, PaletteEntry[] palette)
        {
            CubeTexture cubeTexture;
            D3DX9.CreateCubeTextureFromFileExW(
                device,
                fileName,
                size,
                levelCount,
                (int)usage,
                format,
                pool,
                (int)filter,
                (int)mipFilter,
                *(RawColorBGRA*)&colorKey, 
                imageInformation,
                palette,
                out cubeTexture);
            return cubeTexture;
        }
    }
}