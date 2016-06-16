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

namespace SharpDX.Direct3D10
{
    public partial struct ImageInformation
    {
        /// <summary>	
        /// Retrieves information about a given image file.	
        /// </summary>	
        /// <param name="fileName">File name of image to retrieve information about.</param>
        /// <returns>If the function succeeds, returns a <see cref="SharpDX.Direct3D10.ImageInformation"/> filled with the description of the data in the source file. else returns null </returns>
        /// <unmanaged>HRESULT D3DX11GetImageInfoFromFileW([None] const wchar_t* pSrcFile,[None] ID3DX11ThreadPump* pPump,[None] D3DX11_IMAGE_INFO* pSrcInfo,[None] HRESULT* pHResult)</unmanaged>
        public static ImageInformation? FromFile(string fileName)
        {
            try
            {
                var info = new ImageInformation();
                Result hresult;
                D3DX10.GetImageInfoFromFile(fileName, IntPtr.Zero, ref info, out hresult);
                // TODO check hresult?
                return info;
            }
            catch (SharpDXException)
            { }
            return null;
        }

        /// <summary>	
        /// Retrieves information about a given image file from a memory location.
        /// </summary>	
        /// <param name="memory">an array to the image in memory</param>
        /// <returns>If the function succeeds, returns a <see cref="SharpDX.Direct3D10.ImageInformation"/> filled with the description of the data from the image memory. else returns null </returns>
        /// <unmanaged>HRESULT D3DX11GetImageInfoFromFileW([None] const wchar_t* pSrcFile,[None] ID3DX11ThreadPump* pPump,[None] D3DX11_IMAGE_INFO* pSrcInfo,[None] HRESULT* pHResult)</unmanaged>
        public static ImageInformation? FromMemory(byte[] memory)
        {
            unsafe
            {
                try
                {
                    var info = new ImageInformation();
                    Result hresult;
                    fixed (void* pMemory = &memory[0])
                        D3DX10.GetImageInfoFromMemory((IntPtr)pMemory, memory.Length, IntPtr.Zero, ref info, out hresult);
                    // TODO check hresult?
                    return info;
                }
                catch (SharpDXException)
                {
                }
                return null;
            }
        }
    }
}
