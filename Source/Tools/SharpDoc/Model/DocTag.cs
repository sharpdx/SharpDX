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

namespace SharpDoc.Model
{
    /// <summary>
    /// XML Recommanded Tags for Documentation Comments.
    /// http://msdn.microsoft.com/en-us/library/5ast78ax.aspx
    /// </summary>
    public static class DocTag
    {
        /// <summary>
        /// SharpDX unamanged tag &lt;unmanaged&gt; tag.
        /// </summary>
        public const string UnManaged = "unmanaged";

        /// <summary>
        /// &lt;summary&gt; tag.
        /// </summary>
        public const string Summary = "summary";

        /// <summary>
        /// &lt;remarks&gt; tag.
        /// </summary>
        public const string Remarks = "remarks";

        /// <summary>
        /// &lt;value&gt; tag.
        /// </summary>
        public const string Value = "value";

        /// <summary>
        /// &lt;returns&gt; tag.
        /// </summary>
        public const string Returns = "returns";

        /// <summary>
        /// &lt;seealso&gt; tag.
        /// </summary>
        public const string SeeAlso = "seealso";

        /// <summary>
        /// &lt;see&gt; tag.
        /// </summary>
        public const string See = "see";

        /// <summary>
        /// &lt;example&gt; tag.
        /// </summary>
        public const string Example = "example";

        /// <summary>
        /// &lt;exception&gt; tag.
        /// </summary>
        public const string Exception = "exception";

        /// <summary>
        /// &lt;param&gt; tag.
        /// </summary>
        public const string Parameter = "param";

        /// <summary>
        /// &lt;permission&gt; tag.
        /// </summary>
        public const string Permission = "permission";
    }
}