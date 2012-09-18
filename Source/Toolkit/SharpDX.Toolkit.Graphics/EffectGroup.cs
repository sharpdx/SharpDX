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
using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// This class manages multiple effects.
    /// </summary>
    /// <remarks>
    /// This class is responsible to store all bytecode, create shareable constant buffers betwen effects and reuse shader bytecode instances.
    /// </remarks>
    public class EffectGroup : Component
    {
        #region Delegates

        public delegate Buffer ConstantBufferAllocatorDelegate(GraphicsDevice device, EffectGroup group, EffectConstantBuffer constantBuffer);

        #endregion

        private readonly EffectBytecode bytecodeGroup;
        private readonly List<SharpDX.Direct3D11.DeviceChild> compiledShaders;
        private readonly GraphicsDevice device;
        private readonly List<Effect> effects;

        // GraphicsDevice => (ConstantBufferName => (ConstantBufferKey => EffectConstantBuffer))
        private readonly Dictionary<GraphicsDevice, Dictionary<string, Dictionary<ConstantBufferKey, EffectConstantBuffer>>> mapNameToConstantBuffer;
        private readonly Dictionary<string, EffectBytecode.Effect> mapNameToEffect;
        private readonly Dictionary<EffectBytecode, bool> registered;
        private readonly object sync = new object();
        private ConstantBufferAllocatorDelegate constantBufferAllocator;


        private EffectGroup(GraphicsDevice device)
        {
            bytecodeGroup = new EffectBytecode();
            mapNameToEffect = new Dictionary<string, EffectBytecode.Effect>();
            mapNameToConstantBuffer = new Dictionary<GraphicsDevice, Dictionary<string, Dictionary<ConstantBufferKey, EffectConstantBuffer>>>();
            compiledShaders = new List<DeviceChild>();
            registered = new Dictionary<EffectBytecode, bool>(new IdentityEqualityComparer<EffectBytecode>());
            effects = new List<Effect>();
            this.device = device.MainDevice;
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
        /// Gets the current merged bytecode for this group. See remarks.
        /// </summary>
        /// <value>The bytecode.</value>
        /// <remarks>
        /// This bytecode must not be modified at runtime.
        /// </remarks>
        public EffectBytecode Bytecode
        {
            get { return bytecodeGroup; }
        }

        /// <summary>
        /// Registers a bytecode to this group.
        /// </summary>
        /// <param name="bytecodes">The bytecodes to register.</param>
        public void RegisterBytecode(params EffectBytecode[] bytecodes)
        {
            // Lock the whole EffectGroup in case multiple threads would add bytecode at the same time.
            lock (sync)
            {
                bool hasNewBytecode = false;
                foreach (var bytecode in bytecodes)
                {
                    if (!registered.ContainsKey(bytecode))
                    {
                        bytecodeGroup.MergeFrom(bytecode);
                        registered.Add(bytecode, true);
                        hasNewBytecode = true;
                    }
                }

                if (hasNewBytecode)
                {
                    // Create all mapping
                    mapNameToEffect.Clear();
                    foreach (var effect in bytecodeGroup.Effects)
                        mapNameToEffect.Add(effect.Name, effect);

                    // Just alocate the compiled shaders array according to the currennt size of shader bytecodes
                    for (int i = compiledShaders.Count; i < bytecodeGroup.Shaders.Count; i++)
                    {
                        compiledShaders.Add(null);
                    }
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

        internal EffectBytecode.Effect Find(string name)
        {
            EffectBytecode.Effect rawEffect;
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
                    var bytecodeRaw = bytecodeGroup.Shaders[index].Bytecode;
                    switch (shaderType)
                    {
                        case EffectShaderType.Vertex:
                            shader = new VertexShader(device, bytecodeRaw);
                            break;
                        case EffectShaderType.Domain:
                            shader = new DomainShader(device, bytecodeRaw);
                            break;
                        case EffectShaderType.Hull:
                            shader = new HullShader(device, bytecodeRaw);
                            break;
                        case EffectShaderType.Geometry:
                            shader = new GeometryShader(device, bytecodeRaw);
                            break;
                        case EffectShaderType.Pixel:
                            shader = new PixelShader(device, bytecodeRaw);
                            break;
                        case EffectShaderType.Compute:
                            shader = new ComputeShader(device, bytecodeRaw);
                            break;
                    }
                    compiledShaders[index] = shader;
                }
            }
            return shader;
        }

        internal EffectConstantBuffer GetOrCreateConstantBuffer(GraphicsDevice context, EffectBytecode.ConstantBuffer bufferRaw)
        {
            // Only lock the constant buffer object
            lock (mapNameToConstantBuffer)
            {
                Dictionary<string, Dictionary<ConstantBufferKey, EffectConstantBuffer>> nameToConstantBufferList;

                // ----------------------------------------------------------------------------
                // 1) Get the cache of constant buffers for a particular GraphicsDevice
                // ----------------------------------------------------------------------------
                // TODO cache is not clear if a GraphicsDevice context is disposed
                // To simplify, we assume that a GraphicsDevice is alive during the whole life of the application.
                if (!mapNameToConstantBuffer.TryGetValue(context, out nameToConstantBufferList))
                {
                    nameToConstantBufferList = new Dictionary<string, Dictionary<ConstantBufferKey, EffectConstantBuffer>>();
                    mapNameToConstantBuffer[context] = nameToConstantBufferList;
                }

                // ----------------------------------------------------------------------------
                // 2) Get a set of constant buffers for a particular constant buffer name
                // ----------------------------------------------------------------------------
                Dictionary<ConstantBufferKey, EffectConstantBuffer> bufferSet;
                if (!nameToConstantBufferList.TryGetValue(bufferRaw.Name, out bufferSet))
                {
                    bufferSet = new Dictionary<ConstantBufferKey, EffectConstantBuffer>();
                    nameToConstantBufferList[bufferRaw.Name] = bufferSet;
                }

                // ----------------------------------------------------------------------------
                // 3) Get an existing constant buffer having the same name/sizel/ayout/parameters
                // ----------------------------------------------------------------------------
                var bufferKey = new ConstantBufferKey(bufferRaw);
                EffectConstantBuffer buffer;
                if (!bufferSet.TryGetValue(bufferKey, out buffer))
                {
                    // 4) If this buffer doesn't exist, create a new one and register it.
                    buffer = new EffectConstantBuffer(device, this, bufferRaw);
                    bufferSet[bufferKey] = buffer;
                }

                return buffer;
            }
        }

        /// <summary>
        /// Creates a new effect group from a specified list of <see cref="EffectBytecode"/>.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="bytecodes">The bytecodes.</param>
        /// <returns>An instance of <see cref="EffectGroup"/>.</returns>
        public static EffectGroup New(GraphicsDevice device, params EffectBytecode[] bytecodes)
        {
            var group = new EffectGroup(device);
            group.RegisterBytecode(bytecodes);
            return group;
        }

        private static Buffer DefaultConstantBufferAllocator(GraphicsDevice device, EffectGroup group, EffectConstantBuffer constantBuffer)
        {
            return Buffer.Cosntant.New(device, constantBuffer.Size);
        }

        #region Nested type: ConstantBufferKey

        private class ConstantBufferKey : IEquatable<ConstantBufferKey>
        {
            public readonly EffectBytecode.ConstantBuffer Description;
            public readonly int HashCode;

            public ConstantBufferKey(EffectBytecode.ConstantBuffer description)
            {
                Description = description;
                HashCode = description.GetHashCode();
            }

            #region IEquatable<ConstantBufferKey> Members

            public bool Equals(ConstantBufferKey other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return HashCode == other.HashCode && Description.Equals(other.Description);
            }

            #endregion

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((ConstantBufferKey) obj);
            }

            public override int GetHashCode()
            {
                return HashCode;
            }

            public static bool operator ==(ConstantBufferKey left, ConstantBufferKey right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(ConstantBufferKey left, ConstantBufferKey right)
            {
                return !Equals(left, right);
            }
        }

        #endregion
    }
}