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
namespace SharpDX.MediaFoundation
{
    public partial class ServiceProvider
    {
        /// <summary>
        /// <p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p> </p><p>Retrieves a service interface.</p>
        /// </summary>
        /// <typeparam name="T">Type of the interface to retrieve</typeparam>
        /// <param name="guidService"><dd> <p>The service identifier (SID) of the service. For a list of service identifiers, see Service Interfaces.</p> </dd></param>
        /// <returns>An instance of T if the service is supported</returns>
        /// <exception cref="SharpDXException">if the service is not supported</exception>
        ///   <msdn-id>ms696978</msdn-id>
        ///   <unmanaged>HRESULT IMFGetService::GetService([In] const GUID&amp; guidService,[In] const GUID&amp; riid,[Out] void** ppvObject)</unmanaged>
        ///   <unmanaged-short>IMFGetService::GetService</unmanaged-short>
        public T GetService<T>(System.Guid guidService) where T : ComObject
        {
            return FromPointer<T>(GetService(guidService, Utilities.GetGuidFromType(typeof(T))));
        }
    }
}