// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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
using System.Reflection;
using System.Collections.Generic;


namespace SharpDX.Direct2D1
{
    public partial class Factory1
    {
        private Dictionary<Guid, CustomEffectFactory> registeredEffects = new Dictionary<Guid, CustomEffectFactory>();


        /// <summary>
        /// Default Constructor for a <see cref = "SharpDX.Direct2D1.Factory1" />.
        /// </summary>
        public Factory1()
            : this(FactoryType.SingleThreaded)
        {
        }

        /// <summary>
        /// Default Constructor for a <see cref = "SharpDX.Direct2D1.Factory1" />.
        /// </summary>
        public Factory1(FactoryType factoryType)
            : this(factoryType, DebugLevel.None)
        {
        }

        /// <summary>
        /// Default Constructor for a <see cref = "SharpDX.Direct2D1.Factory1" />.
        /// </summary>
        public Factory1(FactoryType factoryType, DebugLevel debugLevel)
            : base(factoryType, debugLevel)
        {
        }

        /// <summary>	
        /// Get the effects registered
        /// </summary>	
        /// <unmanaged>HRESULT ID2D1Factory1::GetRegisteredEffects([Out, Buffer, Optional] GUID* effects,[In] unsigned int effectsCount,[Out, Optional] unsigned int* effectsReturned,[Out, Optional] unsigned int* effectsRegistered)</unmanaged>	
        public Guid[] RegisteredEffects
        {
            get
            {
                int effectReturned;
                int effectRegistered;
                GetRegisteredEffects(null, 0, out effectReturned, out effectRegistered);
                var guids = new Guid[effectRegistered];
                GetRegisteredEffects(guids, guids.Length, out effectReturned, out effectRegistered);
                return guids;
            }
        }

        /// <summary>
        /// Register a <see cref="CustomEffect"/> factory.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="effectFactory"></param>
        public void RegisterEffect<T>(Func<T> effectFactory) where T : CustomEffect
        {
            var effectId = Utilities.GetGuidFromType(typeof(T));
            RegisterEffect<T>(effectFactory, effectId);
        }

        /// <summary>
        /// Register a <see cref="CustomEffect"/> factory.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="effectFactory"></param>
        /// <param name="effectId"></param>
        public void RegisterEffect<T>(Func<T> effectFactory, Guid effectId) where T : CustomEffect
        {
            CustomEffectFactory factory;
            lock (registeredEffects)
            {
                if (registeredEffects.ContainsKey(effectId))
                    throw new ArgumentException("An effect is already registered with this GUID", "effectFactory");

                factory = new CustomEffectFactory(() => effectFactory(), typeof(T), effectId);
                registeredEffects.Add(effectId, factory);
            }
            RegisterEffectFromString(effectId, factory.ToXml(), factory.Bindings, factory.Bindings.Length, factory.NativePointer);
        }

        /// <summary>
        /// Register a <see cref="CustomEffect"/>.
        /// </summary>
        /// <typeparam name="T">Type of </typeparam>
        public void RegisterEffect<T>() where T : CustomEffect, new()
        {
            RegisterEffect<T>(Utilities.GetGuidFromType(typeof(T)));
        }

        /// <summary>
        /// Register a <see cref="CustomEffect"/>.
        /// </summary>
        /// <typeparam name="T">Type of </typeparam>
        /// <param name="effectId"></param>
        public void RegisterEffect<T>(Guid effectId) where T : CustomEffect, new()
        {
            CustomEffectFactory factory;
            lock (registeredEffects)
            {
                if (registeredEffects.ContainsKey(effectId))
                    throw new ArgumentException("An effect is already registered with this GUID", "effectFactory");

                factory = new CustomEffectFactory(() => new T(), typeof(T), effectId);
                registeredEffects.Add(effectId, factory);
            }

            RegisterEffectFromString(effectId, factory.ToXml(), factory.Bindings, factory.Bindings.Length, factory.NativePointer);
        }

        /// <summary>
        /// Unregister a <see cref="CustomEffect"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void UnRegisterEffect<T>() where T : CustomEffect
        {
            CustomEffectFactory factory;
            var guid = Utilities.GetGuidFromType(typeof(T));

            lock (registeredEffects)
            {
                if (registeredEffects.TryGetValue(guid, out factory))
                {
                    // factory.Dispose();
                    registeredEffects.Remove(guid);
                }
            }

            UnregisterEffect(guid);
        }
    }
}
