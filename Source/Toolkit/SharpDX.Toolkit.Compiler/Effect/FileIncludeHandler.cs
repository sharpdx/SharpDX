// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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

using SharpDX.D3DCompiler;

namespace SharpDX.Toolkit.Graphics
{
    internal class FileIncludeHandler : CallbackBase, Include
    {
        public readonly Stack<string> CurrentDirectory;

        public readonly Dictionary<string, FileItem> FileResolved;
        public readonly List<string> IncludeDirectories;
        public SourceSpan CurrentSpan;
        public IncludeFileDelegate IncludeFileCallback;
        public EffectCompilerLogger Logger;

        public FileIncludeHandler()
        {
            IncludeDirectories = new List<string>();
            CurrentDirectory = new Stack<string>();
            FileResolved = new Dictionary<string, FileItem>();
        }

        #region Include Members

        public Stream Open(IncludeType type, string fileName, Stream parentStream)
        {
            var currentDirectory = CurrentDirectory.Peek();
            if (currentDirectory == null)
                currentDirectory = Environment.CurrentDirectory;

            var filePath = fileName;

            if (!Path.IsPathRooted(filePath))
            {
                var directoryToSearch = new List<string> {currentDirectory};
                directoryToSearch.AddRange(IncludeDirectories);
                foreach (var dirPath in directoryToSearch)
                {
                    var selectedFile = Path.Combine(dirPath, fileName);
                    if (File.Exists(selectedFile))
                    {
                        filePath = selectedFile;
                        break;
                    }
                }
            }

            Stream stream = null;

            // Make sure that this file is not in the resolved list
            FileResolved.Remove(fileName);

            if (filePath == null || !File.Exists(filePath))
            {
                // Else try to use the include file callback
                if (IncludeFileCallback != null)
                {
                    stream = IncludeFileCallback(type == IncludeType.System, fileName);
                    if (stream != null)
                    {
                        FileResolved.Add(fileName, new FileItem(fileName, CleanPath(fileName), DateTime.Now));
                        return stream;
                    }
                }

                Logger.Error("Unable to find file [{0}]", CurrentSpan, filePath ?? fileName);
                return null;
            }

            stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            CurrentDirectory.Push(Path.GetDirectoryName(Path.GetFullPath(filePath)));
            var fullPath = Path.GetFullPath(filePath);
            FileResolved.Add(fileName, new FileItem(fileName, CleanPath(fullPath), File.GetLastWriteTime(fullPath)));
            return stream;
        }

        public void Close(Stream stream)
        {
            stream.Close();
            CurrentDirectory.Pop();
        }

        private static string CleanPath(string path)
        {
            return path.Replace(@"\\", @"\");
        }

        #endregion


        #region Nested type: IncludeHandler

        #endregion

        #region Nested type: Tuple

        internal class FileItem
        {
            public FileItem(string fileName, string filePath, DateTime modifiedTime)
            {
                FileName = fileName;
                FilePath = filePath;
                ModifiedTime = modifiedTime;
            }

            public string FileName;

            public string FilePath;

            public DateTime ModifiedTime;
        }

        #endregion
    }
}