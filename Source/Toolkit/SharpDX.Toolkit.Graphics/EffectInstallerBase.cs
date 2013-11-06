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

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Base class to handle <see cref="IEffectInstaller"/>.
    /// </summary>
    public abstract class EffectInstallerBase : Component, IEffectInstaller
    {
        private GraphicsDevice graphicsDevice;
        private IGraphicsDeviceService graphicsDeviceService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectInstallerBase"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <exception cref="System.ArgumentNullException">services</exception>
        /// <exception cref="System.ArgumentException">Cannot locate IGraphicsDeviceService;services</exception>
        protected EffectInstallerBase(IServiceRegistry services)
        {
            if (services == null)
            {
                throw new ArgumentNullException("services");
            }

            Services = services;
            graphicsDeviceService = (IGraphicsDeviceService)services.GetService(typeof(IGraphicsDeviceService));

            if (graphicsDeviceService == null)
            {
                throw new ArgumentException("Cannot locate IGraphicsDeviceService", "services");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectInstallerBase"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <exception cref="System.ArgumentNullException">graphicsDevice</exception>
        protected EffectInstallerBase(GraphicsDevice graphicsDevice)
        {
            if (graphicsDevice == null)
            {
                throw new ArgumentNullException("graphicsDevice");
            }

            this.graphicsDevice = graphicsDevice;
        }

        /// <summary>
        /// Gets the graphics device.
        /// </summary>
        /// <value>The graphics device.</value>
        protected GraphicsDevice GraphicsDevice
        {
            get
            {
                return graphicsDevice ?? graphicsDeviceService.GraphicsDevice;
            }
        }

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <value>The services.</value>
        /// <remarls>
        /// May be null if this installer was initialized with only a <see cref="SharpDX.Toolkit.Graphics.GraphicsDevice"/>.
        /// </remarls>
        protected IServiceRegistry Services { get; private set; }

        /// <summary>Applies this installer to a model.</summary>
        /// <param name="model">The model to be processed by this installer.</param>
        /// <exception cref="System.ArgumentNullException">model</exception>
        public void Apply(Model model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            model.ForEach(meshPart =>
                {
                    meshPart.Effect = Process(model, meshPart);
                });
        }

        /// <summary>
        /// Implements this method to process a <see cref="ModelMeshPart"/>.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="meshPart">The mesh part.</param>
        /// <returns>Effect to be associated to the <see cref="ModelMeshPart"/>.</returns>
        protected abstract Effect Process(Model model, ModelMeshPart meshPart);
    }
}