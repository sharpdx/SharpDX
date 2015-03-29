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

using SharpDX.Mathematics.Interop;

namespace SharpDX.WIC
{
    public partial class DdsFrameDecode
    {
        /// <summary>	
        /// Gets the width and height, in blocks, of the DDS image.
        /// </summary>	
        /// <remarks>	
        /// For block compressed textures, the returned width and height values do not completely define the texture size because the image is padded to fit the closest whole block size. For example, three BC1 textures with pixel dimensions of 1x1, 2x2 and 4x4 will all report <em>pWidthInBlocks</em> = 1 and <em>pHeightInBlocks</em> = 1. </p><p>If the texture does not use a block-compressed <strong><see cref="SharpDX.DXGI.Format"/></strong>, this method returns the texture size in pixels; for these formats the block size returned by <strong>IWICDdsFrameDecoder::GetFormatInfo</strong> is 1x1.	
        /// </remarks>	
        /// <include file='..\Documentation\CodeComments.xml' path="/comments/comment[@id='IWICDdsFrameDecode::GetSizeInBlocks']/*"/>	
        /// <msdn-id>dn302089</msdn-id>	
        /// <unmanaged>HRESULT IWICDdsFrameDecode::GetSizeInBlocks([Out] unsigned int* pWidthInBlocks,[Out] unsigned int* pHeightInBlocks)</unmanaged>	
        /// <unmanaged-short>IWICDdsFrameDecode::GetSizeInBlocks</unmanaged-short>
        public Size2 SizeInBlocks
        {
            get
            {
                int width;
                int height;
                GetSizeInBlocks(out width, out height);
                return new Size2(width, height);
            }
        }

        /// <summary>	
        /// <p>[This documentation is preliminary and is subject to change.]</p><p>Requests pixel data as it is natively stored within the DDS file.</p>	
        /// </summary>	
        /// <param name="boundsInBlocks"><dd>  <p>The rectangle to copy from the source. A <c>null</c> value specifies the entire texture.</p> <p>If the texture uses a block-compressed <strong><see cref="SharpDX.DXGI.Format"/></strong>, all values of the rectangle are expressed in number of blocks, not pixels.</p> </dd></param>	
        /// <param name="stride"><dd>  <p>The stride, in bytes, of the destination buffer. This represents the number of bytes from the buffer reference to the next row of data. If the texture uses a block-compressed <strong><see cref="SharpDX.DXGI.Format"/></strong>, a "row of data" is defined as a row of blocks which contains multiple pixel scanlines.</p> </dd></param>	
        /// <param name="destination"><dd>  <p>A reference to the destination buffer.</p> </dd></param>	
        /// <returns><p>If this method succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p></returns>	
        /// <remarks>	
        /// <p>If the texture does not use a block-compressed <strong><see cref="SharpDX.DXGI.Format"/></strong>, this method behaves similarly to <strong><see cref="SharpDX.WIC.BitmapSource.CopyPixels"/></strong>. However, it does not perform any pixel format conversion, and instead produces the raw data from the DDS file.</p><p>If the texture uses a block-compressed <strong><see cref="SharpDX.DXGI.Format"/></strong>, this method copies the block data directly into the provided buffer. In this case, the <em>prcBoundsInBlocks</em> parameter is defined in blocks, not pixels. To determine if this is the case, call <strong>GetFormatInfo</strong> and read the <strong>DxgiFormat</strong> member of the returned <strong><see cref="SharpDX.WIC.DdsFormatInfo"/></strong> structure.	
        /// </p>	
        /// </remarks>	
        /// <include file='..\Documentation\CodeComments.xml' path="/comments/comment[@id='IWICDdsFrameDecode::CopyBlocks']/*"/>	
        /// <msdn-id>dn302087</msdn-id>	
        /// <unmanaged>HRESULT IWICDdsFrameDecode::CopyBlocks([In, Optional] const WICRect* prcBoundsInBlocks,[In] unsigned int cbStride,[In] unsigned int cbBufferSize,[In] void* pbBuffer)</unmanaged>	
        /// <unmanaged-short>IWICDdsFrameDecode::CopyBlocks</unmanaged-short>	
        public void CopyBlocks(RawBox? boundsInBlocks, int stride, DataStream destination)
        {
            CopyBlocks(boundsInBlocks, stride, (int)(destination.Length - destination.Position), destination.PositionPointer);
        }
    }
}