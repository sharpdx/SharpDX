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
using SharpDX.Win32;

namespace SharpDX.WIC
{
    public partial class MetadataQueryWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataQueryWriter"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="guidMetadataFormat">The GUID metadata format.</param>
        public MetadataQueryWriter(ImagingFactory factory, System.Guid guidMetadataFormat)
            : base(IntPtr.Zero)
        {
            factory.CreateQueryWriter(guidMetadataFormat, null, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataQueryWriter"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="guidMetadataFormat">The GUID metadata format.</param>
        /// <param name="guidVendorRef">The GUID vendor ref.</param>
        /// <unmanaged>HRESULT IWICImagingFactory::CreateQueryWriter([In] const GUID&amp; guidMetadataFormat,[In, Optional] const GUID* pguidVendor,[Out, Fast] IWICMetadataQueryWriter** ppIQueryWriter)</unmanaged>
        public MetadataQueryWriter(ImagingFactory factory, System.Guid guidMetadataFormat, System.Guid guidVendorRef)
            : base(IntPtr.Zero)
        {
            factory.CreateQueryWriter(guidMetadataFormat, guidVendorRef, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataQueryWriter"/> class from a <see cref="MetadataQueryReader"/>.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="metadataQueryReader">The metadata query reader.</param>
        public MetadataQueryWriter(ImagingFactory factory, MetadataQueryReader metadataQueryReader)
            : base(IntPtr.Zero)
        {
            factory.CreateQueryWriterFromReader(metadataQueryReader, null, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataQueryWriter"/> class from a <see cref="MetadataQueryReader"/>.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="metadataQueryReader">The metadata query reader.</param>
        /// <param name="guidVendorRef">The GUID vendor ref.</param>
        public MetadataQueryWriter(ImagingFactory factory, MetadataQueryReader metadataQueryReader, System.Guid guidVendorRef)
            : base(IntPtr.Zero)
        {
            factory.CreateQueryWriterFromReader(metadataQueryReader, guidVendorRef, this);
        }


        /// <summary>
        /// Sets the value for a metadata name
        /// </summary>
        /// <param name="name">The name of the metadata.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public unsafe void SetMetadataByName(string name, object value)
        {
            byte* variant = stackalloc byte[512];

            var variantStruct = (Variant*)variant;
            variantStruct->Value = value;

            SetMetadataByName(name, (IntPtr) variant);
        }
    }
}