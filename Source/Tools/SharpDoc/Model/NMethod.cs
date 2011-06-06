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
using System.Collections.Generic;

namespace SharpDoc.Model
{
    /// <summary>
    /// A method member.
    /// </summary>
    public class NMethod : NMember
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NMethod"/> class.
        /// </summary>
        public NMethod()
        {
            Parameters = new List<NParameter>();
        }

        /// <summary>
        /// Gets or sets the type of the return.
        /// </summary>
        /// <value>The type of the return.</value>
        public NTypeReference ReturnType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has overrides.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has overrides; otherwise, <c>false</c>.
        /// </value>
        public bool HasOverrides { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is virtual.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is virtual; otherwise, <c>false</c>.
        /// </value>
        public bool IsVirtual { get; set; }

        /// <summary>
        /// Gets or sets the implements method.
        /// </summary>
        /// <value>The implements.</value>
        public INMemberReference Implements { get; set; }

        /// <summary>
        /// Gets or sets the overrides.
        /// </summary>
        /// <value>The overrides.</value>
        public INMemberReference Overrides { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has parameters.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has parameters; otherwise, <c>false</c>.
        /// </value>
        public bool HasParameters { get { return Parameters.Count > 0; } }

            /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public List<NParameter> Parameters { get; private set; }

        /// <summary>
        /// Gets or sets the text of the signature of this method.
        /// </summary>
        /// <value>The text of the signature of this method.</value>
        public string Signature { get; set; }

        /// <summary>
        /// Gets or sets the return description.
        /// </summary>
        /// <value>The return description.</value>
        public string ReturnDescription { get; set; }

        protected internal override void OnDocNodeUpdate()
        {
            base.OnDocNodeUpdate();
            ReturnDescription = DocFromTag("returns");

            // Update DocNode for parameters
            foreach (var parameter in Parameters)
            {
                parameter.DocNode = DocNode;
            }
        }

        /// <summary>
        /// Adds a parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        public void AddParameter(NParameter parameter)
        {
            parameter.ParentMethod = this;
            Parameters.Add(parameter);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("method {0}", FullName);
        }
    }
}