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

namespace SharpDX.DXGI
{
    public partial class Resource1
    {
        /// <summary>	
        /// Creates a handle to a shared resource. You can then use the returned handle with multiple Direct3D devices. 
        /// </summary>	
        /// <param name="attributesRef"><para>A reference to a <see cref="SharpDX.Win32.SecurityAttributes"/> structure that contains two separate but related data members: an optional security descriptor, and a Boolean  value that determines whether child processes can inherit the returned handle.</para> <para>Set this parameter to <c>null</c> if you want child processes that the  application might create to not  inherit  the handle returned by  CreateSharedHandle, and if you want the resource that is associated with the returned handle to get a default security  descriptor.</para> <para>The lpSecurityDescriptor member of the structure specifies a  SECURITY_DESCRIPTOR for the resource. Set  this member to <c>null</c> if you want the runtime to assign a default security descriptor to the resource that is associated with the returned handle.</para></param>	
        /// <param name="dwAccess"><para>The requested access rights to the resource.  In addition to the generic access rights, DXGI defines the following values:</para>  <see cref="SharpDX.DXGI.SharedResourceFlags.Read"/> ( 0x80000000L ) - specifies read access to the resource. <see cref="SharpDX.DXGI.SharedResourceFlags.Write"/> ( 1 ) - specifies  write access to the resource.  <para>You can combine these values by using a bitwise OR operation.</para></param>	
        /// <param name="name"><para>The name of the resource to share. You will need the  resource name if you  call the <see cref="SharpDX.Direct3D11.Device1.OpenSharedResourceByName"/> method to access the shared resource by name. If you instead  call the <see cref="SharpDX.Direct3D11.Device1.OpenSharedResource1"/> method to access the shared resource by handle, set this parameter to <c>null</c>.</para></param>	
        /// <returns><para>A reference to a variable that receives the NT HANDLE value to the resource to share.  You can  use this handle in calls to access the resource.</para></returns>	
        /// <remarks>	
        /// If you  created the resource as shared and specified that it uses NT handles (that is, you set the <see cref="SharpDX.Direct3D11.ResourceOptionFlags.SharedNthandle"/> flag), you must use CreateSharedHandle to get a handle for sharing.  In this situation, you cannot use the <see cref="SharpDX.DXGI.Resource.GetSharedHandle"/> method because it will fail.  Similarly, if you  created the resource as shared and did not specify that it uses NT handles, you cannot use CreateSharedHandle to get a handle for sharing because CreateSharedHandle will fail.You can pass the handle that  CreateSharedHandle returns in a call to the <see cref="SharpDX.Direct3D11.Device1.OpenSharedResourceByName"/> or <see cref="SharpDX.Direct3D11.Device1.OpenSharedResource1"/> method to give a device access to a shared resource that you created on a different device.CreateSharedHandle only returns the NT handle when you  created the resource as shared (that is, you set the <see cref="SharpDX.Direct3D11.ResourceOptionFlags.SharedNthandle"/> and <see cref="SharpDX.Direct3D11.ResourceOptionFlags.SharedKeyedmutex"/> flags).Because the handle that  CreateSharedHandle returns is an NT handle, you can use the handle with CloseHandle, DuplicateHandle, and so on. You can call CreateSharedHandle only once for a shared resource; later calls fail.  If you need more handles to the same shared resource, call DuplicateHandle. When you no longer need the shared resource handle, call CloseHandle to close the handle, in order to avoid memory leaks.The creator of a shared resource must not destroy the resource until all entities that  opened the resource have destroyed the resource. The validity of the handle is tied to the lifetime of the underlying video memory. If no resource objects exist on any devices that refer to this resource, the handle is no longer valid. To extend the lifetime of the handle and video memory, you must open the shared resource on a device.	
        /// </remarks>	
        /// <unmanaged>HRESULT IDXGIResource1::CreateSharedHandle([In, Optional] const SECURITY_ATTRIBUTES* pAttributes,[In] DXGI_SHARED_RESOURCE_FLAGS dwAccess,[In, Optional] const wchar_t* name,[Out] void** pHandle)</unmanaged>	
        public System.IntPtr CreateSharedHandle(string name, SharpDX.DXGI.SharedResourceFlags dwAccess, SharpDX.Win32.SecurityAttributes? attributesRef = null )
        {
            return CreateSharedHandle(attributesRef, dwAccess, name);
        }
    }
}