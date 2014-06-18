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

namespace SharpDX.DirectWrite
{
    /// <summary>
    /// Internal InlineObject Callback
    /// </summary>
    internal class InlineObjectShadow : SharpDX.ComObjectShadow
    {
        private static readonly InlineObjectVtbl Vtbl = new InlineObjectVtbl();

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(InlineObject callback)
        {
            return ToCallbackPtr<InlineObject>(callback);
        }

        private class InlineObjectVtbl : ComObjectVtbl
        {
            public InlineObjectVtbl() : base(4)
            {
                unsafe
                {
                    AddMethod(new DrawDelegate(DrawImpl));
                    AddMethod(new GetMetricsDelegate(GetMetricsImpl));
                    AddMethod(new GetOverhangMetricsDelegate(GetOverhangMetricsImpl));
                    AddMethod(new GetBreakConditionsDelegate(GetBreakConditionsImpl));
                }
            }

            /// <unmanaged>HRESULT IDWriteInlineObject::Draw([None] void* clientDrawingContext,[None] IDWriteTextRenderer* renderer,[None] float originX,[None] float originY,[None] BOOL isSideways,[None] BOOL isRightToLeft,[None] IUnknown* clientDrawingEffect)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int DrawDelegate(
                IntPtr thisPtr, IntPtr clientDrawingContextPtr, IntPtr renderer, float originX, float originY, int isSideways, int isRightToLeft,
                IntPtr clientDrawingEffectPtr);
            private static int DrawImpl(
                IntPtr thisPtr, IntPtr clientDrawingContextPtr, IntPtr renderer, float originX, float originY, int isSideways, int isRightToLeft,
                IntPtr clientDrawingEffectPtr)
            {
                try
                {
                    var shadow = ToShadow<InlineObjectShadow>(thisPtr);
                    var callback = (InlineObject)shadow.Callback;

                    var textRendererCallback = ToShadow<TextRendererShadow>(renderer); 
                    callback.Draw(GCHandle.FromIntPtr(clientDrawingContextPtr).Target, (TextRenderer)textRendererCallback.Callback, originX, originY, isSideways != 0, isRightToLeft != 0, (ComObject)Utilities.GetObjectForIUnknown(clientDrawingEffectPtr));
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT IDWriteInlineObject::GetMetrics([Out] DWRITE_INLINE_OBJECT_METRICS* metrics)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private unsafe delegate int GetMetricsDelegate(IntPtr thisPtr, SharpDX.DirectWrite.InlineObjectMetrics* pMetrics);
            private static unsafe int GetMetricsImpl(IntPtr thisPtr, SharpDX.DirectWrite.InlineObjectMetrics* pMetrics)
            {
                try
                {
                    var shadow = ToShadow<InlineObjectShadow>(thisPtr);
                    var callback = (InlineObject)shadow.Callback;
                    *pMetrics = callback.Metrics;
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT IDWriteInlineObject::GetOverhangMetrics([Out] DWRITE_OVERHANG_METRICS* overhangs)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private unsafe delegate int GetOverhangMetricsDelegate(IntPtr thisPtr, SharpDX.DirectWrite.OverhangMetrics* pOverhangs);
            private static unsafe int GetOverhangMetricsImpl(IntPtr thisPtr, SharpDX.DirectWrite.OverhangMetrics* pOverhangs)
            {
                try
                {
                    var shadow = ToShadow<InlineObjectShadow>(thisPtr);
                    var callback = (InlineObject)shadow.Callback;
                    *pOverhangs = callback.OverhangMetrics;
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT IDWriteInlineObject::GetBreakConditions([Out] DWRITE_BREAK_CONDITION* breakConditionBefore,[Out] DWRITE_BREAK_CONDITION* breakConditionAfter)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int GetBreakConditionsDelegate(IntPtr thisPtr,
                out SharpDX.DirectWrite.BreakCondition breakConditionBefore, out SharpDX.DirectWrite.BreakCondition breakConditionAfter);

            private static int GetBreakConditionsImpl(IntPtr thisPtr,
                out SharpDX.DirectWrite.BreakCondition breakConditionBefore, out SharpDX.DirectWrite.BreakCondition breakConditionAfter)
            {
                breakConditionBefore = BreakCondition.Neutral;
                breakConditionAfter = BreakCondition.Neutral;
                try
                {
                    var shadow = ToShadow<InlineObjectShadow>(thisPtr);
                    var callback = (InlineObject)shadow.Callback;
                    callback.GetBreakConditions(out breakConditionBefore, out breakConditionAfter);
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