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

using System.Runtime.InteropServices;
using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Base class for texture resources.
    /// </summary>
    /// <typeparam name="T">Type of the <see cref="N:SharpDX.Direct3D11"/> texture resource.</typeparam>
    public abstract class Texture<T> : GraphicsResource<T> where T : Resource
    {
        private ShaderResourceView shaderResourceView;
        private RenderTargetView renderTargetView;
        private UnorderedAccessView unorderedAccessView;

        protected void Initialize(GraphicsDevice deviceArg, T resource, BindFlags bindFlags)
        {
            this.Initialize(deviceArg, resource);

            if ((bindFlags & BindFlags.ShaderResource) != 0)
            {
                this.shaderResourceView = new ShaderResourceView(this.GraphicsDevice, this.Resource);
                ToDispose(shaderResourceView);
            }

            if ((bindFlags & BindFlags.RenderTarget) != 0)
            {
                this.renderTargetView = new RenderTargetView(this.GraphicsDevice, this.Resource);
                ToDispose(renderTargetView);
            }

            if ((bindFlags & BindFlags.UnorderedAccess) != 0)
            {
                this.unorderedAccessView = new UnorderedAccessView(this.GraphicsDevice, this.Resource);
                ToDispose(unorderedAccessView);
            }
        }

        /// <summary>
        /// ShaderResourceView casting operator.
        /// </summary>
        /// <param name="from">Source for the.</param>
        public static implicit operator ShaderResourceView(Texture<T> from)
        {
            return from.shaderResourceView;
        }

        /// <summary>
        /// RenderTargetView casting operator.
        /// </summary>
        /// <param name="from">Source for the.</param>
        public static implicit operator RenderTargetView(Texture<T> from)
        {
            return from.renderTargetView;
        }

        /// <summary>
        /// UnorderedAccessView casting operator.
        /// </summary>
        /// <param name="from">Source for the.</param>
        public static implicit operator UnorderedAccessView(Texture<T> from)
        {
            return from.unorderedAccessView;
        }

        protected override void OnNameChanged()
        {
            base.OnNameChanged();
            if (GraphicsDevice.IsDebugMode)
            {
                this.Resource.DebugName = Name;
                if (shaderResourceView != null)
                    shaderResourceView.DebugName = Name == null ? null : string.Format("{0} SRV", Name);
                if (renderTargetView != null)
                    renderTargetView.DebugName = Name == null ? null : string.Format("{0} RTV", Name);
                if (unorderedAccessView != null)
                    unorderedAccessView.DebugName = Name == null ? null : string.Format("{0} UAV", Name);
            }
        }
    }
}