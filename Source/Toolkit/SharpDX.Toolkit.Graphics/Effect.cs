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
using SharpDX.Toolkit.Diagnostics;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Main class to apply shader effects.
    /// </summary>
    public class Effect : Component
    {
        public delegate EffectPass OnApplyDelegate(EffectPass pass);

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
        private EffectData.Effect effectBytecode;

        internal bool ShareConstantBuffers;

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
            Initialize(effectName);
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

        protected virtual void PrepareGroup()
        {
        }

        private void Initialize(string effectName)
        {
            PrepareGroup();
            var effectRaw = group.Find(effectName);
            if (effectRaw == null)
                throw new ArgumentException(string.Format("Unable to find effect [{0}] from the EFfectGroup", effectName), "effectName");
            Initialize(effectRaw);

            // If everything was fine, then we can register it into the group
            group.AddEffect(this);
        }

        /// <summary>
        /// Initializes the specified effect bytecode.
        /// </summary>
        /// <param name="effectBytecodeArg">The effect bytecode.</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        private void Initialize(EffectData.Effect effectBytecodeArg)
        {
            effectBytecode = effectBytecodeArg;

            ShareConstantBuffers = effectBytecodeArg.ShareConstantBuffers;

            var logger = new Logger();
            int techniqueIndex = 0;
            EffectPass parentPass = null;
            foreach (var techniqueRaw in effectBytecodeArg.Techniques)
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
    }
}