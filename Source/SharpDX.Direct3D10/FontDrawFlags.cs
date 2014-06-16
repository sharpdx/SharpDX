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

namespace SharpDX.Direct3D10
{
    /// <summary>
    /// Specifies formatting options for text rendering.
    /// </summary>
    /// <unmanaged>DT</unmanaged>
    [Flags]
    public enum FontDrawFlags
    {
        /// <summary>
        /// Align the text to the bottom.
        /// </summary>
        Bottom = 8,
        /// <summary>
        /// Align the text to the center.
        /// </summary>
        Center = 1,
        /// <summary>
        /// Expand tab characters.
        /// </summary>
        ExpandTabs = 0x40,
        /// <summary>
        /// Align the text to the left.
        /// </summary>
        Left = 0,
        /// <summary>
        /// Don't clip the text.
        /// </summary>
        NoClip = 0x100,
        /// <summary>
        /// Align the text to the right.
        /// </summary>
        Right = 2,
        /// <summary>
        /// Rendering the text in right-to-left reading order.
        /// </summary>
        RtlReading = 0x20000,
        /// <summary>
        /// Force all text to a single line.
        /// </summary>
        SingleLine = 0x20,
        /// <summary>
        /// Align the text to the top.
        /// </summary>
        Top = 0,
        /// <summary>
        /// Vertically align the text to the center.
        /// </summary>
        VerticalCenter = 4,
        /// <summary>
        /// Allow word breaks.
        /// </summary>
        WordBreak = 0x10
    }
}