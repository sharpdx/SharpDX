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
using SharpDX;
using SharpDX.DirectWrite;

namespace CustomFont
{
    /// <summary>
    /// This FontFileStream implem is reading data from a <see cref="DataStream"/>.
    /// </summary>
    public class ResourceFontFileStream : FontFileStream
    {
        private readonly DataStream _stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceFontFileStream"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public ResourceFontFileStream(DataStream stream)
        {
            this._stream = stream;
        }

        /// <summary>
        /// Reads a fragment from a font file.
        /// </summary>
        /// <param name="fragmentStart">When this method returns, contains an address of a  reference to the start of the font file fragment.  This parameter is passed uninitialized.</param>
        /// <param name="fileOffset">The offset of the fragment, in bytes, from the beginning of the font file.</param>
        /// <param name="fragmentSize">The size of the file fragment, in bytes.</param>
        /// <param name="fragmentContext">When this method returns, contains the address of</param>
        /// <remarks>
        /// Note that ReadFileFragment implementations must check whether the requested font file fragment is within the file bounds. Otherwise, an error should be returned from ReadFileFragment.   {{DirectWrite}} may invoke <see cref="SharpDX.DirectWrite.FontFileStream"/> methods on the same object from multiple threads simultaneously. Therefore, ReadFileFragment implementations that rely on internal mutable state must serialize access to such state across multiple threads. For example, an implementation that uses separate Seek and Read operations to read a file fragment must place the code block containing Seek and Read calls under a lock or a critical section.
        /// </remarks>
        /// <unmanaged>HRESULT IDWriteFontFileStream::ReadFileFragment([Out, Buffer] const void** fragmentStart,[None] __int64 fileOffset,[None] __int64 fragmentSize,[Out] void** fragmentContext)</unmanaged>
        void FontFileStream.ReadFileFragment(out IntPtr fragmentStart, long fileOffset, long fragmentSize, out IntPtr fragmentContext)
        {
            lock (this)
            {
                fragmentContext = IntPtr.Zero;
                _stream.Position = fileOffset;
                fragmentStart = _stream.PositionPointer;
            }
        }

        /// <summary>
        /// Releases a fragment from a file.
        /// </summary>
        /// <param name="fragmentContext">A reference to the client-defined context of a font fragment returned from {{ReadFileFragment}}.</param>
        /// <unmanaged>void IDWriteFontFileStream::ReleaseFileFragment([None] void* fragmentContext)</unmanaged>
        void FontFileStream.ReleaseFileFragment(IntPtr fragmentContext)
        {
            // Nothing to release. No context are used
        }

        /// <summary>
        /// Obtains the total size of a file.
        /// </summary>
        /// <returns>the total size of the file.</returns>
        /// <remarks>
        /// Implementing GetFileSize() for asynchronously loaded font files may require downloading the complete file contents. Therefore, this method should be used only for operations that either require a complete font file to be loaded (for example, copying a font file) or that need to make decisions based on the value of the file size (for example, validation against a persisted file size).
        /// </remarks>
        /// <unmanaged>HRESULT IDWriteFontFileStream::GetFileSize([Out] __int64* fileSize)</unmanaged>
        long FontFileStream.GetFileSize()
        {
            return _stream.Length;
        }

        /// <summary>
        /// Obtains the last modified time of the file.
        /// </summary>
        /// <returns>
        /// the last modified time of the file in the format that represents the number of 100-nanosecond intervals since January 1, 1601 (UTC).
        /// </returns>
        /// <remarks>
        /// The "last modified time" is used by DirectWrite font selection algorithms to determine whether one font resource is more up to date than another one.
        /// </remarks>
        /// <unmanaged>HRESULT IDWriteFontFileStream::GetLastWriteTime([Out] __int64* lastWriteTime)</unmanaged>
        long FontFileStream.GetLastWriteTime()
        {
            return 0;
        }
    }
}