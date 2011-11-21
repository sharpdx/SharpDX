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
using SharpDX;

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
        public SharpDX.Direct3D11.BlendState GetBlendState(out SharpDX.Color4 blendFactor, out int sampleMaskRef)
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
        public void ResetTargets()
        {
            SetRenderTargets(0, IntPtr.Zero, null);
        }

        /// <summary>
        ///   Binds a set of render targets to the output-merger stage.
        /// </summary>
        /// <param name = "renderTargetViews">A set of render target views to bind.</param>
        public void SetTargets(params RenderTargetView[] renderTargetViews)
        {
            SetTargets((DepthStencilView)null, renderTargetViews);
        }

        /// <summary>
        ///   Binds a single render target to the output-merger stage.
        /// </summary>
        /// <param name = "renderTargetView">A view of the render target to bind.</param>
        public void SetTargets(RenderTargetView renderTargetView)
        {
            SetTargets((DepthStencilView)null, renderTargetView);
        }

        /// <summary>
        ///   Binds a depth-stencil buffer and a set of render targets to the output-merger stage.
        /// </summary>
        /// <param name = "depthStencilView">A view of the depth-stencil buffer to bind.</param>
        /// <param name = "renderTargetViews">A set of render target views to bind.</param>
        public void SetTargets(DepthStencilView depthStencilView, params RenderTargetView[] renderTargetViews)
        {
            SetRenderTargets(renderTargetViews == null ? 0 : renderTargetViews.Length, renderTargetViews, depthStencilView);
        }

        /// <summary>
        ///   Binds a depth-stencil buffer and a single render target to the output-merger stage.
        /// </summary>
        /// <param name = "depthStencilView">A view of the depth-stencil buffer to bind.</param>
        /// <param name = "renderTargetView">A view of the render target to bind.</param>
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
        public void SetTargets(SharpDX.Direct3D11.DepthStencilView depthStencilView, SharpDX.ComArray<SharpDX.Direct3D11.RenderTargetView> renderTargetViews)
        {
            SetRenderTargets(renderTargetViews == null ? 0 : renderTargetViews.Length, (renderTargetViews == null) ? IntPtr.Zero : renderTargetViews.NativePointer, depthStencilView);
        }

        /// <summary>
        ///   Binds a set of render targets to the output-merger stage and clear the depth stencil view.
        /// </summary>
        /// <param name = "renderTargetViews">A set of render target views to bind.</param>
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargets([In] unsigned int NumViews,[In, Buffer, Optional] const ID3D11RenderTargetView** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView)</unmanaged>	
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
        /// <param name = "initialLengths">An array of Append/Consume buffer offsets. A value of -1 indicates the current offset should be kept. Any other values set the hidden counter for that Appendable/Consumeable UAV.</param>
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
        /// <param name = "initialLengths">An array of Append/Consume buffer offsets. A value of -1 indicates the current offset should be kept. Any other values set the hidden counter for that Appendable/Consumeable UAV.</param>
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
        /// <param name = "initialLengths">An array of Append/Consume buffer offsets. A value of -1 indicates the current offset should be kept. Any other values set the hidden counter for that Appendable/Consumeable UAV.</param>
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
        /// <param name = "initialLengths">An array of Append/Consume buffer offsets. A value of -1 indicates the current offset should be kept. Any other values set the hidden counter for that Appendable/Consumeable UAV.</param>
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
        /// Gets or sets the blend factor.
        /// </summary>
        /// <value>The blend factor.</value>
        public Color4 BlendFactor
        {
            get
            {
                BlendState state;
                Color4 blendFactor;
                int sampleMaskRef;
                GetBlendState(out state, out blendFactor, out sampleMaskRef);
                if (state != null)
                    state.Dispose();
                return blendFactor;

            }
            set
            {
                BlendState state;
                Color4 blendFactor;
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
                Color4 blendFactor;
                int sampleMaskRef;
                GetBlendState(out state, out blendFactor, out sampleMaskRef);
                if (state != null)
                    state.Dispose();
                return sampleMaskRef;
            }
            set
            {
                BlendState state;
                Color4 blendFactor;
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
                Color4 blendFactor;
                int sampleMaskRef;
                GetBlendState(out state, out blendFactor, out sampleMaskRef);
                return state;
            }
            set
            {
                BlendState state;
                Color4 blendFactor;
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