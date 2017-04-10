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
using System.Globalization;
using System.Reflection;
using SharpDX.Mathematics.Interop;

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
        /// <param name="presentationParametersRef"> Pointer to a <see cref="SharpDX.Direct3D9.PresentParameters"/> structure, describing the presentation parameters for the device to be created. If BehaviorFlags specifies {{D3DCREATE_ADAPTERGROUP_DEVICE}}, pPresentationParameters is an array. Regardless of the number of heads that exist, only one depth/stencil surface is automatically created. For Windows 2000 and Windows XP, the full-screen device display refresh rate is set in the following order:   User-specified nonzero ForcedRefreshRate registry key, if supported by the device. Application-specified nonzero refresh rate value in the presentation parameter. Refresh rate of the latest desktop mode, if supported by the device. 75 hertz if supported by the device. 60 hertz if supported by the device. Device default.  An unsupported refresh rate will default to the closest supported refresh rate below it.  For example, if the application specifies 63 hertz, 60 hertz will be used. There are no supported refresh rates below 57 hertz. pPresentationParameters is both an input and an output parameter. Calling this method may change several members including:  If BackBufferCount, BackBufferWidth, and BackBufferHeight  are 0 before the method is called, they will be changed when the method returns. If BackBufferFormat equals <see cref="SharpDX.Direct3D9.Format.Unknown"/> before the method is called, it will be changed when the method returns.  </param>
        /// <returns>  <see cref="int"/>  If the method succeeds, the return value is D3D_OK. If the method fails, the return value can be one of the following: D3DERR_DEVICELOST, D3DERR_INVALIDCALL, D3DERR_NOTAVAILABLE, D3DERR_OUTOFVIDEOMEMORY. </returns>
        /// <unmanaged>HRESULT CreateDevice([None] UINT Adapter,[None] D3DDEVTYPE DeviceType,[None] HWND hFocusWindow,[None] int BehaviorFlags,[None] D3DPRESENT_PARAMETERS* pPresentationParameters,[None] IDirect3DDevice9** ppReturnedDeviceInterface)</unmanaged>
        public Device(Direct3D direct3D, int adapter, SharpDX.Direct3D9.DeviceType deviceType, IntPtr hFocusWindow, CreateFlags behaviorFlags, params SharpDX.Direct3D9.PresentParameters[] presentationParametersRef)
        {
            direct3D.CreateDevice(adapter, deviceType, hFocusWindow, behaviorFlags, presentationParametersRef, this);
        }

        /// <summary>
        /// Gets the available texture memory.
        /// </summary>
        public long AvailableTextureMemory
        {
            get
            {
                return unchecked((uint)GetAvailableTextureMem());
            }
        }

        /// <summary>
        /// Gets the driver level.
        /// </summary>
        public DriverLevel DriverLevel
        {
            get
            {
                return (DriverLevel)D3DX9.GetDriverLevel(this);
            }
        }

        /// <summary>
        /// Gets the pixel shader profile.
        /// </summary>
        public string PixelShaderProfile
        {
            get
            {
                return D3DX9.GetPixelShaderProfile(this);
            }
        }

        /// <summary>
        /// Gets the vertex shader profile.
        /// </summary>
        public string VertexShaderProfile
        {
            get
            {
                return D3DX9.GetVertexShaderProfile(this);
            }
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
        public void Clear(ClearFlags clearFlags, RawColorBGRA color, float zdepth, int stencil)
        {
            this.Clear_(0, null, clearFlags, color, zdepth, stencil);
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
        public void Clear(ClearFlags clearFlags, RawColorBGRA color, float zdepth, int stencil, RawRectangle[] rectangles)
        {
            this.Clear_( rectangles == null?0:rectangles.Length, rectangles, clearFlags, color, zdepth, stencil);
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
        public void ColorFill(SharpDX.Direct3D9.Surface surfaceRef, RawColorBGRA color)
        {
            this.ColorFill(surfaceRef, null, color);
        }

        /// <summary>
        /// Draws the indexed user primitives.
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="primitiveType">Type of the primitive.</param>
        /// <param name="minimumVertexIndex">Minimum index of the vertex.</param>
        /// <param name="vertexCount">The vertex count.</param>
        /// <param name="primitiveCount">The primitive count.</param>
        /// <param name="indexData">The index data.</param>
        /// <param name="indexDataFormat">The index data format.</param>
        /// <param name="vertexData">The vertex data.</param>
        /// <param name="vertexStride">The vertex stride.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        public void DrawIndexedUserPrimitives<S, T>(PrimitiveType primitiveType, int minimumVertexIndex, int vertexCount, int primitiveCount, S[] indexData, Format indexDataFormat, T[] vertexData)
            where S : struct
            where T : struct
        {
            DrawIndexedUserPrimitives(primitiveType, 0, 0, minimumVertexIndex, vertexCount, primitiveCount, indexData, indexDataFormat, vertexData);
        }

        /// <summary>
        /// Draws the indexed user primitives.
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="primitiveType">Type of the primitive.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="minimumVertexIndex">Minimum index of the vertex.</param>
        /// <param name="vertexCount">The vertex count.</param>
        /// <param name="primitiveCount">The primitive count.</param>
        /// <param name="indexData">The index data.</param>
        /// <param name="indexDataFormat">The index data format.</param>
        /// <param name="vertexData">The vertex data.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        public void DrawIndexedUserPrimitives<S, T>(PrimitiveType primitiveType, int startIndex, int minimumVertexIndex, int vertexCount, int primitiveCount, S[] indexData, Format indexDataFormat, T[] vertexData)
            where S : struct
            where T : struct
        {
            DrawIndexedUserPrimitives(primitiveType, startIndex, 0, minimumVertexIndex, vertexCount, primitiveCount, indexData, indexDataFormat, vertexData);
        }

        /// <summary>
        /// Draws the indexed user primitives.
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="primitiveType">Type of the primitive.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="startVertex">The start vertex.</param>
        /// <param name="minimumVertexIndex">Minimum index of the vertex.</param>
        /// <param name="vertexCount">The vertex count.</param>
        /// <param name="primitiveCount">The primitive count.</param>
        /// <param name="indexData">The index data.</param>
        /// <param name="indexDataFormat">The index data format.</param>
        /// <param name="vertexData">The vertex data.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        public void DrawIndexedUserPrimitives<S, T>(PrimitiveType primitiveType, int startIndex, int startVertex, int minimumVertexIndex, int vertexCount, int primitiveCount, S[] indexData, Format indexDataFormat, T[] vertexData)
            where S : struct
            where T : struct
        {
            unsafe
            {
                DrawIndexedPrimitiveUP(primitiveType, minimumVertexIndex, vertexCount, primitiveCount, (IntPtr)Interop.Fixed(ref indexData[startIndex]), indexDataFormat, (IntPtr)Interop.Fixed(ref vertexData[startVertex]), Utilities.SizeOf<T>());
            }
        }


        /// <summary>
        /// Draws the rectangle patch.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="segmentCounts">The segment counts.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::DrawRectPatch([In] unsigned int Handle,[In, Buffer] const float* pNumSegs,[In] const void* pRectPatchInfo)</unmanaged>
        public void DrawRectanglePatch(int handle, float[] segmentCounts)
        {
            DrawRectanglePatch(handle, segmentCounts, IntPtr.Zero);
        }


        /// <summary>
        /// Draws the rectangle patch.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="segmentCounts">The segment counts.</param>
        /// <param name="info">The info.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged href="bb174373">IDirect3DDevice9::DrawRectPatch</unmanaged>
        public void DrawRectanglePatch(int handle, float[] segmentCounts, RectanglePatchInfo info)
        {
            unsafe
            {
                DrawRectanglePatch(handle, segmentCounts, new IntPtr(&info));
            }
        }

        /// <summary>
        /// Draws the triangle patch.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="segmentCounts">The segment counts.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::DrawTriPatch([In] unsigned int Handle,[In, Buffer] const float* pNumSegs,[In] const void* pTriPatchInfo)</unmanaged>
        public void DrawTrianglePatch(int handle, float[] segmentCounts)
        {
            DrawTrianglePatch(handle, segmentCounts, IntPtr.Zero);
        }

        /// <summary>
        /// Draws the triangle patch.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="segmentCounts">The segment counts.</param>
        /// <param name="info">The info.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        public void DrawTrianglePatch(int handle, float[] segmentCounts, TrianglePatchInfo info)
        {
            unsafe
            {
                DrawTrianglePatch(handle, segmentCounts, new IntPtr(&info));
            }
        }

        /// <summary>
        /// Draws the user primitives.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primitiveType">Type of the primitive.</param>
        /// <param name="primitiveCount">The primitive count.</param>
        /// <param name="data">The data.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        public void DrawUserPrimitives<T>(PrimitiveType primitiveType, int primitiveCount, T[] data) where T : struct
        {
            DrawUserPrimitives(primitiveType, 0, primitiveCount, data);
        }

        /// <summary>
        /// Draws the user primitives.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primitiveType">Type of the primitive.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="primitiveCount">The primitive count.</param>
        /// <param name="data">The data.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        public void DrawUserPrimitives<T>(PrimitiveType primitiveType, int startIndex, int primitiveCount, T[] data) where T : struct
        {
            unsafe
            {
                DrawPrimitiveUP(primitiveType, primitiveCount, (IntPtr)Interop.Fixed(ref data[startIndex]), Utilities.SizeOf<T>());
            }    
        }

        /// <summary>
        /// Gets the back buffer.
        /// </summary>
        /// <param name="swapChain">The swap chain.</param>
        /// <param name="backBuffer">The back buffer.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        public SharpDX.Direct3D9.Surface GetBackBuffer(int swapChain, int backBuffer)
        {
            return GetBackBuffer(swapChain, backBuffer, BackBufferType.Mono);
        }


        /// <summary>
        /// Gets the palette entries.
        /// </summary>
        /// <param name="paletteNumber">The palette number.</param>
        /// <returns>An array of <see cref="PaletteEntry"/></returns>
        public PaletteEntry[] GetPaletteEntries(int paletteNumber)
        {
            var entries = new PaletteEntry[256];
            GetPaletteEntries(paletteNumber, entries);
            return entries;
        }

        /// <summary>
        /// Gets the pixel shader boolean constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="count">The count.</param>
        /// <returns>An array of boolean constants</returns>
        public bool[] GetPixelShaderBooleanConstant(int startRegister, int count)
        {
            unsafe
            {
                if (count < 1024)
                {
                    var result = stackalloc int[count];
                    GetPixelShaderConstantB(startRegister, (IntPtr)result, count);
                    return Utilities.ConvertToBoolArray(result, count);
                }
                else
                {
                    var result = new RawBool[count];
                    fixed (void* pResult = result)
                        GetPixelShaderConstantB(startRegister, (IntPtr)pResult, count);
                    return Utilities.ConvertToBoolArray(result);
                }
            }
        }

        /// <summary>
        /// Gets the pixel shader float constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="count">The count.</param>
        /// <returns>An array of float constants</returns>
        public float[] GetPixelShaderFloatConstant(int startRegister, int count)
        {
            var result = new float[count];
            GetPixelShaderConstantF(startRegister, result, count);
            return result;
        }

        /// <summary>
        /// Gets the pixel shader integer constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="count">The count.</param>
        /// <returns>An array of int constants</returns>
        public int[] GetPixelShaderIntegerConstant(int startRegister, int count)
        {
            var result = new int[count];
            GetPixelShaderConstantI(startRegister, result, count);
            return result;
        }

        /// <summary>
        /// Gets the state of the render.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>The render state value</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::GetRenderState([In] D3DRENDERSTATETYPE State,[In] void* pValue)</unmanaged>
        public int GetRenderState(SharpDX.Direct3D9.RenderState state)
        {
            unsafe
            {
                int result = 0;
                GetRenderState(state, new IntPtr(&result));
                return result;
            }
        }

        /// <summary>
        /// Gets the state of the render.
        /// </summary>
        /// <typeparam name="T">Type of the state value.</typeparam>
        /// <param name="state">The state.</param>
        /// <returns>
        /// The render state value
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::GetRenderState([In] D3DRENDERSTATETYPE State,[In] void* pValue)</unmanaged>
        public T GetRenderState<T>(SharpDX.Direct3D9.RenderState state) where T : struct
        {
            unsafe
            {
                T result = default(T);
                GetRenderState(state, (IntPtr)Interop.Fixed(ref result));
                return result;
            }
        }

        /// <summary>
        /// Gets the state of the sampler.
        /// </summary>
        /// <param name="sampler">The sampler.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        /// The sampler state value
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::GetSamplerState([In] unsigned int Sampler,[In] D3DSAMPLERSTATETYPE Type,[In] void* pValue)</unmanaged>
        public int GetSamplerState(int sampler, SharpDX.Direct3D9.SamplerState state)
        {
            unsafe
            {
                int result = 0;
                GetSamplerState(sampler, state, new IntPtr(&result));
                return result;
            }
        }

        /// <summary>
        /// Gets the state of the sampler.
        /// </summary>
        /// <typeparam name="T">Type of the sampler state value</typeparam>
        /// <param name="sampler">The sampler.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        /// The sampler state value
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::GetSamplerState([In] unsigned int Sampler,[In] D3DSAMPLERSTATETYPE Type,[In] void* pValue)</unmanaged>
        public T GetSamplerState<T>(int sampler, SharpDX.Direct3D9.SamplerState state) where T : struct
        {
            unsafe
            {
                T result = default(T);
                GetSamplerState(sampler, state, (IntPtr)Interop.Fixed(ref result));
                return result;
            }
        }

        /// <summary>
        /// Gets the state of the texture stage.
        /// </summary>
        /// <param name="stage">The stage.</param>
        /// <param name="type">The type.</param>
        /// <returns>
        /// The texture stage state.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::GetTextureStageState([In] unsigned int Stage,[In] D3DTEXTURESTAGESTATETYPE Type,[In] void* pValue)</unmanaged>
        public int GetTextureStageState(int stage, SharpDX.Direct3D9.TextureStage type)
        {
            unsafe
            {
                int result = 0;
                GetTextureStageState(stage, type, new IntPtr(&result));
                return result;
            }
        }

        /// <summary>
        /// Gets the state of the texture stage.
        /// </summary>
        /// <typeparam name="T">Type of the texture stage state</typeparam>
        /// <param name="stage">The stage.</param>
        /// <param name="type">The type.</param>
        /// <returns>
        /// The texture stage state.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::GetTextureStageState([In] unsigned int Stage,[In] D3DTEXTURESTAGESTATETYPE Type,[In] void* pValue)</unmanaged>
        public T GetTextureStageState<T>(int stage, SharpDX.Direct3D9.TextureStage type) where T : struct
        {
            unsafe
            {
                T result = default(T);
                GetTextureStageState(stage, type, (IntPtr)Interop.Fixed(ref result));
                return result;
            }
        }

        /// <summary>
        /// Gets the vertex shader boolean constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="count">The count.</param>
        /// <returns>An array of boolean constants</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::GetVertexShaderConstantB([In] unsigned int StartRegister,[In] void* pConstantData,[In] unsigned int BoolCount)</unmanaged>	
        public bool[] GetVertexShaderBooleanConstant(int startRegister, int count)
        {
            unsafe
            {
                if (count < 1024)
                {
                    var result = stackalloc int[count];
                    return Utilities.ConvertToBoolArray(result, count);
                }
                else
                {
                    var result = new RawBool[count];
                    fixed (void* pResult = result)
                        GetVertexShaderConstantB(startRegister, (IntPtr)pResult, count);
                    return Utilities.ConvertToBoolArray(result);
                }
            }
        }

        /// <summary>
        /// Gets the vertex shader float constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="count">The count.</param>
        /// <returns>An array of float constants</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::GetVertexShaderConstantF([In] unsigned int StartRegister,[In, Buffer] float* pConstantData,[In] unsigned int Vector4fCount)</unmanaged>	
        public float[] GetVertexShaderFloatConstant(int startRegister, int count)
        {
            var result = new float[count];
            GetVertexShaderConstantF(startRegister, result, count);
            return result;
        }

        /// <summary>
        /// Gets the vertex shader integer constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="count">The count.</param>
        /// <returns>An array of int constants</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::GetVertexShaderConstantI([In] unsigned int StartRegister,[Out] int* pConstantData,[In] unsigned int Vector4iCount)</unmanaged>	
        public int[] GetVertexShaderIntegerConstant(int startRegister, int count)
        {
            var result = new int[count];
            GetVertexShaderConstantI(startRegister, result, count);
            return result;
        }

        /// <summary>
        /// Sets the cursor position.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="flags">if set to <c>true</c> [flags].</param>
        /// <unmanaged>void IDirect3DDevice9::SetCursorPosition([In] int X,[In] int Y,[In] unsigned int Flags)</unmanaged>
        public void SetCursorPosition(RawPoint point, bool flags)
        {
            SetCursorPosition(point.X, point.Y, flags);
        }

        /// <summary>
        /// Sets the cursor position.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="flags">if set to <c>true</c> [flags].</param>
        /// <unmanaged>void IDirect3DDevice9::SetCursorPosition([In] int X,[In] int Y,[In] unsigned int Flags)</unmanaged>
        public void SetCursorPosition(int x, int y, bool flags)
        {
            SetCursorPosition(x, y, flags?1:0);
        }

        /// <summary>
        /// Sets the cursor properties.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="cursorBitmapRef">The cursor bitmap ref.</param>
        /// <returns></returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetCursorProperties([In] unsigned int XHotSpot,[In] unsigned int YHotSpot,[In] IDirect3DSurface9* pCursorBitmap)</unmanaged>
        public void SetCursorProperties(RawPoint point, SharpDX.Direct3D9.Surface cursorBitmapRef)
        {
            SetCursorProperties(point.X, point.Y, cursorBitmapRef);
        }

        /// <summary>
        /// Sets the gamma ramp.
        /// </summary>
        /// <param name="swapChain">The swap chain.</param>
        /// <param name="rampRef">The ramp ref.</param>
        /// <param name="calibrate">if set to <c>true</c> [calibrate].</param>
        /// <unmanaged>void IDirect3DDevice9::SetGammaRamp([In] unsigned int iSwapChain,[In] unsigned int Flags,[In] const D3DGAMMARAMP* pRamp)</unmanaged>
        public void SetGammaRamp(int swapChain, ref SharpDX.Direct3D9.GammaRamp rampRef, bool calibrate)
        {
            SetGammaRamp(swapChain, calibrate ? 1 : 0, ref rampRef);
        }

        /// <summary>
        /// Presents the contents of the next buffer in the sequence of back buffers to the screen.
        /// </summary>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>IDirect3DDevice9::Present</unmanaged>
        public void Present()
        {
            Present(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Presents the contents of the next buffer in the sequence of back buffers to the screen.
        /// </summary>
        /// <param name="sourceRectangle">The area of the back buffer that should be presented.</param>
        /// <param name="destinationRectangle">The area of the front buffer that should receive the result of the presentation.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>IDirect3DDevice9::Present</unmanaged>
        public void Present(RawRectangle sourceRectangle, RawRectangle destinationRectangle)
        {
            Present(sourceRectangle, destinationRectangle, IntPtr.Zero);
        }

        /// <summary>
        /// Presents the contents of the next buffer in the sequence of back buffers to the screen.
        /// </summary>
        /// <param name="sourceRectangle">The area of the back buffer that should be presented.</param>
        /// <param name="destinationRectangle">The area of the front buffer that should receive the result of the presentation.</param>
        /// <param name="windowOverride">The destination window whose client area is taken as the target for this presentation.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>IDirect3DDevice9::Present</unmanaged>
        public void Present(RawRectangle sourceRectangle, RawRectangle destinationRectangle, IntPtr windowOverride)
        {
            unsafe
            {
                Present(new IntPtr(&sourceRectangle), new IntPtr(&destinationRectangle), windowOverride, IntPtr.Zero);
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
        public void Present(RawRectangle sourceRectangle, RawRectangle destinationRectangle, IntPtr windowOverride, IntPtr dirtyRegionRGNData)
        {
            unsafe
            {
                Present(new IntPtr(&sourceRectangle), new IntPtr(&destinationRectangle), windowOverride, dirtyRegionRGNData);
            }
        }

        /// <summary>
        /// Resets the stream source frequency by setting the frequency to 1.
        /// </summary>
        /// <param name="stream">The stream index.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetStreamSourceFreq([In] unsigned int StreamNumber,[In] unsigned int Setting)</unmanaged>
        public void ResetStreamSourceFrequency(int stream)
        {
            SetStreamSourceFrequency(stream, 1);
        }

        /// <summary>
        /// Sets the pixel shader constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="data">The data.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetPixelShaderConstantF([In] unsigned int StartRegister,[In] const void* pConstantData,[In] unsigned int Vector4fCount)</unmanaged>
        public void SetPixelShaderConstant(int startRegister, RawMatrix[] data)
        {
            unsafe
            {
                fixed (void* pData = data)
                    SetPixelShaderConstantF(startRegister, (IntPtr)pData, data.Length << 2); // *4 is enough
            }                              
        }

        /// <summary>
        /// Sets the pixel shader constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="data">The data.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetPixelShaderConstantF([In] unsigned int StartRegister,[In] const void* pConstantData,[In] unsigned int Vector4fCount)</unmanaged>
        public void SetPixelShaderConstant(int startRegister, RawVector4[] data)
        {
            unsafe
            {
                fixed (void* pData = data)
                    SetPixelShaderConstantF(startRegister, (IntPtr)pData, data.Length); // a vector4 is only one register
            }                  
        }

        /// <summary>
        /// Sets the pixel shader constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="data">The data.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetPixelShaderConstantB([In] unsigned int StartRegister,[In] const void* pConstantData,[In] unsigned int BoolCount)</unmanaged>
        public void SetPixelShaderConstant(int startRegister, bool[] data)
        {
            unsafe
            {
                if (data.Length < 1024)
                {
                    var result = stackalloc int[data.Length];
                    Utilities.ConvertToIntArray(data, result);
                    SetPixelShaderConstantB(startRegister, (IntPtr)result, data.Length);
                }
                else
                {
                    var result = Utilities.ConvertToIntArray(data);
                    fixed (void* pResult = result)
                        SetPixelShaderConstantB(startRegister, (IntPtr)pResult, data.Length);
                }
            }            
        }

        /// <summary>
        /// Sets the pixel shader constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="data">The data.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetPixelShaderConstantI([In] unsigned int StartRegister,[In] const void* pConstantData,[In] unsigned int Vector4iCount)</unmanaged>
        public void SetPixelShaderConstant(int startRegister, int[] data)
        {
            unsafe
            {
                fixed (void* pData = data)
                    SetPixelShaderConstantI(startRegister, (IntPtr)pData, data.Length >> 2); // /4 as it's the count of Vector4i
            }                      
        }

        /// <summary>
        /// Sets the pixel shader constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="data">The data.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetPixelShaderConstantF([In] unsigned int StartRegister,[In] const void* pConstantData,[In] unsigned int Vector4fCount)</unmanaged>
        public void SetPixelShaderConstant(int startRegister, float[] data)
        {
            unsafe
            {
                fixed (void* pData = data)
                    SetPixelShaderConstantF(startRegister, (IntPtr)pData, data.Length >> 2); // /4 as it's the count of Vector4f
            }            
        }

        /// <summary>
        /// Sets the pixel shader constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="data">The data.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetPixelShaderConstantF([In] unsigned int StartRegister,[In] const void* pConstantData,[In] unsigned int Vector4fCount)</unmanaged>
        public unsafe void SetPixelShaderConstant(int startRegister, RawMatrix* data)
        {
            SetPixelShaderConstantF(startRegister, (IntPtr)data, 4); // a matrix is only 4 registers
        }

        /// <summary>
        /// Sets the pixel shader constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="data">The data.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetPixelShaderConstantF([In] unsigned int StartRegister,[In] const void* pConstantData,[In] unsigned int Vector4fCount)</unmanaged>
        public void SetPixelShaderConstant(int startRegister, RawMatrix data)
        {
            unsafe
            {
                SetPixelShaderConstantF(startRegister, new IntPtr(&data), 4); // a matrix is only 4 registers
            }
        }

        /// <summary>
        /// Sets the pixel shader constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="data">The data.</param>
        /// <param name="count">The count.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetPixelShaderConstantF([In] unsigned int StartRegister,[In] const void* pConstantData,[In] unsigned int Vector4fCount)</unmanaged>
        public unsafe void SetPixelShaderConstant(int startRegister, RawMatrix* data, int count)
        {
            SetPixelShaderConstantF(startRegister, (IntPtr)data, count << 2); // *4 is enough as a matrix is still 4 registers
        }

        /// <summary>
        /// Sets the pixel shader constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="data">The data.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetPixelShaderConstantF([In] unsigned int StartRegister,[In] const void* pConstantData,[In] unsigned int Vector4fCount)</unmanaged>
        public void SetPixelShaderConstant(int startRegister, RawMatrix[] data, int offset, int count)
        {
            unsafe
            {
                fixed (void* pData = &data[offset])
					SetPixelShaderConstantF(startRegister, (IntPtr)pData, count << 2); // *4 is enough as a matrix is still 4 registers
            }
        }

        /// <summary>
        /// Sets the pixel shader constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="data">The data.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetPixelShaderConstantF([In] unsigned int StartRegister,[In] const void* pConstantData,[In] unsigned int Vector4fCount)</unmanaged>
        public void SetPixelShaderConstant(int startRegister, RawVector4[] data, int offset, int count)
        {
            unsafe
            {
                fixed (void* pData = &data[offset])
                    SetPixelShaderConstantF(startRegister, (IntPtr)pData, count); // count is enough, as a Vector4f is only one register
            }            
        }

        /// <summary>
        /// Sets the pixel shader constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="data">The data.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetPixelShaderConstantB([In] unsigned int StartRegister,[In] const void* pConstantData,[In] unsigned int BoolCount)</unmanaged>
        public void SetPixelShaderConstant(int startRegister, bool[] data, int offset, int count)
        {
            unsafe
            {
                if (count < 1024)
                {
                    var result = stackalloc int[data.Length];
                    Utilities.ConvertToIntArray(data, result);
                    SetPixelShaderConstantB(startRegister, new IntPtr(&result[offset]), count);
                }
                else
                {
                    var result = Utilities.ConvertToIntArray(data);                    
                    fixed (void* pResult = &result[offset])
                        SetPixelShaderConstantB(startRegister, (IntPtr)pResult, count);
                }
            }
        }

        /// <summary>
        /// Sets the pixel shader constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="data">The data.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetPixelShaderConstantI([In] unsigned int StartRegister,[In] const void* pConstantData,[In] unsigned int Vector4iCount)</unmanaged>
        public void SetPixelShaderConstant(int startRegister, int[] data, int offset, int count)
        {
            unsafe
            {
				fixed (void* pData = &data[offset])
					SetPixelShaderConstantI(startRegister, (IntPtr) pData, count);
            }            
        }

        /// <summary>
        /// Sets the pixel shader constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="data">The data.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetPixelShaderConstantF([In] unsigned int StartRegister,[In] const void* pConstantData,[In] unsigned int Vector4fCount)</unmanaged>
        public void SetPixelShaderConstant(int startRegister, float[] data, int offset, int count)
        {
            unsafe
            {
                fixed (void* pData = &data[offset])
                    SetPixelShaderConstantF(startRegister, (IntPtr)pData, count);
            }
        }

        /// <summary>
        /// Sets the RenderState.
        /// </summary>
        /// <param name="renderState">State of the render.</param>
        /// <param name="enable">if set to <c>true</c> [enable].</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetRenderState([In] D3DRENDERSTATETYPE State,[In] unsigned int Value)</unmanaged>
        public void SetRenderState(RenderState renderState, bool enable)
        {
            SetRenderState(renderState, enable ? 1 : 0);
        }

        /// <summary>
        /// Sets the RenderState.
        /// </summary>
        /// <param name="renderState">State of the render.</param>
        /// <param name="value">A float value.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetRenderState([In] D3DRENDERSTATETYPE State,[In] unsigned int Value)</unmanaged>
        public void SetRenderState(RenderState renderState, float value) 
        {
            unsafe
            {
                SetRenderState(renderState, *(int*)&value);
            }
        }

        /// <summary>
        /// Sets the RenderState.
        /// </summary>
        /// <typeparam name="T">Type of the enum value</typeparam>
        /// <param name="renderState">State of the render.</param>
        /// <param name="value">An enum value.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetRenderState([In] D3DRENDERSTATETYPE State,[In] unsigned int Value)</unmanaged>
        public void SetRenderState<T>(RenderState renderState, T value) where T : struct, IConvertible
        {
            if (!typeof(T).GetTypeInfo().IsEnum)
                throw new ArgumentException("T must be an enum type", "value");

            SetRenderState(renderState, value.ToInt32(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Sets the SamplerState.
        /// </summary>
        /// <param name="sampler">The sampler.</param>
        /// <param name="type">The type.</param>
        /// <param name="textureFilter">The texture filter.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetSamplerState([In] unsigned int Sampler,[In] D3DSAMPLERSTATETYPE Type,[In] unsigned int Value)</unmanaged>
        public void SetSamplerState(int sampler, SharpDX.Direct3D9.SamplerState type, TextureFilter textureFilter)
        {
            SetSamplerState(sampler, type, (int)textureFilter);
        }


        /// <summary>
        /// Sets the SamplerState.
        /// </summary>
        /// <param name="sampler">The sampler.</param>
        /// <param name="type">The type.</param>
        /// <param name="textureAddress">The texture address.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetSamplerState([In] unsigned int Sampler,[In] D3DSAMPLERSTATETYPE Type,[In] unsigned int Value)</unmanaged>
        public void SetSamplerState(int sampler, SharpDX.Direct3D9.SamplerState type, TextureAddress textureAddress)
        {
            SetSamplerState(sampler, type, (int)textureAddress);
        }

        /// <summary>
        /// Sets the SamplerState.
        /// </summary>
        /// <param name="sampler">The sampler.</param>
        /// <param name="type">The type.</param>
        /// <param name="value">A float value.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetSamplerState([In] unsigned int Sampler,[In] D3DSAMPLERSTATETYPE Type,[In] unsigned int Value)</unmanaged>
        public void SetSamplerState(int sampler, SharpDX.Direct3D9.SamplerState type, float value)
        {
            unsafe
            {
                SetSamplerState(sampler, type, *(int*)&value);
            }
        }

        /// <summary>
        /// Sets the stream source frequency.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="frequency">The frequency.</param>
        /// <param name="source">The source.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetStreamSourceFreq([In] unsigned int StreamNumber,[In] unsigned int Setting)</unmanaged>
        public void SetStreamSourceFrequency(int stream, int frequency, StreamSource source)
        {
            int value = (source == StreamSource.IndexedData) ? 0x40000000 : unchecked((int)0x80000000);
            SetStreamSourceFrequency(stream, frequency | value);
        }

        /// <summary>
        /// Sets the state of the texture stage.
        /// </summary>
        /// <param name="stage">The stage.</param>
        /// <param name="type">The type.</param>
        /// <param name="textureArgument">The texture argument.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetTextureStageState([In] unsigned int Stage,[In] D3DTEXTURESTAGESTATETYPE Type,[In] unsigned int Value)</unmanaged>
        public void SetTextureStageState(int stage, TextureStage type, TextureArgument textureArgument)
        {
            SetTextureStageState(stage, type, (int)textureArgument);
        }

        /// <summary>
        /// Sets the state of the texture stage.
        /// </summary>
        /// <param name="stage">The stage.</param>
        /// <param name="type">The type.</param>
        /// <param name="textureOperation">The texture operation.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetTextureStageState([In] unsigned int Stage,[In] D3DTEXTURESTAGESTATETYPE Type,[In] unsigned int Value)</unmanaged>
        public void SetTextureStageState(int stage, TextureStage type, TextureOperation textureOperation)
        {
            SetTextureStageState(stage, type, (int)textureOperation);
        }

        /// <summary>
        /// Sets the state of the texture stage.
        /// </summary>
        /// <param name="stage">The stage.</param>
        /// <param name="type">The type.</param>
        /// <param name="textureTransform">The texture transform.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetTextureStageState([In] unsigned int Stage,[In] D3DTEXTURESTAGESTATETYPE Type,[In] unsigned int Value)</unmanaged>
        public void SetTextureStageState(int stage, TextureStage type, TextureTransform textureTransform)
        {
            SetTextureStageState(stage, type, (int)textureTransform);
        }

        /// <summary>
        /// Sets the state of the texture stage.
        /// </summary>
        /// <param name="stage">The stage.</param>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetTextureStageState([In] unsigned int Stage,[In] D3DTEXTURESTAGESTATETYPE Type,[In] unsigned int Value)</unmanaged>
        public void SetTextureStageState(int stage, TextureStage type, float value)
        {
            unsafe
            {
                SetTextureStageState(stage, type, *(int*)&value);
            }
        }

        /// <summary>
        /// Sets the transform.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="matrixRef">The matrix ref.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetTransform([In] D3DTRANSFORMSTATETYPE State,[In] const D3DMATRIX* pMatrix)</unmanaged>
        public void SetTransform(SharpDX.Direct3D9.TransformState state, ref RawMatrix matrixRef)
        {
            SetTransform_((int)state, ref matrixRef);
        }

        /// <summary>
        /// Sets the transform.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="matrixRef">The matrix ref.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetTransform([In] D3DTRANSFORMSTATETYPE State,[In] const D3DMATRIX* pMatrix)</unmanaged>
        public void SetTransform(int index, ref RawMatrix matrixRef)
        {
            SetTransform_(index + 256, ref matrixRef);
        }

        /// <summary>
        /// Sets the transform.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="matrixRef">The matrix ref.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetTransform([In] D3DTRANSFORMSTATETYPE State,[In] const D3DMATRIX* pMatrix)</unmanaged>
        public void SetTransform(SharpDX.Direct3D9.TransformState state, RawMatrix matrixRef)
        {
            SetTransform_((int)state, ref matrixRef);
        }

        /// <summary>
        /// Sets the transform.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="matrixRef">The matrix ref.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetTransform([In] D3DTRANSFORMSTATETYPE State,[In] const D3DMATRIX* pMatrix)</unmanaged>
        public void SetTransform(int index, RawMatrix matrixRef)
        {
            SetTransform_(index + 256, ref matrixRef);
        }

        /// <summary>
        /// Sets the vertex shader constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="data">The data.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetVertexShaderConstantF([In] unsigned int StartRegister,[In] const void* pConstantData,[In] unsigned int Vector4fCount)</unmanaged>
        public void SetVertexShaderConstant(int startRegister, RawMatrix[] data)
        {
            unsafe
            {
                fixed (void* pData = data)
                    SetVertexShaderConstantF(startRegister, (IntPtr)pData, data.Length << 2);
            }
        }

        /// <summary>
        /// Sets the vertex shader constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="data">The data.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetVertexShaderConstantF([In] unsigned int StartRegister,[In] const void* pConstantData,[In] unsigned int Vector4fCount)</unmanaged>
        public void SetVertexShaderConstant(int startRegister, RawVector4[] data)
        {
            unsafe
            {
                fixed (void* pData = data)
                    SetVertexShaderConstantF(startRegister, (IntPtr)pData, data.Length >> 2);
            }
        }

        /// <summary>
        /// Sets the vertex shader constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="data">The data.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetVertexShaderConstantB([In] unsigned int StartRegister,[In] const void* pConstantData,[In] unsigned int BoolCount)</unmanaged>
        public void SetVertexShaderConstant(int startRegister, bool[] data)
        {
            unsafe
            {
                if (data.Length < 1024)
                {
                    var result = stackalloc int[data.Length];
                    Utilities.ConvertToIntArray(data, result);
                    SetVertexShaderConstantB(startRegister, (IntPtr)result, data.Length);
                }
                else
                {
                    var result = Utilities.ConvertToIntArray(data);
                    fixed (void* pResult = result)
                        SetVertexShaderConstantB(startRegister, (IntPtr)pResult, data.Length);
                }
            }
        }

        /// <summary>
        /// Sets the vertex shader constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="data">The data.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetVertexShaderConstantI([In] unsigned int StartRegister,[In] const void* pConstantData,[In] unsigned int Vector4iCount)</unmanaged>
        public void SetVertexShaderConstant(int startRegister, int[] data)
        {
            unsafe
            {
                fixed (void* pData = data)
                    SetVertexShaderConstantI(startRegister, (IntPtr)pData, data.Length >> 2);
            }
        }

        /// <summary>
        /// Sets the vertex shader constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="data">The data.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetVertexShaderConstantF([In] unsigned int StartRegister,[In] const void* pConstantData,[In] unsigned int Vector4fCount)</unmanaged>
        public void SetVertexShaderConstant(int startRegister, float[] data)
        {
            unsafe
            {
                fixed (void* pData = data)
                    SetVertexShaderConstantF(startRegister, (IntPtr)pData, data.Length >> 2);
            }
        }

        /// <summary>
        /// Sets the vertex shader constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="data">The data.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetVertexShaderConstantF([In] unsigned int StartRegister,[In] const void* pConstantData,[In] unsigned int Vector4fCount)</unmanaged>
        public unsafe void SetVertexShaderConstant(int startRegister, RawMatrix* data)
        {
            SetVertexShaderConstantF(startRegister, (IntPtr)data, 4);
        }

        /// <summary>
        /// Sets the vertex shader constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="data">The data.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetVertexShaderConstantF([In] unsigned int StartRegister,[In] const void* pConstantData,[In] unsigned int Vector4fCount)</unmanaged>
        public void SetVertexShaderConstant(int startRegister, RawMatrix data)
        {
            unsafe
            {
                SetVertexShaderConstantF(startRegister, new IntPtr(&data), 4);
            }
        }

        /// <summary>
        /// Sets the vertex shader constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="data">The data.</param>
        /// <param name="count">The count.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetVertexShaderConstantF([In] unsigned int StartRegister,[In] const void* pConstantData,[In] unsigned int Vector4fCount)</unmanaged>
        public unsafe void SetVertexShaderConstant(int startRegister, RawMatrix* data, int count)
        {
            SetVertexShaderConstantF(startRegister, (IntPtr)data, count << 2);
        }

        /// <summary>
        /// Sets the vertex shader constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="data">The data.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetVertexShaderConstantF([In] unsigned int StartRegister,[In] const void* pConstantData,[In] unsigned int Vector4fCount)</unmanaged>
        public void SetVertexShaderConstant(int startRegister, RawMatrix[] data, int offset, int count)
        {
            unsafe
            {
                fixed (void* pData = &data[offset])
                    SetVertexShaderConstantF(startRegister, (IntPtr)pData, count << 2);
            }
        }

        /// <summary>
        /// Sets the vertex shader constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="data">The data.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetVertexShaderConstantF([In] unsigned int StartRegister,[In] const void* pConstantData,[In] unsigned int Vector4fCount)</unmanaged>
        public void SetVertexShaderConstant(int startRegister, RawVector4[] data, int offset, int count)
        {
            unsafe
            {
                fixed (void* pData = &data[offset])
                    SetVertexShaderConstantF(startRegister, (IntPtr)pData, count >> 2);
            }
        }

        /// <summary>
        /// Sets the vertex shader constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="data">The data.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetVertexShaderConstantB([In] unsigned int StartRegister,[In] const void* pConstantData,[In] unsigned int BoolCount)</unmanaged>
        public void SetVertexShaderConstant(int startRegister, bool[] data, int offset, int count)
        {
            unsafe
            {
                if (count < 1024)
                {
                    var result = stackalloc int[data.Length];
                    Utilities.ConvertToIntArray(data, result);
                    SetVertexShaderConstantB(startRegister, new IntPtr(&result[offset]), count);
                }
                else
                {
                    var result = Utilities.ConvertToIntArray(data);
                    fixed (void* pResult = &result[offset])
                        SetVertexShaderConstantB(startRegister, (IntPtr)pResult, count);
                }
            }
        }

        /// <summary>
        /// Sets the vertex shader constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="data">The data.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetVertexShaderConstantI([In] unsigned int StartRegister,[In] const void* pConstantData,[In] unsigned int Vector4iCount)</unmanaged>
        public void SetVertexShaderConstant(int startRegister, int[] data, int offset, int count)
        {
            unsafe
            {
                fixed (void* pData = &data[offset])
                    SetVertexShaderConstantI(startRegister, (IntPtr)pData, count);
            }
        }

        /// <summary>
        /// Sets the vertex shader constant.
        /// </summary>
        /// <param name="startRegister">The start register.</param>
        /// <param name="data">The data.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::SetVertexShaderConstantF([In] unsigned int StartRegister,[In] const void* pConstantData,[In] unsigned int Vector4fCount)</unmanaged>
        public void SetVertexShaderConstant(int startRegister, float[] data, int offset, int count)
        {
            unsafe
            {
                fixed (void* pData = &data[offset])
                    SetVertexShaderConstantF(startRegister, (IntPtr)pData, count);
            }
        }

        /// <summary>
        /// Stretches the rectangle.
        /// </summary>
        /// <param name="sourceSurfaceRef">The source surface ref.</param>
        /// <param name="destSurfaceRef">The dest surface ref.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::StretchRect([In] IDirect3DSurface9* pSourceSurface,[In, Optional] const RECT* pSourceRect,[In] IDirect3DSurface9* pDestSurface,[In, Optional] const RECT* pDestRect,[In] D3DTEXTUREFILTERTYPE Filter)</unmanaged>
        public void StretchRectangle(SharpDX.Direct3D9.Surface sourceSurfaceRef, SharpDX.Direct3D9.Surface destSurfaceRef, SharpDX.Direct3D9.TextureFilter filter)
        {
            StretchRectangle(sourceSurfaceRef, null, destSurfaceRef, null, filter);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the cursor can be displayed.
        /// </summary>
        /// <value>
        /// <c>true</c> if the cursor can be displayed; otherwise, <c>false</c>.
        /// </value>
        public bool ShowCursor
        {
            get
            {
                bool previousValue = GetSetShowCursor(true);
                GetSetShowCursor(previousValue);
                return previousValue;
            }

            set
            {
                GetSetShowCursor(value);
            }
        }

        /// <summary>
        /// Updates the surface.
        /// </summary>
        /// <param name="sourceSurfaceRef">The source surface ref.</param>
        /// <param name="destinationSurfaceRef">The destination surface ref.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT IDirect3DDevice9::UpdateSurface([In] IDirect3DSurface9* pSourceSurface,[In] const RECT* pSourceRect,[In] IDirect3DSurface9* pDestinationSurface,[In] const POINT* pDestPoint)</unmanaged>
        public void UpdateSurface(SharpDX.Direct3D9.Surface sourceSurfaceRef, SharpDX.Direct3D9.Surface destinationSurfaceRef)
        {
            UpdateSurface(sourceSurfaceRef, null, destinationSurfaceRef, null);
        }

    }
}
