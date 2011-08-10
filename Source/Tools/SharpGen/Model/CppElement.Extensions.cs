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
using System.Text.RegularExpressions;
using SharpGen.Config;
using SharpGen.CppModel;

namespace SharpGen.Model
{
    public static class CppElementExtensions
    {

        private static string LastCppOuterElement = "???";

        public static void Tag<T>(this CppElement element, string regex, MappingRule tag) where T : CppElement
        {
            string regexStr = CppElement.StripRegex(regex);
            if (typeof(CppMethod).IsAssignableFrom(typeof(T)) || typeof(CppStruct).IsAssignableFrom(typeof(T)))
            {
                LastCppOuterElement = regexStr;
            }
            else if ( (typeof(T) == typeof(CppParameter) || typeof(T) == typeof(CppField)) && !regexStr.Contains("::"))
            {
                regexStr = LastCppOuterElement + "::" + regexStr;
            }

            //element.Logger.Flush();
            element.Modify<T>(regexStr, ProcessTag(tag));
        }

        /// <summary>
        /// Tag an Enum and force it to be interpreted as a flag.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="regex"></param>
        public static string GetTypeNameWithMapping(this CppElement cppType)
        {
            var tag = cppType.GetTagOrDefault<MappingRule>();
            if (tag != null && tag.MappingType != null)
                return tag.MappingType;
            if (cppType is CppEnum)
                return "int";
            if (cppType is CppType)
                return (cppType as CppType).TypeName;
            throw new ArgumentException(string.Format("Cannot get typename from type {0}", cppType));
        }

        private static string RegexRename(Regex regex, string fromName, string replaceName)
        {
            return replaceName.Contains("$")? regex.Replace(fromName, replaceName) : replaceName;            
        }

        /// <summary>
        ///   Fully rename a type and all references
        /// </summary>
        /// <param name = "fromType"></param>
        /// <param name = "toType"></param>
        /// <returns></returns>
        private static CppElement.ProcessModifier ProcessTag(MappingRule fromTag)
        {
            return (pathREgex, element) =>
                       {
                           MappingRule tag = element.Tag as MappingRule;
                           if (tag == null)
                           {
                               tag = new MappingRule();
                               element.Tag = tag;
                           }
                           if (fromTag.MethodCheckReturnType.HasValue) tag.MethodCheckReturnType = fromTag.MethodCheckReturnType;
                           if (fromTag.Visibility.HasValue) tag.Visibility = fromTag.Visibility;
                           if (fromTag.NativeCallbackVisibility.HasValue) tag.NativeCallbackVisibility = fromTag.NativeCallbackVisibility;
                           if (fromTag.NativeCallbackName != null) tag.NativeCallbackName = fromTag.NativeCallbackName;
                           if (fromTag.Property.HasValue) tag.Property = fromTag.Property;
                           if (fromTag.MappingName != null) 
                               tag.MappingName = RegexRename(pathREgex, element.FullName, fromTag.MappingName);
                           if (fromTag.NameKeepUnderscore.HasValue) tag.NameKeepUnderscore = fromTag.NameKeepUnderscore.Value;
                           if (fromTag.IsFinalMappingName != null) tag.IsFinalMappingName = fromTag.IsFinalMappingName;
                           if (fromTag.StructHasNativeValueType != null) tag.StructHasNativeValueType = fromTag.StructHasNativeValueType;
                           if (fromTag.StructToClass != null) tag.StructToClass = fromTag.StructToClass;
                           if (fromTag.StructCustomMarshall != null) tag.StructCustomMarshall = fromTag.StructCustomMarshall;
                           if (fromTag.StructCustomNew != null) tag.StructCustomNew = fromTag.StructCustomNew;
                           if (fromTag.StructForceMarshalToToBeGenerated != null)
                               tag.StructForceMarshalToToBeGenerated = fromTag.StructForceMarshalToToBeGenerated;
                           if (fromTag.MappingType != null) tag.MappingType = RegexRename(pathREgex, element.FullName, fromTag.MappingType);

                           var cppType = element as CppType;
                           if (cppType != null)
                           {
                               if (tag.MappingType != null)
                                   cppType.TypeName = tag.MappingType;

                               if (fromTag.Pointer != null)
                               {
                                   cppType.Pointer = fromTag.Pointer;
                                   tag.Pointer = fromTag.Pointer;
                               }
                               if (fromTag.TypeArrayDimension != null)
                               {
                                   cppType.ArrayDimension = fromTag.TypeArrayDimension;
                                   if (fromTag.TypeArrayDimension == null)
                                       cppType.IsArray = false;
                                   tag.TypeArrayDimension = fromTag.TypeArrayDimension;
                               }
                           }
                           if (fromTag.EnumHasFlags != null) tag.EnumHasFlags = fromTag.EnumHasFlags;
                           if (fromTag.EnumHasNone != null) tag.EnumHasNone = fromTag.EnumHasNone;
                           if (fromTag.IsCallbackInterface != null) tag.IsCallbackInterface = fromTag.IsCallbackInterface;
                           if (fromTag.IsDualCallbackInterface != null) tag.IsDualCallbackInterface = fromTag.IsDualCallbackInterface;
                           if (fromTag.IsKeepImplementPublic != null) tag.IsKeepImplementPublic = fromTag.IsKeepImplementPublic;
                           if (fromTag.FunctionDllName != null) tag.FunctionDllName = RegexRename(pathREgex, element.FullName, fromTag.FunctionDllName);
                           if (fromTag.FunctionDllNameFromMacro != null)
                               tag.FunctionDllName = element.ParentRoot.FindFirst<CppDefine>(fromTag.FunctionDllNameFromMacro).StripStringValue;
                           if (fromTag.CsClass != null) tag.CsClass = fromTag.CsClass;
                           if (fromTag.ParameterAttribute != null && element is CppParameter)
                           {
                               (element as CppParameter).Attribute = fromTag.ParameterAttribute.Value;
                               tag.ParameterAttribute = fromTag.ParameterAttribute.Value;
                           }
                           if (fromTag.ParameterUsedAsReturnType != null ) tag.ParameterUsedAsReturnType = fromTag.ParameterUsedAsReturnType;
                           return false;
                       };
        }
    }
}