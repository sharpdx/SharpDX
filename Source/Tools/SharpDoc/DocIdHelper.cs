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
using System.Text;
using Mono.Cecil;

namespace SharpDoc
{
    public class DocIdHelper
    {

        public static string StripXmlId(string xmlId)
        {
            var id = xmlId;
            var index = id.IndexOf("(");
            if (index > 0)
                id = id.Substring(0, index);

            return id.Replace(':', '_').Replace('.', '_').Replace('#', '_').Replace('`', '_').Replace('{','-').Replace('}','-');
        }

        public static string GetXmlId(MemberReference member)
        {
            if (member == null)
                throw new ArgumentNullException("member");

            var stbBuilder = new StringBuilder();
            var Path = new List<string>();

            // get path
            GetXmlDocPathRecursive(member, Path);

            // generate string
            if (Path.Count == 0)
                return String.Empty;

            foreach (string strTemp in Path)
                stbBuilder.Append(strTemp);

            return stbBuilder.ToString();
        }

        private static string GetXmlDocExplicitIfaceImplPath(MemberReference member)
        {
            TypeReference declaringTypeRef = null;
            TypeDefinition declaringTypeDef = null;
            string strPath = String.Empty;

            if (member.DeclaringType is GenericInstanceType)
                declaringTypeRef = (member.DeclaringType as GenericInstanceType).ElementType;
            else
                declaringTypeRef = member.DeclaringType;

            // lookup TypeDefinition for TypeReference
            declaringTypeDef = TryLookUpTypeDefinition(declaringTypeRef);

            if (declaringTypeDef == null || declaringTypeDef.IsInterface)
                return String.Empty;

            foreach (TypeReference tempIfaceRef in declaringTypeDef.Interfaces)
            {
                // check whether this member name begins with interface name (plus generic arguments)
                if (member.Name.StartsWith(StripInterfaceName(tempIfaceRef.FullName)))
                {
                    // element begins with interface name, this is explicit interface implementation,
                    // get explicit interface implementation path

                    // add member's name to path, at this point
                    // name contains interface name (with generic arguments) plus member name
                    strPath = member.Name;

                    // remove text between "<" and ">" and put interface parameters 
                    // (in explicit mode of course)
                    int LeftBrace = strPath.IndexOf("<");
                    int RightBrace = strPath.LastIndexOf(">");

                    if (LeftBrace != -1 && RightBrace != -1)
                    {
                        bool firstAppend = true;
                        GenericInstanceType tempGenericIfaceDef = null;
                        StringBuilder stbParameters = new StringBuilder();

                        // convert to definition
                        tempGenericIfaceDef = tempIfaceRef as GenericInstanceType;

                        if (tempGenericIfaceDef == null)
                            break;

                        strPath = strPath.Remove(LeftBrace, (RightBrace - LeftBrace) + 1);
                        stbParameters.Append("{");
                        foreach (TypeReference tempParam in tempGenericIfaceDef.GenericArguments)
                        {
                            // in "explicit" mode "@" is used as a separator instead of ","
                            // in "normal" mode
                            if (!firstAppend)
                                stbParameters.Append(CanAppendSpecialExplicitChar() ? "@" : ",");

                            GetXmlDocParameterPathRecursive(tempParam, true, stbParameters);
                            firstAppend = false;
                        }
                        stbParameters.Append("}");

                        // insert                             
                        strPath = strPath.Insert(LeftBrace, stbParameters.ToString());
                    }

                    // replace "." with "#"
                    if (CanAppendSpecialExplicitChar())
                        strPath = strPath.Replace(".", "#");

                    return strPath;
                }
            }

            return String.Empty;
        }

        private static TypeDefinition TryLookUpTypeDefinition(TypeReference reference)
        {
            // try find in the current assembly
            foreach (TypeDefinition tempTypeDef in reference.Module.Types)
                if (tempTypeDef.ToString() == reference.ToString())
                    return tempTypeDef;

            return null;
        }

        private static string StripInterfaceName(string originalName)
        {
            StringBuilder builderStrippedName = new StringBuilder();

            // split name 
            string[] strSlices = originalName.Split(new[] {'`'});

            // remove numbers at the begining of each string to "<" charter
            if (strSlices.Length > 1)
                for (int i = 0; i < strSlices.Length; i++)
                    if (strSlices[i].Contains("<"))
                        strSlices[i] = strSlices[i].Remove(0, strSlices[i].IndexOf("<"));

            // build string
            foreach (string tempString in strSlices)
                builderStrippedName.Append(tempString);

            return builderStrippedName.ToString();
        }

        private static void GetXmlDocPathRecursive(MemberReference member, List<string> currentPathStack)
        {
            /*
             * determine type of the current member, if current path is empty
             * we have also to insert to path element type:
             * - "N:" - for namespace (not used here)
             * - "T:" - for a type (class, structure, delegate)
             * - "M:" - for a method (or constructor)
             * - "F:" - for a field
             * - "P:" - for a property or indexer
             * - "E:" - for an event
             */

            var stbTempPath = new StringBuilder();
            string strExplicitPath = String.Empty;

            if (member is TypeReference)
            {
                TypeReference thisTypeRef = null;
                GenericInstanceType thisGenericTypeDef = null;
                GenericParameter thisGenericParam = null;
                string strTempTypeName = String.Empty;

                if (member is GenericInstanceType)
                {
                    thisGenericTypeDef = member as GenericInstanceType;
                    thisTypeRef = thisGenericTypeDef.ElementType;
                }
                else if (member is GenericParameter)
                {
                    thisGenericParam = member as GenericParameter;
                    currentPathStack.Add("`" + thisGenericParam.Position);

                    // return immediatelly, because there is nothing to do.
                    return;
                }
                else
                {
                    // cast to TypeReference
                    thisTypeRef = member as TypeReference;
                }

                // if nested, scan enclosing type 
                if (IsNested(thisTypeRef))
                    GetXmlDocPathRecursive(member.DeclaringType, currentPathStack);

                // determine namespace
                string strNamespace = String.Empty;
                if ((thisTypeRef.Namespace != null && thisTypeRef.Namespace.Length > 0) || thisTypeRef.DeclaringType != null)
                    strNamespace = thisTypeRef.Namespace + ".";

                // remove "`" char or not
                string strTempShortTypeName = thisTypeRef.Name;
                if (thisTypeRef.Name.Contains("`") && thisGenericTypeDef != null)
                    strTempShortTypeName = thisTypeRef.Name.Remove(thisTypeRef.Name.IndexOf("`"));

                // class, interface, structure or delegate
                if (currentPathStack.Count == 0)
                    strTempTypeName = "T:" + strNamespace + strTempShortTypeName;
                else if (currentPathStack.Count > 0 && !IsNested(thisTypeRef))
                    strTempTypeName = strNamespace + strTempShortTypeName;
                else
                    strTempTypeName = "." + strTempShortTypeName;

                currentPathStack.Add(strTempTypeName);

                // add generic _arguments_ (not parameters !)
                if (thisTypeRef.Name.Contains("`") && thisGenericTypeDef != null)
                {
                    bool firstAppend = true;

                    // open bracket
                    currentPathStack.Add("{");

                    foreach (TypeReference tempGenArgument in thisGenericTypeDef.GenericArguments)
                    {
                        // add comma
                        if (!firstAppend)
                            currentPathStack.Add(",");

                        // add argument's xmlDocPath
                        GetXmlDocPathRecursive(tempGenArgument, currentPathStack);

                        // first append done
                        firstAppend = false;
                    }

                    // close bracket
                    currentPathStack.Add("}");
                }
            }
            else if (member is MethodDefinition)
            {
                MethodDefinition thisMethodDef = member as MethodDefinition;

                // method, get type's path firstAppend
                currentPathStack.Add("M:");
                if (member.DeclaringType != null)
                    GetXmlDocPathRecursive(member.DeclaringType, currentPathStack);

                // method's path
                // check whether this is constructor method, or explicitly implemented method
                strExplicitPath = GetXmlDocExplicitIfaceImplPath(member);

                if (thisMethodDef.IsStatic && thisMethodDef.IsConstructor)
                    stbTempPath.Append(".#cctor");
                if (!thisMethodDef.IsStatic && thisMethodDef.IsConstructor)
                    stbTempPath.Append(".#ctor");
                else if (strExplicitPath.Length > 0)
                    stbTempPath.Append("." + strExplicitPath);
                else
                    stbTempPath.Append("." + thisMethodDef.Name);

                // check whether this method is generic
                if (thisMethodDef.GenericParameters.Count > 0)
                    stbTempPath.Append("``" + thisMethodDef.GenericParameters.Count);

                if (thisMethodDef.Parameters.Count > 0)
                    stbTempPath.Append("(");
                bool firstAppend = true;
                foreach (ParameterDefinition TempParam in thisMethodDef.Parameters)
                {
                    if (!firstAppend)
                        stbTempPath.Append(",");

                    stbTempPath.Append(GetXmlDocParameterPath(TempParam.ParameterType, false));
                    firstAppend = false;
                }

                if (thisMethodDef.Parameters.Count > 0)
                    stbTempPath.Append(")");

                // check whether this is a conversion operator (implicit or explicit)
                // if so, we have to read return type and add "~" char.
                if (IsOperator(thisMethodDef))
                {
                    OperatorType OpType = GetOperatorType(thisMethodDef);

                    if (OpType == OperatorType.op_Implicit || OpType == OperatorType.op_Explicit)
                    {
                        // add return type parameter path
                        stbTempPath.Append("~");
                        stbTempPath.Append(GetXmlDocParameterPath(thisMethodDef.ReturnType, false));
                    }
                }

                // add to path
                currentPathStack.Add(stbTempPath.ToString());
            }
            else if (member is FieldDefinition)
            {
                // field, get type's path name
                currentPathStack.Add("F:");
                if (member.DeclaringType != null)
                    GetXmlDocPathRecursive(member.DeclaringType, currentPathStack);

                // field's path
                currentPathStack.Add("." + member.Name);
            }
            else if (member is PropertyDefinition)
            {
                // property or indexer, get declaring type's path 
                currentPathStack.Add("P:");
                if (member.DeclaringType != null)
                    GetXmlDocPathRecursive(member.DeclaringType, currentPathStack);

                // property's path
                // check whether this is explicitly implemented property
                strExplicitPath = GetXmlDocExplicitIfaceImplPath(member);
                if (strExplicitPath.Length > 0)
                    stbTempPath.Append("." + strExplicitPath);
                else
                    stbTempPath.Append("." + member.Name);

                // is it an indexer ?
                bool firstAppend = true;
                PropertyDefinition piProperty = member as PropertyDefinition;
                if (piProperty.Parameters.Count > 0)
                    stbTempPath.Append("(");

                foreach (ParameterDefinition TempParam in piProperty.Parameters)
                {
                    if (!firstAppend)
                        stbTempPath.Append(",");

                    stbTempPath.Append(GetXmlDocParameterPath(TempParam.ParameterType, false));
                    firstAppend = false;
                }

                if (piProperty.Parameters.Count > 0)
                    stbTempPath.Append(")");

                currentPathStack.Add(stbTempPath.ToString());
            }
            else if (member is EventDefinition)
            {
                // event, get type's path firstAppend
                currentPathStack.Add("E:");
                if (member.DeclaringType != null)
                    GetXmlDocPathRecursive(member.DeclaringType, currentPathStack);

                // event's path
                currentPathStack.Add("." + member.Name);
            }
        }

        private static string GetXmlDocParameterPath(TypeReference typeReference, bool explicitMode)
        {
            StringBuilder stbCurrPath = new StringBuilder();

            GetXmlDocParameterPathRecursive(typeReference, explicitMode, stbCurrPath);

            return stbCurrPath.ToString();
        }

        private static void GetXmlDocParameterPathRecursive(TypeReference paramType, bool explicitMode, StringBuilder currentPath)
        {
            if (paramType == null)
                return;

            if (paramType.GenericParameters.Count > 0)
            {
                currentPath.Append(
                    paramType.Namespace +
                    ((CanAppendSpecialExplicitChar() && explicitMode) ? "#" : ".") +
                    StripGenericName(paramType.Name));

                // list parameters or types
                bool firstAppend = true;
                currentPath.Append("{");
                foreach (GenericParameter TempType in paramType.GenericParameters)
                {
                    if (!firstAppend)
                        currentPath.Append(",");

                    currentPath.Append(GetXmlDocParameterPath(TempType, explicitMode));
                    firstAppend = false;
                }
                currentPath.Append("}");
            }
            else if (paramType is GenericInstanceType)
            {
                GenericInstanceType thisGenericType = paramType as GenericInstanceType;

                // if nested, scan enclosing type
                if (paramType.DeclaringType != null)
                    currentPath.Append(GetXmlDocParameterPath(paramType.DeclaringType, explicitMode));

                // determine namespace
                string strNamespace = String.Empty;
                if ((paramType.Namespace != null && paramType.Namespace.Length > 0) || paramType.DeclaringType != null)
                {
                    strNamespace = paramType.Namespace +
                                   ((CanAppendSpecialExplicitChar() && explicitMode) ? "#" : ".");
                }

                currentPath.Append(strNamespace + StripGenericName(thisGenericType.Name));

                // list parameters or types
                bool firstAppend = true;
                currentPath.Append("{");
                foreach (TypeReference tempTypeRef in thisGenericType.GenericArguments)
                {
                    if (!firstAppend)
                        currentPath.Append(",");

                    currentPath.Append(GetXmlDocParameterPath(tempTypeRef, explicitMode));
                    firstAppend = false;
                }
                currentPath.Append("}");
            }
            else if (paramType is GenericParameter)
            {
                GenericParameter thisGenParam = paramType as GenericParameter;

                if (explicitMode)
                {
                    // in explicit mode we print parameter name
                    currentPath.Append(thisGenParam.Name);
                }
                else
                {
                    // in non-explicit mode we print parameter order
                    int paramOrder = 0;

                    // find
                    for (int i = 0; i < thisGenParam.Owner.GenericParameters.Count; i++)
                    {
                        if (thisGenParam.Owner.GenericParameters[i].Name == paramType.Name)
                        {
                            paramOrder = i;
                            break;
                        }
                    }
                    if (thisGenParam.Owner is MethodReference)
                        currentPath.Append("``" + paramOrder);
                    else
                        currentPath.Append("`" + paramOrder);
                }
            }
            else if (paramType is PointerType)
            {
                // parameter is pointer type
                currentPath.Append(GetXmlDocParameterPath((paramType as PointerType).ElementType, explicitMode));
                currentPath.Append("*");
            }
            else if (paramType is ArrayType)
            {
                ArrayType thisArrayType = paramType as ArrayType;
                if (thisArrayType.ElementType != null)
                    currentPath.Append(GetXmlDocParameterPath(thisArrayType.ElementType, explicitMode));

                int iRank = thisArrayType.Rank;
                if (iRank == 1)
                {
                    currentPath.Append("[]");
                }
                else
                {
                    bool firstAppend = true;
                    currentPath.Append("[");

                    for (int i = 0; i < (explicitMode ? iRank - 1 : iRank); i++)
                    {
                        // in explicit mode for .NET3.5/VS2008, 
                        // there is no separator char "," used for multi-dimensional array,
                        // so there are three cases when comma shall be added:
                        // firstAppend = false; ExplicitMode = false; CanAppendSpecialExplicitChar() = true;
                        // firstAppend = false; ExplicitMode = false; CanAppendSpecialExplicitChar() = false;
                        // firstAppend = false; ExplicitMode = true; CanAppendSpecialExplicitChar() = false;
                        // below this is stored in decent manner
                        if (!firstAppend && (!explicitMode || !CanAppendSpecialExplicitChar()))
                            currentPath.Append(",");

                        currentPath.Append(((CanAppendSpecialExplicitChar() && explicitMode) ? "@" : "0:"));
                        if (thisArrayType.Dimensions[i].UpperBound > 0)
                            currentPath.Append(thisArrayType.Dimensions[i].UpperBound.ToString());
                        firstAppend = false;
                    }

                    currentPath.Append("]");
                }
            }
            else if (paramType is ByReferenceType)
            {
                // parameter is passed by reference
                currentPath.Append(GetXmlDocParameterPath((paramType as ByReferenceType).ElementType, false));
                currentPath.Append("@");
            }
            else if (paramType.IsOptionalModifier)
            {
                currentPath.Append(GetXmlDocParameterPath(paramType.GetElementType(), explicitMode));
                currentPath.Append("!");
                
                currentPath.Append(GetXmlDocParameterPath((paramType as IModifierType).ModifierType, explicitMode));
            }
            else if (paramType.IsRequiredModifier)
            {
                currentPath.Append(GetXmlDocParameterPath(paramType.GetElementType(), explicitMode));
                currentPath.Append("|");
                currentPath.Append(GetXmlDocParameterPath((paramType as IModifierType).ModifierType, explicitMode));
            }
            else if (paramType is FunctionPointerType)
            {
                // type is function pointer
                FunctionPointerType thisFuncPtr = paramType as FunctionPointerType;
                string tempString = String.Empty;

                // return type
                currentPath.Append("=FUNC:");
                currentPath.Append(GetXmlDocParameterPath(thisFuncPtr.ReturnType, explicitMode));

                // method's parameters
                if (thisFuncPtr.Parameters.Count > 0)
                {
                    bool firstAppend = true;
                    currentPath.Append("(");

                    foreach (ParameterDefinition tempParam in thisFuncPtr.Parameters)
                    {
                        if (!firstAppend)
                            currentPath.Append(",");

                        currentPath.Append(GetXmlDocParameterPath(tempParam.ParameterType, explicitMode));
                        firstAppend = false;
                    }

                    currentPath.Append(")");
                }
                else
                {
                    currentPath.Append("(System.Void)");
                }
            }
            else if (paramType is PinnedType)
            {
                // type is pinned type
                currentPath.Append(GetXmlDocParameterPath((paramType as PinnedType).ElementType, explicitMode));
                currentPath.Append("^");
            }
            else if (paramType is TypeReference)
            {
                // if nested, scan enclosing type
                if (paramType.DeclaringType != null)
                    currentPath.Append(GetXmlDocParameterPath(paramType.DeclaringType, explicitMode));

                // determine namespace
                string strNamespace = String.Empty;
                if ((paramType.Namespace != null && paramType.Namespace.Length > 0) || paramType.DeclaringType != null)
                {
                    strNamespace = paramType.Namespace +
                                   ((CanAppendSpecialExplicitChar() && explicitMode) ? "#" : ".");
                }

                // concrete type
                currentPath.Append(
                    strNamespace +
                    ((CanAppendSpecialExplicitChar() && explicitMode) ? paramType.Name.Replace(".", "#") : paramType.Name));
            }
        }

        private static OperatorType GetOperatorType(MethodDefinition OperatorMethod)
        {
            try
            {
                return (OperatorType) Enum.Parse(typeof (OperatorType), OperatorMethod.Name.Trim());
            }
            catch
            {
                return OperatorType.None;
            }
        }

        public static bool IsNested(
            TypeReference Type)
        {
            if (Type.IsNested)
                return true;

            return false;
        }

        private static bool IsOperator(MethodDefinition Method)
        {
            if (Method.IsSpecialName && Method.Name.StartsWith("op_"))
                return true;

            return false;
        }

        private static bool CanAppendSpecialExplicitChar()
        {
            return true;
        }

        private static string StripGenericName(string OrginalClassName)
        {
            if (OrginalClassName.IndexOf("`") != -1)
                return OrginalClassName.Remove(OrginalClassName.IndexOf("`"));
            else
                return OrginalClassName;
        }
    }
}