// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
using System.Linq;

namespace SharpGen.Model
{
    /// <summary>
    /// A Namespace container.
    /// </summary>
    public class CsNamespace : CsBase
    {
        public CsNamespace(CsBase parentContainer, string nameSpace)
        {
            Name = nameSpace;
            Parent = parentContainer;
        }

        /// <summary>
        /// Gets the full qualified name of this type. This is the name of this assembly
        /// and equals to <see cref="CsBase.Name"/> property.
        /// </summary>
        /// <value>The full name.</value>
        public override string QualifiedName
        {
            get
            {
                return Name;
            }
        }

        /// <summary>
        /// Gets the assembly that contains this namespace.
        /// </summary>
        /// <value>The assembly.</value>
        public CsAssembly Assembly
        {
            get
            {
                return GetParent<CsAssembly>();
            }
        }

        /// <summary>
        /// Gets or sets the output directory for generated files for this namespace.
        /// </summary>
        /// <value>The output directory.</value>
        public string OutputDirectory { get; set;}


        public IEnumerable<CsTypeBase> Types
        {
            get { return Items.OfType<CsTypeBase>(); }
        }

        /// <summary>
        /// Gets all declared enums from this namespace.
        /// </summary>
        /// <value>The enums.</value>
        public IEnumerable<CsEnum> Enums
        {
            get { return Items.OfType<CsEnum>(); }
        }

        /// <summary>
        /// Gets all declared structs from this namespace.
        /// </summary>
        /// <value>The structs.</value>
        public IEnumerable<CsStruct> Structs
        {
            get { return Items.OfType<CsStruct>(); }
        }

        /// <summary>
        /// Gets all declared interfaces from this namespace.
        /// </summary>
        /// <value>The interfaces.</value>
        public IEnumerable<CsInterface> Interfaces
        {
            get { return Items.OfType<CsInterface>(); }
        }

        /// <summary>
        /// Gets all declared classes from this namespace.
        /// </summary>
        /// <value>The function groups.</value>
        public IEnumerable<CsClass> Classes
        {
            get { return Items.OfType<CsClass>(); }
        }
    }
}