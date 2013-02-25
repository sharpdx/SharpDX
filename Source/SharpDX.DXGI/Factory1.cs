// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
using System.Collections.Generic;

namespace SharpDX.DXGI
{
    public partial class Factory1
    {
        private readonly object lockAdapters1 = new object();
        private Adapter1[] adapters1;

        /// <summary>
        ///   Default Constructor for Factory1.
        /// </summary>
        public Factory1() : base(IntPtr.Zero)
        {
            IntPtr factoryPtr;
            DXGI.CreateDXGIFactory1(Utilities.GetGuidFromType(GetType()), out factoryPtr);
            SetDefaultInstance(factoryPtr, this);
        }

        /// <summary>
        /// Return an array of <see cref="Adapter1" /> available from this factory.
        /// </summary>
        /// <value>The adapters1.</value>
        /// <unmanaged>HRESULT IDXGIFactory1::EnumAdapters1([In] unsigned int Adapter,[Out] IDXGIAdapter1** ppAdapter)</unmanaged>
        ///   <msdn-id>ff471336</msdn-id>
        ///   <unmanaged>HRESULT IDXGIFactory1::EnumAdapters1([In] unsigned int Adapter,[Out] IDXGIAdapter1** ppAdapter)</unmanaged>
        ///   <unmanaged-short>IDXGIFactory1::EnumAdapters1</unmanaged-short>
        /// <remarks>
        /// Adapters are cached on the first call.
        /// </remarks>
        public Adapter1[] Adapters1
        {
            get
            {
                lock (lockAdapters1)
                {
                    if (adapters1 == null)
                    {
                        var adapters = new List<Adapter1>();
                        do
                        {
                            Adapter1 adapter;
                            var result = GetAdapter1(adapters.Count, out adapter);
                            if (result == ResultCode.NotFound) break;
                            adapters.Add(adapter);
                        }
                        while (true);
                        adapters1 = adapters.ToArray();
                    }
                    return adapters1;
                }
            }
        }

        protected override void DisposeCachedMembers()
        {
            Utilities.Release(ref adapters1);
            base.DisposeCachedMembers();
        }
    }
}