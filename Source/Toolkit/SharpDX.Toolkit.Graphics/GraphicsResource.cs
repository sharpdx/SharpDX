// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
using System.Runtime.InteropServices;

using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Base class for all <see cref="GraphicsResource"/>.
    /// </summary>
    public abstract class GraphicsResource : Component
    {
        /// <summary>GraphicsDevice used to create this instance.</summary>
        /// <value>The graphics device.</value>
        public GraphicsDevice GraphicsDevice { get; internal set; }

        /// <summary>The attached Direct3D11 resource to this instance.</summary>
        internal DeviceChild Resource { get; set; }

        /// <summary>Initializes a new instance of the <see cref="GraphicsResource" /> class.</summary>
        internal GraphicsResource()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="GraphicsResource"/> class.</summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        protected GraphicsResource(GraphicsDevice graphicsDevice) : this(graphicsDevice, null)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="GraphicsResource"/> class.</summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException">graphicsDevice</exception>
        protected GraphicsResource(GraphicsDevice graphicsDevice, string name) : base(name)
        {
            if (graphicsDevice == null)
                throw new ArgumentNullException("graphicsDevice");

            GraphicsDevice = graphicsDevice;
        }

        /// <summary>Initializes the specified device local.</summary>
        /// <param name="resource">The resource.</param>
        protected virtual void Initialize(DeviceChild resource)
        {
            Resource = ToDispose(resource);
            if (resource != null)
            {
                resource.Tag = this;
            }
        }

        /// <summary>Implicit casting operator to <see cref="Direct3D11.Resource" /></summary>
        /// <param name="from">The GraphicsResource to convert from.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Resource(GraphicsResource from)
        {
            return from == null ? null : (Resource)from.Resource;
        }

        /// <summary>Gets the CPU access flags from the <see cref="ResourceUsage" />.</summary>
        /// <param name="usage">The usage.</param>
        /// <returns>The CPU access flags</returns>
        protected static CpuAccessFlags GetCpuAccessFlagsFromUsage(ResourceUsage usage)
        {
            switch (usage)
            {
                case ResourceUsage.Dynamic:
                    return CpuAccessFlags.Write;
                case ResourceUsage.Staging:
                    return CpuAccessFlags.Read | CpuAccessFlags.Write;
            }
            return CpuAccessFlags.None;
        }

        /// <summary>Disposes of object resources.</summary>
        /// <param name="disposeManagedResources">If true, managed resources should be
        /// disposed of in addition to unmanaged resources.</param>
        protected override void Dispose(bool disposeManagedResources)
        {
            base.Dispose(disposeManagedResources);
            if (disposeManagedResources)
                Resource = null;
        }

        /// <summary>Called when name changed for this component.</summary>
        /// <param name="propertyName">Name of the property.</param>
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "Name")
            {
                if (GraphicsDevice.IsDebugMode && this.Resource != null) this.Resource.DebugName = Name;
            }
        }

        /// <summary>Functions the pin.</summary>
        /// <param name="handles">The handles.</param>
        protected static void UnPin(GCHandle[] handles)
        {
            if (handles != null)
            {
                for (int i = 0; i < handles.Length; i++)
                {
                    if (handles[i].IsAllocated)
                        handles[i].Free();
                }
            }
        }
    }
}