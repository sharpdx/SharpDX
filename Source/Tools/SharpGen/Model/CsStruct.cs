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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using SharpGen.Config;
using SharpGen.CppModel;

namespace SharpGen.Model
{
    /// <summary>
    ///   A structElement that maps to a native struct
    /// </summary>
    [XmlType("struct")]
    public class CsStruct : CsTypeBase
    {
        public CsStruct()
            : this(null)
        {
        }

        public CsStruct(CppStruct cppStruct) 
        {
            IsIn = true;
            IsOut = false; 
            CppElement = cppStruct;
            // Align was not overloaded by MappingRule tag, then we can take the default value
            if (cppStruct != null &&  Align == 0) 
                Align = cppStruct.Align;
        }

        protected override void UpdateFromTag(MappingRule tag)
        {
            base.UpdateFromTag(tag);
            Align = tag.StructPack != null ? tag.StructPack.Value : Align;
            HasMarshalType = tag.StructHasNativeValueType != null ? tag.StructHasNativeValueType.Value : false;
            GenerateAsClass = tag.StructToClass != null ? tag.StructToClass.Value : false;
            HasCustomMarshal = tag.StructCustomMarshal != null ? tag.StructCustomMarshal.Value : false;
            IsStaticMarshal = tag.IsStaticMarshal != null ? tag.IsStaticMarshal.Value : false;
            HasCustomNew = tag.StructCustomNew != null ? tag.StructCustomNew.Value : false;
            IsOut = tag.StructForceMarshalToToBeGenerated != null ? tag.StructForceMarshalToToBeGenerated.Value : false;

            // Force a marshalling if a struct need to be treated as a class)
            if (GenerateAsClass)
                HasMarshalType = true;
        }

        public IEnumerable<CsField> Fields
        {
            get { return Items.OfType<CsField>(); }
        }

        /// <summary>
        ///   True if this structure is using an explicit layout else it's a sequential structure
        /// </summary>
        public bool ExplicitLayout { get; set; }

        /// <summary>
        ///   True if this struct needs an internal marshal type
        /// </summary>
        public bool HasMarshalType { get; set; }

        public bool HasCustomMarshal { get; set; }

        public bool IsStaticMarshal { get; set; }

        public bool GenerateAsClass { get; set; }

        public bool HasCustomNew { get; set; }

        public string GetConstructor()
        {
            return string.Format(HasCustomNew ? "{0}.__NewNative()" : "new {0}.__Native()", QualifiedName);
        }

        public string StructTypeName
        {
            get
            {
                return GenerateAsClass ? "class" : "struct";
            }
        }

        public bool IsIn { get; set; }

        public bool IsOut { get; set; }

        /// <summary>
        ///   List of declared inner structs
        /// </summary>
        public IEnumerable<CsStruct> InnerStructs
        {
            get { return Items.OfType<CsStruct>(); }
        }
    }
}