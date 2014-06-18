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
    public partial class Surface
    {

        /// <summary>
        /// Creates a depth-stencil surface.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">The format.</param>
        /// <param name="multisampleType">Type of the multisample.</param>
        /// <param name="multisampleQuality">The multisample quality.</param>
        /// <param name="discard">if set to <c>true</c> [discard].</param>
        /// <returns>A reference to a <see cref="Surface"/>, representing the created depth-stencil surface resource. </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::CreateDepthStencilSurface([In] unsigned int Width,[In] unsigned int Height,[In] D3DFORMAT Format,[In] D3DMULTISAMPLE_TYPE MultiSample,[In] unsigned int MultisampleQuality,[In] BOOL Discard,[Out] IDirect3DSurface9** ppSurface,[In] void** pSharedHandle)</unmanaged>
        public static Surface CreateDepthStencil(
            Device device, int width, int height, Format format, MultisampleType multisampleType, int multisampleQuality, bool discard)
        {
            return device.CreateDepthStencilSurface(width, height, format, multisampleType, multisampleQuality, discard, IntPtr.Zero);
        }

        /// <summary>
        /// Creates a depth-stencil surface.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">The format.</param>
        /// <param name="multisampleType">Type of the multisample.</param>
        /// <param name="multisampleQuality">The multisample quality.</param>
        /// <param name="discard">if set to <c>true</c> [discard].</param>
        /// <param name="sharedHandle">The shared handle.</param>
        /// <returns>A reference to a <see cref="Surface"/>, representing the created depth-stencil surface resource. </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::CreateDepthStencilSurface([In] unsigned int Width,[In] unsigned int Height,[In] D3DFORMAT Format,[In] D3DMULTISAMPLE_TYPE MultiSample,[In] unsigned int MultisampleQuality,[In] BOOL Discard,[Out] IDirect3DSurface9** ppSurface,[In] void** pSharedHandle)</unmanaged>
        public static Surface CreateDepthStencil(
            Device device, int width, int height, Format format, MultisampleType multisampleType, int multisampleQuality, bool discard, ref IntPtr sharedHandle)
        {
            unsafe
            {
                fixed (void* pSharedHandle = &sharedHandle)
                    return device.CreateDepthStencilSurface(width, height, format, multisampleType, multisampleQuality, discard, (IntPtr)pSharedHandle);
            }
        }

        /// <summary>
        /// Creates a depth-stencil surface.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">The format.</param>
        /// <param name="multisampleType">Type of the multisample.</param>
        /// <param name="multisampleQuality">The multisample quality.</param>
        /// <param name="discard">if set to <c>true</c> [discard].</param>
        /// <param name="usage">The usage.</param>
        /// <returns>A reference to a <see cref="Surface"/>, representing the created depth-stencil surface resource. </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9Ex::CreateDepthStencilSurfaceEx([In] unsigned int Width,[In] unsigned int Height,[In] D3DFORMAT Format,[In] D3DMULTISAMPLE_TYPE MultiSample,[In] unsigned int MultisampleQuality,[In] BOOL Discard,[Out, Fast] IDirect3DSurface9** ppSurface,[In] void** pSharedHandle,[In] unsigned int Usage)</unmanaged>
        public static Surface CreateDepthStencilEx(
            DeviceEx device, int width, int height, Format format, MultisampleType multisampleType, int multisampleQuality, bool discard, Usage usage)
        {
            return device.CreateDepthStencilSurfaceEx(width, height, format, multisampleType, multisampleQuality, discard, IntPtr.Zero, (int)usage);
        }

        /// <summary>
        /// Creates a depth-stencil surface.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">The format.</param>
        /// <param name="multisampleType">Type of the multisample.</param>
        /// <param name="multisampleQuality">The multisample quality.</param>
        /// <param name="discard">if set to <c>true</c> [discard].</param>
        /// <param name="usage">The usage.</param>
        /// <param name="sharedHandle">The shared handle.</param>
        /// <returns>A reference to a <see cref="Surface"/>, representing the created depth-stencil surface resource. </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9Ex::CreateDepthStencilSurfaceEx([In] unsigned int Width,[In] unsigned int Height,[In] D3DFORMAT Format,[In] D3DMULTISAMPLE_TYPE MultiSample,[In] unsigned int MultisampleQuality,[In] BOOL Discard,[Out, Fast] IDirect3DSurface9** ppSurface,[In] void** pSharedHandle,[In] unsigned int Usage)</unmanaged>
        public static Surface CreateDepthStencilEx(
            DeviceEx device,
            int width,
            int height,
            Format format,
            MultisampleType multisampleType,
            int multisampleQuality,
            bool discard,
            Usage usage,
            ref IntPtr sharedHandle)
        {
            unsafe
            {
                fixed (void* pSharedHandle = &sharedHandle)
                    return device.CreateDepthStencilSurfaceEx(
                        width, height, format, multisampleType, multisampleQuality, discard, (IntPtr)pSharedHandle, (int)usage);
            }
        }

        /// <summary>
        /// Create an off-screen surface.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">The format.</param>
        /// <param name="pool">The pool.</param>
        /// <returns>A <see cref="Surface"/> created.</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::CreateOffscreenPlainSurface([In] unsigned int Width,[In] unsigned int Height,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[Out, Fast] IDirect3DSurface9** ppSurface,[In] void** pSharedHandle)</unmanaged>
        public static Surface CreateOffscreenPlain(Device device, int width, int height, Format format, Pool pool)
        {
            return device.CreateOffscreenPlainSurface(width, height, format, pool, IntPtr.Zero);
        }

        /// <summary>
        /// Create an off-screen surface.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">The format.</param>
        /// <param name="pool">The pool.</param>
        /// <param name="sharedHandle">The shared handle.</param>
        /// <returns>A <see cref="Surface"/> created.</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::CreateOffscreenPlainSurface([In] unsigned int Width,[In] unsigned int Height,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[Out, Fast] IDirect3DSurface9** ppSurface,[In] void** pSharedHandle)</unmanaged>
        public static Surface CreateOffscreenPlain(Device device, int width, int height, Format format, Pool pool, ref IntPtr sharedHandle)
        {
            unsafe
            {
                fixed (void* pSharedHandle = &sharedHandle)
                    return device.CreateOffscreenPlainSurface(width, height, format, pool, (IntPtr)pSharedHandle);
            }
        }

        /// <summary>
        /// Create an off-screen surface.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">The format.</param>
        /// <param name="pool">The pool.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>
        /// A <see cref="Surface"/> created.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9Ex::CreateOffscreenPlainSurfaceEx([In] unsigned int Width,[In] unsigned int Height,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[Out] IDirect3DSurface9** ppSurface,[In] void** pSharedHandle,[In] unsigned int Usage)</unmanaged>
        public static Surface CreateOffscreenPlainEx(DeviceEx device, int width, int height, Format format, Pool pool, Usage usage)
        {
            return device.CreateOffscreenPlainSurfaceEx(width, height, format, pool, IntPtr.Zero, (int)usage);
        }

        /// <summary>
        /// Create an off-screen surface.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">The format.</param>
        /// <param name="pool">The pool.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="sharedHandle">The shared handle.</param>
        /// <returns>
        /// A <see cref="Surface"/> created.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9Ex::CreateOffscreenPlainSurfaceEx([In] unsigned int Width,[In] unsigned int Height,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[Out] IDirect3DSurface9** ppSurface,[In] void** pSharedHandle,[In] unsigned int Usage)</unmanaged>
        public static Surface CreateOffscreenPlainEx(DeviceEx device, int width, int height, Format format, Pool pool, Usage usage, ref IntPtr sharedHandle)
        {
            unsafe
            {
                fixed (void* pSharedHandle = &sharedHandle)
                    return device.CreateOffscreenPlainSurfaceEx(width, height, format, pool, (IntPtr)pSharedHandle, (int)usage);
            }
        }

        /// <summary>
        /// Creates a render-target surface.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">The format.</param>
        /// <param name="multisampleType">Type of the multisample.</param>
        /// <param name="multisampleQuality">The multisample quality.</param>
        /// <param name="lockable">if set to <c>true</c> [lockable].</param>
        /// <returns>
        /// A render-target <see cref="Surface"/>.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::CreateRenderTarget([In] unsigned int Width,[In] unsigned int Height,[In] D3DFORMAT Format,[In] D3DMULTISAMPLE_TYPE MultiSample,[In] unsigned int MultisampleQuality,[In] BOOL Lockable,[Out] IDirect3DSurface9** ppSurface,[In] void** pSharedHandle)</unmanaged>
        public static Surface CreateRenderTarget(
            Device device, int width, int height, Format format, MultisampleType multisampleType, int multisampleQuality, bool lockable)
        {
            return device.CreateRenderTarget(width, height, format, multisampleType, multisampleQuality, lockable, IntPtr.Zero);
        }

        /// <summary>
        /// Creates a render-target surface.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">The format.</param>
        /// <param name="multisampleType">Type of the multisample.</param>
        /// <param name="multisampleQuality">The multisample quality.</param>
        /// <param name="lockable">if set to <c>true</c> [lockable].</param>
        /// <param name="sharedHandle">The shared handle.</param>
        /// <returns>
        /// A render-target <see cref="Surface"/>.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::CreateRenderTarget([In] unsigned int Width,[In] unsigned int Height,[In] D3DFORMAT Format,[In] D3DMULTISAMPLE_TYPE MultiSample,[In] unsigned int MultisampleQuality,[In] BOOL Lockable,[Out] IDirect3DSurface9** ppSurface,[In] void** pSharedHandle)</unmanaged>
        public static Surface CreateRenderTarget(
            Device device, int width, int height, Format format, MultisampleType multisampleType, int multisampleQuality, bool lockable, ref IntPtr sharedHandle)
        {
            unsafe
            {
                fixed (void* pSharedHandle = &sharedHandle)
                    return device.CreateRenderTarget(width, height, format, multisampleType, multisampleQuality, lockable, (IntPtr)pSharedHandle);
            }
        }

        /// <summary>
        /// Creates a render-target surface.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">The format.</param>
        /// <param name="multisampleType">Type of the multisample.</param>
        /// <param name="multisampleQuality">The multisample quality.</param>
        /// <param name="lockable">if set to <c>true</c> [lockable].</param>
        /// <param name="usage">The usage.</param>
        /// <returns>A render-target <see cref="Surface"/>.</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9Ex::CreateRenderTargetEx([In] unsigned int Width,[In] unsigned int Height,[In] D3DFORMAT Format,[In] D3DMULTISAMPLE_TYPE MultiSample,[In] unsigned int MultisampleQuality,[In] BOOL Lockable,[Out] IDirect3DSurface9** ppSurface,[In] void** pSharedHandle,[In] unsigned int Usage)</unmanaged>
        public static Surface CreateRenderTargetEx(
            DeviceEx device, int width, int height, Format format, MultisampleType multisampleType, int multisampleQuality, bool lockable, Usage usage)
        {
            return device.CreateRenderTargetEx(width, height, format, multisampleType, multisampleQuality, lockable, IntPtr.Zero, (int)usage);
        }

        /// <summary>
        /// Creates a render-target surface.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">The format.</param>
        /// <param name="multisampleType">Type of the multisample.</param>
        /// <param name="multisampleQuality">The multisample quality.</param>
        /// <param name="lockable">if set to <c>true</c> [lockable].</param>
        /// <param name="usage">The usage.</param>
        /// <param name="sharedHandle">The shared handle.</param>
        /// <returns>A render-target <see cref="Surface"/>.</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9Ex::CreateRenderTargetEx([In] unsigned int Width,[In] unsigned int Height,[In] D3DFORMAT Format,[In] D3DMULTISAMPLE_TYPE MultiSample,[In] unsigned int MultisampleQuality,[In] BOOL Lockable,[Out] IDirect3DSurface9** ppSurface,[In] void** pSharedHandle,[In] unsigned int Usage)</unmanaged>
        public static Surface CreateRenderTargetEx(
            DeviceEx device,
            int width,
            int height,
            Format format,
            MultisampleType multisampleType,
            int multisampleQuality,
            bool lockable,
            Usage usage,
            ref IntPtr sharedHandle)
        {
            unsafe
            {
                fixed (void* pSharedHandle = &sharedHandle)
                    return device.CreateRenderTargetEx(width, height, format, multisampleType, multisampleQuality, lockable, (IntPtr)pSharedHandle, (int)usage);
            }
        }

        /// <summary>
        /// Loads a surface from a file.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT D3DXLoadSurfaceFromFileW([In] IDirect3DSurface9* pDestSurface,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestRect,[In] const wchar_t* pSrcFile,[In] const void* pSrcRect,[In] D3DX_FILTER Filter,[In] int ColorKey,[In] void* pSrcInfo)</unmanaged>
        public static void FromFile(Surface surface, string fileName, Filter filter, int colorKey)
        {
            D3DX9.LoadSurfaceFromFileW(surface, null, IntPtr.Zero, fileName, IntPtr.Zero, filter, colorKey, IntPtr.Zero);
        }

        /// <summary>
        /// Loads a surface from a file.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <param name="destinationRectangle">The destination rectangle.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT D3DXLoadSurfaceFromFileW([In] IDirect3DSurface9* pDestSurface,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestRect,[In] const wchar_t* pSrcFile,[In] const void* pSrcRect,[In] D3DX_FILTER Filter,[In] int ColorKey,[In] void* pSrcInfo)</unmanaged>
        public static void FromFile(Surface surface, string fileName, Filter filter, int colorKey, RawRectangle sourceRectangle, RawRectangle destinationRectangle)
        {
            unsafe
            {
                D3DX9.LoadSurfaceFromFileW(
                    surface, null, new IntPtr(&destinationRectangle), fileName, new IntPtr(&sourceRectangle), filter, colorKey, IntPtr.Zero);
            }
        }

        /// <summary>
        /// Loads a surface from a file.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <param name="destinationRectangle">The destination rectangle.</param>
        /// <param name="imageInformation">The image information.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT D3DXLoadSurfaceFromFileW([In] IDirect3DSurface9* pDestSurface,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestRect,[In] const wchar_t* pSrcFile,[In] const void* pSrcRect,[In] D3DX_FILTER Filter,[In] int ColorKey,[In] void* pSrcInfo)</unmanaged>
        public static void FromFile(
            Surface surface,
            string fileName,
            Filter filter,
            int colorKey,
            RawRectangle sourceRectangle,
            RawRectangle destinationRectangle,
            out ImageInformation imageInformation)
        {
            FromFile(surface, fileName, filter, colorKey, sourceRectangle, destinationRectangle, null, out imageInformation);
        }

        /// <summary>
        /// Loads a surface from a file.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <param name="destinationRectangle">The destination rectangle.</param>
        /// <param name="palette">The palette.</param>
        /// <param name="imageInformation">The image information.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT D3DXLoadSurfaceFromFileW([In] IDirect3DSurface9* pDestSurface,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestRect,[In] const wchar_t* pSrcFile,[In] const void* pSrcRect,[In] D3DX_FILTER Filter,[In] int ColorKey,[In] void* pSrcInfo)</unmanaged>
        public static void FromFile(
            Surface surface,
            string fileName,
            Filter filter,
            int colorKey,
            RawRectangle sourceRectangle,
            RawRectangle destinationRectangle,
            PaletteEntry[] palette,
            out ImageInformation imageInformation)
        {
            unsafe
            {
                fixed (void* pImageInformation = &imageInformation)
                    D3DX9.LoadSurfaceFromFileW(
                        surface, palette, new IntPtr(&destinationRectangle), fileName, new IntPtr(&sourceRectangle), filter, colorKey, (IntPtr)pImageInformation);
            }
        }

        /// <summary>
        /// Loads a surface from a file in memory.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="memory">The memory.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT D3DXLoadSurfaceFromFileInMemory([In] IDirect3DSurface9* pDestSurface,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestRect,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] const void* pSrcRect,[In] D3DX_FILTER Filter,[In] int ColorKey,[In] void* pSrcInfo)</unmanaged>
        public static void FromFileInMemory(Surface surface, byte[] memory, Filter filter, int colorKey)
        {
            unsafe
            {
                fixed (void* pMemory = memory)
                    D3DX9.LoadSurfaceFromFileInMemory(
                        surface, null, IntPtr.Zero, (IntPtr)pMemory, memory.Length, IntPtr.Zero, filter, colorKey, IntPtr.Zero);
            }
        }

        /// <summary>
        /// Loads a surface from a file in memory.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="memory">The memory.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <param name="destinationRectangle">The destination rectangle.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT D3DXLoadSurfaceFromFileInMemory([In] IDirect3DSurface9* pDestSurface,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestRect,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] const void* pSrcRect,[In] D3DX_FILTER Filter,[In] int ColorKey,[In] void* pSrcInfo)</unmanaged>
        public static void FromFileInMemory(
            Surface surface, byte[] memory, Filter filter, int colorKey, RawRectangle sourceRectangle, RawRectangle destinationRectangle)
        {
            unsafe
            {
                fixed (void* pMemory = memory)
                    D3DX9.LoadSurfaceFromFileInMemory(
                        surface,
                        null,
                        new IntPtr(&destinationRectangle),
                        (IntPtr)pMemory,
                        memory.Length,
                        new IntPtr(&sourceRectangle),
                        filter,
                        colorKey,
                        IntPtr.Zero);
            }
        }

        /// <summary>
        /// Loads a surface from a file in memory.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="memory">The memory.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <param name="destinationRectangle">The destination rectangle.</param>
        /// <param name="imageInformation">The image information.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT D3DXLoadSurfaceFromFileInMemory([In] IDirect3DSurface9* pDestSurface,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestRect,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] const void* pSrcRect,[In] D3DX_FILTER Filter,[In] int ColorKey,[In] void* pSrcInfo)</unmanaged>
        public static void FromFileInMemory(
            Surface surface,
            byte[] memory,
            Filter filter,
            int colorKey,
            RawRectangle sourceRectangle,
            RawRectangle destinationRectangle,
            out ImageInformation imageInformation)
        {
            FromFileInMemory(surface, memory, filter, colorKey, sourceRectangle, destinationRectangle, null, out imageInformation);
        }

        /// <summary>
        /// Loads a surface from a file in memory.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="memory">The memory.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <param name="destinationRectangle">The destination rectangle.</param>
        /// <param name="palette">The palette.</param>
        /// <param name="imageInformation">The image information.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT D3DXLoadSurfaceFromFileInMemory([In] IDirect3DSurface9* pDestSurface,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestRect,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] const void* pSrcRect,[In] D3DX_FILTER Filter,[In] int ColorKey,[In] void* pSrcInfo)</unmanaged>
        public static void FromFileInMemory(
            Surface surface,
            byte[] memory,
            Filter filter,
            int colorKey,
            RawRectangle sourceRectangle,
            RawRectangle destinationRectangle,
            PaletteEntry[] palette,
            out ImageInformation imageInformation)
        {
            unsafe
            {
                fixed (void* pMemory = memory)
                fixed (void* pImageInformation = &imageInformation)
                    D3DX9.LoadSurfaceFromFileInMemory(
                        surface,
                        palette,
                        new IntPtr(&destinationRectangle),
                        (IntPtr)pMemory,
                        memory.Length,
                        new IntPtr(&sourceRectangle),
                        filter,
                        colorKey,
                        (IntPtr)pImageInformation);
            }
        }

        /// <summary>
        /// Loads a surface from a file in memory.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXLoadSurfaceFromFileInMemory([In] IDirect3DSurface9* pDestSurface,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestRect,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] const void* pSrcRect,[In] D3DX_FILTER Filter,[In] int ColorKey,[In] void* pSrcInfo)</unmanaged>
        public static void FromFileInStream(Surface surface, Stream stream, Filter filter, int colorKey)
        {
            if (stream is DataStream)
            {
                D3DX9.LoadSurfaceFromFileInMemory(
                    surface, null, IntPtr.Zero, ((DataStream)stream).PositionPointer, (int)(stream.Length - stream.Position), IntPtr.Zero, filter, colorKey, IntPtr.Zero);
            }
            else
            {
                unsafe
                {
                    var data = Utilities.ReadStream(stream);
                    fixed (void* pData = data)
                        D3DX9.LoadSurfaceFromFileInMemory(surface, null, IntPtr.Zero, (IntPtr)pData, data.Length, IntPtr.Zero, filter, colorKey, IntPtr.Zero);
                }
            }
        }

        /// <summary>
        /// Loads a surface from a file in memory.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <param name="destinationRectangle">The destination rectangle.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXLoadSurfaceFromFileInMemory([In] IDirect3DSurface9* pDestSurface,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestRect,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] const void* pSrcRect,[In] D3DX_FILTER Filter,[In] int ColorKey,[In] void* pSrcInfo)</unmanaged>
        public static void FromFileInStream(
            Surface surface, Stream stream, Filter filter, int colorKey, RawRectangle sourceRectangle, RawRectangle destinationRectangle)
        {
            CreateFromFileInStream(surface, stream, filter, colorKey, sourceRectangle, destinationRectangle, null, IntPtr.Zero);
        }

        /// <summary>
        /// Loads a surface from a file in memory.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <param name="destinationRectangle">The destination rectangle.</param>
        /// <param name="imageInformation">The image information.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXLoadSurfaceFromFileInMemory([In] IDirect3DSurface9* pDestSurface,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestRect,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] const void* pSrcRect,[In] D3DX_FILTER Filter,[In] int ColorKey,[In] void* pSrcInfo)</unmanaged>
        public static void FromFileInStream(
            Surface surface,
            Stream stream,
            Filter filter,
            int colorKey,
            RawRectangle sourceRectangle,
            RawRectangle destinationRectangle,
            out ImageInformation imageInformation)
        {
            FromFileInStream(surface, stream, filter, colorKey, sourceRectangle, destinationRectangle, null, out imageInformation);
        }

        /// <summary>
        /// Loads a surface from a file in memory.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <param name="destinationRectangle">The destination rectangle.</param>
        /// <param name="palette">The palette.</param>
        /// <param name="imageInformation">The image information.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXLoadSurfaceFromFileInMemory([In] IDirect3DSurface9* pDestSurface,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestRect,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] const void* pSrcRect,[In] D3DX_FILTER Filter,[In] int ColorKey,[In] void* pSrcInfo)</unmanaged>
        public static unsafe void FromFileInStream(
            Surface surface,
            Stream stream,
            Filter filter,
            int colorKey,
            RawRectangle sourceRectangle,
            RawRectangle destinationRectangle,
            PaletteEntry[] palette,
            out ImageInformation imageInformation)
        {
            fixed (void* pImageInformation = &imageInformation)
                CreateFromFileInStream(surface, stream, filter, colorKey, sourceRectangle, destinationRectangle, palette, (IntPtr)pImageInformation);
        }

        /// <summary>
        /// Loads a surface from a file in memory.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <param name="destinationRectangle">The destination rectangle.</param>
        /// <param name="palette">The palette.</param>
        /// <param name="imageInformation">The image information.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT D3DXLoadSurfaceFromFileInMemory([In] IDirect3DSurface9* pDestSurface,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestRect,[In] const void* pSrcData,[In] unsigned int SrcDataSize,[In] const void* pSrcRect,[In] D3DX_FILTER Filter,[In] int ColorKey,[In] void* pSrcInfo)</unmanaged>
        private static unsafe void CreateFromFileInStream(
            Surface surface,
            Stream stream,
            Filter filter,
            int colorKey,
            RawRectangle sourceRectangle,
            RawRectangle destinationRectangle,
            PaletteEntry[] palette,
            IntPtr imageInformation)
        {
            if (stream is DataStream)
            {
                D3DX9.LoadSurfaceFromFileInMemory(
                    surface,
                    palette,
                    new IntPtr(&destinationRectangle),
                    ((DataStream)stream).PositionPointer,
                    (int)(stream.Length - stream.Position),
                    new IntPtr(&sourceRectangle),
                    filter,
                    colorKey,
                    imageInformation);
            }
            else
            {
                var data = Utilities.ReadStream(stream);
                fixed (void* pData = data)
                    D3DX9.LoadSurfaceFromFileInMemory(
                        surface,
                        palette,
                        new IntPtr(&destinationRectangle),
                        (IntPtr)pData,
                        data.Length,
                        new IntPtr(&sourceRectangle),
                        filter,
                        colorKey,
                        imageInformation);
            }
        }

        /// <summary>
        /// Loads a surface from memory.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="data">The data.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="sourceFormat">The source format.</param>
        /// <param name="sourcePitch">The source pitch.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXLoadSurfaceFromMemory([In] IDirect3DSurface9* pDestSurface,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestRect,[In] const void* pSrcMemory,[In] D3DFORMAT SrcFormat,[In] unsigned int SrcPitch,[In, Buffer] const PALETTEENTRY* pSrcPalette,[In] const void* pSrcRect,[In] D3DX_FILTER Filter,[In] int ColorKey)</unmanaged>
        public static void FromMemory(
            Surface surface, byte[] data, Filter filter, int colorKey, Format sourceFormat, int sourcePitch, RawRectangle sourceRectangle)
        {
            FromMemory(surface, data, filter, colorKey, sourceFormat, sourcePitch, sourceRectangle, null, null);
        }

        /// <summary>
        /// Loads a surface from memory.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="data">The data.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="sourceFormat">The source format.</param>
        /// <param name="sourcePitch">The source pitch.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <param name="destinationRectangle">The destination rectangle.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXLoadSurfaceFromMemory([In] IDirect3DSurface9* pDestSurface,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestRect,[In] const void* pSrcMemory,[In] D3DFORMAT SrcFormat,[In] unsigned int SrcPitch,[In, Buffer] const PALETTEENTRY* pSrcPalette,[In] const void* pSrcRect,[In] D3DX_FILTER Filter,[In] int ColorKey)</unmanaged>
        public static void FromMemory(
            Surface surface,
            byte[] data,
            Filter filter,
            int colorKey,
            Format sourceFormat,
            int sourcePitch,
            RawRectangle sourceRectangle,
            RawRectangle destinationRectangle)
        {
            FromMemory(surface, data, filter, colorKey, sourceFormat, sourcePitch, sourceRectangle, destinationRectangle, null, null);
        }

        /// <summary>
        /// Loads a surface from memory.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="data">The data.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="sourceFormat">The source format.</param>
        /// <param name="sourcePitch">The source pitch.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <param name="sourcePalette">The source palette.</param>
        /// <param name="destinationPalette">The destination palette.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXLoadSurfaceFromMemory([In] IDirect3DSurface9* pDestSurface,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestRect,[In] const void* pSrcMemory,[In] D3DFORMAT SrcFormat,[In] unsigned int SrcPitch,[In, Buffer] const PALETTEENTRY* pSrcPalette,[In] const void* pSrcRect,[In] D3DX_FILTER Filter,[In] int ColorKey)</unmanaged>
        public static void FromMemory(
            Surface surface,
            byte[] data,
            Filter filter,
            int colorKey,
            Format sourceFormat,
            int sourcePitch,
            RawRectangle sourceRectangle,
            PaletteEntry[] sourcePalette,
            PaletteEntry[] destinationPalette)
        {
            unsafe
            {
                fixed (void* pData = data)
                    D3DX9.LoadSurfaceFromMemory(
                        surface,
                        destinationPalette,
                        IntPtr.Zero,
                        (IntPtr)pData,
                        sourceFormat,
                        sourcePitch,
                        sourcePalette,
                        new IntPtr(&sourceRectangle),
                        filter,
                        colorKey);
            }
        }

        /// <summary>
        /// Loads a surface from memory.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="data">The data.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="sourceFormat">The source format.</param>
        /// <param name="sourcePitch">The source pitch.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <param name="destinationRectangle">The destination rectangle.</param>
        /// <param name="sourcePalette">The source palette.</param>
        /// <param name="destinationPalette">The destination palette.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT D3DXLoadSurfaceFromMemory([In] IDirect3DSurface9* pDestSurface,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestRect,[In] const void* pSrcMemory,[In] D3DFORMAT SrcFormat,[In] unsigned int SrcPitch,[In, Buffer] const PALETTEENTRY* pSrcPalette,[In] const void* pSrcRect,[In] D3DX_FILTER Filter,[In] int ColorKey)</unmanaged>
        public static void FromMemory(
            Surface surface,
            byte[] data,
            Filter filter,
            int colorKey,
            Format sourceFormat,
            int sourcePitch,
            RawRectangle sourceRectangle,
            RawRectangle destinationRectangle,
            PaletteEntry[] sourcePalette,
            PaletteEntry[] destinationPalette)
        {
            unsafe
            {
                fixed (void* pData = data)
                    D3DX9.LoadSurfaceFromMemory(
                        surface,
                        destinationPalette,
                        new IntPtr(&destinationRectangle),
                        (IntPtr)pData,
                        sourceFormat,
                        sourcePitch,
                        sourcePalette,
                        new IntPtr(&sourceRectangle),
                        filter,
                        colorKey);
            }
        }

        /// <summary>
        /// Loads a surface from memory.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="sourceFormat">The source format.</param>
        /// <param name="sourcePitch">The source pitch.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXLoadSurfaceFromMemory([In] IDirect3DSurface9* pDestSurface,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestRect,[In] const void* pSrcMemory,[In] D3DFORMAT SrcFormat,[In] unsigned int SrcPitch,[In, Buffer] const PALETTEENTRY* pSrcPalette,[In] const void* pSrcRect,[In] D3DX_FILTER Filter,[In] int ColorKey)</unmanaged>
        public static void FromStream(
            Surface surface, Stream stream, Filter filter, int colorKey, Format sourceFormat, int sourcePitch, RawRectangle sourceRectangle)
        {
            FromStream(surface, stream, filter, colorKey, sourceFormat, sourcePitch, sourceRectangle, null, null);
        }

        /// <summary>
        /// Loads a surface from memory.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="sourceFormat">The source format.</param>
        /// <param name="sourcePitch">The source pitch.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <param name="destinationRectangle">The destination rectangle.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXLoadSurfaceFromMemory([In] IDirect3DSurface9* pDestSurface,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestRect,[In] const void* pSrcMemory,[In] D3DFORMAT SrcFormat,[In] unsigned int SrcPitch,[In, Buffer] const PALETTEENTRY* pSrcPalette,[In] const void* pSrcRect,[In] D3DX_FILTER Filter,[In] int ColorKey)</unmanaged>
        public static void FromStream(
            Surface surface,
            Stream stream,
            Filter filter,
            int colorKey,
            Format sourceFormat,
            int sourcePitch,
            RawRectangle sourceRectangle,
            RawRectangle destinationRectangle)
        {
            FromStream(surface, stream, filter, colorKey, sourceFormat, sourcePitch, sourceRectangle, destinationRectangle, null, null);
        }

        /// <summary>
        /// Loads a surface from memory.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="sourceFormat">The source format.</param>
        /// <param name="sourcePitch">The source pitch.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <param name="sourcePalette">The source palette.</param>
        /// <param name="destinationPalette">The destination palette.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXLoadSurfaceFromMemory([In] IDirect3DSurface9* pDestSurface,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestRect,[In] const void* pSrcMemory,[In] D3DFORMAT SrcFormat,[In] unsigned int SrcPitch,[In, Buffer] const PALETTEENTRY* pSrcPalette,[In] const void* pSrcRect,[In] D3DX_FILTER Filter,[In] int ColorKey)</unmanaged>
        public static void FromStream(
            Surface surface,
            Stream stream,
            Filter filter,
            int colorKey,
            Format sourceFormat,
            int sourcePitch,
            RawRectangle sourceRectangle,
            PaletteEntry[] sourcePalette,
            PaletteEntry[] destinationPalette)
        {
            unsafe
            {
                if (stream is DataStream)
                {
                    D3DX9.LoadSurfaceFromMemory(
                        surface,
                        destinationPalette,
                        IntPtr.Zero,
                        ((DataStream)stream).PositionPointer,
                        sourceFormat,
                        sourcePitch,
                        sourcePalette,
                        new IntPtr(&sourceRectangle),
                        filter,
                        colorKey);
                }
                else
                {
                    var data = Utilities.ReadStream(stream);
                    fixed (void* pData = data)
                        D3DX9.LoadSurfaceFromMemory(
                            surface,
                            destinationPalette,
                            IntPtr.Zero,
                            (IntPtr)pData,
                            sourceFormat,
                            sourcePitch,
                            sourcePalette,
                            new IntPtr(&sourceRectangle),
                            filter,
                            colorKey);
                }
            }
        }

        /// <summary>
        /// Loads a surface from memory.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="sourceFormat">The source format.</param>
        /// <param name="sourcePitch">The source pitch.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <param name="destinationRectangle">The destination rectangle.</param>
        /// <param name="sourcePalette">The source palette.</param>
        /// <param name="destinationPalette">The destination palette.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXLoadSurfaceFromMemory([In] IDirect3DSurface9* pDestSurface,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestRect,[In] const void* pSrcMemory,[In] D3DFORMAT SrcFormat,[In] unsigned int SrcPitch,[In, Buffer] const PALETTEENTRY* pSrcPalette,[In] const void* pSrcRect,[In] D3DX_FILTER Filter,[In] int ColorKey)</unmanaged>
        public static void FromStream(
            Surface surface,
            Stream stream,
            Filter filter,
            int colorKey,
            Format sourceFormat,
            int sourcePitch,
            RawRectangle sourceRectangle,
            RawRectangle destinationRectangle,
            PaletteEntry[] sourcePalette,
            PaletteEntry[] destinationPalette)
        {
            unsafe
            {
                if (stream is DataStream)
                {
                    D3DX9.LoadSurfaceFromMemory(
                        surface,
                        destinationPalette,
                        new IntPtr(&destinationRectangle),
                        ((DataStream)stream).PositionPointer,
                        sourceFormat,
                        sourcePitch,
                        sourcePalette,
                        new IntPtr(&sourceRectangle),
                        filter,
                        colorKey);
                }
                else
                {
                    var data = Utilities.ReadStream(stream);
                    fixed (void* pData = data)
                        D3DX9.LoadSurfaceFromMemory(
                            surface,
                            destinationPalette,
                            new IntPtr(&destinationRectangle),
                            (IntPtr)pData,
                            sourceFormat,
                            sourcePitch,
                            sourcePalette,
                            new IntPtr(&sourceRectangle),
                            filter,
                            colorKey);
                }
            }
        }

        /// <summary>
        /// Loads a surface from a source surface.
        /// </summary>
        /// <param name="destinationSurface">The destination surface.</param>
        /// <param name="sourceSurface">The source surface.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXLoadSurfaceFromSurface([In] IDirect3DSurface9* pDestSurface,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestRect,[In] IDirect3DSurface9* pSrcSurface,[In, Buffer] const PALETTEENTRY* pSrcPalette,[In] const void* pSrcRect,[In] D3DX_FILTER Filter,[In] int ColorKey)</unmanaged>
        public static void FromSurface(Surface destinationSurface, Surface sourceSurface, Filter filter, int colorKey)
        {
            D3DX9.LoadSurfaceFromSurface(destinationSurface, null, IntPtr.Zero, sourceSurface, null, IntPtr.Zero, filter, colorKey);
        }

        /// <summary>
        /// Loads a surface from a source surface.
        /// </summary>
        /// <param name="destinationSurface">The destination surface.</param>
        /// <param name="sourceSurface">The source surface.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <param name="destinationRectangle">The destination rectangle.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXLoadSurfaceFromSurface([In] IDirect3DSurface9* pDestSurface,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestRect,[In] IDirect3DSurface9* pSrcSurface,[In, Buffer] const PALETTEENTRY* pSrcPalette,[In] const void* pSrcRect,[In] D3DX_FILTER Filter,[In] int ColorKey)</unmanaged>
        public static void FromSurface(
            Surface destinationSurface, Surface sourceSurface, Filter filter, int colorKey, RawRectangle sourceRectangle, RawRectangle destinationRectangle)
        {
            FromSurface(destinationSurface, sourceSurface, filter, colorKey, sourceRectangle, destinationRectangle, null, null);
        }

        /// <summary>
        /// Loads a surface from a source surface.
        /// </summary>
        /// <param name="destinationSurface">The destination surface.</param>
        /// <param name="sourceSurface">The source surface.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="colorKey">The color key.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <param name="destinationRectangle">The destination rectangle.</param>
        /// <param name="destinationPalette">The destination palette.</param>
        /// <param name="sourcePalette">The source palette.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXLoadSurfaceFromSurface([In] IDirect3DSurface9* pDestSurface,[Out, Buffer] const PALETTEENTRY* pDestPalette,[In] const void* pDestRect,[In] IDirect3DSurface9* pSrcSurface,[In, Buffer] const PALETTEENTRY* pSrcPalette,[In] const void* pSrcRect,[In] D3DX_FILTER Filter,[In] int ColorKey)</unmanaged>
        public static void FromSurface(
            Surface destinationSurface,
            Surface sourceSurface,
            Filter filter,
            int colorKey,
            RawRectangle sourceRectangle,
            RawRectangle destinationRectangle,
            PaletteEntry[] destinationPalette,
            PaletteEntry[] sourcePalette)
        {
            unsafe
            {
                D3DX9.LoadSurfaceFromSurface(
                    destinationSurface,
                    destinationPalette,
                    new IntPtr(&destinationRectangle),
                    sourceSurface,
                    sourcePalette,
                    new IntPtr(&sourceRectangle),
                    filter,
                    colorKey);
            }
        }

        /// <summary>
        /// Gets the parent cube texture or texture (mipmap) object, if this surface is a child level of a cube texture or a mipmap. 
        /// This method can also provide access to the parent swap chain if the surface is a back-buffer child.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="guid">The GUID.</param>
        /// <returns>The parent container texture.</returns>
        public T GetContainer<T>(Guid guid) where T : ComObject
        {
            IntPtr containerPtr;
            GetContainer(guid, out containerPtr);
            return FromPointer<T>(containerPtr);
        }

        /// <summary>
        /// Locks a rectangle on a surface.
        /// </summary>
        /// <param name="flags">The type of lock to perform.</param>
        /// <returns>A pointer to the locked region</returns>
        /// <unmanaged>HRESULT IDirect3DSurface9::LockRect([Out] D3DLOCKED_RECT* pLockedRect,[In] const void* pRect,[In] D3DLOCK Flags)</unmanaged>
        public DataRectangle LockRectangle(LockFlags flags)
        {
            LockedRectangle lockedRect;
            LockRectangle(out lockedRect, IntPtr.Zero, flags);
            return new DataRectangle(lockedRect.PBits, lockedRect.Pitch);
        }

        /// <summary>
        /// Locks a rectangle on a surface.
        /// </summary>
        /// <param name="rect">The rectangle to lock.</param>
        /// <param name="flags">The type of lock to perform.</param>
        /// <returns>A pointer to the locked region</returns>
        /// <unmanaged>HRESULT IDirect3DSurface9::LockRect([Out] D3DLOCKED_RECT* pLockedRect,[In] const void* pRect,[In] D3DLOCK Flags)</unmanaged>
        public DataRectangle LockRectangle(RawRectangle rect, LockFlags flags)
        {
            unsafe
            {
                LockedRectangle lockedRect;
                LockRectangle(out lockedRect, new IntPtr(&rect), flags);
                return new DataRectangle(lockedRect.PBits, lockedRect.Pitch);
            }
        }

        /// <summary>
        /// Locks a rectangle on a surface.
        /// </summary>
        /// <param name="flags">The type of lock to perform.</param>
        /// <param name="stream">The stream pointing to the locked region.</param>
        /// <returns>A pointer to the locked region</returns>
        /// <unmanaged>HRESULT IDirect3DSurface9::LockRect([Out] D3DLOCKED_RECT* pLockedRect,[In] const void* pRect,[In] D3DLOCK Flags)</unmanaged>
        public DataRectangle LockRectangle(LockFlags flags, out DataStream stream)
        {
            LockedRectangle lockedRect;
            LockRectangle(out lockedRect, IntPtr.Zero, flags);
            stream = new DataStream(lockedRect.PBits, lockedRect.Pitch * Description.Height, true, (flags & LockFlags.ReadOnly) == 0);
            return new DataRectangle(lockedRect.PBits, lockedRect.Pitch);
        }

        /// <summary>
        /// Locks a rectangle on a surface.
        /// </summary>
        /// <param name="rect">The rectangle to lock.</param>
        /// <param name="flags">The type of lock to perform.</param>
        /// <param name="stream">The stream pointing to the locked region.</param>
        /// <returns>A pointer to the locked region</returns>
        /// <unmanaged>HRESULT IDirect3DSurface9::LockRect([Out] D3DLOCKED_RECT* pLockedRect,[In] const void* pRect,[In] D3DLOCK Flags)</unmanaged>
        public DataRectangle LockRectangle(RawRectangle rect, LockFlags flags, out DataStream stream)
        {
            unsafe
            {
                LockedRectangle lockedRect;
                LockRectangle(out lockedRect, new IntPtr(&rect), flags);
                stream = new DataStream(lockedRect.PBits, lockedRect.Pitch * Description.Height, true, (flags & LockFlags.ReadOnly) == 0);
                return new DataRectangle(lockedRect.PBits, lockedRect.Pitch);
            }
        }

        /// <summary>
        /// Saves a surface to a file.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXSaveSurfaceToFileW([In] const wchar_t* pDestFile,[In] D3DXIMAGE_FILEFORMAT DestFormat,[In] IDirect3DSurface9* pSrcSurface,[In, Buffer] const PALETTEENTRY* pSrcPalette,[In] const void* pSrcRect)</unmanaged>
        public static void ToFile(Surface surface, string fileName, ImageFileFormat format)
        {
            D3DX9.SaveSurfaceToFileW(fileName, format, surface, null, IntPtr.Zero);            
        }

        /// <summary>
        /// Saves a surface to a file.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="format">The format.</param>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXSaveSurfaceToFileW([In] const wchar_t* pDestFile,[In] D3DXIMAGE_FILEFORMAT DestFormat,[In] IDirect3DSurface9* pSrcSurface,[In, Buffer] const PALETTEENTRY* pSrcPalette,[In] const void* pSrcRect)</unmanaged>
        public static void ToFile(Surface surface, string fileName, ImageFileFormat format, RawRectangle rectangle)
        {
            ToFile(surface, fileName, format, rectangle, null);
        }

        /// <summary>
        /// Saves a surface to a file.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="format">The format.</param>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="palette">The palette.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXSaveSurfaceToFileW([In] const wchar_t* pDestFile,[In] D3DXIMAGE_FILEFORMAT DestFormat,[In] IDirect3DSurface9* pSrcSurface,[In, Buffer] const PALETTEENTRY* pSrcPalette,[In] const void* pSrcRect)</unmanaged>
        public static void ToFile(Surface surface, string fileName, ImageFileFormat format, RawRectangle rectangle, PaletteEntry[] palette)
        {
            unsafe
            {
                D3DX9.SaveSurfaceToFileW(fileName, format, surface, palette, new IntPtr(&rectangle));
            }            
        }

        /// <summary>
        /// Saves a surface to a stream.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXSaveSurfaceToFileInMemory([In] ID3DXBuffer** ppDestBuf,[In] D3DXIMAGE_FILEFORMAT DestFormat,[In] IDirect3DSurface9* pSrcSurface,[In, Buffer] const PALETTEENTRY* pSrcPalette,[In] const void* pSrcRect)</unmanaged>
        public static DataStream ToStream(Surface surface, ImageFileFormat format)
        {
            Blob blob;
            D3DX9.SaveSurfaceToFileInMemory(out blob, format, surface, null, IntPtr.Zero);
            return new DataStream(blob);
        }

        /// <summary>
        /// Saves a surface to a stream.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="format">The format.</param>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXSaveSurfaceToFileInMemory([In] ID3DXBuffer** ppDestBuf,[In] D3DXIMAGE_FILEFORMAT DestFormat,[In] IDirect3DSurface9* pSrcSurface,[In, Buffer] const PALETTEENTRY* pSrcPalette,[In] const void* pSrcRect)</unmanaged>
        public static DataStream ToStream(Surface surface, ImageFileFormat format, RawRectangle rectangle)
        {
            return ToStream(surface, format, rectangle, null);
        }

        /// <summary>
        /// Saves a surface to a stream.
        /// </summary>
        /// <param name="surface">The surface.</param>
        /// <param name="format">The format.</param>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="palette">The palette.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT D3DXSaveSurfaceToFileInMemory([In] ID3DXBuffer** ppDestBuf,[In] D3DXIMAGE_FILEFORMAT DestFormat,[In] IDirect3DSurface9* pSrcSurface,[In, Buffer] const PALETTEENTRY* pSrcPalette,[In] const void* pSrcRect)</unmanaged>
        public static DataStream ToStream(Surface surface, ImageFileFormat format, RawRectangle rectangle, PaletteEntry[] palette)
        {
            unsafe
            {
                Blob blob;
                D3DX9.SaveSurfaceToFileInMemory(out blob, format, surface, palette, new IntPtr(&rectangle));
                return new DataStream(blob);
            }
        }
    }
}