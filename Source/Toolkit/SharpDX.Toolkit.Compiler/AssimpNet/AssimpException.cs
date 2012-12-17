/*
* Copyright (c) 2012 Nicholas Woodfield
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;

namespace Assimp {
    /// <summary>
    /// Assimp.NET general exception.
    /// </summary>
    internal class AssimpException : Exception {

        /// <summary>
        /// Initializes a new instance of the <see cref="AssimpException"/> class.
        /// </summary>
        public AssimpException() : base() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="AssimpException"/> class.
        /// </summary>
        /// <param name="msg">The error message.</param>
        public AssimpException(String msg) : base(msg) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="AssimpException"/> class.
        /// </summary>
        /// <param name="paramName">Name of the param.</param>
        /// <param name="msg">The error message.</param>
        public AssimpException(String paramName, String msg)
            : base("Parameter: " + paramName + " Error: " + msg) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssimpException"/> class.
        /// </summary>
        /// <param name="msg">The error message</param>
        /// <param name="innerException">The inner exception.</param>
        public AssimpException(String msg, Exception innerException) : base(msg, innerException) {}
    }
}
