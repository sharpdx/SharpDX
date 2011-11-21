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

namespace SharpDX.DXGI
{
    public partial class Adapter
    {

        /// <summary>	
        /// Checks to see if a device interface for a graphics component is supported by the system.	
        /// </summary>	
        /// <param name="type">The GUID of the interface of the device version for which support is being checked. For example, typeof(ID3D10Device).GUID. </param>
        /// <returns>
        /// 	<c>true</c> if the interface is supported; otherwise, <c>false</c>.
        /// </returns>
        /// <unmanaged>HRESULT IDXGIAdapter::CheckInterfaceSupport([In] GUID* InterfaceName,[Out] __int64* pUMDVersion)</unmanaged>
        public bool IsInterfaceSupported(Type type)
        {
            long userModeVersion;
            return IsInterfaceSupported(type, out userModeVersion);
        }

        /// <summary>	
        /// Checks to see if a device interface for a graphics component is supported by the system.	
        /// </summary>	
        /// <typeparam name="T">the interface of the device version for which support is being checked.</typeparam>
        /// <returns>
        /// 	<c>true</c> if the interface is supported; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInterfaceSupported<T>() where T : ComObject
        {
            long userModeVersion;
            return IsInterfaceSupported(typeof(T), out userModeVersion);
        }

        /// <summary>	
        /// Checks to see if a device interface for a graphics component is supported by the system.	
        /// </summary>	
        /// <typeparam name="T">the interface of the device version for which support is being checked.</typeparam>
        /// <param name="userModeVersion">The user mode driver version of InterfaceName. This is only returned if the interface is supported.</param>
        /// <returns>
        /// 	<c>true</c> if the interface is supported; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInterfaceSupported<T>(out long userModeVersion) where T : ComObject
        {
            return IsInterfaceSupported(typeof (T), out userModeVersion);
        }

        /// <summary>	
        /// Checks to see if a device interface for a graphics component is supported by the system.	
        /// </summary>	
        /// <param name="type">The GUID of the interface of the device version for which support is being checked. For example, typeof(ID3D10Device).GUID. </param>
        /// <param name="userModeVersion">The user mode driver version of InterfaceName. This is only returned if the interface is supported.</param>
        /// <returns>
        /// 	<c>true</c> if the interface is supported; otherwise, <c>false</c>.
        /// </returns>
        /// <unmanaged>HRESULT IDXGIAdapter::CheckInterfaceSupport([In] GUID* InterfaceName,[Out] __int64* pUMDVersion)</unmanaged>
        public bool IsInterfaceSupported(Type type, out long userModeVersion)
        {
            try
            {
                CheckInterfaceSupport(Utilities.GetGuidFromType(type), out userModeVersion);
            }
            catch (SharpDXException)
            {
                userModeVersion = 0;
                return false;
            }
            return true;
        }

        /// <summary>
        ///   Return the number of available outputs from this adapter.
        /// </summary>
        /// <returns>The number of outputs</returns>
        public int GetOutputCount()
        {
            var nbOutputs = 0;
            do
            {
                try
                {
                    var output = GetOutput(nbOutputs);
                    output.Dispose();
                }
                catch (SharpDXException exception)
                {
                    if (exception.ResultCode.Code == DXGIError.NotFound)
                        break;
                    throw;
                }
                nbOutputs++;
            } while (true);
            return nbOutputs;
        }
    }
}