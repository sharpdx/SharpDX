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
namespace SharpDX.DXGI
{
    public partial class SwapChain2
    {
        /// <summary>	
        /// <p>[This documentation is preliminary and is subject to change.]</p><p>Gets the source region used for the swap chain.</p><p>Use <strong>GetSourceSize</strong> to get the portion of the swap chain from which the operating system presents. The source rectangle is always defined by the region [0, 0, Width, Height]. Use <strong>SetSourceSize</strong> to set this portion of the swap chain. </p>	
        /// </summary>	
        /// <returns><p> This method can return error codes that are described in the DXGI_ERROR topic.</p></returns>	
        /// <include file='.\Documentation\CodeComments.xml' path="/comments/comment[@id='IDXGISwapChain2::GetSourceSize']/*"/>	
        /// <msdn-id>dn280408</msdn-id>	
        /// <unmanaged>HRESULT IDXGISwapChain2::GetSourceSize([Out] unsigned int* pWidth,[Out] unsigned int* pHeight)</unmanaged>	
        /// <unmanaged-short>IDXGISwapChain2::GetSourceSize</unmanaged-short>	
        public Size2 SourceSize
        {
            get
            {
                int width;
                int height;
                GetSourceSize(out width, out height);
                return new Size2(width, height);
            }
            set
            {
                SetSourceSize(value.Width, value.Height);
            }
        }
    }
}