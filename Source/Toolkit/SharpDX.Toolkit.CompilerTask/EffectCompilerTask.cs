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
using System.Text;
using System.Text.RegularExpressions;

using Microsoft.Build.Framework;

using SharpDX.IO;
using SharpDX.Toolkit.Graphics;

namespace SharpDX.Toolkit
{
    public class EffectCompilerTask : EffectCompilerDependencyTask
    {
        private static Regex parseMessage = new Regex(@"(.*)\s*\(\s*(\d+)\s*,\s*([^ \)]+)\)\s*:\s*(\w+)\s+(\w+)\s*:\s*(.*)");
        private static Regex matchNumberRange = new Regex(@"(\d+)-(\d+)");

        private static void SetupD3DCompilerPath()
        {
            var assemblyPath = Path.GetDirectoryName(typeof(EffectCompilerTask).Assembly.Location);

            var redistD3DPath = Path.GetFullPath(Path.Combine(assemblyPath, @"..\Redist\D3D\" + (IntPtr.Size == 4 ? "x86" : "x64")));

            var existingPaths = new List<string>(Environment.GetEnvironmentVariable("PATH").Split(';'));

            if (!existingPaths.Contains(redistD3DPath))
            {
                var newPath = redistD3DPath + ";" + Environment.GetEnvironmentVariable("PATH");
                Environment.SetEnvironmentVariable("PATH", newPath);
            }
        }

        private EffectCompiler compiler;

        public override bool Execute()
        {
            SetupD3DCompilerPath();

            compiler = new EffectCompiler();

            return base.Execute();
        }

        protected override bool ProcessItem(TkFxcItem item)
        {
            bool hasErrors = false;

            var inputFilePath = item.InputFilePath;
            var outputFilePath = item.OutputFilePath;

            var dependencyFilePath = Path.Combine(Path.Combine(ProjectDirectory.ItemSpec, IntermediateDirectory.ItemSpec), compiler.GetDependencyFileNameFromEffectPath(item.LinkName));

            // Creates the dependency directory if it does no exist yet.
            var dependencyDirectoryPath = Path.GetDirectoryName(dependencyFilePath);
            if (!Directory.Exists(dependencyDirectoryPath))
            {
                Directory.CreateDirectory(dependencyDirectoryPath);
            }

            Log.LogMessage(MessageImportance.High, "Check Toolkit FX file to compile {0} with dependency file {1}", inputFilePath, dependencyFilePath);

            if (compiler.CheckForChanges(dependencyFilePath) || !File.Exists(outputFilePath))
            {
                Log.LogMessage(MessageImportance.High, "Start to compile {0}", inputFilePath);

                var compilerResult = compiler.CompileFromFile(inputFilePath, Debug ? EffectCompilerFlags.Debug : EffectCompilerFlags.None, null, null, item.DynamicCompiling, dependencyFilePath);

                if (compilerResult.HasErrors)
                {
                    hasErrors = true;
                }
                else
                {
                    Log.LogMessage(MessageImportance.High, "Compiled successfull {0} to {1}", inputFilePath, outputFilePath);
                }

                foreach (var message in compilerResult.Logger.Messages)
                {
                    var text = message.ToString();

                    string line = null;
                    var textReader = new StringReader(text);
                    while ((line = textReader.ReadLine()) != null)
                    {
                        var match = parseMessage.Match(line);
                        if (match.Success)
                        {
                            var filePath = match.Groups[1].Value;
                            var lineNumber = int.Parse(match.Groups[2].Value);
                            var colNumberText = match.Groups[3].Value;
                            int colStartNumber;
                            int colEndNumber;
                            var colMatch = matchNumberRange.Match(colNumberText);
                            if (colMatch.Success)
                            {
                                int.TryParse(colMatch.Groups[1].Value, out colStartNumber);
                                int.TryParse(colMatch.Groups[2].Value, out colEndNumber);
                            }
                            else
                            {
                                int.TryParse(colNumberText, out colStartNumber);
                                colEndNumber = colStartNumber;
                            }
                            var msgType = match.Groups[4].Value;
                            var msgCode = match.Groups[5].Value;
                            var msgText = match.Groups[6].Value;
                            if (string.Compare(msgType, "error", StringComparison.InvariantCultureIgnoreCase) == 0)
                            {
                                Log.LogError(string.Empty, msgCode, string.Empty, filePath, lineNumber, colStartNumber, lineNumber, colEndNumber, msgText);
                            }
                            else if (string.Compare(msgType, "warning", StringComparison.InvariantCultureIgnoreCase) == 0)
                            {
                                Log.LogWarning(string.Empty, msgCode, string.Empty, filePath, lineNumber, colStartNumber, lineNumber, colEndNumber, msgText);
                            }
                            else if (string.Compare(msgType, "info", StringComparison.InvariantCultureIgnoreCase) == 0)
                            {
                                Log.LogWarning(string.Empty, msgCode, string.Empty, filePath, lineNumber, colStartNumber, lineNumber, colEndNumber, msgText);
                            }
                            else
                            {
                                Log.LogWarning(line);
                            }
                        }
                        else
                        {
                            Log.LogWarning(line);
                        }
                    }
                }

                if (!compilerResult.HasErrors && compilerResult.EffectData != null)
                {
                    try
                    {
                        var directoryName = Path.GetDirectoryName(outputFilePath);
                        if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
                        {
                            Directory.CreateDirectory(directoryName);
                        }

                        if (item.OutputCs)
                        {
                            var codeWriter = new EffectDataCodeWriter
                            {
                                Namespace = item.OutputNamespace,
                                ClassName = item.OutputClassName,
                                FieldName = item.OutputFieldName,
                            };

                            using (var stream = new NativeFileStream(outputFilePath, NativeFileMode.Create, NativeFileAccess.Write, NativeFileShare.Write))
                            {
                                codeWriter.Write(compilerResult.EffectData, new StreamWriter(stream, Encoding.UTF8));
                            }
                        }
                        else
                        {
                            compilerResult.EffectData.Save(outputFilePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.LogError("Cannot write compiled file to {0} : {1}", inputFilePath, ex.Message);
                        hasErrors = true;
                    }
                }
            }

            return !hasErrors;
        }
   }
}