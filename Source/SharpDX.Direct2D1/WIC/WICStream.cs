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
    public partial class WICStream
    {
        private ComStreamProxy streamProxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="WICStream"/> class from a file.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileAccess">The file access.</param>
        /// <msdn-id>ee690325</msdn-id>	
        /// <unmanaged>HRESULT IWICImagingFactory::CreateStream([Out, Fast] IWICStream** ppIWICStream)</unmanaged>	
        public WICStream(ImagingFactory factory, string fileName, NativeFileAccess fileAccess)
            : base(IntPtr.Zero)
        {
            factory.CreateStream(this);
            InitializeFromFilename(fileName, (int)fileAccess);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WICStream"/> class from a <see cref="IStream"/>.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="stream">The stream.</param>
        /// <msdn-id>ee719789</msdn-id>	
        /// <unmanaged>HRESULT IWICStream::InitializeFromIStream([In, Optional] IStream* pIStream)</unmanaged>	
        /// <unmanaged-short>IWICStream::InitializeFromIStream</unmanaged-short>	
        public WICStream(ImagingFactory factory, Stream stream)
            : base(IntPtr.Zero)
        {
            factory.CreateStream(this);
            streamProxy = new ComStreamProxy(stream);
            InitializeFromIStream(streamProxy);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WICStream"/> class from an unmanaged memory through a <see cref="DataStream"/>.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="dataStream">The unmanaged memory stream.</param>
        /// <msdn-id>ee719792</msdn-id>	
        /// <unmanaged>HRESULT IWICStream::InitializeFromMemory([In] void* pbBuffer,[In] unsigned int cbBufferSize)</unmanaged>	
        /// <unmanaged-short>IWICStream::InitializeFromMemory</unmanaged-short>	
        public WICStream(ImagingFactory factory, DataPointer dataStream)
            : base(IntPtr.Zero)
        {
            factory.CreateStream(this);
            InitializeFromMemory(dataStream.Pointer, dataStream.Size);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (streamProxy != null)
            {
                streamProxy.Dispose();
                streamProxy = null;
            }
        }
    }
}
