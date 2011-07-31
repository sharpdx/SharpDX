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

namespace SharpDX.Multimedia
{
    /// <summary>
    /// A FourCC descriptor.
    /// </summary>
    public struct FourCC
    {
        private string value;

        /// <summary>
        /// Gets the value.
        /// </summary>
        public string Value
        {
            get { return value; }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SharpDX.Multimedia.FourCC"/> to <see cref="System.Int32"/>.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator uint(FourCC d)
        {
            return ToFourCC(d.Value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SharpDX.Multimedia.FourCC"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator string(FourCC d)
        {
            return d.Value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="SharpDX.Multimedia.FourCC"/>.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator FourCC(string d)
        {
            return new FourCC { value = d };
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Int32"/> to <see cref="SharpDX.Multimedia.FourCC"/>.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator FourCC(uint d)
        {
            return FromFourCC(d);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Value;
        }

        /// <summary>
        /// Converts a FourCC integer to a string
        /// </summary>
        /// <param name="fourCC">The FourCC.</param>
        /// <returns>A FourCC string</returns>
        private static string FromFourCC(uint fourCC)
        {
            return new string(new []
                               {
                                   (char) (fourCC & 0xFF),
                                   (char) ((fourCC >> 8) & 0xFF),
                                   (char) ((fourCC >> 16) & 0xFF),
                                   (char) ((fourCC >> 24) & 0xFF),
                               });
        }

        /// <summary>
        /// Convert a FourCC string to int
        /// </summary>
        /// <param name="fourCC">The fourCC.</param>
        /// <returns>an integer version of the FourCC</returns>
        private static uint ToFourCC(string fourCC)
        {
            if (fourCC.Length != 4)
                throw new ArgumentException(string.Format("Invalid length for FourCC(\"{0}\". Must be be 4 characters long ", fourCC), "fourCC");
            return ((uint)fourCC[3]) << 24 | ((uint)fourCC[2]) << 16 | ((uint)fourCC[1]) << 8 | ((uint)fourCC[0]);
        }
    }
}