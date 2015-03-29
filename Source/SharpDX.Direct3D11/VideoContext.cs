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

namespace SharpDX.Direct3D11
{
    using System;

    /// <summary>	
    /// <p>[This documentation is preliminary and is subject to change.]</p><p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p>Gets a reference to a DirectX Video Acceleration (DXVA) decoder buffer.</p>	
    /// </summary>	
    /// <remarks>	
    /// <p>The graphics driver allocates the buffers that are used for DXVA decoding. This method locks the Microsoft Direct3D surface that contains the buffer. When you are done using the buffer, call <strong><see cref="SharpDX.Direct3D11.VideoContext.ReleaseDecoderBuffer"/></strong> to unlock the surface. </p>	
    /// </remarks>	
    /// <include file='.\Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D11VideoContext']/*"/>	
    /// <msdn-id>hh447711</msdn-id>	
    /// <unmanaged>ID3D11VideoContext</unmanaged>	
    /// <unmanaged-short>ID3D11VideoContext</unmanaged-short>
    public partial class VideoContext
    {
        /// <summary>	
        /// <p>[This documentation is preliminary and is subject to change.]</p><p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p>Gets a reference to a DirectX Video Acceleration (DXVA) decoder buffer.</p>	
        /// </summary>	
        /// <param name="decoder"><dd> <p>A reference to the <strong><see cref="SharpDX.Direct3D11.VideoDecoder"/></strong> interface. To get this reference, call <strong><see cref="SharpDX.Direct3D11.VideoDevice.CreateVideoDecoder"/></strong>.</p> </dd></param>	
        /// <param name="type"><dd> <p>The type of buffer to retrieve, specified as a member of the <strong><see cref="SharpDX.Direct3D11.VideoDecoderBufferType"/></strong> enumeration.</p> </dd></param>	
        /// <returns>An <see cref="DataPointer"/> to the memory buffer.</returns>
        /// <remarks>	
        /// <p>The graphics driver allocates the buffers that are used for DXVA decoding. This method locks the Microsoft Direct3D surface that contains the buffer. When you are done using the buffer, call <strong><see cref="SharpDX.Direct3D11.VideoContext.ReleaseDecoderBuffer"/></strong> to unlock the surface. </p>	
        /// </remarks>	
        /// <include file='.\Documentation\CodeComments.xml' path="/comments/comment[@id='ID3D11VideoContext::GetDecoderBuffer']/*"/>	
        /// <msdn-id>hh447711</msdn-id>	
        /// <unmanaged>HRESULT ID3D11VideoContext::GetDecoderBuffer([In] ID3D11VideoDecoder* pDecoder,[In] D3D11_VIDEO_DECODER_BUFFER_TYPE Type,[Out] unsigned int* pBufferSize,[Out] void** ppBuffer)</unmanaged>	
        /// <unmanaged-short>ID3D11VideoContext::GetDecoderBuffer</unmanaged-short>	
        public DataPointer GetDecoderBuffer(VideoDecoder decoder, VideoDecoderBufferType type)
        {
            int size;
            IntPtr dataPtr;

            GetDecoderBuffer(decoder, type, out size, out dataPtr);

            return new DataPointer(dataPtr, size);
        }
    }
}