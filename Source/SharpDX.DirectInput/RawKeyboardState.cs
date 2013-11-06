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
    /// <summary>The raw keyboard state struct.</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 0 )]
    public unsafe partial struct RawKeyboardState : IDataFormatProvider
    {
        /// <summary>The keys.</summary>
        public fixed byte Keys [256];

        /// <summary>Gets the flags.</summary>
        /// <value>The flags.</value>
        DataFormatFlag IDataFormatProvider.Flags
        {
            get { return DataFormatFlag.RelativeAxis; }
        }

        /// <summary>Gets the objects format.</summary>
        /// <value>The objects format.</value>
        DataObjectFormat[] IDataFormatProvider.ObjectsFormat
        {
            get { return _objectsFormat; }
        }

        /// <summary>The _objects format.</summary>
        private static DataObjectFormat[] _objectsFormat;

        /// <summary>Initializes static members of the <see cref="RawKeyboardState"/> struct.</summary>
        static RawKeyboardState()
        {
            _objectsFormat = new DataObjectFormat[256];
            for (int i = 0; i < _objectsFormat.Length; i++)
                _objectsFormat[i] = new DataObjectFormat(ObjectGuid.Key, i,
                                                         DeviceObjectTypeFlags.PushButton | DeviceObjectTypeFlags.ToggleButton | DeviceObjectTypeFlags.Optional,
                                                         ObjectDataFormatFlags.None, i) {Name = "Key" + i};
        }
    }
}