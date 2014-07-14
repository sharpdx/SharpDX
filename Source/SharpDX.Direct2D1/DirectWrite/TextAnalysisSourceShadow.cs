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
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace SharpDX.DirectWrite
{
   /// <summary>
    /// Internal TextAnalysisSource Callback
    /// </summary>
    internal class TextAnalysisSourceShadow : SharpDX.ComObjectShadow
    {
        private static readonly TextAnalysisSourceVtbl Vtbl = new TextAnalysisSourceVtbl();
        private List<IntPtr> allocatedStrings = new List<IntPtr>();

        protected override CppObjectVtbl GetVtbl
        {
            get { return Vtbl; }
        }

        protected override void Dispose(bool disposing)
        {
            if (allocatedStrings != null)
            {
                foreach (var allocatedString in allocatedStrings)
                    Marshal.FreeHGlobal(allocatedString);
                allocatedStrings = null;
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(TextAnalysisSource callback)
        {
            return ToCallbackPtr<TextAnalysisSource>(callback);
        }

        protected class TextAnalysisSourceVtbl : ComObjectVtbl
        {
            public TextAnalysisSourceVtbl(int methodCount = 0)
                : base(5 + methodCount)
            {
                AddMethod(new GetTextAtPositionDelegate(GetTextAtPositionImpl));
                AddMethod(new GetTextBeforePositionDelegate(GetTextBeforePositionImpl));
                AddMethod(new GetParagraphReadingDirectionDelegate(GetParagraphReadingDirectionImpl));
                AddMethod(new GetLocaleNameDelegate(GetLocaleNameImpl));
                AddMethod(new GetNumberSubstitutionDelegate(GetNumberSubstitutionImpl));
            }

            /// <unmanaged>HRESULT IDWriteTextAnalysisSource::GetTextAtPosition([None] int textPosition,[Out] const wchar_t** textString,[Out] int* textLength)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int GetTextAtPositionDelegate(IntPtr thisPtr, int textPosition, out IntPtr textString, out int textLength);
            private static int GetTextAtPositionImpl(IntPtr thisPtr, int textPosition, out IntPtr textString, out int textLength)
            {
                textString = IntPtr.Zero;
                textLength = 0;
                try
                {
                    var shadow = ToShadow<TextAnalysisSourceShadow>(thisPtr);
                    var callback = (TextAnalysisSource)shadow.Callback;
                    string textToReturn = callback.GetTextAtPosition(textPosition);
                    if (textToReturn != null)
                    {
                        textString = Marshal.StringToHGlobalUni(textToReturn);
                        textLength = textToReturn.Length;
                        shadow.allocatedStrings.Add(textString);
                    }
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }


            /// <unmanaged>HRESULT IDWriteTextAnalysisSource::GetTextBeforePosition([None] int textPosition,[Out] const wchar_t** textString,[Out] int* textLength)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int GetTextBeforePositionDelegate(IntPtr thisPtr, int textPosition, out IntPtr textString, out int textLength);
            private static int GetTextBeforePositionImpl(IntPtr thisPtr, int textPosition, out IntPtr textString, out int textLength)
            {
                textString = IntPtr.Zero;
                textLength = 0;
                try
                {
                    var shadow = ToShadow<TextAnalysisSourceShadow>(thisPtr);
                    var callback = (TextAnalysisSource)shadow.Callback;
                    string textToReturn = callback.GetTextBeforePosition(textPosition);
                    if (textToReturn != null)
                    {
                        textString = Marshal.StringToHGlobalUni(textToReturn);
                        textLength = textToReturn.Length;
                        shadow.allocatedStrings.Add(textString);
                    }
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>DWRITE_READING_DIRECTION IDWriteTextAnalysisSource::GetParagraphReadingDirection()</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate ReadingDirection GetParagraphReadingDirectionDelegate(IntPtr thisPtr);
            private static ReadingDirection GetParagraphReadingDirectionImpl(IntPtr thisPtr)
            {
                var shadow = ToShadow<TextAnalysisSourceShadow>(thisPtr);
                var callback = (TextAnalysisSource)shadow.Callback;
                return callback.ReadingDirection;
            }

            /// <unmanaged>HRESULT IDWriteTextAnalysisSource::GetLocaleName([None] int textPosition,[Out] int* textLength,[Out] const wchar_t** localeName)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int GetLocaleNameDelegate(IntPtr thisPtr, int textPosition, out int textLength, out IntPtr textString);
            private static int GetLocaleNameImpl(IntPtr thisPtr, int textPosition, out int textLength, out IntPtr textString)
            {
                textString = IntPtr.Zero;
                textLength = 0;
                try
                {
                    var shadow = ToShadow<TextAnalysisSourceShadow>(thisPtr);
                    var callback = (TextAnalysisSource)shadow.Callback;
                    string textToReturn = callback.GetLocaleName(textPosition, out textLength);
                    if (textToReturn != null)
                    {
                        textString = Marshal.StringToHGlobalUni(textToReturn);
                        shadow.allocatedStrings.Add(textString);
                    }
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT IDWriteTextAnalysisSource::GetNumberSubstitution([None] int textPosition,[Out] int* textLength,[Out] IDWriteNumberSubstitution** numberSubstitution)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int GetNumberSubstitutionDelegate(IntPtr thisPtr, int textPosition, out int textLength, out IntPtr numberSubstitutionPtr);
            private static int GetNumberSubstitutionImpl(IntPtr thisPtr, int textPosition, out int textLength, out IntPtr numberSubstitutionPtr)
            {
                numberSubstitutionPtr = IntPtr.Zero;
                textLength = 0;
                try
                {
                    var shadow = ToShadow<TextAnalysisSourceShadow>(thisPtr);
                    var callback = (TextAnalysisSource)shadow.Callback;
                    var numberSubstitution = callback.GetNumberSubstitution(textPosition, out textLength);
                    numberSubstitutionPtr = (numberSubstitution == null) ? IntPtr.Zero : numberSubstitution.NativePointer;
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }
        }
    }
}