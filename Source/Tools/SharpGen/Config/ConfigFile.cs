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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using SharpGen.Logging;

namespace SharpGen.Config
{
    /// <summary>
    /// Config File.
    /// </summary>
    [XmlRoot("config", Namespace=NS)]
    public class ConfigFile
    {
        internal const string NS = "urn:SharpGen.Config";

        public ConfigFile()
        {
            Depends = new List<string>();
            Files = new List<string>();
            References = new List<ConfigFile>();
            IncludeProlog = new List<string>();
            IncludeDirs = new List<IncludeDirRule>();
            Variables = new List<KeyValue>();
            Naming = new List<NamingRule>();
            Includes = new List<IncludeRule>();
            Mappings = new List<ConfigBaseRule>();
            DynamicVariables = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets dynamic variables used by dynamic variable substitution #{MyVariable}
        /// </summary>
        /// <value>The dynamic variables.</value>
        [XmlIgnore]
        public Dictionary<string, string> DynamicVariables { get; private set; }

        /// <summary>
        /// Adds a dynamic variable. Remove any previous variable with the same key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void AddDynamicVariable(string key, string value)
        {
            DynamicVariables.Remove(key);
            DynamicVariables.Add(key, value);
        }

        /// <summary>
        /// Gets or sets the parent of this mapping file.
        /// </summary>
        /// <value>The parent.</value>
        [XmlIgnore]
        public ConfigFile Parent { get; set; }

        /// <summary>
        /// Gets or sets the path of this MappingFile. If not null, used when saving this file.
        /// </summary>
        /// <value>The path.</value>
        [XmlIgnore]
        public string FilePath { get; set; }

        [XmlIgnore]
        public string AbsoluteFilePath
        {
            get
            {
                if (Path.IsPathRooted(FilePath))
                    return FilePath;
                if (Parent != null)
                    return Path.Combine(Path.GetDirectoryName(Parent.AbsoluteFilePath), FilePath);;
                return Path.GetFullPath(Path.Combine(".", FilePath));
            }
        }


        /*
        private static void AddDependency(MappingFile configFile, List<MappingFile> dependencyList)
        {
            // Dependency is already done for this file
            foreach (var mappingFile in dependencyList)
                if (mappingFile.Id == configFile.Id)
                    return;

            foreach (var configId in configFile.Depends)
                AddDependency(configFile.MapIdToFile[configId], dependencyList);

            dependencyList.Add(configFile);
        }

        public List<MappingFile> ComputeDependencyList()
        {
            var dependency = new List<string>();
            dependency.Add(GetRoot().Id);

            var configToProcess = new List<MappingFile>();
            configToProcess.AddRange(ConfigFiles);


            do
            {
                for (int i = configToProcess.Count - 1; i >= 0; i--)
                {
                    var config = configToProcess[i];

                    bool dependencyAllResolved = true;
                    foreach (var configId in config.Depends)
                    {
                        if (!dependency.Contains(configId))
                        {
                            dependencyAllResolved = false;
                            break;
                        }
                    }
                    if (dependencyAllResolved)
                    {
                        dependency.Add(config.Id);
                        configToProcess.RemoveAt(i);
                    }
                }
            } while (configToProcess.Count > 0);
        }
        */

        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlElement("depends")]
        public List<string> Depends { get; set; }

        [XmlElement("namespace")]
        public string Namespace { get; set; }

        [XmlElement("assembly")]
        public string Assembly { get; set; }

        [XmlElement("var")]
        public List<KeyValue> Variables { get; set; }

        [XmlElement("file")]
        public List<string> Files { get; set; }

        [XmlIgnore]
        public List<ConfigFile> References { get; set; }

        [XmlElement("include-dir")]
        public List<IncludeDirRule> IncludeDirs { get; set; }

        [XmlElement("include-prolog")]
        public List<string> IncludeProlog { get; set; }

        [XmlElement("include")]
        public List<IncludeRule> Includes { get; set; }

        [XmlArray("naming"),XmlArrayItem(typeof(NamingRuleShort))]
        public List<NamingRule> Naming { get; set; }

        [XmlElement("context-set")]
        public List<ContextSetRule> ContextSets { get; set; }

        [XmlArray("extension")]
        [XmlArrayItem(typeof(ContextRule))]
        [XmlArrayItem(typeof(CreateExtensionRule))]
        [XmlArrayItem(typeof(CreateCppExtensionRule))]
        [XmlArrayItem(typeof(DefineExtensionRule))]
        [XmlArrayItem(typeof(ConstantRule))]
        public List<ConfigBaseRule> Extension { get; set; }

        [XmlIgnore]
        public string ExtensionId { get { return Id + "-ext"; } }

        /// <summary>
        /// Gets the name of the extension header file.
        /// </summary>
        /// <value>The name of the extension header file.</value>
        [XmlIgnore]
        public string ExtensionFileName { get { return ExtensionId + ".h"; } }

        [XmlArray("bindings")]
        public List<BindRule> Bindings { get; set; }

        [XmlArray("mapping")]
        [XmlArrayItem(typeof(MappingRule))]
        [XmlArrayItem(typeof(MoveRule))]
        [XmlArrayItem(typeof(RemoveRule))]
        [XmlArrayItem(typeof(ContextRule))]
        [XmlArrayItem(typeof(ClearContextRule))]
        public List<ConfigBaseRule> Mappings { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this config was updated.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this config was updated; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnore]
        public bool IsConfigUpdated { get; set; }

        /// <summary>
        /// Returns all depend config files
        /// </summary>
        /// <returns></returns>
        public List<ConfigFile> FindAllDependencies()
        {
            var dependencies = new List<ConfigFile>();
            FindAllDependencies(dependencies);
            return dependencies;
        }

        /// <summary>
        /// Finds all dependencies ConfigFile from this instance.
        /// </summary>
        /// <param name="dependencyListOutput">The dependencies list to fill.</param>
        public void FindAllDependencies(List<ConfigFile> dependencyListOutput)
        {
            foreach (var dependConfigFileId in Depends)
            {
                var linkedConfig = GetRoot().MapIdToFile[dependConfigFileId];
                if (!dependencyListOutput.Contains(linkedConfig))
                    dependencyListOutput.Add(linkedConfig);

                linkedConfig.FindAllDependencies(dependencyListOutput);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether we need to process mappings.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is mapping to process; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnore]
        public bool IsMappingToProcess { get; set; }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(ConfigFile other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Id, Id);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (ConfigFile)) return false;
            return Equals((ConfigFile) obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }

        /// <summary>
        /// Finds a context set by Id.
        /// </summary>
        /// <param name="contextSetId">The context set id.</param>
        /// <returns></returns>
        public ContextSetRule FindContextSetById(string contextSetId)
        {
            if (ContextSets != null)
                return ContextSets.FirstOrDefault(contextSetRule => contextSetRule.Id == contextSetId);
            return null;
        }

        /// <summary>
        /// Expands all dynamic variables used inside Bindings and Mappings tags.
        /// </summary>
        /// <param name="expandDynamicVariable">if set to <c>true</c> [expand dynamic variable].</param>
        public void ExpandVariables(bool expandDynamicVariable)
        {
            ExpandVariables(Variables, expandDynamicVariable);
            ExpandVariables(Includes, expandDynamicVariable);
            ExpandVariables(IncludeDirs, expandDynamicVariable);
            ExpandVariables(Bindings, expandDynamicVariable);
            ExpandVariables(Mappings, expandDynamicVariable);
            // Do it recursively
            foreach (var configFile in References)
                configFile.ExpandVariables(expandDynamicVariable);
        }

        /// <summary>
        /// Iterate on all objects and sub-objects to expand dynamic variable
        /// </summary>
        /// <param name="objectToExpand">The object to expand.</param>
        /// <returns>the expanded object</returns>
        private object ExpandVariables(object objectToExpand, bool expandDynamicVariable)
        {
            if (objectToExpand == null)
                return null;
            if (objectToExpand is string)
                return ExpandString((string)objectToExpand, expandDynamicVariable);
            if (objectToExpand.GetType().IsPrimitive)
                return objectToExpand;
            if (objectToExpand is IList)
            {
                var list = ((IList) objectToExpand);
                for (int i = 0; i < list.Count; i++)
                    list[i] = ExpandVariables(list[i], expandDynamicVariable);
                return list;
            }
            foreach (var propertyInfo in objectToExpand.GetType().GetProperties())
            {
                if (propertyInfo.GetCustomAttributes(typeof(XmlIgnoreAttribute), false).Length == 0)
                {
                    // Check that this field is "ShouldSerializable"
                    var method = objectToExpand.GetType().GetMethod("ShouldSerialize" + propertyInfo.Name);
                    if (method != null && !((bool)method.Invoke(objectToExpand, null)))
                        continue;

                    propertyInfo.SetValue(objectToExpand, ExpandVariables(propertyInfo.GetValue(objectToExpand, null), expandDynamicVariable), null);
                }
            }
            return objectToExpand;
        }


        /// <summary>
        /// Gets a variable value. Value is expanded if it contains any reference to other variables.
        /// </summary>
        /// <param name="variableName">Name of the variable.</param>
        /// <returns>the value of this variable</returns>
        private string GetVariable(string variableName)
        {
            foreach (var keyValue in Variables)
            {
                if (keyValue.Name == variableName)
                    return ExpandString(keyValue.Value, false);
            }
            if (Parent != null)
                return Parent.GetVariable(variableName);
            return null;
        }

        /// <summary>
        /// Regex used to expand variable
        /// </summary>
        static readonly Regex ReplaceVariableRegex = new Regex(@"\$\(([a-zA-Z_][\w_]*)\)", RegexOptions.Compiled);
        static readonly Regex ReplaceDynamicVariableRegex = new Regex(@"#\(([a-zA-Z_][\w_]*)\)", RegexOptions.Compiled);

        /// <summary>
        /// Expands a string using environment variable and variables defined in mapping files.
        /// </summary>
        /// <param name="str">The string to expand.</param>
        /// <returns>the expanded string</returns>
        public string ExpandString(string str, bool expandDynamicVariable)
        {
            var result = str;

            // Perform Config Variable substitution
            if (ReplaceVariableRegex.Match(result).Success)
            {
                result = ReplaceVariableRegex.Replace(result, delegate(Match match)
                                                                {
                                                                    string name = match.Groups[1].Value;
                                                                    string localResult = GetVariable(name);
                                                                    if (localResult == null)
                                                                        localResult = Environment.GetEnvironmentVariable(name);
                                                                    if (localResult == null)
                                                                    {
                                                                        Logger.Error("Unable to substitute config/environment variable $({0}). Variable is not defined", name);
                                                                        return "";
                                                                    }
                                                                    return localResult;
                                                                });
            }

            // Perform Dynamic Variable substitution
            if (expandDynamicVariable && ReplaceDynamicVariableRegex.Match(result).Success)
            {
                result = ReplaceDynamicVariableRegex.Replace(result, delegate(Match match)
                                                                         {
                                                                             string name = match.Groups[1].Value;
                                                                             string localResult;
                                                                             if (!GetRoot().DynamicVariables.TryGetValue(name, out localResult))
                                                                             {
                                                                                 Logger.Error("Unable to substitute dynamic variable #({0}). Variable is not defined", name);
                                                                                 return "";
                                                                             }
                                                                             localResult = localResult.Trim('"');
                                                                             return localResult;
                                                                         });
            }           
            return result;
        }

        private void PostLoad(ConfigFile parent, string file, string[] macros, IEnumerable<KeyValue> variables)
        {
            FilePath = file;
            Parent = parent;

            Variables.Add(new KeyValue("THIS_CONFIG_PATH", Path.GetDirectoryName(AbsoluteFilePath)));
            Variables.AddRange(variables);

            // Load all dependencies
            foreach (var dependFile in Files)
            {
                var dependFilePath = ExpandString(dependFile, false);
                if (!Path.IsPathRooted(dependFilePath))
                    dependFilePath = Path.Combine(Path.GetDirectoryName(AbsoluteFilePath), dependFilePath);
                
                var subMapping = Load(this, dependFilePath, macros, variables);
                if (subMapping != null)
                {
                    subMapping.FilePath = dependFile;
                    References.Add(subMapping);
                }
            }

            // Clear all depends file
            Files.Clear();

            // Add this mapping file
            GetRoot().MapIdToFile.Add(Id, this);            
        }

        public IEnumerable<ConfigFile> ConfigFilesLoaded
        {
            get { return GetRoot().MapIdToFile.Values; }
        }


        /// <summary>
        /// Gets the latest timestamp from a set of config files.
        /// </summary>
        /// <param name="files">The files to check.</param>
        /// <returns>The latest timestamp from a set of config files</returns>
        public static DateTime GetLatestTimestamp(IEnumerable<ConfigFile> files)
        {
            var latestTimestmap = new DateTime(0);
            foreach (var configFile in files)
            {
                var fileTime = File.GetLastWriteTime(configFile.AbsoluteFilePath);
                if (fileTime > latestTimestmap)
                    latestTimestmap = fileTime;
            }
            return latestTimestmap;
        }

        /// <summary>
        /// Loads the specified config file attached to a parent config file.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="file">The file.</param>
        /// <returns>The loaded config</returns>
        private static ConfigFile Load(ConfigFile parent, string file, string[] macros, IEnumerable<KeyValue> variables)
        {
            var deserializer = new XmlSerializer(typeof(ConfigFile));
            ConfigFile config = null;
            try
            {
                Logger.PushLocation(file);
                config = (ConfigFile)deserializer.Deserialize(new StringReader(Preprocessor.Preprocess(File.ReadAllText(file), macros)));

                if (config != null)
                    config.PostLoad(parent, file, macros, variables);
            }
            catch (Exception ex)
            {
                Logger.Error("Unable to parse file [{0}]", ex, file);
            }
            finally
            {
                Logger.PopLocation();
            }
            return config;
        }

        public ConfigFile GetRoot()
        {
            var root = this;
            while (root.Parent != null)
                root = root.Parent;
            return root;
        }

        private Dictionary<string,ConfigFile> MapIdToFile = new Dictionary<string, ConfigFile>();

        private void Verify()
        {
            Depends.Remove("");

            // TODO: verify Depends
            foreach (var depend in Depends)
            {
                if (!GetRoot().MapIdToFile.ContainsKey(depend))
                    throw new InvalidOperationException("Unable to resolve dependency [" + depend + "] for config file [" + Id + "]");
            }

            foreach (var includeDir in IncludeDirs)
            {


                includeDir.Path = ExpandString(includeDir.Path, false);

                if (!includeDir.Path.StartsWith("=") && !Directory.Exists(includeDir.Path))
                    throw new DirectoryNotFoundException("Directory [" + includeDir.Path + "] not found in config file [" + Id + "]");
            }

            // Verify all dependencies
            foreach (var mappingFile in References)
                mappingFile.Verify();
        }

        /// <summary>
        /// Loads a specified MappingFile.
        /// </summary>
        /// <param name="file">The MappingFile.</param>
        /// <returns>The MappingFile loaded</returns>
        public static ConfigFile Load(string file, string[] macros, params KeyValue[] variables)
        {
            var root =  Load(null, file, macros, variables);
            root.Verify();
            root.ExpandVariables(false);
            return root;
        }

        public void Write(string file)
        {
            var output = new FileStream(file, FileMode.Create);
            Write(output);
            output.Close();
        }

        /// <summary>
        /// Writes this MappingFile to the disk.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void Write(Stream writer)
        {
            //Create our own namespaces for the output
            var ns = new XmlSerializerNamespaces();
            ns.Add("", NS);
            var serializer = new XmlSerializer(typeof(ConfigFile));
            serializer.Serialize(writer, this, ns);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "config {0}", Id);
        }
    }
}