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
using SharpDX.Toolkit.Diagnostics;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Main class to apply shader effects.
    /// </summary>
    public class Effect : Component
    {
        public delegate EffectPass OnApplyDelegate(EffectPass pass);

        private Dictionary<EffectConstantBufferKey, EffectConstantBuffer> effectConstantBuffersCache;

        /// <summary>
        ///   Gets a collection of constant buffers that are defined for this effect.
        /// </summary>
        public readonly EffectConstantBufferCollection ConstantBuffers;

        internal readonly GraphicsDevice GraphicsDevice;

        /// <summary>
        ///   Gets a collection of parameters that are defined for this effect.
        /// </summary>
        public readonly EffectParameterCollection Parameters;

        internal readonly EffectResourceLinker ResourceLinker;

        /// <summary>
        ///   Gets a collection of techniques that are defined for this effect.
        /// </summary>
        public readonly EffectTechniqueCollection Techniques;

        private readonly EffectGroup group;
        private EffectData.Effect effectData;

        /// <summary>
        /// Set to true to force all constant shaders to be shared between other effects within a common <see cref="EffectGroup"/>. Default is false.
        /// </summary>
        /// <remarks>
        /// This value can also be set in the TKFX file directly by setting ShareConstantBuffers = true; in a pass.
        /// </remarks>
        protected internal bool ShareConstantBuffers;

        /// <summary>
        /// Initializes a new instance of the <see cref="Effect" /> class with the specified effect. See remarks.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="effectName">Name of the effect.</param>
        /// <remarks>
        /// The effect must have been loaded and registered into the <see cref="Graphics.GraphicsDevice.DefaultEffectGroup"/>.
        /// </remarks>
        public Effect(GraphicsDevice device, string effectName) : this(device, device.DefaultEffectGroup, effectName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Effect" /> class with the specified effect loaded from an effect group.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="group">The effect group.</param>
        /// <param name="effectName">Name of the effect.</param>
        public Effect(GraphicsDevice device, EffectGroup group, string effectName) : base(effectName)
        {
            GraphicsDevice = device;
            ConstantBuffers = new EffectConstantBufferCollection();
            Parameters = new EffectParameterCollection();
            Techniques = new EffectTechniqueCollection();
            ResourceLinker = ToDispose(new EffectResourceLinker());
            this.group = group;
            Initialize();
        }

        /// <summary>
        ///   Gets the group this effect attached to.
        /// </summary>
        /// <value> The group. </value>
        public EffectGroup Group
        {
            get { return group; }
        }

        /// <summary>
        ///   Occurs when the on apply is applied on a pass.
        /// </summary>
        /// <remarks>
        ///   This external hook provides a way to pre-configure a pipeline when a pass is applied.
        ///   Subclass of this class can override the method <see cref="OnApply" />.
        /// </remarks>
        public event OnApplyDelegate OnApplyCallback;

        protected virtual void Initialize()
        {
            Initialize(group.Find(Name));

            // If everything was fine, then we can register it into the group
            group.AddEffect(this);
        }

        /// <summary>
        /// Initializes the specified effect bytecode.
        /// </summary>
        /// <param name="effectDataArg">The effect bytecode.</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        private void Initialize(EffectData.Effect effectDataArg)
        {
            if (effectDataArg == null)
                throw new ArgumentException(string.Format("Unable to find effect [{0}] from the EffectGroup", Name), "effectName");

            effectData = effectDataArg;

            ShareConstantBuffers = effectDataArg.ShareConstantBuffers;

            // Create the local effect constant buffers cache
            if (!ShareConstantBuffers)
                effectConstantBuffersCache = new Dictionary<EffectConstantBufferKey, EffectConstantBuffer>();

            var logger = new Logger();
            int techniqueIndex = 0;
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

                    var pass = new EffectPass(logger, this, passRaw, name);

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
            }

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
        }

        internal new DisposeCollector DisposeCollector
        {
            get { return base.DisposeCollector; }
        }

        protected internal virtual EffectPass OnApply(EffectPass pass)
        {
            var handler = OnApplyCallback;
            if (handler != null) return handler(pass);
            return pass;
        }

        protected override void Dispose(bool disposeManagedResources)
        {
            // Remove this instance from the group
            Group.RemoveEffect(this);

            base.Dispose(disposeManagedResources);
        }

        internal EffectConstantBuffer GetOrCreateConstantBuffer(GraphicsDevice context, EffectData.ConstantBuffer bufferRaw)
        {
            EffectConstantBuffer constantBuffer;
            // Is the effect is using shared constant buffers via the EffectGroup?
            if (ShareConstantBuffers)
            {
                // Use the group to share constant buffers
                constantBuffer = Group.GetOrCreateConstantBuffer(context, bufferRaw);
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