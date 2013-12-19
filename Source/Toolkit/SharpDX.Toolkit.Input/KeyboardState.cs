// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyboardState
    {
        internal KeysSet KeysDown;
        internal KeysSet KeysPressed;
        internal KeysSet KeysReleased;

        /// <summary>
        /// Gets the state of specified key
        /// </summary>
        /// <remarks>Cache the returned value if it needs to be reused</remarks>
        /// <param name="key">A <see cref="Keys"/> to check whether it is pressed or not</param>
        /// <returns>The state of a key.</returns>
        public KeyState this[Keys key]
        {
            get
            {
                var state = KeyState.None;
                if (KeysDown.HasKey(key)) state |= KeyState.Down;
                if (KeysPressed.HasKey(key)) state |= KeyState.Pressed;
                if (KeysReleased.HasKey(key)) state |= KeyState.Released;
                return state;
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
            return KeysDown.HasKey(key);
        }

        /// <summary>
        /// Checks if the specified key has been pressed since last frame
        /// </summary>
        /// <remarks>Cache the returned value if it needs to be reused</remarks>
        /// <param name="key">A <see cref="Keys"/> to check whether it is pressed or not</param>
        /// <returns>True if the key is pressed; False - otherwise</returns>
        public bool IsKeyPressed(Keys key)
        {
            return KeysPressed.HasKey(key);
        }

        /// <summary>
        /// Checks if the specified key has been released since last frame.
        /// </summary>
        /// <remarks>Cache the returned value if it needs to be reused</remarks>
        /// <param name="key">A <see cref="Keys"/> to check whether if the specified key has been released since last frame</param>
        /// <returns>True if the specified key has been released since last frame; False - otherwise</returns>
        public bool IsKeyReleased(Keys key)
        {
            return KeysReleased.HasKey(key);
        }

        /// <summary>
        /// Gets an array with all keys down.
        /// </summary>
        /// <param name="keys">The list of keys that will received keys being pressed.</param>
        /// <exception cref="System.ArgumentNullException">keys</exception>
        /// <remarks>This method clears the list before appending</remarks>
        public void GetDownKeys(List<Keys> keys)
        {
            if(keys == null) throw new ArgumentNullException("keys");
            keys.Clear();
            KeysDown.ListKeys(keys);
        }
    }
}