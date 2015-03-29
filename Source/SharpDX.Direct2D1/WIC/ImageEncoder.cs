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
namespace SharpDX.WIC
{
    public partial class ImageEncoder
    {
        /// <summary>
        /// Creates a new image encoder object.
        /// </summary>
        /// <param name="factory">The WIC factory.</param>
        /// <param name="d2dDevice">The <see cref="Direct2D1.Device" /> object on which the corresponding image encoder is created.</param>
        /// <msdn-id>hh880849</msdn-id>
        ///   <unmanaged>HRESULT IWICImagingFactory2::CreateImageEncoder([In] ID2D1Device* pD2DDevice,[In] IWICImageEncoder** ppWICImageEncoder)</unmanaged>
        ///   <unmanaged-short>IWICImagingFactory2::CreateImageEncoder</unmanaged-short>
        public ImageEncoder(ImagingFactory2 factory, Direct2D1.Device d2dDevice)
        {
            factory.CreateImageEncoder(d2dDevice, this);
        }
    }
}