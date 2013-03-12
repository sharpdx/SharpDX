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
    /// <summary>
    /// TODO: COMMENT THIS CODE
    /// </summary>
    public class ModelCompilerTask : CompilerDependencyTask
    {
        protected override bool ProcessItem(TkItem item)
        {
            bool hasErrors = false;

            var inputFilePath = item.InputFilePath;
            var outputFilePath = item.OutputFilePath;

            try
            {
                var dependencyFilePath = Path.Combine(Path.Combine(ProjectDirectory.ItemSpec, IntermediateDirectory.ItemSpec), FileDependencyList.GetDependencyFileNameFromSourcePath(item.LinkName));

                // Creates the dependency directory if it does no exist yet.
                var dependencyDirectoryPath = Path.GetDirectoryName(dependencyFilePath);
                if (!Directory.Exists(dependencyDirectoryPath))
                {
                    Directory.CreateDirectory(dependencyDirectoryPath);
                }

                Log.LogMessage(MessageImportance.Low, "Check Toolkit Model file to compile {0} with dependency file {1}", inputFilePath, dependencyFilePath);

                if (FileDependencyList.CheckForChanges(dependencyFilePath) || !File.Exists(outputFilePath))
                {
                    Log.LogMessage(MessageImportance.Low, "Start to compile {0}", inputFilePath);

                    var compilerOptions = new ModelCompilerOptions()
                                              {
                                                  DependencyFile = dependencyFilePath,
                                                  Quality = Debug ? ModelRealTimeQuality.Low : ModelRealTimeQuality.Maximum
                                              };
                    var compilerResult = ModelCompiler.CompileAndSave(inputFilePath, outputFilePath, compilerOptions);

                    if (compilerResult.HasErrors)
                    {
                        foreach (var message in compilerResult.Logger.Messages)
                        {
                            Log.LogError(message.ToString());
                        }

                        hasErrors = true;
                    }
                    else
                    {
                        Log.LogMessage(MessageImportance.Low, "Compiled successfull {0} to {1}", inputFilePath, outputFilePath);

                        if (item.OutputCs)
                        {
                            Log.LogWarning("Compilation to CS not yet supported for Font file");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogError("Cannot write compiled file to {0} : {1}", inputFilePath, ex.Message);
                hasErrors = true;
            }

            return !hasErrors;
        }
    }
}