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
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpDX
{
    /// <summary>
    /// Result structure for COM methods.
    /// </summary>
#if !WIN8
    [Serializable]
#endif
    [StructLayout(LayoutKind.Sequential)]
    public struct Result : IEquatable<Result>
    {
        private int _code;

        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> struct.
        /// </summary>
        /// <param name="code">The HRESULT error code.</param>
        public Result(int code)
        {
            _code = code;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> struct.
        /// </summary>
        /// <param name="code">The HRESULT error code.</param>
        public Result(uint code)
        {
            _code = unchecked((int)code);
        }

        /// <summary>
        /// Gets the HRESULT error code.
        /// </summary>
        /// <value>The HRESULT error code.</value>
        public int Code
        {
            get { return _code; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Result"/> is success.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Success
        {
            get { return Code >= 0; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Result"/> is failure.
        /// </summary>
        /// <value><c>true</c> if failure; otherwise, <c>false</c>.</value>
        public bool Failure
        {
            get { return Code < 0; }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Int32"/> to <see cref="SharpDX.Result"/>.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Result(int result)
        {
            return new Result(result);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.UInt32"/> to <see cref="SharpDX.Result"/>.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Result(uint result)
        {
            return new Result(result);
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(Result other)
        {
            return this.Code == other.Code;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Result))
                return false;
            return Equals((Result) obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Code;
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Result left, Result right)
        {
            return left.Code == right.Code;
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Result left, Result right)
        {
            return left.Code != right.Code;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "HRESULT = 0x{0:X}", _code);
        }

        /// <summary>
        /// Checks the error.
        /// </summary>
        public void CheckError()
        {
            if (_code < 0)
            {
                throw new SharpDXException(this);
            }
        }

        /// <summary>
        /// Result code Ok
        /// </summary>
        public static Result Ok = new Result(unchecked((int)0x00000000));

        /// <summary>
        /// Result code False
        /// </summary>
        public static Result False = new Result(unchecked((int)0x00000001));

        /// <summary>
        /// Result code Abord
        /// </summary>
        public static Result Abord = new Result(unchecked((int)0x80004004));

        /// <summary>
        /// Result code AccessDenied
        /// </summary>
        public static Result AccessDenied = new Result(unchecked((int)0x80070005));

        /// <summary>
        /// Result code Fail
        /// </summary>
        public static Result Fail = new Result(unchecked((int)0x80004005));

        /// <summary>
        /// Resuld code Handle
        /// </summary>
        public static Result Handle = new Result(unchecked((int)0x80070006));

        /// <summary>
        /// Result code invalid argument
        /// </summary>
        public static Result InvalidArg = new Result(unchecked((int)0x80070057));

        /// <summary>
        /// Result code no interface
        /// </summary>
        public static Result NoInterface = new Result(unchecked((int)0x80004002));

        /// <summary>
        /// Result code not implemented
        /// </summary>
        public static Result NotImplemented = new Result(unchecked((int)0x80004001));

        /// <summary>
        /// Result code out of memory
        /// </summary>
        public static Result OutOfMemory = new Result(unchecked((int)0x8007000E));

        /// <summary>
        /// Result code Invalid pointer
        /// </summary>
        public static Result InvalidPointer = new Result(unchecked((int)0x80004003));

        /// <summary>
        /// Unexpected failure
        /// </summary>
        public static Result UnexpectedFailure = new Result(unchecked((int)0x8000FFFF));
    }
}