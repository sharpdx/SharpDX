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
#if WIN8
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpDX.Direct2D1
{
    /// <summary>
    /// Input attribute for <see cref="CustomEffect"/> description.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class PropertyBindingAttribute : Attribute
    {
        private string displayName;
        private string min;
        private string max;
        private string defaultValue;

        /// <summary>
        /// Initializes a new instance of <see cref="PropertyBindingAttribute"/> attribute.
        /// </summary>
        /// <param name="displayName">Display name</param>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        /// <param name="defaultValue">Default value</param>
        public PropertyBindingAttribute(string displayName, string min, string max, string defaultValue)
        {
            this.displayName = displayName;
            this.min = min;
            this.max = max;
            this.defaultValue = defaultValue;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="PropertyBindingAttribute"/> attribute.
        /// </summary>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        /// <param name="defaultValue">Default value</param>
        public PropertyBindingAttribute(string min, string max, string defaultValue)
        {
            this.displayName = displayName;
            this.min = min;
            this.max = max;
            this.defaultValue = defaultValue;
        }

        /// <summary>
        /// Gets the DisplayName.
        /// </summary>
        public string DisplayName
        {
            get
            {
                return displayName;
            }
        }

        /// <summary>
        /// Gets the Min value.
        /// </summary>
        public string Min
        {
            get
            {
                return min;
            }
        }

        /// <summary>
        /// Gets the Max value.
        /// </summary>
        public string Max
        {
            get
            {
                return max;
            }
        }

        /// <summary>
        /// Gets the Default value.
        /// </summary>
        public string Default
        {
            get
            {
                return defaultValue;
            }
        }
    }
}
#endif