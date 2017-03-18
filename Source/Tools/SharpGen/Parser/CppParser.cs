// Copyright (c) 2010 SharpDX - Alexandre Mutel
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
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using SharpGen;
using SharpGen.Logging;
using SharpGen.Config;
using SharpGen.CppModel;
using SharpGen.Doc;

namespace SharpGen.Parser
{
    static class Extension
    {
        /// <summary>
        /// Get the value from an attribute.
        /// </summary>
        /// <param name="xElement">The <see cref="XElement"/> object to get the attribute from.</param>
        /// <param name="name">The name of the attribute.</param>
        /// <returns></returns>
        public static string AttributeValue(this XElement xElement, string name)
        {
            var attr = xElement.Attribute(name);
            return (attr != null) ? attr.Value : null;
        }
    }

    /// <summary>
    /// Full C++ Parser built on top of <see cref="CastXml"/>.
    /// </summary>
    public class CppParser
    {
        private const string EndTagCustomEnumItem = "__sharpdx_enumitem__";
        private const string EndTagCustomVariable = "__sharpdx_var__";
        private const string Version = "1.0";
        private CppModule _group;
        private readonly Dictionary<string, bool> _includeToProcess = new Dictionary<string, bool>();
        private Dictionary<string, bool> _includeIsAttached = new Dictionary<string, bool>();
        private Dictionary<string, List<string>> _includeAttachedTypes = new Dictionary<string, List<string>>();
        private readonly Dictionary<string, string> _bindings = new Dictionary<string, string>();
        private CastXml _gccxml;
        private string _configRootHeader;
        private ConfigFile _configRoot;
        private CppInclude _currentCppInclude;
        private readonly Dictionary<string, string> _variableMacrosDefined = new Dictionary<string, string>();
        readonly Dictionary<string, XElement> _mapIdToXElement = new Dictionary<string, XElement>();
        readonly Dictionary<string, List<XElement>> _mapFileToXElement = new Dictionary<string, List<XElement>>();
        private readonly Dictionary<string, int> _mapIncludeToAnonymousEnumCount = new Dictionary<string, int>();
        readonly List<string> _filesWithCreateFromMacros = new List<string>();
        private bool _isConfigUpdated;

        /// <summary>
        /// Initializes a new instance of the <see cref="CppParser"/> class.
        /// </summary>
        public CppParser()
        {
            ForceParsing = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is generating doc.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is generating doc; otherwise, <c>false</c>.
        /// </value>
        public bool IsGeneratingDoc { get; set; }

        /// <summary>
        /// Gets or sets the doc provider assembly.
        /// </summary>
        /// <value>The doc provider assembly.</value>
        public string DocProviderAssembly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CppParser"/> should force to parse headers.
        /// </summary>
        /// <value><c>true</c> to force to run parsing regarding of config file updates check; otherwise, <c>false</c>.</value>
        public bool ForceParsing { get; set; }

        /// <summary>
        /// Gets or sets the CastXML executable path.
        /// </summary>
        /// <value>The CastXML executable path.</value>
        public string CastXmlExecutablePath { get; set; }

        /// <summary>
        /// Initialize this Parser from a root ConfigFile.
        /// </summary>
        /// <param name="configRoot">The root config file</param>
        public void Init(ConfigFile configRoot)
        {
            if (configRoot == null) throw new ArgumentNullException("configRoot");
            _configRoot = configRoot;

            // var configRoot = ConfigFile.Load(@"E:\code\sharpdx-v2\Source\Mapping.xml");
            _configRootHeader = _configRoot.Id + ".h";
            _gccxml = new CastXml {ExecutablePath = CastXmlExecutablePath};

            // Config is updated if ForceParsing is true
            _isConfigUpdated = ForceParsing;

            _group = new CppModule();

            // Get All include-directories to add to GccXmlApp
            var prolog = new StringBuilder();

            // Add current directory for gccxml
            _gccxml.IncludeDirectoryList.Add(new IncludeDirRule(Environment.CurrentDirectory));

            // Configure gccxml with include directory
            foreach (var configFile in _configRoot.ConfigFilesLoaded)
            {
                // Add all include directories
                foreach (var includeDir in configFile.IncludeDirs)
                    _gccxml.IncludeDirectoryList.Add(includeDir);

                // Append prolog
                foreach (var includeProlog in configFile.IncludeProlog)
                {
                    if (!string.IsNullOrEmpty(includeProlog))
                        prolog.Append(includeProlog);                    
                }

                // Prepare bindings
                // TODO test duplicate bindings
                foreach (var bindRule in configFile.Bindings)
                    _bindings.Add(bindRule.From, bindRule.To);
            }

            var filesWithIncludes = new List<string>();


            // Check if the file has any includes related config
            foreach (var configFile in _configRoot.ConfigFilesLoaded)
            {
                bool isWithInclude = false;

                // Add this config file as an include to process
                _includeToProcess.Add(configFile.Id, true);
                _includeIsAttached.Add(configFile.Id, true);

                if (configFile.IncludeDirs.Count > 0)
                    isWithInclude = true;

                // Build prolog
                if (configFile.IncludeProlog.Count > 0)
                    isWithInclude = true;

                if (configFile.Includes.Count > 0)
                    isWithInclude = true;

                if (configFile.References.Count > 0)
                    isWithInclude = true;

                // Check if any create from macro
                foreach (var typeBaseRule in configFile.Extension)
                {
                    if (CheckIfRuleIsCreatingHeadersExtension(typeBaseRule))
                    {
                        _filesWithCreateFromMacros.Add(configFile.Id);
                        isWithInclude = true;
                        break;
                    }
                }

                // If this config file has any include rules
                if (isWithInclude)
                    filesWithIncludes.Add(configFile.Id);
            }

            // Dump includes
            foreach (var configFile in _configRoot.ConfigFilesLoaded)
            {
                if (!filesWithIncludes.Contains(configFile.Id))
                    continue;

                var outputConfig = new StringWriter();
                outputConfig.WriteLine("// SharpDX include config [{0}] - Version {1}", configFile.Id, Version);

                if (_configRoot.Id == configFile.Id)
                    outputConfig.WriteLine(prolog);

                // Write includes
                foreach (var includeRule in configFile.Includes)
                {
                    var cppInclude = _group.FindInclude(includeRule.Id);
                    if (cppInclude == null)
                    {
                        _includeToProcess.Add(includeRule.Id, true);

                        cppInclude = new CppInclude() { Name = includeRule.Id };
                        _group.Add(cppInclude);
                    }

                    // Handle attach types
                    // Set that the include is attached (so that all types inside are attached
                    bool isIncludeFullyAttached = includeRule.Attach.HasValue && includeRule.Attach.Value;
                    if (isIncludeFullyAttached || includeRule.AttachTypes.Count > 0)
                    {
                        // An include can be fully attached ( include rule is set to true)
                        // or partially attached (the include rule contains Attach for specific types)
                        // We need to know which includes are attached, if they are fully or partially
                        if (!_includeIsAttached.ContainsKey(includeRule.Id))
                            _includeIsAttached.Add(includeRule.Id, isIncludeFullyAttached);
                        else if (isIncludeFullyAttached)
                        {
                            _includeIsAttached[includeRule.Id] = true;
                        }

                        // Attach types if any
                        if (includeRule.AttachTypes.Count > 0)
                        {
                            List<string> typesToAttach;
                            if (!_includeAttachedTypes.TryGetValue(includeRule.Id, out typesToAttach))
                            {
                                typesToAttach = new List<string>();
                                _includeAttachedTypes.Add(includeRule.Id, typesToAttach);
                            }

                            // For specific attach types, register them
                            foreach (var attachTypeName in includeRule.AttachTypes)
                            {
                                if (!typesToAttach.Contains(attachTypeName))
                                    typesToAttach.Add(attachTypeName);
                            }
                        }
                    }

                    // Add filtering errors
                    if (includeRule.FilterErrors.Count > 0 )
                    {
                        foreach (var filterError in includeRule.FilterErrors)
                            _gccxml.AddFilterError(includeRule.File.ToLower(), filterError);    
                    }

                    if (!string.IsNullOrEmpty(includeRule.Pre))
                        outputConfig.WriteLine(includeRule.Pre);
                    outputConfig.WriteLine("#include \"{0}\"", includeRule.File);
                    if (!string.IsNullOrEmpty(includeRule.Post))
                        outputConfig.WriteLine(includeRule.Post);
                }

                // Write includes to references
                foreach (var reference in configFile.References)
                {
                    if (filesWithIncludes.Contains(reference.Id))
                        outputConfig.WriteLine("#include \"{0}\"", reference.Id + ".h");
                }

                // Dump Create from macros
                if (_filesWithCreateFromMacros.Contains(configFile.Id))
                {
                    foreach (var typeBaseRule in configFile.Extension)
                    {
                        if (CheckIfRuleIsCreatingHeadersExtension(typeBaseRule))
                            outputConfig.WriteLine("// {0}", typeBaseRule);
                    }
                    outputConfig.WriteLine("#include \"{0}\"", configFile.ExtensionFileName);


                    _includeToProcess.Add(configFile.ExtensionId, true);
                    if (!_includeIsAttached.ContainsKey(configFile.ExtensionId))
                        _includeIsAttached.Add(configFile.ExtensionId, true);

                    // Create Extension file name if it doesn't exist);
                    if (!File.Exists(configFile.ExtensionFileName))
                        File.WriteAllText(configFile.ExtensionFileName, "");
                }
                outputConfig.Close();

                var outputConfigStr = outputConfig.ToString();

                var fileName = configFile.Id + ".h";

                // Test if Last config file was generated. If not, then we need to generate it
                // If it exists, then we need to test if it is the same than previous run
                configFile.IsConfigUpdated = ForceParsing;

                if (File.Exists(fileName) && !ForceParsing)
                    configFile.IsConfigUpdated = outputConfigStr != File.ReadAllText(fileName);
                else
                    configFile.IsConfigUpdated = true;

                // Small optim: just write the header file when the file is updated or new
                if (configFile.IsConfigUpdated)
                {
                    if (!ForceParsing)
                        Logger.Message("Config file changed for C++ headers [{0}]/[{1}]", configFile.Id, configFile.FilePath);

                    _isConfigUpdated = true;
                    var fileWriter = new StreamWriter(configFile.Id + ".h");
                    fileWriter.Write(outputConfigStr);
                    fileWriter.Close();
                }
            }
        }

        /// <summary>
        /// Checks if this rule is creating headers extension.
        /// </summary>
        /// <param name="rule">The rule to check.</param>
        /// <returns>true if the rule is creating an header extension.</returns>
        private static bool CheckIfRuleIsCreatingHeadersExtension(ConfigBaseRule rule)
        {
            return ((rule is CreateCppExtensionRule && !string.IsNullOrEmpty(((CreateCppExtensionRule) rule).Macro))
                    || (rule is ConstantRule && !string.IsNullOrEmpty(((ConstantRule) rule).Macro)));
        }

        /// <summary>
        /// Gets the name of the generated GCCXML file.
        /// </summary>
        /// <value>The name of the generated GCCXML file.</value>
        private string GccXmlFileName
        {
            get { return _configRoot.Id + "-gcc.xml"; }
        }

        /// <summary>
        /// Gets the name of the C++ parsed XML file.
        /// </summary>
        /// <value>The name of the C++ parsed XML file.</value>
        private string GroupFileName
        {
            get { return _configRoot.Id + "-out.xml"; }
        }

        /// <summary>
        /// Gets or sets the GccXml doc.
        /// </summary>
        /// <value>The GccXml doc.</value>
        private XDocument GccXmlDoc { get; set; }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        /// <returns></returns>
        public CppModule Run()
        {
            // If config is updated, we need to run the 
            if (_isConfigUpdated)
            {
                Logger.Message("Config files changed.");

                string progressMessage = "Parsing C++ headers starts, please wait...";

                Logger.Progress(10, progressMessage);

                StreamReader xmlReader = null;
                try
                {
                    // TODO Rebuild group

                    var macroManager = new MacroManager(_gccxml);
                    macroManager.Parse(_configRootHeader, _group);

                    // Dump includes
                    foreach (var configFile in _configRoot.ConfigFilesLoaded)
                    {
                        // Dump Create from macros
                        if (_filesWithCreateFromMacros.Contains(configFile.Id) && configFile.IsConfigUpdated)
                        {
                            var extensionWriter = new StreamWriter(configFile.ExtensionFileName);

                            foreach (var typeBaseRule in configFile.Extension)
                            {
                                if (CheckIfRuleIsCreatingHeadersExtension(typeBaseRule))
                                    extensionWriter.Write(CreateCppFromMacro(typeBaseRule));
                                else if (typeBaseRule is ContextRule)
                                    HandleContextRule(configFile, (ContextRule) typeBaseRule);
                            }
                            extensionWriter.Close();
                        }
                    }

                    Logger.Progress(15, progressMessage);

                    xmlReader = _gccxml.Process(_configRootHeader);
                    if (xmlReader != null)
                    {
                        Parse(xmlReader);

                        // If doc must be generated
                        if (IsGeneratingDoc)
                            ApplyDocumentation();
                    }

                    Logger.Progress(30, progressMessage);

                    // Save back the C++ parsed includes
                    _group.Write(GroupFileName);
                }
                catch (Exception ex)
                {
                    Logger.Error("Unexpected error", ex);
                }
                finally
                {
                    if (xmlReader != null)
                        xmlReader.Close();

                    // Write back GCCXML document on the disk
                    if (GccXmlDoc != null)
                        GccXmlDoc.Save(GccXmlFileName);
                    Logger.Message("Parsing headers is finished.");
                }
            }
            else
            {
                Logger.Progress(10, "Config files unchanged. Read previous C++ parsing...");
                _group = CppModule.Read(GroupFileName);
            }

            IncludeMacroCounts = new Dictionary<string, int>();

            // Load all defines and store them in the config file to allow dynamic variable substitution
            foreach (var cppInclude in _group.Includes)
            {
                int count = 0;
                IncludeMacroCounts.TryGetValue(cppInclude.Name, out count);

                foreach (var cppDefine in cppInclude.Macros)
                {
                    _configRoot.DynamicVariables.Remove(cppDefine.Name);
                    _configRoot.DynamicVariables.Add(cppDefine.Name, cppDefine.Value);
                    count ++;
                }

                IncludeMacroCounts[cppInclude.Name] = count;
            }

            // Expand all variables with all dynamic variables
            _configRoot.ExpandVariables(true);

            return _group;
        }

        private Dictionary<string, int> IncludeMacroCounts { get; set; }

        /// <summary>
        /// Prints the statistics.
        /// </summary>
        public void PrintStatistics()
        {

            var keys = IncludeMacroCounts.Keys.ToList();
            keys.Sort(StringComparer.InvariantCultureIgnoreCase);

            Logger.Message("Macro Statistics");
            foreach (var key in keys)
            {
                Logger.Message("\t{0}\t{1}", key, IncludeMacroCounts[key]);
            }
            Logger.Message("\n");
        }

        /// <summary>
        /// Handles the context rule.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="contextRule">The context rule.</param>
        private void HandleContextRule(ConfigFile file, ContextRule contextRule)
        {
            if (contextRule is ClearContextRule)
                _group.ClearContextFind();
            else
            {
                var contextIds = new List<string>();

                if (!string.IsNullOrEmpty(contextRule.ContextSetId))
                {
                    var contextSet = file.FindContextSetById(contextRule.ContextSetId);
                    if (contextSet != null)
                        contextIds.AddRange(contextSet.Contexts);
                }
                contextIds.AddRange(contextRule.Ids);

                _group.AddContextRangeFind(contextIds);
            }
        }

        /// <summary>
        /// Creates a C++ declaration from a macro rule.
        /// </summary>
        /// <param name="rule">The macro rule.</param>
        /// <returns>A C++ declaration string</returns>
        private string CreateCppFromMacro(ConfigBaseRule rule)
        {
            if (rule is CreateCppExtensionRule)
            {
                return CreateEnumFromMacro((CreateCppExtensionRule) rule);
            }
            
            if (rule is ConstantRule)
            {
                return CreateVariableFromMacro((ConstantRule) rule);
            }
            return "";
        }

        /// <summary>
        /// Creates a C++ enum declaration from a macro rule.
        /// </summary>
        /// <param name="createCpp">The macro rule.</param>
        /// <returns>A C++ enum declaration string</returns>
        private string CreateEnumFromMacro(CreateCppExtensionRule createCpp)
        {
            var cppEnumText = new StringBuilder();

            cppEnumText.AppendLine("// Enum created from: " + createCpp);
            cppEnumText.AppendLine("enum " + createCpp.Enum + " {");

            foreach (CppDefine macroDef in _group.Find<CppDefine>(createCpp.Macro))
            {
                string macroName = macroDef.Name + EndTagCustomEnumItem;

                // Only add the macro once (could have multiple identical macro in different includes)
                if (!_variableMacrosDefined.ContainsKey(macroName))
                {
                    cppEnumText.AppendFormat("\t {0} = {1},\n", macroName, macroDef.Value);
                    _variableMacrosDefined.Add(macroName, macroDef.Value);
                }
            }
            cppEnumText.AppendLine("};");

            return cppEnumText.ToString();
        }

        /// <summary>
        /// Creates a C++ variable declaration from a macro rule.
        /// </summary>
        /// <param name="cstRule">The macro rule.</param>
        /// <returns>A C++ variable declaration string</returns>
        private string CreateVariableFromMacro(ConstantRule cstRule)
        {
            var builder = new StringBuilder();

            builder.AppendLine("// Variable created from: " + cstRule);

            // Regex regexValue = new Regex("^(.*)$");

            foreach (CppDefine macroDef in _group.Find<CppDefine>(cstRule.Macro))
            {
                string macroName = macroDef.Name + EndTagCustomVariable;

                // Only add the macro once (could have multiple identical macro in different includes)
                if (!_variableMacrosDefined.ContainsKey(macroName))
                {
                    // string finalValue = string.IsNullOrEmpty(cstRule.Value) ? macroDef.Value : regexValue.Replace(macroDef.Value, cstRule.Value);
                    builder.AppendFormat("extern \"C\" {0} {1} = {3}{2};\n", cstRule.CppType ?? cstRule.Type, macroName, macroDef.Name, cstRule.CppCast ?? "");
                    _variableMacrosDefined.Add(macroName, macroDef.Name);
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// Parses the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        private void Parse(StreamReader reader)
        {
            var doc = XDocument.Load(reader);

            GccXmlDoc = doc;

            // Collects all GccXml elements and build map from their id
            foreach (var xElement in doc.Elements("GCC_XML").Elements())
            {
                string id = xElement.Attribute("id").Value;
                _mapIdToXElement.Add(id, xElement);

                string file = xElement.AttributeValue("file");
                if (file != null)
                {
                    List<XElement> elementsInFile;
                    if (!_mapFileToXElement.TryGetValue(file, out elementsInFile))
                    {
                        elementsInFile = new List<XElement>();
                        _mapFileToXElement.Add(file, elementsInFile);
                    }
                    elementsInFile.Add(xElement);
                }
            }

            // Fix all structure names
            foreach (var xTypedef in doc.Elements("GCC_XML").Elements())
            {
                if (xTypedef.Name.LocalName == CastXml.TagTypedef)
                {
                    var xStruct = _mapIdToXElement[xTypedef.AttributeValue("type")];
                    switch (xStruct.Name.LocalName)
                    {
                        case CastXml.TagStruct:
                        case CastXml.TagUnion:
                        case CastXml.TagEnumeration:
                            string structName = xStruct.AttributeValue("name");
                            // Rename all structure starting with tagXXXX to XXXX
                            //if (structName.Length > 4 && structName.StartsWith("tag") && Char.IsUpper(structName[3]))
                            //    structName = structName.Substring(3);

                            if (structName.StartsWith("tag") || structName.StartsWith("_") || string.IsNullOrEmpty(structName))
                            {
                                var typeName = xTypedef.AttributeValue("name");
                                xStruct.SetAttributeValue("name", typeName);
                                //Logger.Message("Use typedef to rename [{0}] to [{1}]", structName, typeName);
                            }
                            break;
                    }
                }
            }

            // Find all elements that are referring to a context and attach them to 
            // the context as child elements
            foreach (var xElement in _mapIdToXElement.Values)
            {
                string id = xElement.AttributeValue("context");
                if (id != null)
                {
                    xElement.Remove();
                    _mapIdToXElement[id].Add(xElement);
                }
            }

            // AttachToFile(doc);
            ParseAllElements();
        }

        /// <summary>
        /// Parses a C++ function.
        /// </summary>
        /// <param name="xElement">The gccxml <see cref="XElement"/> that describes a C++ function.</param>
        /// <returns>A C++ function parsed</returns>
        private CppFunction ParseFunction(XElement xElement)
        {
            return ParseMethodOrFunction<CppFunction>(xElement);
        }

        /// <summary>
        /// Parses a C++ parameters.
        /// </summary>
        /// <param name="xElement">The gccxml <see cref="XElement"/> that describes a C++ parameter.</param>
        /// <param name="methodOrFunction">The method or function to populate.</param>
        private void ParseParameters(XElement xElement, CppElement methodOrFunction)
        {
            int paramCount = 0;
            foreach (var parameter in xElement.Elements())
            {
                if (parameter.Name.LocalName != "Argument")
                    continue;
                
                var cppParameter = new CppParameter() { Name = parameter.AttributeValue("name") };
                if (string.IsNullOrEmpty(cppParameter.Name))
                    cppParameter.Name = "arg" + paramCount;

                ParseAnnotations(parameter, cppParameter);

                // All parameters without any annotations are considerate as In
                if (cppParameter.Attribute == ParamAttribute.None)
                    cppParameter.Attribute = ParamAttribute.In;

                Logger.PushContext("Parameter:[{0}]", cppParameter.Name);

                ResolveAndFillType(parameter.AttributeValue("type"), cppParameter);
                methodOrFunction.Add(cppParameter);

                Logger.PopContext();
                paramCount++;
            }
        }

        /// <summary>
        /// Parses C++ annotations/attributes.
        /// </summary>
        /// <param name="xElement">The gccxml <see cref="XElement"/> that contains C++ annotations/attributes.</param>
        /// <param name="cppElement">The C++ element to populate.</param>
        private static void ParseAnnotations(XElement xElement, CppElement cppElement)
        {
            // Check that the xml contains the "attributes" attribute
            string attributes = xElement.AttributeValue("attributes");
            if (string.IsNullOrWhiteSpace(attributes))
                return;

            // Strip whitespaces inside annotate("...")
            var stripSpaces = new StringBuilder();
            int doubleQuoteCount = 0;
            for(int i  = 0; i < attributes.Length; i++)
            {
                bool addThisChar = true;
                char attributeChar = attributes[i];
                if (attributeChar == '(')
                {
                    doubleQuoteCount++;
                }
                else if (attributeChar == ')')
                {
                    doubleQuoteCount--;
                }
                else if (doubleQuoteCount > 0 && (char.IsWhiteSpace(attributeChar) | attributeChar == '"'))
                {
                    addThisChar = false;
                }
                if (addThisChar)
                    stripSpaces.Append(attributeChar);                
            }
            attributes = stripSpaces.ToString();
            
            // Default calling convention
            var cppCallingConvention = CppCallingConvention.Unknown;

            // Default parameter attribute
            var paramAttribute = ParamAttribute.None;

            // Default Guid
            string guid = null;

            // Parse attributes
            const string gccXmlAttribute = "annotate(";
            // none == 0
            // pre == 1
            // post == 2
            bool isPre = false;
            bool isPost = false;
            bool hasWritable = false;

            // Clang outputs attributes in reverse order
            // TODO: Check if applies to all declarations
            foreach (var item in attributes.Split(' ').Reverse())
            {
                string newItem = item;
                if (newItem.StartsWith(gccXmlAttribute))
                    newItem = newItem.Substring(gccXmlAttribute.Length);

                if (newItem.StartsWith("SAL_pre"))
                {
                    // paramAttribute |= ParamAttribute.In;
                    isPre = true;
                    isPost = false;
                } 
                else if (newItem.StartsWith("SAL_post"))
                {
                    // paramAttribute |= ParamAttribute.Out;
                    isPre = false;
                    isPost = true;
                }
                else if (isPost && newItem.StartsWith("SAL_valid"))
                {
                    paramAttribute |= ParamAttribute.Out;
                }
                else if (newItem.StartsWith("SAL_maybenull") || (newItem.StartsWith("SAL_null") && newItem.Contains("__maybe")))
                {
                    paramAttribute |= ParamAttribute.Optional;
                }
                else if (newItem.StartsWith("SAL_readableTo") || newItem.StartsWith("SAL_writableTo"))
                {
                    if (newItem.StartsWith("SAL_writableTo"))
                    {
                        if (isPre) paramAttribute |= ParamAttribute.Out;
                        hasWritable = true;
                    }

                    if (!newItem.Contains("SPECSTRINGIZE(1)") && !newItem.Contains("elementCount(1)"))
                        paramAttribute |= ParamAttribute.Buffer;
                }
                else if (newItem.StartsWith("__stdcall__"))
                {
                    cppCallingConvention = CppCallingConvention.StdCall;
                }
                else if (newItem.StartsWith("__cdecl__"))
                {
                    cppCallingConvention = CppCallingConvention.CDecl;
                }
                else if (newItem.StartsWith("uuid("))
                {
                    guid = newItem.Trim(')').Substring("uuid(".Length).Trim('"', '{', '}');
                }
            }

            // If no writable, than this is an In parameter
            if (!hasWritable)
            {
                paramAttribute |= ParamAttribute.In;
            }


            // Update CppElement based on its type
            if (cppElement is CppParameter)
            {
                // Replace in & out with inout.
                // Todo check to use in & out instead of inout
                if ((paramAttribute & ParamAttribute.In) != 0 && (paramAttribute & ParamAttribute.Out) != 0)
                {
                    paramAttribute ^= ParamAttribute.In;
                    paramAttribute ^= ParamAttribute.Out;
                    paramAttribute |= ParamAttribute.InOut;
                }

                ((CppParameter) cppElement).Attribute = paramAttribute;
            } 
            else if (cppElement is CppMethod && cppCallingConvention != CppCallingConvention.Unknown)
            {
                ((CppMethod)cppElement).CallingConvention = cppCallingConvention;
            }
            else if (cppElement is CppInterface && guid != null)
            {
                ((CppInterface)cppElement).Guid = guid;
            }
        }

        /// <summary>
        /// Parses a C++ method or function.
        /// </summary>
        /// <typeparam name="T">The resulting C++ parsed element. Must be a subclass of <see cref="CppMethod"/>.</typeparam>
        /// <param name="xElement">The gccxml <see cref="XElement"/> that describes a C++ method/function declaration.</param>
        /// <returns>The C++ parsed T.</returns>
        private T ParseMethodOrFunction<T>(XElement xElement) where T : CppMethod, new()
        {
            var cppMethod = new T() { Name = xElement.AttributeValue("name") };

            Logger.PushContext("Method:[{0}]", cppMethod.Name);

            // Parse annotations
            ParseAnnotations(xElement, cppMethod);

            // Parse parameters
            ParseParameters(xElement, cppMethod);

            cppMethod.ReturnType = new CppType();
            ResolveAndFillType(xElement.AttributeValue("returns"), cppMethod.ReturnType);

            Logger.PopContext();

            return cppMethod;
        }

        /// <summary>
        /// Parses a C++ COM interface.
        /// </summary>
        /// <param name="xElement">The gccxml <see cref="XElement"/> that describes a C++ COM interface declaration.</param>
        /// <returns>A C++ interface parsed</returns>
        private CppInterface ParseInterface(XElement xElement)
        {
            // If element is already transformed, return it
            var cppInterface = xElement.Annotation<CppInterface>();
            if (cppInterface != null)
                return cppInterface;
           
            // Else, create a new CppInterface
            cppInterface = new CppInterface() { Name = xElement.AttributeValue("name") };         
            xElement.AddAnnotation(cppInterface);

            // Enter Interface description
            Logger.PushContext("Interface:[{0}]", cppInterface.Name);

            if (!IsTypeBinded(xElement))
                Logger.Error("Binding is missing for interface type [{0}] defined in file [{1}]", cppInterface.Name, _mapIdToXElement[xElement.AttributeValue("file")].AttributeValue("name"));

            // Calculate offset method using inheritance
            int offsetMethod = 0;

            var basesValue = xElement.AttributeValue("bases");
            var bases = basesValue != null ? basesValue.Split(' ') : Enumerable.Empty<string>();
            foreach (var xElementBaseId in bases)
            {
                if (string.IsNullOrEmpty(xElementBaseId))
                    continue;

                var xElementBase = _mapIdToXElement[xElementBaseId];
                string baseTypeName = xElementBase.AttributeValue("name");
                string baseTypeFile = _mapIdToXElement[xElementBase.AttributeValue("file")].AttributeValue("name");

                CppInterface cppInterfaceBase = null;
                Logger.RunInContext("Base", () => { cppInterfaceBase = ParseInterface(xElementBase); });

                if (string.IsNullOrEmpty(cppInterface.ParentName))
                    cppInterface.ParentName = cppInterfaceBase.Name;

                // If interface is binded, then check that the bind is a valid interface and not a SharpDX.ComObject/System.IntPtr
                string bindedValueTo;
                if (_bindings.TryGetValue(baseTypeName, out bindedValueTo))
                {
                    if (baseTypeName != "IUnknown" && baseTypeName != "IDispatch" && (
                            bindedValueTo == "SharpDX.ComObject"
                            || bindedValueTo == "System.IntPtr"))
                    {
                        Logger.Error("Error binding interface type [{0}] defined in file [{1}]. Interface is inherited and binded to [{2}] and not valid for inheritance", baseTypeName, baseTypeFile, bindedValueTo);
                    }
                }

                offsetMethod += cppInterfaceBase.TotalMethodCount;
            }

            // Parse annotations
            ParseAnnotations(xElement, cppInterface);

            List<CppMethod> methods = new List<CppMethod>();

            // Parse methods
            foreach (var method in xElement.Elements())
            {
                // Parse method with pure virtual (=0) and that do not override any other methods
                if (method.Name.LocalName == "Method" && !string.IsNullOrWhiteSpace(method.AttributeValue("pure_virtual"))
                    && string.IsNullOrWhiteSpace(method.AttributeValue("overrides")))
                {
                    var cppMethod = ParseMethodOrFunction<CppMethod>(method);
                    methods.Add(cppMethod);
                }
            }

            // The Visual C++ compiler breaks the rules of the COM ABI when overloaded methods are used.
            // It will group the overloads together in memory and lay them out in the reverse of their declaration order.
            // Since GCC always lays them out in the order declared, we have to modify the order of the methods to match Visual C++.
            // See http://support.microsoft.com/kb/131104 for more information.
            for (int i = 0; i < methods.Count; i++)
            {
                string name = methods[i].Name;

                // Look for overloads of this function
                for (int j = i + 1; j < methods.Count; j++)
                {
                    var nextMethod = methods[j];
                    if (nextMethod.Name == name)
                    {
                        // Remove this one from its current position further into the vtable
                        methods.RemoveAt(j);

                        // Put this one before all other overloads (aka reverse declaration order)
                        int k = i - 1;
                        while (k >= 0 && methods[k].Name == name)
                            k--;
                        methods.Insert(k + 1, nextMethod);
                        i++;
                    }
                }
            }

            // Add the methods to the interface with the correct offsets
            foreach (var cppMethod in methods)
            {
                cppMethod.Offset = offsetMethod++;
                cppInterface.Add(cppMethod);
            }

            cppInterface.TotalMethodCount = offsetMethod;

            // Leave Interface
            Logger.PopContext();

            return cppInterface;
        }

        /// <summary>
        /// Parses a C++ field declaration.
        /// </summary>
        /// <param name="xElement">The gccxml <see cref="XElement"/> that describes a C++ structure field declaration.</param>
        /// <returns>A C++ field parsed</returns>
        private CppField ParseField(XElement xElement)
        {
            var cppField = new CppField() { Name = xElement.AttributeValue("name") };

            Logger.PushContext("Field:[{0}]", cppField.Name);

            // Handle bitfield info
            var bitField = xElement.AttributeValue("bits");
            if (!string.IsNullOrEmpty(bitField))
            {
                cppField.IsBitField = true;

                // Todo, int.Parse could failed?
                cppField.BitOffset = int.Parse(bitField);
            }

            ResolveAndFillType(xElement.AttributeValue("type"), cppField);

            Logger.PopContext();
            return cppField;
        }

        /// <summary>
        /// Parses a C++ struct or union declaration.
        /// </summary>
        /// <param name="xElement">The gccxml <see cref="XElement"/> that describes a C++ struct or union declaration.</param>
        /// <param name="cppParent">The C++ parent object (valid for anonymous inner declaration) .</param>
        /// <param name="innerAnonymousIndex">An index that counts the number of anonymous declaration in order to set a unique name</param>
        /// <returns>A C++ struct parsed</returns>
        private CppStruct ParseStructOrUnion(XElement xElement, CppElement cppParent = null, int innerAnonymousIndex = 0)
        {
            // Build struct name directly from the struct name or based on the parent
            var structName = xElement.AttributeValue("name") ?? "";
            if (cppParent != null)
            {
                if (string.IsNullOrEmpty(structName)) 
                    structName = cppParent.Name + "_INNER_" + innerAnonymousIndex;
                else
                    structName = cppParent.Name + "_" + structName + "_INNER";
            }

            // Create struct
            var cppStruct = new CppStruct { Name = structName };
            bool isUnion = (xElement.Name.LocalName == CastXml.TagUnion);

            // Get align from structure
            cppStruct.Align = int.Parse(xElement.AttributeValue("align"))/8;

            // By default, packing is platform x86/x64 dependent (4 or 8)
            // but because gccxml is running in x86, it outputs 4
            // So by default, we are reversing all align by 4 to 0
            // IF the packing is a true 4, than it will be reverse back by a later mapping rules
            if (cppStruct.Align == 4)
                cppStruct.Align = 0;

            // Enter struct/union description
            Logger.PushContext("{0}:[{1}]", xElement.Name.LocalName, cppStruct.Name);

            // Parse all fields
            int fieldOffset = 0;
            int innerStructCount = 0;
            foreach (var field in xElement.Elements())
            {
                if (field.Name.LocalName != CastXml.TagField)
                    continue;

                // Parse the field
                var cppField = ParseField(field);
                cppField.Offset = fieldOffset;

                // Test if the field type is declared inside this struct or union
                var fieldName = field.AttributeValue("name");
                var fieldType = _mapIdToXElement[field.AttributeValue("type")];
                if (fieldType.AttributeValue("context") == xElement.AttributeValue("id"))
                {
                    var fieldSubStruct = ParseStructOrUnion(fieldType, cppStruct, innerStructCount++);

                    // If fieldName is empty, then we need to inline fields from the struct/union.
                    if (string.IsNullOrEmpty(fieldName))
                    {
                        // Make a copy in order to remove fields
                        var listOfSubFields = new List<CppField>(fieldSubStruct.Fields);
                        // Copy the current field offset
                        int lastFieldOffset = fieldOffset;
                        foreach (var subField in listOfSubFields)
                        {
                            subField.Offset = subField.Offset + fieldOffset;
                            cppStruct.Add(subField);
                            lastFieldOffset = subField.Offset;
                        }
                        // Set the current field offset according to the inlined fields
                        if (!isUnion)
                            fieldOffset = lastFieldOffset;
                        // Don't add the current field, as it is actually an inline struct/union
                        cppField = null;
                    }
                    else
                    {
                        // Get the type name from the inner-struct and set it to the field
                        cppField.TypeName = fieldSubStruct.Name;
                        _currentCppInclude.Add(fieldSubStruct);
                    }                    
                }

                // Go to next field offset if not in union
                bool goToNextFieldOffset = !isUnion;

                // Add the field if any
                if (cppField != null)
                {
                    cppStruct.Add(cppField);
                    // TODO managed multiple bitfield group
                    // Current implem is only working with a single set of consecutive bitfield in the same struct
                    goToNextFieldOffset = goToNextFieldOffset && !cppField.IsBitField;
                }

                if (goToNextFieldOffset)
                    fieldOffset++;
            }

            // Leave struct
            Logger.PopContext();

            return cppStruct;
        }

        /// <summary>
        /// Parses a C++ enum declaration.
        /// </summary>
        /// <param name="xElement">The gccxml <see cref="XElement"/> that describes a C++ enum declaration.</param>
        /// <returns>A C++ parsed enum</returns>
        private CppEnum ParseEnum(XElement xElement)
        {
            var cppEnum = new CppEnum() { Name = xElement.AttributeValue("name") };

            // Doh! Anonymous Enum, need to handle them!
            if (cppEnum.Name.StartsWith("$") || string.IsNullOrEmpty(cppEnum.Name))
            {
                var includeFrom = GetIncludeIdFromFileId(xElement.AttributeValue("file"));

                int enumOffset;
                if (!_mapIncludeToAnonymousEnumCount.TryGetValue(includeFrom, out enumOffset))
                    _mapIncludeToAnonymousEnumCount.Add(includeFrom, enumOffset);

                cppEnum.Name = includeFrom.ToUpper() + "_ENUM_" + enumOffset;

                _mapIncludeToAnonymousEnumCount[includeFrom]++;
            }

            foreach (var xEnumItems in xElement.Elements())
            {
                string enumItemName = xEnumItems.AttributeValue("name");
                if (enumItemName.EndsWith(EndTagCustomEnumItem))
                    enumItemName = enumItemName.Substring(0, enumItemName.Length - EndTagCustomEnumItem.Length);

                cppEnum.Add(new CppEnumItem(enumItemName, xEnumItems.AttributeValue("init")));

            }
            return cppEnum;
        }

        /// <summary>
        /// Parses a C++ variable declaration/definition.
        /// </summary>
        /// <param name="xElement">The gccxml <see cref="XElement"/> that describes a C++ variable declaration/definition.</param>
        /// <returns>A C++ parsed variable</returns>
        private CppElement ParseVariable(XElement xElement)
        {
            var name = xElement.AttributeValue("name");
            if (name.EndsWith(EndTagCustomVariable))
                name = name.Substring(0, name.Length - EndTagCustomVariable.Length);

            var cppType = new CppType();
            ResolveAndFillType(xElement.AttributeValue("type"), cppType);


            var value = xElement.AttributeValue("init");
            if (cppType.TypeName == "GUID")
            {
                var guid = ParseGuid(value);
                if (!guid.HasValue)
                    return null;
                return new CppGuid { Name = name, Guid = guid.Value };
            }

            // CastXML outputs initialization expressions. Cast to proper type.
            var match = Regex.Match(value, @"\((?:\(.+\))?(.+)\)");
            if (match.Success)
            {
                value = $"unchecked(({cppType.TypeName}){match.Groups[1].Value})";
            }

            // Handle C++ floating point literals
            value = value.Replace(".F", ".0F");

            return new CppConstant() { Name = name, Value = value };
        }

        /// <summary>
        /// Parses a C++ GUID definition string.
        /// </summary>
        /// <param name="guidInitText">The text of a GUID gccxml initialization.</param>
        /// <returns>The parsed Guid</returns>
        private static Guid? ParseGuid(string guidInitText)
        {
            // init="{-1135593225ul, 9184u, 18784u, {150u, 218u, 51u, 171u, 175u, 89u, 53u, 236u}}"
            if (!guidInitText.StartsWith("{") && !guidInitText.EndsWith("}}"))
                return null;

            guidInitText = guidInitText.Replace("{", "");
            guidInitText = guidInitText.TrimEnd('}');
            guidInitText = guidInitText.Replace("u", "");
            guidInitText = guidInitText.Replace("U", "");
            guidInitText = guidInitText.Replace("l", "");
            guidInitText = guidInitText.Replace("L", "");
            guidInitText = guidInitText.Replace(" ", "");

            string[] guidElements = guidInitText.Split(',');

            if (guidElements.Length != 11)
                return null;

            var values = new int[guidElements.Length];
            for (int i = 0; i < guidElements.Length; i++)
            {
                var guidElement = guidElements[i];
                long value;
                if (!long.TryParse(guidElement, out value))
                    return null;

                values[i] = unchecked((int)value);
            }

            return new Guid(values[0], (short)values[1], (short)values[2], (byte)values[3], (byte)values[4], (byte)values[5], (byte)values[6], (byte)values[7],
                     (byte)values[8], (byte)values[9], (byte)values[10]);
        }

        /// <summary>
        /// Parses all C++ elements. This is the main method that iterates on all types.
        /// </summary>
        private void ParseAllElements()
        {
            foreach (var includeGccXmlId in _mapFileToXElement.Keys)
            {
                string includeId = GetIncludeIdFromFileId(includeGccXmlId);

                // Process only files listed inside the config files
                if (!_includeToProcess.ContainsKey(includeId))
                    continue;

                // Process only files attached (fully or partially) to an assembly/namespace
                bool isIncludeFullyAttached;
                if (!_includeIsAttached.TryGetValue(includeId, out isIncludeFullyAttached))
                    continue;

                // Log current include being processed
                Logger.PushContext("Include:[{0}.h]", includeId);

                _currentCppInclude = _group.FindInclude(includeId);
                if (_currentCppInclude == null)
                {
                    _currentCppInclude = new CppInclude() { Name = includeId };
                    _group.Add(_currentCppInclude);
                }

                foreach (var xElement in _mapFileToXElement[includeGccXmlId])
                {
                    // If the element is not defined from a root namespace
                    // than skip it, as it might be an inner type
                    if (_mapIdToXElement[xElement.AttributeValue("context")].Name.LocalName != CastXml.TagNamespace)
                        continue;

                    // If incomplete flag, than element cannot be parsed
                    if (xElement.AttributeValue("incomplete") != null)
                        continue;

                    string name = xElement.Name.LocalName;

                    string elementName = xElement.AttributeValue("name");

                    // If this include is partially attached and the current type is not attached
                    // Than skip it, as we are not mapping it
                    if (!isIncludeFullyAttached && !_includeAttachedTypes[includeId].Contains(elementName))
                        continue;

                    CppElement cppElement = null;
                    switch (name)
                    {
                        case CastXml.TagEnumeration:
                            cppElement = ParseEnum(xElement);
                            break;
                        case CastXml.TagFunction:
                            // TODO: Find btter criteria for exclusion. In CastXML extern="1" only indicates an explicit external storage modifier.
                            // For now, exlude inline functions instead; may not be sensible since by default all functions have external linkage.
                            if (xElement.AttributeValue("inline") == null)
                                cppElement = ParseFunction(xElement);
                            break;
                        case CastXml.TagStruct:
                            if (xElement.AttributeValue("abstract") != null)
                                cppElement = ParseInterface(xElement);
                            else
                                cppElement = ParseStructOrUnion(xElement);
                            break;
                        case CastXml.TagUnion:
                            cppElement = ParseStructOrUnion(xElement);
                            break;
                        case CastXml.TagVariable:
                            if (xElement.AttributeValue("init") != null)
                                cppElement = ParseVariable(xElement);
                            break;
                    }

                    if (cppElement != null)
                        _currentCppInclude.Add(cppElement);
                }

                Logger.PopContext();
            }
        }

        /// <summary>
        /// Determines whether the specified type is a type included in the mapping process.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        /// 	<c>true</c> if the specified type is included in the mapping process; otherwise, <c>false</c>.
        /// </returns>
        private bool IsTypeFromIncludeToProcess(XElement type)
        {
            string fileId = type.AttributeValue("file");
            if (fileId != null)
                return _includeToProcess.ContainsKey(GetIncludeIdFromFileId(fileId));
            return false;
        }

        /// <summary>
        /// Determines whether the specified type has a binded in the mapping process.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        /// 	<c>true</c> if the specified type has a binded in the mapping process; otherwise, <c>false</c>.
        /// </returns>
        private bool IsTypeBinded(XElement type)
        {
            string typeName = type.AttributeValue("name");
            return IsTypeFromIncludeToProcess(type) || _bindings.ContainsKey(typeName);
        }

        /// <summary>
        /// Resolves a type to its fundamental type or a binded type.
        /// This methods is going through the type declaration in order to return the most fundamental type
        /// or to return a bind.
        /// </summary>
        /// <param name="typeId">The id of the type to resolve.</param>
        /// <param name="type">The C++ type to fill.</param>
        private void ResolveAndFillType(string typeId, CppType type)
        {
            var  fullTypeName = new List<string>();

            var xType = _mapIdToXElement[typeId];

            bool isTypeResolved = false;

            while (!isTypeResolved)
            {
                string name = xType.AttributeValue("name");
                if (name != null)
                    fullTypeName.Add(name);
                string nextType = xType.AttributeValue("type");
                switch (xType.Name.LocalName)
                {
                    case CastXml.TagFundamentalType:
                        type.TypeName = ConvertFundamentalType(name);
                        isTypeResolved = true;
                        break;
                    case CastXml.TagEnumeration:
                        type.TypeName = name;
                        isTypeResolved = true;
                        break;
                    case CastXml.TagStruct:
                    case CastXml.TagUnion:
                        type.TypeName = name;

                        // If the structure being processed is an external include
                        // and the type is not binded, then there is probably a missing binding
                        //if (!IsTypeBinded(xType))
                            //Logger.Error("Binding is missing for type [{0}] defined in file [{1}]", string.Join("/", fullTypeName), _mapIdToXElement[xType.AttributeValue("file")].AttributeValue("name"));

                        isTypeResolved = true;
                        break;
                    case CastXml.TagTypedef:
                        if (_bindings.ContainsKey(name))
                        {
                            type.TypeName = name;
                            isTypeResolved = true;
                        }
                        xType = _mapIdToXElement[nextType];
                        break;
                    case CastXml.TagPointerType:
                        xType = _mapIdToXElement[nextType];
                        type.Pointer = (type.Pointer ?? "") + "*";
                        break;
                    case CastXml.TagArrayType:
                        type.IsArray = true;
                        var maxArrayIndex = xType.AttributeValue("max");
                        var arrayDim = int.Parse(maxArrayIndex.TrimEnd('u')) + 1;
                        if (type.ArrayDimension == null)
                            type.ArrayDimension = "" + arrayDim;
                        else
                            type.ArrayDimension += "," + arrayDim;
                        xType = _mapIdToXElement[nextType];
                        break;
                    case CastXml.TagReferenceType:
                        xType = _mapIdToXElement[nextType];
                        type.Pointer = (type.Pointer ?? "") + "&";
                        break;
                    case CastXml.TagCvQualifiedType:
                        xType = _mapIdToXElement[nextType];
                        type.Const = true;
                        break;
                    case CastXml.TagFunctionType:
                        // TODO, handle different calling convention
                        type.TypeName = "__function__stdcall";
                        isTypeResolved = true;
                        break;
                    default:
                        throw new InvalidOperationException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Unexpected tag type [{0}]", xType.Name.LocalName));
                }
            }
        }

        /// <summary>
        /// Converts a gccxml FundamentalType to a shorter form:
        ///	signed char          => char
        ///	long long int        => longlong
        ///	short unsigned int   => unsigned short
        ///	char                 => char
        ///	long unsigned int    => unsigned int
        ///	short int            => short
        ///	int                  => int
        ///	long int             => int
        ///	float                => float
        ///	unsigned char        => unsigned char
        ///	unsigned int         => unsigned int
        ///	wchar_t              => wchar_t
        ///	long long unsigned int => unsigned longlong
        ///	double               => double
        ///	void                 => void
        ///	long double          => long double
        /// </summary>
        /// <param name="typeName">Name of the gccxml fundamental type.</param>
        /// <returns>a shorten form</returns>
        private static string ConvertFundamentalType(string typeName)
        {
            var types = typeName.Split(' ');

            bool isUnsigned = false;
            string outputType = "";
            int shortCount = 0;
            int longCount = 0;

            foreach (var type in types)
            {
                switch (type)
                {
                    case "unsigned":
                        isUnsigned = true;
                        break;
                    case "signed":
                        outputType = "int";
                        break;
                    case "long":
                        longCount++;
                        break;
                    case "short":
                        shortCount++;
                        break;
                    case "bool":
                    case "void":
                    case "char":
                    case "double":
                    case "int":
                    case "float":
                    case "wchar_t":
                        outputType = type;
                        break;
                    default:
                        Logger.Error("Unhandled partial type [{0}] from Fundamental type [{1}]", type, typeName);
                        break;
                }                    
            }

            if (longCount == 1 && outputType == "double")
                outputType = "long double";     // 96 bytes, unhandled
            if (longCount == 2)
                outputType = "longlong";
            if (shortCount == 1)
                outputType = "short";
            if (isUnsigned)
                outputType = "unsigned " + outputType;
            return outputType;
        }


        /// <summary>
        /// Gets the include id from the file id.
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <returns>A include id</returns>
        private string GetIncludeIdFromFileId(string fileId)
        {
            string filePath = _mapIdToXElement[fileId].AttributeValue("name");
            if (!File.Exists(filePath))
                return "";
            return Path.GetFileNameWithoutExtension(filePath).ToLower();            
        }

        /// <summary>
        /// Apply documentation from an external provider. This is optional.
        /// </summary>
        private void ApplyDocumentation()
        {
            // Use default MSDN doc provider
            DocProvider docProvider = new DocProviderMsdn();

            // Try to load doc provider from an external assembly
            if (DocProviderAssembly != null)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(DocProviderAssembly);

                    foreach (var type in assembly.GetTypes())
                    {
                        if (typeof(DocProvider).IsAssignableFrom(type))
                        {
                            docProvider = (DocProvider)Activator.CreateInstance(type);
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Warning("Warning, Unable to locate/load DocProvider Assembly.");
                    Logger.Warning("Warning, DocProvider was not found from assembly [{0}]", DocProviderAssembly);
                }
            }

            Logger.Progress(20, "Applying C++ documentation");

            docProvider.Begin();

            foreach (CppInclude cppInclude in _group.Includes)
            {
                foreach (CppEnum cppEnum in cppInclude.Enums)
                {
                    DocItem docItem = docProvider.FindDocumentation(cppEnum.Name);
                    cppEnum.Id = docItem.Id;
                    cppEnum.Description = docItem.Description;
                    cppEnum.Remarks = docItem.Remarks;

                    if (cppEnum.IsEmpty)
                        continue;

                    if(cppEnum.Items.Count != docItem.Items.Count)
                    {
                        //Logger.Warning("Warning Invalid number enum items in documentation for Enum {0}",
                        //    cppEnum.Name);
                    }
                    int count = Math.Min(cppEnum.Items.Count, docItem.Items.Count);
                    int i = 0;
                    foreach (CppEnumItem cppEnumItem in cppEnum.EnumItems)
                    {
                        cppEnumItem.Id = docItem.Id;

                        // Try to find the matching item
                        bool foundMatch = false;
                        foreach (var subItem in docItem.Items)
                        {
                            if (Utilities.ContainsCppIdentifier(subItem.Term, cppEnumItem.Name))
                            {
                                cppEnumItem.Description = subItem.Description;
                                foundMatch = true;
                                break;
                            }
                        }
                        if (!foundMatch && i < count)
                            cppEnumItem.Description = docItem.Items[i].Description;
                        i++;
                    }
                }

                foreach (CppStruct cppStruct in cppInclude.Structs)
                {
                    DocItem docItem = docProvider.FindDocumentation(cppStruct.Name);
                    cppStruct.Id = docItem.Id;
                    cppStruct.Description = docItem.Description;
                    cppStruct.Remarks = docItem.Remarks;

                    if (cppStruct.IsEmpty)
                        continue;

                    if(cppStruct.Items.Count != docItem.Items.Count)
                    {
                        //Logger.Warning("Invalid number of fields in documentation for Struct {0}", cppStruct.Name);
                    }
                    int count = Math.Min(cppStruct.Items.Count, docItem.Items.Count);
                    int i = 0;
                    foreach (CppField cppField in cppStruct.Fields)
                    {
                        cppField.Id = docItem.Id;

                        // Try to find the matching item
                        bool foundMatch = false;
                        foreach (var subItem in docItem.Items)
                        {
                            if (Utilities.ContainsCppIdentifier(subItem.Term, cppField.Name))
                            {
                                cppField.Description = subItem.Description;
                                foundMatch = true;
                                break;
                            }
                        }
                        if (!foundMatch && i < count)
                            cppField.Description = docItem.Items[i].Description;
                        i++;
                    }
                }

                foreach (CppInterface cppInterface in cppInclude.Interfaces)
                {
                    DocItem docItem = docProvider.FindDocumentation(cppInterface.Name);
                    cppInterface.Id = docItem.Id;
                    cppInterface.Description = docItem.Description;
                    cppInterface.Remarks = docItem.Remarks;

                    if (cppInterface.IsEmpty)
                        continue;

                    foreach (CppMethod cppMethod in cppInterface.Methods)
                    {
                        string methodName = cppInterface.Name + "::" + cppMethod.Name;
                        DocItem methodDocItem = docProvider.FindDocumentation(methodName);
                        cppMethod.Id = methodDocItem.Id;
                        cppMethod.Description = methodDocItem.Description;
                        cppMethod.Remarks = methodDocItem.Remarks;
                        cppMethod.ReturnType.Description = methodDocItem.Return;

                        if (cppMethod.IsEmpty)
                            continue;

                        if(cppMethod.Items.Count != methodDocItem.Items.Count)
                        {
                            //Logger.Warning("Invalid number of documentation for Parameters for method {0}",
                            //    methodName);
                        }
                        int count = Math.Min(cppMethod.Items.Count, methodDocItem.Items.Count);
                        int i = 0;
                        foreach (CppParameter cppParameter in cppMethod.Parameters)
                        {
                            cppParameter.Id = methodDocItem.Id;

                            // Try to find the matching item
                            bool foundMatch = false;
                            foreach (var subItem in methodDocItem.Items)
                            {
                                if (Utilities.ContainsCppIdentifier(subItem.Term, cppParameter.Name))
                                {
                                    cppParameter.Description = subItem.Description;
                                    foundMatch = true;
                                    break;
                                }
                            }
                            if (!foundMatch && i < count)
                                cppParameter.Description = methodDocItem.Items[i].Description;
                            i++;
                        }
                    }
                }

                foreach (CppFunction cppFunction in cppInclude.Functions)
                {
                    DocItem docItem = docProvider.FindDocumentation(cppFunction.Name);
                    cppFunction.Id = docItem.Id;
                    cppFunction.Description = docItem.Description;
                    cppFunction.Remarks = docItem.Remarks;
                    cppFunction.ReturnType.Description = docItem.Return;

                    if (cppFunction.IsEmpty)
                        continue;

                    if(cppFunction.Items.Count != docItem.Items.Count)
                    {
                        //Logger.Warning("Invalid number of documentation for Parameters for Function {0}",
                        //    cppFunction.Name);
                    }
                    int count = Math.Min(cppFunction.Items.Count, docItem.Items.Count);
                    int i = 0;
                    foreach (CppParameter cppParameter in cppFunction.Parameters)
                    {
                        cppParameter.Id = docItem.Id;

                        // Try to find the matching item
                        bool foundMatch = false;
                        foreach (var subItem in docItem.Items)
                        {
                            if (Utilities.ContainsCppIdentifier(subItem.Term, cppParameter.Name))
                            {
                                cppParameter.Description = subItem.Description;
                                foundMatch = true;
                                break;
                            }
                        }
                        if (!foundMatch && i < count)
                            cppParameter.Description = docItem.Items[i].Description;
                        i++;
                    }
                }
            }
            docProvider.End();
        }
    }
}