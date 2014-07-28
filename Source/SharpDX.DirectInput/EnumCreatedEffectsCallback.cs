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

namespace SharpDX.DirectInput
{
    /// <summary>
    /// Enumerator callback for DirectInput EnumCreatedEffects.
    /// </summary>
    internal class EnumCreatedEffectsCallback
    {
        private readonly IntPtr _nativePointer;
        private readonly DirectInputEnumCreatedEffectsDelegate _callback;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumCreatedEffectsCallback"/> class.
        /// </summary>
        public EnumCreatedEffectsCallback()
        {
            unsafe
            {
                _callback = new DirectInputEnumCreatedEffectsDelegate(DirectInputEnumCreatedEffectsImpl);
                _nativePointer = Marshal.GetFunctionPointerForDelegate(_callback);
                Effects = new List<Effect>();
            }
        }

        /// <summary>
        /// Natives the pointer.
        /// </summary>
        /// <returns></returns>
        public IntPtr NativePointer
        {
            get { return _nativePointer; }
        }

        /// <summary>
        /// Gets or sets the effects.
        /// </summary>
        /// <value>The effects.</value>
        public List<Effect> Effects { get; private set; }

        // BOOL DIEnumCreatedEffectsCallback(LPCDIEffectInfo pdei,LPVOID pvRef)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private unsafe delegate int DirectInputEnumCreatedEffectsDelegate(void* deviceInstance, IntPtr data);
        private unsafe int DirectInputEnumCreatedEffectsImpl(void* deviceInstance, IntPtr data)
        {
            var newEffect = new Effect((IntPtr)deviceInstance);
            Effects.Add(newEffect);
            // Return true to continue iterating
            return 1;
        }
    }
}