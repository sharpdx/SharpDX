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

namespace SharpDX.IO
{
    /// <summary>
    /// Path utility methods.
    /// </summary>
    public class PathUtility
    {
        /// <summary>
        /// Transform a path by replacing '/' by '\' and transforming relative '..' or current path '.' to an absolute path. See remarks.
        /// </summary>
        /// <param name="path">A path string</param>
        /// <returns>A normalized path.</returns>
        /// <remarks>
        /// Unlike <see cref="System.IO.Path"/> , this doesn't make a path absolute to the actual file system.
        /// </remarks>
        public static string GetNormalizedPath(string path)
        {
            // Make sure that all / are translated to \
            path = path.Replace('/', '\\');

            // Then process the path to normalize it (transform relative to absolute)
            var pathStrings = path.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            var pathList = new Stack<string>();
            foreach (var pathItem in pathStrings)
            {
                if (pathItem == ".")
                    continue;
                if (pathItem == "..")
                {
                    if (pathList.Count == 0)
                        throw new ArgumentException("Invalid path can't start with '..'");
                    pathList.Pop();
                }
                else
                {
                    pathList.Push(pathItem);
                }
            }

            var items = pathList.ToArray();

            Array.Reverse(items);

            return Utilities.Join(@"\", items);
        } 
    }
}