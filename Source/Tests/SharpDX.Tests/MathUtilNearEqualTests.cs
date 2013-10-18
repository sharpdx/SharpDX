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

namespace SharpDX.Tests
{
    [TestFixture]
    public class MathUtilNearEqualTests
    {
        [TestCase(1f, 1f, true)]
        [TestCase(0f, 1f, false)]
        [TestCase(-1f, 1f, false)]
        [TestCase(-2f, 1f, false)]
        [TestCase(-3f, 1f, false)]
        [TestCase(-4f, 1f, false)]

        [TestCase(0f, 0f, true)]

        [TestCase(5f, 4f, false)]
        [TestCase(4f, 4f, true)]
        [TestCase(3f, 4f, false)]
        [TestCase(2f, 4f, false)]
        [TestCase(1f, 4f, false)]
        [TestCase(0f, 4f, false)]
        [TestCase(-1f, 4f, false)] // this case produces int.MinValue for ulp
        [TestCase(-1f, 5f, false)]

        [TestCase(-1f, -4f, false)]
        public void ShouldReturnCorrectResult(float a, float b, bool expectedResult)
        {
            var result = MathUtil.NearEqual(a, b);

            Assert.AreEqual(expectedResult, result, string.Format("NearEqual({0}, {1}) should be {2}, but was {3}", a, b, expectedResult, result));
        }
    }
}