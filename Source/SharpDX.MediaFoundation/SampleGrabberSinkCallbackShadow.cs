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
    internal class SampleGrabberSinkCallbackShadow : ClockStateSinkShadow
    {
        private static readonly SampleGrabberSinkCallbackVtbl Vtbl = new SampleGrabberSinkCallbackVtbl(0);

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(SampleGrabberSinkCallback callback)
        {
            return ToCallbackPtr<SampleGrabberSinkCallback>(callback);
        }

        protected class SampleGrabberSinkCallbackVtbl : ClockStateSinkVtbl
        {

            public SampleGrabberSinkCallbackVtbl(int numOfMethods)
                : base(numOfMethods + 3)
            {
                unsafe
                {
                    AddMethod(new PresentationClockDelegate(OnSetPresentationClockImpl));
                    AddMethod(new OnProcessSampleDelegate(OnProcessSampleImpl));
                    AddMethod(new ShutdownDelegate(OnShutdownImpl));
                }
            }

            /// <unmanaged>HRESULT IMFSampleGrabberSinkCallback::OnSetPresentationClock([in] IMFPresentationClock *pPresentationClock);</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int PresentationClockDelegate(IntPtr thisObject, IntPtr presentationClock);
            static private int OnSetPresentationClockImpl(IntPtr thisObject, IntPtr presentationClock)
            {
                try
                {
                    var shadow = ToShadow<SampleGrabberSinkCallbackShadow>(thisObject);
                    var callback = (SampleGrabberSinkCallback)shadow.Callback;

                    callback.OnSetPresentationClock(new PresentationClock(presentationClock));
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }

                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT OnProcessSample([in] REFGUID guidMajorMediaType, [in] DWORD dwSampleFlags, [in] LONGLONG llSampleTime, [in] LONGLONG llSampleDuration, [in] const BYTE *pSampleBuffer, [in] DWORD dwSampleSize);</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private unsafe delegate int OnProcessSampleDelegate(IntPtr thisObject, Guid* guidMajorMediaType, int dwSampleFlags, long llSampleTime, long llSampleDuration, IntPtr sampleBufferRef, int dwSampleSize);
            static unsafe private int OnProcessSampleImpl(IntPtr thisObject, Guid* guidMajorMediaType, int dwSampleFlags, long llSampleTime, long llSampleDuration, IntPtr sampleBufferRef, int dwSampleSize)
            {
                try
                {
                    var shadow = ToShadow<SampleGrabberSinkCallbackShadow>(thisObject);
                    var callback = (SampleGrabberSinkCallback)shadow.Callback;

                    callback.OnProcessSample(*guidMajorMediaType, dwSampleFlags, llSampleTime, llSampleDuration, sampleBufferRef, dwSampleSize);
                    
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }

                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT IMFSampleGrabberSinkCallback::OnShutdown();</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int ShutdownDelegate(IntPtr thisObject);
            static protected int OnShutdownImpl(IntPtr thisObject)
            {
                try
                {
                    var shadow = ToShadow<SampleGrabberSinkCallbackShadow>(thisObject);
                    var callback = (SampleGrabberSinkCallback)shadow.Callback;
                    callback.OnShutdown();
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