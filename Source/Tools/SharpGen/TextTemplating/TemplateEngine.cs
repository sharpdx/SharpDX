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
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using SharpGen.Logging;

namespace SharpGen.TextTemplating
{
    /// <summary>
    /// Lightweight implementation of T4 engine, using Tokenizer from MonoDevelop.TextTemplating.
    /// </summary>
    public class TemplateEngine
    {
        private StringBuilder _doTemplateCode;
        private StringBuilder _doTemplateClassCode;
        private bool _isTemplateClassCode;
        private List<Directive> _directives;
        private Dictionary<string, ParameterValueType> _parameters;

        /// <summary>
        /// Occurs when an include needs to be found.
        /// </summary>
        public event EventHandler<TemplateIncludeArgs> OnInclude;


        /// <summary>
        /// Template code.
        /// Parameter {0} = List of import namespaces.
        /// Parameter {1} = List of parameters declaration.
        /// Parameter {2} = Body of template code
        /// Parameter {3} = Body of template class level code
        /// </summary>
        private const string GenericTemplateCodeText = @"
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
{0}
    public class TemplateImpl : SharpGen.TextTemplating.Templatizer
    {{
{1}
        public override void Process()
        {{
            // DoTemplate code
{2}
        }}

        // DoTemplateClass code
{3}
    }}
";
        public TemplateEngine()
        {
            _parameters = new Dictionary<string, ParameterValueType>();
            // templateCodePath = Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "..\\..\\");
        }


        /// <summary>
        /// Adds a text to Templatizer.Process section .
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="content">The content.</param>
        private void AddDoTemplateCode(Location location, string content)
        {
            if (_isTemplateClassCode)
                throw new InvalidOperationException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Cannot add Process code [{0}] after Process Class level code", content));

            _doTemplateCode.AppendLine().AppendFormat("#line {0} \"{1}\"", location.Line, location.FileName).AppendLine();
            _doTemplateCode.Append(content);
        }

        /// <summary>
        /// Adds a text to at Templatizer class level.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="content">The content.</param>
        private void AddDoTemplateClassCode(Location location, string content)
        {
            _doTemplateClassCode.AppendLine().AppendFormat("#line {0} \"{1}\"", location.Line, location.FileName).AppendLine();
            _doTemplateClassCode.Append(content);
        }

        /// <summary>
        /// Adds some code to the current 
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="code">The code.</param>
        private void AddCode(Location location, string code)
        {
            if (_isTemplateClassCode)
                AddDoTemplateClassCode(location, code);
            else
                AddDoTemplateCode(location, code);
        }

        /// <summary>
        /// Add a multiline string to the template code.
        /// This methods decompose a string in lines and generate template code for each line
        /// using Templatizer.Write/WriteLine methods.
        /// </summary>
        /// <param name="location">The location in the text template file.</param>
        /// <param name="content">The content to add to the code.</param>
        private void AddContent(Location location, string content)        
        {
            content = content.Replace("\"", "\\\"");

            var reader = new StringReader(content);
            var lines = new List<string>();
            string line;
            while ((line = reader.ReadLine()) != null)
                lines.Add(line);

            Location fromLocation = location;

            for (int i = 0; i < lines.Count;i++)
            {
                line = lines[i];

                location = new Location(fromLocation.FileName, fromLocation.Line + i, fromLocation.Column);

                if ((i + 1) == lines.Count)
                {
                    if (content.EndsWith(line))
                    {
                        if (!string.IsNullOrEmpty(line))
                            AddCode(location, "Write(\"" + line + "\");\n");
                    }
                    else
                        AddCode(location, "WriteLine(\"" + line + "\");\n");
                }
                else
                {
                    AddCode(location, "WriteLine(\"" + line + "\");\n");
                }
            }
        }

        private void AddExpression(Location location, string content)
        {
            AddCode(location, "Write(" + content + ");\n");
        }

        /// <summary>
        /// Gets or sets the name of the template file being processed (for debug purpose).
        /// </summary>
        /// <value>The name of the template file.</value>
        public string TemplateFileName { get; set; }

        /// <summary>
        /// Process a template and returns the processed file.
        /// </summary>
        /// <param name="templateText">The template text.</param>
        /// <returns></returns>
        public string ProcessTemplate(string templateText)
        {
            // Initialize TemplateEngine state
            _doTemplateCode = new StringBuilder();
            _doTemplateClassCode = new StringBuilder();
            _isTemplateClassCode = false;
            Assembly templateAssembly = null;
            _directives = new List<Directive>();

            // Parse the T4 template text
            Parse(templateText);

            // Build parameters for template
            var parametersCode = new StringBuilder();
            foreach (var parameterValueType in _parameters.Values)
                parametersCode.Append(string.Format(System.Globalization.CultureInfo.InvariantCulture, "public {0} {1} {{ get; set; }}\n", parameterValueType.Type.FullName, parameterValueType.Name));

            // Build import namespaces for template
            var importNamespaceCode = new StringBuilder();
            foreach (var directive in _directives)
            {
                if (directive.Name == "import")
                    importNamespaceCode.Append("using " + directive.Attributes["namespace"] + ";\n");
            }

            // Expand final template class code
            // Parameter {0} = List of import namespaces.
            // Parameter {1} = List of parameters declaration.
            // Parameter {2} = Body of template code
            // Parameter {3} = Body of template class level code
            string templateSourceCode = string.Format(GenericTemplateCodeText, importNamespaceCode, parametersCode, _doTemplateCode, _doTemplateClassCode);

            // Creates the C# compiler, compiling for 3.5
            var codeProvider = new Microsoft.CSharp.CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } });
            var compilerParameters = new CompilerParameters { GenerateInMemory = true, GenerateExecutable = false };

            // Adds assembly from CurrentDomain
            // TODO, implement T4 directive "assembly"?
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    string location = assembly.Location;
                    if (!String.IsNullOrEmpty(location))
                        compilerParameters.ReferencedAssemblies.Add(location);
                }
                catch (NotSupportedException)
                {
                    // avoid problem with previous dynamic assemblies
                }
            }

            // Compiles the code
            var compilerResults = codeProvider.CompileAssemblyFromSource(compilerParameters, templateSourceCode);

            // Output any errors
            foreach (var compilerError in compilerResults.Errors)
                Logger.Error(compilerError.ToString());

            // If successful, gets the compiled assembly
            if (compilerResults.Errors.Count == 0 && compilerResults.CompiledAssembly != null)
            {
                templateAssembly = compilerResults.CompiledAssembly;
            }
            else
            {
                Logger.Fatal("Template [{0}] contains error", TemplateFileName);
            }

            // Get a new templatizer instance
            var templatizer = (Templatizer)Activator.CreateInstance(templateAssembly.GetType("TemplateImpl"));

            // Set all parameters for the template
            foreach (var parameterValueType in _parameters.Values)
            {
                var propertyInfo = templatizer.GetType().GetProperty(parameterValueType.Name);
                propertyInfo.SetValue(templatizer, parameterValueType.Value, null);
            }

            // Run the templatizer
            templatizer.Process();

            // Returns the text
            return templatizer.ToString();
        }

        /// <summary>
        /// Adds a parameter with the object to this template. 
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">an object.</param>
        /// <exception cref="ArgumentNullException">If value is null</exception>
        public void SetParameter(string name, object value)
        {
            if (value == null) throw new ArgumentNullException("value");

            SetParameter(name, value, value.GetType());
        }

        /// <summary>
        /// Adds a parameter with the object to this template. 
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">an object.</param>
        /// <param name="typeOf">Type of the object.</param>
        /// <exception cref="ArgumentNullException">If typeOf is null</exception>
        public void SetParameter(string name, object value, Type typeOf)
        {
            if (typeOf == null) throw new ArgumentNullException("typeOf");

            if (_parameters.ContainsKey(name))
                _parameters.Remove(name);

            _parameters.Add(name, new ParameterValueType { Name = name, Type = typeOf, Value = value });
        }

        /// <summary>
        /// An association between a Value and a Type.
        /// </summary>
        private class ParameterValueType
        {
            public string Name;
            public Type Type;
            public object Value;
        }

        /// <summary>
        /// Parses the specified template text.
        /// </summary>
        /// <param name="templateText">The template text.</param>
        private void Parse(string templateText)
        {
            //var filePath = Path.GetFullPath(Path.Combine(templateCodePath, TemplateFileName));
            var tokeniser = new Tokeniser(TemplateFileName, templateText);

            AddCode(tokeniser.Location, "");

            bool skip = false;
            while ((skip || tokeniser.Advance()) && tokeniser.State != State.EOF)
            {
                skip = false;
                switch (tokeniser.State)
                {
                    case State.Block:
                        if (!String.IsNullOrEmpty(tokeniser.Value))
                            AddDoTemplateCode(tokeniser.Location, tokeniser.Value);
                        break;
                    case State.Content:
                        if (!String.IsNullOrEmpty(tokeniser.Value))
                            AddContent(tokeniser.Location, tokeniser.Value);
                        break;
                    case State.Expression:
                        if (!String.IsNullOrEmpty(tokeniser.Value))
                            AddExpression(tokeniser.Location, tokeniser.Value);
                        break;
                    case State.Helper:
                        _isTemplateClassCode = true;
                        if (!String.IsNullOrEmpty(tokeniser.Value))
                            AddDoTemplateClassCode(tokeniser.Location, tokeniser.Value);
                        break;
                    case State.Directive:
                        Directive directive = null;
                        string attName = null;
                        while (!skip && tokeniser.Advance())
                        {
                            switch (tokeniser.State)
                            {
                                case State.DirectiveName:
                                    if (directive == null)
                                        directive = new Directive {Name = tokeniser.Value.ToLower()};
                                    else
                                        attName = tokeniser.Value;
                                    break;
                                case State.DirectiveValue:
                                    if (attName != null && directive != null)
                                        directive.Attributes.Add(attName.ToLower(), tokeniser.Value);
                                    attName = null;
                                    break;
                                case State.Directive:
                                    //if (directive != null)
                                    //    directive.EndLocation = tokeniser.TagEndLocation;
                                    break;
                                default:
                                    skip = true;
                                    break;
                            }
                        }
                        if (directive != null)
                        {
                            if (directive.Name == "include")
                            {
                                string includeFile = directive.Attributes["file"];
                                if (OnInclude == null)
                                    throw new InvalidOperationException("Include file found. OnInclude event must be implemented");
                                var includeArgs = new TemplateIncludeArgs() {IncludeName = includeFile};
                                OnInclude(this, includeArgs);
                                Parse(includeArgs.Text ?? "");
                            }
                            _directives.Add(directive);
                        }
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        /// T4 Directive
        /// </summary>
        private class Directive
        {
            public Directive()
            {
                Attributes = new Dictionary<string, string>();
            }

            public string Name { get; set; }
            public Dictionary<string, string> Attributes { get; set; }
        }
    }
}