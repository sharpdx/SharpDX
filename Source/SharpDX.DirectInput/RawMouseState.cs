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

using System.Runtime.InteropServices;

namespace SharpDX.DirectInput
{
    /// <summary>The raw mouse state struct.</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    [DataFormat(DataFormatFlag.RelativeAxis)]
    public struct RawMouseState
    {
        /// <summary>The position X.</summary>
        [DataObjectFormat(Guid = ObjectGuid.XAxisStr, TypeFlags = DeviceObjectTypeFlags.RelativeAxis | DeviceObjectTypeFlags.AbsoluteAxis | DeviceObjectTypeFlags.AnyInstance)]
        internal int X;
        /// <summary>The position Y.</summary>
        [DataObjectFormat(Guid = ObjectGuid.YAxisStr, TypeFlags = DeviceObjectTypeFlags.RelativeAxis | DeviceObjectTypeFlags.AbsoluteAxis | DeviceObjectTypeFlags.AnyInstance)]
        internal int Y;
        /// <summary>The position Z.</summary>
        [DataObjectFormat(Guid = ObjectGuid.ZAxisStr, TypeFlags = DeviceObjectTypeFlags.RelativeAxis | DeviceObjectTypeFlags.AbsoluteAxis | DeviceObjectTypeFlags.AnyInstance | DeviceObjectTypeFlags.Optional)]
        internal int Z;

        /// <summary>The button 0.</summary>
        [DataObjectFormat(DeviceObjectTypeFlags.PushButton | DeviceObjectTypeFlags.ToggleButton | DeviceObjectTypeFlags.AnyInstance)] 
        internal byte Buttons0;

        /// <summary>The button 1.</summary>
        [DataObjectFormat(DeviceObjectTypeFlags.PushButton | DeviceObjectTypeFlags.ToggleButton | DeviceObjectTypeFlags.AnyInstance)]
        internal byte Buttons1;

        /// <summary>The button 2.</summary>
        [DataObjectFormat(DeviceObjectTypeFlags.PushButton | DeviceObjectTypeFlags.ToggleButton | DeviceObjectTypeFlags.AnyInstance | DeviceObjectTypeFlags.Optional)]
        internal byte Buttons2;

        /// <summary>The button 3.</summary>
        [DataObjectFormat(DeviceObjectTypeFlags.PushButton | DeviceObjectTypeFlags.ToggleButton | DeviceObjectTypeFlags.AnyInstance | DeviceObjectTypeFlags.Optional)]
        internal byte Buttons3;

        /// <summary>The button 4.</summary>
        [DataObjectFormat(DeviceObjectTypeFlags.PushButton | DeviceObjectTypeFlags.ToggleButton | DeviceObjectTypeFlags.AnyInstance | DeviceObjectTypeFlags.Optional)]
        internal byte Buttons4;

        /// <summary>The button 5.</summary>
        [DataObjectFormat(DeviceObjectTypeFlags.PushButton | DeviceObjectTypeFlags.ToggleButton | DeviceObjectTypeFlags.AnyInstance | DeviceObjectTypeFlags.Optional)]
        internal byte Buttons5;

        /// <summary>The button 6.</summary>
        [DataObjectFormat(DeviceObjectTypeFlags.PushButton | DeviceObjectTypeFlags.ToggleButton | DeviceObjectTypeFlags.AnyInstance | DeviceObjectTypeFlags.Optional)]
        internal byte Buttons6;

        /// <summary>The button 7.</summary>
        [DataObjectFormat(DeviceObjectTypeFlags.PushButton | DeviceObjectTypeFlags.ToggleButton | DeviceObjectTypeFlags.AnyInstance | DeviceObjectTypeFlags.Optional)]
        internal byte Buttons7;
    }
}