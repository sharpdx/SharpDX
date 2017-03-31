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
using SharpDX.Mathematics.Interop;

namespace SharpDX.Direct3D9
{
    public partial class DeviceEx
    {
        /// <summary>
        /// Creates a device to represent the display adapter.
        /// </summary>
        /// <param name="direct3D">an instance of <see cref="SharpDX.Direct3D9.Direct3D"/></param>
        /// <param name="adapter">Ordinal number that denotes the display adapter. {{D3DADAPTER_DEFAULT}} is always the primary display adapter.</param>
        /// <param name="deviceType">Member of the <see cref="SharpDX.Direct3D9.DeviceType"/> enumerated type that denotes the desired device type. If the desired device type is not available, the method will fail.</param>
        /// <param name="controlHandle">The focus window alerts Direct3D when an application switches from foreground mode to background mode. See Remarks. 	   For full-screen mode, the window specified must be a top-level window. For windowed mode, this parameter may be NULL only if the hDeviceWindow member of pPresentationParameters is set to a valid, non-NULL value.</param>
        /// <param name="createFlags">Combination of one or more options that control device creation. For more information, see {{D3DCREATE}}.</param>
        /// <param name="presentParameters">Pointer to a <see cref="SharpDX.Direct3D9.PresentParameters"/> structure, describing the presentation parameters for the device to be created. If BehaviorFlags specifies {{D3DCREATE_ADAPTERGROUP_DEVICE}}, pPresentationParameters is an array. Regardless of the number of heads that exist, only one depth/stencil surface is automatically created. For Windows 2000 and Windows XP, the full-screen device display refresh rate is set in the following order:   User-specified nonzero ForcedRefreshRate registry key, if supported by the device. Application-specified nonzero refresh rate value in the presentation parameter. Refresh rate of the latest desktop mode, if supported by the device. 75 hertz if supported by the device. 60 hertz if supported by the device. Device default.  An unsupported refresh rate will default to the closest supported refresh rate below it.  For example, if the application specifies 63 hertz, 60 hertz will be used. There are no supported refresh rates below 57 hertz. pPresentationParameters is both an input and an output parameter. Calling this method may change several members including:  If BackBufferCount, BackBufferWidth, and BackBufferHeight  are 0 before the method is called, they will be changed when the method returns. If BackBufferFormat equals <see cref="SharpDX.Direct3D9.Format.Unknown"/> before the method is called, it will be changed when the method returns.</param>
        /// <remarks>
        /// This method returns a fully working device interface, set to the required display mode (or windowed), and allocated with the appropriate back buffers. To begin rendering, the application needs only to create and set a depth buffer (assuming EnableAutoDepthStencil is FALSE in <see cref="SharpDX.Direct3D9.PresentParameters"/>). When you create a Direct3D device, you supply two different window parameters: a focus window (hFocusWindow) and a device window (the hDeviceWindow in <see cref="SharpDX.Direct3D9.PresentParameters"/>). The purpose of each window is:  The focus window alerts Direct3D when an application switches from foreground mode to background mode (via Alt-Tab, a mouse click, or some other method). A single focus window is shared by each device created by an application. The device window determines the location and size of the back buffer on screen. This is used by Direct3D when the back buffer contents are copied to the front buffer during {{Present}}.  This method should not be run during the handling of WM_CREATE. An application should never pass a window handle to Direct3D while handling WM_CREATE.  Any call to create, release, or reset the device must be done using the same thread as the window procedure of the focus window. Note that D3DCREATE_HARDWARE_VERTEXPROCESSING, D3DCREATE_MIXED_VERTEXPROCESSING, and D3DCREATE_SOFTWARE_VERTEXPROCESSING are mutually exclusive flags, and at least one of these vertex processing flags must be specified when calling this method. Back buffers created as part of the device are only lockable if D3DPRESENTFLAG_LOCKABLE_BACKBUFFER is specified in the presentation parameters. (Multisampled back buffers and depth surfaces are never lockable.) The methods {{Reset}}, <see cref="SharpDX.ComObject"/>, and {{TestCooperativeLevel}} must be called from the same thread that used this method to create a device. D3DFMT_UNKNOWN can be specified for the windowed mode back buffer format when calling CreateDevice, {{Reset}}, and {{CreateAdditionalSwapChain}}. This means the application does not have to query the current desktop format before calling CreateDevice for windowed mode. For full-screen mode, the back buffer format must be specified. If you attempt to create a device on a 0x0 sized window, CreateDevice will fail.
        /// </remarks>
        /// <unmanaged>HRESULT CreateDevice([None] UINT Adapter,[None] D3DDEVTYPE DeviceType,[None] HWND hFocusWindow,[None] int BehaviorFlags,[None] D3DPRESENT_PARAMETERS* pPresentationParameters,[None] IDirect3DDevice9** ppReturnedDeviceInterface)</unmanaged>
        public DeviceEx(Direct3DEx direct3D, int adapter, SharpDX.Direct3D9.DeviceType deviceType, IntPtr controlHandle, CreateFlags createFlags, params SharpDX.Direct3D9.PresentParameters[] presentParameters)
            : base(IntPtr.Zero)
        {
            direct3D.CreateDeviceEx(adapter, deviceType, controlHandle, (int)createFlags, presentParameters, null, this);
        }

        /// <summary>
        /// Creates a device to represent the display adapter.
        /// </summary>
        /// <param name="direct3D">an instance of <see cref="SharpDX.Direct3D9.Direct3D"/></param>
        /// <param name="adapter">Ordinal number that denotes the display adapter. {{D3DADAPTER_DEFAULT}} is always the primary display adapter.</param>
        /// <param name="deviceType">Member of the <see cref="SharpDX.Direct3D9.DeviceType"/> enumerated type that denotes the desired device type. If the desired device type is not available, the method will fail.</param>
        /// <param name="controlHandle">The focus window alerts Direct3D when an application switches from foreground mode to background mode. See Remarks. 	   For full-screen mode, the window specified must be a top-level window. For windowed mode, this parameter may be NULL only if the hDeviceWindow member of pPresentationParameters is set to a valid, non-NULL value.</param>
        /// <param name="createFlags">Combination of one or more options that control device creation. For more information, see {{D3DCREATE}}.</param>
        /// <param name="presentParameters">Pointer to a <see cref="SharpDX.Direct3D9.PresentParameters"/> structure, describing the presentation parameters for the device to be created. If BehaviorFlags specifies {{D3DCREATE_ADAPTERGROUP_DEVICE}}, pPresentationParameters is an array. Regardless of the number of heads that exist, only one depth/stencil surface is automatically created. For Windows 2000 and Windows XP, the full-screen device display refresh rate is set in the following order:   User-specified nonzero ForcedRefreshRate registry key, if supported by the device. Application-specified nonzero refresh rate value in the presentation parameter. Refresh rate of the latest desktop mode, if supported by the device. 75 hertz if supported by the device. 60 hertz if supported by the device. Device default.  An unsupported refresh rate will default to the closest supported refresh rate below it.  For example, if the application specifies 63 hertz, 60 hertz will be used. There are no supported refresh rates below 57 hertz. pPresentationParameters is both an input and an output parameter. Calling this method may change several members including:  If BackBufferCount, BackBufferWidth, and BackBufferHeight  are 0 before the method is called, they will be changed when the method returns. If BackBufferFormat equals <see cref="SharpDX.Direct3D9.Format.Unknown"/> before the method is called, it will be changed when the method returns.</param>
        /// <remarks>
        /// This method returns a fully working device interface, set to the required display mode (or windowed), and allocated with the appropriate back buffers. To begin rendering, the application needs only to create and set a depth buffer (assuming EnableAutoDepthStencil is FALSE in <see cref="SharpDX.Direct3D9.PresentParameters"/>). When you create a Direct3D device, you supply two different window parameters: a focus window (hFocusWindow) and a device window (the hDeviceWindow in <see cref="SharpDX.Direct3D9.PresentParameters"/>). The purpose of each window is:  The focus window alerts Direct3D when an application switches from foreground mode to background mode (via Alt-Tab, a mouse click, or some other method). A single focus window is shared by each device created by an application. The device window determines the location and size of the back buffer on screen. This is used by Direct3D when the back buffer contents are copied to the front buffer during {{Present}}.  This method should not be run during the handling of WM_CREATE. An application should never pass a window handle to Direct3D while handling WM_CREATE.  Any call to create, release, or reset the device must be done using the same thread as the window procedure of the focus window. Note that D3DCREATE_HARDWARE_VERTEXPROCESSING, D3DCREATE_MIXED_VERTEXPROCESSING, and D3DCREATE_SOFTWARE_VERTEXPROCESSING are mutually exclusive flags, and at least one of these vertex processing flags must be specified when calling this method. Back buffers created as part of the device are only lockable if D3DPRESENTFLAG_LOCKABLE_BACKBUFFER is specified in the presentation parameters. (Multisampled back buffers and depth surfaces are never lockable.) The methods {{Reset}}, <see cref="SharpDX.ComObject"/>, and {{TestCooperativeLevel}} must be called from the same thread that used this method to create a device. D3DFMT_UNKNOWN can be specified for the windowed mode back buffer format when calling CreateDevice, {{Reset}}, and {{CreateAdditionalSwapChain}}. This means the application does not have to query the current desktop format before calling CreateDevice for windowed mode. For full-screen mode, the back buffer format must be specified. If you attempt to create a device on a 0x0 sized window, CreateDevice will fail.
        /// </remarks>
        /// <unmanaged>HRESULT CreateDevice([None] UINT Adapter,[None] D3DDEVTYPE DeviceType,[None] HWND hFocusWindow,[None] int BehaviorFlags,[None] D3DPRESENT_PARAMETERS* pPresentationParameters,[None] IDirect3DDevice9** ppReturnedDeviceInterface)</unmanaged>
        public DeviceEx(Direct3DEx direct3D, int adapter, DeviceType deviceType, IntPtr controlHandle, CreateFlags createFlags, PresentParameters presentParameters)
            : base(IntPtr.Zero)
        {
            direct3D.CreateDeviceEx(adapter, deviceType, controlHandle, (int)createFlags, new[] { presentParameters }, null, this);
        }

        /// <summary>
        /// Creates a device to represent the display adapter.
        /// </summary>
        /// <param name="direct3D">an instance of <see cref="SharpDX.Direct3D9.Direct3D"/></param>
        /// <param name="adapter">Ordinal number that denotes the display adapter. {{D3DADAPTER_DEFAULT}} is always the primary display adapter.</param>
        /// <param name="deviceType">Member of the <see cref="SharpDX.Direct3D9.DeviceType"/> enumerated type that denotes the desired device type. If the desired device type is not available, the method will fail.</param>
        /// <param name="controlHandle">The focus window alerts Direct3D when an application switches from foreground mode to background mode. See Remarks. 	   For full-screen mode, the window specified must be a top-level window. For windowed mode, this parameter may be NULL only if the hDeviceWindow member of pPresentationParameters is set to a valid, non-NULL value.</param>
        /// <param name="createFlags">Combination of one or more options that control device creation. For more information, see {{D3DCREATE}}.</param>
        /// <param name="presentParameters">Pointer to a <see cref="SharpDX.Direct3D9.PresentParameters"/> structure, describing the presentation parameters for the device to be created. If BehaviorFlags specifies {{D3DCREATE_ADAPTERGROUP_DEVICE}}, pPresentationParameters is an array. Regardless of the number of heads that exist, only one depth/stencil surface is automatically created. For Windows 2000 and Windows XP, the full-screen device display refresh rate is set in the following order:   User-specified nonzero ForcedRefreshRate registry key, if supported by the device. Application-specified nonzero refresh rate value in the presentation parameter. Refresh rate of the latest desktop mode, if supported by the device. 75 hertz if supported by the device. 60 hertz if supported by the device. Device default.  An unsupported refresh rate will default to the closest supported refresh rate below it.  For example, if the application specifies 63 hertz, 60 hertz will be used. There are no supported refresh rates below 57 hertz. pPresentationParameters is both an input and an output parameter. Calling this method may change several members including:  If BackBufferCount, BackBufferWidth, and BackBufferHeight  are 0 before the method is called, they will be changed when the method returns. If BackBufferFormat equals <see cref="SharpDX.Direct3D9.Format.Unknown"/> before the method is called, it will be changed when the method returns.</param>
        /// <param name="fullScreenDisplayMode">The full screen display mode.</param>
        /// <remarks>
        /// This method returns a fully working device interface, set to the required display mode (or windowed), and allocated with the appropriate back buffers. To begin rendering, the application needs only to create and set a depth buffer (assuming EnableAutoDepthStencil is FALSE in <see cref="SharpDX.Direct3D9.PresentParameters"/>). When you create a Direct3D device, you supply two different window parameters: a focus window (hFocusWindow) and a device window (the hDeviceWindow in <see cref="SharpDX.Direct3D9.PresentParameters"/>). The purpose of each window is:  The focus window alerts Direct3D when an application switches from foreground mode to background mode (via Alt-Tab, a mouse click, or some other method). A single focus window is shared by each device created by an application. The device window determines the location and size of the back buffer on screen. This is used by Direct3D when the back buffer contents are copied to the front buffer during {{Present}}.  This method should not be run during the handling of WM_CREATE. An application should never pass a window handle to Direct3D while handling WM_CREATE.  Any call to create, release, or reset the device must be done using the same thread as the window procedure of the focus window. Note that D3DCREATE_HARDWARE_VERTEXPROCESSING, D3DCREATE_MIXED_VERTEXPROCESSING, and D3DCREATE_SOFTWARE_VERTEXPROCESSING are mutually exclusive flags, and at least one of these vertex processing flags must be specified when calling this method. Back buffers created as part of the device are only lockable if D3DPRESENTFLAG_LOCKABLE_BACKBUFFER is specified in the presentation parameters. (Multisampled back buffers and depth surfaces are never lockable.) The methods {{Reset}}, <see cref="SharpDX.ComObject"/>, and {{TestCooperativeLevel}} must be called from the same thread that used this method to create a device. D3DFMT_UNKNOWN can be specified for the windowed mode back buffer format when calling CreateDevice, {{Reset}}, and {{CreateAdditionalSwapChain}}. This means the application does not have to query the current desktop format before calling CreateDevice for windowed mode. For full-screen mode, the back buffer format must be specified. If you attempt to create a device on a 0x0 sized window, CreateDevice will fail.
        /// </remarks>
        /// <unmanaged>HRESULT CreateDevice([None] UINT Adapter,[None] D3DDEVTYPE DeviceType,[None] HWND hFocusWindow,[None] int BehaviorFlags,[None] D3DPRESENT_PARAMETERS* pPresentationParameters,[None] IDirect3DDevice9** ppReturnedDeviceInterface)</unmanaged>
        public DeviceEx(Direct3DEx direct3D, int adapter, DeviceType deviceType, IntPtr controlHandle, CreateFlags createFlags, PresentParameters presentParameters, DisplayModeEx fullScreenDisplayMode)
            : base(IntPtr.Zero)
        {
            direct3D.CreateDeviceEx(adapter, deviceType, controlHandle, (int)createFlags, new[] {presentParameters}, fullScreenDisplayMode == null ? null : new[] {fullScreenDisplayMode}, this);
        }

        /// <summary>
        /// Creates a device to represent the display adapter.
        /// </summary>
        /// <param name="direct3D">an instance of <see cref="SharpDX.Direct3D9.Direct3D"/></param>
        /// <param name="adapter">Ordinal number that denotes the display adapter. {{D3DADAPTER_DEFAULT}} is always the primary display adapter.</param>
        /// <param name="deviceType">Member of the <see cref="SharpDX.Direct3D9.DeviceType"/> enumerated type that denotes the desired device type. If the desired device type is not available, the method will fail.</param>
        /// <param name="controlHandle">The focus window alerts Direct3D when an application switches from foreground mode to background mode. See Remarks. 	   For full-screen mode, the window specified must be a top-level window. For windowed mode, this parameter may be NULL only if the hDeviceWindow member of pPresentationParameters is set to a valid, non-NULL value.</param>
        /// <param name="createFlags">Combination of one or more options that control device creation. For more information, see {{D3DCREATE}}.</param>
        /// <param name="presentParameters">Pointer to a <see cref="SharpDX.Direct3D9.PresentParameters"/> structure, describing the presentation parameters for the device to be created. If BehaviorFlags specifies {{D3DCREATE_ADAPTERGROUP_DEVICE}}, pPresentationParameters is an array. Regardless of the number of heads that exist, only one depth/stencil surface is automatically created. For Windows 2000 and Windows XP, the full-screen device display refresh rate is set in the following order:   User-specified nonzero ForcedRefreshRate registry key, if supported by the device. Application-specified nonzero refresh rate value in the presentation parameter. Refresh rate of the latest desktop mode, if supported by the device. 75 hertz if supported by the device. 60 hertz if supported by the device. Device default.  An unsupported refresh rate will default to the closest supported refresh rate below it.  For example, if the application specifies 63 hertz, 60 hertz will be used. There are no supported refresh rates below 57 hertz. pPresentationParameters is both an input and an output parameter. Calling this method may change several members including:  If BackBufferCount, BackBufferWidth, and BackBufferHeight  are 0 before the method is called, they will be changed when the method returns. If BackBufferFormat equals <see cref="SharpDX.Direct3D9.Format.Unknown"/> before the method is called, it will be changed when the method returns.</param>
        /// <param name="fullScreenDisplayMode">The full screen display mode.</param>
        /// <remarks>
        /// This method returns a fully working device interface, set to the required display mode (or windowed), and allocated with the appropriate back buffers. To begin rendering, the application needs only to create and set a depth buffer (assuming EnableAutoDepthStencil is FALSE in <see cref="SharpDX.Direct3D9.PresentParameters"/>). When you create a Direct3D device, you supply two different window parameters: a focus window (hFocusWindow) and a device window (the hDeviceWindow in <see cref="SharpDX.Direct3D9.PresentParameters"/>). The purpose of each window is:  The focus window alerts Direct3D when an application switches from foreground mode to background mode (via Alt-Tab, a mouse click, or some other method). A single focus window is shared by each device created by an application. The device window determines the location and size of the back buffer on screen. This is used by Direct3D when the back buffer contents are copied to the front buffer during {{Present}}.  This method should not be run during the handling of WM_CREATE. An application should never pass a window handle to Direct3D while handling WM_CREATE.  Any call to create, release, or reset the device must be done using the same thread as the window procedure of the focus window. Note that D3DCREATE_HARDWARE_VERTEXPROCESSING, D3DCREATE_MIXED_VERTEXPROCESSING, and D3DCREATE_SOFTWARE_VERTEXPROCESSING are mutually exclusive flags, and at least one of these vertex processing flags must be specified when calling this method. Back buffers created as part of the device are only lockable if D3DPRESENTFLAG_LOCKABLE_BACKBUFFER is specified in the presentation parameters. (Multisampled back buffers and depth surfaces are never lockable.) The methods {{Reset}}, <see cref="SharpDX.ComObject"/>, and {{TestCooperativeLevel}} must be called from the same thread that used this method to create a device. D3DFMT_UNKNOWN can be specified for the windowed mode back buffer format when calling CreateDevice, {{Reset}}, and {{CreateAdditionalSwapChain}}. This means the application does not have to query the current desktop format before calling CreateDevice for windowed mode. For full-screen mode, the back buffer format must be specified. If you attempt to create a device on a 0x0 sized window, CreateDevice will fail.
        /// </remarks>
        /// <unmanaged>HRESULT CreateDevice([None] UINT Adapter,[None] D3DDEVTYPE DeviceType,[None] HWND hFocusWindow,[None] int BehaviorFlags,[None] D3DPRESENT_PARAMETERS* pPresentationParameters,[None] IDirect3DDevice9** ppReturnedDeviceInterface)</unmanaged>
        public DeviceEx(Direct3DEx direct3D, int adapter, DeviceType deviceType, IntPtr controlHandle, CreateFlags createFlags, PresentParameters[] presentParameters, DisplayModeEx[] fullScreenDisplayMode)
            : base(IntPtr.Zero)
        {
            direct3D.CreateDeviceEx(adapter, deviceType, controlHandle, (int)createFlags, presentParameters, fullScreenDisplayMode, this);
        }

        /// <summary>
        /// Reports the current cooperative-level status of the Direct3D device for a windowed or full-screen application.
        /// </summary>
        /// <param name="windowHandle">The window handle.</param>
        /// <returns>State of the device</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9Ex::CheckDeviceState([In] HWND hDestinationWindow)</unmanaged>
        public DeviceState CheckDeviceState(IntPtr windowHandle)
        {
            var result = CheckDeviceState_(windowHandle);
            return (DeviceState)result.Code;
        }

        /// <summary>
        /// Checks an array of resources to determine if it is likely that they will cause a large stall at Draw time because the system must make the resources GPU-accessible.
        /// </summary>
        /// <param name="resources">An array of <see cref="Resource"/> that indicate the resources to check.</param>
        /// <returns>The <see cref="ResourceResidency"/> status.</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9Ex::CheckDeviceState([In] HWND hDestinationWindow)</unmanaged>
        public ResourceResidency CheckResourceResidency(params Resource[] resources)
        {
            return (ResourceResidency)CheckResourceResidency(resources, resources.Length).Code;
        }

        /// <summary>
        /// Retrieves the display mode's spatial resolution, color resolution, refresh frequency, and rotation settings.
        /// </summary>
        /// <param name="swapChain">The swap chain.</param>
        /// <returns><see cref="DisplayModeEx"/> structure containing data about the display mode of the adapter</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9Ex::GetDisplayModeEx([In] unsigned int iSwapChain,[Out] D3DDISPLAYMODEEX* pMode,[In] void* pRotation)</unmanaged>	
        public DisplayModeEx GetDisplayModeEx(int swapChain)
        {
            return GetDisplayModeEx(swapChain, IntPtr.Zero);
        }

        /// <summary>
        /// Retrieves the display mode's spatial resolution, color resolution, refresh frequency, and rotation settings.
        /// </summary>
        /// <param name="swapChain">The swap chain.</param>
        /// <param name="rotation">The <see cref="DisplayRotation"/> structure indicating the type of screen rotation the application will do.</param>
        /// <returns><see cref="DisplayModeEx"/> structure containing data about the display mode of the adapter</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9Ex::GetDisplayModeEx([In] unsigned int iSwapChain,[Out] D3DDISPLAYMODEEX* pMode,[In] void* pRotation)</unmanaged>	
        public DisplayModeEx GetDisplayModeEx(int swapChain, out DisplayRotation rotation)
        {
            unsafe
            {
                fixed (void* pRotation = &rotation)
                    return GetDisplayModeEx(swapChain, (IntPtr)pRotation);
            }
        }

        /// <summary>
        /// Swap the swapchain's next buffer with the front buffer.
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9Ex::PresentEx([In] const void* pSourceRect,[In] const void* pDestRect,[In] HWND hDestWindowOverride,[In] const RGNDATA* pDirtyRegion,[In] unsigned int dwFlags)</unmanaged>
        public void PresentEx(Present flags)
        {
            PresentEx(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, (int)flags);
        }

        /// <summary>
        /// Swap the swapchain's next buffer with the front buffer.
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <param name="destinationRectangle">The destination rectangle.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9Ex::PresentEx([In] const void* pSourceRect,[In] const void* pDestRect,[In] HWND hDestWindowOverride,[In] const RGNDATA* pDirtyRegion,[In] unsigned int dwFlags)</unmanaged>
        public void PresentEx(Present flags, RawRectangle sourceRectangle, RawRectangle destinationRectangle)
        {
            PresentEx(flags, sourceRectangle, destinationRectangle, IntPtr.Zero);
        }

        /// <summary>
        /// Swap the swapchain's next buffer with the front buffer.
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <param name="destinationRectangle">The destination rectangle.</param>
        /// <param name="windowOverride">The window override.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9Ex::PresentEx([In] const void* pSourceRect,[In] const void* pDestRect,[In] HWND hDestWindowOverride,[In] const RGNDATA* pDirtyRegion,[In] unsigned int dwFlags)</unmanaged>
        public void PresentEx(Present flags, RawRectangle sourceRectangle, RawRectangle destinationRectangle, IntPtr windowOverride)
        {
            unsafe
            {
                PresentEx(new IntPtr(&sourceRectangle), new IntPtr(&destinationRectangle), windowOverride, IntPtr.Zero, (int)flags);
            }
        }

        /// <summary>
        /// Swap the swapchain's next buffer with the front buffer.
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <param name="destinationRectangle">The destination rectangle.</param>
        /// <param name="windowOverride">The window override.</param>
        /// <param name="dirtyRegionRGNData">The region.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9Ex::PresentEx([In] const void* pSourceRect,[In] const void* pDestRect,[In] HWND hDestWindowOverride,[In] const RGNDATA* pDirtyRegion,[In] unsigned int dwFlags)</unmanaged>
        public void PresentEx(Present flags, RawRectangle sourceRectangle, RawRectangle destinationRectangle, IntPtr windowOverride, IntPtr dirtyRegionRGNData)
        {
            unsafe
            {
                PresentEx(new IntPtr(&sourceRectangle), new IntPtr(&destinationRectangle), windowOverride, dirtyRegionRGNData, (int)flags);
            }            
        }

        /// <summary>
        /// Resets the type, size, and format of the swap chain with all other surfaces persistent.
        /// </summary>
        /// <param name="presentationParametersRef">A reference describing the new presentation parameters.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9Ex::ResetEx([In] D3DPRESENT_PARAMETERS* pPresentationParameters,[In] void* pFullscreenDisplayMode)</unmanaged>
        public void ResetEx(ref SharpDX.Direct3D9.PresentParameters presentationParametersRef)
        {
            ResetEx(ref presentationParametersRef, IntPtr.Zero);
        }

        /// <summary>
        /// Resets the type, size, and format of the swap chain with all other surfaces persistent.
        /// </summary>
        /// <param name="presentationParametersRef">A reference describing the new presentation parameters.</param>
        /// <param name="fullScreenDisplayMode">The full screen display mode.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9Ex::ResetEx([In] D3DPRESENT_PARAMETERS* pPresentationParameters,[In] void* pFullscreenDisplayMode)</unmanaged>
        public void ResetEx(ref SharpDX.Direct3D9.PresentParameters presentationParametersRef, DisplayModeEx fullScreenDisplayMode)
        {
            unsafe
            {
                var native = DisplayModeEx.__NewNative();
                fullScreenDisplayMode.__MarshalTo(ref native);
                ResetEx(ref presentationParametersRef, new IntPtr(&native));
            }
        }
    }
}