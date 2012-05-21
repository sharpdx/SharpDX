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
using SharpGen.Config;
using SharpGen.CppModel;

namespace SharpGen.Model
{
    public class CsMethod : CsTypeBase, ICloneable
    {
        protected virtual int MaxSizeReturnParameter
        {
            get { return 4; }
        }

        public CsMethod(CppMethod cppMethod)
        {
            CppElement = cppMethod;
        }

        private List<CsParameter> _parameters;
        public List<CsParameter> Parameters
        {
            get { return _parameters ?? (_parameters = Items.OfType<CsParameter>().ToList()); }
        }

        public IEnumerable<CsParameter> PublicParameters
        {
            get
            {
                return Items.Cast<CsParameter>().Where(param => !param.IsUsedAsReturnType);
            }
        }

        public bool Hidden { get; set; }

        public int ParameterCount
        {
            get { return Parameters.Count; }
        }

        public int PublicParameterCount
        {
            get
            {
                return PublicParameters.Count();
            }
        }

        protected override void FillDocItems(List<string> docItems)
        {
            foreach (var param in PublicParameters)
                docItems.Add("<param name=\"" + param.Name + "\">" + param.SingleDoc + "</param>");

		    if (HasReturnType)
		        docItems.Add("<returns>" + ReturnTypeDoc + "</returns>");
        }

        public bool IsHResult
        {
            get { return HasReturnType && ReturnType.PublicType.Name == Global.Name + ".Result"; }
        }

        public bool IsReturnStructLarge
        {
            get
            {
                var csStruct = ReturnType.PublicType as CsStruct ;
                if (csStruct != null)
                {
                    if (ReturnType.MarshalType.Type == typeof(IntPtr))
                        return false;

                    return csStruct.SizeOf > MaxSizeReturnParameter;
                }
                return false;
            }
        }
        
        protected override void UpdateFromTag(MappingRule tag)
        {
            base.UpdateFromTag(tag);

            AllowProperty = !tag.Property.HasValue || tag.Property.Value;

            IsPersistent = tag.Persist.HasValue && tag.Persist.Value;

            if (tag.MethodCheckReturnType.HasValue)
                CheckReturnType = tag.MethodCheckReturnType.Value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [use DLL import].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use DLL import]; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>Only used by function</remarks>
        public bool UseDllImport { get; set; }

        public bool AllowProperty { get; set; }

        public bool IsPersistent { get; set; }

        public int Offset { get; set; }

        public InteropMethodSignature Interop { get; set; }

        public string CppSignature
        {
            get
            {
                if (CppElement != null)
                {
                    return CppElement.ToString();
                }
                return "Unknown";
            }
        }

        public override string DocUnmanagedName
        {
            get { return CppSignature; }
        }

        public override string DocUnmanagedShortName
        {
            get
            {
                if (CppElement != null) return CppElement.ToShortString();
                return null;
            }
        }

        public bool CheckReturnType { get; set; }

        public bool HasReturnType
        {
            get { return !(ReturnType.PublicType.Type != null && ReturnType.PublicType.Type == typeof (void)); }
        }

        public bool HasPublicReturnType
        {
            get
            {
                foreach (var param in Parameters)
                {
                    if (param.IsUsedAsReturnType)
                        return true;
                }

                return HasReturnType;                
            }
        }


        public string ReturnTypeForFunction
        {
            get
            {
                if (ReturnType.PublicType is CsInterface)
                    return "IntPtr";
                if (ReturnType.PublicType.Type != null && ReturnType.PublicType.Type == typeof(bool))
                    return "int";
                return ReturnType.PublicType.QualifiedName;
            }
        }
        
        public string CastStart
        {
            get
            {
                if (ReturnType.PublicType.Type != null && ReturnType.PublicType.Type == typeof (bool))
                    return "(0!=";
                if (ReturnType.PublicType is CsInterface)
                    return "new " + ReturnType.PublicType.QualifiedName + "((IntPtr)";
                if (ReturnType.PublicType.Type == typeof(string))
                {
                    if (ReturnType.IsWideChar)
                        return "Marshal.PtrToStringUni(";
                    return "Marshal.PtrToStringAnsi(";
                }
                return "";
            }
        }

        public string CastEnd
        {
            get
            {
                if (ReturnType.PublicType.Type != null && ReturnType.PublicType.Type == typeof (bool))
                    return ")";
                if (ReturnType.PublicType is CsInterface)
                    return ")";
                if (ReturnType.PublicType.Type == typeof(string))
                    return ")";
                return "";
            }
        }

        public CsMarshalBase ReturnType { get; set; }

        /// <summary>
        /// Return the Public return type. If a out parameter is used as a public return type
        /// then use the type of the out parameter for the public api
        /// </summary>
        public string PublicReturnTypeQualifiedName
        {
            get
            {
                foreach (var param in Parameters)
                {
                    if (param.IsUsedAsReturnType)
                        return param.PublicType.QualifiedName;
                }
                return ReturnType.PublicType.QualifiedName;
            }
        }

        /// <summary>
        /// Returns the documentation for the return type
        /// </summary>
        public string ReturnTypeDoc
        {
            get
            {
                foreach (var param in Parameters)
                {
                    if (param.IsUsedAsReturnType)
                    {
                        return param.SingleDoc;
                    }
                }
                return ReturnType.SingleDoc;
            }
        }

        /// <summary>
        /// Return the name of the variable used to return the value
        /// </summary>
        public string ReturnName
        {
            get
            {
                foreach (var param in Parameters)
                {
                    if (param.IsUsedAsReturnType)
                        return param.Name;
                }
                return "__result__";
            }
        }

        public override object Clone()
        {
            var method = (CsMethod)base.Clone();

            // Clear cached parameters
            method._parameters = null;
            method.ClearItems();
            foreach (var parameter in Parameters)
                method.Add((CsParameter) parameter.Clone());
            method.Parent = null;
            return method;
        }
    }
}