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

namespace SharpDX.XAudio2
{
    /// <summary>
    /// Internal EngineCallback Callback Implementation
    /// </summary>
    internal class EngineShadow : SharpDX.CppObjectShadow
    {
        private static readonly EngineVtbl Vtbl = new EngineVtbl();

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(EngineCallback callback)
        {
            return ToCallbackPtr<EngineCallback>(callback);
        }

        public class EngineVtbl : CppObjectVtbl {
            public EngineVtbl()
                : base(3)
            {
                AddMethod(new OnProcessingPassStartDelegate(OnProcessingPassStartImpl));
                AddMethod(new OnProcessingPassEndDelegate(OnProcessingPassEndImpl));
                AddMethod(new OnCriticalErrorDelegate(OnCriticalErrorImpl));
            }

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnProcessingPassStartDelegate(IntPtr thisObject);
            private static void OnProcessingPassStartImpl(IntPtr thisObject)
            {
                var shadow = ToShadow<EngineShadow>(thisObject);
                var callback = (EngineCallback)shadow.Callback;
                callback.OnProcessingPassStart();
            }

            /// <summary>	
            /// Called by XAudio2 just after an audio processing pass ends.	
            /// </summary>	
            /// <unmanaged>void IXAudio2EngineCallback::OnProcessingPassEnd()</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnProcessingPassEndDelegate(IntPtr thisObject);
            private static void OnProcessingPassEndImpl(IntPtr thisObject)
            {
                var shadow = ToShadow<EngineShadow>(thisObject);
                var callback = (EngineCallback)shadow.Callback;
                callback.OnProcessingPassEnd();
            }

            /// <summary>	
            /// Called if a critical system error occurs that requires XAudio2 to be closed down and restarted.	
            /// </summary>
            /// <param name="thisObject">This pointer</param>
            /// <param name="error"> Error code returned by XAudio2. </param>
            /// <unmanaged>void IXAudio2EngineCallback::OnCriticalError([None] HRESULT Error)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnCriticalErrorDelegate(IntPtr thisObject, int error);
            private static void OnCriticalErrorImpl(IntPtr thisObject, int error)
            {
                var shadow = ToShadow<EngineShadow>(thisObject);
                var callback = (EngineCallback)shadow.Callback;
                callback.OnCriticalError(new Result(error));            
           }
        }

        protected override CppObjectVtbl GetVtbl
        {
            get { return Vtbl; }
        }
    }
}