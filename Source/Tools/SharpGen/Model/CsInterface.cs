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
using System.Xml.Serialization;
using SharpGen.Config;
using SharpGen.CppModel;

namespace SharpGen.Model
{
    [XmlType("interface")]
    public class CsInterface : CsTypeBase
    {
        public CsInterface() : this(null)
        {
        }

        public CsInterface(CppInterface cppInterface)
        {
            CppElement = cppInterface;
            if (cppInterface != null)
                Guid = cppInterface.Guid;
            NativeImplem = this;
        }

        public IEnumerable<CsMethod> Methods
        {
            get { return Items.OfType<CsMethod>(); }
        }

        public IEnumerable<CsProperty> Properties
        {
            get { return Items.OfType<CsProperty>(); }
        }

        protected override void UpdateFromTag(MappingRule tag)
        {
            base.UpdateFromTag(tag);
            IsCallback = tag.IsCallbackInterface.HasValue?tag.IsCallbackInterface.Value:false;
            IsDualCallback = tag.IsDualCallbackInterface.HasValue ? tag.IsDualCallbackInterface.Value : false;
        }

        /// <summary>
        /// Class Parent inheritance
        /// </summary>
        public CsTypeBase Base { get; set; }

        /// <summary>
        /// Interface Parent inheritance
        /// </summary>
        public CsTypeBase IBase { get; set; }

        public CsInterface NativeImplem { get; set; }

        public string Guid { get; set; }

        /// <summary>
        ///   Only valid for inner interface. Specify the name of the property in the outer interface to access to the inner interface
        /// </summary>
        public string PropertyAccesName { get; set; }

        /// <summary>
        ///   True if this interface is used as a callback to a C# object
        /// </summary>
        public bool IsCallback { get; set; }

        /// <summary>
        ///   True if this interface is used as a dual-callback to a C# object
        /// </summary>
        public bool IsDualCallback { get; set; }

        /// <summary>
        ///   List of declared inner structs
        /// </summary>
        public bool HasInnerInterfaces
        {
            get { return Items.OfType<CsInterface>().Count() > 0; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is base COM object.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is base COM object; otherwise, <c>false</c>.
        /// </value>
        public bool IsBaseComObject
        {
            get { return Base != null && (Base as CsInterface).QualifiedName == Global.Name + ".ComObject"; }
        }

        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "csinterface {0} => {1}", CppElementName, QualifiedName);
        }

        /// <summary>
        ///   List of declared inner structs
        /// </summary>
        public IEnumerable<CsInterface> InnerInterfaces
        {
            get { return Items.OfType<CsInterface>(); }
        }
    }
}