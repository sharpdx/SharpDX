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
using SharpDX.Toolkit.Diagnostics;
using SharpDX.Toolkit.Graphics;
using Microsoft.Build.Framework;

namespace SharpDX.Toolkit
{
    public abstract class ContentCompilerTask : CompilerDependencyTask
    {
        private static readonly Regex parseMessage = new Regex(@"(.*)\s*\(\s*(\d+)\s*,\s*([^ \)]+)\)\s*:\s*(\w+)\s+(\w+)\s*:\s*(.*)");
        private static readonly Regex matchNumberRange = new Regex(@"(\d+)-(\d+)");

        protected bool parseLogMessages;

        protected sealed override bool ProcessItem(TkItem item)
        {
            var hasErrors = false;

            var inputFilePath = item.InputFilePath;
            var outputFilePath = item.OutputFilePath;

            try
            {
                var dependencyFilePath = Path.Combine(Path.Combine(ProjectDirectory.ItemSpec, IntermediateDirectory.ItemSpec),
                                                      FileDependencyList.GetDependencyFileNameFromSourcePath(item.LinkName));

                CreateDirectoryIfNotExists(dependencyFilePath);

                Log.LogMessage(MessageImportance.Low, "Check Toolkit file to compile {0} with dependency file {1}", inputFilePath, dependencyFilePath);
                if (FileDependencyList.CheckForChanges(dependencyFilePath) || !File.Exists(outputFilePath))
                {
                    Log.LogMessage(MessageImportance.Low, "Starting compilation of {0}", inputFilePath);

                    var logger = ProcessFileAndGetLogResults(inputFilePath, outputFilePath, dependencyFilePath, item);

                    LogLogger(logger);

                    if (logger.HasErrors)
                    {
                        hasErrors = true;
                    }
                    else
                    {
                        Log.LogMessage(MessageImportance.High, "Compilation successfull of {0} to {1}", inputFilePath, outputFilePath);
                        if (item.OutputCs)
                            Log.LogWarning("Compilation to CS not yet supported");
                    }
                }

            }
            catch (Exception ex)
            {
                Log.LogError("Cannot process file '{0}' : {1}", inputFilePath, ex.Message);
                hasErrors = true;
            }

            return !hasErrors;
        }

        protected abstract Diagnostics.Logger ProcessFileAndGetLogResults(string inputFilePath, string outputFilePath, string dependencyFilePath, TkItem item);

        protected void CreateDirectoryIfNotExists(string filePath)
        {
            var dependencyDirectoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dependencyDirectoryPath))
                Directory.CreateDirectory(dependencyDirectoryPath);
        }

        private void LogLogger(Diagnostics.Logger logs)
        {
            if (logs == null)
                return;

            foreach (var message in logs.Messages)
            {
                if (parseLogMessages)
                    LogParsedMessage(message);
                else
                    LogSimpleMessage(message);
            }
        }

        private void LogSimpleMessage(LogMessage message)
        {
            switch (message.Type)
            {
            case LogMessageType.Warning:
                Log.LogWarning(message.Text);
                break;
            case LogMessageType.Error:
                Log.LogError(message.Text);
                break;
            case LogMessageType.Info:
                Log.LogMessage(MessageImportance.Low, message.Text);
                break;
            }
        }

        private void LogParsedMessage(LogMessage message)
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

                    var msgCode = match.Groups[5].Value;
                    var msgText = match.Groups[6].Value;

                    switch (message.Type)
                    {
                    case LogMessageType.Warning:
                        Log.LogError(string.Empty, msgCode, string.Empty, filePath, lineNumber, colStartNumber, lineNumber, colEndNumber, msgText);
                        break;
                    case LogMessageType.Error:
                        Log.LogWarning(string.Empty, msgCode, string.Empty, filePath, lineNumber, colStartNumber, lineNumber, colEndNumber, msgText);
                        break;
                    case LogMessageType.Info:
                        Log.LogWarning(string.Empty, msgCode, string.Empty, filePath, lineNumber, colStartNumber, lineNumber, colEndNumber, msgText);
                        break;
                    default:
                        Log.LogWarning(line);
                        break;
                    }
                }
                else
                {
                    Log.LogWarning(line);
                }
            }
        }
    }
}