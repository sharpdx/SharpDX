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
    public partial class Device
    {
        /// <summary>	
        /// Gets the residency status of an array of resources.	
        /// </summary>	
        /// <remarks>	
        /// The information returned by the pResidencyStatus argument array describes the residency status at the time that the QueryResourceResidency method was called.   Note that the residency status will constantly change. If you call the QueryResourceResidency method during a device removed state, the pResidencyStatus argument will return the DXGI_RESIDENCY_EVICTED_TO_DISK flag. Note??This method should not be called every frame as it incurs a non-trivial amount of overhead. 	
        /// </remarks>	
        /// <param name="comObjects">An array of <see cref="SharpDX.DXGI.Resource"/> interfaces. </param>
        /// <returns>Returns an array of <see cref="SharpDX.DXGI.Residency"/> flags. Each element describes the residency status for corresponding element in  the ppResources argument array. </returns>
        /// <unmanaged>HRESULT IDXGIDevice::QueryResourceResidency([In, Buffer] const IUnknown** ppResources,[Out, Buffer] DXGI_RESIDENCY* pResidencyStatus,[None] int NumResources)</unmanaged>
        public Residency[] QueryResourceResidency(params ComObject[] comObjects) 
        {
            int numResources = comObjects.Length;
            var residencies = new Residency[numResources];
            QueryResourceResidency(comObjects, residencies, numResources);
            return residencies;
        }
    }
}