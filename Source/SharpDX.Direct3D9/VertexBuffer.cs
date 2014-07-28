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
    public partial class VertexBuffer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SharpDX.Direct3D9.VertexBuffer" /> class.
        /// </summary>
        /// <param name="device">The device that will be used to create the buffer.</param>
        /// <param name="sizeInBytes">Size of the buffer, in bytes.</param>
        /// <param name="usage">The requested usage of the buffer.</param>
        /// <param name="format">The vertex format of the vertices in the buffer. If set to <see cref="SharpDX.Direct3D9.VertexFormat" />.None, the buffer will be a non-FVF buffer.</param>
        /// <param name="pool">The memory class into which the resource will be placed.</param>
        /// <msdn-id>bb174364</msdn-id>	
        /// <unmanaged>HRESULT IDirect3DDevice9::CreateVertexBuffer([In] unsigned int Length,[In] D3DUSAGE Usage,[In] D3DFVF FVF,[In] D3DPOOL Pool,[Out, Fast] IDirect3DVertexBuffer9** ppVertexBuffer,[In] void** pSharedHandle)</unmanaged>	
        /// <unmanaged-short>IDirect3DDevice9::CreateVertexBuffer</unmanaged-short>	
        public VertexBuffer(Device device, int sizeInBytes, Usage usage, VertexFormat format, Pool pool)
            : base(IntPtr.Zero)
        {
            device.CreateVertexBuffer(sizeInBytes, usage, format, pool, this, IntPtr.Zero);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SharpDX.Direct3D9.VertexBuffer" /> class.
        /// </summary>
        /// <param name="device">The device that will be used to create the buffer.</param>
        /// <param name="sizeInBytes">Size of the buffer, in bytes.</param>
        /// <param name="usage">The requested usage of the buffer.</param>
        /// <param name="format">The vertex format of the vertices in the buffer. If set to <see cref="SharpDX.Direct3D9.VertexFormat" />.None, the buffer will be a non-FVF buffer.</param>
        /// <param name="pool">The memory class into which the resource will be placed.</param>
        /// <param name="sharedHandle">The variable that will receive the shared handle for this resource.</param>
        /// <remarks>This method is only available in Direct3D9 Ex.</remarks>
        /// <msdn-id>bb174364</msdn-id>	
        /// <unmanaged>HRESULT IDirect3DDevice9::CreateVertexBuffer([In] unsigned int Length,[In] D3DUSAGE Usage,[In] D3DFVF FVF,[In] D3DPOOL Pool,[Out, Fast] IDirect3DVertexBuffer9** ppVertexBuffer,[In] void** pSharedHandle)</unmanaged>	
        /// <unmanaged-short>IDirect3DDevice9::CreateVertexBuffer</unmanaged-short>	
        public VertexBuffer(Device device, int sizeInBytes, Usage usage, VertexFormat format, Pool pool, ref IntPtr sharedHandle)
            : base(IntPtr.Zero)
        {
            unsafe
            {
                sharedHandle = IntPtr.Zero;
                fixed (void* pSharedHandle = &sharedHandle)
                    device.CreateVertexBuffer(sizeInBytes, usage, format, pool, this, new IntPtr(pSharedHandle));
            }
        }

        /// <summary>	
        /// Locks a range of vertex data and obtains a pointer to the vertex buffer memory.	
        /// </summary>	
        /// <remarks>	
        ///  As a general rule, do not hold a lock across more than one frame. When working with vertex buffers, you are allowed to make multiple lock calls; however, you must ensure that the number of lock calls match the number of unlock calls. DrawPrimitive calls will not succeed with any outstanding lock count on any currently set vertex buffer. The D3DLOCK_DISCARD and D3DLOCK_NOOVERWRITE flags are valid only on buffers created with D3DUSAGE_DYNAMIC. For information about using D3DLOCK_DISCARD or D3DLOCK_NOOVERWRITE with IDirect3DVertexBuffer9::Lock, see {{Using Dynamic Vertex and Index Buffers}}. 	
        /// </remarks>	
        /// <param name="offsetToLock"> Offset into the vertex data to lock, in bytes. To lock the entire vertex buffer, specify 0 for both parameters, SizeToLock and OffsetToLock. </param>
        /// <param name="sizeToLock"> Size of the vertex data to lock, in bytes. To lock the entire vertex buffer, specify 0 for both parameters, SizeToLock and OffsetToLock. </param>
        /// <param name="lockFlags"> Combination of zero or more locking flags that describe the type of lock to perform. For this method, the valid flags are:    D3DLOCK_DISCARD D3DLOCK_NO_DIRTY_UPDATE D3DLOCK_NOSYSLOCK D3DLOCK_READONLY D3DLOCK_NOOVERWRITE   For a description of the flags, see <see cref="SharpDX.Direct3D9.LockFlags"/>.  </param>
        /// <returns>A <see cref="SharpDX.DataStream"/> if the method succeeds.</returns>
        /// <msdn-id>bb205917</msdn-id>	
        /// <unmanaged>HRESULT IDirect3DVertexBuffer9::Lock([In] unsigned int OffsetToLock,[In] unsigned int SizeToLock,[Out] void** ppbData,[In] D3DLOCK Flags)</unmanaged>	
        /// <unmanaged-short>IDirect3DVertexBuffer9::Lock</unmanaged-short>	
        public DataStream Lock(int offsetToLock, int sizeToLock, SharpDX.Direct3D9.LockFlags lockFlags)
        {
            IntPtr bufferPointer;
            Lock_(offsetToLock, sizeToLock, out bufferPointer, lockFlags);
            if (sizeToLock == 0)
            {
                sizeToLock = Description.SizeInBytes;
            }
            return new DataStream(bufferPointer, sizeToLock, true, (lockFlags & LockFlags.ReadOnly) == 0 );
        }

        /// <summary>	
        /// Locks a range of vertex data and obtains a pointer to the vertex buffer memory.	
        /// </summary>	
        /// <remarks>	
        ///  As a general rule, do not hold a lock across more than one frame. When working with vertex buffers, you are allowed to make multiple lock calls; however, you must ensure that the number of lock calls match the number of unlock calls. DrawPrimitive calls will not succeed with any outstanding lock count on any currently set vertex buffer. The D3DLOCK_DISCARD and D3DLOCK_NOOVERWRITE flags are valid only on buffers created with D3DUSAGE_DYNAMIC. For information about using D3DLOCK_DISCARD or D3DLOCK_NOOVERWRITE with IDirect3DVertexBuffer9::Lock, see {{Using Dynamic Vertex and Index Buffers}}. 	
        /// </remarks>	
        /// <param name="offsetToLock"> Offset into the vertex data to lock, in bytes. To lock the entire vertex buffer, specify 0 for both parameters, SizeToLock and OffsetToLock. </param>
        /// <param name="sizeToLock"> Size of the vertex data to lock, in bytes. To lock the entire vertex buffer, specify 0 for both parameters, SizeToLock and OffsetToLock. </param>
        /// <param name="lockFlags"> Combination of zero or more locking flags that describe the type of lock to perform. For this method, the valid flags are:    D3DLOCK_DISCARD D3DLOCK_NO_DIRTY_UPDATE D3DLOCK_NOSYSLOCK D3DLOCK_READONLY D3DLOCK_NOOVERWRITE   For a description of the flags, see <see cref="SharpDX.Direct3D9.LockFlags"/>.  </param>
        /// <returns>A <see cref="System.IntPtr"/> if the method succeeds.</returns>
        /// <msdn-id>bb205917</msdn-id>	
        /// <unmanaged>HRESULT IDirect3DVertexBuffer9::Lock([In] unsigned int OffsetToLock,[In] unsigned int SizeToLock,[Out] void** ppbData,[In] D3DLOCK Flags)</unmanaged>	
        /// <unmanaged-short>IDirect3DVertexBuffer9::Lock</unmanaged-short>	
        public IntPtr LockToPointer(int offsetToLock, int sizeToLock, SharpDX.Direct3D9.LockFlags lockFlags)
        {
            IntPtr bufferPointer;
            if (sizeToLock == 0)
            {
                sizeToLock = Description.SizeInBytes;
            }
            Lock_(offsetToLock, sizeToLock, out bufferPointer, lockFlags);
            return bufferPointer;
        }
    }
}
