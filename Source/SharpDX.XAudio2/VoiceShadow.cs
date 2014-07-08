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
    /// Internal VoiceCallback callback Implementation
    /// </summary>
    internal class VoiceShadow : SharpDX.CppObjectShadow
    {
        private static readonly VoiceVtbl Vtbl = new VoiceVtbl();

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(VoiceCallback callback)
        {
            return ToCallbackPtr<VoiceCallback>(callback);
        }

        private class VoiceVtbl : CppObjectVtbl {
            public VoiceVtbl()
                : base(7)
            {
                AddMethod(new IntDelegate(OnVoiceProcessingPassStartImpl));
                AddMethod(new VoidDelegate(OnVoiceProcessingPassEndImpl));
                AddMethod(new VoidDelegate(OnStreamEndImpl));
                AddMethod(new IntPtrDelegate(OnBufferStartImpl));
                AddMethod(new IntPtrDelegate(OnBufferEndImpl));
                AddMethod(new IntPtrDelegate(OnLoopEndImpl));
                AddMethod(new IntPtrIntDelegate(OnVoiceErrorImpl));
            }

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void VoidDelegate(IntPtr thisObject);
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void IntDelegate(IntPtr thisObject, int bytes);
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void IntPtrDelegate(IntPtr thisObject, IntPtr address);
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void IntPtrIntDelegate(IntPtr thisObject, IntPtr address, int error);

            static private void OnVoiceProcessingPassStartImpl(IntPtr thisObject, int bytes)
            {
                var shadow = ToShadow<VoiceShadow>(thisObject);
                var callback = (VoiceCallback)shadow.Callback;
                callback.OnVoiceProcessingPassStart(bytes);
            }

            static private void OnVoiceProcessingPassEndImpl(IntPtr thisObject)
            {
                var shadow = ToShadow<VoiceShadow>(thisObject);
                var callback = (VoiceCallback)shadow.Callback;
                callback.OnVoiceProcessingPassEnd();
            }

            static private void OnStreamEndImpl(IntPtr thisObject)
            {
                var shadow = ToShadow<VoiceShadow>(thisObject);
                var callback = (VoiceCallback)shadow.Callback;
                callback.OnStreamEnd();
            }

            static private void OnBufferStartImpl(IntPtr thisObject, IntPtr address)
            {
                var shadow = ToShadow<VoiceShadow>(thisObject);
                var callback = (VoiceCallback)shadow.Callback;
                callback.OnBufferStart(address);
            }

            static private void OnBufferEndImpl(IntPtr thisObject, IntPtr address)
            {
                var shadow = ToShadow<VoiceShadow>(thisObject);
                var callback = (VoiceCallback)shadow.Callback;
                callback.OnBufferEnd(address);
            }


            static private void OnLoopEndImpl(IntPtr thisObject, IntPtr address)
            {
                var shadow = ToShadow<VoiceShadow>(thisObject);
                var callback = (VoiceCallback)shadow.Callback;
                callback.OnLoopEnd(address);
            }

            static private void OnVoiceErrorImpl(IntPtr thisObject, IntPtr bufferContextRef, int error)
            {
                var shadow = ToShadow<VoiceShadow>(thisObject);
                var callback = (VoiceCallback)shadow.Callback;
                callback.OnVoiceError(bufferContextRef, new Result(error));
            }
        }

        protected override CppObjectVtbl GetVtbl
        {
            get { return Vtbl; }
        }
    }
}