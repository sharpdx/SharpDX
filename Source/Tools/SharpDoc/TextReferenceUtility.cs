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
using System.Text;
using System.Text.RegularExpressions;
using SharpDoc.Model;

namespace SharpDoc
{

    /// <summary>
    /// 
    /// </summary>
    public static class TextReferenceUtility
    {
        public static IModelReference CreateReference(string api)
        {
            if (String.IsNullOrEmpty(api)) throw new ArgumentException("api");

            var reference = new NReference();
            reference.Id = api;

            reference.FullName = GetFullName(api);
            reference.Name = GetNameFromFullName(reference.FullName);
            return reference;
        }

        private static Regex ReplaceTemplate = new Regex(@"\{(.*?)\}");
        private static Regex ReplaceGenerics = new Regex(@"`+(\d+)");

        private static string ReplaceGenericsEvaluator(Match match)
        {
            int nbGenerics = Int32.Parse(match.Groups[1].Value);
            var text = new StringBuilder();
            text.Append("<");
            for(int i = 0; i < nbGenerics; i++)
            {
                if (i > 0)
                    text.Append(",");
                text.Append("T").Append(i+1);
            }
            text.Append(">");
            return text.ToString();
        }
        
        private static string GetFullName(string id)
        {
            id = id.Substring("X:".Length, id.Length - "X:".Length);
            id = ReplaceTemplate.Replace(id, "<$1>");
            id = ReplaceGenerics.Replace(id, ReplaceGenericsEvaluator);
            return id;
        }

        private static string GetNameFromFullName(string fullname)
        {
            int index;
            int indexOfMethod = fullname.IndexOf("(");
            if (indexOfMethod > 0)
            {
                index = fullname.LastIndexOf(".", indexOfMethod);
            } else
            {
                index = fullname.LastIndexOf(".");
            }

            return index > 0 ? fullname.Substring(index + 1, fullname.Length - index - 1) : fullname;                
        }
    }
}