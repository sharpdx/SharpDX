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

using SharpDX.IO;
using SharpDX.Toolkit.Graphics;

namespace SharpDX.Toolkit
{
    public class EffectCompilerTask : ContentCompilerTask
    {
        private static void SetupD3DCompilerPath()
        {
            var assemblyPath = Path.GetDirectoryName(typeof(EffectCompilerTask).Assembly.Location);

            var redistD3DPath = Path.GetFullPath(Path.Combine(assemblyPath, @"..\Redist\D3D\" + (IntPtr.Size == 4 ? "x86" : "x64")));

            var existingPaths = new List<string>(Environment.GetEnvironmentVariable("PATH").Split(';'));

            if (Directory.Exists(redistD3DPath) && !existingPaths.Contains(redistD3DPath))
            {
                var newPath = redistD3DPath + ";" + Environment.GetEnvironmentVariable("PATH");
                Environment.SetEnvironmentVariable("PATH", newPath);
            }
        }

        private EffectCompiler compiler;

        protected override void Initialize()
        {
            SetupD3DCompilerPath();

            compiler = new EffectCompiler();
            parseLogMessages = true;

            base.Initialize();
        }

        protected override Diagnostics.Logger ProcessFileAndGetLogResults(string inputFilePath, string outputFilePath, string dependencyFilePath, TkItem item)
        {
            var compilerFlags = Debug ? (EffectCompilerFlags.Debug | EffectCompilerFlags.SkipOptimization | EffectCompilerFlags.PreferFlowControl) : EffectCompilerFlags.None;
            if (!string.IsNullOrEmpty(CompilerFlags))
            {
                compilerFlags |= (EffectCompilerFlags)Enum.Parse(typeof(EffectCompilerFlags), CompilerFlags);
            }

            var compilerResult = compiler.CompileFromFile(inputFilePath,
                                                              compilerFlags,
                                                              null,
                                                              null,
                                                              item.DynamicCompiling,
                                                              dependencyFilePath);

            if (!compilerResult.HasErrors && compilerResult.EffectData != null)
            {
                CreateDirectoryIfNotExists(outputFilePath);

                if (item.OutputCs)
                {
                    var codeWriter = new EffectDataCodeWriter
                                     {
                                         Namespace = item.OutputNamespace,
                                         ClassName = item.OutputClassName,
                                         FieldName = item.OutputFieldName,
                                     };

                    using (var stream = new NativeFileStream(outputFilePath, NativeFileMode.Create, NativeFileAccess.Write, NativeFileShare.Write))
                        codeWriter.Write(compilerResult.EffectData, new StreamWriter(stream, Encoding.UTF8));
                }
                else
                {
                    compilerResult.EffectData.Save(outputFilePath);
                }
            }

            return compilerResult.Logger;
        }
    }
}