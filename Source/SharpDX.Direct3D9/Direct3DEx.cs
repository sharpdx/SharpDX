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

namespace SharpDX.Direct3D9
{
    public partial class Direct3DEx
    {
        /// <summary>	
        /// Creates an <see cref="SharpDX.Direct3D9.Direct3DEx"/> object and returns an interface to it.	
        /// </summary>	
        /// <remarks>	
        /// The <see cref="SharpDX.Direct3D9.Direct3DEx"/> object is the first object that the application creates and the last object that the application releases. Functions for enumerating and retrieving capabilities of a device are accessible through the IDirect3D9Ex object. This enables applications to select devices without creating them.   The <see cref="SharpDX.Direct3D9.Direct3DEx"/> interface supports enumeration of active display adapters and allows the creation of IDirect3D9Ex objects. If the user dynamically adds adapters (either by adding devices to the desktop, or by hot-docking a laptop), these devices are not included in the enumeration. Creating a new IDirect3D9Ex interface will expose the new devices.   Pass the D3D_SDK_VERSION flag to this function to ensure that header files used in the compiled application match the version of the installed runtime DLLs. D3D_SDK_VERSION is changed in the runtime only when a header or another code change would require rebuilding the application. If this function fails, it indicates that the versions of the header file and the runtime DLL do not match.  Note??Direct3DCreate9Ex is supported only in Windows Vista, Windows Server 2008, and Windows 7.   Earlier versions of the D3D9.dll library do not include Direct3D9Ex and Direct3DCreate9Ex.  	
        /// </remarks>	
        /// <returns>D3DERR_NOTAVAILABLE if Direct3DEx features are not supported (no WDDM driver is installed) or if the SDKVersion does not match the version of the DLL.   D3DERR_OUTOFMEMORY if out-of-memory conditions are detected when creating the enumerator object.  S_OK if the creation of the enumerator object is successful.  </returns>
        /// <unmanaged>HRESULT Direct3DCreate9Ex([None] int SDKVersion,[None] IDirect3D9Ex** arg1)</unmanaged>
        public Direct3DEx() : base(IntPtr.Zero)
        {
            D3D9.Create9Ex(D3D9.SdkVersion, this);
            Adapters = new AdapterCollection(this);
            AdaptersEx = new AdapterExCollection(this);
        }

        /// <summary>
        /// Retrieves the current display mode and rotation settings of the adapter.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        /// <returns><see cref="DisplayModeEx"/> structure containing data about the display mode of the adapter</returns>
        /// <unmanaged>HRESULT IDirect3D9Ex::GetAdapterDisplayModeEx([In] unsigned int Adapter,[Out] D3DDISPLAYMODEEX* pMode,[Out] D3DDISPLAYROTATION* pRotation)</unmanaged>
        public SharpDX.Direct3D9.DisplayModeEx GetAdapterDisplayModeEx(int adapter)
        {
            SharpDX.Direct3D9.DisplayRotation rotationRef;
            return GetAdapterDisplayModeEx(adapter, out rotationRef);
        }

        /// <summary>
        /// Gets a collection of installed extended adapters.
        /// </summary>
        public AdapterExCollection AdaptersEx { get; private set; }
    }
}