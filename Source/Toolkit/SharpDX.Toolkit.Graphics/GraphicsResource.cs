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
    public abstract class GraphicsResource : Component
    {
        protected internal GraphicsDevice GraphicsDevice;
        protected internal Resource Resource;

        protected virtual void Initialize(GraphicsDevice deviceLocal, Resource resource)
        {
            GraphicsDevice = deviceLocal;
            Resource = ToDispose(resource);
        }

        /// <summary>
        /// Copies the content of this resource to another <see cref="GraphicsResource"/>.
        /// </summary>
        /// <param name="toTexture">The texture to receive the copy.</param>
        /// <remarks>
        /// See unmanaged documentation for usage and restrictions.
        /// </remarks>
        /// <msdn-id>ff476392</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::CopyResource([In] ID3D11Resource* pDstResource,[In] ID3D11Resource* pSrcResource)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::CopyResource</unmanaged-short>	
        public void CopyTo(GraphicsResource toTexture)
        {
            var context = (DeviceContext)GraphicsDevice.Current;
            context.CopyResource(this, toTexture);
        }

        /// <summary>
        /// Implicit casting operator to <see cref="Direct3D11.Resource"/>
        /// </summary>
        /// <param name="from">The GraphicsResource to convert from.</param>
        public static implicit operator Resource(GraphicsResource from)
        {
            return from.Resource;
        }

        protected static CpuAccessFlags GetCputAccessFlagsFromUsage(ResourceUsage usage)
        {
            switch (usage)
            {
                case ResourceUsage.Dynamic:
                    return CpuAccessFlags.Write;
                case ResourceUsage.Staging:
                    return CpuAccessFlags.Read;
            }
            return CpuAccessFlags.None;
        }

        protected override void OnNameChanged()
        {
            base.OnNameChanged();
            if (GraphicsDevice.IsDebugMode)
            {
                this.Resource.DebugName = Name;
            }
        }

    }

    public abstract class GraphicsResource<T> : GraphicsResource where T : Resource
    {
        protected new T Resource;

        protected override void Initialize(GraphicsDevice deviceLocal, Resource resource)
        {
            base.Initialize(deviceLocal, resource);
            Resource = (T)resource;
        }
    }
}