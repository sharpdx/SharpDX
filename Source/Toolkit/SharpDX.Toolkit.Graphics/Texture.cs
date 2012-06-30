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

using System;
using System.IO;
using System.Runtime.InteropServices;
using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Base class for texture resources.
    /// </summary>
    /// <typeparam name="T">Type of the <see cref="N:SharpDX.Direct3D11"/> texture resource.</typeparam>
    public abstract class Texture : GraphicsResource
    {
        protected ShaderResourceView[] ShaderResourceViews;
        protected RenderTargetView[] RenderTargetViews;
        protected UnorderedAccessView[] UnorderedAccessViews;

        protected override void Initialize(GraphicsDevice deviceArg, Resource resource)
        {
            base.Initialize(deviceArg, resource);
            InitializeViews();
        }

        /// <summary>
        /// Initializes the views provided by this texture.
        /// </summary>
        protected virtual void InitializeViews()
        {
        }

        /// <summary>
        /// Gets a specific <see cref="ShaderResourceView" /> from this texture.
        /// </summary>
        /// <param name="viewSlice">Type of the view slice.</param>
        /// <param name="arrayOrDepthSlice">The texture array slice index.</param>
        /// <param name="mipIndex">The mip map slice index.</param>
        /// <returns>An <see cref="ShaderResourceView" /></returns>
        public abstract ShaderResourceView GetShaderResourceView(ViewSlice viewSlice, int arrayOrDepthSlice, int mipIndex);

        /// <summary>
        /// Gets a specific <see cref="RenderTargetView" /> from this texture.
        /// </summary>
        /// <param name="viewSlice">Type of the view slice.</param>
        /// <param name="arrayOrDepthSlice">The texture array slice index.</param>
        /// <param name="mipMapSlice">The mip map slice index.</param>
        /// <returns>An <see cref="RenderTargetView" /></returns>
        public abstract RenderTargetView GetRenderTargetView(ViewSlice viewSlice, int arrayOrDepthSlice, int mipMapSlice);

        /// <summary>
        /// Gets a specific <see cref="UnorderedAccessView"/> from this texture.
        /// </summary>
        /// <param name="arrayOrDepthSlice">The texture array slice index.</param>
        /// <param name="mipMapSlice">The mip map slice index.</param>
        /// <returns>An <see cref="UnorderedAccessView"/></returns>
        public abstract UnorderedAccessView GetUnorderedAccessView(int arrayOrDepthSlice, int mipMapSlice);

        /// <summary>
        /// ShaderResourceView casting operator.
        /// </summary>
        /// <param name="from">Source for the.</param>
        public static implicit operator ShaderResourceView(Texture from)
        {
            return from.ShaderResourceViews != null ? from.ShaderResourceViews[0] : null;
        }

        /// <summary>
        /// UnorderedAccessView casting operator.
        /// </summary>
        /// <param name="from">Source for the.</param>
        public static implicit operator UnorderedAccessView(Texture from)
        {
            return from.UnorderedAccessViews != null ? from.UnorderedAccessViews[0] : null;
        }

        protected override void OnNameChanged()
        {
            base.OnNameChanged();
            if (GraphicsDevice.IsDebugMode)
            {
                if (ShaderResourceViews != null)
                {
                    for (int i = 0; i < ShaderResourceViews.Length; i++)
                    {
                        var shaderResourceView = ShaderResourceViews[i];
                        if (shaderResourceView != null)
                            shaderResourceView.DebugName = Name == null ? null : string.Format("{0} SRV[{1}]", i, Name);
                    }
                }

                if (RenderTargetViews != null)
                {
                    for (int i = 0; i < RenderTargetViews.Length; i++)
                    {
                        var renderTargetView = RenderTargetViews[i];
                        if (renderTargetView != null)
                            renderTargetView.DebugName = Name == null ? null : string.Format("{0} RTV[{1}]", i, Name);
                    }
                }

                if (UnorderedAccessViews != null)
                {
                    for (int i = 0; i < UnorderedAccessViews.Length; i++)
                    {
                        var unorderedAccessView = UnorderedAccessViews[i];
                        if (unorderedAccessView != null)
                            unorderedAccessView.DebugName = Name == null ? null : string.Format("{0} UAV[{1}]", i, Name);
                    }
                }
            }
        }
    }
}