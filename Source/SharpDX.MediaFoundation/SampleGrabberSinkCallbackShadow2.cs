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
    internal class SampleGrabberSinkCallback2Shadow : SampleGrabberSinkCallbackShadow
    {
        private static readonly SampleGrabberSinkCallback2Vtbl Vtbl = new SampleGrabberSinkCallback2Vtbl(0);

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(SampleGrabberSinkCallback2 callback)
        {
            return ToCallbackPtr<SampleGrabberSinkCallback2>(callback);
        }

        private class SampleGrabberSinkCallback2Vtbl : SampleGrabberSinkCallbackVtbl
        {
            public SampleGrabberSinkCallback2Vtbl(int numOfMethods)
                : base(numOfMethods + 1)
            {
                unsafe
                {
                    AddMethod(new ProcessSampleExDelegate(OnProcessSampleExImpl));
                }
            }

            /// <unmanaged>HRESULT IMFSampleGrabberSinkCallback2::OnProcessSampleEx([In] const GUID& guidMajorMediaType,[In] unsigned int dwSampleFlags,[In] longlong llSampleTime,[In] longlong llSampleDuration,[In, Buffer] const unsigned char* pSampleBuffer,[In] unsigned int dwSampleSize,[In] IMFAttributes* pAttributes)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private unsafe delegate int ProcessSampleExDelegate(IntPtr thisObject, Guid* guidMajorMediaType, int dwSampleFlags, long llSampleTime, long llSampleDuration, IntPtr sampleBufferRef, int dwSampleSize, IntPtr attributesRef);
            static unsafe private int OnProcessSampleExImpl(IntPtr thisObject, Guid* guidMajorMediaType, int dwSampleFlags, long llSampleTime, long llSampleDuration, IntPtr sampleBufferRef, int dwSampleSize, IntPtr attributesRef)
            {
                try
                {
                    var shadow = ToShadow<SampleGrabberSinkCallback2Shadow>(thisObject);
                    var callback = (SampleGrabberSinkCallback2)shadow.Callback;

                    callback.OnProcessSampleEx(*guidMajorMediaType, dwSampleFlags, llSampleTime, llSampleDuration, sampleBufferRef, dwSampleSize, new MediaAttributes(attributesRef));
                    
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