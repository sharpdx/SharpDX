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
using System.Collections.Generic;
using System.Text;

namespace SharpDX.Direct2D1
{
    /// <summary>
    /// Custom Effect interface. Equivalent of C++ ID2D1EffectImpl.
    /// </summary>
    /// <unmanaged>ID2D1EffectImpl</unmanaged>	
    [ShadowAttribute(typeof(CustomEffectShadow))]
    public partial interface CustomEffect
    {
        /// <summary>	
        /// Creates any resources used repeatedly during subsequent rendering calls.
        /// </summary>	
        /// <param name="effectContext"><para>An internal factory interface that creates and returns effect author centric types.</para></param>	
        /// <param name="transformGraph">No documentation.</param>	
        /// <remarks>	
        /// This moves resource creation cost to the CreateEffect call, rather than during rendering.If the implementation fails this call, the corresponding <see cref="SharpDX.Direct2D1.DeviceContext.CreateEffect"/> call also fails.The following example shows an effect implementing an initialize method.	
        /// </remarks>	
        /// <unmanaged>HRESULT ID2D1EffectImpl::Initialize([In] ID2D1EffectContext* effectContext,[In] ID2D1TransformGraph* transformGraph)</unmanaged>	
        void Initialize(SharpDX.Direct2D1.EffectContext effectContext, SharpDX.Direct2D1.TransformGraph transformGraph);

        /// <summary>	
        /// Prepares an effect for the rendering process.	
        /// </summary>	
        /// <param name="changeType"><para>Indicates the type of change the effect should expect.</para></param>	
        /// <remarks>	
        /// This method is called by the renderer when the effect is within an effect graph that is drawn.The method will be called:If the effect has been initialized but has not previously been drawn. If an effect property has been set since the last draw call. If the context state has changed since the effect was last drawn.The method will not otherwise be called. The transforms created by the effect will be called to handle their input and output rectangles for every draw call.Most effects defer creating any resources or specifying a topology until this call is made. They store their properties and map them to a concrete set of rendering techniques when first drawn.	
        /// </remarks>	
        /// <unmanaged>HRESULT ID2D1EffectImpl::PrepareForRender([In] D2D1_CHANGE_TYPE changeType)</unmanaged>	
        void PrepareForRender(SharpDX.Direct2D1.ChangeType changeType);

        /// <summary>	
        /// The renderer calls this method to provide the effect implementation with a way to specify its transform graph and transform graph changes. 
        /// The renderer calls this method when: 1) When the effect is first initialized. 2) If the number of inputs to the effect changes.
        /// </summary>	
        /// <param name="transformGraph">The graph to which the effect describes its transform topology through the SetDescription call..</param>	
        /// <unmanaged>HRESULT ID2D1EffectImpl::SetGraph([In] ID2D1TransformGraph* transformGraph)</unmanaged>	
        void SetGraph(SharpDX.Direct2D1.TransformGraph transformGraph);
    }
}