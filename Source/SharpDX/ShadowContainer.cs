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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Reflection;

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

        private IntPtr guidPtr;
        public IntPtr[] Guids { get; private set; }

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
#if BEFORE_NET45
                    var interfaces = type.GetTypeInfo().GetInterfaces();
#else
                    var interfaces = type.GetTypeInfo().ImplementedInterfaces;
#endif
                    slimInterfaces = new List<Type>();
                    slimInterfaces.AddRange(interfaces);
                    typeToShadowTypes.Add(type, slimInterfaces);

                    // First pass to identify most detailed interfaces
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
#if BEFORE_NET45
                        var inheritList = item.GetTypeInfo().GetInterfaces();
#else
                        var inheritList = item.GetTypeInfo().ImplementedInterfaces;
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
#if BEFORE_NET45
                var inheritList = item.GetTypeInfo().GetInterfaces();
#else
                var inheritList = item.GetTypeInfo().ImplementedInterfaces;
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

            // Precalculate the list of GUID without IUnknown and IInspectable
            // Used for WinRT 
            int countGuids = 0;
            foreach (var guidKey in guidToShadow.Keys)
            {
                if (guidKey != Utilities.GetGuidFromType(typeof(IInspectable)) && guidKey != Utilities.GetGuidFromType(typeof(IUnknown)))
                    countGuids++;
            }

            guidPtr = Marshal.AllocHGlobal(Utilities.SizeOf<Guid>() * countGuids);
            Guids = new IntPtr[countGuids];
            int i = 0;
            unsafe
            {
                var pGuid = (Guid*) guidPtr;
                foreach (var guidKey in guidToShadow.Keys)
                {
                    if (guidKey == Utilities.GetGuidFromType(typeof(IInspectable)) || guidKey == Utilities.GetGuidFromType(typeof(IUnknown)))
                        continue;

                    pGuid[i] = guidKey;
                    // Store the pointer
                    Guids[i] = new IntPtr(pGuid + i);
                    i++;
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

                if (guidPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(guidPtr);
                    guidPtr = IntPtr.Zero;
                }
            }
        }
    }
}