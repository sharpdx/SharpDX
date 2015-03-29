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

namespace SharpDX.MediaFoundation
{
    /// <summary>
    /// Internal ClockStateSink callback
    /// </summary>
    internal class ClockStateSinkShadow : SharpDX.ComObjectShadow
    {
        private static readonly ClockStateSinkVtbl Vtbl = new ClockStateSinkVtbl(0);

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(ClockStateSink callback)
        {
            return ToCallbackPtr<ClockStateSink>(callback);
        }

        protected class ClockStateSinkVtbl : ComObjectVtbl
        {
            public ClockStateSinkVtbl(int numOfMethods)
                : base(numOfMethods + 5)
            {
                AddMethod(new OnClockStartDelegate(OnClockStartImpl));
                AddMethod(new LongDelegate(OnClockStopImpl));
                AddMethod(new LongDelegate(OnClockPauseImpl));
                AddMethod(new LongDelegate(OnClockRestartImpl));
                AddMethod(new OnClockSetRateDelegate(OnClockSetRateImpl));
            }

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            protected delegate int LongDelegate(IntPtr thisObject, long hnsSystemTime);

            /// <unmanaged>HRESULT IMFClockStateSink::OnClockStart([In] longlong hnsSystemTime,[In] longlong llClockStartOffset)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            protected delegate int OnClockStartDelegate(IntPtr thisObject, long hnsSystemTime, long llClockStartOffset);
            static protected int OnClockStartImpl(IntPtr thisObject, long hnsSystemTime, long llClockStartOffset)
            {
                try
                {
                    var shadow = ToShadow<ClockStateSinkShadow>(thisObject);
                    var callback = (ClockStateSink)shadow.Callback;
                    callback.OnClockStart(hnsSystemTime, llClockStartOffset);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }

                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT IMFClockStateSink::OnClockStop([In] longlong hnsSystemTime)</unmanaged>	
            static protected int OnClockStopImpl(IntPtr thisObject, long hnsSystemTime)
            {
                try
                {
                    var shadow = ToShadow<ClockStateSinkShadow>(thisObject);
                    var callback = (ClockStateSink)shadow.Callback;
                    callback.OnClockStop(hnsSystemTime);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }

                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT IMFClockStateSink::OnClockPause([In] longlong hnsSystemTime)</unmanaged>	
            static protected int OnClockPauseImpl(IntPtr thisObject, long hnsSystemTime)
            {
                try
                {
                    var shadow = ToShadow<ClockStateSinkShadow>(thisObject);
                    var callback = (ClockStateSink)shadow.Callback;
                    callback.OnClockPause(hnsSystemTime);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }

                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT IMFClockStateSink::OnClockRestart([In] longlong hnsSystemTime)</unmanaged>	
            static protected int OnClockRestartImpl(IntPtr thisObject, long hnsSystemTime)
            {
                try
                {
                    var shadow = ToShadow<ClockStateSinkShadow>(thisObject);
                    var callback = (ClockStateSink)shadow.Callback;
                    callback.OnClockRestart(hnsSystemTime);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }

                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT IMFClockStateSink::OnClockSetRate([In] longlong hnsSystemTime,[In] float flRate)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            protected delegate int OnClockSetRateDelegate(IntPtr thisObject, long hnsSystemTime, float flRate);
            static protected int OnClockSetRateImpl(IntPtr thisObject, long hnsSystemTime, float flRate)
            {
                try
                {
                    var shadow = ToShadow<ClockStateSinkShadow>(thisObject);
                    var callback = (ClockStateSink)shadow.Callback;
                    callback.OnClockSetRate(hnsSystemTime, flRate);
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