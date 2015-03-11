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
    public partial class Adapter
    {

        /// <summary>
        /// Gets all outputs from this adapter.
        /// </summary>
        /// <msdn-id>bb174525</msdn-id>	
        /// <unmanaged>HRESULT IDXGIAdapter::EnumOutputs([In] unsigned int Output,[Out] IDXGIOutput** ppOutput)</unmanaged>	
        /// <unmanaged-short>IDXGIAdapter::EnumOutputs</unmanaged-short>	
        public Output[] Outputs
        {
            get
            {
                var outputs = new List<Output>();
                do
                {
                    Output output;
                    var result = GetOutput(outputs.Count, out output);
                    if (result == ResultCode.NotFound || output == null)
                        break;
                    outputs.Add(output);
                } while (true);
                return outputs.ToArray();
            }
        }

        /// <summary>
        /// Checks to see if a device interface for a graphics component is supported by the system.
        /// </summary>
        /// <param name="type">The GUID of the interface of the device version for which support is being checked. For example, typeof(ID3D10Device).GUID.</param>
        /// <returns>
        ///   <c>true</c> if the interface is supported; otherwise, <c>false</c>.
        /// </returns>
        /// <msdn-id>Bb174524</msdn-id>	
        /// <unmanaged>HRESULT IDXGIAdapter::CheckInterfaceSupport([In] const GUID&amp; InterfaceName,[Out] LARGE_INTEGER* pUMDVersion)</unmanaged>	
        /// <unmanaged-short>IDXGIAdapter::CheckInterfaceSupport</unmanaged-short>	
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
        ///   <c>true</c> if the interface is supported; otherwise, <c>false</c>.
        /// </returns>
        /// <msdn-id>Bb174524</msdn-id>	
        /// <unmanaged>HRESULT IDXGIAdapter::CheckInterfaceSupport([In] const GUID&amp; InterfaceName,[Out] LARGE_INTEGER* pUMDVersion)</unmanaged>	
        /// <unmanaged-short>IDXGIAdapter::CheckInterfaceSupport</unmanaged-short>	
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
        ///   <c>true</c> if the interface is supported; otherwise, <c>false</c>.
        /// </returns>
        /// <msdn-id>Bb174524</msdn-id>	
        /// <unmanaged>HRESULT IDXGIAdapter::CheckInterfaceSupport([In] const GUID&amp; InterfaceName,[Out] LARGE_INTEGER* pUMDVersion)</unmanaged>	
        /// <unmanaged-short>IDXGIAdapter::CheckInterfaceSupport</unmanaged-short>	
        public bool IsInterfaceSupported<T>(out long userModeVersion) where T : ComObject
        {
            return IsInterfaceSupported(typeof (T), out userModeVersion);
        }

        /// <summary>
        /// Checks to see if a device interface for a graphics component is supported by the system.
        /// </summary>
        /// <param name="type">The GUID of the interface of the device version for which support is being checked. For example, typeof(ID3D10Device).GUID.</param>
        /// <param name="userModeVersion">The user mode driver version of InterfaceName. This is only returned if the interface is supported.</param>
        /// <returns>
        ///   <c>true</c> if the interface is supported; otherwise, <c>false</c>.
        /// </returns>
        /// <msdn-id>Bb174524</msdn-id>	
        /// <unmanaged>HRESULT IDXGIAdapter::CheckInterfaceSupport([In] const GUID&amp; InterfaceName,[Out] LARGE_INTEGER* pUMDVersion)</unmanaged>	
        /// <unmanaged-short>IDXGIAdapter::CheckInterfaceSupport</unmanaged-short>	
        public bool IsInterfaceSupported(Type type, out long userModeVersion)
        {
            return CheckInterfaceSupport(Utilities.GetGuidFromType(type), out userModeVersion).Success;
        }

        /// <summary>
        /// Gets an adapter (video card) outputs.
        /// </summary>
        /// <param name="outputIndex">The index of the output.</param>
        /// <returns>
        /// An instance of <see cref="Output"/> 
        /// </returns>
        /// <unmanaged>HRESULT IDXGIAdapter::EnumOutputs([In] unsigned int Output,[Out] IDXGIOutput** ppOutput)</unmanaged>
        /// <remarks>
        /// When the EnumOutputs method succeeds and fills the ppOutput parameter with the address of the reference to the output interface, EnumOutputs increments the output interface's reference count. To avoid a memory leak, when you finish using the  output interface, call the Release method to decrement the reference count.EnumOutputs first returns the output on which the desktop primary is displayed. This adapter corresponds with an index of zero. EnumOutputs then returns other outputs.
        /// </remarks>
        /// <exception cref="SharpDXException">if the index is greater than the number of outputs, result code <see cref="SharpDX.DXGI.ResultCode.NotFound"/></exception>
        /// <msdn-id>bb174525</msdn-id>	
        /// <unmanaged>HRESULT IDXGIAdapter::EnumOutputs([In] unsigned int Output,[Out] IDXGIOutput** ppOutput)</unmanaged>	
        /// <unmanaged-short>IDXGIAdapter::EnumOutputs</unmanaged-short>	
        public SharpDX.DXGI.Output GetOutput(int outputIndex)
        {
            Output output;
            GetOutput(outputIndex, out output).CheckError();
            return output;
        }

        /// <summary>
        ///   Return the number of available outputs from this adapter.
        /// </summary>
        /// <returns>The number of outputs</returns>
        /// <msdn-id>bb174525</msdn-id>	
        /// <unmanaged>HRESULT IDXGIAdapter::EnumOutputs([In] unsigned int Output,[Out] IDXGIOutput** ppOutput)</unmanaged>	
        /// <unmanaged-short>IDXGIAdapter::EnumOutputs</unmanaged-short>	
        public int GetOutputCount()
        {
            var nbOutputs = 0;
            do
            {
                Output output;
                var result = GetOutput(nbOutputs, out output);
                if (result == ResultCode.NotFound || output == null)
                    break;
                output.Dispose();
                nbOutputs++;
            } while (true);
            return nbOutputs;
        }
    }
}