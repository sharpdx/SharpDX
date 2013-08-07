using System;

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
        [TestCase(10, 0, 15, 4)]
        [TestCase(-10, -1, -15, -5)]
        [TestCase(-1, -10, -15, -5)]
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
        [TestCase(10.0f, 0.0f, 15.0f, 5.0f)]
        [TestCase(-10.0f, -1.0f, -15.0f, -6.0f)]
        [TestCase(-1.0f, -10.0f, -15.0f, -6.0f)]
        [TestCase(-10.0f, 0.0f, 15.0f, -5.0f)]
        public void WrapsFloat(float min, float max, float valueToWrap, float expectedResult)
        {
            var result = MathUtil.Wrap(valueToWrap, min, max);
            Assert.True(MathUtil.WithinEpsilon(expectedResult, result), "Expected [{0}] : Result [{1}]", expectedResult, result);
        }
    }
}