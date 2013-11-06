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

using SharpDX.Toolkit.Graphics;

namespace SharpDX.Toolkit
{
    /// <summary>The model compiler task class.</summary>
    public class ModelCompilerTask : ContentCompilerTask
    {
        /// <summary>Processes the file and get log results.</summary>
        /// <param name="inputFilePath">The input file path.</param>
        /// <param name="outputFilePath">The output file path.</param>
        /// <param name="dependencyFilePath">The dependency file path.</param>
        /// <param name="item">The item.</param>
        /// <returns>Diagnostics.Logger.</returns>
        protected override Diagnostics.Logger ProcessFileAndGetLogResults(string inputFilePath, string outputFilePath, string dependencyFilePath, TkItem item)
        {
            var compilerOptions = new ModelCompilerOptions()
                                  {
                                      DependencyFile = dependencyFilePath,
                                      Quality = Debug ? ModelRealTimeQuality.Low : ModelRealTimeQuality.Maximum
                                  };

            var compilerResult = ModelCompiler.CompileAndSave(inputFilePath, outputFilePath, compilerOptions);

            return compilerResult.Logger;
        }
    }
}