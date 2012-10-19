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
using System.Collections.Generic;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Diagnostics;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Main class to apply shader effects.
    /// </summary>
    [ContentReader(typeof(EffectContentReader))]
    public class Effect : GraphicsResource
    {
        public delegate EffectPass OnApplyDelegate(EffectPass pass);

        private Dictionary<EffectConstantBufferKey, EffectConstantBuffer> effectConstantBuffersCache;

        /// <summary>
        ///   Gets a collection of constant buffers that are defined for this effect.
        /// </summary>
        public readonly EffectConstantBufferCollection ConstantBuffers;

        /// <summary>
        ///   Gets a collection of parameters that are defined for this effect.
        /// </summary>
        public readonly EffectParameterCollection Parameters;

        internal readonly EffectResourceLinker ResourceLinker;

        /// <summary>
        ///   Gets a collection of techniques that are defined for this effect.
        /// </summary>
        public readonly EffectTechniqueCollection Techniques;

        private EffectData.Effect rawEffect;

        /// <summary>
        /// Set to true to force all constant shaders to be shared between other effects within a common <see cref="EffectPool"/>. Default is false.
        /// </summary>
        /// <remarks>
        /// This value can also be set in the TKFX file directly by setting ShareConstantBuffers = true; in a pass.
        /// </remarks>
        protected internal bool ShareConstantBuffers;

        /// <summary>
        /// Initializes a new instance of the <see cref="Effect" /> class with the specified bytecode effect. See remarks.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="bytecode">The bytecode to add to <see cref="GraphicsDevice.DefaultEffectPool"/>. This bytecode must contain only one effect.</param>
        /// <exception cref="ArgumentException">If the bytecode doesn't contain a single effect.</exception>
        /// <remarks>
        /// The effect bytecode must contain only a single effect and will be registered into the <see cref="GraphicsDevice.DefaultEffectPool"/>.
        /// </remarks>
        public Effect(GraphicsDevice device, byte[] bytecode)
            : this(device, EffectData.Load(bytecode))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Effect" /> class with the specified bytecode effect. See remarks.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="effectData">The bytecode to add to <see cref="GraphicsDevice.DefaultEffectPool"/>. This bytecode must contain only one effect.</param>
        /// <exception cref="ArgumentException">If the bytecode doesn't contain a single effect.</exception>
        /// <remarks>
        /// The effect bytecode must contain only a single effect and will be registered into the <see cref="GraphicsDevice.DefaultEffectPool"/>.
        /// </remarks>
        public Effect(GraphicsDevice device, EffectData effectData) : base(device)
        {
            if (effectData.Effects.Count != 1)
                throw new ArgumentException(string.Format("Expecting only one effect in the effect bytecode instead of [{0}]. Use GraphicsDevice.DefaultEffectPool.RegisterBytecode instead", Utilities.Join(",", effectData.Effects)), "bytecode");

            ConstantBuffers = new EffectConstantBufferCollection();
            Parameters = new EffectParameterCollection();
            Techniques = new EffectTechniqueCollection();
            ResourceLinker = ToDispose(new EffectResourceLinker());
            Pool = device.DefaultEffectPool;

            // Sets the effect name
            Name = effectData.Effects[0].Name;

            // Register the bytecode to the pool
            Pool.RegisterBytecode(effectData);

            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Effect" /> class with the specified effect. See remarks.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="effectName">Name of the effect.</param>
        /// <remarks>
        /// The effect must have been loaded and registered into the <see cref="GraphicsDevice.DefaultEffectPool"/>.
        /// </remarks>
        public Effect(GraphicsDevice device, string effectName) : this(device, device.DefaultEffectPool, effectName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Effect" /> class with the specified effect loaded from an effect pool.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="pool">The effect pool.</param>
        /// <param name="effectName">Name of the effect.</param>
        public Effect(GraphicsDevice device, EffectPool pool, string effectName) : base(device, effectName)
        {
            ConstantBuffers = new EffectConstantBufferCollection();
            Parameters = new EffectParameterCollection();
            Techniques = new EffectTechniqueCollection();
            ResourceLinker = ToDispose(new EffectResourceLinker());
            Pool = pool;
            Initialize();
        }

        /// <summary>
        ///   Gets the pool this effect attached to.
        /// </summary>
        /// <value> The pool. </value>
        public readonly EffectPool Pool;

        /// <summary>
        ///   Occurs when the on apply is applied on a pass.
        /// </summary>
        /// <remarks>
        ///   This external hook provides a way to pre-configure a pipeline when a pass is applied.
        ///   Subclass of this class can override the method <see cref="OnApply" />.
        /// </remarks>
        public event OnApplyDelegate OnApplyCallback;

        /// <summary>
        /// Gets or sets the current technique. By default, it is set to the first available technique in this effect.
        /// </summary>
        /// <value>The current technique.</value>
        public EffectTechnique CurrentTechnique { get; set; }

        protected virtual void Initialize()
        {
            Initialize(Pool.Find(Name));

            // If everything was fine, then we can register it into the pool
            Pool.AddEffect(this);
        }

        /// <summary>
        /// Initializes the specified effect bytecode.
        /// </summary>
        /// <param name="effectDataArg">The effect bytecode.</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        private void Initialize(EffectData.Effect effectDataArg)
        {
            if (effectDataArg == null)
                throw new ArgumentException(string.Format("Unable to find effect [{0}] from the EffectPool", Name), "effectName");

            rawEffect = effectDataArg;

            ShareConstantBuffers = effectDataArg.ShareConstantBuffers;

            // Create the local effect constant buffers cache
            if (!ShareConstantBuffers)
                effectConstantBuffersCache = new Dictionary<EffectConstantBufferKey, EffectConstantBuffer>();

            if (effectDataArg.Techniques.Count == 0)
                throw new InvalidOperationException("No techniques found in this effect");

            var logger = new Logger();
            int techniqueIndex = 0;
            int totalPassCount = 0;
            EffectPass parentPass = null;
            foreach (var techniqueRaw in effectDataArg.Techniques)
            {
                var name = techniqueRaw.Name;
                if (string.IsNullOrEmpty(name))
                    name = string.Format("${0}", techniqueIndex++);

                var technique = new EffectTechnique(this, name);
                Techniques.Add(technique);

                int passIndex = 0;
                foreach (var passRaw in techniqueRaw.Passes)
                {
                    name = passRaw.Name;
                    if (string.IsNullOrEmpty(name))
                        name = string.Format("${0}", passIndex++);

                    var pass = new EffectPass(logger, this, technique, passRaw, name);

                    pass.Initialize(logger);

                    // If this is a subpass, add it to the parent pass
                    if (passRaw.IsSubPass)
                    {
                        if (parentPass == null)
                        {
                            logger.Error("Pass [{0}] is declared as a subpass but has no parent");
                        }
                        else
                        {
                            parentPass.SubPasses.Add(pass);
                        }
                    }
                    else
                    {
                        technique.Passes.Add(pass);
                        parentPass = pass;
                    }
                }

                // Count the number of passes
                totalPassCount += technique.Passes.Count;
            }

            if (totalPassCount == 0)
                throw new InvalidOperationException("No passes found in this effect");

            // Log all the exception in a single throw
            if (logger.HasErrors)
                throw new InvalidOperationException(Utilities.Join("\n", logger.Messages));

            // Initialize the resource linker when we are done with all pass/parameters
            ResourceLinker.Initialize();

            //// Sort all parameters by their resource types
            //// in order to achieve better local cache coherency in resource linker
            //Parameters.Items.Sort((left, right) => (int)left.ResourceType - (int)right.ResourceType);

            // Prelink constant buffers
            foreach (var parameter in Parameters)
            {
                // Set the default values 
                parameter.SetDefaultValue();

                if (parameter.ResourceType == EffectResourceType.ConstantBuffer)
                    parameter.SetResource(0, ConstantBuffers[parameter.Name]);
            }

            // Compute slot links
            foreach (var technique in Techniques)
            {
                foreach (var pass in technique.Passes)
                {
                    foreach (var subPass in pass.SubPasses)
                    {
                        subPass.ComputeSlotLinks();
                    }
                    pass.ComputeSlotLinks();
                }
            }

            // Setup the first Current Technique.
            CurrentTechnique = this.Techniques[0];
        }

        internal new DisposeCollector DisposeCollector
        {
            get { return base.DisposeCollector; }
        }

        protected internal virtual EffectPass OnApply(EffectPass pass)
        {
            var handler = OnApplyCallback;
            if (handler != null) pass = handler(pass);

            return pass;
        }

        protected override void Dispose(bool disposeManagedResources)
        {
            // Remove this instance from the pool
            Pool.RemoveEffect(this);

            base.Dispose(disposeManagedResources);
        }

        internal EffectConstantBuffer GetOrCreateConstantBuffer(GraphicsDevice context, EffectData.ConstantBuffer bufferRaw)
        {
            EffectConstantBuffer constantBuffer;
            // Is the effect is using shared constant buffers via the EffectPool?
            if (ShareConstantBuffers)
            {
                // Use the pool to share constant buffers
                constantBuffer = Pool.GetOrCreateConstantBuffer(context, bufferRaw);
            }
            else
            {
                // ----------------------------------------------------------------------------
                // Get an existing constant buffer having the same name/sizel/ayout/parameters
                // ----------------------------------------------------------------------------
                var bufferKey = new EffectConstantBufferKey(bufferRaw);
                if (!effectConstantBuffersCache.TryGetValue(bufferKey, out constantBuffer))
                {
                    // 4) If this buffer doesn't exist, create a new one and register it.
                    constantBuffer = new EffectConstantBuffer(context, bufferRaw);
                    effectConstantBuffersCache.Add(bufferKey, constantBuffer);
                    DisposeCollector.Collect(constantBuffer);
                }
            }

            return constantBuffer;
        }
    }
}