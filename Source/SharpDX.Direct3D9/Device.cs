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

namespace SharpDX.Direct3D9
{
    public partial class Device
    {
        /// <summary>	
        /// Creates a device to represent the display adapter.	
        /// </summary>	
        /// <remarks>	
        ///  This method returns a fully working device interface, set to the required display mode (or windowed), and allocated with the appropriate back buffers. To begin rendering, the application needs only to create and set a depth buffer (assuming EnableAutoDepthStencil is FALSE in <see cref="SharpDX.Direct3D9.PresentParameters"/>). When you create a Direct3D device, you supply two different window parameters: a focus window (hFocusWindow) and a device window (the hDeviceWindow in <see cref="SharpDX.Direct3D9.PresentParameters"/>). The purpose of each window is:  The focus window alerts Direct3D when an application switches from foreground mode to background mode (via Alt-Tab, a mouse click, or some other method). A single focus window is shared by each device created by an application. The device window determines the location and size of the back buffer on screen. This is used by Direct3D when the back buffer contents are copied to the front buffer during {{Present}}.  This method should not be run during the handling of WM_CREATE. An application should never pass a window handle to Direct3D while handling WM_CREATE.  Any call to create, release, or reset the device must be done using the same thread as the window procedure of the focus window. Note that D3DCREATE_HARDWARE_VERTEXPROCESSING, D3DCREATE_MIXED_VERTEXPROCESSING, and D3DCREATE_SOFTWARE_VERTEXPROCESSING are mutually exclusive flags, and at least one of these vertex processing flags must be specified when calling this method. Back buffers created as part of the device are only lockable if D3DPRESENTFLAG_LOCKABLE_BACKBUFFER is specified in the presentation parameters. (Multisampled back buffers and depth surfaces are never lockable.) The methods {{Reset}}, <see cref="SharpDX.ComObject"/>, and {{TestCooperativeLevel}} must be called from the same thread that used this method to create a device. D3DFMT_UNKNOWN can be specified for the windowed mode back buffer format when calling CreateDevice, {{Reset}}, and {{CreateAdditionalSwapChain}}. This means the application does not have to query the current desktop format before calling CreateDevice for windowed mode. For full-screen mode, the back buffer format must be specified. If you attempt to create a device on a 0x0 sized window, CreateDevice will fail. 	
        /// </remarks>
        /// <param name="direct3D">an instance of <see cref = "SharpDX.Direct3D9.Direct3D" /></param> 
        /// <param name="adapter"> Ordinal number that denotes the display adapter. {{D3DADAPTER_DEFAULT}} is always the primary display adapter.  </param>
        /// <param name="deviceType"> Member of the <see cref="SharpDX.Direct3D9.DeviceType"/> enumerated type that denotes the desired device type. If the desired device type is not available, the method will fail.  </param>
        /// <param name="hFocusWindow"> The focus window alerts Direct3D when an application switches from foreground mode to background mode. See Remarks. 	   For full-screen mode, the window specified must be a top-level window. For windowed mode, this parameter may be NULL only if the hDeviceWindow member of pPresentationParameters is set to a valid, non-NULL value.  </param>
        /// <param name="behaviorFlags"> Combination of one or more options that control device creation. For more information, see {{D3DCREATE}}. </param>
        /// <param name="resentationParametersRef"> Pointer to a <see cref="SharpDX.Direct3D9.PresentParameters"/> structure, describing the presentation parameters for the device to be created. If BehaviorFlags specifies {{D3DCREATE_ADAPTERGROUP_DEVICE}}, pPresentationParameters is an array. Regardless of the number of heads that exist, only one depth/stencil surface is automatically created. For Windows 2000 and Windows XP, the full-screen device display refresh rate is set in the following order:   User-specified nonzero ForcedRefreshRate registry key, if supported by the device. Application-specified nonzero refresh rate value in the presentation parameter. Refresh rate of the latest desktop mode, if supported by the device. 75 hertz if supported by the device. 60 hertz if supported by the device. Device default.  An unsupported refresh rate will default to the closest supported refresh rate below it.  For example, if the application specifies 63 hertz, 60 hertz will be used. There are no supported refresh rates below 57 hertz. pPresentationParameters is both an input and an output parameter. Calling this method may change several members including:  If BackBufferCount, BackBufferWidth, and BackBufferHeight  are 0 before the method is called, they will be changed when the method returns. If BackBufferFormat equals <see cref="SharpDX.Direct3D9.Format.Unknown"/> before the method is called, it will be changed when the method returns.  </param>
        /// <param name="returnedDeviceInterfaceRef"> Address of a pointer to the returned <see cref="SharpDX.Direct3D9.Device"/> interface, which represents the created device.  </param>
        /// <returns>  <see cref="int"/>  If the method succeeds, the return value is D3D_OK. If the method fails, the return value can be one of the following: D3DERR_DEVICELOST, D3DERR_INVALIDCALL, D3DERR_NOTAVAILABLE, D3DERR_OUTOFVIDEOMEMORY. </returns>
        /// <unmanaged>HRESULT CreateDevice([None] UINT Adapter,[None] D3DDEVTYPE DeviceType,[None] HWND hFocusWindow,[None] int BehaviorFlags,[None] D3DPRESENT_PARAMETERS* pPresentationParameters,[None] IDirect3DDevice9** ppReturnedDeviceInterface)</unmanaged>
        public Device(Direct3D direct3D, int adapter, SharpDX.Direct3D9.DeviceType deviceType, IntPtr hFocusWindow, CreateFlags behaviorFlags, params SharpDX.Direct3D9.PresentParameters[] presentationParametersRef)
        {
            Device temp;
            direct3D.CreateDevice(adapter, deviceType, hFocusWindow, behaviorFlags, presentationParametersRef, out temp);
            NativePointer = temp.NativePointer;
        }

        /// <summary>
        /// Clears one or more surfaces such as a render target, a stencil buffer, and a depth buffer.
        /// </summary>
        /// <param name="clearFlags">Flags that specify which surfaces will be cleared.</param>
        /// <param name="color">The color that will be used to fill the cleared render target.</param>
        /// <param name="zdepth">The value that will be used to fill the cleared depth buffer.</param>
        /// <param name="stencil">The value that will be used to fill the cleared stencil buffer.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::Clear([None] int Count,[In, Buffer, Optional] const D3DRECT* pRects,[None] int Flags,[None] D3DCOLOR Color,[None] float Z,[None] int Stencil)</unmanaged>
        public Result Clear(ClearFlags clearFlags, SharpDX.Color4 color, float zdepth, int stencil)
        {
            return this.Clear_(0 , null, clearFlags, color, zdepth, stencil);
        }

        /// <summary>
        /// Clears one or more surfaces such as a render target, a stencil buffer, and a depth buffer.
        /// </summary>
        /// <param name="clearFlags">Flags that specify which surfaces will be cleared.</param>
        /// <param name="color">The color that will be used to fill the cleared render target.</param>
        /// <param name="zdepth">The value that will be used to fill the cleared depth buffer.</param>
        /// <param name="stencil">The value that will be used to fill the cleared stencil buffer.</param>
        /// <param name="rectangles">The areas on the surfaces that will be cleared.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::Clear([None] int Count,[In, Buffer, Optional] const D3DRECT* pRects,[None] int Flags,[None] D3DCOLOR Color,[None] float Z,[None] int Stencil)</unmanaged>
        public Result Clear(ClearFlags clearFlags, SharpDX.Color4 color, float zdepth, int stencil, Rectangle[] rectangles)
        {
            return this.Clear_( rectangles == null?0:rectangles.Length, rectangles, clearFlags, color, zdepth, stencil);
        }

        /// <summary>	
        /// Allows an application to fill a rectangular area of a D3DPOOL_DEFAULT surface with a specified color.	
        /// </summary>	
        /// <remarks>	
        ///  This method can only be applied to a render target, a render-target texture surface, or an off-screen plain surface with a pool type of D3DPOOL_DEFAULT. IDirect3DDevice9::ColorFill will work with all formats. However, when using a reference or software device, the only formats supported are D3DFMT_X1R5G5B5, D3DFMT_A1R5G5B5, D3DFMT_R5G6B5, D3DFMT_X8R8G8B8, D3DFMT_A8R8G8B8, D3DFMT_YUY2, D3DFMT_G8R8_G8B8, D3DFMT_UYVY, D3DFMT_R8G8_B8G8, D3DFMT_R16F, D3DFMT_G16R16F, D3DFMT_A16B16G16R16F, D3DFMT_R32F, D3DFMT_G32R32F, and D3DFMT_A32B32G32R32F. When using a DirectX 7 or DirectX 8.x driver, the only YUV formats supported are D3DFMT_UYVY and D3DFMT_YUY2. 	
        /// </remarks>	
        /// <param name="surfaceRef"> Pointer to the surface to be filled. </param>
        /// <param name="color"> Color used for filling. </param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::ColorFill([None] IDirect3DSurface9* pSurface,[In, Optional] const RECT* pRect,[None] D3DCOLOR color)</unmanaged>
        public SharpDX.Result ColorFill(SharpDX.Direct3D9.Surface surfaceRef, SharpDX.Color4 color)
        {
            return this.ColorFill(surfaceRef, null, color);
        }

        /// <summary>
        /// Presents the contents of the next buffer in the sequence of back buffers to the screen.
        /// </summary>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>IDirect3DDevice9::Present</unmanaged>
        public void Present()
        {
            Present(null, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Presents the contents of the next buffer in the sequence of back buffers to the screen.
        /// </summary>
        /// <param name="sourceRectangle">The area of the back buffer that should be presented.</param>
        /// <param name="destinationRectangle">The area of the front buffer that should receive the result of the presentation.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>IDirect3DDevice9::Present</unmanaged>
        public void Present(Rectangle sourceRectangle, Rectangle destinationRectangle)
        {
            Present(sourceRectangle, destinationRectangle);
        }

        /// <summary>
        /// Presents the contents of the next buffer in the sequence of back buffers to the screen.
        /// </summary>
        /// <param name="sourceRectangle">The area of the back buffer that should be presented.</param>
        /// <param name="destinationRectangle">The area of the front buffer that should receive the result of the presentation.</param>
        /// <param name="windowOverride">The destination window whose client area is taken as the target for this presentation.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>IDirect3DDevice9::Present</unmanaged>
        public void Present(Rectangle sourceRectangle, Rectangle destinationRectangle, IntPtr windowOverride)
        {
            unsafe
            {
                Rectangle? sourceRectangleOptional = null;
                if (sourceRectangle != Rectangle.Empty)
                    sourceRectangleOptional = sourceRectangle;

                IntPtr destPtr = IntPtr.Zero;
                if (destinationRectangle != Rectangle.Empty)
                    destPtr = new IntPtr(&destinationRectangle);

                Present(sourceRectangleOptional, destPtr, windowOverride, IntPtr.Zero);
            }
        }


        /// <summary>
        /// Presents the contents of the next buffer in the sequence of back buffers to the screen.
        /// </summary>
        /// <param name="sourceRectangle">The area of the back buffer that should be presented.</param>
        /// <param name="destinationRectangle">The area of the front buffer that should receive the result of the presentation.</param>
        /// <param name="windowOverride">The destination window whose client area is taken as the target for this presentation.</param>
        /// <param name="region">Specifies a region on the back buffer that contains the minimal amount of pixels that need to be updated.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>IDirect3DDevice9::Present</unmanaged>
        public void Present(Rectangle sourceRectangle, Rectangle destinationRectangle, IntPtr windowOverride, System.Drawing.Region region)
        {
            unsafe
            {
                var graphics = System.Drawing.Graphics.FromHwnd(windowOverride);
                IntPtr regionPtr = region.GetHrgn(graphics);
                graphics.Dispose();

                Rectangle? sourceRectangleOptional = null;
                if (sourceRectangle != Rectangle.Empty)
                    sourceRectangleOptional = sourceRectangle;

                IntPtr destPtr = IntPtr.Zero;
                if (destinationRectangle != Rectangle.Empty)
                    destPtr = new IntPtr(&destinationRectangle);

                Present(sourceRectangleOptional, destPtr, windowOverride, regionPtr);
            }
        }
    }
}
