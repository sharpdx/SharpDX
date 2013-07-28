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

using System.IO;
using SharpDX.IO;

namespace SharpDX.Toolkit.Content
{
    /// <summary>
    /// This <see cref="IContentResolver" /> is loading an asset name from a root directory from a physical disk.
    /// </summary>
    public class FileSystemContentResolver : IContentResolver
    {
        private const string DefaultExtension = ".tkb";

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemContentResolver" /> class.
        /// </summary>
        /// <param name="rootDirectory">The root directory.</param>
        public FileSystemContentResolver(string rootDirectory)
        {
            RootDirectory = rootDirectory;
        }

        /// <summary>
        /// Gets the root directory from where assets will be loaded from the disk.
        /// </summary>
        /// <value>The root directory.</value>
        public string RootDirectory { get; private set; }

        public bool Exists(string assetName)
        {
            return NativeFile.Exists(GetAssetPath(assetName));
        }

        public Stream Resolve(string assetName)
        {
            try
            {
                return new NativeFileStream(GetAssetPath(assetName), NativeFileMode.Open, NativeFileAccess.Read);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the full asset path based on the root directory and default extension.
        /// </summary>
        /// <param name="assetName">The asset name.</param>
        /// <returns>The full asset path.</returns>
        protected string GetAssetPath(string assetName)
        {
            if (string.IsNullOrEmpty(Path.GetExtension(assetName)))
                assetName += DefaultExtension;

            return PathUtility.GetNormalizedPath(Path.Combine(RootDirectory ?? string.Empty, assetName));
        }
    }
}