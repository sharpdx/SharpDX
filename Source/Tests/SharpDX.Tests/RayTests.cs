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
using SharpDX.Mathematics;

namespace SharpDX.Tests
{
    [TestFixture]
    public class RayTests
    {
        [Test]
        public void WillNotIntersectPlaneIfPointingAway()
        {
            var rayPosition = new Vector3(6.66666651f, 3.33333325f, 3.33333325f);
            var rayDirection = new Vector3(-0.49999997f, 0.49999997f, 0.49999997f);

            var ray = new Ray(rayPosition, rayDirection);
            var plane = new Plane(0.0f, 0.707106769f, 0.707106769f, -3.535534f);

            float distance;
            var result = plane.Intersects(ref ray, out distance);

            Assert.IsFalse(result);
        }
    }
}