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
        }

        protected override void UpdateFromTag(MappingRule tag)
        {
            base.UpdateFromTag(tag);
            Align = tag.StructPack ?? 0;
            HasMarshalType = tag.StructHasNativeValueType ?? false;
            GenerateAsClass = tag.StructToClass ?? false;
            HasCustomMarshal = tag.StructCustomMarshal ?? false;
            IsStaticMarshal = tag.IsStaticMarshal ?? false;
            HasCustomNew = tag.StructCustomNew ?? false;
            IsOut = tag.StructForceMarshalToToBeGenerated ?? false;

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

        public override int CalculateAlignment()
        {
            int structAlignment = 0;
            foreach(var field in Fields)
            {
                var fieldAlignment = (field.MarshalType ?? field.PublicType).CalculateAlignment();
                if(fieldAlignment < 0)
                {
                    structAlignment = fieldAlignment;
                    break;
                }
                if(fieldAlignment > structAlignment)
                {
                    structAlignment = fieldAlignment;
                }
            }

            return structAlignment;
        }
    }
}