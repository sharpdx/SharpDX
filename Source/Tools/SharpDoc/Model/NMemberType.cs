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

namespace SharpDoc.Model
{
    /// <summary>
    /// Type of a member.
    /// </summary>
    public enum NMemberType
    {
        /// <summary>
        /// An Interface member.
        /// </summary>
        Interface,
        /// <summary>
        /// A Class member.
        /// </summary>
        Class,
        /// <summary>
        /// A Struct member.
        /// </summary>
        Struct,
        /// <summary>
        /// An Enum member.
        /// </summary>
        Enum,
        /// <summary>
        /// A Constructor member.
        /// </summary>
        Constructor,
        /// <summary>
        /// A Method member.
        /// </summary>
        Method,
        /// <summary>
        /// An operator member.
        /// </summary>
        Operator,
        /// <summary>
        /// A Field member.
        /// </summary>
        Field,
        /// <summary>
        /// A Property member.
        /// </summary>
        Property,
        /// <summary>
        /// An Event member.
        /// </summary>
        Event,
        /// <summary>
        /// A Delegate member.
        /// </summary>
        Delegate,    
    }
}