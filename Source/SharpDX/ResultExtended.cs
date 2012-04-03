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
using System.Runtime.InteropServices;

namespace SharpDX
{
    /// <summary>
    /// Extended Result structure used to provide detailed message for a particular <see cref="Result"/>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public sealed class ResultExtended
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultExtended"/> class.
        /// </summary>
        /// <param name="code">The HRESULT error code.</param>
        /// <param name="module">The module (ex: SharpDX.Direct2D1).</param>
        /// <param name="apiCode">The API code (ex: D2D1_ERR_...).</param>
        /// <param name="description">The description of the result code if any.</param>
        public ResultExtended(int code, string module, string apiCode, string description)
        {
            Result = new Result(code);
            Module = module;
            this.ApiCode = apiCode;
            Description = description;
        }

        /// <summary>
        /// Gets the result.
        /// </summary>
        public Result Result { get; private set; }

        /// <summary>
        /// Gets the module (ex: SharpDX.Direct2D1)
        /// </summary>
        public string Module { get; private set; }

        /// <summary>
        /// Gets the API code (ex: D2D1_ERR_...)
        /// </summary>
        public string ApiCode { get; private set; }

        /// <summary>
        /// Gets the description of the result code if any.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SharpDX.ResultExtended"/> to <see cref="SharpDX.Result"/>.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Result(ResultExtended result)
        {
            return result.Result;
        }

        /// <summary>
        /// Determines whether the specified <see cref="ResultExtended"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="ResultExtended"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="ResultExtended"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(ResultExtended other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return other.Result.Equals(this.Result);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(ResultExtended))
                return false;
            return Equals((ResultExtended)obj);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.Result.GetHashCode();
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(ResultExtended left, Result right)
        {
            if (left == null)
                return false;
            return left.Result.Code == right.Code;
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(ResultExtended left, Result right)
        {
            if (left == null)
                return false;
            return left.Result.Code != right.Code;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return string.Format("Result: 0x{0:X}, Module: {1}, ApiCode: {2}, Description: {3}", this.Result.Code, this.Module, this.ApiCode, this.Description);
        }
    }
}