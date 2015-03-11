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
    public partial class Texture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Texture"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="levelCount">The level count.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="format">The format.</param>
        /// <param name="pool">The pool.</param>
        /// <unmanaged>HRESULT IDirect3DDevice9::CreateTexture([In] unsigned int Width,[In] unsigned int Height,[In] unsigned int Levels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[Out, Fast] IDirect3DTexture9** ppTexture,[In] void** pSharedHandle)</unmanaged>
        public Texture(Device device, int width, int height, int levelCount, Usage usage, Format format, Pool pool)
            : base(IntPtr.Zero)
        {
            device.CreateTexture(width, height, levelCount, (int)usage, format, pool, this, IntPtr.Zero);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Texture"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="levelCount">The level count.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="format">The format.</param>
        /// <param name="pool">The pool.</param>
        /// <param name="sharedHandle">The shared handle.</param>
        /// <unmanaged>HRESULT IDirect3DDevice9::CreateTexture([In] unsigned int Width,[In] unsigned int Height,[In] unsigned int Levels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[Out, Fast] IDirect3DTexture9** ppTexture,[In] void** pSharedHandle)</unmanaged>
        public Texture(Device device, int width, int height, int levelCount, Usage usage, Format format, Pool pool, ref IntPtr sharedHandle)
            : base(IntPtr.Zero)
        {
            unsafe
            {
                fixed (void* pSharedHandle = &sharedHandle)
                    device.CreateTexture(width, height, levelCount, (int)usage, format, pool, this, new IntPtr(pSharedHandle));
            }
        }

        /// <summary>
        /// Checks texture-creation parameters.
        /// </summary>
        /// <param name="device">Device associated with the texture.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="mipLevelCount">Requested number of mipmap levels for the texture.</param>
        /// <param name="usage">The requested usage for the texture.</param>
        /// <param name="format">Requested format for the texture.</param>
        /// <param name="pool">Memory class where the resource will be placed.</param>
        /// <returns>
        /// A value type containing the proposed values to pass to the texture creation functions.
        /// </returns>
        /// <unmanaged>HRESULT D3DXCheckTextureRequirements([In] IDirect3DDevice9* pDevice,[InOut] unsigned int* pWidth,[InOut] unsigned int* pHeight,[InOut] unsigned int* pNumMipLevels,[In] unsigned int Usage,[InOut] D3DFORMAT* pFormat,[In] D3DPOOL Pool)</unmanaged>
        public static TextureRequirements CheckRequirements(Device device, int width, int height, int mipLevelCount, Usage usage, Format format, Pool pool)
        {
            var result = new TextureRequirements();
            D3DX9.CheckTextureRequirements(device, ref result.Width, ref result.Height, ref result.MipLevelCount, (int)usage, ref result.Format, pool);
            return result;
        }

        /// <summary>
        /// Computes the normal map.
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <param name="sourceTexture">The source texture.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="channel">The channel.</param>
        /// <param name="amplitude">The amplitude.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT D3DXComputeNormalMap([In] IDirect3DTexture9* pTexture,[In] IDirect3DTexture9* pSrcTexture,[Out, Buffer] const PALETTEENTRY* pSrcPalette,[In] unsigned int Flags,[In] unsigned int Channel,[In] float Amplitude)</unmanaged>
        public static void ComputeNormalMap(Texture texture, Texture sourceTexture, NormalMapFlags flags, Channel channel, float amplitude)
        {
            D3DX9.ComputeNormalMap(texture, sourceTexture, null, (int)flags, (int)channel, amplitude);
        }

        /// <summary>
        /// Computes the normal map.
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <param name="sourceTexture">The source texture.</param>
        /// <param name="palette">The palette.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="channel">The channel.</param>
        /// <param name="amplitude">The amplitude.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT D3DXComputeNormalMap([In] IDirect3DTexture9* pTexture,[In] IDirect3DTexture9* pSrcTexture,[Out, Buffer] const PALETTEENTRY* pSrcPalette,[In] unsigned int Flags,[In] unsigned int Channel,[In] float Amplitude)</unmanaged>
        public static void ComputeNormalMap(Texture texture, Texture sourceTexture, PaletteEntry[] palette, NormalMapFlags flags, Channel channel, float amplitude)
        {
            D3DX9.ComputeNormalMap(texture, sourceTexture, palette, (int)flags, (int)channel, amplitude);
        }

        /// <summary>
        /// Uses a user-provided function to fill each texel of each mip level of a given texture.
        /// </summary>
        /// <param name="callback">A function that is used to fill the texture.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT D3DXFillTexture([In] IDirect3DTexture9* pTexture,[In] __function__stdcall* pFunction,[In] void* pData)</unmanaged>
        public void Fill(Fill2DCallback callback)
        {
            var handle = GCHandle.Alloc(callback);
            try
            {
                D3DX9.FillTexture(this, FillCallbackHelper.Native2DCallbackPtr, GCHandle.ToIntPtr(handle));
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
        /// <unmanaged>HRESULT D3DXFillTextureTX([In] IDirect3DTexture9* pTexture,[In] ID3DXTextureShader* pTextureShader)</unmanaged>
        public void Fill(TextureShader shader)
        {
            D3DX9.FillTextureTX(this, shader);
        }

        /// <summary>
        /// Locks a rectangle on a texture resource.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>
        /// A <see cref="DataRectangle"/> describing the region locked.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DTexture9::LockRect([In] unsigned int Level,[Out] D3DLOCKED_RECT* pLockedRect,[In] const void* pRect,[In] D3DLOCK Flags)</unmanaged>
        public DataRectangle LockRectangle(int level, SharpDX.Direct3D9.LockFlags flags)
        {
            LockedRectangle lockedRect;
            LockRectangle(level, out lockedRect, IntPtr.Zero, flags);
            return new DataRectangle(lockedRect.PBits, lockedRect.Pitch);
        }


        /// <summary>
        /// Locks a rectangle on a texture resource.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="stream">The stream pointing to the locked region.</param>
        /// <returns>
        /// A <see cref="DataRectangle"/> describing the region locked.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DTexture9::LockRect([In] unsigned int Level,[Out] D3DLOCKED_RECT* pLockedRect,[In] const void* pRect,[In] D3DLOCK Flags)</unmanaged>
        public DataRectangle LockRectangle(int level, SharpDX.Direct3D9.LockFlags flags, out DataStream stream)
        {
            LockedRectangle lockedRect;
            LockRectangle(level, out lockedRect, IntPtr.Zero, flags);
            stream = new DataStream(lockedRect.PBits, lockedRect.Pitch * GetLevelDescription(level).Height, true, (flags & LockFlags.ReadOnly) == 0);
            return new DataRectangle(lockedRect.PBits, lockedRect.Pitch);
        }

        
        /// <summary>
        /// Locks a rectangle on a texture resource.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>
        /// A <see cref="DataRectangle"/> describing the region locked.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DTexture9::LockRect([In] D3DCUBEMAP_FACES FaceType,[In] unsigned int Level,[In] D3DLOCKED_RECT* pLockedRect,[In] const void* pRect,[In] D3DLOCK Flags)</unmanaged>
        public DataRectangle LockRectangle(int level, RawRectangle rectangle, SharpDX.Direct3D9.LockFlags flags)
        {
            unsafe
            {
                LockedRectangle lockedRect;
                LockRectangle(level, out lockedRect, new IntPtr(&rectangle), flags);
                return new DataRectangle(lockedRect.PBits, lockedRect.Pitch);
            }
        }

        /// <summary>
        /// Locks a rectangle on a texture resource.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="stream">The stream pointing to the locked region.</param>
        /// <returns>
        /// A <see cref="DataRectangle"/> describing the region locked.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DTexture9::LockRect([In] D3DCUBEMAP_FACES FaceType,[In] unsigned int Level,[In] D3DLOCKED_RECT* pLockedRect,[In] const void* pRect,[In] D3DLOCK Flags)</unmanaged>
        public DataRectangle LockRectangle(int level, RawRectangle rectangle, SharpDX.Direct3D9.LockFlags flags, out DataStream stream)
        {
            unsafe
            {
                LockedRectangle lockedRect;
                LockRectangle(level, out lockedRect, new IntPtr(&rectangle), flags);
                stream = new DataStream(lockedRect.PBits, lockedRect.Pitch * GetLevelDescription(level).Height, true, (flags & LockFlags.ReadOnly) == 0);
                return new DataRectangle(lockedRect.PBits, lockedRect.Pitch);
            }
        }

        /// <summary>
        /// Adds a dirty region to a texture resource.
        /// </summary>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DTexture9::AddDirtyRect([In] const void* pDirtyRect)</unmanaged>
        public void AddDirtyRectangle()
        {
            AddDirtyRectangle(IntPtr.Zero);
        }

        /// <summary>
        /// Adds a dirty region to a texture resource.
        /// </summary>
        /// <param name="dirtyRectRef">The dirty rect ref.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DTexture9::AddDirtyRect([In] const void* pDirtyRect)</unmanaged>
        public void AddDirtyRectangle(RawRectangle dirtyRectRef)
        {
            unsafe
            {
                AddDirtyRectangle(new IntPtr(&dirtyRectRef));
            }
        }

        /// <summary>
        /// Creates a <see cref="Texture"/> from a file
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="filename">The filename.</param>
        /// <returns>
        /// A <see cref="Texture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateTextureFromFileExW([In] IDirect3DDevice9* pDevice,[In] const wchar_t* pSrcFile,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[In] void* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DTexture9** ppTexture)</unmanaged>
        public static Texture FromFile(Device device, string filename)
        {
            Texture cubeTexture;
            D3DX9.CreateTextureFromFileW(device, filename, out cubeTexture);
            return cubeTexture;
        }

        /// <summary>
        /// Creates a <see cref="Texture"/> from a file
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="pool">The pool.</param>
        /// <returns>
        /// A <see cref="Texture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateTextureFromFileExW([In] IDirect3DDevice9* pDevice,[In] const wchar_t* pSrcFile,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[In] void* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DTexture9** ppTexture)</unmanaged>
        public static Texture FromFile(Device device, string filename, Usage usage, Pool pool)
        {
            return FromFile(device, filename, -1, -1, -1, usage, Format.Unknown, pool, Filter.Default, Filter.Default, 0);
        }

        /// <summary>
        /// Creates a <see cref="Texture"/> from a file
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="levelCount">The level count.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="format">The format.</param>
        /// <param name="pool">The pool.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="mipFilter">The mip filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <returns>
        /// A <see cref="Texture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateTextureFromFileExW([In] IDirect3DDevice9* pDevice,[In] const wchar_t* pSrcFile,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[In] void* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DTexture9** ppTexture)</unmanaged>
        public static Texture FromFile(Device device, string filename, int width, int height, int levelCount, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey)
        {
            return CreateFromFile(device, filename, width, height, levelCount, usage, format, pool, filter, mipFilter, colorKey, IntPtr.Zero, null);
        }

        /// <summary>
        /// Creates a <see cref="Texture"/> from a file
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="levelCount">The level count.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="format">The format.</param>
        /// <param name="pool">The pool.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="mipFilter">The mip filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="imageInformation">The image information.</param>
        /// <returns>
        /// A <see cref="Texture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateTextureFromFileExW([In] IDirect3DDevice9* pDevice,[In] const wchar_t* pSrcFile,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[In] void* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DTexture9** ppTexture)</unmanaged>
        public static unsafe Texture FromFile(Device device, string filename, int width, int height, int levelCount, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey, out ImageInformation imageInformation)
        {
            fixed (void* pImageInfo = &imageInformation)
                return CreateFromFile(device, filename, width, height, levelCount, usage, format, pool, filter, mipFilter, colorKey, (IntPtr)pImageInfo, null);
        }

        /// <summary>
        /// Creates a <see cref="Texture"/> from a file
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
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
        /// A <see cref="Texture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateTextureFromFileExW([In] IDirect3DDevice9* pDevice,[In] const wchar_t* pSrcFile,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[In] void* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DTexture9** ppTexture)</unmanaged>
        public static unsafe Texture FromFile(Device device, string filename, int width, int height, int levelCount, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey, out ImageInformation imageInformation, out PaletteEntry[] palette)
        {
            palette = new PaletteEntry[256];
            fixed (void* pImageInfo = &imageInformation)
                return CreateFromFile(device, filename, width, height, levelCount, usage, format, pool, filter, mipFilter, colorKey, (IntPtr)pImageInfo, palette);
        }

        /// <summary>
        /// Creates a <see cref="Texture"/> from a memory buffer.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="buffer">The buffer.</param>
        /// <returns>
        /// A <see cref="Texture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateTextureFromFileInMemory([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] IDirect3DTexture9** ppTexture)</unmanaged>
        public static Texture FromMemory(Device device, byte[] buffer)
        {
            Texture cubeTexture;
            unsafe
            {
                fixed (void* pData = buffer)
                    D3DX9.CreateTextureFromFileInMemory(device, (IntPtr)pData, buffer.Length, out cubeTexture);
            }
            return cubeTexture;
        }

        /// <summary>
        /// Creates a <see cref="Texture"/> from a memory buffer.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="pool">The pool.</param>
        /// <returns>
        /// A <see cref="Texture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateTextureFromFileInMemoryEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[Out] D3DXIMAGE_INFO* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DTexture9** ppTexture)</unmanaged>
        public static Texture FromMemory(Device device, byte[] buffer, Usage usage, Pool pool)
        {
            return FromMemory(device, buffer, -1, -1, -1, usage, Format.Unknown, pool, Filter.Default, Filter.Default, 0);
        }

        /// <summary>
        /// Creates a <see cref="Texture"/> from a memory buffer.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="levelCount">The level count.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="format">The format.</param>
        /// <param name="pool">The pool.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="mipFilter">The mip filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <returns>
        /// A <see cref="Texture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateTextureFromFileInMemoryEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[Out] D3DXIMAGE_INFO* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DTexture9** ppTexture)</unmanaged>
        public static Texture FromMemory(Device device, byte[] buffer, int width, int height, int levelCount, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey)
        {
            return CreateFromMemory(device, buffer, width, height, levelCount, usage, format, pool, filter, mipFilter, colorKey, IntPtr.Zero, null);
        }

        /// <summary>
        /// Creates a <see cref="Texture"/> from a memory buffer.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="levelCount">The level count.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="format">The format.</param>
        /// <param name="pool">The pool.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="mipFilter">The mip filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="imageInformation">The image information.</param>
        /// <returns>
        /// A <see cref="Texture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateTextureFromFileInMemoryEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[Out] D3DXIMAGE_INFO* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DTexture9** ppTexture)</unmanaged>
        public static unsafe Texture FromMemory(Device device, byte[] buffer, int width, int height, int levelCount, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey, out ImageInformation imageInformation)
        {
            fixed (void* pImageInfo = &imageInformation)
                return CreateFromMemory(device, buffer, width, height, levelCount, usage, format, pool, filter, mipFilter, colorKey, (IntPtr)pImageInfo, null);
        }

        /// <summary>
        /// Creates a <see cref="Texture"/> from a memory buffer.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
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
        /// A <see cref="Texture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateTextureFromFileInMemoryEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[Out] D3DXIMAGE_INFO* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DTexture9** ppTexture)</unmanaged>
        public static unsafe Texture FromMemory(Device device, byte[] buffer, int width, int height, int levelCount, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey, out ImageInformation imageInformation, out PaletteEntry[] palette)
        {
            palette = new PaletteEntry[256];
            fixed (void* pImageInfo = &imageInformation)
                return CreateFromMemory(device, buffer, width, height, levelCount, usage, format, pool, filter, mipFilter, colorKey, (IntPtr)pImageInfo, palette);
        }

        /// <summary>
        /// Creates a <see cref="Texture"/> from a stream.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="stream">The stream.</param>
        /// <returns>A <see cref="Texture"/></returns>
        /// <unmanaged>HRESULT D3DXCreateTextureFromFileInMemory([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] IDirect3DTexture9** ppTexture)</unmanaged>
        public static Texture FromStream(Device device, Stream stream)
        {
            Texture cubeTexture;
            if (stream is DataStream)
            {
                D3DX9.CreateTextureFromFileInMemory(device, ((DataStream)stream).PositionPointer, (int)(stream.Length - stream.Position), out cubeTexture);
            }
            else
            {
                unsafe
                {
                    var data = Utilities.ReadStream(stream);
                    fixed (void* pData = data)
                        D3DX9.CreateTextureFromFileInMemory(device, (IntPtr)pData, data.Length, out cubeTexture);
                }
            }
            return cubeTexture;
        }

        /// <summary>
        /// Creates a <see cref="Texture"/> from a stream.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="pool">The pool.</param>
        /// <returns>
        /// A <see cref="Texture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateTextureFromFileInMemoryEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[Out] D3DXIMAGE_INFO* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DTexture9** ppTexture)</unmanaged>
        public static Texture FromStream(Device device, Stream stream, Usage usage, Pool pool)
        {
            return FromStream(device, stream, 0, -1, -1, usage, Format.Unknown, pool, Filter.Default, Filter.Default, 0);
        }

        /// <summary>
        /// Creates a <see cref="Texture"/> from a stream.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="levelCount">The level count.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="format">The format.</param>
        /// <param name="pool">The pool.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="mipFilter">The mip filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <returns>
        /// A <see cref="Texture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateTextureFromFileInMemoryEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[Out] D3DXIMAGE_INFO* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DTexture9** ppTexture)</unmanaged>
        public static Texture FromStream(Device device, Stream stream, int width, int height, int levelCount, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey)
        {
            return FromStream(device, stream, 0, width, height, levelCount, usage, format, pool, filter, mipFilter, colorKey);
        }

        /// <summary>
        /// Creates a <see cref="Texture"/> from a stream.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="sizeBytes">The size bytes.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="levelCount">The level count.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="format">The format.</param>
        /// <param name="pool">The pool.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="mipFilter">The mip filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <returns>
        /// A <see cref="Texture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateTextureFromFileInMemoryEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[Out] D3DXIMAGE_INFO* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DTexture9** ppTexture)</unmanaged>
        public static Texture FromStream(Device device, Stream stream, int sizeBytes, int width, int height, int levelCount, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey)
        {
            return CreateFromStream(device, stream, sizeBytes, width, height, levelCount, usage, format, pool, filter, mipFilter, colorKey, IntPtr.Zero, null);
        }

        /// <summary>
        /// Creates a <see cref="Texture"/> from a stream.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="sizeBytes">The size bytes.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height</param>
        /// <param name="levelCount">The level count.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="format">The format.</param>
        /// <param name="pool">The pool.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="mipFilter">The mip filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="imageInformation">The image information.</param>
        /// <returns>
        /// A <see cref="Texture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateTextureFromFileInMemoryEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[Out] D3DXIMAGE_INFO* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DTexture9** ppTexture)</unmanaged>
        public static unsafe Texture FromStream(Device device, Stream stream, int sizeBytes, int width, int height, int levelCount, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey, out ImageInformation imageInformation)
        {
            fixed (void* pImageInfo = &imageInformation)
                return CreateFromStream(device, stream, sizeBytes, width, height, levelCount, usage, format, pool, filter, mipFilter, colorKey, (IntPtr)pImageInfo, null);
        }

        /// <summary>
        /// Creates a <see cref="Texture"/> from a stream.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="sizeBytes">The size bytes.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
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
        /// A <see cref="Texture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateTextureFromFileInMemoryEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[Out] D3DXIMAGE_INFO* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DTexture9** ppTexture)</unmanaged>
        public static unsafe Texture FromStream(Device device, Stream stream, int sizeBytes, int width, int height, int levelCount, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey, out ImageInformation imageInformation, out PaletteEntry[] palette)
        {
            palette = new PaletteEntry[256];
            fixed (void* pImageInfo = &imageInformation)
                return CreateFromStream(device, stream, sizeBytes, width, height, levelCount, usage, format, pool, filter, mipFilter, colorKey, (IntPtr)pImageInfo, palette);
        }

        /// <summary>
        /// Creates a <see cref="Texture"/> from a stream.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
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
        /// A <see cref="Texture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateTextureFromFileInMemoryEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[Out] D3DXIMAGE_INFO* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DTexture9** ppTexture)</unmanaged>
        private static unsafe Texture CreateFromMemory(Device device, byte[] buffer, int width, int height, int levelCount, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey, IntPtr imageInformation, PaletteEntry[] palette)
        {
            Texture cubeTexture;
            fixed (void* pBuffer = buffer)
                cubeTexture = CreateFromPointer(
                    device,
                    (IntPtr)pBuffer,
                    buffer.Length,
                    width,
                    height,
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
        /// Creates a <see cref="Texture"/> from a stream.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="sizeBytes">The size bytes.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height</param>
        /// <param name="levelCount">The level count.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="format">The format.</param>
        /// <param name="pool">The pool.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="mipFilter">The mip filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="imageInformation">The image information.</param>
        /// <param name="palette">The palette.</param>
        /// <returns>A <see cref="Texture"/></returns>
        /// <unmanaged>HRESULT D3DXCreateTextureFromFileInMemoryEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[Out] D3DXIMAGE_INFO* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DTexture9** ppTexture)</unmanaged>
        private static unsafe Texture CreateFromStream(Device device, Stream stream, int sizeBytes, int width, int height, int levelCount, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey, IntPtr imageInformation, PaletteEntry[] palette)
        {
            Texture cubeTexture;
            sizeBytes = sizeBytes == 0 ? (int)(stream.Length - stream.Position): sizeBytes;
            if (stream is DataStream)
            {
                cubeTexture = CreateFromPointer(
                    device,
                    ((DataStream)stream).PositionPointer,
                    sizeBytes,
                    width,
                    height,
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
                        width,
                        height,
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
        /// Creates a <see cref="Texture"/> from a stream.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="pointer">The pointer.</param>
        /// <param name="sizeInBytes">The size in bytes.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
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
        /// A <see cref="Texture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateTextureFromFileInMemoryEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[Out] D3DXIMAGE_INFO* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DTexture9** ppTexture)</unmanaged>
        private static unsafe Texture CreateFromPointer(Device device, IntPtr pointer, int sizeInBytes, int width, int height, int levelCount, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey, IntPtr imageInformation, PaletteEntry[] palette)
        {
            Texture cubeTexture;
            D3DX9.CreateTextureFromFileInMemoryEx(
                device,
                pointer,
                sizeInBytes,
                width,
                height,
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
        /// Creates a <see cref="Texture"/> from a stream.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
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
        /// A <see cref="Texture"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateTextureFromFileInMemoryEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] unsigned int Size,[In] unsigned int MipLevels,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[In] unsigned int Filter,[In] unsigned int MipFilter,[In] D3DCOLOR ColorKey,[Out] D3DXIMAGE_INFO* pSrcInfo,[Out, Buffer] PALETTEENTRY* pPalette,[In] IDirect3DTexture9** ppTexture)</unmanaged>
        private static unsafe Texture CreateFromFile(Device device, string fileName, int width, int height, int levelCount, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey, IntPtr imageInformation, PaletteEntry[] palette)
        {
            Texture cubeTexture;
            D3DX9.CreateTextureFromFileExW(
                device,
                fileName,
                width,
                height,
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