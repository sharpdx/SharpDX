// Copyright (c) 2010-2013 SharpDoc - Alexandre Mutel
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
// -----------------------------------------------------------------------------
// Code from Przemyslaw Celej
// http://www.codeproject.com/KB/dotnet/xmlDocId.aspx
// -----------------------------------------------------------------------------
//Preamble
//This License governs Your use of the Work. This License is intended to allow developers to use the Source Code and Executable Files provided as part of the Work in any application in any form.
//The main points subject to the terms of the License are:
//    Source Code and Executable Files can be used in commercial applications;
//    Source Code and Executable Files can be redistributed; and
//    Source Code can be modified to create derivative works.
//    No claim of suitability, guarantee, or any warranty whatsoever is provided. The software is provided "as-is".
//    The Article accompanying the Work may not be distributed or republished without the Author's consent
//This License is entered between You, the individual or other entity reading or otherwise making use of the Work licensed pursuant to this License and the individual or other entity which offers the Work under the terms of this License ("Author").
//License
//THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS CODE PROJECT OPEN LICENSE ("LICENSE"). THE WORK IS PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. ANY USE OF THE WORK OTHER THAN AS AUTHORIZED UNDER THIS LICENSE OR COPYRIGHT LAW IS PROHIBITED.
//BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HEREIN, YOU ACCEPT AND AGREE TO BE BOUND BY THE TERMS OF THIS LICENSE. THE AUTHOR GRANTS YOU THE RIGHTS CONTAINED HEREIN IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS. IF YOU DO NOT AGREE TO ACCEPT AND BE BOUND BY THE TERMS OF THIS LICENSE, YOU CANNOT MAKE ANY USE OF THE WORK.
//    Definitions.
//        "Articles" means, collectively, all articles written by Author which describes how the Source Code and Executable Files for the Work may be used by a user.
//        "Author" means the individual or entity that offers the Work under the terms of this License.
//        "Derivative Work" means a work based upon the Work or upon the Work and other pre-existing works.
//        "Executable Files" refer to the executables, binary files, configuration and any required data files included in the Work.
//        "Publisher" means the provider of the website, magazine, CD-ROM, DVD or other medium from or by which the Work is obtained by You.
//        "Source Code" refers to the collection of source code and configuration files used to create the Executable Files.
//        "Standard Version" refers to such a Work if it has not been modified, or has been modified in accordance with the consent of the Author, such consent being in the full discretion of the Author.
//        "Work" refers to the collection of files distributed by the Publisher, including the Source Code, Executable Files, binaries, data files, documentation, whitepapers and the Articles.
//        "You" is you, an individual or entity wishing to use the Work and exercise your rights under this License.
//    Fair Use/Fair Use Rights. Nothing in this License is intended to reduce, limit, or restrict any rights arising from fair use, fair dealing, first sale or other limitations on the exclusive rights of the copyright owner under copyright law or other applicable laws.
//    License Grant. Subject to the terms and conditions of this License, the Author hereby grants You a worldwide, royalty-free, non-exclusive, perpetual (for the duration of the applicable copyright) license to exercise the rights in the Work as stated below:
//        You may use the standard version of the Source Code or Executable Files in Your own applications.
//        You may apply bug fixes, portability fixes and other modifications obtained from the Public Domain or from the Author. A Work modified in such a way shall still be considered the standard version and will be subject to this License.
//        You may otherwise modify Your copy of this Work (excluding the Articles) in any way to create a Derivative Work, provided that You insert a prominent notice in each changed file stating how, when and where You changed that file.
//        You may distribute the standard version of the Executable Files and Source Code or Derivative Work in aggregate with other (possibly commercial) programs as part of a larger (possibly commercial) software distribution.
//        The Articles discussing the Work published in any form by the author may not be distributed or republished without the Author's consent. The author retains copyright to any such Articles. You may use the Executable Files and Source Code pursuant to this License but you may not repost or republish or otherwise distribute or make available the Articles, without the prior written consent of the Author.
//    Any subroutines or modules supplied by You and linked into the Source Code or Executable Files of this Work shall not be considered part of this Work and will not be subject to the terms of this License.
//    Patent License. Subject to the terms and conditions of this License, each Author hereby grants to You a perpetual, worldwide, non-exclusive, no-charge, royalty-free, irrevocable (except as stated in this section) patent license to make, have made, use, import, and otherwise transfer the Work.
//    Restrictions. The license granted in Section 3 above is expressly made subject to and limited by the following restrictions:
//        You agree not to remove any of the original copyright, patent, trademark, and attribution notices and associated disclaimers that may appear in the Source Code or Executable Files.
//        You agree not to advertise or in any way imply that this Work is a product of Your own.
//        The name of the Author may not be used to endorse or promote products derived from the Work without the prior written consent of the Author.
//        You agree not to sell, lease, or rent any part of the Work. This does not restrict you from including the Work or any part of the Work inside a larger software distribution that itself is being sold. The Work by itself, though, cannot be sold, leased or rented.
//        You may distribute the Executable Files and Source Code only under the terms of this License, and You must include a copy of, or the Uniform Resource Identifier for, this License with every copy of the Executable Files or Source Code You distribute and ensure that anyone receiving such Executable Files and Source Code agrees that the terms of this License apply to such Executable Files and/or Source Code. You may not offer or impose any terms on the Work that alter or restrict the terms of this License or the recipients' exercise of the rights granted hereunder. You may not sublicense the Work. You must keep intact all notices that refer to this License and to the disclaimer of warranties. You may not distribute the Executable Files or Source Code with any technological measures that control access or use of the Work in a manner inconsistent with the terms of this License.
//        You agree not to use the Work for illegal, immoral or improper purposes, or on pages containing illegal, immoral or improper material. The Work is subject to applicable export laws. You agree to comply with all such laws and regulations that may apply to the Work after Your receipt of the Work.
//    Representations, Warranties and Disclaimer. THIS WORK IS PROVIDED "AS IS", "WHERE IS" AND "AS AVAILABLE", WITHOUT ANY EXPRESS OR IMPLIED WARRANTIES OR CONDITIONS OR GUARANTEES. YOU, THE USER, ASSUME ALL RISK IN ITS USE, INCLUDING COPYRIGHT INFRINGEMENT, PATENT INFRINGEMENT, SUITABILITY, ETC. AUTHOR EXPRESSLY DISCLAIMS ALL EXPRESS, IMPLIED OR STATUTORY WARRANTIES OR CONDITIONS, INCLUDING WITHOUT LIMITATION, WARRANTIES OR CONDITIONS OF MERCHANTABILITY, MERCHANTABLE QUALITY OR FITNESS FOR A PARTICULAR PURPOSE, OR ANY WARRANTY OF TITLE OR NON-INFRINGEMENT, OR THAT THE WORK (OR ANY PORTION THEREOF) IS CORRECT, USEFUL, BUG-FREE OR FREE OF VIRUSES. YOU MUST PASS THIS DISCLAIMER ON WHENEVER YOU DISTRIBUTE THE WORK OR DERIVATIVE WORKS.
//    Indemnity. You agree to defend, indemnify and hold harmless the Author and the Publisher from and against any claims, suits, losses, damages, liabilities, costs, and expenses (including reasonable legal or attorneys’ fees) resulting from or relating to any use of the Work by You.
//    Limitation on Liability. EXCEPT TO THE EXTENT REQUIRED BY APPLICABLE LAW, IN NO EVENT WILL THE AUTHOR OR THE PUBLISHER BE LIABLE TO YOU ON ANY LEGAL THEORY FOR ANY SPECIAL, INCIDENTAL, CONSEQUENTIAL, PUNITIVE OR EXEMPLARY DAMAGES ARISING OUT OF THIS LICENSE OR THE USE OF THE WORK OR OTHERWISE, EVEN IF THE AUTHOR OR THE PUBLISHER HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGES.
//    Termination.
//        This License and the rights granted hereunder will terminate automatically upon any breach by You of any term of this License. Individuals or entities who have received Derivative Works from You under this License, however, will not have their licenses terminated provided such individuals or entities remain in full compliance with those licenses. Sections 1, 2, 6, 7, 8, 9, 10 and 11 will survive any termination of this License.
//        If You bring a copyright, trademark, patent or any other infringement claim against any contributor over infringements You claim are made by the Work, your License from such contributor to the Work ends automatically.
//        Subject to the above terms and conditions, this License is perpetual (for the duration of the applicable copyright in the Work). Notwithstanding the above, the Author reserves the right to release the Work under different license terms or to stop distributing the Work at any time; provided, however that any such election will not serve to withdraw this License (or any other license that has been, or is required to be, granted under the terms of this License), and this License will continue in full force and effect unless terminated as stated above.
//    Publisher. The parties hereby confirm that the Publisher shall not, under any circumstances, be responsible for and shall not have any liability in respect of the subject matter of this License. The Publisher makes no warranty whatsoever in connection with the Work and shall not be liable to You or any party on any legal theory for any damages whatsoever, including without limitation any general, special, incidental or consequential damages arising in connection to this license. The Publisher reserves the right to cease making the Work available to You at any time without notice
//    Miscellaneous
//        This License shall be governed by the laws of the location of the head office of the Author or if the Author is an individual, the laws of location of the principal place of residence of the Author.
//        If any provision of this License is invalid or unenforceable under applicable law, it shall not affect the validity or enforceability of the remainder of the terms of this License, and without further action by the parties to this License, such provision shall be reformed to the minimum extent necessary to make such provision valid and enforceable.
//        No term or provision of this License shall be deemed waived and no breach consented to unless such waiver or consent shall be in writing and signed by the party to be charged with such waiver or consent.
//        This License constitutes the entire agreement between the parties with respect to the Work licensed herein. There are no understandings, agreements or representations with respect to the Work not specified herein. The Author shall not be bound by any additional provisions that may appear in any communication from You. This License may not be modified without the mutual written agreement of the Author and You.

using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace WikiGenApp
{
    /// <summary>
    /// Helper class to transform a member reference (type, method...etc) to an xml doc Id
    /// according to http://msdn.microsoft.com/en-us/library/fsbx0t7x.aspx
    /// </summary>
    public class DocIdHelper
    {
        /// <summary>
        /// Strips a XML doc id by replacing characters that cannot be used for a filename
        /// by valid chars for filename.
        /// Replace: ':' => '_', '.' => '_', '#', '_', '`' => '_', '{' => '-', '}' => '-'
        /// </summary>
        /// <param name="xmlId">The XML doc id.</param>
        /// <returns>A stripped version of the xml doc id suitable to use in filenames</returns>
        public static string StripXmlId(string xmlId)
        {
            string id = xmlId;
            int index = id.IndexOf("(");
            if (index > 0)
                id = id.Substring(0, index);

            return id.Replace(':', '_').Replace('.', '_').Replace('#', '_').Replace('`', '_').Replace('{', '-').Replace('}', '-');
        }

        /// <summary>
        /// Gets a Xml doc identifier for the given member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns>An xml doc id.</returns>
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
                        var stbParameters = new StringBuilder();

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
            var builderStrippedName = new StringBuilder();

            // split name 
            string[] strSlices = originalName.Split(new[] { '`' });

            // remove numbers at the beginning of each string to "<" charter
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

                    // return immediately, because there is nothing to do.
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

                // remove any trailing reference syntax.
                strTempShortTypeName = strTempShortTypeName.TrimEnd('&', '%');

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
                var thisMethodDef = member as MethodDefinition;

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
                var piProperty = member as PropertyDefinition;
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
            var stbCurrPath = new StringBuilder();

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
                var thisGenericType = paramType as GenericInstanceType;

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
                var thisGenParam = paramType as GenericParameter;

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
                var thisArrayType = paramType as ArrayType;
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
                var thisFuncPtr = paramType as FunctionPointerType;
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
                return (OperatorType)Enum.Parse(typeof(OperatorType), OperatorMethod.Name.Trim());
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

        public enum OperatorType
        {
            None,
            op_Implicit,
            op_Explicit,

            // overloadable unary operators
            op_Decrement,                   // --
            op_Increment,                   // ++
            op_UnaryNegation,               // -
            op_UnaryPlus,                   // +
            op_LogicalNot,                  // !
            op_True,                        // true
            op_False,                       // false
            op_OnesComplement,              // ~
            op_Like,                        // Like (Visual Basic)


            // overloadable binary operators
            op_Addition,                    // +
            op_Subtraction,                 // -
            op_Division,                    // /
            op_Multiply,                    // *
            op_Modulus,                     // %
            op_BitwiseAnd,                  // &
            op_ExclusiveOr,                 // ^
            op_LeftShift,                   // <<
            op_RightShift,                  // >>
            op_BitwiseOr,                   // |

            // overloadable comparison operators
            op_Equality,                    // ==
            op_Inequality,                  // != 
            op_LessThanOrEqual,             // <=
            op_LessThan,                    // <
            op_GreaterThanOrEqual,          // >=
            op_GreaterThan,                 // >

            // not overloadable operators
            op_AddressOf,                       // &
            op_PointerDereference,              // *
            op_LogicalAnd,                      // &&
            op_LogicalOr,                       // ||
            op_Assign,                          // Not defined (= is not the same)
            op_SignedRightShift,                // Not defined
            op_UnsignedRightShift,              // Not defined
            op_UnsignedRightShiftAssignment,    // Not defined
            op_MemberSelection,                 // ->
            op_RightShiftAssignment,            // >>=
            op_MultiplicationAssignment,        // *=
            op_PointerToMemberSelection,        // ->*
            op_SubtractionAssignment,           // -=
            op_ExclusiveOrAssignment,           // ^=
            op_LeftShiftAssignment,             // <<=
            op_ModulusAssignment,               // %=
            op_AdditionAssignment,              // +=
            op_BitwiseAndAssignment,            // &=
            op_BitwiseOrAssignment,             // |=
            op_Comma,                           // ,
            op_DivisionAssignment               // /=
        }
    }
}