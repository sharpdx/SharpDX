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
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace SharpDX
{
    /// <summary>
    ///   The base class for errors that occur in SharpDX.
    /// </summary>
    /// <unmanaged>None</unmanaged>
    [Serializable]
    public class SharpDXException : Exception
    {
        private Result m_Result;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.SharpDXException" /> class.
        /// </summary>
        public SharpDXException()
            : base("A SharpDX exception occurred.")
        {
            this.m_Result = Result.Fail;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.SharpDXException" /> class.
        /// </summary>
        /// <param name = "result">The result code that caused this exception.</param>
        public SharpDXException(Result result)
            : this(result, result.ToString())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpDX.SharpDXException"/> class.
        /// </summary>
        /// <param name="result">The error result code.</param>
        /// <param name="message">The message describing the exception.</param>
        /// <param name="args">formatting arguments</param>
        public SharpDXException(Result result, string message, params object[] args)
            : base(string.Format(message, args))
        {
            this.m_Result = result;
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
        /// Initializes a new instance of the <see cref="SharpDXException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected SharpDXException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.m_Result = (Result) info.GetValue("Result", typeof (Result));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.SharpDXException" /> class.
        /// </summary>
        /// <param name = "message">The message describing the exception.</param>
        /// <param name = "innerException">The exception that caused this exception.</param>
        /// <param name="args">formatting arguments</param>
        public SharpDXException(string message, Exception innerException, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            this.m_Result = Result.Fail;
        }

        /// <summary>
        ///   When overridden in a derived class, sets the <see cref = "T:System.Runtime.Serialization.SerializationInfo" /> with information about the exception.
        /// </summary>
        /// <param name = "info">The <see cref = "T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name = "context">The <see cref = "T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            info.AddValue("Result", this.m_Result);
            base.GetObjectData(info, context);
        }

        /// <summary>
        ///   Gets the <see cref = "T:SharpDX.Result">Result code</see> for the exception. This value indicates
        ///   the specific type of failure that occured within SharpDX.
        /// </summary>
        public Result ResultCode
        {
            get { return this.m_Result; }
        }
    }
}