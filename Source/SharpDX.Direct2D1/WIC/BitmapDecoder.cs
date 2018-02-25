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

using SharpDX.IO;
using SharpDX.Win32;

namespace SharpDX.WIC
{
    public partial class BitmapDecoder 
    {
        private WICStream internalWICStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapDecoder"/> class from a <see cref="BitmapDecoderInfo"/>.
        /// </summary>
        /// <param name="bitmapDecoderInfo">The bitmap decoder info.</param>
        /// <unmanaged>HRESULT IWICBitmapDecoderInfo::CreateInstance([Out, Fast] IWICBitmapDecoder** ppIBitmapDecoder)</unmanaged>
        public BitmapDecoder(BitmapDecoderInfo bitmapDecoderInfo)
        {
            bitmapDecoderInfo.CreateInstance(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapDecoder"/> class from a guid. <see cref="BitmapDecoderGuids"/> for a list of default supported decoder.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="containerFormatGuid">The container format GUID.</param>
        /// <unmanaged>HRESULT IWICImagingFactory::CreateDecoder([In] const GUID&amp; guidContainerFormat,[In, Optional] const GUID* pguidVendor,[Out, Fast] IWICBitmapDecoder** ppIDecoder)</unmanaged>	
        public BitmapDecoder(ImagingFactory factory, Guid containerFormatGuid)
        {
            factory.CreateDecoder(containerFormatGuid, null, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapDecoder"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="containerFormatGuid">The container format GUID.</param>
        /// <param name="guidVendorRef">The GUID vendor ref.</param>
        /// <unmanaged>HRESULT IWICImagingFactory::CreateDecoder([In] const GUID&amp; guidContainerFormat,[In, Optional] const GUID* pguidVendor,[Out, Fast] IWICBitmapDecoder** ppIDecoder)</unmanaged>	
        public BitmapDecoder(ImagingFactory factory, Guid containerFormatGuid, System.Guid guidVendorRef)
        {
            factory.CreateDecoder(containerFormatGuid, guidVendorRef, this);        
    
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapDecoder"/> class from a <see cref="IStream"/>.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="streamRef">The stream ref.</param>
        /// <param name="metadataOptions">The metadata options.</param>
        /// <unmanaged>HRESULT IWICImagingFactory::CreateDecoderFromStream([In, Optional] IStream* pIStream,[In, Optional] const GUID* pguidVendor,[In] WICDecodeOptions metadataOptions,[Out, Fast] IWICBitmapDecoder** ppIDecoder)</unmanaged>
        public BitmapDecoder(ImagingFactory factory, IStream streamRef, SharpDX.WIC.DecodeOptions metadataOptions)
        {
            factory.CreateDecoderFromStream(streamRef, null, metadataOptions, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapDecoder"/> class from a <see cref="IStream"/>.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="streamRef">The stream ref.</param>
        /// <param name="metadataOptions">The metadata options.</param>
        /// <unmanaged>HRESULT IWICImagingFactory::CreateDecoderFromStream([In, Optional] IStream* pIStream,[In, Optional] const GUID* pguidVendor,[In] WICDecodeOptions metadataOptions,[Out, Fast] IWICBitmapDecoder** ppIDecoder)</unmanaged>
        public BitmapDecoder(ImagingFactory factory, Stream streamRef, SharpDX.WIC.DecodeOptions metadataOptions)
        {
            internalWICStream = new WICStream(factory, streamRef);
            factory.CreateDecoderFromStream(internalWICStream, null, metadataOptions, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapDecoder"/> class from a <see cref="IStream"/>.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="streamRef">The stream ref.</param>
        /// <param name="guidVendorRef">The GUID vendor ref.</param>
        /// <param name="metadataOptions">The metadata options.</param>
        /// <unmanaged>HRESULT IWICImagingFactory::CreateDecoderFromStream([In, Optional] IStream* pIStream,[In, Optional] const GUID* pguidVendor,[In] WICDecodeOptions metadataOptions,[Out, Fast] IWICBitmapDecoder** ppIDecoder)</unmanaged>
        public BitmapDecoder(ImagingFactory factory, IStream streamRef, System.Guid guidVendorRef, SharpDX.WIC.DecodeOptions metadataOptions)
        {
            factory.CreateDecoderFromStream(streamRef, guidVendorRef, metadataOptions, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapDecoder"/> class from a <see cref="IStream"/>.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="streamRef">The stream ref.</param>
        /// <param name="guidVendorRef">The GUID vendor ref.</param>
        /// <param name="metadataOptions">The metadata options.</param>
        /// <unmanaged>HRESULT IWICImagingFactory::CreateDecoderFromStream([In, Optional] IStream* pIStream,[In, Optional] const GUID* pguidVendor,[In] WICDecodeOptions metadataOptions,[Out, Fast] IWICBitmapDecoder** ppIDecoder)</unmanaged>
        public BitmapDecoder(ImagingFactory factory, Stream streamRef, System.Guid guidVendorRef, SharpDX.WIC.DecodeOptions metadataOptions)
        {
            internalWICStream = new WICStream(factory, streamRef);
            factory.CreateDecoderFromStream(internalWICStream, guidVendorRef, metadataOptions, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapDecoder"/> class from a file in read mode.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="metadataOptions">The metadata options.</param>
        /// <unmanaged>HRESULT IWICImagingFactory::CreateDecoderFromFilename([In] const wchar_t* wzFilename,[In, Optional] const GUID* pguidVendor,[In] unsigned int dwDesiredAccess,[In] WICDecodeOptions metadataOptions,[Out, Fast] IWICBitmapDecoder** ppIDecoder)</unmanaged>
        public BitmapDecoder(ImagingFactory factory, string filename, SharpDX.WIC.DecodeOptions metadataOptions)
            : this(factory, filename, null, NativeFileAccess.Read, metadataOptions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapDecoder"/> class from a file.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="desiredAccess">The desired access.</param>
        /// <param name="metadataOptions">The metadata options.</param>
        /// <unmanaged>HRESULT IWICImagingFactory::CreateDecoderFromFilename([In] const wchar_t* wzFilename,[In, Optional] const GUID* pguidVendor,[In] unsigned int dwDesiredAccess,[In] WICDecodeOptions metadataOptions,[Out, Fast] IWICBitmapDecoder** ppIDecoder)</unmanaged>
        public BitmapDecoder(ImagingFactory factory, string filename, NativeFileAccess desiredAccess, SharpDX.WIC.DecodeOptions metadataOptions) : this(factory, filename, null, desiredAccess, metadataOptions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapDecoder"/> class from a file.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="guidVendorRef">The GUID vendor ref.</param>
        /// <param name="desiredAccess">The desired access.</param>
        /// <param name="metadataOptions">The metadata options.</param>
        /// <unmanaged>HRESULT IWICImagingFactory::CreateDecoderFromFilename([In] const wchar_t* wzFilename,[In, Optional] const GUID* pguidVendor,[In] unsigned int dwDesiredAccess,[In] WICDecodeOptions metadataOptions,[Out, Fast] IWICBitmapDecoder** ppIDecoder)</unmanaged>
        public BitmapDecoder(ImagingFactory factory, string filename, System.Guid? guidVendorRef, NativeFileAccess desiredAccess, SharpDX.WIC.DecodeOptions metadataOptions)
        {
            factory.CreateDecoderFromFilename(filename, guidVendorRef, (int)desiredAccess, metadataOptions, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapDecoder"/> class from a file stream.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="fileStream">The filename.</param>
        /// <param name="metadataOptions">The metadata options.</param>
        /// <unmanaged>HRESULT IWICImagingFactory::CreateDecoderFromFileHandle([In] unsigned int hFile,[In, Optional] const GUID* pguidVendor,[In] WICDecodeOptions metadataOptions,[Out, Fast] IWICBitmapDecoder** ppIDecoder)</unmanaged>	
        public BitmapDecoder(ImagingFactory factory, NativeFileStream fileStream, SharpDX.WIC.DecodeOptions metadataOptions)
        {
            factory.CreateDecoderFromFileHandle(fileStream.Handle, null, metadataOptions, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapDecoder"/> class from a file stream.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="fileStream">The filename.</param>
        /// <param name="guidVendorRef">The GUID vendor ref.</param>
        /// <param name="metadataOptions">The metadata options.</param>
        /// <unmanaged>HRESULT IWICImagingFactory::CreateDecoderFromFileHandle([In] unsigned int hFile,[In, Optional] const GUID* pguidVendor,[In] WICDecodeOptions metadataOptions,[Out, Fast] IWICBitmapDecoder** ppIDecoder)</unmanaged>	
        public BitmapDecoder(ImagingFactory factory, NativeFileStream fileStream, System.Guid guidVendorRef, SharpDX.WIC.DecodeOptions metadataOptions)
        {
            factory.CreateDecoderFromFileHandle(fileStream.Handle, guidVendorRef, metadataOptions, this);
        }


        /// <summary>
        /// Initializes the decoder with the provided stream.
        /// </summary>
        /// <param name="stream">The stream to use for initialization.</param>
        /// <param name="cacheOptions">The cache options.</param>
        /// <returns>If the method succeeds, it returns <see cref="Result.Ok"/>. Otherwise, it throws an exception.</returns>
        /// <unmanaged>HRESULT IWICBitmapDecoder::Initialize([In, Optional] IStream* pIStream,[In] WICDecodeOptions cacheOptions)</unmanaged>
        public void Initialize(IStream stream, SharpDX.WIC.DecodeOptions cacheOptions)
        {
            if (this.internalWICStream != null)
                throw new InvalidOperationException("This instance is already initialized with an existing stream");
            Initialize_(stream, cacheOptions);
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

        /// <summary>
        /// Get the <see cref="ColorContext"/> of the image (if any)
        /// </summary>
        /// <param name="imagingFactory">The factory for creating new color contexts</param>
        /// <param name="colorContexts">The color context array, or null</param>
        /// <remarks>
        /// When the image format does not support color contexts,
        /// <see cref="ResultCode.UnsupportedOperation"/> is returned.
        /// </remarks>
        /// <unmanaged>HRESULT IWICBitmapDecoder::GetColorContexts([In] unsigned int cCount,[Out, Buffer, Optional] IWICColorContext** ppIColorContexts,[Out] unsigned int* pcActualCount)</unmanaged>	
        public Result TryGetColorContexts(ImagingFactory imagingFactory, out ColorContext[] colorContexts)
        {
            return ColorContextsHelper.TryGetColorContexts(GetColorContexts, imagingFactory, out colorContexts);
        }

        /// <summary>
        /// Get the <see cref="ColorContext"/> of the image (if any)
        /// </summary>
        /// <returns>
        /// null if the decoder does not support color contexts;
        /// otherwise an array of zero or more ColorContext objects
        /// </returns>
        /// <unmanaged>HRESULT IWICBitmapDecoder::GetColorContexts([In] unsigned int cCount,[Out, Buffer, Optional] IWICColorContext** ppIColorContexts,[Out] </unmanaged>
        public ColorContext[] TryGetColorContexts(ImagingFactory imagingFactory)
        {
            return ColorContextsHelper.TryGetColorContexts(GetColorContexts, imagingFactory);
        }

        [Obsolete("Use TryGetColorContexts instead")]
        public ColorContext[] ColorContexts
        {
            get { return new ColorContext[0]; }
        }
    }
}