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
using System.Reflection;
using System.Runtime.InteropServices;

namespace SharpDX.DXGI
{
    public partial class Factory
    {
        /// <summary>
        ///   Default Constructor for Factory
        /// </summary>
        public Factory() : base(IntPtr.Zero)
        {
            IntPtr factoryPtr;
            DXGI.CreateDXGIFactory(GetType().GUID, out factoryPtr);
            NativePointer = factoryPtr;
        }

        /// <summary>	
        /// Create an adapter interface that represents a software adapter.	
        /// </summary>	
        /// <remarks>	
        /// A software adapter is a DLL that implements the entirety of a device driver interface, plus emulation, if necessary, of kernel-mode graphics components for Windows. Details on implementing a software adapter can be found in the Windows Vista Driver Development Kit. This is a very complex development task, and is not recommended for general readers. Calling this method will increment the module's reference count by one. The reference count can be decremented by calling {{FreeLibrary}}. The typical calling scenario is to call {{LoadLibrary}}, pass the handle to CreateSoftwareAdapter, then immediately call {{FreeLibrary}} on the DLL and forget the DLL's {{HMODULE}}. Since the software adapter calls FreeLibrary when it is destroyed, the lifetime of the DLL will now be owned by the adapter, and the application is free of any further consideration of its lifetime. 	
        /// </remarks>	
        /// <param name="module">Handle to the software adapter's dll.</param>
        /// <returns>A reference to an adapter (see <see cref="T:SharpDX.DXGI.Adapter" />). </returns>
        /// <unmanaged>HRESULT IDXGIFactory::CreateSoftwareAdapter([None] void* Module,[Out] IDXGIAdapter** ppAdapter)</unmanaged>
        public Adapter CreateSoftwareAdapter(Module module)
        {
            return CreateSoftwareAdapter(Marshal.GetHINSTANCE(module));
        }

        /// <summary>
        ///   Return the number of available adapters from this factory.
        /// </summary>
        /// <returns>The number of adapters</returns>
        public int GetAdapterCount()
        {
            int nbAdapters = 0;          
            do
            {
                try
                {
                    var adapter = GetAdapter(nbAdapters);
                    adapter.Dispose();
                }
                catch (SharpDXException exception)
                {
                    if (exception.ResultCode.Code == (int) DXGIError.NotFound)
                        break;
                    throw;
                }
                nbAdapters++;
            } while (true);
            return nbAdapters;
        }
    }
}