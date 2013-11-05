﻿// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
    /// RasterizerState is equivalent to <see cref="SharpDX.Direct3D11.RasterizerState"/>.
    /// </summary>
    /// <msdn-id>ff476580</msdn-id>	
    /// <unmanaged>ID3D11RasterizerState</unmanaged>	
    /// <unmanaged-short>ID3D11RasterizerState</unmanaged-short>	
    public class RasterizerState : GraphicsResource
    {
        /// <summary>
        /// Gets the description of this blend state.
        /// </summary>
        public readonly RasterizerStateDescription Description;

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterizerState" /> class.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="description">The description.</param>
        private RasterizerState(GraphicsDevice device, RasterizerStateDescription description) : base(device.MainDevice)
        {
            this.Description = description;
            this.Initialize(new Direct3D11.RasterizerState(GraphicsDevice, Description));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterizerState" /> class.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="nativeState">State of the native.</param>
        private RasterizerState(GraphicsDevice device, Direct3D11.RasterizerState nativeState) : base(device.MainDevice)
        {
            this.Description = nativeState.Description;
            this.Initialize(nativeState);
        }

        /// <summary>
        /// Initializes the specified device local.
        /// </summary>
        /// <param name="resource">The resource.</param>
        protected override void Initialize(DeviceChild resource)
        {
            base.Initialize(resource);
        }

        /// <summary>	
        /// <p>Create a rasterizer state object that tells the rasterizer stage how to behave.</p>	
        /// </summary>	
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="rasterizerState">An existing <see cref="Direct3D11.RasterizerState"/> instance.</param>	
        /// <remarks>	
        /// <p>4096 unique rasterizer state objects can be created on a device at a time.</p><p>If an application attempts to create a rasterizer-state interface with the same state as an existing interface, the same interface will be returned and the total number of unique rasterizer state objects will stay the same.</p>	
        /// </remarks>	
        /// <msdn-id>ff476516</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateRasterizerState([In] const D3D11_RASTERIZER_DESC* pRasterizerDesc,[Out, Fast] ID3D11RasterizerState** ppRasterizerState)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateRasterizerState</unmanaged-short>	
        public static RasterizerState New(GraphicsDevice device, Direct3D11.RasterizerState rasterizerState)
        {
            return new RasterizerState(device, rasterizerState);
        }

        /// <summary>	
        /// <p>Create a rasterizer state object that tells the rasterizer stage how to behave.</p>	
        /// </summary>	
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="description">A rasterizer state description</param>	
        /// <remarks>	
        /// <p>4096 unique rasterizer state objects can be created on a device at a time.</p><p>If an application attempts to create a rasterizer-state interface with the same state as an existing interface, the same interface will be returned and the total number of unique rasterizer state objects will stay the same.</p>	
        /// </remarks>	
        /// <msdn-id>ff476516</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateRasterizerState([In] const D3D11_RASTERIZER_DESC* pRasterizerDesc,[Out, Fast] ID3D11RasterizerState** ppRasterizerState)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateRasterizerState</unmanaged-short>	
        public static RasterizerState New(GraphicsDevice device, RasterizerStateDescription description)
        {
            return new RasterizerState(device, description);
        }

        /// <summary>	
        /// <p>Create a rasterizer state object that tells the rasterizer stage how to behave.</p>	
        /// </summary>	
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="name">Name of this depth stencil state.</param>
        /// <param name="description">A rasterizer state description</param>	
        /// <remarks>	
        /// <p>4096 unique rasterizer state objects can be created on a device at a time.</p><p>If an application attempts to create a rasterizer-state interface with the same state as an existing interface, the same interface will be returned and the total number of unique rasterizer state objects will stay the same.</p>	
        /// </remarks>	
        /// <msdn-id>ff476516</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateRasterizerState([In] const D3D11_RASTERIZER_DESC* pRasterizerDesc,[Out, Fast] ID3D11RasterizerState** ppRasterizerState)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateRasterizerState</unmanaged-short>	
        public static RasterizerState New(GraphicsDevice device, string name, RasterizerStateDescription description)
        {
            return new RasterizerState(device, description) {Name = name};
        }
        
        /// <summary>
        /// Implicit casting operator to <see cref="Direct3D11.Resource"/>
        /// </summary>
        /// <param name="from">The GraphicsState to convert from.</param>
        public static implicit operator Direct3D11.RasterizerState(RasterizerState from)
        {
            return (Direct3D11.RasterizerState) (from == null ? null : from.Resource);
        }

        internal static RasterizerState New(GraphicsDevice device, string name, CullMode mode)
        {
            var description = RasterizerStateDescription.Default();
            description.CullMode = mode;

            var state = New(device, description);
            state.Name = name;
            return state;
        }
    }
}