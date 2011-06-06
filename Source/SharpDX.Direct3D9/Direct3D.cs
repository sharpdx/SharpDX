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

namespace SharpDX.Direct3D9
{
    public partial class Direct3D {
        /// <summary>	
        /// Create an IDirect3D9 object and return an interface to it.	
        /// </summary>	
        /// <remarks>	
        ///  The Direct3D object is the first Direct3D COM object that your graphical application needs to create and the last object that your application needs to release. Functions for enumerating and retrieving capabilities of a device are accessible through the Direct3D object. This enables applications to select devices without creating them. Create an IDirect3D9 object as shown here: 	
        /// <code> LPDIRECT3D9 g_pD3D = NULL; if( NULL == (g_pD3D = Direct3DCreate9(D3D_SDK_VERSION))) return E_FAIL; </code>	
        /// 	
        ///  The IDirect3D9 interface supports enumeration of active display adapters and allows the creation of <see cref="SharpDX.Direct3D9.Device"/> objects. If the user dynamically adds adapters (either by adding devices to the desktop, or by hot-docking a laptop), those devices will not be included in the enumeration. Creating a new IDirect3D9 interface will expose the new devices. D3D_SDK_VERSION is passed to this function to ensure that the header files against which an application is compiled match the version of the runtime DLL's that are installed on the machine. D3D_SDK_VERSION is only changed in the runtime when a header change (or other code change) would require an application to be rebuilt. If this function fails, it indicates that the header file version does not match the runtime DLL version. For an example, see {{Creating a Device (Direct3D 9)}}. 	
        /// </remarks>
        public Direct3D()
        {
            FromTemp(D3D9.Direct3DCreate9(D3D9.SdkVersion));
        }
    }
}
