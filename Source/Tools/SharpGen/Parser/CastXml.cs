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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Win32;

using SharpGen.Logging;

using SharpGen.Config;

namespace SharpGen.Parser
{
    /// <summary>
    /// CastXML front end for command line.
    /// see https://github.com/CastXML/CastXML
    /// </summary>
    public class CastXml
    {
        private static readonly Regex MatchError = new Regex("error:");

        /// <summary>
        /// GccXml tag for FundamentalType
        /// </summary>
        public const string TagFundamentalType = "FundamentalType";

        /// <summary>
        /// GccXml tag for Enumeration
        /// </summary>
        public const string TagEnumeration = "Enumeration";

        /// <summary>
        /// GccXml tag for Struct
        /// </summary>
        public const string TagStruct = "Struct";

        /// <summary>
        /// GccXml tag for Field
        /// </summary>
        public const string TagField = "Field";

        /// <summary>
        /// GccXml tag for Union
        /// </summary>
        public const string TagUnion = "Union";

        /// <summary>
        /// GccXml tag for Typedef
        /// </summary>
        public const string TagTypedef = "Typedef";

        /// <summary>
        /// GccXml tag for Function
        /// </summary>
        public const string TagFunction = "Function";

        /// <summary>
        /// GccXml tag for PointerType
        /// </summary>
        public const string TagPointerType = "PointerType";

        /// <summary>
        /// GccXml tag for ArrayType
        /// </summary>
        public const string TagArrayType = "ArrayType";

        /// <summary>
        /// GccXml tag for ReferenceType
        /// </summary>
        public const string TagReferenceType = "ReferenceType";

        /// <summary>
        /// GccXml tag for CvQualifiedType
        /// </summary>
        public const string TagCvQualifiedType = "CvQualifiedType";

        /// <summary>
        /// GccXml tag for Namespace
        /// </summary>
        public const string TagNamespace = "Namespace";

        /// <summary>
        /// GccXml tag for Variable
        /// </summary>
        public const string TagVariable = "Variable";

        /// <summary>
        /// GccXml tag for FunctionType
        /// </summary>
        public const string TagFunctionType = "FunctionType";

        /// <summary>
        /// Gets or sets the executable path of gccxml.exe.
        /// </summary>
        /// <value>The executable path.</value>
        public string ExecutablePath {get;set;}

        /// <summary>
        /// Gets or sets the include directory list.
        /// </summary>
        /// <value>The include directory list.</value>
        public List<IncludeDirRule> IncludeDirectoryList { get; private set; }

        /// <summary>
        /// List of error filters regexp.
        /// </summary>
        private readonly List<Regex> _filterErrors;

        /// <summary>
        /// Initializes a new instance of the <see cref="CastXml"/> class.
        /// </summary>
        public CastXml()
        {
            IncludeDirectoryList = new List<IncludeDirRule>();
            _filterErrors = new List<Regex>();
        }

        /// <summary>
        /// Adds a filter error that will ignore a particular error from gccxml.
        /// </summary>
        /// <param name="file">The headerFile.</param>
        /// <param name="regexpError">a regexp that filters a particular gccxml error message.</param>
        public void AddFilterError(string file, string regexpError)
        {
            string fullRegexpError = @"[\\/]" + Regex.Escape(file) + ":.*" + regexpError;
            _filterErrors.Add(new Regex(fullRegexpError));
        }

        /// <summary>
        /// Preprocesses the specified header file.
        /// </summary>
        /// <param name="headerFile">The header file.</param>
        /// <param name="handler">The handler.</param>
        public void Preprocess(string headerFile, DataReceivedEventHandler handler)
        {
            Logger.RunInContext("castxml", () =>
                    {
                        if (!File.Exists(ExecutablePath))
                            Logger.Fatal("castxml.exe not found from path: [{0}]", ExecutablePath);

                        if (!File.Exists(headerFile))
                            Logger.Fatal("C++ Header file [{0}] not found", headerFile);

                        var currentProcess = new Process();
                        var startInfo = new ProcessStartInfo(ExecutablePath)
                            {
                                RedirectStandardOutput = true,
                                RedirectStandardError = true,
                                UseShellExecute = false,
                                CreateNoWindow = true,
                                WorkingDirectory = Environment.CurrentDirectory
                            };

                        var arguments = GetCastXmlArgs();
                        arguments += " -E -dD";

                        foreach (var directory in GetIncludePaths())
                            arguments += " " + directory;

                        startInfo.Arguments = arguments + " " + headerFile;
                        Console.WriteLine(startInfo.Arguments);
                        currentProcess.StartInfo = startInfo;
                        currentProcess.ErrorDataReceived += ProcessErrorFromHeaderFile;
                        currentProcess.OutputDataReceived += handler;
                        currentProcess.Start();
                        currentProcess.BeginOutputReadLine();
                        currentProcess.BeginErrorReadLine();

                        currentProcess.WaitForExit();
                        currentProcess.Close();

                    });
        }

        private List<string> GetIncludePaths()
        {
            var paths = new List<string>();

            foreach (var directory in IncludeDirectoryList)
            {
                var path = directory.Path;

                // Is Using registry?
                if (path.StartsWith("="))
                {
                    var registryPath = directory.Path.Substring(1);
                    var indexOfSubPath = registryPath.IndexOf(";");
                    string subPath = "";
                    if (indexOfSubPath >= 0)
                    {
                        subPath = registryPath.Substring(indexOfSubPath + 1);
                        registryPath = registryPath.Substring(0, indexOfSubPath);
                    }
                    var indexOfKey = registryPath.LastIndexOf("\\");
                    var subKeyStr = registryPath.Substring(indexOfKey + 1);
                    registryPath = registryPath.Substring(0, indexOfKey);

                    var indexOfHive = registryPath.IndexOf("\\");
                    var hiveStr = registryPath.Substring(0, indexOfHive).ToUpper();
                    registryPath = registryPath.Substring(indexOfHive+1);

                    try
                    {
                        var hive = RegistryHive.LocalMachine;
                        switch (hiveStr)
                        {
                            case "HKEY_LOCAL_MACHINE":
                                hive = RegistryHive.LocalMachine;
                                break;
                            case "HKEY_CURRENT_USER":
                                hive = RegistryHive.CurrentUser;
                                break;
                            case "HKEY_CURRENT_CONFIG":
                                hive = RegistryHive.CurrentConfig;
                                break;
                        }
                        var rootKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry32);
                        var subKey = rootKey.OpenSubKey(registryPath);
                        if (subKey == null)
                        {
                            Logger.Error("Unable to locate key [{0}] in registry", registryPath);
                            continue;

                        }
                        path = Path.Combine(subKey.GetValue(subKeyStr).ToString(), subPath);
                    } catch (Exception ex)
                    {
                        Logger.Error("Unable to locate key [{0}] in registry", registryPath);
                        continue;
                    }
                }

                if (directory.IsOverride)
                {
                    paths.Add("-I\"" + path.TrimEnd('\\') + "\"");
                }
                else
                {
                    paths.Add("-isystem\"" + path.TrimEnd('\\') + "\"");
                }
            }

            foreach (var path in paths)
            {
                Logger.Message("Path used for castxml [{0}]", path);
            }

            return paths;
        }

        /// <summary>
        /// Processes the specified header headerFile.
        /// </summary>
        /// <param name="headerFile">The header headerFile.</param>
        /// <returns></returns>
        public StreamReader Process(string headerFile)
        {
            StreamReader result = null;

            Logger.RunInContext("castxml", () =>
            {
                ExecutablePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, ExecutablePath));

                if (!File.Exists(ExecutablePath)) Logger.Fatal("castxml.exe not found from path: [{0}]", ExecutablePath);

                if (!File.Exists(headerFile)) Logger.Fatal("C++ Header file [{0}] not found", headerFile);

                var currentProcess = new Process();
                var startInfo = new ProcessStartInfo(ExecutablePath)
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = Environment.CurrentDirectory
                };
                var xmlFile = Path.ChangeExtension(headerFile, "xml");

                // Delete any previously generated xml file
                File.Delete(xmlFile);

                string arguments = GetCastXmlArgs();
                arguments += " -o " + xmlFile;

                foreach (var directory in GetIncludePaths())
                    arguments += " " + directory;

                startInfo.Arguments = arguments + " " + headerFile;

                Console.WriteLine(startInfo.Arguments);
                currentProcess.StartInfo = startInfo;
                currentProcess.ErrorDataReceived += ProcessErrorFromHeaderFile;
                currentProcess.OutputDataReceived += ProcessOutputFromHeaderFile;
                currentProcess.Start();
                currentProcess.BeginOutputReadLine();
                currentProcess.BeginErrorReadLine();

                currentProcess.WaitForExit();

                currentProcess.Close();

                if (!File.Exists(xmlFile) || Logger.HasErrors)
                {
                    Logger.Error("Unable to generate XML file with castxml [{0}]. Check previous errors.", xmlFile);
                }
                else
                {
                    result = new StreamReader(xmlFile);
                }
            });

            return result;
        }

        private static string GetCastXmlArgs()
        {
            var arguments = "";
            arguments += " --castxml-gccxml"; 
            arguments += " -x c++ -std=c++11 -fmsc-version=1900 -fms-extensions -fms-compatibility";
            arguments += " -Wno-microsoft-enum-value -Wmacro-redefined -Wno-invalid-token-paste -Wno-ignored-attributes";
            return arguments;
        }

        // E:/Code/Microsoft DirectX SDK (June 2010)//include/xaudio2fx.h:68:1: error:
        private static Regex matchFileErrorRegex = new Regex(@"^(.*):(\d+):(\d+):\s+error:(.*)");

        /// <summary>
        /// Processes the error from header file.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Diagnostics.DataReceivedEventArgs"/> instance containing the event data.</param>
        void ProcessErrorFromHeaderFile(object sender, DataReceivedEventArgs e)
        {
            bool popContext = false;
            try
            {
                if (e.Data != null)
                {

                    var matchError = matchFileErrorRegex.Match(e.Data);

                    bool lineFiltered = false;
                    foreach (var filterError in _filterErrors)
                    {
                        if (filterError.Match(e.Data).Success)
                        {
                            lineFiltered = true;
                            break;
                        }

                    }
                    string errorText = e.Data;

                    if (matchError.Success)
                    {
                        Logger.PushLocation(matchError.Groups[1].Value, int.Parse(matchError.Groups[2].Value), int.Parse(matchError.Groups[3].Value));
                        popContext = true;
                        errorText = matchError.Groups[4].Value;
                    }

                    if (!lineFiltered)
                    {
                        if (MatchError.Match(e.Data).Success)
                            Logger.Error(errorText);
                        else
                            Logger.Warning(errorText);
                    }
                    else
                    {
                        Logger.Warning(errorText);
                    }
                }
            }
            finally
            {
                if (popContext)
                    Logger.PopLocation();
            }
        }

        /// <summary>
        /// Processes the output from header file.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Diagnostics.DataReceivedEventArgs"/> instance containing the event data.</param>
        static void ProcessOutputFromHeaderFile(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
                Logger.Message(e.Data);
        }
    }
}