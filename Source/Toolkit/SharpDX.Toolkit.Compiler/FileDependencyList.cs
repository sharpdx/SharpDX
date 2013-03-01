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
using System.Reflection;
using System.Text.RegularExpressions;

using SharpDX.IO;

namespace SharpDX.Toolkit.Graphics
{
    public class FileDependencyList : Dictionary<string, DateTime>
    {
        private static Regex MatchLine = new Regex(@"^(.*)\s(\d+)$");

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDependencyList"/> class.
        /// </summary>
        public FileDependencyList()
        {
        }

        public void AddDefaultDependencies()
        {
            // Add reference to this assembly
            AddDependencyPath(typeof(FileDependencyList).Assembly.Location);

            // Add reference to SharpDX.Toolkit Assembly
            AddDependencyPath(typeof(SpriteFontData).Assembly.Location);
        }

        public static FileDependencyList FromReader(TextReader textReader)
        {
            var effectDependency = new FileDependencyList();
            string line;
            while ((line = textReader.ReadLine()) != null)
            {

                var match = MatchLine.Match(line);
                if (match.Success)
                {
                    effectDependency.Add(match.Groups[1].Value, new DateTime(long.Parse(match.Groups[2].Value)));
                }
            }
            return effectDependency;
        }

        public static FileDependencyList FromStream(Stream textStream)
        {
            var reader = new StreamReader(textStream);
            return FromReader(reader);
        }

        public void AddDependencyPath(string filePath)
        {
            if (!ContainsKey(filePath))
                Add(filePath, NativeFile.GetLastWriteTime(filePath));
        }

        public static FileDependencyList FromFile(string file)
        {
            using (var stream = new NativeFileStream(file, NativeFileMode.Open, NativeFileAccess.Read, NativeFileShare.ReadWrite)) return FromStream(stream);
        }

        public static string GetDependencyFileNameFromSourcePath(string pathToFxFile)
        {
            pathToFxFile = pathToFxFile.Replace("\\", "___");
            pathToFxFile += ".deps";
            return pathToFxFile;
        }

        public static List<string> FromFileRaw(string dependencyFilePath)
        {
            // If the file does not exist, than return true as it is a new dependency to generate
            if (!File.Exists(dependencyFilePath))
            {
                return new List<string>();
            }
            return new List<string>(FromFile(dependencyFilePath).Keys);
        }

        public static bool CheckForChanges(string dependencyFilePath)
        {
            // If the file does not exist, than return true as it is a new dependency to generate
            if (!File.Exists(dependencyFilePath))
            {
                return true;
            }

            return FromFile(dependencyFilePath).CheckForChanges();
        }

        public void Save(TextWriter writer)
        {
            foreach (var value in this)
            {
                writer.WriteLine("{0} {1}", value.Key, value.Value.Ticks);
            }
            writer.Flush();
        }

        public void Save(Stream stream)
        {
            var writer = new StreamWriter(stream);
            Save(writer);            
        }

        public void Save(string file)
        {
            var dirPath = Path.GetDirectoryName(file);
            // If output directory doesn't exist, we can create it
            if (dirPath != null && !Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            using (var stream = new NativeFileStream(file, NativeFileMode.Create, NativeFileAccess.Write, NativeFileShare.ReadWrite)) Save(stream);
        }

        /// <summary>
        /// Checks for changes in the dependency file.
        /// </summary>
        /// <returns><c>true</c> if a file has been updated, <c>false</c> otherwise</returns>
        public bool CheckForChanges()
        {
            // No files? Then it is considered as changed.
            if (Count == 0)
            {
                return true;
            }

            foreach (var fileItem in this)
            {
                if (!File.Exists(fileItem.Key))
                {
                    return true;
                }

                var fileTime = NativeFile.GetLastWriteTime(fileItem.Key);

                if (fileItem.Value != fileTime)
                {
                    return true;
                }
            }

            return false;
        }
    }
}