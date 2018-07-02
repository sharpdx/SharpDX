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
using SharpDX.Mathematics.Interop;
using SharpDX.Win32;
using SharpDX.IO;

#if STORE_APP
using Windows.Storage.Streams;
#endif

namespace SharpDX.MediaFoundation
{
    /// <summary>
    /// ByteStream class used 
    /// </summary>
    public partial class ByteStream
    {
        private Stream sourceStream;
        private ComStream comStream;
        private ComStreamProxy streamProxy;
        private ComObject randomAccessStreamCom;

        /// <summary>
        /// Instantiates a new instance <see cref="ByteStream"/> from a <see cref="Stream"/>.
        /// </summary>
        /// <msdn-id>hh162754</msdn-id>	
        /// <unmanaged>HRESULT MFCreateMFByteStreamOnStreamEx([In] IUnknown* punkStream,[Out] IMFByteStream** ppByteStream)</unmanaged>	
        /// <unmanaged-short>MFCreateMFByteStreamOnStreamEx</unmanaged-short>	
        public ByteStream(Stream sourceStream)
        {
            this.sourceStream = sourceStream;
#if STORE_APP
            var randomAccessStream = sourceStream.AsRandomAccessStream();
            MediaFactory.CreateMFByteStreamOnStreamEx(new ComObject(Marshal.GetIUnknownForObject(randomAccessStream)), this);
#else
            streamProxy = new ComStreamProxy(sourceStream);
            MediaFactory.CreateMFByteStreamOnStream(streamProxy, this);
#endif
        }

        /// <summary>
        /// Instantiates a new instance <see cref="ByteStream"/> from a <see cref="Stream"/>.
        /// </summary>
        /// <msdn-id>hh162754</msdn-id>	
        /// <unmanaged>HRESULT MFCreateMFByteStreamOnStreamEx([In] IUnknown* punkStream,[Out] IMFByteStream** ppByteStream)</unmanaged>	
        /// <unmanaged-short>MFCreateMFByteStreamOnStreamEx</unmanaged-short>	
        public ByteStream(byte[] sourceStream) : this(new MemoryStream(sourceStream))
        {
        }

#if STORE_APP
        /// <summary>
        /// Instantiates a new instance <see cref="ByteStream"/> from a <see cref="Stream"/>.
        /// </summary>
        /// <msdn-id>hh162754</msdn-id>	
        /// <unmanaged>HRESULT MFCreateMFByteStreamOnStreamEx([In] IUnknown* punkStream,[Out] IMFByteStream** ppByteStream)</unmanaged>	
        /// <unmanaged-short>MFCreateMFByteStreamOnStreamEx</unmanaged-short>	
        public ByteStream(IRandomAccessStream sourceStream)
        {
            var randomAccessStream = sourceStream;
            MediaFactory.CreateMFByteStreamOnStreamEx(new ComObject(Marshal.GetIUnknownForObject(randomAccessStream)), this);
        }
#endif

#if DESKTOP_APP
        /// <summary>
        /// Instantiates a new instance <see cref="ByteStream"/> from a <see cref="Stream"/>.
        /// </summary>
        /// <msdn-id>hh162754</msdn-id>	
        /// <unmanaged>HRESULT MFCreateMFByteStreamOnStreamEx([In] IUnknown* punkStream,[Out] IMFByteStream** ppByteStream)</unmanaged>	
        /// <unmanaged-short>MFCreateMFByteStreamOnStreamEx</unmanaged-short>	
        public ByteStream(ComStream sourceStream)
        {
            this.comStream = sourceStream;
            MediaFactory.CreateMFByteStreamOnStream(sourceStream, this);
        }
#endif
        /// <summary>	
        /// <p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p> Retrieves the characteristics of the byte stream. </p>	
        /// </summary>	
        /// <returns>The capabilities of the stream.</returns>	
        /// <remarks>	
        /// <p>This interface is available on the following platforms if the Windows Media Format 11 SDK redistributable components are installed:</p><ul> <li>Windows?XP with Service Pack?2 (SP2) and later.</li> <li>Windows?XP Media Center Edition?2005 with KB900325 (Windows?XP Media Center Edition?2005) and KB925766 (October 2006 Update Rollup for Windows?XP Media Center Edition) installed.</li> </ul>	
        /// </remarks>	
        /// <msdn-id>ms698962</msdn-id>	
        /// <unmanaged>HRESULT IMFByteStream::GetCapabilities([Out] unsigned int* pdwCapabilities)</unmanaged>	
        /// <unmanaged-short>IMFByteStream::GetCapabilities</unmanaged-short>	
        public int Capabilities
        {
            get
            {
                int result;
                GetCapabilities_(out result);
                return result;
            }
        }

        /// <summary>	
        /// <p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p> Retrieves the length of the stream. </p>	
        /// </summary>	
        /// <returns>The length of the stream, in bytes. If the length is unknown, this value is -1.</returns>	
        /// <remarks>	
        /// <p>This interface is available on the following platforms if the Windows Media Format 11 SDK redistributable components are installed:</p><ul> <li>Windows?XP with Service Pack?2 (SP2) and later.</li> <li>Windows?XP Media Center Edition?2005 with KB900325 (Windows?XP Media Center Edition?2005) and KB925766 (October 2006 Update Rollup for Windows?XP Media Center Edition) installed.</li> </ul>	
        /// </remarks>	
        /// <msdn-id>ms698941</msdn-id>	
        /// <unmanaged>HRESULT IMFByteStream::GetLength([Out] unsigned longlong* pqwLength)</unmanaged>	
        /// <unmanaged-short>IMFByteStream::GetLength</unmanaged-short>	
        public long Length
        {
            get
            {
                long length;
                GetLength_(out length);
                return length;
            }
            set { SetLength_(value); }
        }

        /// <summary>	
        /// <p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p> Retrieves the current read or write position in the stream. </p>	
        /// </summary>	
        /// <returns>The current position, in bytes.</returns>	
        /// <remarks>	
        /// <p> The methods that update the current position are <strong>Read</strong>, <strong>BeginRead</strong>, <strong>Write</strong>, <strong>BeginWrite</strong>, <strong>SetCurrentPosition</strong>, and <strong>Seek</strong>. </p><p>This interface is available on the following platforms if the Windows Media Format 11 SDK redistributable components are installed:</p><ul> <li>Windows?XP with Service Pack?2 (SP2) and later.</li> <li>Windows?XP Media Center Edition?2005 with KB900325 (Windows?XP Media Center Edition?2005) and KB925766 (October 2006 Update Rollup for Windows?XP Media Center Edition) installed.</li> </ul>	
        /// </remarks>	
        /// <msdn-id>ms704059</msdn-id>	
        /// <unmanaged>HRESULT IMFByteStream::GetCurrentPosition([Out] unsigned longlong* pqwPosition)</unmanaged>	
        /// <unmanaged-short>IMFByteStream::GetCurrentPosition</unmanaged-short>	
        public long CurrentPosition
        {
            get
            {
                long position;
                GetCurrentPosition_(out position);
                return position;
            }
            set { SetCurrentPosition_(value); }
        }

        /// <summary>	
        /// <p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p> </p><p>Queries whether the current position has reached the end of the stream.</p>	
        /// </summary>	
        /// <returns>true if the end of the stream has been reached</returns>	
        /// <remarks>	
        /// <p>This interface is available on the following platforms if the Windows Media Format 11 SDK redistributable components are installed:</p><ul> <li>Windows?XP with Service Pack?2 (SP2) and later.</li> <li>Windows?XP Media Center Edition?2005 with KB900325 (Windows?XP Media Center Edition?2005) and KB925766 (October 2006 Update Rollup for Windows?XP Media Center Edition) installed.</li> </ul>	
        /// </remarks>	
        /// <msdn-id>ms697369</msdn-id>	
        /// <unmanaged>HRESULT IMFByteStream::IsEndOfStream([Out] BOOL* pfEndOfStream)</unmanaged>	
        /// <unmanaged-short>IMFByteStream::IsEndOfStream</unmanaged-short>	
        public bool IsEndOfStream
        {
            get
            {
                return IsEndOfStream_;
            }
        }

        /// <summary>	
        /// <p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p> Reads data from the stream. </p>	
        /// </summary>	
        /// <param name="bRef"><dd> <p> Pointer to a buffer that receives the data. The caller must allocate the buffer. </p> </dd></param>
        /// <param name="offset">Offset into the buffer.</param>
        /// <param name="count"><dd> <p> Size of the buffer in bytes. </p> </dd></param>	
        /// <returns>The number of bytes that are copied into the buffer</returns>	
        /// <remarks>	
        /// <p> This method reads at most <em>cb</em> bytes from the current position in the stream and copies them into the buffer provided by the caller. The number of bytes that were read is returned in the <em>pcbRead</em> parameter. The method does not return an error code on reaching the end of the file, so the application should check the value in <em>pcbRead</em> after the method returns. </p><p> This method is synchronous. It blocks until the read operation completes. </p><p>This interface is available on the following platforms if the Windows Media Format 11 SDK redistributable components are installed:</p><ul> <li>Windows?XP with Service Pack?2 (SP2) and later.</li> <li>Windows?XP Media Center Edition?2005 with KB900325 (Windows?XP Media Center Edition?2005) and KB925766 (October 2006 Update Rollup for Windows?XP Media Center Edition) installed.</li> </ul>	
        /// </remarks>	
        /// <msdn-id>ms698913</msdn-id>	
        /// <unmanaged>HRESULT IMFByteStream::Read([Out, Buffer] unsigned char* pb,[In] unsigned int cb,[Out] unsigned int* pcbRead)</unmanaged>	
        /// <unmanaged-short>IMFByteStream::Read</unmanaged-short>	
        public unsafe int Read(byte[] bRef, int offset, int count)
        {
            int result;
            fixed (void* ptr = &bRef[offset])
                Read_((System.IntPtr) ptr, count, out result);
            return result;
        }

        /// <summary>	
        /// <p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p> Begins an asynchronous read operation from the stream. </p>	
        /// </summary>	
        /// <param name="bRef"><dd> <p> Pointer to a buffer that receives the data. The caller must allocate the buffer. </p> </dd></param>
        /// <param name="offset">The offset in the buffer to begin reading from.</param>
        /// <param name="count"><dd> <p> Size of the buffer in bytes. </p> </dd></param>	
        /// <param name="callbackRef"><dd> <p> Pointer to the <strong><see cref="SharpDX.MediaFoundation.IAsyncCallback"/></strong> interface of a callback object. The caller must implement this interface. </p> </dd></param>	
        /// <param name="context"><dd> <p> Pointer to the <strong><see cref="SharpDX.ComObject"/></strong> interface of a state object, defined by the caller. This parameter can be <strong><c>null</c></strong>. You can use this object to hold state information. The object is returned to the caller when the callback is invoked. </p> </dd></param>	
        /// <returns><p>If this method succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p></returns>	
        /// <remarks>	
        /// <p> When all of the data has been read into the buffer, the callback object's <strong><see cref="SharpDX.MediaFoundation.IAsyncCallback.Invoke"/></strong> method is called. At that point, the application should call <strong><see cref="SharpDX.MediaFoundation.IByteStream.EndRead"/></strong> to complete the asynchronous request. </p><p> Do not read from, write to, free, or reallocate the buffer while an asynchronous read is pending. </p><p>This interface is available on the following platforms if the Windows Media Format 11 SDK redistributable components are installed:</p><ul> <li>Windows?XP with Service Pack?2 (SP2) and later.</li> <li>Windows?XP Media Center Edition?2005 with KB900325 (Windows?XP Media Center Edition?2005) and KB925766 (October 2006 Update Rollup for Windows?XP Media Center Edition) installed.</li> </ul>	
        /// </remarks>	
        /// <msdn-id>ms704810</msdn-id>	
        /// <unmanaged>HRESULT IMFByteStream::BeginRead([Out, Buffer] unsigned char* pb,[In] unsigned int cb,[In] IMFAsyncCallback* pCallback,[In] IUnknown* punkState)</unmanaged>	
        /// <unmanaged-short>IMFByteStream::BeginRead</unmanaged-short>	
        public unsafe void BeginRead(byte[] bRef, int offset, int count, SharpDX.MediaFoundation.IAsyncCallback callbackRef, object context)
        {
            fixed (void* ptr = &bRef[offset])
                BeginRead_((System.IntPtr) ptr, count, callbackRef, context != null ? Marshal.GetIUnknownForObject(context) : IntPtr.Zero);
        }

        /// <summary>	
        /// <p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p> Completes an asynchronous read operation. </p>	
        /// </summary>	
        /// <param name="resultRef"><dd> <p> Pointer to the <strong><see cref="SharpDX.MediaFoundation.AsyncResult"/></strong> interface. Pass in the same reference that your callback object received in the <strong><see cref="SharpDX.MediaFoundation.IAsyncCallback.Invoke"/></strong> method. </p> </dd></param>	
        /// <returns>The number of bytes that were read</returns>	
        /// <remarks>	
        /// <p> Call this method after the <strong><see cref="SharpDX.MediaFoundation.IByteStream.BeginRead"/></strong> method completes asynchronously. </p><p>This interface is available on the following platforms if the Windows Media Format 11 SDK redistributable components are installed:</p><ul> <li>Windows?XP with Service Pack?2 (SP2) and later.</li> <li>Windows?XP Media Center Edition?2005 with KB900325 (Windows?XP Media Center Edition?2005) and KB925766 (October 2006 Update Rollup for Windows?XP Media Center Edition) installed.</li> </ul>	
        /// </remarks>	
        /// <msdn-id>ms704042</msdn-id>	
        /// <unmanaged>HRESULT IMFByteStream::EndRead([In] IMFAsyncResult* pResult,[Out] unsigned int* pcbRead)</unmanaged>	
        /// <unmanaged-short>IMFByteStream::EndRead</unmanaged-short>	
        public int EndRead(SharpDX.MediaFoundation.AsyncResult resultRef)
        {
            int result;
            EndRead_(resultRef, out result);
            return result;
        }

        /// <summary>	
        /// <p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p> </p><p>Writes data to the stream.</p>	
        /// </summary>	
        /// <param name="bRef"><dd> <p> Pointer to a buffer that contains the data to write. </p> </dd></param>
        /// <param name="offset">The offset within the buffer to begin writing at.</param>
        /// <param name="count"><dd> <p> Size of the buffer in bytes. </p> </dd></param>	
        /// <returns>The number of bytes that are written.</returns>	
        /// <remarks>	
        /// <p> This method writes the contents of the <em>pb</em> buffer to the stream, starting at the current stream position. The number of bytes that were written is returned in the <em>pcbWritten</em> parameter. </p><p> This method is synchronous. It blocks until the write operation completes. </p><p>This interface is available on the following platforms if the Windows Media Format 11 SDK redistributable components are installed:</p><ul> <li>Windows?XP with Service Pack?2 (SP2) and later.</li> <li>Windows?XP Media Center Edition?2005 with KB900325 (Windows?XP Media Center Edition?2005) and KB925766 (October 2006 Update Rollup for Windows?XP Media Center Edition) installed.</li> </ul>	
        /// </remarks>	
        /// <msdn-id>ms703843</msdn-id>	
        /// <unmanaged>HRESULT IMFByteStream::Write([In, Buffer] const unsigned char* pb,[In] unsigned int cb,[Out] unsigned int* pcbWritten)</unmanaged>	
        /// <unmanaged-short>IMFByteStream::Write</unmanaged-short>	
        public unsafe int Write(byte[] bRef, int offset, int count)
        {
            int result;
            fixed (void* ptr = &bRef[offset])
                Write_((IntPtr) ptr, count, out result);
            return result;
        }

        /// <summary>	
        /// <p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p> Begins an asynchronous write operation to the stream. </p>	
        /// </summary>	
        /// <param name="bRef"><dd> <p> Pointer to a buffer containing the data to write. </p> </dd></param>
        /// <param name="offset">The offset within the buffer to begin writing at.</param>
        /// <param name="count"><dd> <p> Size of the buffer in bytes. </p> </dd></param>	
        /// <param name="callbackRef"><dd> <p> Pointer to the <strong><see cref="SharpDX.MediaFoundation.IAsyncCallback"/></strong> interface of a callback object. The caller must implement this interface. </p> </dd></param>	
        /// <param name="context"><dd> <p> Pointer to the <strong><see cref="SharpDX.ComObject"/></strong> interface of a state object, defined by the caller. This parameter can be <strong><c>null</c></strong>. You can use this object to hold state information. The object is returned to the caller when the callback is invoked. </p> </dd></param>	
        /// <returns><p>If this method succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p></returns>	
        /// <remarks>	
        /// <p> When all of the data has been written to the stream, the callback object's <strong><see cref="SharpDX.MediaFoundation.IAsyncCallback.Invoke"/></strong> method is called. At that point, the application should call <strong><see cref="SharpDX.MediaFoundation.IByteStream.EndWrite"/></strong> to complete the asynchronous request. </p><p> Do not reallocate, free, or write to the buffer while an asynchronous write is still pending. </p><p>This interface is available on the following platforms if the Windows Media Format 11 SDK redistributable components are installed:</p><ul> <li>Windows?XP with Service Pack?2 (SP2) and later.</li> <li>Windows?XP Media Center Edition?2005 with KB900325 (Windows?XP Media Center Edition?2005) and KB925766 (October 2006 Update Rollup for Windows?XP Media Center Edition) installed.</li> </ul>	
        /// </remarks>	
        /// <msdn-id>ms694005</msdn-id>	
        /// <unmanaged>HRESULT IMFByteStream::BeginWrite([In, Buffer] const unsigned char* pb,[In] unsigned int cb,[In] IMFAsyncCallback* pCallback,[In] IUnknown* punkState)</unmanaged>	
        /// <unmanaged-short>IMFByteStream::BeginWrite</unmanaged-short>	
        public unsafe void BeginWrite(byte[] bRef, int offset, int count, SharpDX.MediaFoundation.IAsyncCallback callbackRef, object context)
        {
            fixed (void* ptr = &bRef[offset])
                BeginWrite_((System.IntPtr)ptr, count, callbackRef, context != null ? Marshal.GetIUnknownForObject(context) : IntPtr.Zero);
        }

        /// <summary>	
        /// <p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p> </p><p>Completes an asynchronous write operation.</p>	
        /// </summary>	
        /// <param name="resultRef"><dd> <p>Pointer to the <strong><see cref="SharpDX.MediaFoundation.AsyncResult"/></strong> interface. Pass in the same reference that your callback object received in the <strong><see cref="SharpDX.MediaFoundation.IAsyncCallback.Invoke"/></strong> method.</p> </dd></param>	
        /// <returns>The number of bytes that were written</returns>	
        /// <remarks>	
        /// <p> Call this method when the <strong><see cref="SharpDX.MediaFoundation.IByteStream.BeginWrite"/></strong> method completes asynchronously. </p><p>This interface is available on the following platforms if the Windows Media Format 11 SDK redistributable components are installed:</p><ul> <li>Windows?XP with Service Pack?2 (SP2) and later.</li> <li>Windows?XP Media Center Edition?2005 with KB900325 (Windows?XP Media Center Edition?2005) and KB925766 (October 2006 Update Rollup for Windows?XP Media Center Edition) installed.</li> </ul>	
        /// </remarks>	
        /// <msdn-id>ms703863</msdn-id>	
        /// <unmanaged>HRESULT IMFByteStream::EndWrite([In] IMFAsyncResult* pResult,[Out] unsigned int* pcbWritten)</unmanaged>	
        /// <unmanaged-short>IMFByteStream::EndWrite</unmanaged-short>	
        public int EndWrite(SharpDX.MediaFoundation.AsyncResult resultRef)
        {
            int result;
            EndWrite_(resultRef, out result);
            return result;
        }

        /// <summary>	
        /// <p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p> </p><p>Moves the current position in the stream by a specified offset.</p>	
        /// </summary>	
        /// <param name="seekOrigin"><dd> <p> Specifies the origin of the seek as a member of the <strong><see cref="SharpDX.MediaFoundation.BytestreamSeekOrigin"/></strong> enumeration. The offset is calculated relative to this position. </p> </dd></param>	
        /// <param name="llSeekOffset"><dd> <p> Specifies the new position, as a byte offset from the seek origin. </p> </dd></param>	
        /// <param name="dwSeekFlags"><dd> <p> Specifies zero or more flags. The following flags are defined. </p> <table> <tr><th>Value</th><th>Meaning</th></tr> <tr><td><dl> <dt><strong>MFBYTESTREAM_SEEK_FLAG_CANCEL_PENDING_IO</strong></dt> </dl> </td><td> <p> All pending I/O requests are canceled after the seek request completes successfully. </p> </td></tr> </table> <p>?</p> </dd></param>	
        /// <returns>The new position after the seek</returns>	
        /// <remarks>	
        /// <p>This interface is available on the following platforms if the Windows Media Format 11 SDK redistributable components are installed:</p><ul> <li>Windows?XP with Service Pack?2 (SP2) and later.</li> <li>Windows?XP Media Center Edition?2005 with KB900325 (Windows?XP Media Center Edition?2005) and KB925766 (October 2006 Update Rollup for Windows?XP Media Center Edition) installed.</li> </ul>	
        /// </remarks>	
        /// <msdn-id>ms697053</msdn-id>	
        /// <unmanaged>HRESULT IMFByteStream::Seek([In] MFBYTESTREAM_SEEK_ORIGIN SeekOrigin,[In] longlong llSeekOffset,[In] unsigned int dwSeekFlags,[Out] unsigned longlong* pqwCurrentPosition)</unmanaged>	
        /// <unmanaged-short>IMFByteStream::Seek</unmanaged-short>	
        public long Seek(SharpDX.MediaFoundation.ByteStreamSeekOrigin seekOrigin, long llSeekOffset, int dwSeekFlags)
        {
            long result;
            Seek_(seekOrigin, llSeekOffset, dwSeekFlags, out result);
            return result;
        }

        /// <summary>	
        /// <p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p> Clears any internal buffers used by the stream. If you are writing to the stream, the buffered data is written to the underlying file or device. </p>	
        /// </summary>	
        /// <returns><p>If this method succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p></returns>	
        /// <remarks>	
        /// <p> If the byte stream is read-only, this method has no effect.</p><p>This interface is available on the following platforms if the Windows Media Format 11 SDK redistributable components are installed:</p><ul> <li>Windows?XP with Service Pack?2 (SP2) and later.</li> <li>Windows?XP Media Center Edition?2005 with KB900325 (Windows?XP Media Center Edition?2005) and KB925766 (October 2006 Update Rollup for Windows?XP Media Center Edition) installed.</li> </ul>	
        /// </remarks>	
        /// <msdn-id>ms694833</msdn-id>	
        /// <unmanaged>HRESULT IMFByteStream::Flush()</unmanaged>	
        /// <unmanaged-short>IMFByteStream::Flush</unmanaged-short>	
        public void Flush()
        {
            Flush_();
        }

        /// <summary>	
        /// <p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p> Closes the stream and releases any resources associated with the stream, such as sockets or file handles. This method also cancels any pending asynchronous I/O requests. </p>	
        /// </summary>	
        /// <returns><p>If this method succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p></returns>	
        /// <remarks>	
        /// <p>This interface is available on the following platforms if the Windows Media Format 11 SDK redistributable components are installed:</p><ul> <li>Windows?XP with Service Pack?2 (SP2) and later.</li> <li>Windows?XP Media Center Edition?2005 with KB900325 (Windows?XP Media Center Edition?2005) and KB925766 (October 2006 Update Rollup for Windows?XP Media Center Edition) installed.</li> </ul>	
        /// </remarks>	
        /// <msdn-id>ms703909</msdn-id>	
        /// <unmanaged>HRESULT IMFByteStream::Close()</unmanaged>	
        /// <unmanaged-short>IMFByteStream::Close</unmanaged-short>	
        public void Close()
        {
            Close_();
        }

        protected override unsafe void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (streamProxy != null)
            {
                streamProxy.Dispose();
                streamProxy = null;
            }

            if (randomAccessStreamCom != null)
            {
                randomAccessStreamCom.Dispose();
                randomAccessStreamCom = null;
            }
        }
    }
}