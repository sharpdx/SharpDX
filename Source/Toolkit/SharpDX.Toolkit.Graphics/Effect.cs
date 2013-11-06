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
        /// <summary>
        /// Occurs when the effect is being initialized (after a recompilation at runtime for example)
        /// </summary>
        public event EventHandler<EventArgs> Initialized;

        /// <summary>The configuration apply delegate delegate.</summary>
        /// <param name="pass">The pass.</param>
        /// <returns>EffectPass.</returns>
        public delegate EffectPass OnApplyDelegate(EffectPass pass);

        private Dictionary<EffectConstantBufferKey, EffectConstantBuffer> effectConstantBuffersCache;

        internal EffectResourceLinker ResourceLinker { get; private set; }

        /// <summary>
        /// Gets a collection of constant buffers that are defined for this effect.
        /// </summary>
        public EffectConstantBufferCollection ConstantBuffers { get; private set; }

        /// <summary>
        /// Gets a collection of parameters that are defined for this effect.
        /// </summary>
        public EffectParameterCollection Parameters { get; private set; }

        /// <summary>
        /// Gets a collection of techniques that are defined for this effect.
        /// </summary>
        public EffectTechniqueCollection Techniques { get; private set; }

        /// <summary>
        /// Gets the data associated to this effect.
        /// </summary> 
        public EffectData.Effect RawEffectData { get; private set; }

        /// <summary>
        /// Set to <c>true</c> to force all constant shaders to be shared between other effects within a common <see cref="EffectPool"/>. Default is <c>false</c>.
        /// </summary>
        /// <remarks>
        /// This value can also be set in the TKFX file directly by setting ShareConstantBuffers = <c>true</c>; in a pass.
        /// </remarks>
        protected internal bool ShareConstantBuffers;

        /// <summary>
        /// Initializes a new instance of the <see cref="Effect"/> class with the specified bytecode effect. See remarks.
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
        /// <param name="effectData">The bytecode to add to the Effect Pool. This bytecode must contain only one effect.</param>
        /// <param name="effectPool">The effect pool used to register the bytecode. Default is <see cref="GraphicsDevice.DefaultEffectPool"/>.</param>
        /// <exception cref="ArgumentException">If the bytecode doesn't contain a single effect.</exception>
        /// <remarks>The effect bytecode must contain only a single effect and will be registered into the <see cref="GraphicsDevice.DefaultEffectPool"/>.</remarks>
        public Effect(GraphicsDevice device, EffectData effectData, EffectPool effectPool = null)
            : base(device)
        {
            CreateInstanceFrom(device, effectData, effectPool);

        }

        /// <summary>
        /// Gets the pool this effect attached to.
        /// </summary>
        /// <value> The pool. </value>
        public EffectPool Pool { get; private set; }

        /// <summary>
        /// Occurs when the on apply is applied on a pass.
        /// </summary>
        /// <remarks>
        /// This external hook provides a way to pre-configure a pipeline when a pass is applied.
        /// Subclass of this class can override the method <see cref="OnApply"/>.
        /// </remarks>
        public event OnApplyDelegate OnApplyCallback;

        /// <summary>
        /// Gets or sets the current technique. By default, it is set to the first available technique in this effect.
        /// </summary>
        /// <value>The current technique.</value>
        public EffectTechnique CurrentTechnique { get; set; }

        /// <summary>
        /// The effect is supporting dynamic compilation.
        /// </summary>
        public bool IsSupportingDynamicCompilation { get; private set; }

        /// <summary>
        /// The default parameters of this effect used by <see cref="Model.Draw"/>.
        /// </summary>
        public EffectDefaultParameters DefaultParameters { get; private set; }

        internal void CreateInstanceFrom(GraphicsDevice device, EffectData effectData, EffectPool effectPool)
        {
            GraphicsDevice = device;
            ConstantBuffers = new EffectConstantBufferCollection();
            Parameters = new EffectParameterCollection();
            Techniques = new EffectTechniqueCollection();

            Pool = effectPool ?? device.DefaultEffectPool;

            // Sets the effect name
            Name = effectData.Description.Name;

            // Register the bytecode to the pool
            var effect = Pool.RegisterBytecode(effectData);

            // Initialize from effect
            InitializeFrom(effect, null);

            // If everything was fine, then we can register it into the pool
            Pool.AddEffect(this);
        }

        /// <summary>
        /// Binds the specified effect data to this instance.
        /// </summary>
        /// <param name="effectDataArg">The effect data arg.</param>
        /// <param name="cloneFromEffect">The clone from effect.</param>
        /// <exception cref="System.InvalidOperationException">If no techniques found in this effect.</exception>
        /// <exception cref="System.ArgumentException">If unable to find effect [effectName] from the EffectPool.</exception>
        internal void InitializeFrom(EffectData.Effect effectDataArg, Effect cloneFromEffect)
        {
            RawEffectData = effectDataArg;

            // Clean any previously allocated resources
            if (DisposeCollector != null)
            {
                DisposeCollector.DisposeAndClear();
            }
            ConstantBuffers.Clear();
            Parameters.Clear();
            Techniques.Clear();
            ResourceLinker = ToDispose(new EffectResourceLinker());
            if (effectConstantBuffersCache != null)
            {
                effectConstantBuffersCache.Clear();
            }

            // Copy data
            IsSupportingDynamicCompilation = RawEffectData.Arguments != null;
            ShareConstantBuffers = RawEffectData.ShareConstantBuffers;

            // Create the local effect constant buffers cache
            if (!ShareConstantBuffers)
                effectConstantBuffersCache = new Dictionary<EffectConstantBufferKey, EffectConstantBuffer>();

            var logger = new Logger();
            int techniqueIndex = 0;
            int totalPassCount = 0;
            EffectPass parentPass = null;
            foreach (var techniqueRaw in RawEffectData.Techniques)
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
                            logger.Error("Pass [{0}] is declared as a subpass but has no parent.");
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
                throw new InvalidOperationException("No passes found in this effect.");

            // Log all the exception in a single throw
            if (logger.HasErrors)
                throw new InvalidOperationException(Utilities.Join("\n", logger.Messages));

            // Initialize the resource linker when we are done with all pass/parameters
            ResourceLinker.Initialize();

            //// Sort all parameters by their resource types
            //// in order to achieve better local cache coherency in resource linker
            Parameters.Items.Sort((left, right) =>
                {
                    // First, order first all value types, then resource type
                    var comparison = left.IsValueType != right.IsValueType ? left.IsValueType ? -1 : 1 : 0;

                    // If same type
                    if (comparison == 0)
                    {
                        // Order by resource type
                        comparison = ((int)left.ResourceType).CompareTo((int)right.ResourceType);

                        // If same, order by resource index
                        if (comparison == 0)
                        {
                            comparison = left.Offset.CompareTo(right.Offset);
                        }
                    }
                    return comparison;
                });

            // Prelink constant buffers
            int resourceIndex = 0;
            foreach (var parameter in Parameters)
            {
                // Recalculate parameter resource index
                if (!parameter.IsValueType)
                {
                    parameter.Offset = resourceIndex++;
                }

                // Set the default values 
                parameter.SetDefaultValue();

                if (parameter.ResourceType == EffectResourceType.ConstantBuffer)
                    parameter.SetResource(ConstantBuffers[parameter.Name]);
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

            // Initialize predefined parameters used by Model.Draw (to speedup things internally)
            DefaultParameters = new EffectDefaultParameters(this);

            // If this is a clone, we need to 
            if (cloneFromEffect != null)
            {
                // Copy the content of the constant buffers to the new instance.
                for (int i = 0; i < ConstantBuffers.Count; i++)
                {
                    cloneFromEffect.ConstantBuffers[i].CopyTo(ConstantBuffers[i]);
                }

                // Copy back all bound resources except constant buffers
                // that are already initialized with InitializeFrom method.
                for (int i = 0; i < cloneFromEffect.ResourceLinker.Count; i++)
                {
                    if (cloneFromEffect.ResourceLinker.BoundResources[i] is EffectConstantBuffer)
                        continue;

                    ResourceLinker.BoundResources[i] = cloneFromEffect.ResourceLinker.BoundResources[i];
                    unsafe
                    {
                        ResourceLinker.Pointers[i] = cloneFromEffect.ResourceLinker.Pointers[i];
                    }
                }

                // If everything was fine, then we can register it into the pool
                Pool.AddEffect(this);
            }

            // Allow subclasses to complete initialization.
            Initialize();

            OnInitialized();
        }

        /// <summary>Initializes this instance.</summary>
        protected virtual void Initialize()
        {
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>A new instance of this Effect.</returns>
        public virtual Effect Clone()
        {
            var effect = (Effect)MemberwiseClone();
            effect.DisposeCollector = new DisposeCollector();
            effect.ConstantBuffers = new EffectConstantBufferCollection();
            effect.Parameters = new EffectParameterCollection();
            effect.Techniques = new EffectTechniqueCollection();
            effect.effectConstantBuffersCache = null;

            // Initialize from effect
            effect.InitializeFrom(effect.RawEffectData, this);

            return effect;
        }

        /// <summary>Gets or sets the disposables.</summary>
        /// <value>The disposables.</value>
        internal new DisposeCollector DisposeCollector
        {
            get { return base.DisposeCollector; }
            private set { base.DisposeCollector = value; }
        }

        /// <summary>Called when [apply].</summary>
        /// <param name="pass">The pass.</param>
        /// <returns>EffectPass.</returns>
        protected internal virtual EffectPass OnApply(EffectPass pass)
        {
            var handler = OnApplyCallback;
            if (handler != null) pass = handler(pass);

            return pass;
        }

        /// <summary>Releases unmanaged and - optionally - managed resources.</summary>
        /// <param name="disposeManagedResources"><see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposeManagedResources)
        {
            // Remove this instance from the pool
            Pool.RemoveEffect(this);

            base.Dispose(disposeManagedResources);
        }

        /// <summary>Gets the original create constant buffer.</summary>
        /// <param name="context">The context.</param>
        /// <param name="bufferRaw">The buffer raw.</param>
        /// <returns>EffectConstantBuffer.</returns>
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

        /// <summary>Called when [initialized].</summary>
        protected virtual void OnInitialized()
        {
            EventHandler<EventArgs> handler = Initialized;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
