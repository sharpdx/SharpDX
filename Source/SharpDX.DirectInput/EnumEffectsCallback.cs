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
    /// Enumerator callback for DirectInput EnumEffects.
    /// </summary>
    internal class EnumEffectsCallback
    {
        private readonly IntPtr _nativePointer;
        private readonly DirectInputEnumEffectsDelegate _callback;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumEffectsCallback"/> class.
        /// </summary>
        public EnumEffectsCallback()
        {
            unsafe
            {
                _callback = new DirectInputEnumEffectsDelegate(DirectInputEnumEffectsImpl);
                _nativePointer = Marshal.GetFunctionPointerForDelegate(_callback);
                EffectInfos = new List<EffectInfo>();
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
        /// Gets or sets the effect infos.
        /// </summary>
        /// <value>The effect infos.</value>
        public List<EffectInfo> EffectInfos { get; private set; }

        // BOOL DIEnumEffectsCallback(LPCDIEffectInfo pdei,LPVOID pvRef)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private unsafe delegate int DirectInputEnumEffectsDelegate(void* deviceInstance, IntPtr data);
        private unsafe int DirectInputEnumEffectsImpl(void* deviceInstance, IntPtr data)
        {
            var newEffect = new EffectInfo();
            newEffect.__MarshalFrom(ref *((EffectInfo.__Native*)deviceInstance));
            EffectInfos.Add(newEffect);
            // Return true to continue iterating
            return 1;
        }
    }
}