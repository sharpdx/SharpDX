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

namespace SharpDX.Direct3D9
{
    public partial class IndexBuffer
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexBuffer"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="sizeInBytes">The size in bytes.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="pool">The pool.</param>
        /// <param name="sixteenBit">if set to <c>true</c> use 16bit index buffer, otherwise, use 32bit index buffer.</param>
        /// <msdn-id>bb174357</msdn-id>	
        /// <unmanaged>HRESULT IDirect3DDevice9::CreateIndexBuffer([In] unsigned int Length,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[Out, Fast] IDirect3DIndexBuffer9** ppIndexBuffer,[In] void** pSharedHandle)</unmanaged>	
        /// <unmanaged-short>IDirect3DDevice9::CreateIndexBuffer</unmanaged-short>	
        public IndexBuffer(Device device, int sizeInBytes, Usage usage, Pool pool, bool sixteenBit)
            : base(IntPtr.Zero)
        {
            device.CreateIndexBuffer(sizeInBytes, (int)usage, sixteenBit ? Format.Index16 : Format.Index32, pool, this, IntPtr.Zero);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexBuffer"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="sizeInBytes">The size in bytes.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="pool">The pool.</param>
        /// <param name="sixteenBit">if set to <c>true</c> use 16bit index buffer, otherwise, use 32bit index buffer.</param>
        /// <param name="sharedHandle">The shared handle.</param>
        /// <msdn-id>bb174357</msdn-id>	
        /// <unmanaged>HRESULT IDirect3DDevice9::CreateIndexBuffer([In] unsigned int Length,[In] unsigned int Usage,[In] D3DFORMAT Format,[In] D3DPOOL Pool,[Out, Fast] IDirect3DIndexBuffer9** ppIndexBuffer,[In] void** pSharedHandle)</unmanaged>	
        /// <unmanaged-short>IDirect3DDevice9::CreateIndexBuffer</unmanaged-short>	
        public IndexBuffer(Device device, int sizeInBytes, Usage usage, Pool pool, bool sixteenBit, ref IntPtr sharedHandle)
            : base(IntPtr.Zero)
        {
            unsafe
            {
                fixed (void* pSharedHandle = &sharedHandle)
                    device.CreateIndexBuffer(sizeInBytes, (int)usage, sixteenBit ? Format.Index16 : Format.Index32, pool, this, (IntPtr)pSharedHandle);
            }
        }

        /// <summary>
        /// Locks the specified index buffer.
        /// </summary>
        /// <param name="offsetToLock">The offset in the buffer.</param>
        /// <param name="sizeToLock">The size of the buffer to lock.</param>
        /// <param name="lockFlags">The lock flags.</param>
        /// <returns>A <see cref="SharpDX.DataStream" /> containing the locked index buffer.</returns>
        /// <msdn-id>bb205867</msdn-id>	
        /// <unmanaged>HRESULT IDirect3DIndexBuffer9::Lock([In] unsigned int OffsetToLock,[In] unsigned int SizeToLock,[Out] void** ppbData,[In] D3DLOCK Flags)</unmanaged>	
        /// <unmanaged-short>IDirect3DIndexBuffer9::Lock</unmanaged-short>	
        public SharpDX.DataStream Lock(int offsetToLock, int sizeToLock, LockFlags lockFlags)
        {
            IntPtr pOut;
            if (sizeToLock == 0)
                sizeToLock = Description.Size;

            Lock(offsetToLock, sizeToLock, out pOut, lockFlags);

            return new DataStream(pOut, sizeToLock, true, (lockFlags & LockFlags.ReadOnly) == 0);
        }

        /// <summary>
        /// Locks the specified index buffer.
        /// </summary>
        /// <param name="offsetToLock">The offset in the buffer.</param>
        /// <param name="sizeToLock">The size of the buffer to lock.</param>
        /// <param name="lockFlags">The lock flags.</param>
        /// <returns>A <see cref="SharpDX.DataStream" /> containing the locked index buffer.</returns>
        /// <msdn-id>bb205867</msdn-id>	
        /// <unmanaged>HRESULT IDirect3DIndexBuffer9::Lock([In] unsigned int OffsetToLock,[In] unsigned int SizeToLock,[Out] void** ppbData,[In] D3DLOCK Flags)</unmanaged>	
        /// <unmanaged-short>IDirect3DIndexBuffer9::Lock</unmanaged-short>	
        public IntPtr LockToPointer(int offsetToLock, int sizeToLock, LockFlags lockFlags)
        {
            IntPtr pOut;
            if (sizeToLock == 0)
                sizeToLock = Description.Size;

            Lock(offsetToLock, sizeToLock, out pOut, lockFlags);

            return pOut;
        }
    }
}