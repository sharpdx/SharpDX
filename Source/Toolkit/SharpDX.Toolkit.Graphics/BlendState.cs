// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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

using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// BlendState is equivalent to <see cref="SharpDX.Direct3D11.BlendState"/>.
    /// </summary>
    /// <remarks>
    /// This class provides default stock blend states and easier constructors. It is also associating the <see cref="BlendFactor"/> and <see cref="MultiSampleMask"/> into the same object.
    /// </remarks>
    public class BlendState : GraphicsResource
    {
        /// <summary>
        /// A built-in state object with settings for additive blend, that is adding the destination data to the source data without using alpha.
        /// </summary>
        public static readonly BlendState Additive = New("BlendState.Additive", BlendOption.SourceAlpha, BlendOption.One);

        /// <summary>
        /// A built-in state object with settings for alpha blend, that is blending the source and destination data using alpha.
        /// </summary>
        public static readonly BlendState AlphaBlend = New("BlendState.AlphaBlend", BlendOption.One, BlendOption.InverseSourceAlpha);

        /// <summary>
        /// A built-in state object with settings for blending with non-premultipled alpha, that is blending source and destination data using alpha while assuming the color data contains no alpha information.
        /// </summary>
        public static readonly BlendState NonPremultiplied = New("BlendState.NonPremultiplied", BlendOption.SourceAlpha, BlendOption.InverseSourceAlpha);

        /// <summary>
        /// A built-in state object with settings for opaque blend, that is overwriting the source with the destination data.
        /// </summary>
        public static readonly BlendState Opaque = New("BlendState.Opaque", BlendOption.One, BlendOption.Zero);

        /// <summary>
        /// A built-in default state object (no blending).
        /// </summary>
        public static readonly BlendState Default = New("Default", BlendStateDescription.Default());
        
        /// <summary>
        /// Gets the description of this blend state.
        /// </summary>
        public readonly BlendStateDescription Description;

        /// <summary>
        /// RGBA component. This requires a blend state object that specifies the <see cref="SharpDX.Direct3D11.BlendOption.BlendFactor"/>.
        /// </summary>
        public readonly Color4 BlendFactor;

        /// <summary>
        /// <p>Blend state is used by the output-merger stage to determine how to blend together two pixel values. The two values are commonly the current pixel value and the pixel value already in the output render target. Use the <strong>blend operation</strong> to control where the two pixel values come from and how they are mathematically combined.</p><p>To create a blend-state interface, call <strong><see cref="SharpDX.Direct3D11.Device.CreateBlendState"/></strong>.</p><p>Passing in <strong><c>null</c></strong> for the blend-state interface indicates to the runtime to set a default blending state.  The following table indicates the default blending parameters.</p><table> <tr><th>State</th><th>Default Value</th></tr> <tr><td>AlphaToCoverageEnable</td><td><strong><see cref="SharpDX.Result.False"/></strong></td></tr> <tr><td>BlendEnable</td><td><strong><see cref="SharpDX.Result.False"/></strong>[8]</td></tr> <tr><td>SrcBlend</td><td><see cref="SharpDX.Direct3D11.BlendOption.One"/></td></tr> <tr><td>DstBlend</td><td><see cref="SharpDX.Direct3D11.BlendOption.Zero"/></td></tr> <tr><td>BlendOp</td><td><see cref="SharpDX.Direct3D11.BlendOperation.Add"/></td></tr> <tr><td>SrcBlendAlpha</td><td><see cref="SharpDX.Direct3D11.BlendOption.One"/></td></tr> <tr><td>DstBlendAlpha</td><td><see cref="SharpDX.Direct3D11.BlendOption.Zero"/></td></tr> <tr><td>BlendOpAlpha</td><td><see cref="SharpDX.Direct3D11.BlendOperation.Add"/></td></tr> <tr><td>RenderTargetWriteMask[8]</td><td><see cref="SharpDX.Direct3D11.ColorWriteMaskFlags.All"/>[8]</td></tr> </table><p>?</p><p>A sample mask determines which samples get updated in all the active render targets. The mapping of bits in a sample mask to samples in a multisample render target is the responsibility of an individual application. A sample mask is always applied; it is independent of whether multisampling is enabled, and does not depend on whether an application uses multisample render targets.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p>	
        /// </summary>
        public readonly int MultiSampleMask;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlendState" /> class.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="blendFactor">The blend factor.</param>
        /// <param name="mask">The mask.</param>
        private BlendState(BlendStateDescription description, Color4 blendFactor, int mask)
            : this(GraphicsDevice.CurrentSafe.MainDevice, description, blendFactor, mask)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlendState" /> class.
        /// </summary>
        /// <param name="deviceLocal">The device local.</param>
        /// <param name="description">The description.</param>
        /// <param name="blendFactor">The blend factor.</param>
        /// <param name="mask">The mask.</param>
        private BlendState(GraphicsDevice deviceLocal, BlendStateDescription description, Color4 blendFactor, int mask)
        {
            Description = description;
            BlendFactor = blendFactor;
            MultiSampleMask = mask;
            Initialize(deviceLocal, null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlendState" /> class.
        /// </summary>
        /// <param name="deviceLocal">The device local.</param>
        /// <param name="nativeState">State of the native.</param>
        /// <param name="blendFactor">The blend factor.</param>
        /// <param name="mask">The mask.</param>
        private BlendState(GraphicsDevice deviceLocal, Direct3D11.BlendState nativeState, Color4 blendFactor, int mask)
        {
            Description = nativeState.Description;
            BlendFactor = blendFactor;
            MultiSampleMask = mask;
            Resource = ToDispose(nativeState);
            Initialize(deviceLocal, null);
        }

        /// <summary>
        /// <p>Create a blend-state object that encapsules blend state for the output-merger stage.</p>
        /// </summary>
        /// <param name="renderTargetBlend0">The render target blend description for the first render target.</param>
        /// <param name="blendFactor">The blend factor.</param>
        /// <param name="mask">The mask.</param>
        /// <returns>A new <see cref="BlendState" /> instance.</returns>
        /// <msdn-id>ff476500</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateBlendState([In] const D3D11_BLEND_DESC* pBlendStateDesc,[Out, Fast] ID3D11BlendState** ppBlendState)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateBlendState</unmanaged-short>
        /// <remarks><p>An application can create up to 4096 unique blend-state objects. For each object created, the runtime checks to see if a previous object  has the same state. If such a previous object exists, the runtime will return a reference to previous instance instead of creating a duplicate object.</p></remarks>
        public static BlendState New(RenderTargetBlendDescription renderTargetBlend0, Color4 blendFactor, int mask = -1)
        {
            var description = BlendStateDescription.Default();
            description.RenderTarget[0] = renderTargetBlend0;

            return new BlendState(description, blendFactor, mask);
        }

        /// <summary>
        /// <p>Create a blend-state object that encapsules blend state for the output-merger stage.</p>
        /// </summary>
        /// <param name="renderTargetBlend0">The render target blend description for the first render target.</param>
        /// <param name="mask">The mask.</param>
        /// <returns>A new <see cref="BlendState" /> instance.</returns>
        /// <msdn-id>ff476500</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateBlendState([In] const D3D11_BLEND_DESC* pBlendStateDesc,[Out, Fast] ID3D11BlendState** ppBlendState)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateBlendState</unmanaged-short>
        /// <remarks><p>An application can create up to 4096 unique blend-state objects. For each object created, the runtime checks to see if a previous object  has the same state. If such a previous object exists, the runtime will return a reference to previous instance instead of creating a duplicate object.</p></remarks>
        public static BlendState New(RenderTargetBlendDescription renderTargetBlend0, int mask = -1)
        {
            return New(renderTargetBlend0, Color.White, mask);
        }

        /// <summary>
        /// <p>Create a blend-state object that encapsules blend state for the output-merger stage.</p>
        /// </summary>
        /// <param name="sourceBlend">The source blend.</param>
        /// <param name="destinationBlend">The destination blend.</param>
        /// <param name="blendOperation">The blend operation.</param>
        /// <param name="sourceAlphaBlend">The source alpha blend.</param>
        /// <param name="destinationAlphaBlend">The destination alpha blend.</param>
        /// <param name="alphaBlendOperation">The alpha blend operation.</param>
        /// <param name="renderTargetWriteMask">The render target write mask.</param>
        /// <param name="mask">The mask.</param>
        /// <returns>A new <see cref="BlendState" /> instance.</returns>
        /// <msdn-id>ff476500</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateBlendState([In] const D3D11_BLEND_DESC* pBlendStateDesc,[Out, Fast] ID3D11BlendState** ppBlendState)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateBlendState</unmanaged-short>
        /// <remarks><p>An application can create up to 4096 unique blend-state objects. For each object created, the runtime checks to see if a previous object  has the same state. If such a previous object exists, the runtime will return a reference to previous instance instead of creating a duplicate object.</p></remarks>
        public static BlendState New(BlendOption sourceBlend, BlendOption destinationBlend, BlendOperation blendOperation, BlendOption sourceAlphaBlend, BlendOption destinationAlphaBlend, BlendOperation alphaBlendOperation, ColorWriteMaskFlags renderTargetWriteMask = ColorWriteMaskFlags.All, int mask = -1)
        {
            return New(sourceBlend, destinationBlend, blendOperation, sourceAlphaBlend, destinationAlphaBlend, alphaBlendOperation, renderTargetWriteMask, Color.White, mask);            
        }

        /// <summary>
        /// <p>Create a blend-state object that encapsules blend state for the output-merger stage.</p>
        /// </summary>
        /// <param name="sourceBlend">The source blend.</param>
        /// <param name="destinationBlend">The destination blend.</param>
        /// <param name="blendOperation">The blend operation.</param>
        /// <param name="sourceAlphaBlend">The source alpha blend.</param>
        /// <param name="destinationAlphaBlend">The destination alpha blend.</param>
        /// <param name="alphaBlendOperation">The alpha blend operation.</param>
        /// <param name="renderTargetWriteMask">The render target write mask.</param>
        /// <param name="blendFactor">The blend factor.</param>
        /// <param name="mask">The mask.</param>
        /// <returns>A new <see cref="BlendState"/> instance.</returns>
        /// <msdn-id>ff476500</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateBlendState([In] const D3D11_BLEND_DESC* pBlendStateDesc,[Out, Fast] ID3D11BlendState** ppBlendState)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateBlendState</unmanaged-short>
        /// <remarks><p>An application can create up to 4096 unique blend-state objects. For each object created, the runtime checks to see if a previous object  has the same state. If such a previous object exists, the runtime will return a reference to previous instance instead of creating a duplicate object.</p></remarks>
        public static BlendState New(BlendOption sourceBlend, BlendOption destinationBlend, BlendOperation blendOperation, BlendOption sourceAlphaBlend, BlendOption destinationAlphaBlend, BlendOperation alphaBlendOperation, ColorWriteMaskFlags renderTargetWriteMask, Color4 blendFactor, int mask = -1)
        {
            return New(new RenderTargetBlendDescription(true, sourceBlend, destinationBlend, blendOperation, sourceAlphaBlend, destinationAlphaBlend, alphaBlendOperation, renderTargetWriteMask), blendFactor, mask);
        }

        /// <summary>
        /// <p>Create a blend-state object that encapsules blend state for the output-merger stage.</p>
        /// </summary>
        /// <param name="description"><dd>  <p>Pointer to a blend-state description (see <strong><see cref="SharpDX.Direct3D11.BlendStateDescription" /></strong>).</p> </dd></param>
        /// <param name="mask">The mask.</param>
        /// <returns>A new <see cref="BlendState"/> instance.</returns>
        /// <msdn-id>ff476500</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateBlendState([In] const D3D11_BLEND_DESC* pBlendStateDesc,[Out, Fast] ID3D11BlendState** ppBlendState)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateBlendState</unmanaged-short>
        /// <remarks><p>An application can create up to 4096 unique blend-state objects. For each object created, the runtime checks to see if a previous object  has the same state. If such a previous object exists, the runtime will return a reference to previous instance instead of creating a duplicate object.</p></remarks>
        public static BlendState New(BlendStateDescription description, int mask = -1)
        {
            return new BlendState(description, Color.White, mask);
        }

        /// <summary>
        /// <p>Create a blend-state object that encapsules blend state for the output-merger stage.</p>
        /// </summary>
        /// <param name="name">Name of this blend state.</param>
        /// <param name="description"><dd>  <p>Pointer to a blend-state description (see <strong><see cref="SharpDX.Direct3D11.BlendStateDescription" /></strong>).</p> </dd></param>
        /// <param name="mask">The mask.</param>
        /// <returns>A new <see cref="BlendState"/> instance.</returns>
        /// <msdn-id>ff476500</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateBlendState([In] const D3D11_BLEND_DESC* pBlendStateDesc,[Out, Fast] ID3D11BlendState** ppBlendState)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateBlendState</unmanaged-short>
        /// <remarks><p>An application can create up to 4096 unique blend-state objects. For each object created, the runtime checks to see if a previous object  has the same state. If such a previous object exists, the runtime will return a reference to previous instance instead of creating a duplicate object.</p></remarks>
        public static BlendState New(string name, BlendStateDescription description, int mask = -1)
        {
            return new BlendState(description, Color.White, mask) {Name = name};
        }

        /// <summary>
        /// <p>Create a blend-state object that encapsules blend state for the output-merger stage.</p>
        /// </summary>
        /// <param name="description"><dd>  <p>Pointer to a blend-state description (see <strong><see cref="SharpDX.Direct3D11.BlendStateDescription" /></strong>).</p> </dd></param>
        /// <param name="blendFactor">The blend factor.</param>
        /// <param name="mask">The mask.</param>
        /// <returns>A new <see cref="BlendState"/> instance.</returns>
        /// <msdn-id>ff476500</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateBlendState([In] const D3D11_BLEND_DESC* pBlendStateDesc,[Out, Fast] ID3D11BlendState** ppBlendState)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateBlendState</unmanaged-short>
        /// <remarks><p>An application can create up to 4096 unique blend-state objects. For each object created, the runtime checks to see if a previous object  has the same state. If such a previous object exists, the runtime will return a reference to previous instance instead of creating a duplicate object.</p></remarks>
        public static BlendState New(BlendStateDescription description, Color4 blendFactor, int mask = -1)
        {
            return new BlendState(description, blendFactor, mask);
        }

        protected override DeviceChild CreateResource()
        {
            return new Direct3D11.BlendState(GraphicsDevice, Description);
        }

        /// <summary>
        /// Implicit casting operator to <see cref="Direct3D11.Resource"/>
        /// </summary>
        /// <param name="from">The GraphicsState to convert from.</param>
        public static implicit operator Direct3D11.BlendState(BlendState from)
        {
            return (Direct3D11.BlendState) (from == null ? null : from.GetOrCreateResource());
        }

        private static BlendState New(string name, BlendOption sourceBlend, BlendOption destinationBlend)
        {
            var description = BlendStateDescription.Default();

            description.RenderTarget[0].IsBlendEnabled = true;
            description.RenderTarget[0].SourceBlend = sourceBlend;
            description.RenderTarget[0].DestinationBlend = destinationBlend;
            description.RenderTarget[0].SourceAlphaBlend = sourceBlend;
            description.RenderTarget[0].DestinationAlphaBlend = destinationBlend;

            return new BlendState(description, Color.White, -1) {Name = name};
        }
    }
}