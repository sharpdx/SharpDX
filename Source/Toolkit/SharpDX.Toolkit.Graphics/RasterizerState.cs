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
    /// RasterizerState is equivalent to <see cref="SharpDX.Direct3D11.RasterizerState"/>.
    /// </summary>
    /// <msdn-id>ff476580</msdn-id>	
    /// <unmanaged>ID3D11RasterizerState</unmanaged>	
    /// <unmanaged-short>ID3D11RasterizerState</unmanaged-short>	
    public class RasterizerState : GraphicsResource
    {
        /// <summary>
        /// Built-in raterizer state object with settings for culling primitives with clockwise winding order (front facing).
        /// </summary>
        public static readonly RasterizerState CullFront = New("RasterizerState.CullFront", CullMode.Front);

        /// <summary>
        /// Built-in raterizer state object with settings for culling primitives with counter-clockwise winding order (back facing).
        /// </summary>
        public static readonly RasterizerState CullBack = New("RasterizerState.CullBack", CullMode.Back);

        /// <summary>
        /// Built-in raterizer state object with settings for not culling any primitives.
        /// </summary>
        public static readonly RasterizerState CullNone = New("RasterizerState.CullNone", CullMode.None);

        /// <summary>
        /// Built-in default raterizer state object is back facing (see <see cref="CullBack"/>).
        /// </summary>
        public static readonly RasterizerState Default = CullBack;

        /// <summary>
        /// Gets the description of this blend state.
        /// </summary>
        public readonly RasterizerStateDescription Description;

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterizerState" /> class.
        /// </summary>
        /// <param name="description">The description.</param>
        private RasterizerState(RasterizerStateDescription description)
            : this(GraphicsDevice.CurrentSafe, description)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterizerState" /> class.
        /// </summary>
        /// <param name="deviceLocal">The device local.</param>
        /// <param name="description">The description.</param>
        private RasterizerState(GraphicsDevice deviceLocal, RasterizerStateDescription description)
        {
            Description = description;
            Initialize(deviceLocal, null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterizerState" /> class.
        /// </summary>
        /// <param name="deviceLocal">The device local.</param>
        /// <param name="nativeState">State of the native.</param>
        private RasterizerState(GraphicsDevice deviceLocal, Direct3D11.RasterizerState nativeState)
        {
            Description = nativeState.Description;
            Resource = ToDispose(nativeState);
            Initialize(deviceLocal, null);
        }

        /// <summary>	
        /// <p>Create a rasterizer state object that tells the rasterizer stage how to behave.</p>	
        /// </summary>	
        /// <param name="rasterizerState">An existing <see cref="Direct3D11.RasterizerState"/> instance.</param>	
        /// <remarks>	
        /// <p>4096 unique rasterizer state objects can be created on a device at a time.</p><p>If an application attempts to create a rasterizer-state interface with the same state as an existing interface, the same interface will be returned and the total number of unique rasterizer state objects will stay the same.</p>	
        /// </remarks>	
        /// <msdn-id>ff476516</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateRasterizerState([In] const D3D11_RASTERIZER_DESC* pRasterizerDesc,[Out, Fast] ID3D11RasterizerState** ppRasterizerState)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateRasterizerState</unmanaged-short>	
        public static RasterizerState New(Direct3D11.RasterizerState rasterizerState)
        {
            return new RasterizerState(GraphicsDevice.CurrentSafe, rasterizerState);
        }

        /// <summary>	
        /// <p>Create a rasterizer state object that tells the rasterizer stage how to behave.</p>	
        /// </summary>	
        /// <param name="description">A rasterizer state description</param>	
        /// <remarks>	
        /// <p>4096 unique rasterizer state objects can be created on a device at a time.</p><p>If an application attempts to create a rasterizer-state interface with the same state as an existing interface, the same interface will be returned and the total number of unique rasterizer state objects will stay the same.</p>	
        /// </remarks>	
        /// <msdn-id>ff476516</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateRasterizerState([In] const D3D11_RASTERIZER_DESC* pRasterizerDesc,[Out, Fast] ID3D11RasterizerState** ppRasterizerState)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateRasterizerState</unmanaged-short>	
        public static RasterizerState New(RasterizerStateDescription description)
        {
            return new RasterizerState(description);
        }

        /// <summary>	
        /// <p>Create a rasterizer state object that tells the rasterizer stage how to behave.</p>	
        /// </summary>	
        /// <param name="name">Name of this depth stencil state.</param>
        /// <param name="description">A rasterizer state description</param>	
        /// <remarks>	
        /// <p>4096 unique rasterizer state objects can be created on a device at a time.</p><p>If an application attempts to create a rasterizer-state interface with the same state as an existing interface, the same interface will be returned and the total number of unique rasterizer state objects will stay the same.</p>	
        /// </remarks>	
        /// <msdn-id>ff476516</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateRasterizerState([In] const D3D11_RASTERIZER_DESC* pRasterizerDesc,[Out, Fast] ID3D11RasterizerState** ppRasterizerState)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateRasterizerState</unmanaged-short>	
        public static RasterizerState New(string name, RasterizerStateDescription description)
        {
            return new RasterizerState(description) {Name = name};
        }
        
        protected override DeviceChild CreateResource()
        {
            return new Direct3D11.RasterizerState(GraphicsDevice, Description);
        }

        /// <summary>
        /// Implicit casting operator to <see cref="Direct3D11.Resource"/>
        /// </summary>
        /// <param name="from">The GraphicsState to convert from.</param>
        public static implicit operator Direct3D11.RasterizerState(RasterizerState from)
        {
            return (Direct3D11.RasterizerState) (from == null ? null : from.GetOrCreateResource());
        }

        private static RasterizerState New(string name, CullMode mode)
        {
            var description = RasterizerStateDescription.Default();
            description.CullMode = mode;

            var state = New(description);
            state.Name = name;
            return state;
        }
    }
}