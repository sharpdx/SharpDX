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
using System.Runtime.InteropServices;

namespace SharpDX.XAudio2
{
    /// <summary>
    /// Internal VoiceCallback Callback Impl
    /// </summary>
    internal class VoiceCallBackImpl : SharpDX.CppObjectCallbackNative
    {
        private VoiceCallback Callback { get; set; }

        public VoiceCallBackImpl(VoiceCallback callback)
            : base(7)
        {
            Callback = callback;
            AddMethod(new intDelegate(OnVoiceProcessingPassStartImpl));
            AddMethod(new voidDelegate(OnVoiceProcessingPassEndImpl));
            AddMethod(new voidDelegate(OnStreamEndImpl));
            AddMethod(new IntPtrDelegate(OnBufferStartImpl));
            AddMethod(new IntPtrDelegate(OnBufferEndImpl));
            AddMethod(new IntPtrDelegate(OnLoopEndImpl));
            AddMethod(new IntPtrIntDelegate(OnVoiceErrorImpl));
        }


        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void voidDelegate(IntPtr thisObject);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void intDelegate(IntPtr thisObject, int bytes);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void IntPtrDelegate(IntPtr thisObject, IntPtr address);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void IntPtrIntDelegate(IntPtr thisObject, IntPtr address, int error);

        void OnVoiceProcessingPassStartImpl(IntPtr thisObject, int bytes)
        {
            Callback.OnVoiceProcessingPassStart(bytes);
        }

        void OnVoiceProcessingPassEndImpl(IntPtr thisObject)
        {
            Callback.OnVoiceProcessingPassEnd();
        }

        void OnStreamEndImpl(IntPtr thisObject)
        {
            Callback.OnStreamEnd();
        }

        void OnBufferStartImpl(IntPtr thisObject, IntPtr address)
        {
            Callback.OnBufferStart(address);
        }

        void OnBufferEndImpl(IntPtr thisObject, IntPtr address)
        {
            Callback.OnBufferEnd(address);
        }


        void OnLoopEndImpl(IntPtr thisObject, IntPtr address)
        {
            Callback.OnLoopEnd(address);
        }

        private void OnVoiceErrorImpl(IntPtr thisObject, IntPtr bufferContextRef, int error)
        {
            Callback.OnVoiceError(bufferContextRef, new Result(error));
        }
    }
}