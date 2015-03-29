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
    /// Internal CustomEffect Callback
    /// </summary>
    internal class CustomEffectShadow : SharpDX.ComObjectShadow
    {
        private static readonly CustomEffectVtbl Vtbl = new CustomEffectVtbl();

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(CustomEffect callback)
        {
            return ToCallbackPtr<CustomEffect>(callback);
        }

        public class CustomEffectVtbl : ComObjectVtbl
        {
            public CustomEffectVtbl() : base(3)
            {
                AddMethod(new InitializeDelegate(InitializeImpl));
                AddMethod(new PrepareForRenderDelegate(PrepareForRenderImpl));
                AddMethod(new SetGraphDelegate(SetGraphImpl));
            }

            /// <unmanaged>HRESULT ID2D1EffectImpl::Initialize([In] ID2D1EffectContext* effectContext,[In] ID2D1TransformGraph* transformGraph)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int InitializeDelegate(IntPtr thisPtr, IntPtr effectContext, IntPtr transformationGraph);
            private static int InitializeImpl(IntPtr thisPtr, IntPtr effectContext, IntPtr transformationGraph)
            {
                try
                {
                    var shadow = ToShadow<CustomEffectShadow>(thisPtr);
                    var callback = (CustomEffect)shadow.Callback;
                    callback.Initialize(new EffectContext(effectContext), new TransformGraph(transformationGraph));
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }
            

            /// <unmanaged>HRESULT ID2D1EffectImpl::PrepareForRender([In] D2D1_CHANGE_TYPE changeType)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int PrepareForRenderDelegate(IntPtr thisPtr, SharpDX.Direct2D1.ChangeType changeType);
            private static int PrepareForRenderImpl(IntPtr thisPtr, SharpDX.Direct2D1.ChangeType changeType)
            {
                try
                {
                    var shadow = ToShadow<CustomEffectShadow>(thisPtr);
                    var callback = (CustomEffect)shadow.Callback;
                    callback.PrepareForRender(changeType);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <summary>	
            /// The renderer calls this method to provide the effect implementation with a way to specify its transform graph and transform graph changes. 
            /// The renderer calls this method when: 1) When the effect is first initialized. 2) If the number of inputs to the effect changes.
            /// </summary>	
            /// <param name="transformGraph">The graph to which the effect describes its transform topology through the SetDescription call..</param>	
            /// <unmanaged>HRESULT ID2D1EffectImpl::SetGraph([In] ID2D1TransformGraph* transformGraph)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int SetGraphDelegate(IntPtr thisPtr, IntPtr transformGraph);
            private static int SetGraphImpl(IntPtr thisPtr, IntPtr transformGraph)
            {
                try
                {
                    var shadow = ToShadow<CustomEffectShadow>(thisPtr);
                    var callback = (CustomEffect)shadow.Callback;
                    callback.SetGraph(new TransformGraph(transformGraph));
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