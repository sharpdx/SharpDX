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
using SharpCore.Logging;

namespace SharpDoc
{
    /// <summary>
    /// Handles Styles directories.
    /// </summary>
    public class StyleManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StyleManager"/> class.
        /// </summary>
        public StyleManager()
        {
            StyleDirectories = new List<string>();
        }

        /// <summary>
        /// Gets or sets styles override.
        /// </summary>
        /// <value>The styles override.</value>
        private List<string> StyleDirectories { get; set; }

        /// <summary>
        /// Style directory name
        /// </summary>
        public const string DefaultStyleDirectoryName = "Styles";

        /// <summary>
        /// Gets the available styles.
        /// </summary>
        /// <value>The available styles.</value>
        public List<StyleDefinition> AvailableStyles
        {
             get
             {
                 var styles = new List<StyleDefinition>();

                 var styleDirectories = new List<string>(StyleDirectories);
                 styleDirectories.Reverse();
                 foreach (var styleDirectory in styleDirectories)
                 {
                     var dirInfo = new DirectoryInfo(styleDirectory);
                     foreach (var subStylePath in dirInfo.GetDirectories())
                     {
                         var styleFilename = Path.Combine(subStylePath.FullName, StyleDefinition.DefaultStyleFilename);

                         // If no definition file was found continue next entry
                         if (!File.Exists(styleFilename))
                             continue;
                         var style = StyleDefinition.Load(styleFilename);

                         // Check that style name matches the directory name
                         if (style.Name != subStylePath.Name)
                         {
                             Logger.Error("Style [{0}] from path [{1}] doesn't match directory name [{2}]", style.Name, style.FilePath, subStylePath.Name);
                             continue;
                         }                                                         
                        styles.Add(style);
                     }
                 }

                 // Verify integrity of styles
                 foreach (var style in styles)
                 {
                     // If the style inherits from another style, check that inherited style is already registered
                     if (!string.IsNullOrEmpty(style.BaseStyle) && !styles.Exists((match) => match.Name == style.BaseStyle))
                     {
                         Logger.Error("Base Style [{0}] for Style [{1}] from path [{2}] doesn't not exist", style.BaseStyle, style.Name, style.FilePath);
                         continue;
                     }

                     // TODO Check that the style is runnable (either by an inherited style)
                 }
                 styles.Reverse();
                 return styles;
             }
        }

        /// <summary>
        /// Adds a list of style paths.
        /// </summary>
        /// <param name="pathList">The style path list.</param>
        public void AddPath(IEnumerable<string> pathList)
        {
            foreach (var path in pathList)
                AddPath(path);
        }

        /// <summary>
        /// Adds the path to a style directory.
        /// </summary>
        /// <param name="path">The path.</param>
        public void AddPath(string path)
        {
            path = Path.GetFullPath(path);
            if (Directory.Exists(path) && !StyleDirectories.Contains(path))
                StyleDirectories.Insert(0, path);
        }

        /// <summary>
        /// Initialize this instance from the specified config.
        /// </summary>
        /// <param name="config">The config.</param>
        public void Init(Config config)
        {
            var exePath = Path.GetDirectoryName(typeof(Config).Assembly.Location);
            // Locate global styles
            var dirInfo = new DirectoryInfo(exePath);
            if (dirInfo.Name.ToLower() == "debug" || dirInfo.Name.ToLower() == "release")
                AddPath(Path.Combine(exePath, "..\\..\\" + DefaultStyleDirectoryName));
            else if (dirInfo.Name.ToLower() == "bin")
                AddPath(Path.Combine(exePath, "..\\" + DefaultStyleDirectoryName));

            // Add Style directory from sharpdoc executable path
            AddPath(Path.Combine(exePath, DefaultStyleDirectoryName));

            // Add from current directory
            AddPath(Path.Combine(Environment.CurrentDirectory, DefaultStyleDirectoryName));

            // Add from current path of config
            if (!string.IsNullOrEmpty(config.FilePath))
                AddPath(Path.Combine(Path.GetDirectoryName(config.FilePath), DefaultStyleDirectoryName));

            // Add path declared from config file
            AddPath(config.StyleDirectories);
        }

        /// <summary>
        /// Determines whether a style is declared and runnable with the specified style name.
        /// </summary>
        /// <param name="styleName">Name of the style.</param>
        /// <returns>
        /// 	<c>true</c> if a style is declared and runnable with the specified style name; otherwise, <c>false</c>.
        /// </returns>
        public bool StyleExist(string styleName)
        {
            foreach(var style in AvailableStyles)
            {
                if (style.Name == styleName)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Writes the available styles.
        /// </summary>
        public void WriteAvailaibleStyles(TextWriter writer)
        {
            writer.WriteLine();
            writer.WriteLine("Styles available:");
            foreach (var availableStyle in AvailableStyles)
            {
                writer.Write("\t");
                writer.WriteLine(availableStyle);
            }
        }
    }
}