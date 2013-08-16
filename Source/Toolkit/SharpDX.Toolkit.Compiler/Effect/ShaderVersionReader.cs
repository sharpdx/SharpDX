// Copyright (c) 2010-2013 SharpDX - SharpDX Team
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

namespace SharpDX.Toolkit.Graphics
{
    using System;
    using System.Collections.Generic;
    using D3DCompiler;

    /// <summary>
    /// Provides helper method to read the shader version string from the bytecode.
    /// </summary>
    internal static class ShaderVersionReader
    {
        /// <summary>
        /// Gets the shader type and version string from the provided bytecode.
        /// </summary>
        /// <param name="shaderBytecode">The shader bytecode data.</param>
        /// <returns>The type and version string of the provided shader bytecode.</returns>
        /// <exception cref="ArgumentNullException">Is thrown when <paramref name="shaderBytecode"/> is null.</exception>
        /// <exception cref="ArgumentException">Is thrown when bytecode contains invalid data or the version could not be read.</exception>
        /// <exception cref="IndexOutOfRangeException">Is thrown when bytecode contains invalid data.</exception>
        public static string GetVersion(byte[] shaderBytecode)
        {
            if (shaderBytecode == null) throw new ArgumentNullException("shaderBytecode");

            // the offset where chunks count is stored
            const int chunksCountOffset = 4 * 7;
            // the FourCC of the shader chunk which contains the shader version
            const uint shaderCodeChunkFourCc = 0x58454853;
            // the offset of the version bytes from the start of the chunk
            const int shaderCodeVersionOffset = 4 * 2;
            // invalid offset marker - used to find out if the shader version cannot be read
            const int invalidOffset = -1;

            // read the number of data chunks in the bytecode
            var chunksCount = BitConverter.ToUInt32(shaderBytecode, chunksCountOffset);

            // find the offset of the chunk where we can read the data:
            var chunkOffset = invalidOffset;
            for (var i = 0; i < chunksCount; i++)
            {
                var offset = BitConverter.ToInt32(shaderBytecode, chunksCountOffset + 4 + i);
                if (BitConverter.ToUInt32(shaderBytecode, offset) == shaderCodeChunkFourCc)
                {
                    chunkOffset = offset;
                    break;
                }
            }

            if (chunkOffset == invalidOffset)
                throw new ArgumentException("Cannot find the chunk with version in provided bytecode");

            // read the shader version
            // TODO: check shader profiles like "4_0_profile_9_3" - can we read them from bits 8..15?
            var versionValue = BitConverter.ToUInt32(shaderBytecode, chunkOffset + shaderCodeVersionOffset);
            var minor = DecodeValue(versionValue, 0, 3);
            var major = DecodeValue(versionValue, 4, 7);
            var type = DecodeValue(versionValue, 16, 31);

            // find the actual shader type from the decoded number
            string typeText;
            switch (type)
            {
                case 0: typeText = "ps"; break;
                case 1: typeText = "vs"; break;
                case 2: typeText = "gs"; break;
                case 3: typeText = "hs"; break;
                case 4: typeText = "ds"; break;
                case 5: typeText = "cs"; break;
                default: throw new ArgumentException("Cannot read shader type from the provided bytecode");
            }

            return string.Format("{0}_{1}_{2}", typeText, major, minor);
        }

        /// <summary>
        /// Reads the value between start and end bits from the provided token.
        /// </summary>
        /// <param name="token">The source of the data to read.</param>
        /// <param name="start">The start bit.</param>
        /// <param name="end">The end bit.</param>
        /// <returns></returns>
        private static uint DecodeValue(uint token, byte start, byte end)
        {
            // create the mask to read the data
            uint mask = 0;
            for (int i = start; i <= end; i++)
                mask |= (uint)(1 << i);

            // read the needed bits and shift them accordingly
            return (token & mask) >> start;
        }
    }
}