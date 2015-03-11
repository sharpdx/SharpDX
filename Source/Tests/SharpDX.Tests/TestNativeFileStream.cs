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

using System.IO;

using NUnit.Framework;

using SharpDX.IO;

namespace SharpDX.Tests
{
    /// <summary>
    /// Tests for <see cref="NativeFileStream"/>
    /// </summary>
    [TestFixture]
    [Description("Tests SharpDX.IO.NativeFileStrean")]
    public class TestNativeFileStream
    {
        /// <summary>
        /// Filename used for the tests.
        /// </summary>
        private const string Name = "__SharpDX_IO_NativeFileStrean__.txt";

        private bool isTestingNativeFileStream = false;


        /// <summary>
        /// Creates a file stream.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="acccess">The access.</param>
        /// <returns></returns>
        private Stream CreateFileStream(string filename, NativeFileMode mode, NativeFileAccess acccess)
        {
            FileAccess defaultAccess = FileAccess.Read;
            if ((acccess & NativeFileAccess.Read) != 0 )
                defaultAccess = FileAccess.Read;
            if ((acccess & NativeFileAccess.Write) != 0 )
                defaultAccess = FileAccess.Write;
            if ((acccess & NativeFileAccess.ReadWrite) != 0 )
                defaultAccess = FileAccess.ReadWrite;
            return (isTestingNativeFileStream)
                       ? (Stream)new NativeFileStream(Name, mode, acccess)
                       : new FileStream(filename, (FileMode)mode, defaultAccess);
        }

        /// <summary>
        /// Core functions to test a file stream
        /// </summary>
        private void CoreTestAll()
        {
            const int size = 150;
            File.Delete(Name);

            // Create a file
            using (var stream = CreateFileStream(Name, NativeFileMode.Create, NativeFileAccess.Write))
            {
                Assert.True(File.Exists(Name));
                Assert.True(stream.Length == 0);
                for (int i = 0; i < size; i++)
                    stream.WriteByte((byte)i);
                Assert.True(stream.Length == size);
            }
            Assert.True(File.Exists(Name));

            // Loads a file
            using (var stream = CreateFileStream(Name, NativeFileMode.Open, NativeFileAccess.Read))
            {
                // Read to a buffer
                var buffer = new byte[4096];
                var length = stream.Read(buffer, 0, buffer.Length);
                Assert.True(length == size);
                Assert.True(stream.Position == size);

                // Verify the content of the file
                for (int i = 0; i < length; i++)
                    Assert.AreEqual(buffer[i], (byte)i);

                // Seek to the beginning of the file
                stream.Seek(0, SeekOrigin.Begin);
                Assert.True(stream.Position == 0);

                // Seek to the beginning of the file
                stream.Seek(0, SeekOrigin.End);
                Assert.True(stream.Position == stream.Length);

                // Seek to the beginning of the file
                stream.Seek(-1, SeekOrigin.Current);
                Assert.True(stream.Position == stream.Length - 1);

                // Seek to position 0 using property
                stream.Position = 0;
                Assert.True(stream.Position == 0);

                // Read again to the buffer
                length = stream.Read(buffer, 0, buffer.Length);
                Assert.True(length == size);
                Assert.True(stream.Position == size);

                // Try to read again
                length = stream.Read(buffer, 0, buffer.Length);
                Assert.True(length == 0);

                // Rewind
                stream.Position = 0;
                var buffer2 = Utilities.ReadStream(stream);
                Assert.True(buffer2.Length == size);
            }

            // Loads a file
            using (var stream = CreateFileStream(Name, NativeFileMode.Open, NativeFileAccess.ReadWrite))
            {
                stream.Write(new byte[4096], 0, 4096);
                Assert.True(stream.Length == 4096);
                Assert.True(stream.Position == 4096);

                stream.SetLength(256);
                Assert.True(stream.Length == 256);
                Assert.True(stream.Position == 256);

                stream.SetLength(4096);
                Assert.True(stream.Length == 4096);
                Assert.True(stream.Position == 256);
            }

            File.Delete(Name);            
        }


        [Test]
        public void TestNative()
        {
            isTestingNativeFileStream = true;
            CoreTestAll();
        }

        [Test]
        public void VerifyWithFileStream()
        {
            isTestingNativeFileStream = false;
            CoreTestAll();
        }

        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void CheckFileStreamException()
        {            
            var test = new NativeFileStream("blabla", NativeFileMode.Open, NativeFileAccess.Read);
        }


        [Test]
        public void CheckFileExists()
        {
            var testFile = "test.txt";
            File.Delete(testFile);
            Assert.False(NativeFile.Exists(testFile));
            File.WriteAllText(testFile, string.Empty);
            Assert.True(NativeFile.Exists(testFile));
            File.Delete(testFile);
        }
    }
}
