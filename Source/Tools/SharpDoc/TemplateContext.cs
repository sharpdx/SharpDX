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
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using RazorEngine;
using RazorEngine.Compilation;
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
        private bool assembliesProcessed;
        private bool topicsProcessed;

        private ICompilerServiceFactory razorCompiler;
        private TemplateService razor;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateContext"/> class.
        /// </summary>
        public TemplateContext()
        {
            StyleDirectories = new List<string>();
            _regexItems = new List<TagExpandItem>();
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
        /// Gets or sets the style manager.
        /// </summary>
        /// <value>The style manager.</value>
        public StyleManager StyleManager { get; set; }

        /// <summary>
        /// Gets or sets the registry.
        /// </summary>
        /// <value>The registry.</value>
        public MemberRegistry Registry { get; set; }

        /// <summary>
        /// Gets or sets the topics.
        /// </summary>
        /// <value>The topics.</value>
        public NTopic RootTopic { get; set; }

        /// <summary>
        /// Gets or sets the search topic.
        /// </summary>
        /// <value>The search topic.</value>
        public NTopic SearchTopic { get; set;}

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
        /// Gets or sets the style directories.
        /// </summary>
        /// <value>The style directories.</value>
        private List<string> StyleDirectories {get; set;}

        /// <summary>
        /// Gets or sets the output directory.
        /// </summary>
        /// <value>The output directory.</value>
        public string OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets the link resolver.
        /// </summary>
        /// <value>The link resolver.</value>
        public Func<LinkDescriptor, string> LinkResolver { get; set; }

        /// <summary>
        /// Gets or sets the page id function.
        /// </summary>
        /// <value>
        /// The page id function.
        /// </value>
        public Func<IModelReference, string> PageIdFunction { get; set; }

        /// <summary>
        /// Gets or sets the write to function.
        /// </summary>
        /// <value>
        /// The write to function.
        /// </value>
        public Action<IModelReference, string> WriteTo { get; set; }

        /// <summary>
        /// Gets or sets the config.
        /// </summary>
        /// <value>
        /// The config.
        /// </value>
        public Config Config { get; set; }

        private ModelProcessor modelProcessor;
        private TopicBuilder topicBuilder;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            Config.FilePath = Config.FilePath ?? Path.Combine(Environment.CurrentDirectory, "unknown.xml");

            OutputDirectory = Config.AbsoluteOutputDirectory;

            // Set title
            Param.DocumentationTitle = Config.Title;

            // Add parameters
            if (Config.Parameters.Count > 0)
            {
                var dictionary = (DynamicParam)Param;
                foreach (var configParam in Config.Parameters)
                {
                    dictionary.Properties.Remove(configParam.Name);
                    dictionary.Properties.Add(configParam.Name, configParam.value);
                }
            }

            // Add styles
            if (Config.StyleParameters.Count > 0)
            {
                var dictionary = (IDictionary<string, object>)Style;
                foreach (var configParam in Config.StyleParameters)
                {
                    dictionary.Remove(configParam.Name);
                    dictionary.Add(configParam.Name, configParam.value);
                }
            }
        }

        /// <summary>
        /// Processes the assemblies.
        /// </summary>
        public void ProcessAssemblies()
        {
            if (!assembliesProcessed)
            {
                // Process the assemblies
                modelProcessor = new ModelProcessor { AssemblyManager = new MonoCecilAssemblyManager(), ModelBuilder = new MonoCecilModelBuilder(), PageIdFunction = PageIdFunction };
                modelProcessor.Run(Config);

                if (Logger.HasErrors)
                    Logger.Fatal("Too many errors in config file. Check previous message.");

                Assemblies = new List<NAssembly>(modelProcessor.Assemblies);
                Registry = modelProcessor.Registry;

                assembliesProcessed = true;
            }
        }


        /// <summary>
        /// Processes the topics.
        /// </summary>
        public void ProcessTopics()
        {
            if (!topicsProcessed)
            {
                // Build the topics
                topicBuilder = new TopicBuilder() { Assemblies = modelProcessor.Assemblies, Registry = modelProcessor.Registry };
                topicBuilder.Run(Config, PageIdFunction);

                if (Logger.HasErrors)
                    Logger.Fatal("Too many errors in config file. Check previous message.");

                RootTopic = topicBuilder.RootTopic;
                SearchTopic = topicBuilder.SearchTopic;

                topicsProcessed = true;
            }
        }

        /// <summary>
        /// Finds the topic by id.
        /// </summary>
        /// <param name="topicId">The topic id.</param>
        /// <returns></returns>
        public NTopic FindTopicById(string topicId)
        {
            if (RootTopic == null)
                return null;
            return RootTopic.FindTopicById(topicId);
        }

        /// <summary>
        /// Gets the current context.
        /// </summary>
        /// <value>The current context.</value>
        public IModelReference CurrentContext
        {
            get
            {
                if (Type != null)
                    return Type;
                if (Namespace != null)
                    return Namespace;
                if (Assembly != null)
                    return Assembly;
                if (Topic != null)
                    return Topic;
                return null;
            }
        }

        /// <summary>
        /// Resolve a local element Id (ie. "T:System.Object") to an url.
        /// </summary>
        /// <param name="reference">The reference.</param>
        /// <param name="linkName">Name of the link.</param>
        /// <param name="forceLocal">if set to <c>true</c> [force local].</param>
        /// <param name="attributes">The attributes.</param>
        /// <param name="useSelf">if set to <c>true</c> [use self when possible].</param>
        /// <returns></returns>
        public string ToUrl(IModelReference reference, string linkName = null, bool forceLocal = false, string attributes = null, bool useSelf = true)
        {
            return ToUrl(reference.Id, linkName, forceLocal, attributes, reference, useSelf);
        }

        /// <summary>
        /// Resolve a document Id (ie. "T:System.Object") to an url.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="linkName">Name of the link.</param>
        /// <param name="forceLocal">if set to <c>true</c> [force local].</param>
        /// <param name="attributes">The attributes.</param>
        /// <param name="localReference">The local reference.</param>
        /// <param name="useSelf"></param>
        /// <returns></returns>
        public string ToUrl(string id, string linkName = null, bool forceLocal = false, string attributes = null, IModelReference localReference = null, bool useSelf = true)
        {
            var linkDescriptor = new LinkDescriptor { Type = LinkType.None, Index = -1 };
            var typeReference = localReference as NTypeReference;
            NTypeReference genericInstance = null;
            bool isGenericInstance = typeReference != null && typeReference.IsGenericInstance;
            bool isGenericParameter = typeReference != null && typeReference.IsGenericParameter;

            if (isGenericInstance)
            {
                var elementType = typeReference.ElementType;
                id = elementType.Id;
                genericInstance = typeReference;
                linkName = elementType.Name.Substring(0, elementType.Name.IndexOf('<'));
            }

            linkDescriptor.LocalReference = FindLocalReference(id);

            if (isGenericParameter)
            {
                linkDescriptor.Name = typeReference.Name;
            } else if (linkDescriptor.LocalReference != null)
            {
                // For local references, use short name
                linkDescriptor.Name = linkDescriptor.LocalReference.Name;
                linkDescriptor.PageId = linkDescriptor.LocalReference.PageId;
                linkDescriptor.Type = LinkType.Local;
                linkDescriptor.Index = linkDescriptor.LocalReference.Index;
                if (!forceLocal && CurrentContext != null && linkDescriptor.LocalReference is NMember)
                {
                    var declaringType = ((NMember) linkDescriptor.LocalReference).DeclaringType;
                    // If link is self referencing the current context, then use a self link
                    if ((id == CurrentContext.Id || (declaringType != null && declaringType.Id == CurrentContext.Id)) && useSelf)
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
                    linkDescriptor.Name = reference.Name;                    
                }
            }

            if (LinkResolver == null)
            {
                Logger.Warning("Model.LinkResolver must be set");
                return id;
            }

            if (linkName != null)
                linkDescriptor.Name = linkName;

            linkDescriptor.Id = id;
            linkDescriptor.Attributes = attributes;

            var urlBuilder = new StringBuilder();
            urlBuilder.Append(LinkResolver(linkDescriptor));

            // Handle url for generic instance
            if (genericInstance != null)
            {
                urlBuilder.Append("&lt;");
                for (int i = 0; i < genericInstance.GenericArguments.Count; i++)
                {
                    if (i > 0)
                        urlBuilder.Append(", ");
                    var genericArgument = genericInstance.GenericArguments[i];
                    // Recursive call here
                    urlBuilder.Append(ToUrl(genericArgument));
                }
                urlBuilder.Append("&gt;");
            }
            return urlBuilder.ToString();
        }

        /// <summary>
        /// Uses the style.
        /// </summary>
        /// <param name="styleName">Name of the style.</param>
        /// <returns></returns>
        internal void UseStyle(string styleName)
        {
            razorCompiler = new DefaultCompilerServiceFactory();
            razor = new TemplateService(razorCompiler.CreateCompilerService(Language.CSharp, false, null), null);
            razor.SetTemplateBase(typeof(TemplateHelperBase));
            razor.AddResolver(this);

            if (!StyleManager.StyleExist(styleName))
                Logger.Fatal("Cannot us style [{0}]. Style doesn't exist", styleName);

            StyleDirectories.Clear();

            var includeBaseStyle = new List<string>();

            var styles = StyleManager.AvailableStyles;
            includeBaseStyle.Add(styleName);

            bool isNotComplete = true;

            // Compute directories to look, by following inheritance
            // In the same order they are declared
            while (isNotComplete)
            {
                isNotComplete = false;
                // Build directories to look for this specific style and all its base style);
                var toRemove = new List<StyleDefinition>();
                foreach (var style in styles)
                {
                    if (includeBaseStyle.Contains(style.Name))
                    {
                        toRemove.Add(style);
                        StyleDirectories.Add(style.DirectoryPath);
                        isNotComplete = true;
                        if (style.HasBaseStyle)
                            includeBaseStyle.Add(style.BaseStyle);
                    }
                }

                // Remove the style that was processed by the previous loop
                foreach (var styleDefinition in toRemove)
                    styles.Remove(styleDefinition);
            }
        }

        /// <summary>
        /// Resolves a path from template directories.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A filepath to the file or directory</returns>
        public string ResolvePath(string path)
        {
            for (int i = 0; i < StyleDirectories.Count; i++)
            {
                string filePath = Path.Combine(StyleDirectories[i], path);
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
                Logger.Fatal("Cannot find file [{0}] from the following Template Directories [{1}]", file, string.Join(",", StyleDirectories));
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
                Logger.Fatal("Cannot find template [{0}] from the following Template Directories [{1}]", name, string.Join(",", StyleDirectories));
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
            foreach (var templateDirectory in StyleDirectories)
            {
                string filePath = Path.Combine(templateDirectory, directoryName);
                if (Directory.Exists(filePath))
                    CopyDirectory(filePath, Path.Combine(OutputDirectory, directoryName));
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
                File.Copy(filePath, newpath, true);
            }
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
                return razor.Parse(template, this, templateName, location);
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
            } catch (Exception ex)
            {
                Logger.PushLocation(location);
                Logger.Error("Unexpected exceprion", ex);
                Logger.PopLocation();
                throw ex;
            }
            return "";
        }
    }
}