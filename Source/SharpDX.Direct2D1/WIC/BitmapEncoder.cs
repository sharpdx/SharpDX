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
using SharpDX.Win32;

namespace SharpDX.WIC
{
    public partial class BitmapEncoder
    {
        private ImagingFactory factory;
        private WICStream internalWICStream;


        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapEncoder"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="containerFormatGuid">The container format GUID. List from <see cref="ContainerFormatGuids"/> </param>
        /// <unmanaged>HRESULT IWICImagingFactory::CreateEncoder([In] const GUID&amp; guidContainerFormat,[In, Optional] const GUID* pguidVendor,[Out] IWICBitmapEncoder** ppIEncoder)</unmanaged>	
        public BitmapEncoder(ImagingFactory factory, Guid containerFormatGuid)
        {
            this.factory = factory;
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
            this.factory = factory;
            factory.CreateEncoder(containerFormatGuid, guidVendorRef, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapEncoder"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="containerFormatGuid">The container format GUID. List from <see cref="ContainerFormatGuids"/> </param>
        /// <param name="stream">A stream to use as the output of this bitmap encoder.</param>
        /// <unmanaged>HRESULT IWICImagingFactory::CreateEncoder([In] const GUID&amp; guidContainerFormat,[In, Optional] const GUID* pguidVendor,[Out] IWICBitmapEncoder** ppIEncoder)</unmanaged>	
        public BitmapEncoder(ImagingFactory factory, Guid containerFormatGuid, WICStream stream = null)
        {
            this.factory = factory;
            factory.CreateEncoder(containerFormatGuid, null, this);
            if (stream != null)
                Initialize(stream);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapEncoder"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="containerFormatGuid">The container format GUID. List from <see cref="ContainerFormatGuids"/> </param>
        /// <param name="stream">A stream to use as the output of this bitmap encoder.</param>
        /// <unmanaged>HRESULT IWICImagingFactory::CreateEncoder([In] const GUID&amp; guidContainerFormat,[In, Optional] const GUID* pguidVendor,[Out] IWICBitmapEncoder** ppIEncoder)</unmanaged>	
        public BitmapEncoder(ImagingFactory factory, Guid containerFormatGuid, System.IO.Stream stream = null)
        {
            this.factory = factory;
            factory.CreateEncoder(containerFormatGuid, null, this);
            if (stream != null)
                Initialize(stream);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapEncoder"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="containerFormatGuid">The container format GUID. List from <see cref="ContainerFormatGuids"/></param>
        /// <param name="guidVendorRef">The GUID vendor ref.</param>
        /// <param name="stream">A stream to use as the output of this bitmap encoder.</param>
        /// <unmanaged>HRESULT IWICImagingFactory::CreateEncoder([In] const GUID&amp; guidContainerFormat,[In, Optional] const GUID* pguidVendor,[Out] IWICBitmapEncoder** ppIEncoder)</unmanaged>	
        public BitmapEncoder(ImagingFactory factory, Guid containerFormatGuid, System.Guid guidVendorRef, WICStream stream = null)
        {
            this.factory = factory;
            factory.CreateEncoder(containerFormatGuid, guidVendorRef, this);
            if (stream != null)
                Initialize(stream);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapEncoder"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="containerFormatGuid">The container format GUID. List from <see cref="ContainerFormatGuids"/></param>
        /// <param name="guidVendorRef">The GUID vendor ref.</param>
        /// <param name="stream">A stream to use as the output of this bitmap encoder.</param>
        /// <unmanaged>HRESULT IWICImagingFactory::CreateEncoder([In] const GUID&amp; guidContainerFormat,[In, Optional] const GUID* pguidVendor,[Out] IWICBitmapEncoder** ppIEncoder)</unmanaged>	
        public BitmapEncoder(ImagingFactory factory, Guid containerFormatGuid, System.Guid guidVendorRef, System.IO.Stream stream = null)
        {
            this.factory = factory;
            factory.CreateEncoder(containerFormatGuid, guidVendorRef, this);
            if (stream != null)
                Initialize(stream);
        }

        /// <summary>
        /// Initializes the encoder with the provided stream.
        /// </summary>
        /// <param name="stream">The stream to use for initialization.</param>
        /// <returns>If the method succeeds, it returns <see cref="Result.Ok"/>. Otherwise, it throws an exception.</returns>
        /// <unmanaged>HRESULT IWICBitmapEncoder::Initialize([In, Optional] IStream* pIStream,[In] WICBitmapEncoderCacheOption cacheOption)</unmanaged>	
        public void Initialize(IStream stream)
        {
            if (this.internalWICStream != null)
                throw new InvalidOperationException("This instance is already initialized with an existing stream");
            Initialize(stream, SharpDX.WIC.BitmapEncoderCacheOption.NoCache);
        }

        /// <summary>
        /// Initializes the encoder with the provided stream.
        /// </summary>
        /// <param name="stream">The stream to use for initialization.</param>
        /// <returns>If the method succeeds, it returns <see cref="Result.Ok"/>. Otherwise, it throws an exception.</returns>
        /// <unmanaged>HRESULT IWICBitmapEncoder::Initialize([In, Optional] IStream* pIStream,[In] WICBitmapEncoderCacheOption cacheOption)</unmanaged>	
        public void Initialize(System.IO.Stream stream)
        {
            if (this.internalWICStream != null)
                throw new InvalidOperationException("This instance is already initialized with an existing stream");
            this.internalWICStream = new WICStream(factory, stream);
            Initialize(this.internalWICStream, SharpDX.WIC.BitmapEncoderCacheOption.NoCache);
        }

        /// <summary>
        /// Sets the <see cref="ColorContext"/> objects for the encoder.
        /// </summary>
        /// <param name="colorContextOut">The color contexts to set for the encoder.</param>
        /// <returns>If the method succeeds, it returns <see cref="Result.Ok"/>. Otherwise, it throws an exception.</returns>
        /// <unmanaged>HRESULT IWICBitmapEncoder::SetColorContexts([In] unsigned int cCount,[In, Buffer] IWICColorContext** ppIColorContext)</unmanaged>
        public void SetColorContexts(SharpDX.WIC.ColorContext[] colorContextOut)
        {
            SetColorContexts(colorContextOut != null ? colorContextOut.Length : 0, colorContextOut);
        }

        protected override unsafe void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (this.internalWICStream != null)
                    internalWICStream.Dispose();
                internalWICStream = null;
            }
        }
    }
}