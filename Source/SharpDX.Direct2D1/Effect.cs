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
using System.Reflection;

namespace SharpDX.Direct2D1
{
    public partial class Effect
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Effect"/> class.
        /// </summary>
        /// <param name="deviceContext">The device context.</param>
        /// <param name="effectId"><para>The class ID of the effect to create.</para></param>	
        /// <exception cref="SharpDXException">If no sufficient memory to complete the call, or if it does not have enough display memory to perform the operation, or if the specified effect is not registered by the system.</exception>
        /// <remarks>
        /// The created effect does not increment the reference count for the dynamic-link library (DLL) from which the effect was created. If the application deletes an effect while that effect is loaded, the resulting behavior will be unpredictable.	
        /// </remarks>	
        /// <unmanaged>HRESULT ID2D1DeviceContext::CreateEffect([In] const GUID&amp; effectId,[Out, Fast] ID2D1Effect** effect)</unmanaged>	
        public Effect(DeviceContext deviceContext, Guid effectId)
            : base(IntPtr.Zero)
        {
            deviceContext.CreateEffect(effectId, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Effect"/> class.
        /// </summary>
        /// <param name="effectContext">The effect context.</param>
        /// <param name="effectId"><para>The class ID of the effect to create.</para></param>	
        /// <returns>No documentation.</returns>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='ID2D1EffectContext::CreateEffect']/*"/>	
        /// <unmanaged>HRESULT ID2D1EffectContext::CreateEffect([In] const GUID&amp; effectId,[Out] ID2D1Effect** effect)</unmanaged>	
        public Effect(EffectContext effectContext, System.Guid effectId) : base(IntPtr.Zero)
        {
            effectContext.CreateEffect(effectId, this);
        }

        /// <summary>
        /// Sets the input by using the output of a given effect.
        /// </summary>
        /// <param name="index">Index of the input</param>
        /// <param name="effect">Effect output to use as input</param>
        /// <param name="invalidate">To invalidate</param>
        public void SetInputEffect(int index, Effect effect, bool invalidate = true)
        {
            using (var output = effect.Output)
                SetInput(index, output, invalidate);
        }
    }

    /// <summary>
    /// Class used to instantiate custom effects.
    /// </summary>
    /// <typeparam name="T">Type of the custom effect</typeparam>
    public partial class Effect<T> : Effect where T : CustomEffect
    {
        /// <summary>
        /// Initializes a new instance of a custom <see cref="Effect"/> class.
        /// </summary>
        /// <param name="deviceContext">The device context.</param>
        /// <exception cref="SharpDXException">If no sufficient memory to complete the call, or if it does not have enough display memory to perform the operation, or if the specified effect is not registered by the system.</exception>
        /// <remarks>
        /// The created effect does not increment the reference count for the dynamic-link library (DLL) from which the effect was created. If the application deletes an effect while that effect is loaded, the resulting behavior will be unpredictable.	
        /// </remarks>	
        /// <unmanaged>HRESULT ID2D1DeviceContext::CreateEffect([In] const GUID&amp; effectId,[Out, Fast] ID2D1Effect** effect)</unmanaged>	
        public Effect(DeviceContext deviceContext)
            : this(deviceContext, Utilities.GetGuidFromType(typeof(T)))
        {
        }

        /// <summary>
        /// Initializes a new instance of a custom <see cref="Effect"/> class.
        /// </summary>
        /// <param name="deviceContext">The device context.</param>
        /// <param name="effectId">Effect ID.</param>
        /// <exception cref="SharpDXException">If no sufficient memory to complete the call, or if it does not have enough display memory to perform the operation, or if the specified effect is not registered by the system.</exception>
        /// <remarks>
        /// The created effect does not increment the reference count for the dynamic-link library (DLL) from which the effect was created. If the application deletes an effect while that effect is loaded, the resulting behavior will be unpredictable.	
        /// </remarks>	
        /// <unmanaged>HRESULT ID2D1DeviceContext::CreateEffect([In] const GUID&amp; effectId,[Out, Fast] ID2D1Effect** effect)</unmanaged>	
        public Effect(DeviceContext deviceContext, Guid effectId)
            : base(deviceContext, effectId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Effect"/> class.
        /// </summary>
        /// <param name="effectContext">The effect context.</param>
        /// <returns>No documentation.</returns>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='ID2D1EffectContext::CreateEffect']/*"/>	
        /// <unmanaged>HRESULT ID2D1EffectContext::CreateEffect([In] const GUID&amp; effectId,[Out] ID2D1Effect** effect)</unmanaged>	
        public Effect(EffectContext effectContext)
            : base(effectContext, Utilities.GetGuidFromType(typeof(T)))
        {
        }
    }
}
