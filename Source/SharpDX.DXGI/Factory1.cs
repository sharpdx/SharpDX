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
using System.Collections.Generic;

namespace SharpDX.DXGI
{
    public partial class Factory1
    {
        /// <summary>
        ///   Default Constructor for Factory1.
        /// </summary>
        public Factory1() : base(IntPtr.Zero)
        {
            IntPtr factoryPtr;
            DXGI.CreateDXGIFactory1(Utilities.GetGuidFromType(GetType()), out factoryPtr);
            NativePointer = factoryPtr;
        }

        /// <summary>	
        /// Gets both adapters (video cards) with or without outputs.	
        /// </summary>	
        /// <param name="index"><para>The index of the adapter to enumerate.</para></param>	
        /// <returns>a reference to an <see cref="SharpDX.DXGI.Adapter1"/> interface at the position specified by the Adapter parameter</returns>	
        /// <remarks>	
        /// This method is not supported by DXGI 1.0, which shipped in Windows?Vista and Windows Server?2008. DXGI 1.1 support is required, which is available on  Windows?7, Windows Server?2008?R2, and as an update to Windows?Vista with Service Pack?2 (SP2) (KB 971644) and Windows Server?2008 (KB 971512).When you create a factory, the factory enumerates the set of adapters that are available in the system. Therefore, if you change the adapters in a system, you must destroy  and recreate the <see cref="SharpDX.DXGI.Factory1"/> object. The number of adapters in a system changes when you add or remove a display card, or dock or undock a laptop.When the EnumAdapters1 method succeeds and fills the ppAdapter parameter with the address of the reference to the adapter interface, EnumAdapters1 increments the adapter interface's reference count. When you finish using the  adapter interface, call the Release method to decrement the reference count before you destroy the reference.EnumAdapters1 first returns the local adapter with the output on which the desktop primary is displayed. This adapter corresponds with an index of zero. EnumAdapters1 next returns other adapters with outputs. EnumAdapters1 finally returns adapters without outputs.	
        /// </remarks>	
        /// <unmanaged>HRESULT IDXGIFactory1::EnumAdapters1([In] unsigned int Adapter,[Out] IDXGIAdapter1** ppAdapter)</unmanaged>	
        public Adapter1 GetAdapter1(int index)
        {
            Adapter1 adapter;
            GetAdapter1(index, out adapter).CheckError();
            return adapter;
        }

        /// <summary>
        /// Return an array of <see cref="Adapter1"/> available from this factory.
        /// </summary>
        /// <unmanaged>HRESULT IDXGIFactory1::EnumAdapters1([In] unsigned int Adapter,[Out] IDXGIAdapter1** ppAdapter)</unmanaged>	
        public Adapter1[] Adapters1
        {
            get
            {
                var adapters = new List<Adapter1>();
                do
                {
                    Adapter1 adapter;
                    var result = GetAdapter1(adapters.Count, out adapter);
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
        /// <unmanaged>HRESULT IDXGIFactory1::EnumAdapters1([In] unsigned int Adapter,[Out] IDXGIAdapter1** ppAdapter)</unmanaged>	
        public int GetAdapterCount1()
        {
            int nbAdapters = 0;
            do
            {
                Adapter1 adapter;
                var result = GetAdapter1(nbAdapters, out adapter);
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