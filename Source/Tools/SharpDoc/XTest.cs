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
using System.Xml.Serialization;

namespace SharpDoc
{
    /// <summary>
    /// This is the description for this class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// This is the remarks for this class.
    /// </remarks>
    public class XTest<T> : List<T>
    {

        /// <summary>
        /// Loads from.
        /// </summary>
        public void LoadFrom()
        {
            
        }

        /// <summary>
        /// Loads from.
        /// </summary>
        public void LoadFrom(int test)
        {

        }
        /// <summary>
        /// Loads from string.
        /// </summary>
        /// <typeparam name="TMode">The type of the mode.</typeparam>
        /// <param name="toto">The toto.</param>
        /// <param name="tata">The tata.</param>
        /// <param name="tutu">The tutu.</param>
        public void LoadFrom<TMode>(string toto, TMode tata, T tutu) 
        {
            
        }

    }
}