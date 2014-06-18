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
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SharpDX.Toolkit.Input
{
    /// <summary>
    /// Represents the immediate state of keyboard (pressed keys)
    /// </summary>
    /// <remarks>The returned values from member methods require computation - it is advised to cache them when they needs to be reused</remarks>
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct KeyboardState
    {
        [StructLayout(LayoutKind.Explicit)]
        internal struct KeyState
        {
            [FieldOffset(0)] public Keys Key;
            [FieldOffset(1)] public ButtonState State;
        }

        [FieldOffset(0)]
        private KeyState key0;
        [FieldOffset(2)]
        private KeyState key1;
        [FieldOffset(4)]
        private KeyState key2;
        [FieldOffset(6)]
        private KeyState key3;
        [FieldOffset(8)]
        private KeyState key4;
        [FieldOffset(10)]
        private KeyState key5;
        [FieldOffset(12)]
        private KeyState key6;
        [FieldOffset(14)]
        private KeyState key7;

        /// <summary>
        /// Gets the state of specified key
        /// </summary>
        /// <remarks>Cache the returned value if it needs to be reused</remarks>
        /// <param name="key">A <see cref="Keys"/> to check whether it is pressed or not</param>
        /// <returns>The state of a key.</returns>
        public ButtonState this[Keys key]
        {
            get
            {
                return GetState(key);
            }
            set
            {
                SetState(key, value);
            }
        }

        /// <summary>
        /// Checks if the specified key is being pressed
        /// </summary>
        /// <remarks>Cache the returned value if it needs to be reused</remarks>
        /// <param name="key">A <see cref="Keys"/> to check whether it is pressed or not</param>
        /// <returns>True if the specified key is being pressed; False - otherwise</returns>
        public bool IsKeyDown(Keys key)
        {
            return GetState(key).Down;
        }

        /// <summary>
        /// Checks if the specified key has been pressed since last frame
        /// </summary>
        /// <remarks>Cache the returned value if it needs to be reused</remarks>
        /// <param name="key">A <see cref="Keys"/> to check whether it is pressed or not</param>
        /// <returns>True if the key is pressed; False - otherwise</returns>
        public bool IsKeyPressed(Keys key)
        {
            return GetState(key).Pressed;
        }

        /// <summary>
        /// Checks if the specified key has been released since last frame.
        /// </summary>
        /// <remarks>Cache the returned value if it needs to be reused</remarks>
        /// <param name="key">A <see cref="Keys"/> to check whether if the specified key has been released since last frame</param>
        /// <returns>True if the specified key has been released since last frame; False - otherwise</returns>
        public bool IsKeyReleased(Keys key)
        {
            return GetState(key).Released;
        }

        /// <summary>
        /// Gets an array with all keys down.
        /// </summary>
        /// <param name="keys">The list of keys that will received keys being pressed.</param>
        /// <exception cref="System.ArgumentNullException">keys</exception>
        /// <remarks>This method clears the list before appending</remarks>
        public unsafe void GetDownKeys(List<Keys> keys)
        {
            if(keys == null) throw new ArgumentNullException("keys");
            keys.Clear();
            fixed (void* keysRawPtr = &key0)
            {
                var keysPtr = (KeyState*)keysRawPtr;
                for (int i = 0; i < 8; i++)
                {
                    if (keysPtr->State.Down)
                    {
                        keys.Add(keysPtr->Key);
                    }
                    keysPtr++;
                }
            }
        }

        private unsafe ButtonState GetState(Keys key)
        {
            fixed (void* keysPtr = &key0)
            {
                var keys = (KeyState*)keysPtr;
                for (int i = 0; i < 8; i++)
                {
                    if (keys->Key == key)
                    {
                        return keys->State;
                    }
                    keys++;
                }
            }
            return default(ButtonState);
        }

        internal unsafe void ResetPressedReleased()
        {
            fixed (void* keysPtr = &key0)
            {
                var keys = (KeyState*)keysPtr;
                for (int i = 0; i < 8; i++)
                {
                    keys->State &= ~(ButtonStateFlags.Pressed | ButtonStateFlags.Released);
                    if(keys->State == ButtonStateFlags.None)
                    {
                        keys->Key = Keys.None;
                    }
                    keys++;
                }
            }
        }

        private unsafe void SetState(Keys key, ButtonState state)
        {
            fixed (void* keysPtr = &key0)
            {
                var keys = (KeyState*)keysPtr;
                var firstNullKey = (KeyState*)0;
                for (int i = 0; i < 8; i++)
                {
                    if (keys->Key == key)
                    {
                        keys->State = state;
                        if(state == ButtonStateFlags.None)
                        {
                            keys->Key = Keys.None;
                        }
                        return;
                    }

                    if (keys->Key == Keys.None && firstNullKey == (KeyState*)0)
                    {
                        firstNullKey = keys;
                    }
                    keys++;
                }

                if (firstNullKey != (KeyState*)0 && state != ButtonStateFlags.None)
                {
                    firstNullKey->Key = key;
                    firstNullKey->State = state;
                }
            }            
        }
    }
}