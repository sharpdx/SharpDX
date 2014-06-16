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

namespace SharpDX.Direct3D9
{
    public partial class Direct3D {
        /// <summary>	
        /// Create an IDirect3D9 object and return an interface to it.	
        /// </summary>	
        /// <remarks>	
        ///  The Direct3D object is the first Direct3D COM object that your graphical application needs to create and the last object that your application needs to release. Functions for enumerating and retrieving capabilities of a device are accessible through the Direct3D object. This enables applications to select devices without creating them. Create an IDirect3D9 object as shown here: 	
        /// <code> LPDIRECT3D9 g_pD3D = NULL; if( NULL == (g_pD3D = Direct3DCreate9(D3D_SDK_VERSION))) return E_FAIL; </code>	
        /// 	
        ///  The IDirect3D9 interface supports enumeration of active display adapters and allows the creation of <see cref="SharpDX.Direct3D9.Device"/> objects. If the user dynamically adds adapters (either by adding devices to the desktop, or by hot-docking a laptop), those devices will not be included in the enumeration. Creating a new IDirect3D9 interface will expose the new devices. D3D_SDK_VERSION is passed to this function to ensure that the header files against which an application is compiled match the version of the runtime DLL's that are installed on the machine. D3D_SDK_VERSION is only changed in the runtime when a header change (or other code change) would require an application to be rebuilt. If this function fails, it indicates that the header file version does not match the runtime DLL version. For an example, see {{Creating a Device (Direct3D 9)}}. 	
        /// </remarks>
        public Direct3D()
        {
            FromTemp(D3D9.Create9(D3D9.SdkVersion));
            Adapters = new AdapterCollection(this);
        }

        /// <summary>
        /// Checks the version of D3DX runtime against the version of this library..
        /// </summary>
        public static void CheckVersion()
        {
            if (!D3DX.CheckVersion())
                throw new SharpDXException("Direct3DX9 was not found. Install latest DirectX redistributable runtimes from Microsoft");            
        }

        /// <summary>
        /// Gets the adapters.
        /// </summary>
        public AdapterCollection Adapters { get; internal set; }

        /// <summary>
        /// Determines whether a depth-stencil format is compatible with a render-target format in a particular display mode.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        /// <param name="deviceType">Type of the device.</param>
        /// <param name="adapterFormat">The adapter format.</param>
        /// <param name="renderTargetFormat">The render target format.</param>
        /// <param name="depthStencilFormat">The depth stencil format.</param>
        /// <returns>If the depth-stencil format is compatible with the render-target format in the display mode, this method returns <c>true</c></returns>
        /// <unmanaged>HRESULT IDirect3D9::CheckDepthStencilMatch([In] unsigned int Adapter,[In] D3DDEVTYPE DeviceType,[In] D3DFORMAT AdapterFormat,[In] D3DFORMAT RenderTargetFormat,[In] D3DFORMAT DepthStencilFormat)</unmanaged>
        public bool CheckDepthStencilMatch(int adapter, DeviceType deviceType, Format adapterFormat, Format renderTargetFormat, Format depthStencilFormat)
        {
            return CheckDepthStencilMatch_(adapter, deviceType, adapterFormat, renderTargetFormat, depthStencilFormat) == 0;
        }

        /// <summary>
        /// Determines whether a depth-stencil format is compatible with a render-target format in a particular display mode.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        /// <param name="deviceType">Type of the device.</param>
        /// <param name="adapterFormat">The adapter format.</param>
        /// <param name="renderTargetFormat">The render target format.</param>
        /// <param name="depthStencilFormat">The depth stencil format.</param>
        /// <param name="result">The result.</param>
        /// <returns>
        /// If the depth-stencil format is compatible with the render-target format in the display mode, this method returns <c>true</c>
        /// </returns>
        /// <unmanaged>HRESULT IDirect3D9::CheckDepthStencilMatch([In] unsigned int Adapter,[In] D3DDEVTYPE DeviceType,[In] D3DFORMAT AdapterFormat,[In] D3DFORMAT RenderTargetFormat,[In] D3DFORMAT DepthStencilFormat)</unmanaged>
        public bool CheckDepthStencilMatch(int adapter, DeviceType deviceType, Format adapterFormat, Format renderTargetFormat, Format depthStencilFormat, out Result result)
        {
            result = CheckDepthStencilMatch_(adapter, deviceType, adapterFormat, renderTargetFormat, depthStencilFormat);
            return result == 0;
        }

        /// <summary>
        /// Determines whether a surface format is available as a specified resource type and can be used as a texture, depth-stencil buffer, or render target, or any combination of the three, on a device representing this adapter.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        /// <param name="deviceType">Type of the device.</param>
        /// <param name="adapterFormat">The adapter format.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="checkFormat">The check format.</param>
        /// <returns>
        /// If the format is compatible with the specified device for the requested usage, this method returns <c>true</c>
        /// </returns>
        /// <unmanaged>HRESULT IDirect3D9::CheckDeviceFormat([In] unsigned int Adapter,[In] D3DDEVTYPE DeviceType,[In] D3DFORMAT AdapterFormat,[In] unsigned int Usage,[In] D3DRESOURCETYPE RType,[In] D3DFORMAT CheckFormat)</unmanaged>
        public bool CheckDeviceFormat(int adapter, DeviceType deviceType, Format adapterFormat, Usage usage, ResourceType resourceType, Format checkFormat)
        {
            return CheckDeviceFormat_(adapter, deviceType, adapterFormat, (int)usage, resourceType, checkFormat) == 0;
        }

        /// <summary>
        /// Determines whether a surface format is available as a specified resource type and can be used as a texture, depth-stencil buffer, or render target, or any combination of the three, on a device representing this adapter.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        /// <param name="deviceType">Type of the device.</param>
        /// <param name="adapterFormat">The adapter format.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="checkFormat">The check format.</param>
        /// <param name="result">The result.</param>
        /// <returns>If the format is compatible with the specified device for the requested usage, this method returns <c>true</c></returns>
        /// <unmanaged>HRESULT IDirect3D9::CheckDeviceFormat([In] unsigned int Adapter,[In] D3DDEVTYPE DeviceType,[In] D3DFORMAT AdapterFormat,[In] unsigned int Usage,[In] D3DRESOURCETYPE RType,[In] D3DFORMAT CheckFormat)</unmanaged>
        public bool CheckDeviceFormat(int adapter, DeviceType deviceType, Format adapterFormat, Usage usage, ResourceType resourceType, Format checkFormat, out Result result)
        {
            result = CheckDeviceFormat_(adapter, deviceType, adapterFormat, (int)usage, resourceType, checkFormat);
            return result == 0;
        }

        /// <summary>
        /// Tests the device to see if it supports conversion from one display format to another.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        /// <param name="deviceType">Type of the device.</param>
        /// <param name="sourceFormat">The source format.</param>
        /// <param name="targetFormat">The target format.</param>
        /// <returns>
        /// True if the method succeeds.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3D9::CheckDeviceFormatConversion([In] unsigned int Adapter,[In] D3DDEVTYPE DeviceType,[In] D3DFORMAT SourceFormat,[In] D3DFORMAT TargetFormat)</unmanaged>
        public bool CheckDeviceFormatConversion(int adapter, DeviceType deviceType, Format sourceFormat, Format targetFormat)
        {
            return CheckDeviceFormatConversion_(adapter, deviceType, sourceFormat, targetFormat) == 0;
        }

        /// <summary>
        /// Tests the device to see if it supports conversion from one display format to another.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        /// <param name="deviceType">Type of the device.</param>
        /// <param name="sourceFormat">The source format.</param>
        /// <param name="targetFormat">The target format.</param>
        /// <param name="result">The result.</param>
        /// <returns>True if the method succeeds.</returns>
        /// <unmanaged>HRESULT IDirect3D9::CheckDeviceFormatConversion([In] unsigned int Adapter,[In] D3DDEVTYPE DeviceType,[In] D3DFORMAT SourceFormat,[In] D3DFORMAT TargetFormat)</unmanaged>
        public bool CheckDeviceFormatConversion(int adapter, DeviceType deviceType, Format sourceFormat, Format targetFormat, out Result result)
        {
            result = CheckDeviceFormatConversion_(adapter, deviceType, sourceFormat, targetFormat);
            return result == 0;
        }

        /// <summary>
        /// Determines if a multisampling technique is available on this device.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        /// <param name="deviceType">Type of the device.</param>
        /// <param name="surfaceFormat">The surface format.</param>
        /// <param name="windowed">if set to <c>true</c> [windowed].</param>
        /// <param name="multisampleType">Type of the multisample.</param>
        /// <returns>
        /// f the device can perform the specified multisampling method, this method returns <c>true</c>
        /// </returns>
        /// <unmanaged>HRESULT IDirect3D9::CheckDeviceMultiSampleType([In] unsigned int Adapter,[In] D3DDEVTYPE DeviceType,[In] D3DFORMAT SurfaceFormat,[In] BOOL Windowed,[In] D3DMULTISAMPLE_TYPE MultiSampleType,[Out] unsigned int* pQualityLevels)</unmanaged>
        public bool CheckDeviceMultisampleType(int adapter, DeviceType deviceType, Format surfaceFormat, bool windowed, MultisampleType multisampleType)
        {
            int qualityLevels;
            return CheckDeviceMultiSampleType_(adapter, deviceType, surfaceFormat, windowed, multisampleType, out qualityLevels) == 0;
        }

        /// <summary>
        /// Determines if a multisampling technique is available on this device.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        /// <param name="deviceType">Type of the device.</param>
        /// <param name="surfaceFormat">The surface format.</param>
        /// <param name="windowed">if set to <c>true</c> [windowed].</param>
        /// <param name="multisampleType">Type of the multisample.</param>
        /// <param name="qualityLevels">The quality levels.</param>
        /// <returns>
        /// f the device can perform the specified multisampling method, this method returns <c>true</c>
        /// </returns>
        /// <unmanaged>HRESULT IDirect3D9::CheckDeviceMultiSampleType([In] unsigned int Adapter,[In] D3DDEVTYPE DeviceType,[In] D3DFORMAT SurfaceFormat,[In] BOOL Windowed,[In] D3DMULTISAMPLE_TYPE MultiSampleType,[Out] unsigned int* pQualityLevels)</unmanaged>
        public bool CheckDeviceMultisampleType(int adapter, DeviceType deviceType, Format surfaceFormat, bool windowed, MultisampleType multisampleType, out int qualityLevels)
        {
            return CheckDeviceMultiSampleType_(adapter, deviceType, surfaceFormat, windowed, multisampleType, out qualityLevels) == 0;
        }

        /// <summary>
        /// Determines if a multisampling technique is available on this device.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        /// <param name="deviceType">Type of the device.</param>
        /// <param name="surfaceFormat">The surface format.</param>
        /// <param name="windowed">if set to <c>true</c> [windowed].</param>
        /// <param name="multisampleType">Type of the multisample.</param>
        /// <param name="qualityLevels">The quality levels.</param>
        /// <param name="result">The result.</param>
        /// <returns>f the device can perform the specified multisampling method, this method returns <c>true</c></returns>
        /// <unmanaged>HRESULT IDirect3D9::CheckDeviceMultiSampleType([In] unsigned int Adapter,[In] D3DDEVTYPE DeviceType,[In] D3DFORMAT SurfaceFormat,[In] BOOL Windowed,[In] D3DMULTISAMPLE_TYPE MultiSampleType,[Out] unsigned int* pQualityLevels)</unmanaged>
        public bool CheckDeviceMultisampleType(int adapter, DeviceType deviceType, Format surfaceFormat, bool windowed, MultisampleType multisampleType, out int qualityLevels, out Result result)
        {
            result = CheckDeviceMultiSampleType_(adapter, deviceType, surfaceFormat, windowed, multisampleType, out qualityLevels);
            return result == 0;
        }

        /// <summary>
        /// Verifies whether a hardware accelerated device type can be used on this adapter.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        /// <param name="deviceType">Type of the device.</param>
        /// <param name="adapterFormat">The adapter format.</param>
        /// <param name="backBufferFormat">The back buffer format.</param>
        /// <param name="windowed">if set to <c>true</c> [windowed].</param>
        /// <returns>
        ///   <c>true</c> if the device can be used on this adapter
        /// </returns>
        /// <unmanaged>HRESULT IDirect3D9::CheckDeviceType([In] unsigned int Adapter,[In] D3DDEVTYPE DevType,[In] D3DFORMAT AdapterFormat,[In] D3DFORMAT BackBufferFormat,[In] BOOL bWindowed)</unmanaged>
        public bool CheckDeviceType(int adapter, DeviceType deviceType, Format adapterFormat, Format backBufferFormat, bool windowed)
        {
            return CheckDeviceType_(adapter, deviceType, adapterFormat, backBufferFormat, windowed) == 0;
        }

        /// <summary>
        /// Verifies whether a hardware accelerated device type can be used on this adapter.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        /// <param name="deviceType">Type of the device.</param>
        /// <param name="adapterFormat">The adapter format.</param>
        /// <param name="backBufferFormat">The back buffer format.</param>
        /// <param name="windowed">if set to <c>true</c> [windowed].</param>
        /// <param name="result">The result.</param>
        /// <returns><c>true</c> if the device can be used on this adapter</returns>
        /// <unmanaged>HRESULT IDirect3D9::CheckDeviceType([In] unsigned int Adapter,[In] D3DDEVTYPE DevType,[In] D3DFORMAT AdapterFormat,[In] D3DFORMAT BackBufferFormat,[In] BOOL bWindowed)</unmanaged>
        public bool CheckDeviceType(int adapter, DeviceType deviceType, Format adapterFormat, Format backBufferFormat, bool windowed, out Result result)
        {
            result = CheckDeviceType_(adapter, deviceType, adapterFormat, backBufferFormat, windowed);
            return result == 0;
        }

        /// <summary>
        /// Get the physical display adapters present in the system when this <see cref="Direct3D"/> was instantiated.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        /// <returns>The physical display adapters</returns>
        /// <unmanaged>HRESULT IDirect3D9::GetAdapterIdentifier([In] unsigned int Adapter,[In] unsigned int Flags,[Out] D3DADAPTER_IDENTIFIER9* pIdentifier)</unmanaged>
        public SharpDX.Direct3D9.AdapterDetails GetAdapterIdentifier(int adapter)
        {
            return GetAdapterIdentifier(adapter, 0);
        }
    }
}
