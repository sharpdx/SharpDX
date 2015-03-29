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

namespace SharpDX.Direct2D1
{
    /// <summary>
    /// Internal TransformNode Callback
    /// </summary>
    internal class TransformNodeShadow : SharpDX.ComObjectShadow
    {
        private static readonly TransformNodeVtbl Vtbl = new TransformNodeVtbl(0);

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(TransformNode callback)
        {
            return ToCallbackPtr<TransformNode>(callback);
        }

        public class TransformNodeVtbl : ComObjectVtbl
        {
            public TransformNodeVtbl(int methods) : base(1 + methods)
            {
                AddMethod(new GetInputCountDelegate(GetInputCountImpl));
            }

            /// <unmanaged>unsigned int ID2D1TransformNode::GetInputCount()</unmanaged>	
            /// <unmanaged>HRESULT ID2D1EffectImpl::Initialize([In] ID2D1EffectContext* effectContext,[In] ID2D1TransformGraph* transformGraph)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int GetInputCountDelegate(IntPtr thisPtr);
            private static int GetInputCountImpl(IntPtr thisPtr)
            {
                var shadow = ToShadow<TransformNodeShadow>(thisPtr);
                var callback = (TransformNode)shadow.Callback;
                return callback.InputCount;
            }
       }

        protected override CppObjectVtbl GetVtbl
        {
            get { return Vtbl; }
        }
    }
}