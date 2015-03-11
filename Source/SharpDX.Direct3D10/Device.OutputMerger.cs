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

namespace SharpDX.Direct3D10
{
    public partial class OutputMergerStage
    {
        /// <summary>	
        /// Get references to the render targets that are available to the {{output-merger stage}}.	
        /// </summary>	
        /// <remarks>	
        /// Any returned interfaces will have their reference count incremented by one. Applications should call {{IUnknown::Release}} on the returned interfaces when they are no longer needed to avoid memory leaks. 	
        /// </remarks>	
        /// <returns>a depth-stencil view (see <see cref="SharpDX.Direct3D10.DepthStencilView"/>) to be filled with the depth-stencil information from the device.</returns>
        /// <unmanaged>void OMGetRenderTargets([In] int NumViews,[Out, Buffer, Optional] ID3D10RenderTargetView** ppRenderTargetViews,[Out, Optional] ID3D10DepthStencilView** ppDepthStencilView)</unmanaged>
        public void GetRenderTargets(out SharpDX.Direct3D10.DepthStencilView depthStencilViewRef)
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
        /// <returns>an array of render targets views (see <see cref="SharpDX.Direct3D10.RenderTargetView"/>) to be filled with the render targets from the device.</returns>
        /// <unmanaged>void OMGetRenderTargets([In] int NumViews,[Out, Buffer, Optional] ID3D10RenderTargetView** ppRenderTargetViews,[Out, Optional] ID3D10DepthStencilView** ppDepthStencilView)</unmanaged>
        public SharpDX.Direct3D10.RenderTargetView[] GetRenderTargets(int numViews)
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
        /// <param name="depthStencilViewRef">Pointer to a depth-stencil view (see <see cref="SharpDX.Direct3D10.DepthStencilView"/>) to be filled with the depth-stencil information from the device.</param>
        /// <returns>an array of render targets views (see <see cref="SharpDX.Direct3D10.RenderTargetView"/>) to be filled with the render targets from the device.</returns>
        /// <unmanaged>void OMGetRenderTargets([In] int NumViews,[Out, Buffer, Optional] ID3D10RenderTargetView** ppRenderTargetViews,[Out, Optional] ID3D10DepthStencilView** ppDepthStencilView)</unmanaged>
        public SharpDX.Direct3D10.RenderTargetView[] GetRenderTargets(int numViews, out SharpDX.Direct3D10.DepthStencilView depthStencilViewRef)
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
        /// <returns>a reference to a blend-state interface (see <see cref="SharpDX.Direct3D10.BlendState"/>).</returns>
        /// <unmanaged>void OMGetBlendState([Out, Optional] ID3D10BlendState** ppBlendState,[Out, Optional] float BlendFactor[4],[Out, Optional] int* pSampleMask)</unmanaged>
        public SharpDX.Direct3D10.BlendState GetBlendState(out RawColor4 blendFactor, out int sampleMaskRef)
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
        /// <returns>a reference to a depth-stencil state interface (see <see cref="SharpDX.Direct3D10.DepthStencilState"/>) to be filled with information from the device.</returns>
        /// <unmanaged>void OMGetDepthStencilState([Out, Optional] ID3D10DepthStencilState** ppDepthStencilState,[Out, Optional] int* pStencilRef)</unmanaged>
        public SharpDX.Direct3D10.DepthStencilState GetDepthStencilState(out int stencilRefRef)
        {
            DepthStencilState temp;
            GetDepthStencilState(out temp, out stencilRefRef);
            return temp;
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
            SetRenderTargets(renderTargetViews.Length, renderTargetViews, depthStencilView);
        }

        /// <summary>
        ///   Binds a depth-stencil buffer and a single render target to the output-merger stage.
        /// </summary>
        /// <param name = "depthStencilView">A view of the depth-stencil buffer to bind.</param>
        /// <param name = "renderTargetView">A view of the render target to bind.</param>
        public void SetTargets(DepthStencilView depthStencilView, RenderTargetView renderTargetView)
        {
            SetTargets(depthStencilView, new[] { renderTargetView });
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