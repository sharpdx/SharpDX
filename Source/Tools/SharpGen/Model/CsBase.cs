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
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using SharpGen.Config;
using SharpGen.CppModel;
using SharpGen.Generator;

namespace SharpGen.Model
{
    /// <summary>
    /// Root class for all model elements.
    /// </summary>
    public class CsBase
    {
        private List<CsBase> _items;
        private CppElement _cppElement;
        private string _cppElementName;
        private static readonly Regex RegexLinkStart = new Regex(@"^\s*\{\{.*?}}\s*(.*)", RegexOptions.Compiled);
        private static readonly Regex RegexLink = new Regex(@"\{\{(.*?)}}", RegexOptions.Compiled);
        private static readonly Regex RegexSpaceBegin = new Regex(@"^\s*(.*)", RegexOptions.Compiled);

        /// <summary>
        /// Initializes a new instance of the <see cref="CsBase"/> class.
        /// </summary>
        public CsBase()
        {
            _items = new List<CsBase>();
            Generator = CallContext.GetData("Generator") as TransformManager;
            Visibility = Visibility.Public;
            IsFullyMapped = true;
            Description = "No documentation.";
            Remarks = "";
        }

        internal TransformManager Generator { get; private set; }

        /// <summary>
        /// Gets or sets the parent of this container.
        /// </summary>
        /// <value>The parent.</value>
        public CsBase Parent { get; set; }

        protected void ClearItems()
        {
            _items = new List<CsBase>();
        }

        /// <summary>
        /// Gets the parent of a specified type. This method goes back
        /// to all parent and returns the first parent of the type T or null if no parent were found.
        /// </summary>
        /// <typeparam name="T">Type of the parent</typeparam>
        /// <returns>a valid reference to the parent T or null if no parent of this type</returns>
        public T GetParent<T>() where T : CsBase
        {
            CsBase parent = Parent;
            while (parent != null && !(parent is T))
                parent = parent.Parent;
            return (T) parent;
        }

        /// <summary>
        /// Gets items stored in this container.
        /// </summary>
        /// <value>The items.</value>
        public ReadOnlyCollection<CsBase> Items
        {
            get { return _items.AsReadOnly(); }
        }

        /// <summary>
        /// Gets the variables stored in this container.
        /// TODO: move this method in another inherited class.
        /// </summary>
        /// <value>The variables.</value>
        public IEnumerable<CsVariable> Variables
        {
            get { return Items.OfType<CsVariable>(); }
        }

        /// <summary>
        /// Adds the specified inner container to this container.
        /// </summary>
        /// <remarks>
        /// The Parent property of the innerContainer is set to this container.
        /// </remarks>
        /// <param name="innerCs">The inner container.</param>
        public void Add(CsBase innerCs)
        {
            innerCs.Parent = this;
            _items.Add(innerCs);
        }

        /// <summary>
        /// Removes the specified inner container from this container
        /// </summary>
        /// <remarks>
        /// The Parent property of the innerContainer is set to null.
        /// </remarks>
        /// <param name="innerCs">The inner container.</param>
        public void Remove(CsBase innerCs)
        {
            innerCs.Parent = null;
            _items.Remove(innerCs);
        }

        /// <summary>
        /// Sorts all elements inside this instance
        /// </summary>
        public void Sort()
        {
            _items.Sort(delegate(CsBase x, CsBase y) { return x.Name.CompareTo(y.Name); });
        }

        /// <summary>
        /// Gets or sets the name of this element.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Visibility"/> of this element. Default is public.
        /// </summary>
        /// <value>The visibility.</value>
        public Visibility Visibility { get; set; }

        /// <summary>
        /// Returns a textual representation of the <see cref="CsBase.Visibility"/> property.
        /// </summary>
        /// <value>The full name of the visibility.</value>
        public string VisibilityName
        {
            get
            {
                var builder = new StringBuilder();

                if ((Visibility & Visibility.Public) != 0)
                    builder.Append("public ");
                else if ((Visibility & Visibility.Protected) != 0)
                    builder.Append("protected ");
                else if ((Visibility & Visibility.Internal) != 0)
                    builder.Append("internal ");
                else if ((Visibility & Visibility.PublicProtected) != 0)
                    builder.Append("");

                if ((Visibility & Visibility.Const) != 0)
                    builder.Append("const ");

                if ((Visibility & Visibility.Static) != 0)
                    builder.Append("static ");

                if ((Visibility & Visibility.Sealed) != 0)
                    builder.Append("sealed ");

                if ((Visibility & Visibility.Readonly) != 0)
                    builder.Append("readonly ");

                if ((Visibility & Visibility.Override) != 0)
                    builder.Append("override ");

                if ((Visibility & Visibility.Abstract) != 0)
                    builder.Append("abstract ");

                if ((Visibility & Visibility.Virtual) != 0)
                    builder.Append("virtual ");

                return builder.ToString();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is already fully mapped.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is fully mapped; otherwise, <c>false</c>.
        /// </value>
        public bool IsFullyMapped { get; set; }

        /// <summary>
        /// Gets the full qualified name of this type.
        /// </summary>
        /// <value>The full name.</value>
        public virtual string QualifiedName
        {
            get
            {
                string path = Parent != null ? Parent.QualifiedName : null;
                string name = Name ?? "";
                return string.IsNullOrEmpty(path) ? name : path + "." + name;
            }
        }

        /// <summary>
        /// Gets or sets the C++ element associated to this container.
        /// </summary>
        /// <value>The CPP element.</value>
        public CppElement CppElement
        {
            get { return _cppElement; }
            set
            {
                _cppElement = value;
                if (_cppElement != null )
                {
                    Description = string.IsNullOrEmpty(CppElement.Description) ? Description : CppElement.Description;
                    Remarks = string.IsNullOrEmpty(CppElement.Remarks) ? Remarks : CppElement.Remarks;

                    // Update this container with tag
                    if (_cppElement.Tag != null)
                        UpdateFromTag(_cppElement.GetTagOrDefault<MappingRule>());
                }
            }
        }

        /// <summary>
        /// Gets the name of the C++ element.
        /// </summary>
        /// <value>The name of the C++ element. "None" if no C++ element attached to this container.</value>
        public string CppElementName
        {
            get
            {
                if (!string.IsNullOrEmpty(_cppElementName))
                    return _cppElementName;
                if (CppElement != null && CppElement.Name != null)
                    return CppElement.Name;
                return "None";
            } 
            set { _cppElementName = value; }
        }

        /// <summary>
        /// Gets or sets the sizeof this element.
        /// </summary>
        /// <value>The size of.</value>
        public int SizeOf { get; set; }

        /// <summary>
        ///   Packing alignement for this structure (Default is 0 => Platform default)
        /// </summary>
        public int Align { get; set; }

        /// <summary>
        /// Gets or sets the description documentation.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the remarks documentation.
        /// </summary>
        /// <value>The remarks.</value>
        public string Remarks { get; set; }


        /// <summary>
        /// Gets the XML C# doc items as a list of string (contains summary and remarks tags).
        /// </summary>
        /// <value>The XML C# doc items.</value>
        public List<string> DocItems
        {
            get
            {
                var docItems = new List<string>();

                // If doc comments are already stored in an external file, than don't emit them
                if (!IsCodeCommentsExternal)
                {
                    string description = Description;
                    string remarks = Remarks;

                    description = RegexSpaceBegin.Replace(description, "$1");

                    description = RegexLink.Replace(description, RegexReplaceCReference);
                    // evaluator => "<see cref=\"$1\"/>");

                    string line = null;

                    StringReader stringReader = new StringReader(description);
                    docItems.Add("<summary>");
                    while ((line = stringReader.ReadLine()) != null)
                        docItems.Add(line);
                    docItems.Add("</summary>");

                    FillDocItems(docItems);

                    if (!string.IsNullOrEmpty(remarks))
                    {
                        remarks = RegexSpaceBegin.Replace(remarks, "$1");
                        remarks = RegexLink.Replace(remarks, RegexReplaceCReference);

                        stringReader = new StringReader(remarks);
                        docItems.Add("<remarks>");
                        while ((line = stringReader.ReadLine()) != null)
                            docItems.Add(line);
                        docItems.Add("</remarks>");
                    }
                }

                docItems.Add(DocIncludeDirective);
                if (CppElement != null)
                    docItems.Add("<unmanaged>" + Utilities.EscapeXml(DocUnmanagedName) + "</unmanaged>");

                return docItems;
            }
        }

        protected virtual void FillDocItems(List<string> docItems) {}
        
        public virtual string DocUnmanagedName
        {
            get { return CppElementName; }
        }

            /// <summary>
        /// Gets the description as a single line of documentation.
        /// </summary>
        /// <value>The single doc.</value>
        public string SingleDoc
        {
            get
            {
                string description = Description;

                if (RegexLinkStart.Match(description).Success)
                    description = RegexLinkStart.Replace(description, "$1");

                description = RegexSpaceBegin.Replace(description, "$1");

                description = RegexLink.Replace(description, RegexReplaceCReference);

                StringBuilder docItems = new StringBuilder();
                string line = null;

                StringReader stringReader = new StringReader(description);
                while ((line = stringReader.ReadLine()) != null)
                    docItems.Append(line);

                return docItems.ToString();
            }
        }

        private string DocIncludeDirective
        {
            get
            {
                var subDir = GetParent<CsNamespace>().OutputDirectory;
                string relativePath = string.IsNullOrEmpty(subDir) ? "." : "..";
                return "<include file='" + relativePath + "\\..\\" + CsAssembly.CodeCommentsPath + "' path=\"" + CodeCommentsXPath + "/*\"/>";
            }
        }

        private bool IsCodeCommentsExternal
        {
            get { return GetParent<CsAssembly>().CodeComments.SelectSingleNode(CodeCommentsXPath) != null; }
        }

        private string CodeCommentsXPath
        {
            get { return "/comments/comment[@id='" + ((CppElement != null) ? CppElement.FullName : QualifiedName) + "']"; }
        }

        /// <summary>
        /// Updates this element from a tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        protected virtual void UpdateFromTag(MappingRule tag)
        {
            if (tag.Visibility.HasValue)
                Visibility = tag.Visibility.Value;
        }

        private static Regex regextWithMethodW = new Regex("([^W])::");
        private static Regex regexWithTypeW = new Regex("([^W])$");

        private string RegexReplaceCReference(Match match)
        {
            string matchName = match.Groups[1].Value;
            var csType = Generator.FindBindType(matchName);

            // Tries to match with W::
            if (csType == null && regextWithMethodW.Match(matchName).Success)
                csType = Generator.FindBindType(regextWithMethodW.Replace(matchName, "$1W::"));

            // Or with W
            if (csType == null && regexWithTypeW.Match(matchName).Success)
                csType = Generator.FindBindType(regexWithTypeW.Replace(matchName, "$1W"));

            if (csType == null) 
                return match.Value;

            return string.Format("<see cref=\"{0}\"/>", csType.QualifiedName);
        }
    }
}