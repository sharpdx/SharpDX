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

namespace SharpDX.Animation
{
    /// <summary>
    /// Internal ManagerEventHandler Callback
    /// </summary>
    internal class ManagerEventHandlerShadow : SharpDX.ComObjectShadow
    {
        private static readonly ManagerEventHandlerVtbl Vtbl = new ManagerEventHandlerVtbl();

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(ManagerEventHandler callback)
        {
            return ToCallbackPtr<ManagerEventHandler>(callback);
        }

        public class ManagerEventHandlerVtbl : ComObjectVtbl
        {
            public ManagerEventHandlerVtbl() : base(1)
            {
                AddMethod(new OnManagerStatusChangedDelegate(OnManagerStatusChangedImpl));
            }

            /// <unmanaged>HRESULT IUIAnimationManagerEventHandler::OnManagerStatusChanged([In] UI_ANIMATION_MANAGER_STATUS newStatus,[In] UI_ANIMATION_MANAGER_STATUS previousStatus)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate SharpDX.Result OnManagerStatusChangedDelegate(IntPtr thisPtr, SharpDX.Animation.ManagerStatus newStatus, SharpDX.Animation.ManagerStatus previousStatus);
            private static SharpDX.Result OnManagerStatusChangedImpl(IntPtr thisPtr, SharpDX.Animation.ManagerStatus newStatus, SharpDX.Animation.ManagerStatus previousStatus)
            {
                try
                {
                    var shadow = ToShadow<ManagerEventHandlerShadow>(thisPtr);
                    var callback = (ManagerEventHandler)shadow.Callback;
                    callback.OnManagerStatusChanged(previousStatus, newStatus);
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