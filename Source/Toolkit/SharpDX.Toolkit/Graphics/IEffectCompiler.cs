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
using System.Collections.Generic;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Interface to compile an effect.
    /// </summary>
    public interface IEffectCompiler
    {
        /// <summary>
        /// Gets the dependency filename from the (fx) effect path.
        /// </summary>
        /// <param name="pathToFxFile">The (fx) effect path.</param>
        /// <returns>A dependency filename.</returns>
        string GetDependencyFileNameFromSourcePath(string pathToFxFile);

        /// <summary>
        /// Loads a dependency file.
        /// </summary>
        /// <param name="dependencyFilePath">The dependency file path.</param>
        /// <returns>A list of file path.</returns>
        List<string> LoadDependency(string dependencyFilePath);

        /// <summary>
        /// Checks for changes from a dependency file.
        /// </summary>
        /// <param name="dependencyFilePath">The dependency file path.</param>
        /// <returns><c>true</c> if a file has been updated, <c>false</c> otherwise</returns>
        bool CheckForChanges(string dependencyFilePath);

        /// <summary>
        /// Compiles an effect from file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="macros">The macrosArgs.</param>
        /// <param name="includeDirectoryList">The include directory list.</param>
        /// <returns>The result of compilation.</returns>
        EffectCompilerResult CompileFromFile(string filePath, EffectCompilerFlags flags = EffectCompilerFlags.None, List<EffectData.ShaderMacro> macros = null, List<string> includeDirectoryList = null, bool alloDynamicCompiling = false, string dependencyFilePath = null);

        /// <summary>
        /// Compiles an effect from the specified source code and file path.
        /// </summary>
        /// <param name="sourceCode">The source code.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="macrosArgs">The macrosArgs.</param>
        /// <param name="includeDirectoryList">The include directory list.</param>
        /// <returns>The result of compilation.</returns>
        EffectCompilerResult Compile(string sourceCode, string filePath, EffectCompilerFlags flags = EffectCompilerFlags.None, List<EffectData.ShaderMacro> macrosArgs = null, List<string> includeDirectoryList = null, bool allowDynamicCompiling = false, string dependencyFilePath = null);

        /// <summary>
        /// Disassembles a shader HLSL bytecode to asm code.
        /// </summary>
        /// <param name="shader">The shader.</param>
        /// <returns>A string containing asm code decoded from HLSL bytecode.</returns>
        string DisassembleShader(EffectData.Shader shader);
    }
}