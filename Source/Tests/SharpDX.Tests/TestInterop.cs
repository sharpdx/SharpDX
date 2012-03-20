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
