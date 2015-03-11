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
using SharpDX.Mathematics;

namespace SharpDX.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class MathUtilWrapTests
    {
        [TestCase(0, 10, 2, 2)]
        [TestCase(0, 10, 15, 4)]
        [TestCase(0, 10, -15, 7)]
        [TestCase(0, 10, 0, 0)]
        [TestCase(0, 10, 10, 10)]
        [TestCase(0, 0, 10, 0)]
        [TestCase(10, 0, 15, 4, ExpectedException = typeof(ArgumentException))]
        [TestCase(-10, -1, -15, -5)]
        [TestCase(-1, -10, -15, -5, ExpectedException = typeof(ArgumentException))]
        [TestCase(-10, 0, 15, -7)]
        public void WrapsInt(int min, int max, int valueToWrap, int expectedResult)
        {
            var result = MathUtil.Wrap(valueToWrap, min, max);
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(0.0f, 10.0f, 2.0f, 2.0f)]
        [TestCase(0.0f, 10.0f, 15.0f, 5.0f)]
        [TestCase(0.0f, 10.0f, -15.0f, 5.0f)]
        [TestCase(0.0f, 10.0f, 0.0f, 0.0f)]
        [TestCase(0.0f, 10.0f, 10.0f, 0.0f)]
        [TestCase(0.0f, 0.0f, 10.0f, 0.0f)]
        [TestCase(10.0f, 0.0f, 15.0f, 5.0f, ExpectedException = typeof(ArgumentException))]
        [TestCase(-10.0f, -1.0f, -15.0f, -6.0f)]
        [TestCase(-1.0f, -10.0f, -15.0f, -6.0f, ExpectedException = typeof(ArgumentException))]
        [TestCase(-10.0f, 0.0f, 15.0f, -5.0f)]
        [TestCase(0.0f, 0.1f, 10.0f, 0.0f)]
        public void WrapsFloat(float min, float max, float valueToWrap, float expectedResult)
        {
            var result = MathUtil.Wrap(valueToWrap, min, max);
            Assert.True(Math.Abs(expectedResult - result) < 0.00001f ||
            Math.Abs(max - result) < 0.00001f, "Expected [{0}] : Result [{1}]", expectedResult, result);
        }
    }
}