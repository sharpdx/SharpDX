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
namespace SharpDX.MediaFoundation
{
    public partial class DXGIDeviceManager
    {
        /// <summary>
        /// A token that identifies this instance of the DXGI Device Manager. Use this token when calling <strong><see cref="SharpDX.MediaFoundation.DXGIDeviceManager.ResetDevice"/></strong>
        /// </summary>
        private int ResetToken;

        /// <summary>	
        /// <p>[This documentation is preliminary and is subject to change.]</p><p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p>Creates an instance of the Microsoft DirectX Graphics Infrastructure (DXGI) Device Manager.</p>	
        /// </summary>	
        /// <msdn-id>hh162750</msdn-id>	
        /// <unmanaged>HRESULT MFCreateDXGIDeviceManager([Out] unsigned int* resetToken,[Out] IMFDXGIDeviceManager** ppDeviceManager)</unmanaged>	
        /// <unmanaged-short>MFCreateDXGIDeviceManager</unmanaged-short>	
        public DXGIDeviceManager()
        {
            int resetToken;
            MediaFactory.CreateDXGIDeviceManager(out resetToken, this);
            ResetToken = resetToken;
        }

        /// <summary>	
        /// <p>[This documentation is preliminary and is subject to change.]</p><p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p>Sets the Microsoft Direct3D device or notifies the device manager that the Direct3D device was reset.</p>	
        /// </summary>	
        /// <param name="direct3D11Device"><dd> <p>A reference to the <strong><see cref="SharpDX.ComObject"/></strong> interface of the DXGI device.</p> </dd></param>	
        /// <remarks>	
        /// <p>When you first create the DXGI Device Manager, call this method with a reference to the Direct3D device. (The device manager does not create the device; the caller must provide the device reference initially.) Also call this method if the Direct3D device becomes lost and you need to reset the device or create a new device. </p><p>The <em>resetToken</em> parameter ensures that only the component that originally created the device manager can invalidate the current device.</p><p>If this method succeeds, all open device handles become invalid.</p>	
        /// </remarks>	
        /// <msdn-id>hh447911</msdn-id>	
        /// <unmanaged>HRESULT IMFDXGIDeviceManager::ResetDevice([In] IUnknown* pUnkDevice,[In] unsigned int resetToken)</unmanaged>	
        /// <unmanaged-short>IMFDXGIDeviceManager::ResetDevice</unmanaged-short>	
        public void ResetDevice(ComObject direct3D11Device)
        {
            this.ResetDevice(direct3D11Device, ResetToken);
        }

        /// <summary>	
        /// <p>[This documentation is preliminary and is subject to change.]</p><p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p>Unlocks the Microsoft Direct3D device.</p>	
        /// </summary>	
        /// <param name="hDevice"><dd> <p>A handle to the Direct3D device. To get the device handle, call <strong><see cref="SharpDX.MediaFoundation.DXGIDeviceManager.OpenDeviceHandle"/></strong>.</p> </dd></param>	
        /// <remarks>	
        /// <p> Call this method to release the device after calling <strong><see cref="SharpDX.MediaFoundation.DXGIDeviceManager.LockDevice"/></strong>.</p>	
        /// </remarks>	
        /// <msdn-id>hh447913</msdn-id>	
        /// <unmanaged>HRESULT IMFDXGIDeviceManager::UnlockDevice([In] void* hDevice,[In] BOOL fSaveState)</unmanaged>	
        /// <unmanaged-short>IMFDXGIDeviceManager::UnlockDevice</unmanaged-short>	
        public void UnlockDevice(System.IntPtr hDevice)
        {
            UnlockDevice(hDevice, false);
        }
    }
}