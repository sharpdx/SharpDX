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

namespace SharpDX.DirectInput
{
    /// <summary>The joystick class.</summary>
    public class Joystick : CustomDevice<JoystickState, RawJoystickState, JoystickUpdate>
    {
        /// <summary>Initializes a new instance of the <see cref="Joystick"/> class.</summary>
        /// <param name="directInput">The direct input.</param>
        /// <param name="deviceGuid">The device unique identifier.</param>
        public Joystick(DirectInput directInput, Guid deviceGuid) : base(directInput, deviceGuid)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Joystick"/> class.</summary>
        /// <param name="nativePtr">The native PTR.</param>
        public Joystick(IntPtr nativePtr) : base(nativePtr)
        {
        }
    }
}