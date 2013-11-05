﻿// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
using System.Collections.ObjectModel;

using SharpDX.Collections;
using SharpDX.Direct3D11;
using SharpDX.Toolkit.Diagnostics;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>This class manages a pool of <see cref="Effect" />.</summary>
    /// <remarks>This class is responsible to store all EffectData, create shareable constant buffers between effects and reuse shader EffectData instances.</remarks>
    public sealed class EffectPool : Component
    {
        #region Delegates

        /// <summary>The constant buffer allocator delegate delegate.</summary>
        /// <param name="device">The device.</param>
        /// <param name="pool">The pool.</param>
        /// <param name="constantBuffer">The constant buffer.</param>
        /// <returns>Buffer.</returns>
        public delegate Buffer ConstantBufferAllocatorDelegate(GraphicsDevice device, EffectPool pool, EffectConstantBuffer constantBuffer);

        #endregion

        /// <summary>The registered shaders.</summary>
        internal readonly List<EffectData.Shader> RegisteredShaders;
        /// <summary>The compiled shaders group.</summary>
        private readonly List<DeviceChild>[] compiledShadersGroup;
        /// <summary>The graphics device.</summary>
        private readonly GraphicsDevice graphicsDevice;
        /// <summary>The effects.</summary>
        private readonly List<Effect> effects;

        // GraphicsDevice => (ConstantBufferName => (EffectConstantBufferKey => EffectConstantBuffer))
        /// <summary>The map name automatic constant buffer.</summary>
        private readonly Dictionary<GraphicsDevice, Dictionary<string, Dictionary<EffectConstantBufferKey, EffectConstantBuffer>>> mapNameToConstantBuffer;
        /// <summary>The registered.</summary>
        private readonly Dictionary<EffectData, EffectData.Effect> registered;
        /// <summary>The asynchronous.</summary>
        private readonly object sync = new object();
        /// <summary>The constant buffer allocator.</summary>
        private ConstantBufferAllocatorDelegate constantBufferAllocator;


        /// <summary>Initializes a new instance of the <see cref="EffectPool"/> class.</summary>
        /// <param name="device">The device.</param>
        /// <param name="name">The name.</param>
        private EffectPool(GraphicsDevice device, string name = null) : base(name)
        {
            RegisteredShaders = new List<EffectData.Shader>();
            mapNameToConstantBuffer = new Dictionary<GraphicsDevice, Dictionary<string, Dictionary<EffectConstantBufferKey, EffectConstantBuffer>>>();
            compiledShadersGroup = new List<DeviceChild>[(int)EffectShaderType.Compute + 1];
            for (int i = 0; i < compiledShadersGroup.Length; i++)
            {
                compiledShadersGroup[i] = new List<DeviceChild>(256);
            }

            registered = new Dictionary<EffectData, EffectData.Effect>(new IdentityEqualityComparer<EffectData>());
            effects = new List<Effect>();
            RegisteredEffects = new ReadOnlyCollection<Effect>(effects);
            this.graphicsDevice = device.MainDevice;
            constantBufferAllocator = DefaultConstantBufferAllocator;
            graphicsDevice.EffectPools.Add(this);
        }

        /// <summary>
        ///   Gets or sets the constant buffer allocator used to allocate a GPU constant buffer declared in an Effect.
        /// </summary>
        /// <remarks>
        ///   This delegate must be overridden when you want to control the creation of the GPU Constant buffer.
        ///   By default, the allocator is just allocating the buffer using "Buffer.Constant.New(size)" but
        ///   It is sometimes needed to create a constant buffer with different usage scenarios (using for example
        ///   a RawBuffer with multiple usages).
        ///   Setting this property to null will revert the default allocator.
        /// </remarks>
        public ConstantBufferAllocatorDelegate ConstantBufferAllocator
        {
            get
            {
                lock (mapNameToConstantBuffer)
                {
                    return constantBufferAllocator;
                }
            }
            set
            {
                lock (mapNameToConstantBuffer)
                {
                    constantBufferAllocator = value ?? DefaultConstantBufferAllocator;
                }
            }
        }

        /// <summary>
        /// Registers a EffectData to this pool.
        /// </summary>
        /// <param name="data">The effect data to register.</param>
        /// <returns>The effect description.</returns>
        public EffectData.Effect RegisterBytecode(EffectData data)
        {
            // Lock the whole EffectPool in case multiple threads would add EffectData at the same time.
            lock (sync)
            {
                EffectData.Effect effect;

                if (!registered.TryGetValue(data, out effect))
                {
                    // Pre-cache all input signatures
                    CacheInputSignature(data);

                    effect = RegisterInternal(data);
                    registered.Add(data, effect);

                    // Just allocate the compiled shaders array according to the current size of shader data
                    foreach (var compiledShaders in compiledShadersGroup)
                    {
                        for (int i = compiledShaders.Count; i < RegisteredShaders.Count; i++)
                        {
                            compiledShaders.Add(null);
                        }
                    }
                }

                return effect;
            }
        }

        /// <summary>Caches the input signature.</summary>
        /// <param name="effectData">The effect data.</param>
        private void CacheInputSignature(EffectData effectData)
        {
            // Iterate on all vertex shaders and make unique the bytecode
            // for faster comparison when creating input layout.
            foreach (var shader in effectData.Shaders)
            {
                if (shader.Type == EffectShaderType.Vertex && shader.InputSignature.Bytecode != null)
                {
                    var inputSignature = graphicsDevice.GetOrCreateInputSignatureManager(shader.InputSignature.Bytecode, shader.InputSignature.Hashcode);
                    shader.InputSignature.Bytecode = inputSignature.Bytecode;
                }
            }
        }

        /// <summary>The registered effects.</summary>
        public readonly ReadOnlyCollection<Effect> RegisteredEffects;

        /// <summary>Occurs when [effect added].</summary>
        public event EventHandler<ObservableCollectionEventArgs<Effect>> EffectAdded;

        /// <summary>Occurs when [effect removed].</summary>
        public event EventHandler<ObservableCollectionEventArgs<Effect>> EffectRemoved;

        /// <summary>Adds the effect.</summary>
        /// <param name="effect">The effect.</param>
        internal void AddEffect(Effect effect)
        {
            lock (effects)
            {
                this.effects.Add(effect);
            }
            OnEffectAdded(new ObservableCollectionEventArgs<Effect>(effect));
        }

        /// <summary>Removes the effect.</summary>
        /// <param name="effect">The effect.</param>
        internal void RemoveEffect(Effect effect)
        {
            lock (effects)
            {
                this.effects.Remove(effect);
            }
            OnEffectRemoved(new ObservableCollectionEventArgs<Effect>(effect));
        }

        /// <summary>Gets the original compile shader.</summary>
        /// <param name="shaderType">Type of the shader.</param>
        /// <param name="index">The index.</param>
        /// <param name="soRasterizedStream">The so rasterized stream.</param>
        /// <param name="soElements">The so elements.</param>
        /// <param name="profileError">The profile error.</param>
        /// <returns>DeviceChild.</returns>
        internal DeviceChild GetOrCompileShader(EffectShaderType shaderType, int index, int soRasterizedStream, StreamOutputElement[] soElements, out string profileError)
        {
            DeviceChild shader;
            profileError = null;
            lock (sync)
            {
                shader = compiledShadersGroup[(int)shaderType][index];
                if (shader == null)
                {
                    if (RegisteredShaders[index].Level > graphicsDevice.Features.Level)
                    {
                        profileError = string.Format("{0}", RegisteredShaders[index].Level);
                        return null;
                    }

                    var bytecodeRaw = RegisteredShaders[index].Bytecode;
                    switch (shaderType)
                    {
                        case EffectShaderType.Vertex:
                            shader = new VertexShader(graphicsDevice, bytecodeRaw);
                            break;
                        case EffectShaderType.Domain:
                            shader = new DomainShader(graphicsDevice, bytecodeRaw);
                            break;
                        case EffectShaderType.Hull:
                            shader = new HullShader(graphicsDevice, bytecodeRaw);
                            break;
                        case EffectShaderType.Geometry:
                            if (soElements != null)
                            {
                                // Calculate the strides
                                var soStrides = new List<int>();
                                foreach (var streamOutputElement in soElements)
                                {
                                    for (int i = soStrides.Count; i < (streamOutputElement.Stream+1); i++)
                                    {
                                        soStrides.Add(0);
                                    }

                                    soStrides[streamOutputElement.Stream] += streamOutputElement.ComponentCount * sizeof(float);
                                }
                                shader = new GeometryShader(graphicsDevice, bytecodeRaw, soElements, soStrides.ToArray(), soRasterizedStream);
                            }
                            else
                            {
                                shader = new GeometryShader(graphicsDevice, bytecodeRaw);
                            }
                            break;
                        case EffectShaderType.Pixel:
                            shader = new PixelShader(graphicsDevice, bytecodeRaw);
                            break;
                        case EffectShaderType.Compute:
                            shader = new ComputeShader(graphicsDevice, bytecodeRaw);
                            break;
                    }
                    compiledShadersGroup[(int)shaderType][index] = ToDispose(shader);
                }
            }
            return shader;
        }

        /// <summary>Gets the original create constant buffer.</summary>
        /// <param name="context">The context.</param>
        /// <param name="bufferRaw">The buffer raw.</param>
        /// <returns>EffectConstantBuffer.</returns>
        internal EffectConstantBuffer GetOrCreateConstantBuffer(GraphicsDevice context, EffectData.ConstantBuffer bufferRaw)
        {
            // Only lock the constant buffer object
            lock (mapNameToConstantBuffer)
            {
                Dictionary<string, Dictionary<EffectConstantBufferKey, EffectConstantBuffer>> nameToConstantBufferList;

                // ----------------------------------------------------------------------------
                // 1) Get the cache of constant buffers for a particular GraphicsDevice
                // ----------------------------------------------------------------------------
                // TODO cache is not clear if a GraphicsDevice context is disposed
                // To simplify, we assume that a GraphicsDevice is alive during the whole life of the application.
                if (!mapNameToConstantBuffer.TryGetValue(context, out nameToConstantBufferList))
                {
                    nameToConstantBufferList = new Dictionary<string, Dictionary<EffectConstantBufferKey, EffectConstantBuffer>>();
                    mapNameToConstantBuffer[context] = nameToConstantBufferList;
                }

                // ----------------------------------------------------------------------------
                // 2) Get a set of constant buffers for a particular constant buffer name
                // ----------------------------------------------------------------------------
                Dictionary<EffectConstantBufferKey, EffectConstantBuffer> bufferSet;
                if (!nameToConstantBufferList.TryGetValue(bufferRaw.Name, out bufferSet))
                {
                    bufferSet = new Dictionary<EffectConstantBufferKey, EffectConstantBuffer>();
                    nameToConstantBufferList[bufferRaw.Name] = bufferSet;
                }

                // ----------------------------------------------------------------------------
                // 3) Get an existing constant buffer having the same name/size/layout/parameters
                // ----------------------------------------------------------------------------
                var bufferKey = new EffectConstantBufferKey(bufferRaw);
                EffectConstantBuffer buffer;
                if (!bufferSet.TryGetValue(bufferKey, out buffer))
                {
                    // 4) If this buffer doesn't exist, create a new one and register it.
                    buffer = new EffectConstantBuffer(graphicsDevice, bufferRaw);
                    bufferSet[bufferKey] = ToDispose(buffer);
                }

                return buffer;
            }
        }

        /// <summary>Creates a new effect pool from a specified list of <see cref="EffectData" />.</summary>
        /// <param name="device">The device.</param>
        /// <returns>An instance of <see cref="EffectPool" />.</returns>
        public static EffectPool New(GraphicsDevice device)
        {
            return new EffectPool(device);
        }

        /// <summary>Creates a new named effect pool from a specified list of <see cref="EffectData" />.</summary>
        /// <param name="device">The device.</param>
        /// <param name="name">The name of this effect pool.</param>
        /// <returns>An instance of <see cref="EffectPool" />.</returns>
        public static EffectPool New(GraphicsDevice device, string name)
        {
            return new EffectPool(device);
        }

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("EffectPool [{0}]", Name);
        }

        /// <summary>Disposes of object resources.</summary>
        /// <param name="disposeManagedResources">If true, managed resources should be
        /// disposed of in addition to unmanaged resources.</param>
        protected override void Dispose(bool disposeManagedResources)
        {
            base.Dispose(disposeManagedResources);

            if(disposeManagedResources)
                graphicsDevice.EffectPools.Remove(this);
        }

        /// <summary>Merges an existing <see cref="EffectData" /> into this instance.</summary>
        /// <param name="source">The EffectData to merge.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <remarks>This method is useful to build an archive of several effects.</remarks>
        private EffectData.Effect RegisterInternal(EffectData source)
        {
            var effect = source.Description;

            var effectRuntime = new EffectData.Effect {
                                        Name = effect.Name,
                                        Arguments = effect.Arguments,
                                        ShareConstantBuffers = effect.ShareConstantBuffers,
                                        Techniques = new List<EffectData.Technique>(effect.Techniques.Count)
                                    };

            Logger logger = null;

            foreach (var techniqueOriginal in effect.Techniques)
            {
                var technique = techniqueOriginal.Clone();
                effectRuntime.Techniques.Add(technique);

                foreach (var pass in technique.Passes)
                {
                    foreach (var shaderLink in pass.Pipeline)
                    {
                        // No shader set for this stage
                        if (shaderLink == null) continue;

                        // If the shader is an import, we try first to resolve it directly
                        if (shaderLink.IsImport)
                        {
                            var index = FindShaderByName(shaderLink.ImportName);
                            if (index >= 0)
                            {
                                shaderLink.ImportName = null;
                                shaderLink.Index = index;
                            }
                            else
                            {
                                if (logger == null)
                                {
                                    logger = new Logger();
                                }

                                logger.Error("Cannot find shader import by name [{0}]", shaderLink.ImportName);
                            }
                        }
                        else if (!shaderLink.IsNullShader)
                        {
                            var shader = source.Shaders[shaderLink.Index];

                            // Find a similar shader
                            var shaderIndex = FindSimilarShader(shader);

                            if (shaderIndex >= 0)
                            {
                                var previousShader = RegisteredShaders[shaderIndex];

                                // If the previous shader is 
                                if (shader.Name != null)
                                {
                                    // if shader from this instance is local and shader from source is global => transform current shader to global
                                    if (previousShader.Name == null)
                                    {
                                        previousShader.Name = shader.Name;
                                    }
                                    else if (shader.Name != previousShader.Name)
                                    {
                                        if (logger == null)
                                        {
                                            logger = new Logger();
                                        }
                                        // If shader from this instance is global and shader from source is global => check names. If exported names are different, this is an error
                                        logger.Error("Cannot merge shader [{0}] into this instance, as there is already a global shader with a different name [{1}]", shader.Name, previousShader.Name);
                                    }
                                }

                                shaderLink.Index = shaderIndex;
                            }
                            else
                            {
                                shaderLink.Index = RegisteredShaders.Count;
                                RegisteredShaders.Add(shader);
                            }
                        }
                    }
                }
            }

            if (logger != null && logger.HasErrors)
                throw new InvalidOperationException(Utilities.Join("\r\n", logger.Messages));

            return effectRuntime;
        }

        /// <summary>Finds the similar shader.</summary>
        /// <param name="shader">The shader.</param>
        /// <returns>System.Int32.</returns>
        private int FindSimilarShader(EffectData.Shader shader)
        {
            for (int i = 0; i < RegisteredShaders.Count; i++)
            {
                if (RegisteredShaders[i].IsSimilar(shader))
                    return i;
            }
            return -1;
        }

        /// <summary>Finds the name of the shader by.</summary>
        /// <param name="name">The name.</param>
        /// <returns>System.Int32.</returns>
        private int FindShaderByName(string name)
        {
            for (int i = 0; i < RegisteredShaders.Count; i++)
            {
                if (RegisteredShaders[i].Name == name)
                    return i;
            }
            return -1;

        }

        /// <summary>Defaults the constant buffer allocator.</summary>
        /// <param name="device">The device.</param>
        /// <param name="pool">The pool.</param>
        /// <param name="constantBuffer">The constant buffer.</param>
        /// <returns>Buffer.</returns>
        private static Buffer DefaultConstantBufferAllocator(GraphicsDevice device, EffectPool pool, EffectConstantBuffer constantBuffer)
        {
            return Buffer.Constant.New(device, constantBuffer.Size);
        }

        /// <summary>Called when [effect added].</summary>
        /// <param name="e">The decimal.</param>
        private void OnEffectAdded(ObservableCollectionEventArgs<Effect> e)
        {
            EventHandler<ObservableCollectionEventArgs<Effect>> handler = EffectAdded;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>Called when [effect removed].</summary>
        /// <param name="e">The decimal.</param>
        private void OnEffectRemoved(ObservableCollectionEventArgs<Effect> e)
        {
            EventHandler<ObservableCollectionEventArgs<Effect>> handler = EffectRemoved;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /*
        private class CompileShader
        {
            public SharpDX.Direct3D11.DeviceChild Shader;

            public SharpDX.Direct3D11.DeviceChild GeometryShader;
        }
        */
    }
}