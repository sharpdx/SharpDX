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
using SharpDX.Win32;

namespace SharpDX.WIC
{
    public partial class BitmapEncoder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapEncoder"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="containerFormatGuid">The container format GUID. List from <see cref="ContainerFormatGuids"/> </param>
        /// <unmanaged>HRESULT IWICImagingFactory::CreateEncoder([In] const GUID&amp; guidContainerFormat,[In, Optional] const GUID* pguidVendor,[Out] IWICBitmapEncoder** ppIEncoder)</unmanaged>	
        public BitmapEncoder(ImagingFactory factory, Guid containerFormatGuid)
        {
            factory.CreateEncoder(containerFormatGuid, null, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapEncoder"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="containerFormatGuid">The container format GUID. List from <see cref="ContainerFormatGuids"/></param>
        /// <param name="guidVendorRef">The GUID vendor ref.</param>
        /// <unmanaged>HRESULT IWICImagingFactory::CreateEncoder([In] const GUID&amp; guidContainerFormat,[In, Optional] const GUID* pguidVendor,[Out] IWICBitmapEncoder** ppIEncoder)</unmanaged>	
        public BitmapEncoder(ImagingFactory factory, Guid containerFormatGuid, System.Guid guidVendorRef)
        {
            factory.CreateEncoder(containerFormatGuid, guidVendorRef, this);
        }

        /// <summary>
        /// Initializes the encoder with the provided stream.
        /// </summary>
        /// <param name="stream">The stream to use for initialization.</param>
        /// <returns>If the method succeeds, it returns <see cref="Result.Ok"/>. Otherwise, it throws an exception.</returns>
        /// <unmanaged>HRESULT IWICBitmapEncoder::Initialize([In, Optional] IStream* pIStream,[In] WICBitmapEncoderCacheOption cacheOption)</unmanaged>	
        public SharpDX.Result Initialize(IStream stream)
        {
            return Initialize_(ComStream.ToComPointer(stream), SharpDX.WIC.BitmapEncoderCacheOption.NoCache);
        }

        /// <summary>
        /// Sets the <see cref="ColorContext"/> objects for the encoder.
        /// </summary>
        /// <param name="colorContextOut">The color contexts to set for the encoder.</param>
        /// <returns>If the method succeeds, it returns <see cref="Result.Ok"/>. Otherwise, it throws an exception.</returns>
        /// <unmanaged>HRESULT IWICBitmapEncoder::SetColorContexts([In] unsigned int cCount,[In, Buffer] IWICColorContext** ppIColorContext)</unmanaged>
        public SharpDX.Result SetColorContexts(SharpDX.WIC.ColorContext[] colorContextOut)
        {
            return SetColorContexts(colorContextOut != null ? colorContextOut.Length : 0, colorContextOut);
        }
    }
}