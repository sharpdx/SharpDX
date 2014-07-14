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
    /// Internal AsyncCallback Callback
    /// </summary>
    internal class AsyncCallbackShadow : SharpDX.ComObjectShadow
    {
        private static readonly AsyncCallbackVtbl Vtbl = new AsyncCallbackVtbl();

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(IAsyncCallback callback)
        {
            return ToCallbackPtr<IAsyncCallback>(callback);
        }

        public class AsyncCallbackVtbl : ComObjectVtbl
        {
            public AsyncCallbackVtbl() : base(2)
            {
                AddMethod(new GetParametersDelegate(GetParametersImpl));
                AddMethod(new InvokeDelegate(InvokeImpl));
            }

            /// <unmanaged>HRESULT IMFAsyncCallback::GetParameters([Out] MFASYNC_CALLBACK_FLAGS* pdwFlags,[Out] unsigned int* pdwQueue)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int GetParametersDelegate(IntPtr thisPtr, out AsyncCallbackFlags flags, out WorkQueueId workQueueId);
            private static int GetParametersImpl(IntPtr thisPtr, out AsyncCallbackFlags flags, out WorkQueueId workQueueId)
            {
                flags = AsyncCallbackFlags.None;
                workQueueId = WorkQueueId.Standard;
                try
                {
                    var shadow = ToShadow<AsyncCallbackShadow>(thisPtr);
                    var callback = (IAsyncCallback)shadow.Callback;
                    workQueueId = callback.WorkQueueId;
                    flags = callback.Flags;
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT IMFAsyncCallback::Invoke([In, Optional] IMFAsyncResult* pAsyncResult)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int InvokeDelegate(IntPtr thisPtr, IntPtr asyncResult);
            private static int InvokeImpl(IntPtr thisPtr, IntPtr asyncResultPtr)
            {
                AsyncResult asyncResult = null;
                try
                {
                    var shadow = ToShadow<AsyncCallbackShadow>(thisPtr);
                    var callback = (IAsyncCallback) shadow.Callback;
                    asyncResult = new AsyncResult(asyncResultPtr);
                    callback.Invoke(asyncResult);
                }
                catch (Exception exception)
                {
                    return (int) SharpDX.Result.GetResultFromException(exception);
                }
                finally
                {
                    // Clear the NativePointer to make sure that there will be no false indication from ObjectTracker
                    if (asyncResult != null)
                    {
                        asyncResult.NativePointer = IntPtr.Zero;
                    }
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
