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

namespace SharpDX.Direct3D11
{
    public partial class ResourceView
    {
        /// <summary>	
        /// <p>Get the resource that is accessed through this view.</p>	
        /// </summary>	
        /// <remarks>	
        /// <p>This function increments the reference count of the resource by one, so it is necessary to call <strong>Release</strong> on the returned reference when the application is done with it. Destroying (or losing) the returned reference before <strong>Release</strong> is called will result in a memory leak.</p>	
        /// </remarks>	
        /// <msdn-id>ff476643</msdn-id>	
        /// <unmanaged>GetResource</unmanaged>	
        /// <unmanaged-short>GetResource</unmanaged-short>	
        /// <unmanaged>void ID3D11View::GetResource([Out] ID3D11Resource** ppResource)</unmanaged>
        public SharpDX.Direct3D11.Resource Resource
        {
            get
            {
                IntPtr __output__; 
                GetResource(out __output__); 
                return new Resource(__output__);
            }
        }

        /// <summary>	
        /// <p>Get the resource that is accessed through this view.</p>	
        /// </summary>	
        /// <remarks>	
        /// <p>This function increments the reference count of the resource by one, so it is necessary to call <strong>Dispose</strong> on the returned reference when the application is done with it. Destroying (or losing) the returned reference before <strong>Release</strong> is called will result in a memory leak.</p>	
        /// </remarks>	
        /// <msdn-id>ff476643</msdn-id>	
        /// <unmanaged>GetResource</unmanaged>	
        /// <unmanaged-short>GetResource</unmanaged-short>	
        /// <unmanaged>void ID3D11View::GetResource([Out] ID3D11Resource** ppResource)</unmanaged>
        public T ResourceAs<T>() where T : Resource
        {
            IntPtr resourcePtr;
            GetResource(out resourcePtr);
            return As<T>(resourcePtr);
        }
    }
}