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

using SharpDX.Direct3D;
using SharpDX.Mathematics;

namespace SharpDX.Tests
{
    [TestFixture]
    [Description("Tests SharpDX.Utilities Fast GetterSetter")]
    public class TestGetterSetter
    {
        public enum TestEnum
        {
            None = 0,
            Value1 = 1,
            Value2 = 2
        }


        public class CustomEffect
        {
            public int Int { get; set; }

            public float Float { get; set; }

            public double Double { get; set; }

            public Vector2 Vector2 { get; set; }

            public TestEnum TestEnum { get; set; }

            public Blob Blob { get; set; }
        }

        [Test]
        public unsafe void Test()
        {
            var customEffect = new CustomEffect();

            // Test int
            var getInt = Utilities.BuildPropertyGetter<int>(typeof(CustomEffect), typeof(CustomEffect).GetProperty("Int"));
            var setInt = Utilities.BuildPropertySetter<int>(typeof(CustomEffect), typeof(CustomEffect).GetProperty("Int"));

            int intValue = 5;
            setInt(customEffect, ref intValue);
            getInt(customEffect, out intValue);
            Assert.AreEqual(intValue, 5);

            var getFloat = Utilities.BuildPropertyGetter<float>(typeof(CustomEffect), typeof(CustomEffect).GetProperty("Float"));
            var setFloat = Utilities.BuildPropertySetter<float>(typeof(CustomEffect), typeof(CustomEffect).GetProperty("Float"));

            float floatValue = 5.0f;
            setFloat(customEffect, ref floatValue);
            getFloat(customEffect, out floatValue);
            Assert.AreEqual(floatValue, 5);
            
            var getDouble = Utilities.BuildPropertyGetter<double>(typeof(CustomEffect), typeof(CustomEffect).GetProperty("Double"));
            var setDouble = Utilities.BuildPropertySetter<double>(typeof(CustomEffect), typeof(CustomEffect).GetProperty("Double"));

            double doubleValue = 5.0;
            setDouble(customEffect, ref doubleValue);
            getDouble(customEffect, out doubleValue);
            Assert.AreEqual(doubleValue, 5);

            var getVector2 = Utilities.BuildPropertyGetter<Vector2>(typeof(CustomEffect), typeof(CustomEffect).GetProperty("Vector2"));
            var setVector2 = Utilities.BuildPropertySetter<Vector2>(typeof(CustomEffect), typeof(CustomEffect).GetProperty("Vector2"));

            var Vector2Value = new Vector2(1,2);
            setVector2(customEffect, ref Vector2Value);
            getVector2(customEffect, out Vector2Value);
            Assert.AreEqual(Vector2Value, new Vector2(1, 2));

            var getTestEnum = Utilities.BuildPropertyGetter<int>(typeof(CustomEffect), typeof(CustomEffect).GetProperty("TestEnum"));
            var setTestEnum = Utilities.BuildPropertySetter<int>(typeof(CustomEffect), typeof(CustomEffect).GetProperty("TestEnum"));

            int TestEnumValue = (int)TestEnum.Value2;
            setTestEnum(customEffect, ref TestEnumValue);
            getTestEnum(customEffect, out TestEnumValue);
            Assert.AreEqual(TestEnumValue, (int)TestEnum.Value2);

            var getBlob = Utilities.BuildPropertyGetter<IntPtr>(typeof(CustomEffect), typeof(CustomEffect).GetProperty("Blob"));
            var setBlob = Utilities.BuildPropertySetter<IntPtr>(typeof(CustomEffect), typeof(CustomEffect).GetProperty("Blob"));

            var BlobValue = new IntPtr(5);
            setBlob(customEffect, ref BlobValue);
            getBlob(customEffect, out BlobValue);
            Assert.AreEqual(BlobValue, new IntPtr(5));
        }
         
    }
}