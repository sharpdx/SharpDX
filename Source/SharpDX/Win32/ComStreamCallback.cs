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
using System.Runtime.InteropServices;

namespace SharpDX.Win32
{
    /// <summary>
    /// Internal FontFileEnumerator Callback
    /// </summary>
    internal class ComStreamCallback : ComStreamBaseCallback
    {
        /// <summary>
        /// Gets or sets the callback.
        /// </summary>
        /// <value>The callback.</value>
        private IStream Callback { get; set; }

        /// <summary>
        /// Callbacks to pointer.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static IntPtr CallbackToPtr(IStream stream)
        {
            return CallbackToPtr<IStream, ComStreamCallback>(stream);
        }

        public override void Attach<T>(T callback)
        {
            Attach(callback, 9);
            Callback = (IStream)callback;
            AddMethod(new SeekDelegate(SeekImpl));
            AddMethod(new SetSizeDelegate(SetSizeImpl));
            AddMethod(new CopyToDelegate(CopyToImpl));
            AddMethod(new CommitDelegate(CommitImpl));
            AddMethod(new RevertDelegate(RevertImpl));
            AddMethod(new LockRegionDelegate(LockRegionImpl));
            AddMethod(new UnlockRegionDelegate(UnlockRegionImpl));
            AddMethod(new StatDelegate(StatImpl));
            AddMethod(new CloneDelegate(CloneImpl));            
        }

        /// <unmanaged>HRESULT IStream::Seek([In] LARGE_INTEGER dlibMove,[In] SHARPDX_SEEKORIGIN dwOrigin,[Out, Optional] ULARGE_INTEGER* plibNewPosition)</unmanaged>	
        /* public long Seek(long dlibMove, System.IO.SeekOrigin dwOrigin) */
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int SeekDelegate(IntPtr thisPtr, long offset, SeekOrigin origin, out long newPosition);
        private int SeekImpl(IntPtr thisPtr, long offset, SeekOrigin origin, out long newPosition)
        {
            newPosition = 0;
            try
            {
                newPosition = Callback.Seek(offset, origin);
            }
            catch (SharpDXException exception)
            {
                return exception.ResultCode.Code;
            }
            catch (Exception)
            {
                return Result.Fail.Code;
            }
            return Result.Ok.Code;
        }

        /// <unmanaged>HRESULT IStream::SetSize([In] ULARGE_INTEGER libNewSize)</unmanaged>	
        /* public SharpDX.Result SetSize(long libNewSize) */
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Result SetSizeDelegate(IntPtr thisPtr, long newSize);
        private Result SetSizeImpl(IntPtr thisPtr, long newSize)
        {
            var result = Result.Ok;
            try
            {
                result = Callback.SetSize(newSize);
            }
            catch (SharpDXException exception)
            {
                result = exception.ResultCode;
            }
            catch (Exception)
            {
                result = Result.Fail.Code;
            }
            return result;
        }

        /// <unmanaged>HRESULT IStream::CopyTo([In] IStream* pstm,[In] ULARGE_INTEGER cb,[Out, Optional] ULARGE_INTEGER* pcbRead,[Out, Optional] ULARGE_INTEGER* pcbWritten)</unmanaged>	
        /* internal long CopyTo_(System.IntPtr stmRef, long cb, out long cbWrittenRef) */
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int CopyToDelegate(IntPtr thisPtr, IntPtr streamPointer, long numberOfBytes, out long numberOfBytesRead, out long numberOfBytesWritten);
        private int CopyToImpl(IntPtr thisPtr, IntPtr streamPointer, long numberOfBytes, out long numberOfBytesRead, out long numberOfBytesWritten)
        {
            numberOfBytesRead = 0;
            numberOfBytesWritten = 0;
            try
            {
                numberOfBytesRead = Callback.CopyTo(new ComStream(streamPointer), numberOfBytes, out numberOfBytesWritten);
            }
            catch (SharpDXException exception)
            {
                return exception.ResultCode.Code;
            }
            catch (Exception)
            {
                return Result.Fail.Code;
            }
            return Result.Ok.Code;
        }

        /// <unmanaged>HRESULT IStream::Commit([In] STGC grfCommitFlags)</unmanaged>	
        /* public SharpDX.Result Commit(SharpDX.Win32.CommitFlags grfCommitFlags) */
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Result CommitDelegate(IntPtr thisPtr, CommitFlags flags);
        private Result CommitImpl(IntPtr thisPtr, CommitFlags flags)
        {
            var result = Result.Ok;
            try
            {
                result = Callback.Commit(flags);
            }
            catch (SharpDXException exception)
            {
                result = exception.ResultCode;
            }
            catch (Exception)
            {
                result = Result.Fail.Code;
            }
            return result;
        }

        /// <unmanaged>HRESULT IStream::Revert()</unmanaged>	
        /* public SharpDX.Result Revert() */
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Result RevertDelegate(IntPtr thisPtr);
        private Result RevertImpl(IntPtr thisPtr)
        {
            var result = Result.Ok;
            try
            {
                result = Callback.Revert();
            }
            catch (SharpDXException exception)
            {
                result = exception.ResultCode;
            }
            catch (Exception)
            {
                result = Result.Fail.Code;
            }
            return result;
        }

        /// <unmanaged>HRESULT IStream::LockRegion([In] ULARGE_INTEGER libOffset,[In] ULARGE_INTEGER cb,[In] LOCKTYPE dwLockType)</unmanaged>	
        /* public SharpDX.Result LockRegion(long libOffset, long cb, SharpDX.Win32.LockType dwLockType) */
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Result LockRegionDelegate(IntPtr thisPtr, long offset, long numberOfBytes, LockType lockType);
        private Result LockRegionImpl(IntPtr thisPtr, long offset, long numberOfBytes, LockType lockType)
        {
            var result = Result.Ok;
            try
            {
                result = Callback.LockRegion(offset, numberOfBytes, lockType);
            }
            catch (SharpDXException exception)
            {
                result = exception.ResultCode;
            }
            catch (Exception)
            {
                result = Result.Fail.Code;
            }
            return result;
        }


        /// <unmanaged>HRESULT IStream::UnlockRegion([In] ULARGE_INTEGER libOffset,[In] ULARGE_INTEGER cb,[In] LOCKTYPE dwLockType)</unmanaged>	
        /* public SharpDX.Result UnlockRegion(long libOffset, long cb, SharpDX.Win32.LockType dwLockType) */
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Result UnlockRegionDelegate(IntPtr thisPtr, long offset, long numberOfBytes, LockType lockType);
        private Result UnlockRegionImpl(IntPtr thisPtr, long offset, long numberOfBytes, LockType lockType)
        {
            var result = Result.Ok;
            try
            {
                result = Callback.UnlockRegion(offset, numberOfBytes, lockType);
            }
            catch (SharpDXException exception)
            {
                result = exception.ResultCode;
            }
            catch (Exception)
            {
                result = Result.Fail.Code;
            }
            return result;
        }

        /// <unmanaged>HRESULT IStream::Stat([Out] STATSTG* pstatstg,[In] STATFLAG grfStatFlag)</unmanaged>	
        /* public SharpDX.Win32.StorageStatistics GetStatistics(SharpDX.Win32.StorageStatisticsFlags grfStatFlag) */
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Result StatDelegate(IntPtr thisPtr, ref StorageStatistics.__Native statisticsPtr, StorageStatisticsFlags flags );
        private Result StatImpl(IntPtr thisPtr, ref StorageStatistics.__Native statisticsPtr, StorageStatisticsFlags flags )
        {
            try
            {
                var statistics = Callback.GetStatistics(flags);
                statistics.__MarshalTo(ref statisticsPtr);
            }
            catch (SharpDXException exception)
            {
                return exception.ResultCode;
            }
            catch (Exception)
            {
                return Result.Fail.Code;
            }
            return Result.Ok;
        }

        /// <unmanaged>HRESULT IStream::Clone([Out] IStream** ppstm)</unmanaged>	
        /* public SharpDX.Win32.IStream Clone() */
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Result CloneDelegate(IntPtr thisPtr, out IntPtr streamPointer);
        private Result CloneImpl(IntPtr thisPtr, out IntPtr streamPointer)
        {
            streamPointer = IntPtr.Zero;
            var result = Result.Ok;
            try
            {
                var clone = Callback.Clone();
                streamPointer = ComStream.ToComPointer(clone);
            }
            catch (SharpDXException exception)
            {
                result = exception.ResultCode;
            }
            catch (Exception)
            {
                result = Result.Fail.Code;
            }
            return result;
        }
    }
}

