// Copyright (c) 2010-2014 SharpDX - SharpDX Team
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
#if DESKTOP_APP
namespace SharpDX.MediaFoundation.DirectX
{
    /// <summary>	
    /// <p><strong>Applies to: </strong>desktop apps only</p><p>Gets the range of values for an image filter that the Microsoft DirectX Video Acceleration High Definition (DXVA-HD) device supports. </p>	
    /// </summary>	
    /// <remarks>	
    /// <p>To find out which image filters the device supports, check the <strong>FilterCaps</strong> member of the <strong><see cref="SharpDX.MediaFoundation.DirectX.Vpdevcaps"/></strong> structure. Call the <strong><see cref="SharpDX.MediaFoundation.DirectX.HDDevice.GetVideoProcessorDeviceCaps"/></strong> method to get this value.</p>	
    /// </remarks>	
    /// <include file='..\Documentation\CodeComments.xml' path="/comments/comment[@id='IDXVAHD_Device']/*"/>	
    /// <msdn-id>dd373915</msdn-id>	
    /// <unmanaged>IDXVAHD_Device</unmanaged>	
    /// <unmanaged-short>IDXVAHD_Device</unmanaged-short>
    public partial class HDDevice
    {
        /// <summary>	
        /// <p><strong>Applies to: </strong>desktop apps only</p><p>Creates a Microsoft DirectX Video Acceleration High Definition (DXVA-HD) device.</p>	
        /// </summary>	
        /// <param name="device"><dd> <p>A reference to the <strong><see cref="SharpDX.Direct3D9.DeviceEx"/></strong> interface of a Direct3D 9 device.</p> </dd></param>	
        /// <param name="contentDescription"><dd> <p>A reference to a <strong><see cref="SharpDX.MediaFoundation.DirectX.ContentDescription"/></strong> structure that describes the video content. The driver uses this information as a hint when it creates the device.</p> </dd></param>	
        /// <param name="usage"><dd> <p>A member of the <strong><see cref="SharpDX.MediaFoundation.DirectX.DeviceUsage"/></strong> enumeration, describing how the device will be used. The value indicates the desired trade-off between speed and video quality. The driver uses this flag as a hint when it creates the device.</p> </dd></param>	
        /// <remarks>	
        /// <p> Use the <strong><see cref="SharpDX.MediaFoundation.DirectX.HDDevice"/></strong> interface to get the device capabilities, create the video processor, and allocate video surfaces. </p>	
        /// </remarks>	
        /// <include file='..\Documentation\CodeComments.xml' path="/comments/comment[@id='DXVAHD_CreateDevice']/*"/>	
        /// <msdn-id>dd318412</msdn-id>	
        /// <unmanaged>HRESULT DXVAHD_CreateDevice([In] IDirect3DDevice9Ex* pD3DDevice,[In] const DXVAHD_CONTENT_DESC* pContentDesc,[In] DXVAHD_DEVICE_USAGE Usage,[In, Optional] __function__stdcall* pPlugin,[Out, Fast] IDXVAHD_Device** ppDevice)</unmanaged>	
        /// <unmanaged-short>DXVAHD_CreateDevice</unmanaged-short>	
        public HDDevice(SharpDX.Direct3D9.DeviceEx device, ContentDescription contentDescription, DeviceUsage usage)
        {
            DXVAFactory.CreateDevice(device, ref contentDescription, usage, null, this);
        }
    }
}
#endif