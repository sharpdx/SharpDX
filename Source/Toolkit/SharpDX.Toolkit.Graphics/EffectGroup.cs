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

using System.Collections.Generic;
using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// This class manages multiple effects.
    /// </summary>
    /// <remarks>
    /// This class is responsible to store all EffectData, create shareable constant buffers betwen effects and reuse shader EffectData instances.
    /// </remarks>
    public sealed class EffectGroup : Component
    {
        #region Delegates

        public delegate Buffer ConstantBufferAllocatorDelegate(GraphicsDevice device, EffectGroup group, EffectConstantBuffer constantBuffer);

        #endregion


        private readonly EffectData dataGroup;
        private readonly List<SharpDX.Direct3D11.DeviceChild> compiledShaders;
        private readonly GraphicsDevice graphicsDevice;
        private readonly List<Effect> effects;

        // GraphicsDevice => (ConstantBufferName => (EffectConstantBufferKey => EffectConstantBuffer))
        private readonly Dictionary<GraphicsDevice, Dictionary<string, Dictionary<EffectConstantBufferKey, EffectConstantBuffer>>> mapNameToConstantBuffer;
        private readonly Dictionary<string, EffectData.Effect> mapNameToEffect;
        private readonly Dictionary<EffectData, bool> registered;
        private readonly object sync = new object();
        private ConstantBufferAllocatorDelegate constantBufferAllocator;


        private EffectGroup(GraphicsDevice device, string name = null) : base(name)
        {
            dataGroup = new EffectData();
            mapNameToEffect = new Dictionary<string, EffectData.Effect>();
            mapNameToConstantBuffer = new Dictionary<GraphicsDevice, Dictionary<string, Dictionary<EffectConstantBufferKey, EffectConstantBuffer>>>();
            compiledShaders = new List<DeviceChild>();
            registered = new Dictionary<EffectData, bool>(new IdentityEqualityComparer<EffectData>());
            effects = new List<Effect>();
            this.graphicsDevice = device.MainDevice;
            constantBufferAllocator = DefaultConstantBufferAllocator;
        }

        /// <summary>
        ///   Gets or sets the constant buffer allocator used to allocate a GPU constant buffer declared in an Effect.
        /// </summary>
        /// <remarks>
        ///   This delegate must be overriden when you want to control the creation of the GPU Constant buffer.
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
        /// Gets the current merged EffectData for this group. See remarks.
        /// </summary>
        /// <value>The EffectData.</value>
        /// <remarks>
        /// This EffectData must not be modified at runtime.
        /// </remarks>
        public EffectData EffectData
        {
            get { return dataGroup; }
        }

        /// <summary>
        /// Registers a EffectData to this group.
        /// </summary>
        /// <param name="datas">The datas to register.</param>
        public void RegisterBytecode(params EffectData[] datas)
        {
            // Lock the whole EffectGroup in case multiple threads would add EffectData at the same time.
            lock (sync)
            {
                bool hasNewBytecode = false;
                foreach (var bytecode in datas)
                {
                    if (!registered.ContainsKey(bytecode))
                    {
                        // Pre-cache all input signatures
                        CacheInputSignature(bytecode);

                        dataGroup.MergeFrom(bytecode);
                        registered.Add(bytecode, true);
                        hasNewBytecode = true;
                    }
                }

                if (hasNewBytecode)
                {
                    // Create all mapping
                    mapNameToEffect.Clear();
                    foreach (var effect in dataGroup.Effects)
                        mapNameToEffect.Add(effect.Name, effect);

                    // Just alocate the compiled shaders array according to the currennt size of shader datas
                    for (int i = compiledShaders.Count; i < dataGroup.Shaders.Count; i++)
                    {
                        compiledShaders.Add(null);
                    }
                }
            }
        }

        private void CacheInputSignature(EffectData effectData)
        {
            // Iterate on all vertex shaders and make unique the bytecode
            // for faster comparison when creating input layout.
            foreach (var shader in effectData.Shaders)
            {
                if (shader.Type == EffectShaderType.Vertex && shader.InputSignature.Bytecode != null)
                {
                    var signature = graphicsDevice.GetOrCreateInputSignatureManager(shader.InputSignature.Bytecode, shader.InputSignature.Hashcode);
                    shader.Bytecode = signature.Bytecode;
                }
            }
        }

        internal void AddEffect(Effect effect)
        {
            lock (effects)
            {
                this.effects.Add(effect);
            }
        }

        internal void RemoveEffect(Effect effect)
        {
            lock (effects)
            {
                this.effects.Remove(effect);
            }
        }

        internal EffectData.Effect Find(string name)
        {
            EffectData.Effect rawEffect;
            lock (sync)
            {
                mapNameToEffect.TryGetValue(name, out rawEffect);
            }

            return rawEffect;
        }

        internal DeviceChild GetOrCompileShader(EffectShaderType shaderType, int index)
        {
            DeviceChild shader = null;
            lock (sync)
            {
                shader = compiledShaders[index];
                if (shader == null)
                {
                    var bytecodeRaw = dataGroup.Shaders[index].Bytecode;
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
                            shader = new GeometryShader(graphicsDevice, bytecodeRaw);
                            break;
                        case EffectShaderType.Pixel:
                            shader = new PixelShader(graphicsDevice, bytecodeRaw);
                            break;
                        case EffectShaderType.Compute:
                            shader = new ComputeShader(graphicsDevice, bytecodeRaw);
                            break;
                    }
                    compiledShaders[index] = shader;
                }
            }
            return shader;
        }

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
                // 3) Get an existing constant buffer having the same name/sizel/ayout/parameters
                // ----------------------------------------------------------------------------
                var bufferKey = new EffectConstantBufferKey(bufferRaw);
                EffectConstantBuffer buffer;
                if (!bufferSet.TryGetValue(bufferKey, out buffer))
                {
                    // 4) If this buffer doesn't exist, create a new one and register it.
                    buffer = new EffectConstantBuffer(graphicsDevice, bufferRaw);
                    bufferSet[bufferKey] = buffer;
                }

                return buffer;
            }
        }

        /// <summary>
        /// Creates a new effect group from a specified list of <see cref="EffectData"/>.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="datas">The datas.</param>
        /// <returns>An instance of <see cref="EffectGroup"/>.</returns>
        public static EffectGroup New(GraphicsDevice device, params EffectData[] datas)
        {
            var group = new EffectGroup(device);
            group.RegisterBytecode(datas);
            return group;
        }

        /// <summary>
        /// Creates a new named effect group from a specified list of <see cref="EffectData" />.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="name">The name of this effect group.</param>
        /// <param name="datas">The datas.</param>
        /// <returns>An instance of <see cref="EffectGroup" />.</returns>
        public static EffectGroup New(GraphicsDevice device, string name, params EffectData[] datas)
        {
            var group = new EffectGroup(device);
            group.RegisterBytecode(datas);
            return group;
        }

        public override string ToString()
        {
            return string.Format("EffectGroup [{0}]", Name);
        }

        private static Buffer DefaultConstantBufferAllocator(GraphicsDevice device, EffectGroup group, EffectConstantBuffer constantBuffer)
        {
            return Buffer.Cosntant.New(device, constantBuffer.Size);
        }

        #region Nested type: EffectConstantBufferKey

        #endregion
    }
}