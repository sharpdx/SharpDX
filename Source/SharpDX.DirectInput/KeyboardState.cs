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

namespace SharpDX.DirectInput
{
    public class KeyboardState : IDeviceState<RawKeyboardState, KeyboardUpdate>
    {
        private static readonly List<Key> _allKeys = new List<Key>(256);

        static KeyboardState()
        {
            foreach(var key in Enum.GetValues(typeof(Key)))
                _allKeys.Add((Key)key);
        }

        public KeyboardState()
        {
            PressedKeys = new List<Key>(16);
        }

        public List<Key> AllKeys
        {
            get { return _allKeys; }
        }

        public List<Key> PressedKeys { get; private set; }
        
        public bool IsPressed(Key key)
        {
            return PressedKeys.Contains(key);
        }

        public void Update(KeyboardUpdate update)
        {
            if (update.Key == Key.Unknown)
                return;

            bool isPreviousPressed = IsPressed(update.Key);
            if (update.IsPressed && !isPreviousPressed)
                PressedKeys.Add(update.Key);
            else if (update.IsReleased && isPreviousPressed)
                PressedKeys.Remove(update.Key);
        }

        public void MarshalFrom(ref RawKeyboardState value)
        {
            PressedKeys.Clear();

            unsafe
            {
                var update = new KeyboardUpdate();
                
                fixed (byte* pRawKeys = value.Keys)
                    for (int i = 0; i < 256; i++)
                    {
                        update.rawOffset = i;
                        update.value = pRawKeys[i];
                        //if (update.Key == Key.Unknown)
                        //    continue;
                        
                        if (update.IsPressed)
                            PressedKeys.Add(update.Key);
                    }
            }
        }

        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "PressedKeys: {0}", Utilities.Join(",", PressedKeys));
        }
    }
}