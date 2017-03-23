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
    /// <p><strong>Applies to: </strong>desktop apps only</p><p>Enables two threads to share the same Direct3D 9 device, and provides access to the DirectX Video Acceleration (DXVA) features of the device.</p>	
    /// </summary>	
    /// <remarks>	
    /// <p>This interface is exposed by the Direct3D Device Manager. To create the Direct3D device manager, call <strong><see cref="SharpDX.MediaFoundation.DirectX.DXVAFactory.CreateDirect3DDeviceManager9"/></strong>.</p><p>To get this interface from the Enhanced Video Renderer (EVR), call <strong><see cref="SharpDX.MediaFoundation.ServiceProvider.GetService"/></strong>. The service <see cref="System.Guid"/> is <strong><see cref="SharpDX.MediaFoundation.MediaServiceKeys.VideoAcceleration"/></strong>. For the DirectShow EVR filter, call <strong>GetService</strong> on the filter's pins.</p><p>The Direct3D Device Manager supports Direct3D 9 devices only. It does not support DXGI devices.</p>	
    /// </remarks>	
    /// <include file='..\Documentation\CodeComments.xml' path="/comments/comment[@id='IDirect3DDeviceManager9']/*"/>	
    /// <msdn-id>ms704727</msdn-id>	
    /// <unmanaged>IDirect3DDeviceManager9</unmanaged>	
    /// <unmanaged-short>IDirect3DDeviceManager9</unmanaged-short>
    public partial class Direct3DDeviceManager
    {
        private readonly int token;

        /// <summary>	
        /// <p><strong>Applies to: </strong>desktop apps only</p><p> Creates an instance of the Direct3D Device Manager. </p>	
        /// </summary>	
        /// <returns><p>If this function succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p></returns>	
        /// <include file='..\Documentation\CodeComments.xml' path="/comments/comment[@id='DXVA2CreateDirect3DDeviceManager9']/*"/>	
        /// <msdn-id>bb970490</msdn-id>	
        /// <unmanaged>HRESULT DXVA2CreateDirect3DDeviceManager9([Out] unsigned int* pResetToken,[Out, Fast] IDirect3DDeviceManager9** ppDeviceManager)</unmanaged>	
        /// <unmanaged-short>DXVA2CreateDirect3DDeviceManager9</unmanaged-short>	
        public Direct3DDeviceManager()
        {
            DXVAFactory.CreateDirect3DDeviceManager9(out token, this);
        }

        /// <summary>
        /// A token that identifies this instance of the Direct3D device manager. Use this token when calling <see cref="ResetDevice"/>.
        /// </summary>
        public int CreationToken { get { return token; } }
    }
}
#endif