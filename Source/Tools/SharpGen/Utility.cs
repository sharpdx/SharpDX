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
using System.IO;
using System.Text.RegularExpressions;

using Microsoft.Win32;

namespace SharpGen
{
    public class Utility
    {
        /// <summary>
        /// Escapes the xml/html text in order to use it inside xml.
        /// </summary>
        /// <param name="stringToEscape">The string to escape.</param>
        /// <returns></returns>
        public static string EscapeHtml(string stringToEscape)
        {
            return stringToEscape.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
        }

        /// <summary>
        /// Regular expression to match invalid chars in filename
        /// </summary>
        private static readonly Regex RegexInvalidPathChars = new Regex("[" + Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars())) + "]");

        /// <summary>
        /// Determines whether the specified file name is a valid filename.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>
        /// 	<c>true</c> if the specified file name is a valid filename; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidFilename(string fileName)
        {
            return !RegexInvalidPathChars.IsMatch(fileName);
        }

        public static string GetProperDirectoryCapitalization(DirectoryInfo dirInfo)
        {
            // Code from http://stackoverflow.com/questions/478826/c-sharp-filepath-recasing
            var parentDirInfo = dirInfo.Parent;
            if (null == parentDirInfo)
                return dirInfo.Name;
            return Path.Combine(GetProperDirectoryCapitalization(parentDirInfo),
                                parentDirInfo.GetDirectories(dirInfo.Name)[0].Name);
        }

        public static string GetProperFilePathCapitalization(string filename)
        {
            // Code from http://stackoverflow.com/questions/478826/c-sharp-filepath-recasing
            var fileInfo = new FileInfo(filename);
            DirectoryInfo dirInfo = fileInfo.Directory;
            return Path.Combine(GetProperDirectoryCapitalization(dirInfo),
                                dirInfo.GetFiles(fileInfo.Name)[0].Name);
        }

        private static string frameworkRootDirectory = null;

        public static string GetFrameworkRootDirectory()
        {
            if (frameworkRootDirectory == null)
            {

                // Retrieve the install root path for the framework
                frameworkRootDirectory = Path.GetFullPath(Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\.NetFramework", false).GetValue("InstallRoot").ToString());
            }
            return frameworkRootDirectory;
        }
    }
}