// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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
using System.Collections.Generic;

namespace SharpDX.Toolkit.Input
{
    /// <summary>
    /// Represents the immediate state of keyboard (pressed keys)
    /// </summary>
    /// <remarks>The returned values from member methods require computation - it is advised to cache them when they needs to be reused</remarks>
    public struct KeyboardState
    {
        // The key bit flag storage method was inspired from MonoGame and ExEn engines
        
        /// <summary>
        /// Represents information about where should be stored information about a key.
        /// Created to avoid code duplication.
        /// </summary>
        private struct KeyInfo
        {
            /// <summary>
            /// Index of 32-bit chunk where the key flag is stored
            /// </summary>
            internal readonly int ChunkIndex;

            /// <summary>
            /// Index of flag in the chunk
            /// </summary>
            internal readonly uint KeyBitFlagIndex;

            /// <summary>
            /// Initializes a new instance of <see cref="KeyInfo"/> structure
            /// </summary>
            /// <param name="key">The key whose storage information needs to be computed.</param>
            public KeyInfo(Keys key)
            {
                // higher 3 bits encode chunk index
                ChunkIndex = ((byte)key) >> 5;

                // lower 5 bits encode flag index in chunk
                KeyBitFlagIndex = (uint)1 << (((int)key) & 0x1f);
            }
        }

        private static readonly Keys[] Empty = new Keys[0];

        // Count of chunks in data storage array
        private const int dataChunksCount = 8;

        // Data chunks. Do not use arrays as they are reference types and will generate garbage
        private uint data0;
        private uint data1;
        private uint data2;
        private uint data3;
        private uint data4;
        private uint data5;
        private uint data6;
        private uint data7;

        /// <summary>
        /// Initializes a new instance of <see cref="KeyboardState"/> structure
        /// </summary>
        /// <param name="pressedKeys">a set of keys which are pressed</param>
        /// <exception cref="ArgumentNullException">Is thrown when <paramref name="pressedKeys"/> is null</exception>
#if WinFormsInterop && !NET35Plus
        internal KeyboardState(List<Keys> pressedKeys)
#else
        internal KeyboardState(HashSet<Keys> pressedKeys)
#endif
        {
            // do not use IEnumerable<Keys> as parameter - because it involves either class creation or boxing
            // during enumeration and generates garbage
            if (pressedKeys == null) throw new ArgumentNullException("pressedKeys");

            data0 = 0;
            data1 = 0;
            data2 = 0;
            data3 = 0;
            data4 = 0;
            data5 = 0;
            data6 = 0;
            data7 = 0;

            // HashSet<T>.Enumerator is a struct, so this loop will not generate garbage
            foreach (var key in pressedKeys)
                SetKeyDown(key);
        }

        /// <summary>
        /// Gets the state of specified key
        /// </summary>
        /// <remarks>Cache the returned value if it needs to be reused</remarks>
        /// <param name="key">A <see cref="Keys"/> to check whether it is pressed or not</param>
        /// <returns><see cref="KeyState.Down"/> if the <paramref name="key"/> is pressed; <see cref="KeyState.Up"/> otherwise.</returns>
        public KeyState this[Keys key]
        {
            get { return IsKeyDown(key) ? KeyState.Down : KeyState.Up; }
        }

        /// <summary>
        /// Checks it the specified key is not pressed
        /// </summary>
        /// <remarks>Cache the returned value if it needs to be reused</remarks>
        /// <param name="key">A <see cref="Keys"/> to check whether it is pressed or not</param>
        /// <returns>True if the key is not pressed; False - otherwise</returns>
        public bool IsKeyUp(Keys key)
        {
            return !IsKeyDown(key);
        }

        /// <summary>
        /// Checks it the specified key is pressed
        /// </summary>
        /// <remarks>Cache the returned value if it needs to be reused</remarks>
        /// <param name="key">A <see cref="Keys"/> to check whether it is pressed or not</param>
        /// <returns>True if the key is pressed; False - otherwise</returns>
        private bool IsKeyDown(Keys key)
        {
            var info = new KeyInfo(key);
            uint chunk;
            switch (info.ChunkIndex)
            {
            case 0: chunk = data0; break;
            case 1: chunk = data1; break;
            case 2: chunk = data2; break;
            case 3: chunk = data3; break;
            case 4: chunk = data4; break;
            case 5: chunk = data5; break;
            case 6: chunk = data6; break;
            case 7: chunk = data7; break;
            default: chunk = 0; break;
            }

            return (chunk & info.KeyBitFlagIndex) != 0;
        }

        /// <summary>
        /// Gets an array with all pressed keys from this instance
        /// </summary>
        /// <remarks>Cache the returned value if it needs to be reused</remarks>
        /// <returns>An array of all pressed keys</returns>
        public Keys[] GetPressedKeys()
        {
            var count = CountSetBits(data0) + CountSetBits(data1) + CountSetBits(data2) + CountSetBits(data3) + CountSetBits(data4) + CountSetBits(data5) + CountSetBits(data6) + CountSetBits(data7);
            if (count == 0)
                return Empty;

            var keys = new Keys[count];

            var index = 0;
            AddKeysToArray(data0, 0 * 32, keys, ref index);
            AddKeysToArray(data1, 1 * 32, keys, ref index);
            AddKeysToArray(data2, 2 * 32, keys, ref index);
            AddKeysToArray(data3, 3 * 32, keys, ref index);
            AddKeysToArray(data4, 4 * 32, keys, ref index);
            AddKeysToArray(data5, 5 * 32, keys, ref index);
            AddKeysToArray(data6, 6 * 32, keys, ref index);
            AddKeysToArray(data7, 7 * 32, keys, ref index);

            return keys;
        }

        /// <summary>
        /// Sets the specified key as pressed
        /// </summary>
        /// <param name="key">A <see cref="Keys"/> which state needs to be set as pressed</param>
        private void SetKeyDown(Keys key)
        {
            var info = new KeyInfo(key);
            switch (info.ChunkIndex)
            {
                case 0: data0 |= info.KeyBitFlagIndex; break;
                case 1: data1 |= info.KeyBitFlagIndex; break;
                case 2: data2 |= info.KeyBitFlagIndex; break;
                case 3: data3 |= info.KeyBitFlagIndex; break;
                case 4: data4 |= info.KeyBitFlagIndex; break;
                case 5: data5 |= info.KeyBitFlagIndex; break;
                case 6: data6 |= info.KeyBitFlagIndex; break;
                case 7: data7 |= info.KeyBitFlagIndex; break;
            }
        }

        /// <summary>
        /// Counts how many bits are set in provided value
        /// </summary>
        /// <param name="chunk">The value whose bits should be counted</param>
        /// <returns>The count of bits with value 1</returns>
        private static uint CountSetBits(uint chunk)
        {
            // http://graphics.stanford.edu/~seander/bithacks.html#CountBitsSetParallel
            chunk = chunk - ((chunk >> 1) & 0x55555555);
            chunk = (chunk & 0x33333333) + ((chunk >> 2) & 0x33333333);
            return ((chunk + (chunk >> 4) & 0xF0F0F0F) * 0x1010101) >> 24;
        }

        /// <summary>
        /// Recomputes the <see cref="Keys"/> value from a bit in specified <paramref name="chunk"/>,
        /// using specified <paramref name="arrayOffset"/> and increases the <paramref name="index"/>
        /// </summary>
        /// <param name="chunk">The chunk from whose bits should be recreated the <see cref="Keys"/> values</param>
        /// <param name="arrayOffset">The offset of bit (0-256)</param>
        /// <param name="pressedKeys">The destination array</param>
        /// <param name="index">The index of element in array. Increased for evey new element</param>
        private static void AddKeysToArray(uint chunk, int arrayOffset, Keys[] pressedKeys, ref int index)
        {
            if (chunk == 0) return;

            for (var i = 0; i < 32; i++)
            {
                if ((chunk & (1 << i)) != 0)
                    pressedKeys[index++] = (Keys)(arrayOffset + i);
            }
        }
    }
}