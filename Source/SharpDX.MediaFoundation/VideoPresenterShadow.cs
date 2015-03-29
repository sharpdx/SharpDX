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

#if DESKTOP_APP

using System;
using System.Runtime.InteropServices;

namespace SharpDX.MediaFoundation
{
    class VideoPresenterShadow : ClockStateSinkShadow
    {
        private static readonly VideoPresenterVtbl Vtbl = new VideoPresenterVtbl(0);

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(VideoPresenter callback)
        {
            return ToCallbackPtr<VideoPresenter>(callback);
        }

        protected class VideoPresenterVtbl : ClockStateSinkVtbl
        {
            public VideoPresenterVtbl(int numOfMethods)
                : base(numOfMethods + 2)
            {
                unsafe
                {
                    AddMethod(new ProcessMessageDelegate(ProcessMessageImpl));
                    AddMethod(new GetMediaTypeDelegate(GetMediaTypeImpl));
                }
            }

            /// <unmanaged>HRESULT IMFVideoPresenter::ProcessMessage([In] MFVP_MESSAGE_TYPE eMessage,[In] ULONG_PTR ulParam)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            protected delegate int ProcessMessageDelegate(IntPtr thisObject, int eMessage, IntPtr ulParam);
            static protected int ProcessMessageImpl(IntPtr thisObject, int eMessage, IntPtr ulParam)
            {
                try
                {
                    var shadow = ToShadow<VideoPresenterShadow>(thisObject);
                    var presenter = (VideoPresenter)shadow.Callback;
                    presenter.ProcessMessage((VpMessageType)eMessage, ulParam);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }

                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT GetCurrentMediaType([out] IMFVideoMediaType **ppMediaType)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            protected unsafe delegate int GetMediaTypeDelegate(IntPtr thisObject, IntPtr* ppMediaType);
            static unsafe protected int GetMediaTypeImpl(IntPtr thisObject, IntPtr* ppMediaType)
            {
                try
                {
                    var shadow = ToShadow<VideoPresenterShadow>(thisObject);
                    var presenter = (VideoPresenter)shadow.Callback;
                    *ppMediaType = presenter.CurrentMediaType.NativePointer;
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

#endif