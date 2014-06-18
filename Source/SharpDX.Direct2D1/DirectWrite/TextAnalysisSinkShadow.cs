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
    /// Internal TextAnalysisSink Callback
    /// </summary>
    internal class TextAnalysisSinkShadow : SharpDX.ComObjectShadow
    {
        private static readonly TextAnalysisSinkVtbl Vtbl = new TextAnalysisSinkVtbl();

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(TextAnalysisSink callback)
        {
            return ToCallbackPtr<TextAnalysisSink>(callback);
        }

        protected class TextAnalysisSinkVtbl : ComObjectVtbl
        {
            public TextAnalysisSinkVtbl(int methodCount = 0) : base(4 + methodCount)
            {
                unsafe
                {
                    AddMethod(new SetScriptAnalysisDelegate(SetScriptAnalysisImpl));
                    AddMethod(new SetLineBreakpointsDelegate(SetLineBreakpointsImpl));
                    AddMethod(new SetBidiLevelDelegate(SetBidiLevelImpl));
                    AddMethod(new SetNumberSubstitutionDelegate(SetNumberSubstitutionImpl));
                }
            }

            /// <unmanaged>HRESULT IDWriteTextAnalysisSink::SetScriptAnalysis([None] int textPosition,[None] int textLength,[In] const DWRITE_SCRIPT_ANALYSIS* scriptAnalysis)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private unsafe delegate int SetScriptAnalysisDelegate(IntPtr thisPtr, int textPosition, int textLength, SharpDX.DirectWrite.ScriptAnalysis* scriptAnalysis);
            private static unsafe int SetScriptAnalysisImpl(IntPtr thisPtr, int textPosition, int textLength, SharpDX.DirectWrite.ScriptAnalysis* scriptAnalysis)
            {
                try
                {
                    var shadow = ToShadow<TextAnalysisSinkShadow>(thisPtr);
                    var callback = (TextAnalysisSink)shadow.Callback; 
                    callback.SetScriptAnalysis(textPosition, textLength, *scriptAnalysis);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }


            /// <unmanaged>HRESULT IDWriteTextAnalysisSink::SetLineBreakpoints([None] int textPosition,[None] int textLength,[In, Buffer] const DWRITE_LINE_BREAKPOINT* lineBreakpoints)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private unsafe delegate int SetLineBreakpointsDelegate(IntPtr thisPtr, int textPosition, int textLength, IntPtr pLineBreakpoints);
            private static unsafe int SetLineBreakpointsImpl(IntPtr thisPtr, int textPosition, int textLength, IntPtr pLineBreakpoints)
            {
                try
                {
                    var shadow = ToShadow<TextAnalysisSinkShadow>(thisPtr);
                    var callback = (TextAnalysisSink)shadow.Callback;
                    var lineBreakpoints = new LineBreakpoint[textLength];
                    Utilities.Read(pLineBreakpoints, lineBreakpoints, 0, textLength);
                    callback.SetLineBreakpoints(textPosition, textLength, lineBreakpoints);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT IDWriteTextAnalysisSink::SetBidiLevel([None] int textPosition,[None] int textLength,[None] int explicitLevel,[None] int resolvedLevel)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int SetBidiLevelDelegate(IntPtr thisPtr, int textPosition, int textLength, byte explicitLevel, byte resolvedLevel);
            private static int SetBidiLevelImpl(IntPtr thisPtr, int textPosition, int textLength, byte explicitLevel, byte resolvedLevel)
            {
                try
                {
                    var shadow = ToShadow<TextAnalysisSinkShadow>(thisPtr);
                    var callback = (TextAnalysisSink)shadow.Callback;
                    callback.SetBidiLevel(textPosition, textLength, explicitLevel, resolvedLevel);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT IDWriteTextAnalysisSink::SetNumberSubstitution([None] int textPosition,[None] int textLength,[None] IDWriteNumberSubstitution* numberSubstitution)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private unsafe delegate int SetNumberSubstitutionDelegate(IntPtr thisPtr, int textPosition, int textLength, IntPtr numberSubstitution);
            private static unsafe int SetNumberSubstitutionImpl(IntPtr thisPtr, int textPosition, int textLength, IntPtr numberSubstitution)
            {
                try
                {
                    var shadow = ToShadow<TextAnalysisSinkShadow>(thisPtr);
                    var callback = (TextAnalysisSink)shadow.Callback;
                    callback.SetNumberSubstitution(textPosition, textLength, new NumberSubstitution(numberSubstitution));
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