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

using NUnit.Framework;

namespace SharpDX.Toolkit.Graphics.Tests
{

    /// <summary>
    /// Tests for Matrix Array in fx
    /// </summary>
    [TestFixture]
    public class TestEffectMatrix
    {
        [Test]
        public void Test()
        {
            var device = GraphicsDevice.New();

            // Compile a toolkit effect from a file
            var result = new EffectCompiler().CompileFromFile("TestEffectMatrix.fx");

            // Check that we don't have any errors
            Assert.False(result.HasErrors);

            var bytecode = result.EffectData;

            var effect = new Effect(device, bytecode);

            
            var matrices = new Matrix[4];
            for (int i = 0; i < matrices.Length; i++)
            {
                matrices[i] = new Matrix();
                for (int j = 0; j < 16; j++)
                {
                    matrices[i][j] = (j + i * 16);
                }
            }

            effect.Parameters["Matrices4x4"].SetValue(matrices);
            effect.Parameters["Matrices4x3"].SetValue(matrices);
            effect.Parameters["Matrices3x3"].SetValue(matrices);
            effect.Parameters["Matrices3x4"].SetValue(matrices);

            var locals4x4 = effect.Parameters["Matrices4x4"].GetMatrixArray(4);
            var locals4x3 = effect.Parameters["Matrices4x3"].GetMatrixArray(4);
            var locals3x3 = effect.Parameters["Matrices3x3"].GetMatrixArray(4);
            var locals3x4 = effect.Parameters["Matrices3x4"].GetMatrixArray(4);

            Assert.That(CompareMatrixArray(matrices, locals4x4, 4, 4), Is.True);
            Assert.That(CompareMatrixArray(matrices, locals4x3, 4, 3), Is.True);
            Assert.That(CompareMatrixArray(matrices, locals3x3, 3, 3), Is.True);
            Assert.That(CompareMatrixArray(matrices, locals3x4, 3, 4), Is.True);

            var local4x4 = effect.Parameters["Matrices4x4"].GetMatrix(3);
            var local4x3 = effect.Parameters["Matrices4x3"].GetMatrix(3);
            var local3x3 = effect.Parameters["Matrices3x3"].GetMatrix(3);
            var local3x4 = effect.Parameters["Matrices3x4"].GetMatrix(3);

            Assert.That(CompareMatrix(ref local4x4, ref matrices[3], 4, 4));
            Assert.That(CompareMatrix(ref local4x3, ref matrices[3], 4, 3));
            Assert.That(CompareMatrix(ref local3x3, ref matrices[3], 3, 3));
            Assert.That(CompareMatrix(ref local3x4, ref matrices[3], 3, 4));

            var buffer = Buffer.Structured.New<Matrix>(device, 4, true);
            effect.Parameters["BufferOut"].SetResource(buffer);

            effect.CurrentTechnique.Passes[0].Apply();
            device.Dispatch(4, 1, 1);
            var compute4x4 = Transpose(buffer.GetData<Matrix>());
            Assert.That(CompareMatrixArray(matrices, compute4x4, 4, 4), Is.True);

            effect.CurrentTechnique.Passes[1].Apply();
            device.Dispatch(4, 1, 1);
            var compute4x3 = Transpose(buffer.GetData<Matrix>());
            Assert.That(CompareMatrixArray(matrices, compute4x3, 4, 3), Is.True);

            effect.CurrentTechnique.Passes[2].Apply();
            device.Dispatch(4, 1, 1);
            var compute3x3 = Transpose(buffer.GetData<Matrix>());
            Assert.That(CompareMatrixArray(matrices, compute3x3, 3, 3), Is.True);

            effect.CurrentTechnique.Passes[3].Apply();
            device.Dispatch(4, 1, 1);
            var compute3x4 = Transpose(buffer.GetData<Matrix>());
            Assert.That(CompareMatrixArray(matrices, compute3x4, 3, 4), Is.True);

            effect.Dispose();
            device.Dispose();
        }

        private bool CompareMatrixArray(Matrix[] left, Matrix[] right, int rowCount, int colCount)
        {
            if (left.Length != right.Length) return false;

            for (int i = 0; i < left.Length; i++)
            {
                if (!CompareMatrix(ref left[i], ref right[i], rowCount, colCount))
                {
                    return false;
                }
            }
            return true;
        }

        private Matrix[] Transpose(Matrix[] input)
        {
            for(int i = 0; i < input.Length; i++)
                input[i].Transpose();
            return input;
        }

        private bool CompareMatrix(ref Matrix left, ref Matrix right, int rowCount, int colCount)
        {
            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    if (!MathUtil.WithinEpsilon(left[row, col], right[row, col]))
                    {
                        return false;

                    }
                }
            }
            return true;
        }
    }
}