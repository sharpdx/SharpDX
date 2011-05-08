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

namespace SharpDX.DirectWrite
{
    /// <summary>
    /// Internal TextAnalysisSink Callback
    /// </summary>
    internal class TextAnalysisSinkCallback : SharpDX.ComObjectCallback
    {
        /// <summary>
        /// Gets or sets the callback.
        /// </summary>
        /// <value>The callback.</value>
        public TextAnalysisSink Callback { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextAnalysisSinkCallback"/> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public TextAnalysisSinkCallback(TextAnalysisSink callback) : base(callback, 4)
        {
            Callback = callback;
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
        private unsafe int SetScriptAnalysisImpl(IntPtr thisPtr, int textPosition, int textLength, SharpDX.DirectWrite.ScriptAnalysis* scriptAnalysis)
        {
            try
            {
                Callback.SetScriptAnalysis(textPosition, textLength, *scriptAnalysis);
            }
            catch (SharpDXException exception)
            {
                return exception.ResultCode.Code;
            }
            catch (Exception)
            {
                return Result.Fail.Code;
            }
            return Result.Ok.Code;            
        }


        /// <unmanaged>HRESULT IDWriteTextAnalysisSink::SetLineBreakpoints([None] int textPosition,[None] int textLength,[In, Buffer] const DWRITE_LINE_BREAKPOINT* lineBreakpoints)</unmanaged>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private unsafe delegate int SetLineBreakpointsDelegate(IntPtr thisPtr, int textPosition, int textLength, IntPtr pLineBreakpoints);
        private unsafe int SetLineBreakpointsImpl(IntPtr thisPtr, int textPosition, int textLength, IntPtr pLineBreakpoints)
        {
            try
            {
                var lineBreakpoints = new LineBreakpoint[textLength];
                Utilities.Read(pLineBreakpoints, lineBreakpoints, 0, textLength);
                Callback.SetLineBreakpoints(textPosition, textLength, lineBreakpoints);
            }
            catch (SharpDXException exception)
            {
                return exception.ResultCode.Code;
            }
            catch (Exception)
            {
                return Result.Fail.Code;
            }
            return Result.Ok.Code;
        }

        /// <unmanaged>HRESULT IDWriteTextAnalysisSink::SetBidiLevel([None] int textPosition,[None] int textLength,[None] int explicitLevel,[None] int resolvedLevel)</unmanaged>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int SetBidiLevelDelegate(IntPtr thisPtr, int textPosition, int textLength, byte explicitLevel, byte resolvedLevel);
        private int SetBidiLevelImpl(IntPtr thisPtr, int textPosition, int textLength, byte explicitLevel, byte resolvedLevel)
        {
            try
            {
                Callback.SetBidiLevel(textPosition, textLength, explicitLevel, resolvedLevel);
            }
            catch (SharpDXException exception)
            {
                return exception.ResultCode.Code;
            }
            catch (Exception)
            {
                return Result.Fail.Code;
            }
            return Result.Ok.Code;
        }

        /// <unmanaged>HRESULT IDWriteTextAnalysisSink::SetNumberSubstitution([None] int textPosition,[None] int textLength,[None] IDWriteNumberSubstitution* numberSubstitution)</unmanaged>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private unsafe delegate int SetNumberSubstitutionDelegate(IntPtr thisPtr, int textPosition, int textLength, IntPtr numberSubstitution);
        private unsafe int SetNumberSubstitutionImpl(IntPtr thisPtr, int textPosition, int textLength, IntPtr numberSubstitution)
        {
            try
            {
                Callback.SetNumberSubstitution(textPosition, textLength, new NumberSubstitution(numberSubstitution));
            }
            catch (SharpDXException exception)
            {
                return exception.ResultCode.Code;
            }
            catch (Exception)
            {
                return Result.Fail.Code;
            }
            return Result.Ok.Code;
        }
    }
}