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

namespace SharpDX.Win32
{
    public partial class ErrorCodeHelper
    {
        /// <summary>
        /// Converts a win32 error code to a <see cref="Result"/>.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <returns>A HRESULT code</returns>
        public static Result ToResult(ErrorCode errorCode)
        {
            return ToResult((int)errorCode);
        }
        
        /// <summary>
        /// Converts a win32 error code to a <see cref="Result"/>.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <returns>A HRESULT code</returns>
        public static Result ToResult(int errorCode)
        {
            return new Result(((errorCode <= 0) ? unchecked((uint)errorCode) : ((unchecked((uint)errorCode) & 0x0000FFFF) | 0x80070000)));
        }
    }
}

