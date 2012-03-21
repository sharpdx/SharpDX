// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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

using NUnit.Framework;

namespace SharpDX.Tests
{
    /// <summary>
    /// Tests for <see cref="SharpDX.Interop"/>
    /// </summary>
    [TestFixture]
    [Description("Tests SharpDX.Interop")]
    public class TestInterop
    {
        [Test]
        public unsafe void TestCast()
        {
            var fromMatrix = new RawMatrix
                {
                    Row1 = new RawVector4 { X = 1.0f, Y = 2.0f, Z = 3.0f, W = 4.0f }, 
                    Row4 = new RawVector4 { X = 5.0f, Y = 6.0f, Z = 7.0f, W = 8.0f }
                };

            // Cast RawMatrix to Matrix structure
            Matrix toMatrix = *(Matrix*)Interop.Cast(ref fromMatrix);

            Assert.True(toMatrix.Row1 == new Vector4 { X = 1.0f, Y = 2.0f, Z = 3.0f, W = 4.0f });
            Assert.True(toMatrix.Row4 == new Vector4 { X = 5.0f, Y = 6.0f, Z = 7.0f, W = 8.0f });
        }

        [Test]
        public unsafe void TestCpBlk()
        {
            // Assume that allocation is linear on the stack, and matrix1 is located 
            // between matrix0 & matrix2
            var matrix0 = new Matrix
                {
                    Row1 = new Vector4 { X = 1.0f, Y = 2.0f, Z = 3.0f, W = 4.0f },
                    Row4 = new Vector4 { X = 5.0f, Y = 6.0f, Z = 7.0f, W = 8.0f },
                };
            var matrix1 = new Matrix();

            // Clear matrix1 only. matrix0 and matrix2 should not be modified
            Utilities.CopyMemory(new IntPtr(&matrix1), new IntPtr(&matrix0), Utilities.SizeOf<Matrix>());

            Assert.True(matrix1.Row1 == new Vector4 { X = 1.0f, Y = 2.0f, Z = 3.0f, W = 4.0f });
            Assert.True(matrix1.Row4 == new Vector4 { X = 5.0f, Y = 6.0f, Z = 7.0f, W = 8.0f });
        }

        [Test]
        public unsafe void TestInitBlk()
        {
            // Assume that allocation is linear on the stack, and matrix1 is located 
            // between matrix0 & matrix2
            var matrix0 = new Matrix { M44 = 1.0f };
            var matrix1 = new Matrix() { M11 = 2.0f };
            var matrix2 = new Matrix { M11 = 1.0f };

            // Clear matrix1 only. matrix0 and matrix2 should not be modified
            Utilities.ClearMemory(new IntPtr(&matrix1), 0, Utilities.SizeOf<Matrix>());

            Assert.True(matrix0.M44 == 1.0f);
            Assert.True(matrix1.M11 == 0.0f);
            Assert.True(matrix2.M11 == 1.0f);
        }

        [Test]
        public unsafe void TestCastArray()
        {
            var fromMatrices = new RawMatrix[] { new RawMatrix
            {
                Row1 = new RawVector4 { X = 1.0f, Y = 2.0f, Z = 3.0f, W = 4.0f },
                Row4 = new RawVector4 { X = 5.0f, Y = 6.0f, Z = 7.0f, W = 8.0f }
            }};

            // Cast RawMatrix[] to Matrix[]
            var toMatrices = Interop.CastArray<Matrix, RawMatrix>(fromMatrices);

            // Check validity
            Assert.True(toMatrices != null);
            Assert.True(fromMatrices.Length == toMatrices.Length);

            // Check that access is possible
            ValidateArray(toMatrices);
        }

        private void ValidateArray(Matrix[] values)
        {
            Assert.True(values[0].Row1 == new Vector4 { X = 1.0f, Y = 2.0f, Z = 3.0f, W = 4.0f });
            Assert.True(values[0].Row4 == new Vector4 { X = 5.0f, Y = 6.0f, Z = 7.0f, W = 8.0f });            
        }

    }
}
