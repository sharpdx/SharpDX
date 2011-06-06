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

namespace SharpDoc.Model
{
    /// <summary>
    /// A parameter document.
    /// </summary>
    public class NParameter : NModelBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is in.
        /// </summary>
        /// <value><c>true</c> if this instance is in; otherwise, <c>false</c>.</value>
        public bool IsIn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is optional.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is optional; otherwise, <c>false</c>.
        /// </value>
        public bool IsOptional { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is out.
        /// </summary>
        /// <value><c>true</c> if this instance is out; otherwise, <c>false</c>.</value>
        public bool IsOut { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is return value.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is return value; otherwise, <c>false</c>.
        /// </value>
        public bool IsReturnValue { get; set; }

        /// <summary>
        /// Gets or sets the type of the parameter.
        /// </summary>
        /// <value>The type of the parameter.</value>
        public NTypeReference ParameterType { get; set; }

        /// <summary>
        /// Gets or sets the parent method.
        /// </summary>
        /// <value>The parent method.</value>
        public NMethod ParentMethod { get; set; }

        protected internal override void OnDocNodeUpdate()
        {
            // Don't call base, as we need to get param description from param tag
            // base.OnDocNodeUpdate();
            if (DocNode != null)
            {
                var selectedNode = DocNode.SelectSingleNode("param[@name='" + Name + "']");
                if (selectedNode != null)
                    Description = selectedNode.InnerXml.Trim();
            }
        }
    }
}