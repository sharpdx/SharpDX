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
using SharpDX.Mathematics.Interop;

namespace SharpDX.WIC
{
    public partial class BitmapClipper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapClipper"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public BitmapClipper(ImagingFactory factory) : base(IntPtr.Zero)
        {
            factory.CreateBitmapClipper(this);
        }

        /// <summary>	
        /// <p>Initializes the bitmap clipper with the provided parameters.</p>	
        /// </summary>	
        /// <param name="sourceRef"><dd>  <p>he input bitmap source.</p> </dd></param>	
        /// <param name="rectangleRef"><dd>  <p>The rectangle of the bitmap source to clip.</p> </dd></param>	
        /// <returns><p>If this method succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p></returns>	
        /// <msdn-id>ee719677</msdn-id>	
        /// <unmanaged>HRESULT IWICBitmapClipper::Initialize([In, Optional] IWICBitmapSource* pISource,[In] const WICRect* prc)</unmanaged>	
        /// <unmanaged-short>IWICBitmapClipper::Initialize</unmanaged-short>	
        public unsafe void Initialize(SharpDX.WIC.BitmapSource sourceRef, RawBox rectangleRef)
        {
            Initialize(sourceRef, new IntPtr(&rectangleRef));
        }
    }
}