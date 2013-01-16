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
namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Location of a portion of source.
    /// </summary>
    internal struct SourceSpan
    {
        /// <summary>
        /// Path of the file.
        /// </summary>
        public string FilePath;

        /// <summary>
        /// Column of the span.
        /// </summary>
        public int Column;

        /// <summary>
        /// Line of the span.
        /// </summary>
        public int Line;

        /// <summary>
        /// Absolute index in the input string.
        /// </summary>
        public int Index;

        /// <summary>
        /// Length of the source span in the input string.
        /// </summary>
        public int Length;

        public override string ToString()
        {
            return string.Format("{0} ({1},{2})", FilePath, Line, Column);
        }
    }
}