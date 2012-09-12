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
    /// DepthStencilState is equivalent to <see cref="SharpDX.Direct3D11.DepthStencilState"/>.
    /// </summary>
    /// <msdn-id>ff476375</msdn-id>	
    /// <unmanaged>ID3D11DepthStencilState</unmanaged>	
    /// <unmanaged-short>ID3D11DepthStencilState</unmanaged-short>	
    public class DepthStencilState : GraphicsResource
    {
        /// <summary>
        /// A built-in state object with default settings for using a depth stencil buffer.
        /// </summary>
        public static readonly DepthStencilState Default = New("DepthStencilState.Default", true, true);

        /// <summary>
        /// A built-in state object with settings for enabling a read-only depth stencil buffer.
        /// </summary>
        public static readonly DepthStencilState DepthRead = New("DepthStencilState.DepthRead", true, false);

        /// <summary>
        /// A built-in state object with settings for not using a depth stencil buffer.
        /// </summary>
        public static readonly DepthStencilState None = New("DepthStencilState.None", false, false);

        /// <summary>
        /// Gets the description of this blend state.
        /// </summary>
        public readonly DepthStencilStateDescription Description;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepthStencilState" /> class.
        /// </summary>
        /// <param name="description">The description.</param>
        private DepthStencilState(DepthStencilStateDescription description)
            : this(GraphicsDevice.CurrentSafe.MainDevice, description)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DepthStencilState" /> class.
        /// </summary>
        /// <param name="deviceLocal">The device local.</param>
        /// <param name="description">The description.</param>
        private DepthStencilState(GraphicsDevice deviceLocal, DepthStencilStateDescription description)
        {
            Description = description;
            Initialize(deviceLocal, null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DepthStencilState" /> class.
        /// </summary>
        /// <param name="deviceLocal">The device local.</param>
        /// <param name="nativeState">State of the native.</param>
        private DepthStencilState(GraphicsDevice deviceLocal, Direct3D11.DepthStencilState nativeState)
        {
            Description = nativeState.Description;
            Resource = ToDispose(nativeState);
            Initialize(deviceLocal, null);
        }

        /// <summary>	
        /// Create a depth-stencil state object that encapsulates depth-stencil test information for the output-merger stage.
        /// </summary>	
        /// <param name="depthStencilState">An existing <see cref="Direct3D11.DepthStencilState"/> instance.</param>	
        /// <returns>A new instance of <see cref="DepthStencilState"/></returns>	
        /// <remarks>	
        /// <p>4096 unique depth-stencil state objects can be created on a device at a time.</p><p>If an application attempts to create a depth-stencil-state interface with the same state as an existing interface, the same interface will be returned and the total number of unique depth-stencil state objects will stay the same.</p>	
        /// </remarks>	
        /// <msdn-id>ff476506</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateDepthStencilState([In] const D3D11_DEPTH_STENCIL_DESC* pDepthStencilDesc,[Out, Fast] ID3D11DepthStencilState** ppDepthStencilState)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateDepthStencilState</unmanaged-short>	
        public static DepthStencilState New(Direct3D11.DepthStencilState depthStencilState)
        {
            return new DepthStencilState(GraphicsDevice.CurrentSafe.MainDevice, depthStencilState);
        }

        /// <summary>	
        /// Create a depth-stencil state object that encapsulates depth-stencil test information for the output-merger stage.
        /// </summary>	
        /// <param name="description">A depth-stencil state description</param>	
        /// <returns>A new instance of <see cref="DepthStencilState"/></returns>	
        /// <remarks>	
        /// <p>4096 unique depth-stencil state objects can be created on a device at a time.</p><p>If an application attempts to create a depth-stencil-state interface with the same state as an existing interface, the same interface will be returned and the total number of unique depth-stencil state objects will stay the same.</p>	
        /// </remarks>	
        /// <msdn-id>ff476506</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateDepthStencilState([In] const D3D11_DEPTH_STENCIL_DESC* pDepthStencilDesc,[Out, Fast] ID3D11DepthStencilState** ppDepthStencilState)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateDepthStencilState</unmanaged-short>	
        public static DepthStencilState New(DepthStencilStateDescription description)
        {
            return new DepthStencilState(description);
        }

        /// <summary>	
        /// Create a depth-stencil state object that encapsulates depth-stencil test information for the output-merger stage.
        /// </summary>
        /// <param name="name">Name of this depth stencil state.</param>
        /// <param name="description">A depth-stencil state description</param>	
        /// <returns>A new instance of <see cref="DepthStencilState"/></returns>	
        /// <remarks>	
        /// <p>4096 unique depth-stencil state objects can be created on a device at a time.</p><p>If an application attempts to create a depth-stencil-state interface with the same state as an existing interface, the same interface will be returned and the total number of unique depth-stencil state objects will stay the same.</p>	
        /// </remarks>	
        /// <msdn-id>ff476506</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateDepthStencilState([In] const D3D11_DEPTH_STENCIL_DESC* pDepthStencilDesc,[Out, Fast] ID3D11DepthStencilState** ppDepthStencilState)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateDepthStencilState</unmanaged-short>	
        public static DepthStencilState New(string name, DepthStencilStateDescription description)
        {
            return new DepthStencilState(description) {Name = name};
        }

        protected override DeviceChild CreateResource()
        {
            return new Direct3D11.DepthStencilState(GraphicsDevice, Description);
        }

        /// <summary>
        /// Implicit casting operator to <see cref="Direct3D11.Resource"/>
        /// </summary>
        /// <param name="from">The GraphicsState to convert from.</param>
        public static implicit operator Direct3D11.DepthStencilState(DepthStencilState from)
        {
            return (Direct3D11.DepthStencilState) (from == null ? null : from.GetOrCreateResource());
        }

        private static DepthStencilState New(string name, bool depthEnable, bool depthWriteEnable)
        {
            var description = DepthStencilStateDescription.Default();
            description.IsDepthEnabled = depthEnable;
            description.DepthWriteMask = depthWriteEnable ? DepthWriteMask.All : DepthWriteMask.Zero;

            var state = New(description);
            state.Name = name;
            return state;
        }
    }
}