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
using System.Runtime.InteropServices;

namespace SharpDX.MediaFoundation
{
    public partial class MediaEngineEx
    {
        /// <summary>	
        /// <p>[This documentation is preliminary and is subject to change.]</p><p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p>Opens a media resource from a byte stream.</p>	
        /// </summary>	
        /// <param name="byteStream"><dd> <p>A reference to the <strong><see cref="SharpDX.MediaFoundation.IByteStream"/></strong> interface of the byte stream.</p> </dd></param>	
        /// <param name="url"><dd> <p>The URL of the byte stream.</p> </dd></param>	
        /// <returns><p>If this method succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p></returns>	
        /// <msdn-id>hh447956</msdn-id>	
        /// <unmanaged>HRESULT IMFMediaEngineEx::SetSourceFromByteStream([In] IMFByteStream* pByteStream,[In] wchar_t* pURL)</unmanaged>	
        /// <unmanaged-short>IMFMediaEngineEx::SetSourceFromByteStream</unmanaged-short>	
        public void SetSourceFromByteStream(ByteStream byteStream, string url)
        {
            var bstrUrl = Utilities.StringToHGlobalUni(url);
            try
            {
                SetSourceFromByteStream(byteStream, bstrUrl);
            } finally
            {
                Marshal.FreeHGlobal(bstrUrl);
            }
        }
    }
}