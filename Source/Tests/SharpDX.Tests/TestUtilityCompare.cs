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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using NUnit.Framework;

namespace SharpDX.Tests
{
    [TestFixture]
    [Description("Tests SharpDX.Utility.Compare methods")]
    public class TestUtilityCompare
    {
        [Test]
        public void TestIEnumerable()
        {
            var arrayFrom = new byte[] { 1, 2, 3, 4 };
            var arrayTo = new byte[] { 1, 2, 3, 4 };
            
            IEnumerator from;
            IEnumerator to;

            // Equal size, Equals
            from = arrayFrom.GetEnumerator();
            to = arrayTo.GetEnumerator();
            Assert.IsTrue(Utilities.Compare(from, to));

            // Check with list
            from = arrayFrom.GetEnumerator();
            var list = new List<byte>() { 1, 2, 3, 4 }.GetEnumerator();
            Assert.IsTrue(Utilities.Compare(from, list));

            // Shorter size for "to"
            from = arrayFrom.GetEnumerator();
            to = new byte[] { 1, 2, 3 }.GetEnumerator();
            Assert.IsFalse(Utilities.Compare(from, to));

            // Shorter size for "from"
            from = new byte[] { 1 }.GetEnumerator();
            to = new byte[] { 1, 2, 3 }.GetEnumerator();
            Assert.IsFalse(Utilities.Compare(from, to));

            // "to" null
            from = new byte[] { 1 }.GetEnumerator();
            to = null;
            Assert.IsFalse(Utilities.Compare(from, to));

            // "from" and "to" null
            from = null;
            to = null;
            Assert.IsTrue(Utilities.Compare(from, to));
        }

        [Test]
        public void TestICollection()
        {
            // Equal size, Equals
            var from = new List<byte> { 1, 2, 3, 4 };
            var to = new List<byte> { 1, 2, 3, 4 };
            Assert.IsTrue(Utilities.Compare(from, to));

            // Shorter size for "to"
            to = new List<byte> { 1, 2, 3 };
            Assert.IsFalse(Utilities.Compare(from, to));

            // Shorter size for "from"
            from = new List<byte> { 1 };
            Assert.IsFalse(Utilities.Compare(from, to));

            // "to" null
            to = null;
            Assert.IsFalse(Utilities.Compare(from, to));

            // "from" and "to" null
            from = null;
            Assert.IsTrue(Utilities.Compare(from, to));
        }
    }
}