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
using System.IO;

namespace SharpDX.Win32
{
    [Shadow(typeof(ComStreamShadow))]
    public partial interface IStream
    {
        /// <summary>
        /// Changes the seek pointer to a new location relative to the beginning of the stream, to the end of the stream, or to the current seek pointer.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="origin">The origin.</param>
        /// <returns>The offset of the seek pointer from the beginning of the stream.</returns>
        long Seek(long offset, SeekOrigin origin);

        /// <summary>
        /// Changes the size of the stream object.
        /// </summary>
        /// <param name="newSize">The new size.</param>
        void SetSize(long newSize);

        /// <summary>
        /// Copies a specified number of bytes from the current seek pointer in the stream to the current seek pointer in another stream.
        /// </summary>
        /// <param name="streamDest">The stream destination.</param>
        /// <param name="numberOfBytesToCopy">The number of bytes to copy.</param>
        /// <param name="bytesWritten">The number of bytes written.</param>
        /// <returns>The number of bytes read</returns>
        long CopyTo(IStream streamDest, long numberOfBytesToCopy, out long bytesWritten);

        /// <summary>
        /// Commit method ensures that any changes made to a stream object open in transacted mode are reflected in the parent storage. If the stream object is open in direct mode, Commit has no effect other than flushing all memory buffers to the next-level storage object. The COM compound file implementation of streams does not support opening streams in transacted mode.
        /// </summary>
        /// <param name="commitFlags">The GRF commit flags.</param>
        void Commit(CommitFlags commitFlags);

        /// <summary>
        /// Discards all changes that have been made to a transacted stream since the last <see cref="Commit"/> call. 
        /// </summary>
        void Revert();

        /// <summary>
        /// Restricts access to a specified range of bytes in the stream.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="numberOfBytesToLock">The number of bytes to lock.</param>
        /// <param name="dwLockType">Type of the dw lock.</param>
        void LockRegion(long offset, long numberOfBytesToLock, LockType dwLockType);

        /// <summary>
        /// Unlocks access to a specified range of bytes in the stream.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="numberOfBytesToLock">The number of bytes to lock.</param>
        /// <param name="dwLockType">Type of the dw lock.</param>
        void UnlockRegion(long offset, long numberOfBytesToLock, LockType dwLockType);

        /// <summary>
        /// Gets the statistics.
        /// </summary>
        /// <param name="storageStatisticsFlags">The storage statistics flags.</param>
        /// <returns></returns>
        StorageStatistics GetStatistics(StorageStatisticsFlags storageStatisticsFlags);

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        IStream Clone();
    }
}