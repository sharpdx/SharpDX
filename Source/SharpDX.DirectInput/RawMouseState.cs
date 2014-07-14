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

using System.Runtime.InteropServices;

namespace SharpDX.DirectInput
{
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    [DataFormat(DataFormatFlag.RelativeAxis)]
    public struct RawMouseState
    {
        [DataObjectFormat(Guid = ObjectGuid.XAxisStr, TypeFlags = DeviceObjectTypeFlags.RelativeAxis | DeviceObjectTypeFlags.AbsoluteAxis | DeviceObjectTypeFlags.AnyInstance)]
        internal int X;
        [DataObjectFormat(Guid = ObjectGuid.YAxisStr, TypeFlags = DeviceObjectTypeFlags.RelativeAxis | DeviceObjectTypeFlags.AbsoluteAxis | DeviceObjectTypeFlags.AnyInstance)]
        internal int Y;
        [DataObjectFormat(Guid = ObjectGuid.ZAxisStr, TypeFlags = DeviceObjectTypeFlags.RelativeAxis | DeviceObjectTypeFlags.AbsoluteAxis | DeviceObjectTypeFlags.AnyInstance | DeviceObjectTypeFlags.Optional)]
        internal int Z;

        [DataObjectFormat(DeviceObjectTypeFlags.PushButton | DeviceObjectTypeFlags.ToggleButton | DeviceObjectTypeFlags.AnyInstance)] 
        internal byte Buttons0;

        [DataObjectFormat(DeviceObjectTypeFlags.PushButton | DeviceObjectTypeFlags.ToggleButton | DeviceObjectTypeFlags.AnyInstance)]
        internal byte Buttons1;

        [DataObjectFormat(DeviceObjectTypeFlags.PushButton | DeviceObjectTypeFlags.ToggleButton | DeviceObjectTypeFlags.AnyInstance | DeviceObjectTypeFlags.Optional)]
        internal byte Buttons2;

        [DataObjectFormat(DeviceObjectTypeFlags.PushButton | DeviceObjectTypeFlags.ToggleButton | DeviceObjectTypeFlags.AnyInstance | DeviceObjectTypeFlags.Optional)]
        internal byte Buttons3;

        [DataObjectFormat(DeviceObjectTypeFlags.PushButton | DeviceObjectTypeFlags.ToggleButton | DeviceObjectTypeFlags.AnyInstance | DeviceObjectTypeFlags.Optional)]
        internal byte Buttons4;

        [DataObjectFormat(DeviceObjectTypeFlags.PushButton | DeviceObjectTypeFlags.ToggleButton | DeviceObjectTypeFlags.AnyInstance | DeviceObjectTypeFlags.Optional)]
        internal byte Buttons5;

        [DataObjectFormat(DeviceObjectTypeFlags.PushButton | DeviceObjectTypeFlags.ToggleButton | DeviceObjectTypeFlags.AnyInstance | DeviceObjectTypeFlags.Optional)]
        internal byte Buttons6;

        [DataObjectFormat(DeviceObjectTypeFlags.PushButton | DeviceObjectTypeFlags.ToggleButton | DeviceObjectTypeFlags.AnyInstance | DeviceObjectTypeFlags.Optional)]
        internal byte Buttons7;
    }
}