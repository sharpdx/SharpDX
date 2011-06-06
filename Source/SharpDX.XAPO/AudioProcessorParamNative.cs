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

namespace SharpDX.XAPO
{

    /// <summary>
    /// Implements this class to call an existing unmanaged AudioProcessor which supports parameter.
    /// </summary>
    /// <typeparam name="T">the parameter type of this AudioProcessor</typeparam>
    public abstract partial class AudioProcessorParamNative<T> : AudioProcessorNative where T : struct
    {
        private ParameterProviderNative _parameterProviderNative;
        private int _sizeOfParamT = Utilities.SizeOf<T>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioProcessorParamNative&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="basePtr">The base PTR.</param>
        protected AudioProcessorParamNative(IntPtr basePtr) : base(basePtr)
        {
        }

        /// <summary>
        /// Update the Native Poinder. Rebuild ParameterProviderNative.
        /// </summary>
        protected override void NativePointerUpdated(IntPtr oldPointer)
        {
            base.NativePointerUpdated(oldPointer);

            if (NativePointer != IntPtr.Zero)
            {
                IntPtr parameterProviderPtr;
                QueryInterface(typeof (ParameterProvider).GUID, out parameterProviderPtr);
                _parameterProviderNative = new ParameterProviderNative(parameterProviderPtr);
            }
        }


        /// <summary>
        /// Get or Set the parameters for this AudioProcessor
        /// </summary>
        public T Parameter
        {
            get
            {
                unsafe
                {
                    T param = default(T);
                    byte* pParameters = stackalloc byte[_sizeOfParamT];
                    _parameterProviderNative.GetParameters(new DataStream(pParameters, _sizeOfParamT, true,true,false));
                    Utilities.Read((IntPtr) pParameters, ref param);
                    return param;
                }
            }

            set
            {
                unsafe
                {
                    byte* pParameters = stackalloc byte[_sizeOfParamT];
                    Utilities.Write((IntPtr)pParameters, ref value);
                    _parameterProviderNative.SetParameters(new DataStream(pParameters, _sizeOfParamT, true, true, false));
                }
            }
        }
    }
}