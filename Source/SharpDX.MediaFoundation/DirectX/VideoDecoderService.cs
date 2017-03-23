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
    /// <p><strong>Applies to: </strong>desktop apps only</p><p>Provides access to DirectX Video Acceleration (DXVA) decoder services. Use this interface to query which hardware-accelerated decoding operations are available and to create DXVA video decoder devices. </p><p>To get a reference to this interface, call <strong><see cref="SharpDX.MediaFoundation.DirectX.Direct3DDeviceManager.GetVideoService"/></strong> or <strong><see cref="SharpDX.MediaFoundation.DirectX.DXVAFactory.CreateVideoService"/></strong> with the interface identifier IID_IDirectXVideoDecoderService.</p>	
    /// </summary>	
    /// <include file='..\Documentation\CodeComments.xml' path="/comments/comment[@id='IDirectXVideoDecoderService']/*"/>	
    /// <msdn-id>ms704820</msdn-id>	
    /// <unmanaged>IDirectXVideoDecoderService</unmanaged>	
    /// <unmanaged-short>IDirectXVideoDecoderService</unmanaged-short>
    public partial class VideoDecoderService
    {
        /// <summary>	
        /// <p><strong>Applies to: </strong>desktop apps only</p><p>Creates a DirectX Video Acceleration (DXVA) services object. Call this function if your application uses DXVA directly, without using DirectShow or Media Foundation. </p>	
        /// </summary>	
        /// <param name="device"><dd> <p> A reference to the <strong><see cref="SharpDX.Direct3D9.Device"/></strong> interface of a Direct3D device. </p> </dd></param>	
        /// <returns><p>If this function succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p></returns>	
        /// <include file='..\Documentation\CodeComments.xml' path="/comments/comment[@id='DXVA2CreateVideoService']/*"/>	
        /// <msdn-id>ms704721</msdn-id>	
        /// <unmanaged>HRESULT DXVA2CreateVideoService([In] IDirect3DDevice9* pDD,[In] const GUID&amp; riid,[Out] void** ppService)</unmanaged>	
        /// <unmanaged-short>DXVA2CreateVideoService</unmanaged-short>	
        public VideoDecoderService(SharpDX.Direct3D9.Device device)
            : base(device)
        {
        }
    }
}
#endif