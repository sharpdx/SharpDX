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

using System.Runtime.InteropServices;

namespace SharpDX.RawInput
{
    /// <summary>	
    /// No documentation.	
    /// </summary>	
    /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='RAWMOUSE']/*"/>	
    /// <unmanaged>RAWMOUSE</unmanaged>	
    [StructLayout(LayoutKind.Explicit, Pack = 0)]
    internal partial struct RawMouse
    {
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='RAWMOUSE::usFlags']/*"/>	
        /// <unmanaged>unsigned short usFlags</unmanaged>	
        [FieldOffset(0)]
        public short Flags;

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='RAWMOUSE::ulButtons']/*"/>	
        /// <unmanaged>unsigned int ulButtons</unmanaged>	
        [FieldOffset(2)]
        public int Buttons;

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='RAWMOUSE::usButtonFlags']/*"/>	
        /// <unmanaged>unsigned short usButtonFlags</unmanaged>	
        [FieldOffset(2)]
        public short ButtonFlags;

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='RAWMOUSE::usButtonData']/*"/>	
        /// <unmanaged>unsigned short usButtonData</unmanaged>	
        [FieldOffset(4)]
        public short ButtonData;

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='RAWMOUSE::ulRawButtons']/*"/>	
        /// <unmanaged>unsigned int ulRawButtons</unmanaged>	
        [FieldOffset(6)]
        public int RawButtons;

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='RAWMOUSE::lLastX']/*"/>	
        /// <unmanaged>int lLastX</unmanaged>	
        [FieldOffset(10)]
        public int LastX;

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='RAWMOUSE::lLastY']/*"/>	
        /// <unmanaged>int lLastY</unmanaged>	
        [FieldOffset(14)]
        public int LastY;

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='RAWMOUSE::ulExtraInformation']/*"/>	
        /// <unmanaged>unsigned int ulExtraInformation</unmanaged>	
        [FieldOffset(18)]
        public int ExtraInformation;
    }
}

