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
    public class MathCollisionTest
    {
        [Test]
        public void ClosestPointPointTriangleTest()
        {
            Assert.AreEqual(ClosestPointPointTriangle(new Vector3(0, 0, 0), new Vector3(1, 1, 0), new Vector3(2, 2, 0), new Vector3(3, 1, 0)), new Vector3(1, 1, 0));
            Assert.AreEqual(ClosestPointPointTriangle(new Vector3(1, 1, 0), new Vector3(1, 1, 0), new Vector3(2, 2, 0), new Vector3(3, 1, 0)), new Vector3(1, 1, 0));
            Assert.AreEqual(ClosestPointPointTriangle(new Vector3(2, 3, 0), new Vector3(1, 1, 0), new Vector3(2, 2, 0), new Vector3(3, 1, 0)), new Vector3(2, 2, 0));
            Assert.AreEqual(ClosestPointPointTriangle(new Vector3(4, 1, 0), new Vector3(1, 1, 0), new Vector3(2, 2, 0), new Vector3(3, 1, 0)), new Vector3(3, 1, 0));
            Assert.AreEqual(ClosestPointPointTriangle(new Vector3(1, 2, 0), new Vector3(1, 1, 0), new Vector3(2, 2, 0), new Vector3(3, 1, 0)), new Vector3(1.5f, 1.5f, 0));
            Assert.AreEqual(ClosestPointPointTriangle(new Vector3(3, 2, 0), new Vector3(1, 1, 0), new Vector3(2, 2, 0), new Vector3(3, 1, 0)), new Vector3(2.5f, 1.5f, 0));
            Assert.AreEqual(ClosestPointPointTriangle(new Vector3(2, 0, 0), new Vector3(1, 1, 0), new Vector3(2, 2, 0), new Vector3(3, 1, 0)), new Vector3(2, 1, 0));

            Assert.AreEqual(ClosestPointPointTriangle(new Vector3(2, 1.5f, 1), new Vector3(1, 1, 0), new Vector3(2, 2, 0), new Vector3(3, 1, 0)), new Vector3(2, 1.5f, 0));
        }
        private static Vector3 ClosestPointPointTriangle(Vector3 point, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
        {
            Vector3 result;
            Collision.ClosestPointPointTriangle(ref point, ref vertex1, ref vertex2, ref vertex3, out result);
            return result;
        }
    }
}