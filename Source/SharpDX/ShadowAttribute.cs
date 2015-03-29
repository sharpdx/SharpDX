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
using System.Reflection;

namespace SharpDX
{
    /// <summary>
    /// Shadow attribute used to associate a COM callbackable interface to its Shadow implementation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    internal class ShadowAttribute : Attribute
    {
        private Type type;

        /// <summary>
        /// Gets the value.
        /// </summary>
        public Type Type
        {
            get
            {
                return type;
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ShadowAttribute"/> class.
        /// </summary>
        /// <param name="typeOfTheAssociatedShadow">Type of the associated shadow</param>
        public ShadowAttribute(Type typeOfTheAssociatedShadow)
        {
            type = typeOfTheAssociatedShadow;
        }

        /// <summary>
        /// Get ShadowAttribute from type.
        /// </summary>
        /// <param name="type">Type to get shadow attribute</param>
        /// <returns>The associated shadow attribute or null if no shadow attribute were found</returns>
        public static ShadowAttribute Get(Type type)
        {
            return type.GetTypeInfo().GetCustomAttribute<ShadowAttribute>();
        }
    }
}
