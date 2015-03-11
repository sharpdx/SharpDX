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

using System;
using System.IO;

using NUnit.Framework;

using SharpDX.IO;
using SharpDX.MediaFoundation;
using SharpDX.Mathematics;

namespace SharpDX.Tests
{
    /// <summary>
    /// Tests for <see cref="SharpDX.MediaFoundation.MediaAttributes"/>
    /// </summary>
    [TestFixture]
    [Description("Tests SharpDX.MediaFoundation.MediaAttributes")]
    public class TestMediaAttributes
    {
        [Test]
        public void TestBasicTypes()
        {
            MediaManager.Startup();

            var attributes = new MediaAttributes();

            // 1) Test int
            var guid1 = Guid.NewGuid();
            attributes.Set(guid1, 5);
            Assert.AreEqual(attributes.Get<int>(guid1), 5);

            // 2) Test short
            guid1 = Guid.NewGuid();
            attributes.Set(guid1, (short)5);
            Assert.AreEqual(attributes.Get<short>(guid1), 5);

            // 3) Test uint
            guid1 = Guid.NewGuid();
            attributes.Set(guid1, (uint)6);
            Assert.AreEqual(attributes.Get<uint>(guid1), (uint)6);

            // 4) Test double
            guid1 = Guid.NewGuid();
            attributes.Set(guid1, 5.5);
            Assert.AreEqual(attributes.Get<double>(guid1), 5.5);

            // 5) Test float
            guid1 = Guid.NewGuid();
            attributes.Set(guid1, 5.5f);
            Assert.AreEqual(attributes.Get<float>(guid1), 5.5f);

            // 6) Test Enum
            guid1 = Guid.NewGuid();
            attributes.Set(guid1, MediaEventTypes.BufferingStarted);
            Assert.AreEqual(attributes.Get<MediaEventTypes>(guid1), MediaEventTypes.BufferingStarted);

            // 7) Test long
            guid1 = Guid.NewGuid();
            attributes.Set(guid1, (long)6);
            Assert.AreEqual(attributes.Get<long>(guid1), (long)6);

            // 8) Test ulong
            guid1 = Guid.NewGuid();
            attributes.Set(guid1, (ulong)6);
            Assert.AreEqual(attributes.Get<ulong>(guid1), (ulong)6);

            // 9) Test IntPtr
            guid1 = Guid.NewGuid();
            attributes.Set(guid1, (IntPtr)6);
            Assert.AreEqual(attributes.Get<IntPtr>(guid1), new IntPtr(6));

            // 10) Test string
            guid1 = Guid.NewGuid();
            attributes.Set(guid1, "Toto");
            Assert.AreEqual(attributes.Get<string>(guid1), "Toto");

            // 11) Test guid
            guid1 = Guid.NewGuid();
            attributes.Set(guid1, guid1);
            Assert.AreEqual(attributes.Get<Guid>(guid1), guid1);

            // 12) Test ComObject
            guid1 = Guid.NewGuid();
            attributes.Set(guid1, attributes);
            Assert.AreEqual(attributes.Get<MediaAttributes>(guid1).NativePointer, attributes.NativePointer);

            // 13) Test byte[]
            guid1 = Guid.NewGuid();
            attributes.Set(guid1, new byte[] { 1, 2, 3, 4});
            Assert.AreEqual(attributes.Get<byte[]>(guid1), new byte[] { 1, 2, 3, 4 });

            // 14) Test Vector4
            guid1 = Guid.NewGuid();
            attributes.Set(guid1, new Vector4(1,2,3,4));
            Assert.AreEqual(attributes.Get<Vector4>(guid1), new Vector4(1,2,3,4));

            // Check size of media attributes
            Assert.AreEqual(attributes.Count, 14);

            for (int i = 0; i < attributes.Count; i++)
            {
                object value = attributes.GetByIndex(i, out guid1);
                Console.WriteLine("{0}) {1} ({2})", i, value, value.GetType().Name);
            }
        }
    }
}
