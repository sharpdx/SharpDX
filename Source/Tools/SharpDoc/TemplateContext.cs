// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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
using System.Dynamic;
using System.IO;
using System.Text.RegularExpressions;
using RazorEngine;
using RazorEngine.Templating;
using SharpCore;
using SharpCore.Logging;
using SharpDoc.Model;

namespace SharpDoc
{
    /// <summary>
    /// Template context used by all templates.
    /// </summary>
    public class TemplateContext : ITemplateResolver
    {
        private const string StyleDirectory = "Styles";
        private List<TagExpandItem> _regexItems;
        private MsdnRegistry _msnRegistry;

        /// <summary>
        /// Provides a delegate to transform a link into a url
        /// </summary>
        public delegate string LinkResolverDelegate(LinkDescriptor link);

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateContext"/> class.
        /// </summary>
        public TemplateContext()
        {
            TemplateDirectories = new List<string>();
            OutputDirectory = "Output";
            _regexItems = new List<TagExpandItem>();
            Topics = new List<NTopic>();
            _msnRegistry = new MsdnRegistry();
            Param = new DynamicParam();
            Style = new DynamicParam();
        }

        /// <summary>
        /// Finds a <see cref="IModelReference"/> from an id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// A registered reference or null if it's an external or invalid reference
        /// </returns>
        private IModelReference FindLocalReference(string id)
        {
            if (CurrentContext == null)
                return Registry.FindById(id);

            return Registry.FindById(id, CurrentContext) ?? Registry.FindById(id);
        }


        /// <summary>
        /// Gets the param dynamic properties.
        /// </summary>
        /// <value>The param dynamic properties.</value>
        public dynamic Param { get; private set; }

        /// <summary>
        /// Gets the style dynamic properties.
        /// </summary>
        /// <value>The style dynamic properties.</value>
        public dynamic Style { get; private set; }

        /// <summary>
        /// Gets or sets the name of the class library.
        /// </summary>
        /// <value>The name of the class library.</value>
        public string ClassLibraryName { get; set; }

        /// <summary>
        /// Gets or sets the registry.
        /// </summary>
        /// <value>The registry.</value>
        public MemberRegistry Registry { get; set; }

        /// <summary>
        /// Gets or sets the topics.
        /// </summary>
        /// <value>The topics.</value>
        public List<NTopic> Topics { get; set; }

        /// <summary>
        /// Gets or sets the assemblies.
        /// </summary>
        /// <value>The assemblies.</value>
        public List<NAssembly> Assemblies { get; set; }

        /// <summary>
        /// Gets or sets the current topic.
        /// </summary>
        /// <value>The current topic.</value>
        public NTopic Topic { get; set; }

        /// <summary>
        /// Gets or sets the current assembly being processed.
        /// </summary>
        /// <value>The current assembly being processed.</value>
        public NAssembly Assembly { get; set; }

        /// <summary>
        /// Gets or sets the current namespace being processed.
        /// </summary>
        /// <value>The current namespace being processed.</value>
        public NNamespace Namespace { get; set; }

        /// <summary>
        /// Gets or sets the current type being processed.
        /// </summary>
        /// <value>The current type being processed.</value>
        public NType Type { get; set; }

        /// <summary>
        /// Gets or sets the current member being processed.
        /// </summary>
        /// <value>The current member.</value>
        public NMember Member { get; set; }

        /// <summary>
        /// Gets or sets the template directories.
        /// </summary>
        /// <value>The template directories.</value>
        public List<string> TemplateDirectories {get; private set;}

        /// <summary>
        /// Gets or sets the output directory.
        /// </summary>
        /// <value>The output directory.</value>
        public string OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets the link resolver.
        /// </summary>
        /// <value>The link resolver.</value>
        public LinkResolverDelegate LinkResolver { get; set; }

        /// <summary>
        /// Gets the current context.
        /// </summary>
        /// <value>The current context.</value>
        public IModelReference CurrentContext
        {
            get
            {
                if (Topic != null)
                    return Topic;
                if (Type != null)
                    return Type;
                if (Namespace != null)
                    return Namespace;
                if (Assembly != null)
                    return Assembly;
                return null;
            }
        }

        public string ToUrl(IModelReference reference, string linkName = null, bool forceLocal = false, string attributes = null)
        {
            return ToUrl(reference.Id, linkName, forceLocal, attributes);
        }

        /// <summary>
        /// Resolve a document Id (ie. "T:System.Object") to an url.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="linkName">Name of the link.</param>
        /// <param name="forceLocal">if set to <c>true</c> [force local].</param>
        /// <param name="attributes">The attributes.</param>
        /// <returns></returns>
        public string ToUrl(string id, string linkName = null, bool forceLocal = false, string attributes = null)
        {
            var linkDescriptor = new LinkDescriptor {Id = id, Type = LinkType.None, LocalReference = FindLocalReference(id)};
            if (linkDescriptor.LocalReference != null)
            {
                linkDescriptor.Type = LinkType.Local;
                // For local references, use short name
                linkDescriptor.Name = linkDescriptor.LocalReference.Name;
                if (!forceLocal && CurrentContext != null && linkDescriptor.LocalReference is NMember)
                {
                    var declaringType = ((NMember) linkDescriptor.LocalReference).DeclaringType;
                    // If link is self referencing the current context, then use a self link
                    if ( id == CurrentContext.Id || (declaringType != null && declaringType.Id == CurrentContext.Id))
                    {
                        linkDescriptor.Type = LinkType.Self;
                    }
                }

            } else
            {
                linkDescriptor.Location = _msnRegistry.FindUrl(id);
                var reference = TextReferenceUtility.CreateReference(id);                    
                if (linkDescriptor.Location != null)
                {                    
                    linkDescriptor.Type = LinkType.Msdn;
                    linkDescriptor.Name = reference.FullName;
                    // Open MSDN documentation to a new window
                    attributes = (attributes ?? "") + " target='_blank'";
                } else
                {
                    linkDescriptor.Type = LinkType.None;
                    linkDescriptor.Name = reference.FullName;                    
                }
            }
            if (LinkResolver == null)
            {
                Logger.Warning("Model.LinkResolver must be set");
                return id;
            }

            if (linkName != null)
                linkDescriptor.Name = linkName;

            linkDescriptor.Attributes = attributes;

            return LinkResolver(linkDescriptor);
        }

        /// <summary>
        /// Uses the style.
        /// </summary>
        /// <param name="styleName">Name of the style.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        public void UseStyle(string styleName, bool inherit)
        {
            string executablePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            string styleSubpath = StyleDirectory + "\\" + styleName;

            // Locate global styles
            InsertTemplateDirectory(Path.Combine(executablePath, "..\\..\\" + styleSubpath), inherit);

            string exePath = Path.Combine(executablePath, styleSubpath);
            // Locate local styles from exe directory
            InsertTemplateDirectory(exePath, inherit);

            string localPath = Path.Combine(Path.GetFullPath(Environment.CurrentDirectory), styleSubpath);
            // Locate local styles from current location
            if (localPath != exePath)
                InsertTemplateDirectory(localPath, inherit);
        }

        private void InsertTemplateDirectory(string path, bool inheritedTemplateDir)
        {
            int position = inheritedTemplateDir ? 0 : TemplateDirectories.Count;
            if (Directory.Exists(path))
                TemplateDirectories.Insert(position, path);
        }

        /// <summary>
        /// Resolves a path from template directories.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A filepath to the file or directory</returns>
        public string ResolvePath(string path)
        {
            for (int i = TemplateDirectories.Count - 1; i >= 0; i--)
            {
                string filePath = Path.Combine(TemplateDirectories[i], path);
                if (File.Exists(filePath))
                    return filePath;
            }
            return null;
        }

        /// <summary>
        /// Resolves and load a file from template directories.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>The content of the file</returns>
        public string Loadfile(string file)
        {
            string filePath = ResolvePath(file);
            if (filePath == null) {
                Logger.Fatal("Cannot find file [{0}] from the following Template Directories [{1}]", file, string.Join(",", TemplateDirectories));
                // Fatal is stopping the process
                return "";
            }
            return File.ReadAllText(filePath);
        }

        /// <summary>
        /// Gets the template.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public string GetTemplate(string name, out string location)
        {
            location = ResolvePath(name + ".cshtml");
            if (location == null)
                location = ResolvePath(Path.Combine("html", name + ".cshtml"));

            if (location == null)
            {
                Logger.Fatal("Cannot find template [{0}] from the following Template Directories [{1}]", name, string.Join(",", TemplateDirectories));
                // Fatal is stopping the process
                return "";
            }
            return File.ReadAllText(location);
        }

        /// <summary>
        /// Copies the content of the directory from all template directories.
        /// using inheritance directory.
        /// </summary>
        /// <param name="directoryName">Name of the directory.</param>
        public void CopyDirectoryContent(string directoryName)
        {
            foreach (var templateDirectory in TemplateDirectories)
            {
                string filePath = Path.Combine(templateDirectory, directoryName);
                if (Directory.Exists(filePath))
                    CopyDirectory(filePath, Path.Combine(Path.GetFullPath(OutputDirectory), directoryName));
            }
        }

        private void CopyDirectory(string from, string to)
        {
            // string source, destination; - folder paths 
            int pathLen = from.Length + 1;

            foreach (string filePath in Directory.GetFiles(from, "*.*", SearchOption.AllDirectories))
            {
                string subPath = filePath.Substring(pathLen);
                string newpath = Path.Combine(to, subPath);
                string dirPath = Path.GetDirectoryName(newpath);
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);
                File.Copy(filePath, newpath);
            }
        }

        /// <summary>
        /// Removes the output directory.
        /// </summary>
        internal void RemoveOutputDirectory()
        {
            try
            {
                Directory.Delete(Path.GetFullPath(OutputDirectory), true);
            } catch (Exception)
            {
                Logger.Warning("Unable to remove output directory [{0}]", OutputDirectory);
            }
        }

        /// <summary>
        /// Write a content to an output file.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="content">The content.</param>
        public void WriteToFile(string name, string content)
        {
            string path = Path.Combine(Path.GetFullPath(OutputDirectory), name);

            string dirPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            File.WriteAllText(path, content);
        }

        // private List<>

        private class TagExpandItem
        {
            public TagExpandItem(Regex expression, string replaceString)
            {
                Expression = expression;
                ReplaceString = replaceString;
            }

            public TagExpandItem(Regex expression, MatchEvaluator replaceEvaluator)
            {
                Expression = expression;
                ReplaceEvaluator = replaceEvaluator;
            }

            Regex Expression;
            string ReplaceString;
            MatchEvaluator ReplaceEvaluator;

            public string Replace(string content)
            {
                if (content == null)
                    return null;
                if (ReplaceString != null)
                    return Expression.Replace(content, ReplaceString);
                return Expression.Replace(content, ReplaceEvaluator);
            }
        }



        /// <summary>
        /// Add regular expression for TagExpand function.
        /// </summary>
        /// <param name="regexp">The regexp.</param>
        /// <param name="substitution">The substitution.</param>
        public void RegisterTagResolver(string regexp, string substitution)
        {
           _regexItems.Add( new TagExpandItem(new Regex(regexp), substitution));
        }

        /// <summary>
        /// Add regular expression for RegexExpand function.
        /// </summary>
        /// <param name="regexp">The regexp.</param>
        /// <param name="evaluator">The evaluator.</param>
        public void RegisterTagResolver(string regexp, MatchEvaluator evaluator)
        {
            _regexItems.Add(new TagExpandItem(new Regex(regexp), evaluator));
        }

        /// <summary>
        /// Perform regular expression expansion.
        /// </summary>
        /// <param name="content">The content to replace.</param>
        /// <returns>The content replaced</returns>
        public string TagExpand(string content)
        {
            foreach (var regexItem in _regexItems)
            {
                content = regexItem.Replace(content);
            }
            return content;
        }

        /// <summary>
        /// Parses the specified template name.
        /// </summary>
        /// <param name="templateName">Name of the template.</param>
        /// <returns></returns>
        public string Parse(string templateName)
        {
            string location = null;
            try
            {
                string template = GetTemplate(templateName, out location);
                return Razor.Parse(template, this, templateName, location);
            }
            catch (TemplateCompilationException ex)
            {
                foreach (var compilerError in ex.Errors)
                {
                    Logger.PushLocation(location, compilerError.Line, compilerError.Column);
                    if (compilerError.IsWarning)
                    {
                        Logger.Warning("{0}: {1}", compilerError.ErrorNumber, compilerError.ErrorText);
                    } else
                    {
                        Logger.Error("{0}: {1}", compilerError.ErrorNumber, compilerError.ErrorText);
                    }
                    Logger.PopLocation();
                }
                Logger.PopLocation();
                Logger.Fatal("Error when compiling template [{0}]", templateName);
            }
            return "";
        }
    }
}