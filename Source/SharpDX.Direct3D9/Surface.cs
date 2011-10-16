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
        public static Surface CreateDepthStencil(Device device, int width, int height, Format format, MultisampleType multisampleType, int multisampleQuality, bool discard)
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
        public static Surface CreateDepthStencil(Device device, int width, int height, Format format, MultisampleType multisampleType, int multisampleQuality, bool discard, out IntPtr sharedHandle)
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
        public static Surface CreateDepthStencilEx(DeviceEx device, int width, int height, Format format, MultisampleType multisampleType, int multisampleQuality, bool discard, Usage usage)
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
        public static Surface CreateDepthStencilEx(DeviceEx device, int width, int height, Format format, MultisampleType multisampleType, int multisampleQuality, bool discard, Usage usage, out IntPtr sharedHandle)
        {
            unsafe
            {
                fixed (void* pSharedHandle = &sharedHandle)
                    return device.CreateDepthStencilSurfaceEx(width, height, format, multisampleType, multisampleQuality, discard, (IntPtr)pSharedHandle, (int)usage);
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
        public static Surface CreateOffscreenPlain(Device device, int width, int height, Format format, Pool pool, out IntPtr sharedHandle)
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
        public static Surface CreateOffscreenPlainEx(DeviceEx device, int width, int height, Format format, Pool pool, Usage usage, out IntPtr sharedHandle)
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
        public static Surface CreateRenderTarget(Device device, int width, int height, Format format, MultisampleType multisampleType, int multisampleQuality, bool lockable)
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
        public static Surface CreateRenderTarget(Device device, int width, int height, Format format, MultisampleType multisampleType, int multisampleQuality, bool lockable, out IntPtr sharedHandle)
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
        public static Surface CreateRenderTargetEx(DeviceEx device, int width, int height, Format format, MultisampleType multisampleType, int multisampleQuality, bool lockable, Usage usage)
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
        public static Surface CreateRenderTargetEx(DeviceEx device, int width, int height, Format format, MultisampleType multisampleType, int multisampleQuality, bool lockable, Usage usage, out IntPtr sharedHandle)
        {
            unsafe
            {
                fixed (void* pSharedHandle = &sharedHandle)
                    return device.CreateRenderTargetEx(width, height, format, multisampleType, multisampleQuality, lockable, (IntPtr)pSharedHandle, (int)usage);
            }
        }
   
    }
}