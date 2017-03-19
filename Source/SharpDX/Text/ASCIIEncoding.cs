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
namespace SharpDX.Text
{
    /// <summary>
    /// Overrides <see cref="System.Text.ASCIIEncoding"/> in order to provide <see cref="ASCIIEncoding"/> for Win8 Modern App.
    /// </summary>
    public abstract class Encoding : System.Text.Encoding
    {
#if NETSTANDARD1_1
        /// <summary>
        /// Returns an encoding for the ASCII character set. The returned encoding
        ///  will be an instance of the ASCIIEncoding class.
        /// </summary>
        public static readonly System.Text.Encoding ASCII = new ASCIIEncoding();
#endif
    }
#if NETSTANDARD1_1
    /// <summary>
    /// Provides a basic implementation to replace <see cref="System.Text.ASCIIEncoding"/> (not available on Win8 Modern App).
    /// </summary>
    public class ASCIIEncoding : Encoding 
    {
        public override int GetByteCount(char[] chars, int index, int count)
        {
            return count;
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            for (int i = 0; i < charCount; i++)
                bytes[byteIndex + i] = (byte)(chars[charIndex + i] & 0x7F);
            return charCount;
        }

        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            return count;
        }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            for (int i = 0; i < byteCount; i++)
                chars[charIndex + i] = (char)(bytes[byteIndex + i] & 0x7F);
            return byteCount;
        }

        public override int GetMaxByteCount(int charCount)
        {
            return charCount;
        }

        public override int GetMaxCharCount(int byteCount)
        {
            return byteCount;
        }

        public string GetString(byte[] bytes)
        {
            return base.GetString(bytes, 0, bytes.Length);
        }
    }
#endif
}