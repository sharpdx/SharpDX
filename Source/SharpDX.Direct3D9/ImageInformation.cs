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

namespace SharpDX.Direct3D9
{
    public partial struct ImageInformation
    {
        /// <summary>
        /// Retrieves information about a given image file on the disk.
        /// </summary>
        /// <param name="fileName">The filename.</param>
        /// <returns>A <see cref="ImageInformation"/> structure</returns>
        /// <unmanaged>HRESULT D3DXGetImageInfoFromFileInMemory([In] const void* pSrcData,[In] unsigned int SrcDataSize,[Out] D3DXIMAGE_INFO* pSrcInfo)</unmanaged>
        public static ImageInformation FromFile(string fileName)
        {
            return FromMemory(File.ReadAllBytes(fileName));
        }

        /// <summary>
        /// Retrieves information about a given image file in memory.
        /// </summary>
        /// <param name="memory">The memory.</param>
        /// <returns>A <see cref="ImageInformation"/> structure</returns>
        /// <unmanaged>HRESULT D3DXGetImageInfoFromFileInMemory([In] const void* pSrcData,[In] unsigned int SrcDataSize,[Out] D3DXIMAGE_INFO* pSrcInfo)</unmanaged>
        public static ImageInformation FromMemory(byte[] memory)
        {
            unsafe
            {
                fixed (void* pMemory = memory)
                    return D3DX9.GetImageInfoFromFileInMemory((IntPtr)pMemory, memory.Length);
            }
        }

        /// <summary>
        /// Retrieves information about a given image file from a stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>A <see cref="ImageInformation"/> structure</returns>
        /// <remarks>This method keeps the position of the stream</remarks>
        /// <unmanaged>HRESULT D3DXGetImageInfoFromFileInMemory([In] const void* pSrcData,[In] unsigned int SrcDataSize,[Out] D3DXIMAGE_INFO* pSrcInfo)</unmanaged>
        public static ImageInformation FromStream(Stream stream)
        {
            return FromStream(stream, true);
        }

        /// <summary>
        /// Retrieves information about a given image file from a stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="keepPosition">if set to <c>true</c> preserve the stream position; <c>false</c> will move the stream pointer.</param>
        /// <returns>A <see cref="ImageInformation"/> structure</returns>
        /// <unmanaged>HRESULT D3DXGetImageInfoFromFileInMemory([In] const void* pSrcData,[In] unsigned int SrcDataSize,[Out] D3DXIMAGE_INFO* pSrcInfo)</unmanaged>
        public static ImageInformation FromStream(Stream stream, bool keepPosition)
        {
            long savedPosition = 0;
            if (keepPosition)
                savedPosition = stream.Position;

            ImageInformation result;
            if (stream is DataStream)
            {
                var dataStream = ((DataStream)stream);
                result = D3DX9.GetImageInfoFromFileInMemory(dataStream.PositionPointer, (int)(dataStream.Length - dataStream.Position));
                dataStream.Position = dataStream.Length;
            } else
            {
                var buffer = Utilities.ReadStream(stream);
                result = FromMemory(buffer);
            }
            if (keepPosition)
                stream.Position = savedPosition;

            return result;
        }
    }
}