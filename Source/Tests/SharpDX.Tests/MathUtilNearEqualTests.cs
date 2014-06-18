// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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

using System.Runtime.InteropServices;
using NUnit.Framework;
using SharpDX.Mathematics;

namespace SharpDX.Tests
{
    [TestFixture]
    public class MathUtilNearEqualTests
    {
        // 0x4783809e 0x4783809f  (1 ulp)
        [TestCase(67329.234f, 67329.242f, true)]
        // 0x4783809e 0x478380A0  (2 ulp)
        [TestCase(67329.234f, 67329.25f, true)]
        // 0x4783809e 0x478380A1  (3 ulp)
        [TestCase(67329.234f, 67329.257812f, true)]
        // 0x4783809e 0x478380A2  (4 ulp)
        [TestCase(67329.234f, 67329.265625f, true)]
        // 0x4783809e 0x478380A3  (5 ulp)
        [TestCase(67329.234f, 67329.273438f, false)] // should fail. MathUtil.NearEqual is expecting a max ulp of 4

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

        //Float       Hexadecimal Decimal
        //1.99999976    0x3FFFFFFE  1073741822
        //1.99999988    0x3FFFFFFF  1073741823
        //2             0x40000000  1073741824
        //2.00000024    0x40000001  1073741825
        //2.00000048    0x40000002  1073741826
        [TestCase(2.00000000f, 2.00000024f, true)]
        [TestCase(1.99999976f, 2.00000048f, true)]
        public void ShouldReturnCorrectResult(float a, float b, bool expectedResult)
        {
            var result = MathUtil.NearEqual(a, b);

            Assert.AreEqual(expectedResult, result, string.Format("NearEqual({0}, {1}) should be {2}, but was {3}", a, b, expectedResult, result));
        }

        //[StructLayout(LayoutKind.Explicit)]
        //struct Floater
        //{
        //    public Floater(float a)
        //        : this()
        //    {
        //        this.a = a;
        //    }

        //    public Floater(uint aInt)
        //        : this()
        //    {
        //        this.aInt = aInt;
        //    }

        //    [FieldOffset(0)]
        //    public float a;

        //    [FieldOffset(0)]
        //    public uint aInt;
        //}
    }
}