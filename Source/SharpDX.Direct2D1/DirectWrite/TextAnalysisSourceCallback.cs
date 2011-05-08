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
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace SharpDX.DirectWrite
{
   /// <summary>
    /// Internal TextAnalysisSource Callback
    /// </summary>
    internal class TextAnalysisSourceCallback : SharpDX.ComObjectCallback
    {
        /// <summary>
        /// Gets or sets the callback.
        /// </summary>
        /// <value>The callback.</value>
        public TextAnalysisSource Callback { get; private set; }

       private List<IntPtr> _allocatedStrings;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextAnalysisSourceCallback"/> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public TextAnalysisSourceCallback(TextAnalysisSource callback) : base(callback, 5)
        {
            Callback = callback;
            _allocatedStrings = new List<IntPtr>();
            unsafe
            {
                AddMethod(new GetTextAtPositionDelegate(GetTextAtPositionImpl));
                AddMethod(new GetTextBeforePositionDelegate(GetTextBeforePositionImpl));
                AddMethod(new GetParagraphReadingDirectionDelegate(GetParagraphReadingDirectionImpl));
                AddMethod(new GetLocaleNameDelegate(GetLocaleNameImpl));
                AddMethod(new GetNumberSubstitutionDelegate(GetNumberSubstitutionImpl));
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            if (_allocatedStrings != null)
            {
                foreach (var allocatedString in _allocatedStrings)
                    Marshal.FreeHGlobal(allocatedString);
                _allocatedStrings = null;
            }
            base.Dispose();
        }

       /// <unmanaged>HRESULT IDWriteTextAnalysisSource::GetTextAtPosition([None] int textPosition,[Out] const wchar_t** textString,[Out] int* textLength)</unmanaged>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int GetTextAtPositionDelegate(IntPtr thisPtr, int textPosition, out IntPtr textString, out int textLength);
        private int GetTextAtPositionImpl(IntPtr thisPtr, int textPosition, out IntPtr textString, out int textLength)
        {
            textString = IntPtr.Zero;
            textLength = 0;
            try
            {
                string textToReturn = Callback.GetTextAtPosition(textPosition);
                if (textToReturn != null)
                {
                    textString = Marshal.StringToHGlobalUni(textToReturn);
                    textLength = textToReturn.Length;
                    _allocatedStrings.Add(textString);
                }
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


        /// <unmanaged>HRESULT IDWriteTextAnalysisSource::GetTextBeforePosition([None] int textPosition,[Out] const wchar_t** textString,[Out] int* textLength)</unmanaged>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int GetTextBeforePositionDelegate(IntPtr thisPtr, int textPosition, out IntPtr textString, out int textLength);
        private int GetTextBeforePositionImpl(IntPtr thisPtr, int textPosition, out IntPtr textString, out int textLength)
        {
            textString = IntPtr.Zero;
            textLength = 0;
            try
            {
                string textToReturn = Callback.GetTextBeforePosition(textPosition);
                if (textToReturn != null)
                {
                    textString = Marshal.StringToHGlobalUni(textToReturn);
                    textLength = textToReturn.Length;
                    _allocatedStrings.Add(textString);
                }
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

        /// <unmanaged>DWRITE_READING_DIRECTION IDWriteTextAnalysisSource::GetParagraphReadingDirection()</unmanaged>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate ReadingDirection GetParagraphReadingDirectionDelegate(IntPtr thisPtr);
        private ReadingDirection GetParagraphReadingDirectionImpl(IntPtr thisPtr)
        {
            return Callback.ReadingDirection;
        }

        /// <unmanaged>HRESULT IDWriteTextAnalysisSource::GetLocaleName([None] int textPosition,[Out] int* textLength,[Out] const wchar_t** localeName)</unmanaged>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int GetLocaleNameDelegate(IntPtr thisPtr, int textPosition, out int textLength, out IntPtr textString);
        private int GetLocaleNameImpl(IntPtr thisPtr, int textPosition, out int textLength, out IntPtr textString)
        {
            textString = IntPtr.Zero;
            textLength = 0;
            try
            {
                string textToReturn = Callback.GetLocaleName(textPosition);
                if (textToReturn != null)
                {
                    textString = Marshal.StringToHGlobalUni(textToReturn);
                    textLength = textToReturn.Length;
                    _allocatedStrings.Add(textString);
                }
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

        /// <unmanaged>HRESULT IDWriteTextAnalysisSource::GetNumberSubstitution([None] int textPosition,[Out] int* textLength,[Out] IDWriteNumberSubstitution** numberSubstitution)</unmanaged>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int GetNumberSubstitutionDelegate(IntPtr thisPtr, int textPosition, out int textLength, out IntPtr numberSubstitutionPtr);
        private int GetNumberSubstitutionImpl(IntPtr thisPtr, int textPosition, out int textLength, out IntPtr numberSubstitutionPtr)
        {
            numberSubstitutionPtr = IntPtr.Zero;
            textLength = 0;
            try
            {
                var numberSubstitution = Callback.GetNumberSubstitution(textPosition, out textLength);
                numberSubstitutionPtr = (numberSubstitution == null) ? IntPtr.Zero : numberSubstitution.NativePointer;
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