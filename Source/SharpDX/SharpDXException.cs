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
using System.Globalization;

namespace SharpDX
{
    /// <summary>
    ///   The base class for errors that occur in SharpDX.
    /// </summary>
    public class SharpDXException : Exception
    {
        private ResultDescriptor descriptor;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.SharpDXException" /> class.
        /// </summary>
        public SharpDXException() : base("A SharpDX exception occurred.")
        {
            this.descriptor = ResultDescriptor.Find(Result.Fail);
            HResult = (int)Result.Fail;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.SharpDXException" /> class.
        /// </summary>
        /// <param name = "result">The result code that caused this exception.</param>
        public SharpDXException(Result result)
            : this(ResultDescriptor.Find(result))
        {
            HResult = (int)result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpDX.SharpDXException"/> class.
        /// </summary>
        /// <param name="descriptor">The result descriptor.</param>
        public SharpDXException(ResultDescriptor descriptor)
            : base(descriptor.ToString())
        {
            this.descriptor = descriptor;
            HResult = (int)descriptor.Result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpDX.SharpDXException"/> class.
        /// </summary>
        /// <param name="result">The error result code.</param>
        /// <param name="message">The message describing the exception.</param>
        public SharpDXException(Result result, string message)
            : base(message)
        {
            this.descriptor = ResultDescriptor.Find(result);
            HResult = (int)result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpDX.SharpDXException"/> class.
        /// </summary>
        /// <param name="result">The error result code.</param>
        /// <param name="message">The message describing the exception.</param>
        /// <param name="args">formatting arguments</param>
        public SharpDXException(Result result, string message, params object[] args)
            : base(string.Format(CultureInfo.InvariantCulture, message, args))
        {
            this.descriptor = ResultDescriptor.Find(result);
            HResult = (int)result;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.SharpDXException" /> class.
        /// </summary>
        /// <param name = "message">The message describing the exception.</param>
        /// <param name="args">formatting arguments</param>
        public SharpDXException(string message, params object[] args) : this(Result.Fail, message, args)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.SharpDXException" /> class.
        /// </summary>
        /// <param name = "message">The message describing the exception.</param>
        /// <param name = "innerException">The exception that caused this exception.</param>
        /// <param name="args">formatting arguments</param>
        public SharpDXException(string message, Exception innerException, params object[] args)
            : base(string.Format(CultureInfo.InvariantCulture, message, args), innerException)
        {
            this.descriptor = ResultDescriptor.Find(Result.Fail);
            HResult = (int)Result.Fail;
        }

        /// <summary>
        ///   Gets the <see cref = "T:SharpDX.Result">Result code</see> for the exception. This value indicates
        ///   the specific type of failure that occurred within SharpDX.
        /// </summary>
        public Result ResultCode
        {
            get { return this.descriptor.Result; }
        }

        /// <summary>
        ///   Gets the <see cref = "T:SharpDX.Result">Result code</see> for the exception. This value indicates
        ///   the specific type of failure that occurred within SharpDX.
        /// </summary>
        public ResultDescriptor Descriptor
        {
            get { return this.descriptor; }
        }
    }
}