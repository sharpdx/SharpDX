﻿// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
using System.Reflection;
using System.Runtime.InteropServices;

namespace SharpDX.DXGI
{
    public partial class Factory
    {
#if !W8CORE
        /// <summary>
        ///   Default Constructor for Factory
        /// </summary>
        public Factory() : base(IntPtr.Zero)
        {
            IntPtr factoryPtr;
            DXGI.CreateDXGIFactory(Utilities.GetGuidFromType(GetType()), out factoryPtr);
            NativePointer = factoryPtr;
        }
        /// <summary>	
        /// Create an adapter interface that represents a software adapter.	
        /// </summary>	
        /// <remarks>	
        /// A software adapter is a DLL that implements the entirety of a device driver interface, plus emulation, if necessary, of kernel-mode graphics components for Windows. Details on implementing a software adapter can be found in the Windows Vista Driver Development Kit. This is a very complex development task, and is not recommended for general readers. Calling this method will increment the module's reference count by one. The reference count can be decremented by calling {{FreeLibrary}}. The typical calling scenario is to call {{LoadLibrary}}, pass the handle to CreateSoftwareAdapter, then immediately call {{FreeLibrary}} on the DLL and forget the DLL's {{HMODULE}}. Since the software adapter calls FreeLibrary when it is destroyed, the lifetime of the DLL will now be owned by the adapter, and the application is free of any further consideration of its lifetime. 	
        /// </remarks>	
        /// <param name="module">Handle to the software adapter's DLL.</param>
        /// <returns>A reference to an adapter (see <see cref="T:SharpDX.DXGI.Adapter" />). </returns>
        /// <unmanaged>HRESULT IDXGIFactory::CreateSoftwareAdapter([None] void* Module,[Out] IDXGIAdapter** ppAdapter)</unmanaged>
        public Adapter CreateSoftwareAdapter(Module module)
        {
            return CreateSoftwareAdapter(Marshal.GetHINSTANCE(module));
        }
#endif
        /// <summary>	
        /// Gets both adapters (video cards) with or without outputs.	
        /// </summary>	
        /// <param name="adapter"><para>The index of the adapter to enumerate.</para></param>	
        /// <returns>a reference to an <see cref="SharpDX.DXGI.Adapter"/> interface at the position specified by the Adapter parameter</returns>
        /// <remarks>	
        /// When you create a factory, the factory enumerates the set of adapters that are available in the system. Therefore, if you change the adapters in a system, you must destroy  and recreate the <see cref="SharpDX.DXGI.Factory"/> object. The number of adapters in a system changes when you add or remove a display card, or dock or undock a laptop.When the EnumAdapters method succeeds and fills the ppAdapter parameter with the address of the reference to the adapter interface, EnumAdapters increments the adapter interface's reference count. When you finish using the  adapter interface, call the Release method to decrement the reference count before you destroy the reference.EnumAdapters first returns the local adapter with the output on which the desktop primary is displayed. This adapter corresponds with an index of zero. EnumAdapters then returns other adapters with outputs.	
        /// </remarks>	
        /// <unmanaged>HRESULT IDXGIFactory::EnumAdapters([In] unsigned int Adapter,[Out] IDXGIAdapter** ppAdapter)</unmanaged>	
        public Adapter GetAdapter(int index)
        {
            Adapter adapter;
            GetAdapter(index, out adapter).CheckError();
            return adapter;
        }

        /// <summary>
        /// Return an array of <see cref="Adapter"/> available from this factory.
        /// </summary>
        /// <unmanaged>HRESULT IDXGIFactory::EnumAdapters([In] unsigned int Adapter,[Out] IDXGIAdapter** ppAdapter)</unmanaged>	
        public Adapter[] Adapters
        {
            get
            {
                var adapters = new List<Adapter>();
                do
                {
                    Adapter adapter;
                    var result = GetAdapter(adapters.Count, out adapter);
                    if (result == ResultCode.NotFound)
                        break;
                    adapters.Add(adapter);
                } while (true);
                return adapters.ToArray();
            }
        }

        /// <summary>
        ///   Return the number of available adapters from this factory.
        /// </summary>
        /// <returns>The number of adapters</returns>
        /// <unmanaged>HRESULT IDXGIFactory::EnumAdapters([In] unsigned int Adapter,[Out] IDXGIAdapter** ppAdapter)</unmanaged>	
        public int GetAdapterCount()
        {
            int nbAdapters = 0;
            do
            {
                Adapter adapter;
                var result = GetAdapter(nbAdapters, out adapter);
                if (adapter != null)
                    adapter.Dispose();
                if (result == ResultCode.NotFound)
                    break;
                nbAdapters++;
            } while (true);
            return nbAdapters;
        }
    }
}