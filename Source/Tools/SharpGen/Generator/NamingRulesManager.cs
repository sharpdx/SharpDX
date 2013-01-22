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
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SharpGen.Config;
using SharpGen.CppModel;

namespace SharpGen.Generator
{
    /// <summary>
    /// This class handles renaming according to conventions. Pascal case (NamingRulesManager) for global types,  
    /// Camel case (namingRulesManager) for parameters.
    /// </summary>
    public class NamingRulesManager
    {
        private readonly List<ShortNameMapper> _expandShortName = new List<ShortNameMapper>();

        /// <summary>
        /// The recorded names, a list of previous name and new name.
        /// </summary>
        public readonly List<Tuple<CppElement, string>> RecordNames = new List<Tuple<CppElement, string>>();

        /// <summary>
        /// Adds the short name rule.
        /// </summary>
        /// <param name="regexShortName">Short name of the regex.</param>
        /// <param name="expandedName">Name of the expanded.</param>
        public void AddShortNameRule(string regexShortName, string expandedName)
        {
            _expandShortName.Add(new ShortNameMapper(regexShortName, expandedName));
            // Not efficient, but we order from longest to shortest regex
            _expandShortName.Sort((left, right) => -left.Regex.ToString().Length.CompareTo(right.Regex.ToString().Length));
        }

        /// <summary>
        /// Renames a C++++ element
        /// </summary>
        /// <param name="cppElement">The C++ element.</param>
        /// <param name="rootName">Name of the root.</param>
        /// <returns>The new name</returns>
        private string RenameCore(CppElement cppElement, string rootName = null)
        {
            string originalName = cppElement.Name;
            string name = cppElement.Name;

            var namingFlags = NamingFlags.Default;
            bool nameModifiedByTag = false;

            // Handle Tag
            var tag = cppElement.GetTagOrDefault<MappingRule>();
            if (tag != null)
            {
                if (!string.IsNullOrEmpty(tag.MappingName))
                {
                    nameModifiedByTag = true;
                    name = tag.MappingName;
                    // If Final Mapping name then don't proceed further
                    if (tag.IsFinalMappingName.HasValue && tag.IsFinalMappingName.Value)
                        return name;
                }

                if (tag.NamingFlags.HasValue)
                    namingFlags = tag.NamingFlags.Value;
            }

            // Rename is tagged as final, then return the string
            // If the string still contains some "_" then continue while processing
            if (!name.Contains("_") && name.ToUpper() != name && char.IsUpper(name[0]))
                return name;

            // Remove Prefix (for enums). Don't modify names that are modified by tag
            if (!nameModifiedByTag && rootName != null && originalName.StartsWith(rootName))
                name = originalName.Substring(rootName.Length, originalName.Length - rootName.Length);

            // Remove leading '_'
            name = name.TrimStart('_');

            // Convert rest of the string in CamelCase
            name = ConvertToPascalCase(name, namingFlags);
            return name;
        }

        /// <summary>
        /// Renames the specified C++ element.
        /// </summary>
        /// <param name="cppElement">The C++ element.</param>
        /// <returns>The C# name</returns>
        public string Rename(CppElement cppElement)
        {
            return RecordRename(cppElement, UnKeyword(RenameCore(cppElement)));
        }

        /// <summary>
        /// Renames the specified C++ enum item.
        /// </summary>
        /// <param name="cppEnumItem">The C++ enum item.</param>
        /// <param name="rootEnumName">Name of the root C++ enum.</param>
        /// <returns>The C# name of this enum item</returns>
        public string Rename(CppEnumItem cppEnumItem, string rootEnumName)
        {
            return RecordRename(cppEnumItem, UnKeyword(RenameCore(cppEnumItem, rootEnumName)));
        }

        /// <summary>
        /// Renames the specified C++ parameter.
        /// </summary>
        /// <param name="cppParameter">The C++ parameter.</param>
        /// <returns>The C# name of this parameter.</returns>
        public string Rename(CppParameter cppParameter)
        {
            string oldName = cppParameter.Name;
            string name = RenameCore(cppParameter, null);

            bool hasPointer = !string.IsNullOrEmpty(cppParameter.Pointer) &&
                              (cppParameter.Pointer.Contains("*") || cppParameter.Pointer.Contains("&"));
            if (hasPointer)
            {
                if (oldName.StartsWith("pp"))
                    name = name.Substring(2) + "Out";
                else if (oldName.StartsWith("p"))
                    name = name.Substring(1) + "Ref";
            }
            if (char.IsDigit(name[0]))
                name = "arg" + name;


            name = new string(name[0], 1).ToLower() + name.Substring(1);

            return RecordRename(cppParameter, UnKeyword(name));
        }

        /// <summary>
        /// Dump the names changes as a comma separated list of [FromName,ToName]
        /// </summary>
        /// <param name="writer">Text output of the dump</param>
        public void DumpRenames(TextWriter writer)
        {
            foreach (var recordName in RecordNames)
            {
                writer.WriteLine("{0},{1}", recordName.Item1.FullName, recordName.Item2);
            }
        }

        /// <summary>
        /// Record the name source and the modified name.
        /// </summary>
        /// <param name="fromElement">The element to rename</param>
        /// <param name="toName">The new name</param>
        /// <returns>The new name</returns>
        private string RecordRename(CppElement fromElement, string toName)
        {
            RecordNames.Add(new Tuple<CppElement, string>(fromElement, toName));
            return toName;
        }

        /// <summary>
        /// Protecte the name from all C# reserved words.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private static string UnKeyword(string name)
        {
            if (IsKeyword(name))
            {
                if (name == "string")
                    return "text";
                name = "@" + name;
            }
            return name;            
        }

        /// <summary>
        /// Determines whether the specified string is a valid pascal case.
        /// </summary>
        /// <param name="str">The string to validate.</param>
        /// <param name="lowerCount">The lower count.</param>
        /// <returns>
        /// 	<c>true</c> if the specified string is a valid pascal case; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsPascalCase(string str, out int lowerCount)
        {
            // Count the number of char in lower case
            lowerCount = str.Count(charInStr => char.IsLower(charInStr));

            if (str.Length == 0)
                return false;

            // First char must be a letter
            if (!char.IsLetter(str[0]))
                return false;

            // First letter must be upper
            if (!char.IsUpper(str[0]))
                return false;

            // Second letter must be lower
            if (str.Length > 1 && char.IsUpper(str[1]))
                return false;

            // other chars must be letter or numbers
            //foreach (char charInStr in str)
            //{
            //    if (!char.IsLetterOrDigit(charInStr))
            //        return false;
            //}
            return str.All(charInStr => char.IsLetterOrDigit(charInStr));
        }

        /// <summary>
        /// Converts a string to PascalCase..
        /// </summary>
        /// <param name="text">The text to convert.</param>
        /// <param name="keepUnderscore">if set to <c>true</c> keep underscore in this name.</param>
        /// <returns></returns>
        public string ConvertToPascalCase(string text, NamingFlags namingFlags)
        {
            string[] splittedPhrase = text.Split('_');
            var sb = new StringBuilder();

            for (int i = 0; i < splittedPhrase.Length; i++)
            {
                string subPart = splittedPhrase[i];

                // Don't perform expansion when asked
                if ((namingFlags & NamingFlags.NoShortNameExpand) == 0)
                {
                    while (subPart.Length > 0)
                    {
                        bool continueReplace = false;
                        foreach (var regExp in _expandShortName)
                        {
                            var regex = regExp.Regex;
                            var newText = regExp.Replace;

                            if (regex.Match(subPart).Success)
                            {
                                if (regExp.HasRegexReplace)
                                {
                                    subPart = regex.Replace(subPart, regExp.Replace);
                                    sb.Append(subPart);
                                    subPart = string.Empty;
                                }
                                else
                                {
                                    subPart = regex.Replace(subPart, string.Empty);
                                    sb.Append(newText);
                                    continueReplace = true;
                                }
                                break;
                            }
                        }

                        if (!continueReplace)
                        {
                            break;
                        }
                    }
                }

                // Else, perform a standard convertion
                if (subPart.Length > 0)
                {
                    int numberOfCharLowercase;
                    // If string is not Pascal Case, then Pascal Case it
                    if (IsPascalCase(subPart, out numberOfCharLowercase))
                    {
                        sb.Append(subPart);
                    }
                    else
                    {
                        char[] splittedPhraseChars = (numberOfCharLowercase > 0)
                                                         ? subPart.ToCharArray()
                                                         : subPart.ToLower().ToCharArray();

                        if (splittedPhraseChars.Length > 0)
                            splittedPhraseChars[0] = char.ToUpper(splittedPhraseChars[0]);
                        sb.Append(new String(splittedPhraseChars));
                    }
                }

                if ( (namingFlags & NamingFlags.KeepUnderscore) != 0 && (i + 1) < splittedPhrase.Length)
                    sb.Append("_");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns true if the name is a C# keyword
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static bool IsKeyword(string name)
        {
            return CSharpKeywords.Contains(name);
        }

        private class ShortNameMapper
        {
            public ShortNameMapper(string regex, string replace)
            {
                Regex = new Regex("^" + regex);
                Replace = replace;
                HasRegexReplace = replace.Contains("$");
            }

            public Regex Regex;

            public string Replace;

            public bool HasRegexReplace;
        }

        /// <summary>
        /// Reserved C# keywords.
        /// </summary>
        private static readonly string[] CSharpKeywords = new[]
                                                               {
                                                                   "abstract",
                                                                   "as",
                                                                   "base",
                                                                   "bool",
                                                                   "break",
                                                                   "byte",
                                                                   "case",
                                                                   "catch",
                                                                   "char",
                                                                   "checked",
                                                                   "class",
                                                                   "const",
                                                                   "continue",
                                                                   "decimal",
                                                                   "default",
                                                                   "delegate",
                                                                   "do",
                                                                   "double",
                                                                   "else",
                                                                   "enum",
                                                                   "event",
                                                                   "explicit",
                                                                   "extern",
                                                                   "false",
                                                                   "finally",
                                                                   "fixed",
                                                                   "float",
                                                                   "for",
                                                                   "foreach",
                                                                   "goto",
                                                                   "if",
                                                                   "implicit",
                                                                   "in",
                                                                   "int",
                                                                   "interface",
                                                                   "internal",
                                                                   "is",
                                                                   "lock",
                                                                   "long",
                                                                   "namespace",
                                                                   "new",
                                                                   "null",
                                                                   "object",
                                                                   "operator",
                                                                   "out",
                                                                   "override",
                                                                   "params",
                                                                   "private",
                                                                   "protected",
                                                                   "public",
                                                                   "readonly",
                                                                   "ref",
                                                                   "return",
                                                                   "sbyte",
                                                                   "sealed",
                                                                   "short",
                                                                   "sizeof",
                                                                   "stackalloc",
                                                                   "static",
                                                                   "string",
                                                                   "struct",
                                                                   "switch",
                                                                   "this",
                                                                   "throw",
                                                                   "true",
                                                                   "try",
                                                                   "typeof",
                                                                   "uint",
                                                                   "ulong",
                                                                   "unchecked",
                                                                   "unsafe",
                                                                   "ushort",
                                                                   "using",
                                                                   "virtual",
                                                                   "volatile",
                                                                   "void",
                                                                   "while",
                                                               };
    }
}