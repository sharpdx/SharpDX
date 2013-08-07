namespace SharpDX.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class MathUtilWrapTests
    {
        [TestCase(0, 10, 2, 2)]
        [TestCase(0, 10, 15, 5)]
        [TestCase(0, 10, -5, 5)]
        [TestCase(0, 10, 0, 0)]
        [TestCase(0, 10, 10, 10)]
        [TestCase(0, 0, 10, 0)]
        [TestCase(10, 0, 15, 5)]
        [TestCase(-10, -1, -15, -6)]
        [TestCase(-1, -10, -15, -4)]
        public void WrapsInt(int min, int max, int valueToWrap, int expectedResult)
        {
            var result = MathUtil.Wrap(valueToWrap, min, max);

            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(0f, 1.0f, 0.2f, 0.2f)]
        [TestCase(0f, 1.0f, 1.5f, 0.5f)]
        [TestCase(0f, 1.0f, -0.5f, 0.5f)]
        [TestCase(0f, 1.0f, 0f, 0f)]
        [TestCase(0f, 1.0f, 1.0f, 1.0f)]
        [TestCase(0f, 0f, 1.0f, 0f)]
        [TestCase(1.0f, 0f, 1.5f, 0.5f)]
        [TestCase(-1.0f, -0.1f, -1.5f, -0.6f)]
        [TestCase(-0.1f, -1.0f, -1.5f, -0.4f)]
        public void WrapsFloat(float min, float max, float valueToWrap, float expectedResult)
        {
            var result = MathUtil.Wrap(valueToWrap, min, max);

            Assert.That(MathUtil.WithinEpsilon(expectedResult, result, float.Epsilon), string.Format("Expected {0} but was {1}", expectedResult, result));
        }
    }
}