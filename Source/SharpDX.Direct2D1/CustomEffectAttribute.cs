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
using System.Collections.Generic;
using System.Text;

namespace SharpDX.Direct2D1
{
    /// <summary>
    /// Global attribute for <see cref="CustomEffect"/> description.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class CustomEffectAttribute : Attribute
    {
        private string description;
        private string category;
        private string author;

        /// <summary>
        /// Initializes a new instance of <see cref="CustomEffectAttribute"/> class.
        /// </summary>
        /// <param name="description">Description of the custom effect</param>
        /// <param name="category">Category of the custom effect</param>
        /// <param name="author">Author of the custom effect</param>
        public CustomEffectAttribute(string description, string category, string author)
        {
            this.description = description;
            this.category = category;
            this.author = author;
        }

        /// <summary>
        /// Gets the DisplayName name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets the Description name.
        /// </summary>
        public string Description { get { return description; } }

        /// <summary>
        /// Gets the Category name.
        /// </summary>
        public string Category { get { return category; } }

        /// <summary>
        /// Gets the Author name.
        /// </summary>
        public string Author { get { return author; } }
    }
}