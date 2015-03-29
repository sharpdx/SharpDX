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

namespace SharpDX.MediaFoundation
{
    /// <summary>
    /// Internal MediaEngineNotify Callback
    /// </summary>
    internal class MediaEngineNotifyShadow : SharpDX.ComObjectShadow
    {
        private static readonly MediaEngineNotifyVtbl Vtbl = new MediaEngineNotifyVtbl();

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(MediaEngineNotify callback)
        {
            return ToCallbackPtr<MediaEngineNotify>(callback);
        }

        public class MediaEngineNotifyVtbl : ComObjectVtbl
        {
            public MediaEngineNotifyVtbl() : base(1)
            {
                AddMethod(new EventNotifyDelegate(EventNotifyImpl));
            }

            /// <unmanaged>HRESULT IMFMediaEngineNotify::EventNotify([In] unsigned int event,[In] ULONG_PTR param1,[In] unsigned int param2)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int EventNotifyDelegate(IntPtr thisPtr, int eventId, IntPtr param1, int param2);
            private static int EventNotifyImpl(IntPtr thisPtr, int eventId, IntPtr param1, int param2)
            {
                try
                {
                    var shadow = ToShadow<MediaEngineNotifyShadow>(thisPtr);
                    var callback = (MediaEngineNotify)shadow.Callback;
                    callback.OnPlaybackEvent((MediaEngineEvent)eventId, param1.ToInt64(), param2);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }
       }

        protected override CppObjectVtbl GetVtbl
        {
            get { return Vtbl; }
        }
    }
}