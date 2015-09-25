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
using SharpDX;
using SharpDX.Mathematics.Interop;

namespace SharpDX.Direct3D11
{
    public partial class OutputMergerStage
    {
        /// <summary>	
        /// Get references to the render targets that are available to the {{output-merger stage}}.	
        /// </summary>	
        /// <remarks>	
        /// Any returned interfaces will have their reference count incremented by one. Applications should call {{IUnknown::Release}} on the returned interfaces when they are no longer needed to avoid memory leaks. 	
        /// </remarks>	
        /// <returns>a depth-stencil view (see <see cref="SharpDX.Direct3D11.DepthStencilView"/>) to be filled with the depth-stencil information from the device.</returns>
        /// <unmanaged>void OMGetRenderTargets([In] int NumViews,[Out, Buffer, Optional] ID3D10RenderTargetView** ppRenderTargetViews,[Out, Optional] ID3D10DepthStencilView** ppDepthStencilView)</unmanaged>
        public void GetRenderTargets(out SharpDX.Direct3D11.DepthStencilView depthStencilViewRef)
        {
            GetRenderTargets(0, new RenderTargetView[0], out depthStencilViewRef);
        }

        /// <summary>	
        /// Get references to the render targets that are available to the {{output-merger stage}}.	
        /// </summary>	
        /// <remarks>	
        /// Any returned interfaces will have their reference count incremented by one. Applications should call {{IUnknown::Release}} on the returned interfaces when they are no longer needed to avoid memory leaks. 	
        /// </remarks>	
        /// <param name="numViews">Number of render targets to retrieve. </param>
        /// <returns>an array of render targets views (see <see cref="SharpDX.Direct3D11.RenderTargetView"/>) to be filled with the render targets from the device.</returns>
        /// <unmanaged>void OMGetRenderTargets([In] int NumViews,[Out, Buffer, Optional] ID3D10RenderTargetView** ppRenderTargetViews,[Out, Optional] ID3D10DepthStencilView** ppDepthStencilView)</unmanaged>
        public SharpDX.Direct3D11.RenderTargetView[] GetRenderTargets(int numViews)
        {
            var renderTargets = new RenderTargetView[numViews];
            DepthStencilView depthStencilView;
            GetRenderTargets(numViews, renderTargets, out depthStencilView);
            if (depthStencilView != null)
                depthStencilView.Dispose();
            return renderTargets;
        }

        /// <summary>	
        /// Get references to the render targets and the depth-stencil buffer that are available to the {{output-merger stage}}.	
        /// </summary>	
        /// <remarks>	
        /// Any returned interfaces will have their reference count incremented by one. Applications should call {{IUnknown::Release}} on the returned interfaces when they are no longer needed to avoid memory leaks. 	
        /// </remarks>	
        /// <param name="numViews">Number of render targets to retrieve. </param>
        /// <param name="depthStencilViewRef">Pointer to a depth-stencil view (see <see cref="SharpDX.Direct3D11.DepthStencilView"/>) to be filled with the depth-stencil information from the device.</param>
        /// <returns>an array of render targets views (see <see cref="SharpDX.Direct3D11.RenderTargetView"/>) to be filled with the render targets from the device.</returns>
        /// <unmanaged>void OMGetRenderTargets([In] int NumViews,[Out, Buffer, Optional] ID3D10RenderTargetView** ppRenderTargetViews,[Out, Optional] ID3D10DepthStencilView** ppDepthStencilView)</unmanaged>
        public SharpDX.Direct3D11.RenderTargetView[] GetRenderTargets(int numViews, out SharpDX.Direct3D11.DepthStencilView depthStencilViewRef)
        {
            var renderTargets = new RenderTargetView[numViews];
            GetRenderTargets(numViews, renderTargets, out depthStencilViewRef);
            return renderTargets;
        }

        /// <summary>	
        /// Get the {{blend state}} of the output-merger stage.	
        /// </summary>	
        /// <remarks>	
        /// The reference count of the returned interface will be incremented by one when the blend state is retrieved. Applications must release returned reference(s) when they are no longer needed, or else there will be a memory leak. 	
        /// </remarks>	
        /// <param name="blendFactor">Array of blend factors, one for each RGBA component. </param>
        /// <param name="sampleMaskRef">Pointer to a {{sample mask}}. </param>
        /// <returns>a reference to a blend-state interface (see <see cref="SharpDX.Direct3D11.BlendState"/>).</returns>
        /// <unmanaged>void OMGetBlendState([Out, Optional] ID3D10BlendState** ppBlendState,[Out, Optional] float BlendFactor[4],[Out, Optional] int* pSampleMask)</unmanaged>
        public SharpDX.Direct3D11.BlendState GetBlendState(out RawColor4 blendFactor, out int sampleMaskRef)
        {
            BlendState blendState;
            GetBlendState(out blendState, out blendFactor, out sampleMaskRef);
            return blendState;
        }

        /// <summary>	
        /// Gets the {{depth-stencil}} state of the output-merger stage.	
        /// </summary>	
        /// <remarks>	
        /// Any returned interfaces will have their reference count incremented by one. Applications should call {{IUnknown::Release}} on the returned interfaces when they are no longer needed to avoid memory leaks. 	
        /// </remarks>	
        /// <param name="stencilRefRef">Pointer to the stencil reference value used in the {{depth-stencil}} test. </param>
        /// <returns>a reference to a depth-stencil state interface (see <see cref="SharpDX.Direct3D11.DepthStencilState"/>) to be filled with information from the device.</returns>
        /// <unmanaged>void OMGetDepthStencilState([Out, Optional] ID3D10DepthStencilState** ppDepthStencilState,[Out, Optional] int* pStencilRef)</unmanaged>
        public SharpDX.Direct3D11.DepthStencilState GetDepthStencilState(out int stencilRefRef)
        {
            DepthStencilState temp;
            GetDepthStencilState(out temp, out stencilRefRef);
            return temp;
        }

        /// <summary>	
        /// Gets an array of views for an unordered resource.	
        /// </summary>	
        /// <remarks>	
        /// Any returned interfaces will have their reference count incremented by one. Applications should call IUnknown::Release on the returned interfaces when they are no longer needed to avoid memory leaks. 	
        /// </remarks>	
        /// <param name="startSlot">Index of the first element in the zero-based array to return (ranges from 0 to D3D11_PS_CS_UAV_REGISTER_COUNT - 1). </param>
        /// <param name="count">Number of views to get (ranges from 0 to D3D11_PS_CS_UAV_REGISTER_COUNT - StartSlot). </param>
        /// <unmanaged>void OMGetRenderTargetsAndUnorderedAccessViews([In] int NumRTVs,[Out, Buffer, Optional] ID3D11RenderTargetView** ppRenderTargetViews,[Out, Optional] ID3D11DepthStencilView** ppDepthStencilView,[In] int UAVStartSlot,[In] int NumUAVs,[Out, Buffer, Optional] ID3D11UnorderedAccessView** ppUnorderedAccessViews)</unmanaged>
        public UnorderedAccessView[] GetUnorderedAccessViews(int startSlot, int count)
        {
            var temp = new UnorderedAccessView[count];
            DepthStencilView depthStencilView;
            GetRenderTargetsAndUnorderedAccessViews(0, new RenderTargetView[0], out depthStencilView, startSlot, count, temp);
            depthStencilView.Dispose();
            return temp;
        }

        /// <summary>
        ///   Unbinds all depth-stencil buffer and render targets from the output-merger stage.
        /// </summary>
        /// <msdn-id>ff476464</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargets([In] unsigned int NumViews,[In] const void** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargets</unmanaged-short>	
        public void ResetTargets()
        {
            SetRenderTargets(0, IntPtr.Zero, null);
        }

        /// <summary>	
        /// <p>Bind one or more render targets atomically and the depth-stencil buffer to the output-merger stage.</p>	
        /// </summary>	
        /// <param name = "renderTargetViews">A set of render target views to bind.</param>
        /// <remarks>	
        /// <p>The maximum number of active render targets a device can have active at any given time is set by a #define in D3D11.h called  D3D11_SIMULTANEOUS_RENDER_TARGET_COUNT. It is invalid to try to set the same subresource to multiple render target slots.  Any render targets not defined by this call are set to <strong><c>null</c></strong>.</p><p>If any subresources are also currently bound for reading in a different stage or writing (perhaps in a different part of the pipeline),  those bind points will be set to <strong><c>null</c></strong>, in order to prevent the same subresource from being read and written simultaneously in a single rendering operation.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p><p>If the render-target views were created from an array resource type, then all of the render-target views must have the same array size.   This restriction also applies to the depth-stencil view, its array size must match that of the render-target views being bound.</p><p>The pixel shader must be able to simultaneously render to at least eight separate render targets. All of these render targets must access the same type of resource: Buffer, Texture1D, Texture1DArray, Texture2D, Texture2DArray, Texture3D, or TextureCube. All render targets must have the same size in all dimensions (width and height, and depth for 3D or array size for *Array types). If render targets use multisample anti-aliasing, all bound render targets and depth buffer must be the same form of multisample resource (that is, the sample counts must be the same). Each render target can have a different data format. These render target formats are not required to have identical bit-per-element counts.</p><p>Any combination of the eight slots for render targets can have a render target set or not set.</p><p>The same resource view cannot be bound to multiple render target slots simultaneously. However, you can set multiple non-overlapping resource views of a single resource as simultaneous multiple render targets.</p>	
        /// </remarks>	
        /// <msdn-id>ff476464</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargets([In] unsigned int NumViews,[In] const void** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargets</unmanaged-short>	
        public void SetTargets(params RenderTargetView[] renderTargetViews)
        {
            SetTargets((DepthStencilView)null, renderTargetViews);
        }

        /// <summary>	
        ///   Binds a single render target to the output-merger stage.
        /// </summary>	
        /// <param name = "renderTargetView">A view of the render target to bind.</param>
        /// <remarks>	
        /// <p>The maximum number of active render targets a device can have active at any given time is set by a #define in D3D11.h called  D3D11_SIMULTANEOUS_RENDER_TARGET_COUNT. It is invalid to try to set the same subresource to multiple render target slots.  Any render targets not defined by this call are set to <strong><c>null</c></strong>.</p><p>If any subresources are also currently bound for reading in a different stage or writing (perhaps in a different part of the pipeline),  those bind points will be set to <strong><c>null</c></strong>, in order to prevent the same subresource from being read and written simultaneously in a single rendering operation.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p><p>If the render-target views were created from an array resource type, then all of the render-target views must have the same array size.   This restriction also applies to the depth-stencil view, its array size must match that of the render-target views being bound.</p><p>The pixel shader must be able to simultaneously render to at least eight separate render targets. All of these render targets must access the same type of resource: Buffer, Texture1D, Texture1DArray, Texture2D, Texture2DArray, Texture3D, or TextureCube. All render targets must have the same size in all dimensions (width and height, and depth for 3D or array size for *Array types). If render targets use multisample anti-aliasing, all bound render targets and depth buffer must be the same form of multisample resource (that is, the sample counts must be the same). Each render target can have a different data format. These render target formats are not required to have identical bit-per-element counts.</p><p>Any combination of the eight slots for render targets can have a render target set or not set.</p><p>The same resource view cannot be bound to multiple render target slots simultaneously. However, you can set multiple non-overlapping resource views of a single resource as simultaneous multiple render targets.</p>	
        /// </remarks>	
        /// <msdn-id>ff476464</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargets([In] unsigned int NumViews,[In] const void** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargets</unmanaged-short>	
        public void SetTargets(RenderTargetView renderTargetView)
        {
            SetTargets((DepthStencilView)null, renderTargetView);
        }

        /// <summary>
        ///   Binds a depth-stencil buffer and a set of render targets to the output-merger stage.
        /// </summary>
        /// <param name = "depthStencilView">A view of the depth-stencil buffer to bind.</param>
        /// <param name = "renderTargetViews">A set of render target views to bind.</param>
        /// <remarks>	
        /// <p>The maximum number of active render targets a device can have active at any given time is set by a #define in D3D11.h called  D3D11_SIMULTANEOUS_RENDER_TARGET_COUNT. It is invalid to try to set the same subresource to multiple render target slots.  Any render targets not defined by this call are set to <strong><c>null</c></strong>.</p><p>If any subresources are also currently bound for reading in a different stage or writing (perhaps in a different part of the pipeline),  those bind points will be set to <strong><c>null</c></strong>, in order to prevent the same subresource from being read and written simultaneously in a single rendering operation.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p><p>If the render-target views were created from an array resource type, then all of the render-target views must have the same array size.   This restriction also applies to the depth-stencil view, its array size must match that of the render-target views being bound.</p><p>The pixel shader must be able to simultaneously render to at least eight separate render targets. All of these render targets must access the same type of resource: Buffer, Texture1D, Texture1DArray, Texture2D, Texture2DArray, Texture3D, or TextureCube. All render targets must have the same size in all dimensions (width and height, and depth for 3D or array size for *Array types). If render targets use multisample anti-aliasing, all bound render targets and depth buffer must be the same form of multisample resource (that is, the sample counts must be the same). Each render target can have a different data format. These render target formats are not required to have identical bit-per-element counts.</p><p>Any combination of the eight slots for render targets can have a render target set or not set.</p><p>The same resource view cannot be bound to multiple render target slots simultaneously. However, you can set multiple non-overlapping resource views of a single resource as simultaneous multiple render targets.</p>	
        /// </remarks>	
        /// <msdn-id>ff476464</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargets([In] unsigned int NumViews,[In] const void** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargets</unmanaged-short>	
        public void SetTargets(DepthStencilView depthStencilView, params RenderTargetView[] renderTargetViews)
        {
            SetRenderTargets(renderTargetViews == null ? 0 : renderTargetViews.Length, renderTargetViews, depthStencilView);
        }

        /// <summary>
        /// Binds a depth-stencil buffer and a set of render targets to the output-merger stage.
        /// </summary>
        /// <param name="depthStencilView">A view of the depth-stencil buffer to bind.</param>
        /// <param name="renderTargetCount">The render target count.</param>
        /// <param name="renderTargetViews">A set of render target views to bind.</param>
        /// <msdn-id>ff476464</msdn-id>
        ///   <unmanaged>void ID3D11DeviceContext::OMSetRenderTargets([In] unsigned int NumViews,[In] const void** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView)</unmanaged>
        ///   <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargets</unmanaged-short>
        /// <remarks><p>The maximum number of active render targets a device can have active at any given time is set by a #define in D3D11.h called  D3D11_SIMULTANEOUS_RENDER_TARGET_COUNT. It is invalid to try to set the same subresource to multiple render target slots.  Any render targets not defined by this call are set to <strong><c>null</c></strong>.</p><p>If any subresources are also currently bound for reading in a different stage or writing (perhaps in a different part of the pipeline),  those bind points will be set to <strong><c>null</c></strong>, in order to prevent the same subresource from being read and written simultaneously in a single rendering operation.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p><p>If the render-target views were created from an array resource type, then all of the render-target views must have the same array size.   This restriction also applies to the depth-stencil view, its array size must match that of the render-target views being bound.</p><p>The pixel shader must be able to simultaneously render to at least eight separate render targets. All of these render targets must access the same type of resource: Buffer, Texture1D, Texture1DArray, Texture2D, Texture2DArray, Texture3D, or TextureCube. All render targets must have the same size in all dimensions (width and height, and depth for 3D or array size for *Array types). If render targets use multisample anti-aliasing, all bound render targets and depth buffer must be the same form of multisample resource (that is, the sample counts must be the same). Each render target can have a different data format. These render target formats are not required to have identical bit-per-element counts.</p><p>Any combination of the eight slots for render targets can have a render target set or not set.</p><p>The same resource view cannot be bound to multiple render target slots simultaneously. However, you can set multiple non-overlapping resource views of a single resource as simultaneous multiple render targets.</p></remarks>
        public void SetTargets(DepthStencilView depthStencilView, int renderTargetCount, RenderTargetView[] renderTargetViews)
        {
            SetRenderTargets(renderTargetCount, renderTargetViews, depthStencilView);
        }

        /// <summary>
        ///   Binds a depth-stencil buffer and a single render target to the output-merger stage.
        /// </summary>
        /// <param name = "depthStencilView">A view of the depth-stencil buffer to bind.</param>
        /// <param name = "renderTargetView">A view of the render target to bind.</param>
        /// <remarks>	
        /// <p>The maximum number of active render targets a device can have active at any given time is set by a #define in D3D11.h called  D3D11_SIMULTANEOUS_RENDER_TARGET_COUNT. It is invalid to try to set the same subresource to multiple render target slots.  Any render targets not defined by this call are set to <strong><c>null</c></strong>.</p><p>If any subresources are also currently bound for reading in a different stage or writing (perhaps in a different part of the pipeline),  those bind points will be set to <strong><c>null</c></strong>, in order to prevent the same subresource from being read and written simultaneously in a single rendering operation.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p><p>If the render-target views were created from an array resource type, then all of the render-target views must have the same array size.   This restriction also applies to the depth-stencil view, its array size must match that of the render-target views being bound.</p><p>The pixel shader must be able to simultaneously render to at least eight separate render targets. All of these render targets must access the same type of resource: Buffer, Texture1D, Texture1DArray, Texture2D, Texture2DArray, Texture3D, or TextureCube. All render targets must have the same size in all dimensions (width and height, and depth for 3D or array size for *Array types). If render targets use multisample anti-aliasing, all bound render targets and depth buffer must be the same form of multisample resource (that is, the sample counts must be the same). Each render target can have a different data format. These render target formats are not required to have identical bit-per-element counts.</p><p>Any combination of the eight slots for render targets can have a render target set or not set.</p><p>The same resource view cannot be bound to multiple render target slots simultaneously. However, you can set multiple non-overlapping resource views of a single resource as simultaneous multiple render targets.</p>	
        /// </remarks>	
        /// <msdn-id>ff476464</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargets([In] unsigned int NumViews,[In] const void** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargets</unmanaged-short>	
        public void SetTargets(DepthStencilView depthStencilView, RenderTargetView renderTargetView)
        {
            IntPtr targetPtr = renderTargetView == null ? IntPtr.Zero : renderTargetView.NativePointer;
            unsafe
            {
                // Optimized version for one render target
                SetRenderTargets(1, new IntPtr(&targetPtr), depthStencilView);
            }
        }

        /// <summary>
        ///   Binds a depth-stencil buffer and a set of render targets to the output-merger stage.
        /// </summary>
        /// <param name = "depthStencilView">A view of the depth-stencil buffer to bind.</param>
        /// <param name = "renderTargetViews">A set of render target views to bind.</param>
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargets([In] unsigned int NumViews,[In, Buffer, Optional] const ID3D11RenderTargetView** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView)</unmanaged>	
        /// <remarks>	
        /// <p>The maximum number of active render targets a device can have active at any given time is set by a #define in D3D11.h called  D3D11_SIMULTANEOUS_RENDER_TARGET_COUNT. It is invalid to try to set the same subresource to multiple render target slots.  Any render targets not defined by this call are set to <strong><c>null</c></strong>.</p><p>If any subresources are also currently bound for reading in a different stage or writing (perhaps in a different part of the pipeline),  those bind points will be set to <strong><c>null</c></strong>, in order to prevent the same subresource from being read and written simultaneously in a single rendering operation.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p><p>If the render-target views were created from an array resource type, then all of the render-target views must have the same array size.   This restriction also applies to the depth-stencil view, its array size must match that of the render-target views being bound.</p><p>The pixel shader must be able to simultaneously render to at least eight separate render targets. All of these render targets must access the same type of resource: Buffer, Texture1D, Texture1DArray, Texture2D, Texture2DArray, Texture3D, or TextureCube. All render targets must have the same size in all dimensions (width and height, and depth for 3D or array size for *Array types). If render targets use multisample anti-aliasing, all bound render targets and depth buffer must be the same form of multisample resource (that is, the sample counts must be the same). Each render target can have a different data format. These render target formats are not required to have identical bit-per-element counts.</p><p>Any combination of the eight slots for render targets can have a render target set or not set.</p><p>The same resource view cannot be bound to multiple render target slots simultaneously. However, you can set multiple non-overlapping resource views of a single resource as simultaneous multiple render targets.</p>	
        /// </remarks>	
        /// <msdn-id>ff476464</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargets([In] unsigned int NumViews,[In] const void** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargets</unmanaged-short>	
        public void SetTargets(SharpDX.Direct3D11.DepthStencilView depthStencilView, SharpDX.ComArray<SharpDX.Direct3D11.RenderTargetView> renderTargetViews)
        {
            SetRenderTargets(renderTargetViews == null ? 0 : renderTargetViews.Length, (renderTargetViews == null) ? IntPtr.Zero : renderTargetViews.NativePointer, depthStencilView);
        }

        /// <summary>
        ///   Binds a set of render targets to the output-merger stage and clear the depth stencil view.
        /// </summary>
        /// <param name = "renderTargetViews">A set of render target views to bind.</param>
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargets([In] unsigned int NumViews,[In, Buffer, Optional] const ID3D11RenderTargetView** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView)</unmanaged>	
        /// <remarks>	
        /// <p>The maximum number of active render targets a device can have active at any given time is set by a #define in D3D11.h called  D3D11_SIMULTANEOUS_RENDER_TARGET_COUNT. It is invalid to try to set the same subresource to multiple render target slots.  Any render targets not defined by this call are set to <strong><c>null</c></strong>.</p><p>If any subresources are also currently bound for reading in a different stage or writing (perhaps in a different part of the pipeline),  those bind points will be set to <strong><c>null</c></strong>, in order to prevent the same subresource from being read and written simultaneously in a single rendering operation.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p><p>If the render-target views were created from an array resource type, then all of the render-target views must have the same array size.   This restriction also applies to the depth-stencil view, its array size must match that of the render-target views being bound.</p><p>The pixel shader must be able to simultaneously render to at least eight separate render targets. All of these render targets must access the same type of resource: Buffer, Texture1D, Texture1DArray, Texture2D, Texture2DArray, Texture3D, or TextureCube. All render targets must have the same size in all dimensions (width and height, and depth for 3D or array size for *Array types). If render targets use multisample anti-aliasing, all bound render targets and depth buffer must be the same form of multisample resource (that is, the sample counts must be the same). Each render target can have a different data format. These render target formats are not required to have identical bit-per-element counts.</p><p>Any combination of the eight slots for render targets can have a render target set or not set.</p><p>The same resource view cannot be bound to multiple render target slots simultaneously. However, you can set multiple non-overlapping resource views of a single resource as simultaneous multiple render targets.</p>	
        /// </remarks>	
        /// <msdn-id>ff476464</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargets([In] unsigned int NumViews,[In] const void** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargets</unmanaged-short>	
        public void SetTargets(SharpDX.ComArray<SharpDX.Direct3D11.RenderTargetView> renderTargetViews)
        {
            SetRenderTargets(renderTargetViews == null ? 0 : renderTargetViews.Length, (renderTargetViews == null) ? IntPtr.Zero : renderTargetViews.NativePointer, null);
        }

        /// <summary>
        ///   Binds a set of unordered access views and a single render target to the output-merger stage.
        /// </summary>
        /// <param name = "startSlot">Index into a zero-based array to begin setting unordered access views.</param>
        /// <param name = "unorderedAccessViews">A set of unordered access views to bind.</param>
        /// <param name = "renderTargetView">A view of the render target to bind.</param>
        /// <msdn-id>ff476465</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews([In] unsigned int NumRTVs,[In, Buffer, Optional] const ID3D11RenderTargetView** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView,[In] unsigned int UAVStartSlot,[In] unsigned int NumUAVs,[In, Buffer, Optional] const ID3D11UnorderedAccessView** ppUnorderedAccessViews,[In, Buffer, Optional] const unsigned int* pUAVInitialCounts)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews</unmanaged-short>	
        public void SetTargets(
            RenderTargetView renderTargetView,
            int startSlot,
            UnorderedAccessView[] unorderedAccessViews)
        {
            SetTargets(startSlot, unorderedAccessViews, renderTargetView);
        }

        /// <summary>
        ///   Binds a set of unordered access views and a set of render targets to the output-merger stage.
        /// </summary>
        /// <param name = "startSlot">Index into a zero-based array to begin setting unordered access views.</param>
        /// <param name = "unorderedAccessViews">A set of unordered access views to bind.</param>
        /// <param name = "renderTargetViews">A set of render target views to bind.</param>
        /// <msdn-id>ff476465</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews([In] unsigned int NumRTVs,[In, Buffer, Optional] const ID3D11RenderTargetView** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView,[In] unsigned int UAVStartSlot,[In] unsigned int NumUAVs,[In, Buffer, Optional] const ID3D11UnorderedAccessView** ppUnorderedAccessViews,[In, Buffer, Optional] const unsigned int* pUAVInitialCounts)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews</unmanaged-short>	
        public void SetTargets(
            int startSlot,
            UnorderedAccessView[] unorderedAccessViews,
            params RenderTargetView[] renderTargetViews)
        {
            SetTargets(null, startSlot, unorderedAccessViews, renderTargetViews);
        }

        /// <summary>
        ///   Binds a depth-stencil buffer, a set of unordered access views, and a single render target to the output-merger stage.
        /// </summary>
        /// <param name = "depthStencilView">A view of the depth-stencil buffer to bind.</param>
        /// <param name = "startSlot">Index into a zero-based array to begin setting unordered access views.</param>
        /// <param name = "unorderedAccessViews">A set of unordered access views to bind.</param>
        /// <param name = "renderTargetView">A view of the render target to bind.</param>
        /// <msdn-id>ff476465</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews([In] unsigned int NumRTVs,[In, Buffer, Optional] const ID3D11RenderTargetView** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView,[In] unsigned int UAVStartSlot,[In] unsigned int NumUAVs,[In, Buffer, Optional] const ID3D11UnorderedAccessView** ppUnorderedAccessViews,[In, Buffer, Optional] const unsigned int* pUAVInitialCounts)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews</unmanaged-short>	
        public void SetTargets(
            DepthStencilView depthStencilView,
            RenderTargetView renderTargetView,
            int startSlot,
            UnorderedAccessView[] unorderedAccessViews)
        {
            SetTargets(depthStencilView, startSlot, unorderedAccessViews, renderTargetView);
        }

        /// <summary>
        ///   Binds a depth-stencil buffer, a set of unordered access views, and a set of render targets to the output-merger stage.
        /// </summary>
        /// <param name = "depthStencilView">A view of the depth-stencil buffer to bind.</param>
        /// <param name = "startSlot">Index into a zero-based array to begin setting unordered access views.</param>
        /// <param name = "unorderedAccessViews">A set of unordered access views to bind.</param>
        /// <param name = "renderTargetViews">A set of render target views to bind.</param>
        /// <msdn-id>ff476465</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews([In] unsigned int NumRTVs,[In, Buffer, Optional] const ID3D11RenderTargetView** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView,[In] unsigned int UAVStartSlot,[In] unsigned int NumUAVs,[In, Buffer, Optional] const ID3D11UnorderedAccessView** ppUnorderedAccessViews,[In, Buffer, Optional] const unsigned int* pUAVInitialCounts)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews</unmanaged-short>	
        public void SetTargets(
            DepthStencilView depthStencilView,
            int startSlot,
            UnorderedAccessView[] unorderedAccessViews,
            params RenderTargetView[] renderTargetViews)
        {
            int[] lengths = new int[unorderedAccessViews.Length];
            for (int i = 0; i < unorderedAccessViews.Length; i++)
                lengths[i] = -1;

            SetTargets(depthStencilView, startSlot, unorderedAccessViews, lengths, renderTargetViews);
        }

        /// <summary>
        ///   Binds a set of unordered access views and a single render target to the output-merger stage.
        /// </summary>
        /// <param name = "startSlot">Index into a zero-based array to begin setting unordered access views.</param>
        /// <param name = "unorderedAccessViews">A set of unordered access views to bind.</param>
        /// <param name = "renderTargetView">A view of the render target to bind.</param>
        /// <param name = "initialLengths">An array of Append/Consume buffer offsets. A value of -1 indicates the current offset should be kept. Any other values set the hidden counter for that Appendable/Consumable UAV.</param>
        /// <msdn-id>ff476465</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews([In] unsigned int NumRTVs,[In, Buffer, Optional] const ID3D11RenderTargetView** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView,[In] unsigned int UAVStartSlot,[In] unsigned int NumUAVs,[In, Buffer, Optional] const ID3D11UnorderedAccessView** ppUnorderedAccessViews,[In, Buffer, Optional] const unsigned int* pUAVInitialCounts)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews</unmanaged-short>	
        public void SetTargets(
            RenderTargetView renderTargetView,
            int startSlot,
            UnorderedAccessView[] unorderedAccessViews,
            int[] initialLengths)
        {
            SetTargets(startSlot, unorderedAccessViews, initialLengths, renderTargetView);
        }

        /// <summary>
        ///   Binds a set of unordered access views and a set of render targets to the output-merger stage.
        /// </summary>
        /// <param name = "startSlot">Index into a zero-based array to begin setting unordered access views.</param>
        /// <param name = "unorderedAccessViews">A set of unordered access views to bind.</param>
        /// <param name = "renderTargetViews">A set of render target views to bind.</param>
        /// <param name = "initialLengths">An array of Append/Consume buffer offsets. A value of -1 indicates the current offset should be kept. Any other values set the hidden counter for that Appendable/Consumable UAV.</param>
        /// <msdn-id>ff476465</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews([In] unsigned int NumRTVs,[In, Buffer, Optional] const ID3D11RenderTargetView** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView,[In] unsigned int UAVStartSlot,[In] unsigned int NumUAVs,[In, Buffer, Optional] const ID3D11UnorderedAccessView** ppUnorderedAccessViews,[In, Buffer, Optional] const unsigned int* pUAVInitialCounts)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews</unmanaged-short>	
        public void SetTargets(
            int startSlot,
            UnorderedAccessView[] unorderedAccessViews,
            int[] initialLengths,
            params RenderTargetView[] renderTargetViews)
        {
            SetTargets(null, startSlot, unorderedAccessViews, initialLengths, renderTargetViews);
        }

        /// <summary>
        ///   Binds a depth-stencil buffer, a set of unordered access views, and a single render target to the output-merger stage.
        /// </summary>
        /// <param name = "depthStencilView">A view of the depth-stencil buffer to bind.</param>
        /// <param name = "startSlot">Index into a zero-based array to begin setting unordered access views.</param>
        /// <param name = "unorderedAccessViews">A set of unordered access views to bind.</param>
        /// <param name = "renderTargetView">A view of the render target to bind.</param>
        /// <param name = "initialLengths">An array of Append/Consume buffer offsets. A value of -1 indicates the current offset should be kept. Any other values set the hidden counter for that Appendable/Consumable UAV.</param>
        /// <msdn-id>ff476465</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews([In] unsigned int NumRTVs,[In, Buffer, Optional] const ID3D11RenderTargetView** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView,[In] unsigned int UAVStartSlot,[In] unsigned int NumUAVs,[In, Buffer, Optional] const ID3D11UnorderedAccessView** ppUnorderedAccessViews,[In, Buffer, Optional] const unsigned int* pUAVInitialCounts)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews</unmanaged-short>	
        public void SetTargets(
            DepthStencilView depthStencilView,
            RenderTargetView renderTargetView,
            int startSlot,
            UnorderedAccessView[] unorderedAccessViews,
            int[] initialLengths)
        {
            SetTargets(depthStencilView, startSlot, unorderedAccessViews, initialLengths, renderTargetView);
        }

        /// <summary>
        ///   Binds a depth-stencil buffer, a set of unordered access views, and a set of render targets to the output-merger stage.
        /// </summary>
        /// <param name = "depthStencilView">A view of the depth-stencil buffer to bind.</param>
        /// <param name = "startSlot">Index into a zero-based array to begin setting unordered access views.</param>
        /// <param name = "unorderedAccessViews">A set of unordered access views to bind.</param>
        /// <param name = "renderTargetViews">A set of render target views to bind.</param>
        /// <param name = "initialLengths">An array of Append/Consume buffer offsets. A value of -1 indicates the current offset should be kept. Any other values set the hidden counter for that Appendable/Consumable UAV.</param>
        /// <msdn-id>ff476465</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews([In] unsigned int NumRTVs,[In, Buffer, Optional] const ID3D11RenderTargetView** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView,[In] unsigned int UAVStartSlot,[In] unsigned int NumUAVs,[In, Buffer, Optional] const ID3D11UnorderedAccessView** ppUnorderedAccessViews,[In, Buffer, Optional] const unsigned int* pUAVInitialCounts)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews</unmanaged-short>	
        public void SetTargets(
            DepthStencilView depthStencilView,
            int startSlot,
            UnorderedAccessView[] unorderedAccessViews,
            int[] initialLengths,
            params RenderTargetView[] renderTargetViews)
        {
            SetRenderTargetsAndUnorderedAccessViews(
                renderTargetViews.Length,
                renderTargetViews,
                depthStencilView,
                startSlot,
                unorderedAccessViews.Length,
                unorderedAccessViews,
                initialLengths);
        }

        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargets([In] unsigned int NumViews,[In, Buffer, Optional] const ID3D11RenderTargetView** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView)</unmanaged>	
        private void SetRenderTargets(int numViews, SharpDX.Direct3D11.RenderTargetView[] renderTargetViews, SharpDX.Direct3D11.DepthStencilView depthStencilViewRef)
        {
            unsafe
            {
                IntPtr* renderTargetViewsPtr = (IntPtr*)0;
                if (numViews > 0)
                {
                    IntPtr* tempPtr = stackalloc IntPtr[numViews];
                    renderTargetViewsPtr = tempPtr;
                    for (int i = 0; i < numViews; i++)
                        renderTargetViewsPtr[i] = (renderTargetViews[i] == null) ? IntPtr.Zero : renderTargetViews[i].NativePointer;
                }
                SetRenderTargets(numViews, (IntPtr)renderTargetViewsPtr, depthStencilViewRef);
            }
        }

        /// <summary>
        ///   Binds a depth stencil view and a render target view to the output-merger stage keeping existing unordered access views bindings.
        /// </summary>
        /// <param name = "depthStencilView">A view of the depth-stencil buffer to bind.</param>
        /// <param name = "renderTargetView">A view to a render target to bind.</param>
        /// <msdn-id>ff476465</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews([In] unsigned int NumRTVs,[In, Buffer, Optional] const ID3D11RenderTargetView** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView,[In] unsigned int UAVStartSlot,[In] unsigned int NumUAVs,[In, Buffer, Optional] const ID3D11UnorderedAccessView** ppUnorderedAccessViews,[In, Buffer, Optional] const unsigned int* pUAVInitialCounts)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews</unmanaged-short>	
        public unsafe void SetRenderTargets(DepthStencilView depthStencilView, RenderTargetView renderTargetView)
        {
            var renderTargetViewPtr = IntPtr.Zero;
            if (renderTargetView != null)
            {
                renderTargetViewPtr = renderTargetView.NativePointer;
            }
            SetRenderTargetsAndKeepUAV(1, new IntPtr(&renderTargetViewPtr),  depthStencilView);
        }

        /// <summary>
        ///   Binds a render target view to the output-merger stage keeping existing unordered access views bindings.
        /// </summary>
        /// <param name = "renderTargetView">A view to a render target to bind.</param>
        /// <msdn-id>ff476465</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews([In] unsigned int NumRTVs,[In, Buffer, Optional] const ID3D11RenderTargetView** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView,[In] unsigned int UAVStartSlot,[In] unsigned int NumUAVs,[In, Buffer, Optional] const ID3D11UnorderedAccessView** ppUnorderedAccessViews,[In, Buffer, Optional] const unsigned int* pUAVInitialCounts)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews</unmanaged-short>	
        public void SetRenderTargets(RenderTargetView renderTargetView)
        {
            SetRenderTargets(null, renderTargetView);
        }

        /// <summary>
        ///   Binds a depth stencil view and a render target view to the output-merger stage keeping existing unordered access views bindings.
        /// </summary>
        /// <param name = "depthStencilView">A view of the depth-stencil buffer to bind.</param>
        /// <param name = "renderTargetViews">A set of render target views to bind.</param>
        /// <msdn-id>ff476465</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews([In] unsigned int NumRTVs,[In, Buffer, Optional] const ID3D11RenderTargetView** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView,[In] unsigned int UAVStartSlot,[In] unsigned int NumUAVs,[In, Buffer, Optional] const ID3D11UnorderedAccessView** ppUnorderedAccessViews,[In, Buffer, Optional] const unsigned int* pUAVInitialCounts)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews</unmanaged-short>	
        public unsafe void SetRenderTargets(DepthStencilView depthStencilView, params RenderTargetView[] renderTargetViews)
        {
            var renderTargetViewsPtr = (IntPtr*)0;

            int count = 0;
            if (renderTargetViews != null)
            {
                count = renderTargetViews.Length;
                IntPtr* tempPtr = stackalloc IntPtr[renderTargetViews.Length];
                renderTargetViewsPtr = tempPtr;
                for (int i = 0; i < renderTargetViews.Length; i++)
                    renderTargetViewsPtr[i] = (renderTargetViews[i] == null) ? IntPtr.Zero : renderTargetViews[i].NativePointer;
            }
            SetRenderTargetsAndKeepUAV(count, new IntPtr(renderTargetViewsPtr), depthStencilView);
        }

        /// <summary>	
        /// Sets an array of views for an unordered resource keeping existing render targets bindings.
        /// </summary>	
        /// <remarks>	
        /// </remarks>	
        /// <param name="startSlot">Index of the first element in the zero-based array to begin setting. </param>
        /// <param name="unorderedAccessView">A reference to an <see cref="SharpDX.Direct3D11.UnorderedAccessView"/> references to be set by the method. </param>
        /// <msdn-id>ff476465</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews([In] unsigned int NumRTVs,[In, Buffer, Optional] const ID3D11RenderTargetView** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView,[In] unsigned int UAVStartSlot,[In] unsigned int NumUAVs,[In, Buffer, Optional] const ID3D11UnorderedAccessView** ppUnorderedAccessViews,[In, Buffer, Optional] const unsigned int* pUAVInitialCounts)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews</unmanaged-short>	
        public void SetUnorderedAccessView(int startSlot, SharpDX.Direct3D11.UnorderedAccessView unorderedAccessView)
        {
            SetUnorderedAccessView(startSlot, unorderedAccessView, -1);
        }

        /// <summary>	
        /// Sets an array of views for an unordered resource keeping existing render targets bindings.
        /// </summary>	
        /// <remarks>	
        /// </remarks>	
        /// <param name="startSlot">Index of the first element in the zero-based array to begin setting. </param>
        /// <param name="unorderedAccessView">A reference to an <see cref="SharpDX.Direct3D11.UnorderedAccessView"/> references to be set by the method. </param>
        /// <param name="uavInitialCount">An Append/Consume buffer offsets. A value of -1 indicates the current offset should be kept.   Any other values set the hidden counter for that Appendable/Consumable UAV. uAVInitialCount is only relevant for UAVs which have the <see cref="SharpDX.Direct3D11.UnorderedAccessViewBufferFlags"/> flag,  otherwise the argument is ignored. </param>
        /// <msdn-id>ff476465</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews([In] unsigned int NumRTVs,[In, Buffer, Optional] const ID3D11RenderTargetView** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView,[In] unsigned int UAVStartSlot,[In] unsigned int NumUAVs,[In, Buffer, Optional] const ID3D11UnorderedAccessView** ppUnorderedAccessViews,[In, Buffer, Optional] const unsigned int* pUAVInitialCounts)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews</unmanaged-short>	
        public void SetUnorderedAccessView(int startSlot, SharpDX.Direct3D11.UnorderedAccessView unorderedAccessView, int uavInitialCount)
        {
            SetUnorderedAccessViews(startSlot, new[] { unorderedAccessView }, new[] { uavInitialCount });
        }

        /// <summary>	
        /// Sets an array of views for an unordered resource keeping existing render targets bindings.
        /// </summary>	
        /// <remarks>	
        /// </remarks>	
        /// <param name="startSlot">Index of the first element in the zero-based array to begin setting. </param>
        /// <param name="unorderedAccessViews">A reference to an array of <see cref="SharpDX.Direct3D11.UnorderedAccessView"/> references to be set by the method. </param>
        /// <msdn-id>ff476465</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews([In] unsigned int NumRTVs,[In, Buffer, Optional] const ID3D11RenderTargetView** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView,[In] unsigned int UAVStartSlot,[In] unsigned int NumUAVs,[In, Buffer, Optional] const ID3D11UnorderedAccessView** ppUnorderedAccessViews,[In, Buffer, Optional] const unsigned int* pUAVInitialCounts)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews</unmanaged-short>	
        public void SetUnorderedAccessViews(int startSlot, params SharpDX.Direct3D11.UnorderedAccessView[] unorderedAccessViews)
        {
            var uavInitialCounts = new int[unorderedAccessViews.Length];
            for (int i = 0; i < unorderedAccessViews.Length; i++)
                uavInitialCounts[i] = -1;
            SetUnorderedAccessViews(startSlot, unorderedAccessViews, uavInitialCounts);
        }

        /// <summary>	
        /// Sets an array of views for an unordered resource  keeping existing render targets bindings.	
        /// </summary>	
        /// <remarks>	
        /// </remarks>	
        /// <param name="startSlot">Index of the first element in the zero-based array to begin setting. </param>
        /// <param name="unorderedAccessViews">A reference to an array of <see cref="SharpDX.Direct3D11.UnorderedAccessView"/> references to be set by the method. </param>
        /// <param name="uavInitialCounts">An array of Append/Consume buffer offsets. A value of -1 indicates the current offset should be kept.   Any other values set the hidden counter for that Appendable/Consumable UAV.  pUAVInitialCounts is only relevant for UAVs which have the <see cref="SharpDX.Direct3D11.UnorderedAccessViewBufferFlags"/> flag,  otherwise the argument is ignored. </param>
        /// <msdn-id>ff476465</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews([In] unsigned int NumRTVs,[In, Buffer, Optional] const ID3D11RenderTargetView** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView,[In] unsigned int UAVStartSlot,[In] unsigned int NumUAVs,[In, Buffer, Optional] const ID3D11UnorderedAccessView** ppUnorderedAccessViews,[In, Buffer, Optional] const unsigned int* pUAVInitialCounts)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews</unmanaged-short>	
        public unsafe void SetUnorderedAccessViews(int startSlot, SharpDX.Direct3D11.UnorderedAccessView[] unorderedAccessViews, int[] uavInitialCounts)
        {
            IntPtr* unorderedAccessViewsOut_ = (IntPtr*)0;
            if (unorderedAccessViews != null)
            {
                IntPtr* unorderedAccessViewsOut__ = stackalloc IntPtr[unorderedAccessViews.Length];
                unorderedAccessViewsOut_ = unorderedAccessViewsOut__;
                for (int i = 0; i < unorderedAccessViews.Length; i++)
                    unorderedAccessViewsOut_[i] = (unorderedAccessViews[i] == null) ? IntPtr.Zero : unorderedAccessViews[i].NativePointer;
            }
            fixed (void* puav = uavInitialCounts)
            SetUnorderedAccessViewsKeepRTV(startSlot, unorderedAccessViews != null ? unorderedAccessViews.Length : 0, (IntPtr)unorderedAccessViewsOut_, (IntPtr)puav);
        }

        /// <summary>	
        /// <p>Binds resources to the output-merger stage.</p>	
        /// </summary>	
        /// <param name="numRTVs"><dd>  <p>Number of render-target views (<em>ppRenderTargetViews</em>) and depth-stencil view (<em>ppDepthStencilView</em>)  to bind. If you set <em>NumViews</em> to D3D11_KEEP_RENDER_TARGETS_AND_DEPTH_STENCIL (0xffffffff), this method does not modify the currently bound render-target views (RTVs) and also does not modify depth-stencil view (DSV).</p> </dd></param>	
        /// <param name="renderTargetViewsOut"><dd>  <p>Pointer to an array of <strong><see cref="SharpDX.Direct3D11.RenderTargetView"/></strong>s, which represent render-target views. Specify <strong><c>null</c></strong> to set none.</p> </dd></param>	
        /// <param name="depthStencilViewRef"><dd>  <p>Pointer to a <strong><see cref="SharpDX.Direct3D11.DepthStencilView"/></strong>, which represents a depth-stencil view. Specify <strong><c>null</c></strong> to set none.</p> </dd></param>	
        /// <param name="uAVStartSlot"><dd>  <p>Index into a zero-based array to begin setting unordered-access views (ranges from 0 to <see cref="SharpDX.Direct3D11.ComputeShaderStage.UnorderedAccessViewSlotCount"/> - 1).</p> <p> For the Direct3D 11.1 runtime, which is available starting with Windows Developer Preview, this value can range from 0 to D3D11_1_UAV_SLOT_COUNT - 1. D3D11_1_UAV_SLOT_COUNT is defined as 64.</p> <p>For pixel shaders, <em>UAVStartSlot</em> should be equal to the number of render-target views being bound. </p> </dd></param>	
        /// <param name="numUAVs"><dd>  <p>Number of unordered-access views (UAVs) in <em>ppUnorderedAccessView</em>. If you set <em>NumUAVs</em> to D3D11_KEEP_UNORDERED_ACCESS_VIEWS (0xffffffff), this method does not modify the currently bound unordered-access views.</p> <p>For the Direct3D 11.1 runtime, which is available starting with Windows Developer Preview, this value can range from 0 to D3D11_1_UAV_SLOT_COUNT - <em>UAVStartSlot</em>.</p> </dd></param>	
        /// <param name="unorderedAccessViewsOut"><dd>  <p>Pointer to an array of <strong><see cref="SharpDX.Direct3D11.UnorderedAccessView"/></strong>s, which represent unordered-access views.</p> </dd></param>	
        /// <param name="uAVInitialCountsRef"><dd>  <p>An array of append and consume buffer offsets. A value of -1 indicates to keep the current offset. Any other values set the hidden counter  for that appendable and consumable UAV. <em>pUAVInitialCounts</em> is  relevant only for UAVs that were created with either  <strong><see cref="SharpDX.Direct3D11.UnorderedAccessViewBufferFlags.Append"/></strong> or <strong><see cref="SharpDX.Direct3D11.UnorderedAccessViewBufferFlags.Counter"/></strong> specified  when the UAV was created; otherwise, the argument is ignored.</p> </dd></param>	
        /// <remarks>	
        /// <p>For pixel shaders, the render targets and unordered-access views share the same resource slots when being written out. This means that UAVs must be  given an offset so that they are placed in the slots after the render target views that are being bound. </p><p><strong>Note</strong>??RTVs, DSV, and UAVs cannot be set independently; they all need to be set at the same time.</p><p>Two RTVs conflict if they share a subresource (and therefore share the same resource).</p><p>Two UAVs conflict if they share a subresource (and therefore share the same resource).</p><p>An RTV conflicts with a UAV if they share a subresource or share a bind point.</p><p><strong>OMSetRenderTargetsAndUnorderedAccessViews</strong> operates properly in the following situations:</p><ol> <li> <p><em>NumViews</em> != D3D11_KEEP_RENDER_TARGETS_AND_DEPTH_STENCIL and <em>NumUAVs</em> != D3D11_KEEP_UNORDERED_ACCESS_VIEWS</p> <p>The following conditions must be true for <strong>OMSetRenderTargetsAndUnorderedAccessViews</strong> to succeed and for the runtime to pass the bind information to the driver:</p> <ul> <li><em>NumViews</em> &lt;= 8</li> <li><em>UAVStartSlot</em> &gt;= <em>NumViews</em></li> <li><em>UAVStartSlot</em> + <em>NumUAVs</em> &lt;= 8</li> <li>There must be no conflicts in the set of all <em>ppRenderTargetViews</em> and <em>ppUnorderedAccessView</em>.</li> <li><em>ppDepthStencilView</em> must match the render-target views. For more information about resource views, see Introduction to a Resource in Direct3D 11.</li> </ul> <p><strong>OMSetRenderTargetsAndUnorderedAccessViews</strong> performs the following tasks:</p> <ul> <li>Unbinds all currently bound conflicting resources (stream-output target resources (SOTargets), compute shader (CS) UAVs, shader-resource views (SRVs)).</li> <li>Binds <em>ppRenderTargetViews</em>, <em>ppDepthStencilView</em>, and <em>ppUnorderedAccessView</em>.</li> </ul> </li> <li> <p><em>NumViews</em> == D3D11_KEEP_RENDER_TARGETS_AND_DEPTH_STENCIL </p> <p>In this situation, <strong>OMSetRenderTargetsAndUnorderedAccessViews</strong> binds only UAVs. </p> <p>The following conditions must be true for <strong>OMSetRenderTargetsAndUnorderedAccessViews</strong> to succeed and for the runtime to pass the bind information to the driver:</p> <ul> <li><em>UAVStartSlot</em> + <em>NumUAVs</em> &lt;= 8</li> <li>There must be no conflicts in <em>ppUnorderedAccessView</em>.</li> </ul> <p><strong>OMSetRenderTargetsAndUnorderedAccessViews</strong> unbinds the following items:</p> <ul> <li>All RTVs in slots &gt;= <em>UAVStartSlot</em></li> <li>All RTVs that conflict with any UAVs in <em>ppUnorderedAccessView</em></li> <li>All currently bound resources (SOTargets, CS UAVs, SRVs) that conflict with <em>ppUnorderedAccessView</em></li> </ul> <p><strong>OMSetRenderTargetsAndUnorderedAccessViews</strong> binds <em>ppUnorderedAccessView</em>.</p> <p><strong>OMSetRenderTargetsAndUnorderedAccessViews</strong> ignores <em>ppDepthStencilView</em>, and the current depth-stencil view remains bound.</p> </li> <li> <p><em>NumUAVs</em> == D3D11_KEEP_UNORDERED_ACCESS_VIEWS</p> <p>In this situation, <strong>OMSetRenderTargetsAndUnorderedAccessViews</strong> binds only RTVs and DSV. </p> <p>The following conditions must be true for <strong>OMSetRenderTargetsAndUnorderedAccessViews</strong> to succeed and for the runtime to pass the bind information to the driver:</p> <ul> <li><em>NumViews</em> &lt;= 8</li> <li>There must be no conflicts in <em>ppRenderTargetViews</em>.</li> <li><em>ppDepthStencilView</em> must match the render-target views. For more information about resource views, see Introduction to a Resource in Direct3D 11.</li> </ul> <p><strong>OMSetRenderTargetsAndUnorderedAccessViews</strong> unbinds the following items:</p> <ul> <li>All UAVs in slots &lt; <em>NumViews</em></li> <li>All UAVs that conflict with any RTVs in <em>ppRenderTargetViews</em></li> <li>All currently bound resources (SOTargets, CS UAVs, SRVs) that conflict with <em>ppRenderTargetViews</em></li> </ul> <p><strong>OMSetRenderTargetsAndUnorderedAccessViews</strong> binds <em>ppRenderTargetViews</em> and <em>ppDepthStencilView</em>.</p> <p><strong>OMSetRenderTargetsAndUnorderedAccessViews</strong> ignores <em>UAVStartSlot</em>.</p> </li> </ol>	
        /// </remarks>	
        /// <msdn-id>ff476465</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews([In] unsigned int NumRTVs,[In, Buffer, Optional] const ID3D11RenderTargetView** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView,[In] unsigned int UAVStartSlot,[In] unsigned int NumUAVs,[In, Buffer, Optional] const ID3D11UnorderedAccessView** ppUnorderedAccessViews,[In, Buffer, Optional] const unsigned int* pUAVInitialCounts)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews</unmanaged-short>	
        internal void SetRenderTargetsAndUnorderedAccessViews(int numRTVs, SharpDX.Direct3D11.RenderTargetView[] renderTargetViewsOut, SharpDX.Direct3D11.DepthStencilView depthStencilViewRef, int uAVStartSlot, int numUAVs, SharpDX.Direct3D11.UnorderedAccessView[] unorderedAccessViewsOut, int[] uAVInitialCountsRef)
        {
            unsafe
            {
                IntPtr* renderTargetViewsOut_ = (IntPtr*)0;
                if (renderTargetViewsOut != null)
                {
                    IntPtr* renderTargetViewsOut__ = stackalloc IntPtr[renderTargetViewsOut.Length];
                    renderTargetViewsOut_ = renderTargetViewsOut__;
                    for (int i = 0; i < renderTargetViewsOut.Length; i++)
                        renderTargetViewsOut_[i] = (renderTargetViewsOut[i] == null) ? IntPtr.Zero : renderTargetViewsOut[i].NativePointer;
                }
                IntPtr* unorderedAccessViewsOut_ = (IntPtr*)0;
                if (unorderedAccessViewsOut != null)
                {
                    IntPtr* unorderedAccessViewsOut__ = stackalloc IntPtr[unorderedAccessViewsOut.Length];
                    unorderedAccessViewsOut_ = unorderedAccessViewsOut__;
                    for (int i = 0; i < unorderedAccessViewsOut.Length; i++)
                        unorderedAccessViewsOut_[i] = (unorderedAccessViewsOut[i] == null) ? IntPtr.Zero : unorderedAccessViewsOut[i].NativePointer;
                }

                fixed (void* puav = uAVInitialCountsRef)
                SetRenderTargetsAndUnorderedAccessViews(numRTVs, (IntPtr) renderTargetViewsOut_, depthStencilViewRef, uAVStartSlot, numUAVs, (IntPtr) unorderedAccessViewsOut_, (IntPtr)puav);
            }
        }

        /// <summary>	
        /// <p>Binds resources to the output-merger stage.</p>	
        /// </summary>	
        /// <param name="numRTVs"><dd>  <p>Number of render-target views (<em>ppRenderTargetViews</em>) and depth-stencil view (<em>ppDepthStencilView</em>)  to bind. If you set <em>NumViews</em> to D3D11_KEEP_RENDER_TARGETS_AND_DEPTH_STENCIL (0xffffffff), this method does not modify the currently bound render-target views (RTVs) and also does not modify depth-stencil view (DSV).</p> </dd></param>	
        /// <param name="renderTargetViewsOut"><dd>  <p>Pointer to an array of <strong><see cref="SharpDX.Direct3D11.RenderTargetView"/></strong>s, which represent render-target views. Specify <strong><c>null</c></strong> to set none.</p> </dd></param>	
        /// <param name="depthStencilViewRef"><dd>  <p>Pointer to a <strong><see cref="SharpDX.Direct3D11.DepthStencilView"/></strong>, which represents a depth-stencil view. Specify <strong><c>null</c></strong> to set none.</p> </dd></param>	
        /// <param name="uAVStartSlot"><dd>  <p>Index into a zero-based array to begin setting unordered-access views (ranges from 0 to <see cref="SharpDX.Direct3D11.ComputeShaderStage.UnorderedAccessViewSlotCount"/> - 1).</p> <p> For the Direct3D 11.1 runtime, which is available starting with Windows Developer Preview, this value can range from 0 to D3D11_1_UAV_SLOT_COUNT - 1. D3D11_1_UAV_SLOT_COUNT is defined as 64.</p> <p>For pixel shaders, <em>UAVStartSlot</em> should be equal to the number of render-target views being bound. </p> </dd></param>	
        /// <param name="numUAVs"><dd>  <p>Number of unordered-access views (UAVs) in <em>ppUnorderedAccessView</em>. If you set <em>NumUAVs</em> to D3D11_KEEP_UNORDERED_ACCESS_VIEWS (0xffffffff), this method does not modify the currently bound unordered-access views.</p> <p>For the Direct3D 11.1 runtime, which is available starting with Windows Developer Preview, this value can range from 0 to D3D11_1_UAV_SLOT_COUNT - <em>UAVStartSlot</em>.</p> </dd></param>	
        /// <param name="unorderedAccessViewsOut"><dd>  <p>Pointer to an array of <strong><see cref="SharpDX.Direct3D11.UnorderedAccessView"/></strong>s, which represent unordered-access views.</p> </dd></param>	
        /// <param name="uAVInitialCountsRef"><dd>  <p>An array of append and consume buffer offsets. A value of -1 indicates to keep the current offset. Any other values set the hidden counter  for that appendable and consumable UAV. <em>pUAVInitialCounts</em> is  relevant only for UAVs that were created with either  <strong><see cref="SharpDX.Direct3D11.UnorderedAccessViewBufferFlags.Append"/></strong> or <strong><see cref="SharpDX.Direct3D11.UnorderedAccessViewBufferFlags.Counter"/></strong> specified  when the UAV was created; otherwise, the argument is ignored.</p> </dd></param>	
        /// <remarks>	
        /// <p>For pixel shaders, the render targets and unordered-access views share the same resource slots when being written out. This means that UAVs must be  given an offset so that they are placed in the slots after the render target views that are being bound. </p><p><strong>Note</strong>??RTVs, DSV, and UAVs cannot be set independently; they all need to be set at the same time.</p><p>Two RTVs conflict if they share a subresource (and therefore share the same resource).</p><p>Two UAVs conflict if they share a subresource (and therefore share the same resource).</p><p>An RTV conflicts with a UAV if they share a subresource or share a bind point.</p><p><strong>OMSetRenderTargetsAndUnorderedAccessViews</strong> operates properly in the following situations:</p><ol> <li> <p><em>NumViews</em> != D3D11_KEEP_RENDER_TARGETS_AND_DEPTH_STENCIL and <em>NumUAVs</em> != D3D11_KEEP_UNORDERED_ACCESS_VIEWS</p> <p>The following conditions must be true for <strong>OMSetRenderTargetsAndUnorderedAccessViews</strong> to succeed and for the runtime to pass the bind information to the driver:</p> <ul> <li><em>NumViews</em> &lt;= 8</li> <li><em>UAVStartSlot</em> &gt;= <em>NumViews</em></li> <li><em>UAVStartSlot</em> + <em>NumUAVs</em> &lt;= 8</li> <li>There must be no conflicts in the set of all <em>ppRenderTargetViews</em> and <em>ppUnorderedAccessView</em>.</li> <li><em>ppDepthStencilView</em> must match the render-target views. For more information about resource views, see Introduction to a Resource in Direct3D 11.</li> </ul> <p><strong>OMSetRenderTargetsAndUnorderedAccessViews</strong> performs the following tasks:</p> <ul> <li>Unbinds all currently bound conflicting resources (stream-output target resources (SOTargets), compute shader (CS) UAVs, shader-resource views (SRVs)).</li> <li>Binds <em>ppRenderTargetViews</em>, <em>ppDepthStencilView</em>, and <em>ppUnorderedAccessView</em>.</li> </ul> </li> <li> <p><em>NumViews</em> == D3D11_KEEP_RENDER_TARGETS_AND_DEPTH_STENCIL </p> <p>In this situation, <strong>OMSetRenderTargetsAndUnorderedAccessViews</strong> binds only UAVs. </p> <p>The following conditions must be true for <strong>OMSetRenderTargetsAndUnorderedAccessViews</strong> to succeed and for the runtime to pass the bind information to the driver:</p> <ul> <li><em>UAVStartSlot</em> + <em>NumUAVs</em> &lt;= 8</li> <li>There must be no conflicts in <em>ppUnorderedAccessView</em>.</li> </ul> <p><strong>OMSetRenderTargetsAndUnorderedAccessViews</strong> unbinds the following items:</p> <ul> <li>All RTVs in slots &gt;= <em>UAVStartSlot</em></li> <li>All RTVs that conflict with any UAVs in <em>ppUnorderedAccessView</em></li> <li>All currently bound resources (SOTargets, CS UAVs, SRVs) that conflict with <em>ppUnorderedAccessView</em></li> </ul> <p><strong>OMSetRenderTargetsAndUnorderedAccessViews</strong> binds <em>ppUnorderedAccessView</em>.</p> <p><strong>OMSetRenderTargetsAndUnorderedAccessViews</strong> ignores <em>ppDepthStencilView</em>, and the current depth-stencil view remains bound.</p> </li> <li> <p><em>NumUAVs</em> == D3D11_KEEP_UNORDERED_ACCESS_VIEWS</p> <p>In this situation, <strong>OMSetRenderTargetsAndUnorderedAccessViews</strong> binds only RTVs and DSV. </p> <p>The following conditions must be true for <strong>OMSetRenderTargetsAndUnorderedAccessViews</strong> to succeed and for the runtime to pass the bind information to the driver:</p> <ul> <li><em>NumViews</em> &lt;= 8</li> <li>There must be no conflicts in <em>ppRenderTargetViews</em>.</li> <li><em>ppDepthStencilView</em> must match the render-target views. For more information about resource views, see Introduction to a Resource in Direct3D 11.</li> </ul> <p><strong>OMSetRenderTargetsAndUnorderedAccessViews</strong> unbinds the following items:</p> <ul> <li>All UAVs in slots &lt; <em>NumViews</em></li> <li>All UAVs that conflict with any RTVs in <em>ppRenderTargetViews</em></li> <li>All currently bound resources (SOTargets, CS UAVs, SRVs) that conflict with <em>ppRenderTargetViews</em></li> </ul> <p><strong>OMSetRenderTargetsAndUnorderedAccessViews</strong> binds <em>ppRenderTargetViews</em> and <em>ppDepthStencilView</em>.</p> <p><strong>OMSetRenderTargetsAndUnorderedAccessViews</strong> ignores <em>UAVStartSlot</em>.</p> </li> </ol>	
        /// </remarks>	
        /// <msdn-id>ff476465</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews([In] unsigned int NumRTVs,[In, Buffer, Optional] const ID3D11RenderTargetView** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView,[In] unsigned int UAVStartSlot,[In] unsigned int NumUAVs,[In, Buffer, Optional] const ID3D11UnorderedAccessView** ppUnorderedAccessViews,[In, Buffer, Optional] const unsigned int* pUAVInitialCounts)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargetsAndUnorderedAccessViews</unmanaged-short>	
        internal unsafe void SetRenderTargetsAndUnorderedAccessViews(int numRTVs, SharpDX.ComArray<SharpDX.Direct3D11.RenderTargetView> renderTargetViewsOut, SharpDX.Direct3D11.DepthStencilView depthStencilViewRef, int uAVStartSlot, int numUAVs, SharpDX.ComArray<SharpDX.Direct3D11.UnorderedAccessView> unorderedAccessViewsOut, int[] uAVInitialCountsRef)
        {
            fixed (void* puav = uAVInitialCountsRef)
                SetRenderTargetsAndUnorderedAccessViews(numRTVs, ((renderTargetViewsOut == null) ? IntPtr.Zero : renderTargetViewsOut.NativePointer), depthStencilViewRef, uAVStartSlot, numUAVs, ((unorderedAccessViewsOut == null) ? IntPtr.Zero : unorderedAccessViewsOut.NativePointer), (IntPtr)puav);
        }

        internal void SetRenderTargetsAndKeepUAV(int numRTVs, IntPtr rtvs, SharpDX.Direct3D11.DepthStencilView depthStencilViewRef)
        {
            const int D3D11_KEEP_UNORDERED_ACCESS_VIEWS = -1;
            SetRenderTargetsAndUnorderedAccessViews(numRTVs, rtvs, depthStencilViewRef, 0, D3D11_KEEP_UNORDERED_ACCESS_VIEWS, IntPtr.Zero, IntPtr.Zero);
        }

        internal void SetUnorderedAccessViewsKeepRTV(int startSlot, int numBuffers, IntPtr unorderedAccessBuffer, IntPtr uavCount)
        {
            const int D3D11_KEEP_RENDER_TARGETS_AND_DEPTH_STENCIL = -1;
            SetRenderTargetsAndUnorderedAccessViews(D3D11_KEEP_RENDER_TARGETS_AND_DEPTH_STENCIL, IntPtr.Zero, null, startSlot, numBuffers, unorderedAccessBuffer, uavCount);
        }
       
        /// <summary>	
        /// Set the blend state of the output-merger stage.	
        /// </summary>	
        /// <param name="blendStateRef"><para>Pointer to a blend-state interface (see <see cref="SharpDX.Direct3D11.BlendState"/>). Passing in <c>null</c> implies a default blend state. See remarks for further details.</para></param>	
        /// <param name="blendFactor"><para>Array of blend factors, one for each RGBA component. This requires a blend state object that specifies the <see cref="SharpDX.Direct3D11.BlendOption.BlendFactor"/> option.</para></param>	
        /// <param name="sampleMask"><para>32-bit sample coverage. The default value is 0xffffffff. See remarks.</para></param>	
        /// <remarks>	
        /// Blend state is used by the output-merger stage to determine how to blend together two pixel values. The two values are commonly the current pixel value and the pixel value already in the output render target. Use the blend operation to control where the two pixel values come from and how they are mathematically combined.To create a blend-state interface, call <see cref="SharpDX.Direct3D11.Device.CreateBlendState"/>.Passing in <c>null</c> for the blend-state interface indicates to the runtime to set a default blending state.  The following table indicates the default blending parameters.StateDefault Value AlphaToCoverageEnableFALSE BlendEnableFALSE[8] SrcBlendD3D11_BLEND_ONE DstBlendD3D11_BLEND_ZERO BlendOpD3D11_BLEND_OP_ADD SrcBlendAlphaD3D11_BLEND_ONE DstBlendAlphaD3D11_BLEND_ZERO BlendOpAlphaD3D11_BLEND_OP_ADD RenderTargetWriteMask[8]<see cref="SharpDX.Direct3D11.ColorWriteMaskFlags.All"/>[8]?A sample mask determines which samples get updated in all the active render targets. The mapping of bits in a sample mask to samples in a multisample render target is the responsibility of an individual application. A sample mask is always applied; it is independent of whether multisampling is enabled, and does not depend on whether an application uses multisample render targets.The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10.	
        /// </remarks>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetBlendState([In, Optional] ID3D11BlendState* pBlendState,[In, Optional] const SHARPDX_COLOR4* BlendFactor,[In] unsigned int SampleMask)</unmanaged>	
        public void SetBlendState(SharpDX.Direct3D11.BlendState blendStateRef, RawColor4? blendFactor, uint sampleMask)
        {
            SetBlendState(blendStateRef, blendFactor, unchecked((int)sampleMask));
        }

        /// <summary>
        /// Gets or sets the blend factor.
        /// </summary>
        /// <value>The blend factor.</value>
        public RawColor4 BlendFactor
        {
            get
            {
                BlendState state;
                RawColor4 blendFactor;
                int sampleMaskRef;
                GetBlendState(out state, out blendFactor, out sampleMaskRef);
                if (state != null)
                    state.Dispose();
                return blendFactor;

            }
            set
            {
                BlendState state;
                RawColor4 blendFactor;
                int sampleMaskRef;
                GetBlendState(out state, out blendFactor, out sampleMaskRef);
                SetBlendState(state, value, sampleMaskRef);
                if (state != null)
                    state.Dispose();
            }
        }

        /// <summary>
        /// Gets or sets the blend sample mask.
        /// </summary>
        /// <value>The blend sample mask.</value>
        public int BlendSampleMask
        {
            get
            {
                BlendState state;
                RawColor4 blendFactor;
                int sampleMaskRef;
                GetBlendState(out state, out blendFactor, out sampleMaskRef);
                if (state != null)
                    state.Dispose();
                return sampleMaskRef;
            }
            set
            {
                BlendState state;
                RawColor4 blendFactor;
                int sampleMaskRef;
                GetBlendState(out state, out blendFactor, out sampleMaskRef);
                SetBlendState(state, blendFactor, value);
                if (state != null)
                    state.Dispose();
            }
        }

        /// <summary>
        /// Gets or sets the state of the blend.
        /// </summary>
        /// <value>The state of the blend.</value>
        public BlendState BlendState
        {
            get
            {
                BlendState state;
                RawColor4 blendFactor;
                int sampleMaskRef;
                GetBlendState(out state, out blendFactor, out sampleMaskRef);
                return state;
            }
            set
            {
                BlendState state;
                RawColor4 blendFactor;
                int sampleMaskRef;
                GetBlendState(out state, out blendFactor, out sampleMaskRef);
                if (state != null)
                    state.Dispose();
                SetBlendState(value, blendFactor, sampleMaskRef);
            }
        }

        /// <summary>
        /// Gets or sets the depth stencil reference.
        /// </summary>
        /// <value>The depth stencil reference.</value>
        public int DepthStencilReference
        {
            get
            {
                DepthStencilState state;
                int reference;
                GetDepthStencilState(out state, out reference);
                if (state != null)
                    state.Dispose();
                return reference;
            }
            set
            {
                DepthStencilState state;
                int reference;
                GetDepthStencilState(out state, out reference);
                SetDepthStencilState(state, value);
                if (state != null)
                    state.Dispose();
            }
        }

        /// <summary>
        /// Gets or sets the state of the depth stencil.
        /// </summary>
        /// <value>The state of the depth stencil.</value>
        public DepthStencilState DepthStencilState
        {
            get
            {
                DepthStencilState state;
                int reference;
                GetDepthStencilState(out state, out reference);
                return state;
            }
            set
            {
                DepthStencilState state;
                int reference;
                GetDepthStencilState(out state, out reference);
                if (state != null)
                    state.Dispose();
                SetDepthStencilState(value, reference);
            }
        }
    }
}