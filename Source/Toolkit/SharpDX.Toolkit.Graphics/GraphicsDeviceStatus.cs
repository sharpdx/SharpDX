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

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Describes the current status of a <see cref="GraphicsDevice"/>.
    /// </summary>
    public enum GraphicsDeviceStatus
    {
        /// <summary>
        /// The device is running fine.
        /// </summary>
        /// <msdn-id>bb509553</msdn-id>	
        /// <unmanaged>S_OK</unmanaged>	
        /// <unmanaged-short>S_OK</unmanaged-short>	
        Normal,

        /// <summary>
        /// The video card has been physically removed from the system, or a driver upgrade for the video card has occurred. The application should destroy and recreate the device.
        /// </summary>
        /// <msdn-id>bb509553</msdn-id>	
        /// <unmanaged>DXGI_ERROR_DEVICE_REMOVED</unmanaged>	
        /// <unmanaged-short>DXGI_ERROR_DEVICE_REMOVED</unmanaged-short>	
        Removed,

        /// <summary>
        /// The application's device failed due to badly formed commands sent by the application. This is an design-time issue that should be investigated and fixed.
        /// </summary>
        /// <msdn-id>bb509553</msdn-id>	
        /// <unmanaged>DXGI_ERROR_DEVICE_HUNG</unmanaged>	
        /// <unmanaged-short>DXGI_ERROR_DEVICE_HUNG</unmanaged-short>	
        Hung,

        /// <summary>
        /// The device failed due to a badly formed command. This is a run-time issue; The application should destroy and recreate the device.
        /// </summary>
        /// <msdn-id>bb509553</msdn-id>	
        /// <unmanaged>DXGI_ERROR_DEVICE_RESET</unmanaged>	
        /// <unmanaged-short>DXGI_ERROR_DEVICE_RESET</unmanaged-short>	
        Reset,

        /// <summary>
        /// The driver encountered a problem and was put into the device removed state.
        /// </summary>
        /// <msdn-id>bb509553</msdn-id>	
        /// <unmanaged>DXGI_ERROR_DRIVER_INTERNAL_ERROR</unmanaged>	
        /// <unmanaged-short>DXGI_ERROR_DRIVER_INTERNAL_ERROR</unmanaged-short>	
        InternalError,

        /// <summary>
        /// The application provided invalid parameter data; this must be debugged and fixed before the application is released.
        /// </summary>
        /// <msdn-id>bb509553</msdn-id>	
        /// <unmanaged>DXGI_ERROR_INVALID_CALL</unmanaged>	
        /// <unmanaged-short>DXGI_ERROR_INVALID_CALL</unmanaged-short>	
        InvalidCall,
    }
}