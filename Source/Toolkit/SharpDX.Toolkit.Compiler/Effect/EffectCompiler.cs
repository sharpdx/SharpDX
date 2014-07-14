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

using System.Collections.Generic;
using System.IO;

using SharpDX.IO;
using SharpDX.Text;

namespace SharpDX.Toolkit.Graphics
{
    using D3DCompiler;

    public delegate Stream IncludeFileDelegate(bool isSystemInclude, string file);

    /// <summary>
    /// Main class used to compile a Toolkit FX file.
    /// </summary>
    public class EffectCompiler : IEffectCompiler
    {
        public string GetDependencyFileNameFromSourcePath(string pathToFxFile)
        {
            return FileDependencyList.GetDependencyFileNameFromSourcePath(pathToFxFile);
        }

        public List<string> LoadDependency(string dependencyFilePath)
        {
            return FileDependencyList.FromFileRaw(dependencyFilePath);
        }

        /// <summary>
        /// Checks for changes from a dependency file.
        /// </summary>
        /// <param name="dependencyFilePath">The dependency file path.</param>
        /// <returns><c>true</c> if a file has been updated, <c>false</c> otherwise</returns>
        public bool CheckForChanges(string dependencyFilePath)
        {
            return FileDependencyList.CheckForChanges(dependencyFilePath);
        }

        /// <summary>
        /// Compiles an effect from file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="macros">The macrosArgs.</param>
        /// <param name="includeDirectoryList">The include directory list.</param>
        /// <param name="alloDynamicCompiling">Whether or not to allow dynamic compilation.</param>
        /// <param name="dependencyFilePath">Path to dependency files.</param>
        /// <returns>The result of compilation.</returns>
        public EffectCompilerResult CompileFromFile(string filePath, EffectCompilerFlags flags = EffectCompilerFlags.None, List<EffectData.ShaderMacro> macros = null, List<string> includeDirectoryList = null, bool alloDynamicCompiling = false, string dependencyFilePath = null)
        {
            return Compile(NativeFile.ReadAllText(filePath, Encoding.UTF8, NativeFileShare.ReadWrite), filePath, flags, macros, includeDirectoryList, alloDynamicCompiling, dependencyFilePath);
        }

        /// <summary>
        /// Compiles an effect from the specified source code and file path.
        /// </summary>
        /// <param name="sourceCode">The source code.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="macrosArgs">The macrosArgs.</param>
        /// <param name="includeDirectoryList">The include directory list.</param>
        /// <param name="allowDynamicCompiling">Whether or not to allow dynamic compilation.</param>
        /// <param name="dependencyFilePath">Path to dependency files.</param>
        /// <returns>The result of compilation.</returns>
        public EffectCompilerResult Compile(string sourceCode, string filePath, EffectCompilerFlags flags = EffectCompilerFlags.None, List<EffectData.ShaderMacro> macrosArgs = null, List<string> includeDirectoryList = null, bool allowDynamicCompiling = false, string dependencyFilePath = null)
        {
            var compiler = new EffectCompilerInternal();
            return compiler.Compile(sourceCode, filePath, flags, macrosArgs, includeDirectoryList, allowDynamicCompiling, dependencyFilePath);
        }

        /// <summary>
        /// Disassembles a shader HLSL bytecode to asm code.
        /// </summary>
        /// <param name="shader">The shader.</param>
        /// <returns>A string containing asm code decoded from HLSL bytecode.</returns>
        public string DisassembleShader(EffectData.Shader shader)
        {
            var compiler = new EffectCompilerInternal();
            return compiler.DisassembleShader(shader);
        }

        /// <summary>
        /// Builds effect data from the provided bytecode.
        /// </summary>
        /// <param name="shaderSource">The bytecode list to for the provided effect.</param>
        /// <returns>Built effect data.</returns>
        public EffectData Compile(params ShaderBytecode[] shaderSource)
        {
            var compiler = new EffectCompilerInternal();
            return compiler.Build(shaderSource);
        }
    }
}