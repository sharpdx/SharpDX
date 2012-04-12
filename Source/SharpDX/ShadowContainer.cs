// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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

namespace SharpDX
{
    /// <summary>
    /// The ShadowContainer is the main container used internally to keep references to all native COM/C++ callbacks.
    /// It is stored in the property <see cref="ICallbackable.Shadow"/>.
    /// </summary>
    internal class ShadowContainer : DisposeBase
    {
        private readonly Dictionary<Guid, CppObjectShadow> guidToShadow = new Dictionary<Guid, CppObjectShadow>();

        private static readonly Dictionary<Type, List<Type>> typeToShadowTypes = new Dictionary<Type, List<Type>>();

        public void Initialize(ICallbackable callbackable)
        {
            callbackable.Shadow = this;

            var type = callbackable.GetType();
            List<Type> slimInterfaces;

            // Cache reflection on COM interface inheritance
            lock (typeToShadowTypes)
            {
                if (!typeToShadowTypes.TryGetValue(type, out slimInterfaces))
                {
#if WIN8METRO
                    var interfaces = type.GetTypeInfo().ImplementedInterfaces;
#else
                    var interfaces = type.GetInterfaces();
#endif
                    slimInterfaces = new List<Type>();
                    slimInterfaces.AddRange(interfaces);
                    typeToShadowTypes.Add(type, slimInterfaces);

                    // First pass to identify most detailled interfaces
                    foreach (var item in interfaces)
                    {
                        // Only process interfaces that are using shadow
                        var shadowAttribute = ShadowAttribute.Get(item);
                        if (shadowAttribute == null)
                        {
                            slimInterfaces.Remove(item);
                            continue;
                        }

                        // Keep only final interfaces and not intermediate.
#if WIN8METRO
                        var inheritList = item.GetTypeInfo().ImplementedInterfaces;
#else
                        var inheritList = item.GetInterfaces();
#endif
                        foreach (var inheritInterface in inheritList)
                        {
                            slimInterfaces.Remove(inheritInterface);
                        }
                    }
                }
            }

            CppObjectShadow iunknownShadow = null;

            // Second pass to instantiate shadow
            foreach (var item in slimInterfaces)
            {
                // Only process interfaces that are using shadow
                var shadowAttribute = ShadowAttribute.Get(item);

                // Initialize the shadow with the callback
                var shadow = (CppObjectShadow)Activator.CreateInstance(shadowAttribute.Type);
                shadow.Initialize(callbackable);

                // Take the first shadow as the main IUnknown
                if (iunknownShadow == null)
                {
                    iunknownShadow = shadow;
                    // Add IUnknown as a supported interface
                    guidToShadow.Add(ComObjectShadow.IID_IUnknown, iunknownShadow);
                }

                guidToShadow.Add(Utilities.GetGuidFromType(item), shadow);

                // Associate also inherited interface to this shadow
#if WIN8METRO
                var inheritList = item.GetTypeInfo().ImplementedInterfaces;
#else
                var inheritList = item.GetInterfaces();
#endif
                foreach (var inheritInterface in inheritList)
                {
                    var inheritShadowAttribute = ShadowAttribute.Get(inheritInterface);
                    if (inheritShadowAttribute == null)
                        continue;

                    // Use same shadow as derived
                    guidToShadow.Add(Utilities.GetGuidFromType(inheritInterface), shadow);
                }
            }
        }

        internal IntPtr Find(Type type)
        {
            return Find(Utilities.GetGuidFromType(type));
        }

        internal IntPtr Find(Guid guidType)
        {
            var shadow = FindShadow(guidType);
            return (shadow == null) ? IntPtr.Zero : shadow.NativePointer;
        }

        internal CppObjectShadow FindShadow(Guid guidType)
        {
            CppObjectShadow shadow;
            guidToShadow.TryGetValue(guidType, out shadow);
            return shadow;
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var comObjectCallbackNative in guidToShadow.Values)
                    comObjectCallbackNative.Dispose();
                guidToShadow.Clear();
            }
        }
    }
}