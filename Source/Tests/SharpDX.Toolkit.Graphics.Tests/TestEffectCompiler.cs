// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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

using System.IO;
using NUnit.Framework;
using SharpDX.Direct3D;

namespace SharpDX.Toolkit.Graphics.Tests
{

    /// <summary>
    /// Tests for all Textures
    /// </summary>
    [TestFixture]
    [Description("Tests SharpDX.Toolkit.Graphics")]
    public class TestEffectCompiler
    {
        [Test]
        public void TestCompiler()
        {
            var fileName = "TestEffect.fx";
            var sourceCode = File.ReadAllText(fileName);
            var result = EffectCompiler.Compile(sourceCode, fileName);

            // Check that we don't have any errors
            Assert.False(result.HasErrors);

            var bytecode = result.Bytecode;

            // Check that we have a single effect compiled in this archive.
            Assert.AreEqual(bytecode.Effects.Count, 1);

            // Check that the name of this effect is the file name
            Assert.AreEqual(bytecode.Effects[0].Name, "TestEffect");

            // We have 3 shaders compiled: VS, VS2, PS
            Assert.AreEqual(bytecode.Shaders.Count, 3);

            // Check that this is the profile 10.0
            Assert.AreEqual(bytecode.Shaders[0].Level, FeatureLevel.Level_10_0);

            // TODO ADD MORE TESTS
        }
    }
}