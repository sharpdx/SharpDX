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
using System.Runtime.InteropServices;

namespace SharpDX.XAPO
{
    /// <summary>
    /// Internal AudioProcessorShadow
    /// </summary>
    /// <unmanaged>IXAPOParameters</unmanaged>
    [Guid("a90bc001-e897-e897-55e4-9e4700000001")]
    internal class ParameterProviderShadow : ComObjectShadow
    {
        private static readonly ParameterProviderVtbl Vtbl = new ParameterProviderVtbl();

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(ParameterProvider callback)
        {
            return ToCallbackPtr<ParameterProvider>(callback);
        }

        public class ParameterProviderVtbl : ComObjectVtbl
        {
            public ParameterProviderVtbl() : base(2)
            {
                AddMethod(new GetSetParametersDelegate(SetParametersImpl));
                AddMethod(new GetSetParametersDelegate(GetParameters));
            }

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void GetSetParametersDelegate(IntPtr thisObject, IntPtr paramPointer, int paramSize);
            /// <summary>	
            /// Sets effect-specific parameters.	
            /// </summary>	
            /// <param name="thisObject">This pointer</param>
            /// <param name="paramPointer"> Effect-specific parameter block. </param>
            /// <param name="paramSize">size of the parameters</param>
            /// <unmanaged>void IXAPOParameters::SetParameters([In, Buffer] const void* pParameters,[None] UINT32 ParameterByteSize)</unmanaged>
            /* public void SetParameters(IntPtr arametersRef, int parameterByteSize) */
            private static void SetParametersImpl(IntPtr thisObject, IntPtr paramPointer, int paramSize)
            {
                var shadow = ToShadow<ParameterProviderShadow>(thisObject);
                var callback = (ParameterProvider) shadow.Callback;
                callback.SetParameters(new DataStream(paramPointer, paramSize, true, true));
            }

            /// <summary>	
            /// Gets the current values for any effect-specific parameters.	
            /// </summary>	
            /// <param name="thisObject">This pointer</param>
            /// <param name="paramPointer">[in, out]  Receives an effect-specific parameter block. </param>
            /// <param name="paramSize">size of the parameters</param>
            /// <unmanaged>void IXAPOParameters::GetParameters([Out, Buffer] void* pParameters,[None] UINT32 ParameterByteSize)</unmanaged>
            private void GetParameters(IntPtr thisObject, IntPtr paramPointer, int paramSize)
            {
                var shadow = ToShadow<ParameterProviderShadow>(thisObject);
                var callback = (ParameterProvider)shadow.Callback;
                callback.GetParameters(new DataStream(paramPointer, paramSize, true, true));
            }
        }

        protected override CppObjectVtbl GetVtbl
        {
            get { return Vtbl; }
        }
    }
}