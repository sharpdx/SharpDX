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
    /// SamplerState is equivalent to <see cref="SharpDX.Direct3D11.SamplerState"/>.
    /// </summary>
    /// <msdn-id>ff476588</msdn-id>	
    /// <unmanaged>ID3D11SamplerState</unmanaged>	
    /// <unmanaged-short>ID3D11SamplerState</unmanaged-short>	
    public class SamplerState : GraphicsResource
    {
        /// <summary>
        /// Default state for point filtering with texture coordinate wrapping.
        /// </summary>
        public static readonly SamplerState PointWrap = New("SamplerState.PointWrap", Filter.MinMagMipPoint, TextureAddressMode.Wrap);

        /// <summary>
        /// Default state for point filtering with texture coordinate clamping.
        /// </summary>
        public static readonly SamplerState PointClamp = New("SamplerState.PointClamp", Filter.MinMagMipPoint, TextureAddressMode.Clamp);

        /// <summary>
        /// Default state for linear filtering with texture coordinate wrapping.
        /// </summary>
        public static readonly SamplerState LinearWrap = New("SamplerState.LinearWrap", Filter.MinMagMipLinear, TextureAddressMode.Wrap);

        /// <summary>
        /// Default state for linear filtering with texture coordinate clamping.
        /// </summary>
        public static readonly SamplerState LinearClamp = New("SamplerState.LinearClamp", Filter.MinMagMipLinear, TextureAddressMode.Clamp);

        /// <summary>
        /// Default state for anisotropic filtering with texture coordinate wrapping.
        /// </summary>
        public static readonly SamplerState AnisotropicWrap = New("SamplerState.AnisotropicWrap", Filter.Anisotropic, TextureAddressMode.Wrap);

        /// <summary>
        /// Default state for anisotropic filtering with texture coordinate clamping.
        /// </summary>
        public static readonly SamplerState AnisotropicClamp = New("SamplerState.AnisotropicClamp", Filter.Anisotropic, TextureAddressMode.Clamp);

        /// <summary>
        /// Gets the description of this sampler state.
        /// </summary>
        public readonly SamplerStateDescription Description;

        /// <summary>
        /// Initializes a new instance of the <see cref="SamplerState" /> class.
        /// </summary>
        /// <param name="description">The description.</param>
        private SamplerState(SamplerStateDescription description)
            : this(GraphicsDevice.Current, description)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SamplerState" /> class.
        /// </summary>
        /// <param name="deviceLocal">The device local.</param>
        /// <param name="description">The description.</param>
        private SamplerState(GraphicsDevice deviceLocal, SamplerStateDescription description)
        {
            Description = description;
            Initialize(deviceLocal, null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SamplerState" /> class.
        /// </summary>
        /// <param name="deviceLocal">The device local.</param>
        /// <param name="nativeState">State of the native.</param>
        private SamplerState(GraphicsDevice deviceLocal, Direct3D11.SamplerState nativeState)
        {
            Description = nativeState.Description;
            Resource = ToDispose(nativeState);
            Initialize(deviceLocal, null);
        }

        /// <summary>	
        /// <p>Create a sampler-state object that encapsulates sampling information for a texture.</p>	
        /// </summary>	
        /// <param name="samplerState">An existing <see cref="Direct3D11.SamplerState"/> instance.</param>	
        /// <returns>A new <see cref="SamplerState"/> instance</returns>	
        /// <remarks>	
        /// <p>4096 unique sampler state objects can be created on a device at a time.</p><p>If an application attempts to create a sampler-state interface with the same state as an existing interface, the same interface will be returned and the total number of unique sampler state objects will stay the same.</p>	
        /// </remarks>	
        /// <msdn-id>ff476518</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateSamplerState([In] const D3D11_SAMPLER_DESC* pSamplerDesc,[Out, Fast] ID3D11SamplerState** ppSamplerState)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateSamplerState</unmanaged-short>	
        public static SamplerState New(Direct3D11.SamplerState samplerState)
        {
            return new SamplerState(GraphicsDevice.Current, samplerState);
        }

        /// <summary>	
        /// <p>Create a sampler-state object that encapsulates sampling information for a texture.</p>	
        /// </summary>	
        /// <param name="description">A sampler state description</param>	
        /// <returns>A new <see cref="SamplerState"/> instance</returns>	
        /// <remarks>	
        /// <p>4096 unique sampler state objects can be created on a device at a time.</p><p>If an application attempts to create a sampler-state interface with the same state as an existing interface, the same interface will be returned and the total number of unique sampler state objects will stay the same.</p>	
        /// </remarks>	
        /// <msdn-id>ff476518</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateSamplerState([In] const D3D11_SAMPLER_DESC* pSamplerDesc,[Out, Fast] ID3D11SamplerState** ppSamplerState)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateSamplerState</unmanaged-short>	
        public static SamplerState New(SamplerStateDescription description)
        {
            return new SamplerState(description);
        }

        /// <summary>	
        /// <p>Create a sampler-state object that encapsulates sampling information for a texture.</p>	
        /// </summary>	
        /// <param name="name">Name of this sampler state.</param>
        /// <param name="description">A sampler state description</param>	
        /// <returns>A new <see cref="SamplerState"/> instance</returns>	
        /// <remarks>	
        /// <p>4096 unique sampler state objects can be created on a device at a time.</p><p>If an application attempts to create a sampler-state interface with the same state as an existing interface, the same interface will be returned and the total number of unique sampler state objects will stay the same.</p>	
        /// </remarks>	
        /// <msdn-id>ff476518</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateSamplerState([In] const D3D11_SAMPLER_DESC* pSamplerDesc,[Out, Fast] ID3D11SamplerState** ppSamplerState)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateSamplerState</unmanaged-short>	
        public static SamplerState New(string name, SamplerStateDescription description)
        {
            return new SamplerState(description) {Name = name};
        }
        
        protected override DeviceChild CreateResource()
        {
            return new Direct3D11.SamplerState(GraphicsDevice, Description);
        }

        /// <summary>
        /// Implicit casting operator to <see cref="Direct3D11.Resource"/>
        /// </summary>
        /// <param name="from">The GraphicsState to convert from.</param>
        public static implicit operator Direct3D11.SamplerState(SamplerState from)
        {
            return (Direct3D11.SamplerState) (from == null ? null : from.GetOrCreateResource());
        }

        private static SamplerState New(string name, Filter filterMode, TextureAddressMode uvwMode)
        {
            var description = SamplerStateDescription.Default();
            description.Filter = filterMode;
            description.AddressU = uvwMode;
            description.AddressV = uvwMode;
            description.AddressW = uvwMode;
            return New(name, description);
        }
    }
}