// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace SharpGen.CppModel
{
    /// <summary>
    ///   Base class for all C++ element.
    /// </summary>
    public class CppElement
    {
        /// <summary>
        ///   Delegate Modifier
        /// </summary>
        public delegate bool ProcessModifier(Regex regex, CppElement cppElement);

        /// <summary>
        /// Current context used by the finder.
        /// </summary>
        private List<string> _findContext;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [XmlElement("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the remarks.
        /// </summary>
        /// <value>The remarks.</value>
        [XmlElement("remarks")]
        public string Remarks { get; set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        [XmlIgnore]
        public CppElement Parent { get; set; }

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>The tag.</value>
        [XmlIgnore]
        public object Tag { get; set; }

        /// <summary>
        /// Gets the parent include.
        /// </summary>
        /// <value>The parent include.</value>
        [XmlIgnore]
        public CppInclude ParentInclude
        {
            get
            {
                CppElement cppInclude = Parent;
                while (cppInclude != null && !(cppInclude is CppInclude))
                    cppInclude = cppInclude.Parent;
                return cppInclude as CppInclude;
            }
        }

        /// <summary>
        /// Gets the parent root C++ element.
        /// </summary>
        /// <value>The parent root.</value>
        [XmlIgnore]
        public CppElement ParentRoot
        {
            get
            {
                CppElement cppRoot = this;
                while (cppRoot.Parent != null)
                    cppRoot = cppRoot.Parent;
                return cppRoot;
            }
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <value>The path.</value>
        [XmlIgnore]
        public virtual string Path
        {
            get
            {
                if (Parent != null)
                    return Parent.FullName;
                return "";
            }
        }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <value>The full name.</value>
        [XmlIgnore]
        public virtual string FullName
        {
            get
            {
                string path = Path;
                string name = Name ?? "";
                return String.IsNullOrEmpty(path) ? name : path + "::" + name;
            }
        }

        /// <summary>
        /// Return all items inside this C++ element.
        /// </summary>
        [XmlArray("items")]
        [XmlArrayItem(typeof (CppConstant))]
        [XmlArrayItem(typeof (CppDefine))]
        [XmlArrayItem(typeof (CppEnum))]
        [XmlArrayItem(typeof (CppEnumItem))]
        [XmlArrayItem(typeof (CppField))]
        [XmlArrayItem(typeof (CppFunction))]
        [XmlArrayItem(typeof (CppGuid))]
        [XmlArrayItem(typeof (CppInclude))]
        [XmlArrayItem(typeof (CppInterface))]
        [XmlArrayItem(typeof (CppMethod))]
        [XmlArrayItem(typeof (CppParameter))]
        [XmlArrayItem(typeof (CppStruct))]
        [XmlArrayItem(typeof (CppType))]
        public List<CppElement> Items { get; set; }

        /// <summary>
        ///   Gets the context find list.
        /// </summary>
        /// <value>The context find list.</value>
        private List<string> ContextFindList
        {
            get
            {
                if (_findContext == null)
                    _findContext = new List<string>();
                return _findContext;
            }
        }

        protected virtual IEnumerable<CppElement> AllItems
        {
            get { return Iterate<CppElement>(); }
        }

        [XmlIgnore]
        public bool IsEmpty
        {
            get { return Items == null || Items.Count == 0; }
        }

        public T GetTagOrDefault<T>() where T : new()
        {
            return (T) (Tag ?? new T());
        }

        public static string ShortName<T>() where T : CppElement
        {
            var type = typeof (T);
            string tagname = type.Name;
            var attributes = type.GetCustomAttributes(typeof (DataContractAttribute), false);
            if (attributes.Length == 1)
                tagname = (attributes[0] as DataContractAttribute).Name;
            return tagname;
        }

        /// <summary>
        ///   Add an inner element to this CppElement
        /// </summary>
        /// <param name = "element"></param>
        public void Add(CppElement element)
        {
            if (element.Parent != null)
                element.Parent.Remove(element);
            element.Parent = this;
            GetSafeItems().Add(element);
        }

        public void Add<T>(IEnumerable<T> elements) where T : CppElement
        {
            foreach (var cppElement in elements)
                Add(cppElement);
        }

        /// <summary>
        ///   Remove an inner element to this CppElement
        /// </summary>
        /// <param name = "element"></param>
        public void Remove(CppElement element)
        {
            element.Parent = null;
            GetSafeItems().Remove(element);
        }

        private List<CppElement> GetSafeItems()
        {
            if (Items == null)
                Items = new List<CppElement>();
            return Items;
        }

        /// <summary>
        ///   Adds a context to the finder.
        /// </summary>
        /// <param name = "contextName">Name of the context.</param>
        public void AddContextFind(string contextName)
        {
            ContextFindList.Add(contextName);
        }

        /// <summary>
        ///   Adds a set of context to the finder.
        /// </summary>
        /// <param name = "contextNames">The context names.</param>
        public void AddContextRangeFind(IEnumerable<string> contextNames)
        {
            foreach (var contextName in contextNames)
                AddContextFind(contextName);
        }

        /// <summary>
        ///   Clears the context finder.
        /// </summary>
        public void ClearContextFind()
        {
            ContextFindList.Clear();
        }

        /// <summary>
        ///   Iterates on items on this instance.
        /// </summary>
        /// <typeparam name = "T">Type of the item to iterate</typeparam>
        /// <returns>An enumeration on items</returns>
        public IEnumerable<T> Iterate<T>() where T : CppElement
        {
            return Items == null ? Enumerable.Empty<T>() : Items.OfType<T>();
        }

        /// <summary>
        ///   Finds the first element by regex.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "regex">The regex.</param>
        /// <returns></returns>
        public T FindFirst<T>(string regex) where T : CppElement
        {
            return Find<T>(regex).FirstOrDefault();
        }

        /// <summary>
        ///   Finds the specified elements by regex.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "regex">The regex.</param>
        /// <returns></returns>
        public IEnumerable<T> Find<T>(string regex) where T : CppElement
        {
            var cppElements = new List<T>();
            Find(new Regex("^" + regex + "$"), cppElements, null);
            return cppElements.OfType<T>();
        }

        /// <summary>
        ///   Remove recursively elements matching the regex of type T
        /// </summary>
        /// <typeparam name = "T">the T type to match</typeparam>
        /// <param name = "regex">the regex to match</param>
        public void Remove<T>(string regex) where T : CppElement
        {
            Modify<T>(regex, (pathREgex, element) => true);
        }

        /// <summary>
        ///   Modifies the specified elements by regex with the specified modifier.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "regex">The regex.</param>
        /// <param name = "modifier">The modifier.</param>
        public void Modify<T>(string regex, ProcessModifier modifier) where T : CppElement
        {
            var cppElements = new List<T>();
            regex = "^" + regex + "$";
            Find(new Regex(regex), cppElements, modifier);
        }

        /// <summary>
        ///   Strips the regex. Removes ^ and $ at the end of the string
        /// </summary>
        /// <param name = "regex">The regex.</param>
        /// <returns></returns>
        public static string StripRegex(string regex)
        {
            string friendlyRegex = regex;
            // Remove ^ and $
            if (friendlyRegex.StartsWith("^"))
                friendlyRegex = friendlyRegex.Substring(1);
            if (friendlyRegex.EndsWith("$"))
                friendlyRegex = friendlyRegex.Substring(0, friendlyRegex.Length - 1);
            return friendlyRegex;
        }

        /// <summary>
        ///   Finds all elements by regex.
        /// </summary>
        /// <param name = "regex">The regex.</param>
        /// <returns></returns>
        public IEnumerable<CppElement> FindAll(string regex)
        {
            return Find<CppElement>(regex);
        }

        /// <summary>
        ///   Finds the specified elements by regex and modifier.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "regex">The regex.</param>
        /// <param name = "toAdd">To add.</param>
        /// <param name = "modifier">The modifier.</param>
        /// <returns></returns>
        private bool Find<T>(Regex regex, List<T> toAdd, ProcessModifier modifier) where T : CppElement
        {
            bool isToRemove = false;
            string path = FullName;

            if ((this is T) && path != null && regex.Match(path).Success)
            {
                if (toAdd != null)
                    toAdd.Add((T) this);
                if (modifier != null)
                    isToRemove = modifier(regex, this);
            }

            var elementsToRemove = new List<CppElement>();

            // Force _findContext to reverse to null if not used anymore
            if (_findContext != null && _findContext.Count == 0)
                _findContext = null;

            if (_findContext == null)
            {
                // Optimized version with findContext
                foreach (var innerElement in AllItems)
                {
                    if (innerElement.Find(regex, toAdd, modifier))
                        elementsToRemove.Add(innerElement);
                }
            }
            else
            {
                foreach (var innerElement in AllItems)
                {
                    if (_findContext.Contains(innerElement.Name))
                        if (innerElement.Find(regex, toAdd, modifier))
                            elementsToRemove.Add(innerElement);
                }
            }

            foreach (var innerElementToRemove in elementsToRemove)
                Remove(innerElementToRemove);

            return isToRemove;
        }

        protected void ResetParents()
        {
            foreach (var innerElement in Iterate<CppElement>())
            {
                innerElement.Parent = this;
                innerElement.ResetParents();
            }
        }

        public override string ToString()
        {
            return GetType().Name + " [" + Name + "]";
        }
    }
}