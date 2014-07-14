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

namespace SharpDX.Toolkit.Input
{
    /// <summary>
    /// Indicates the kind of the pointer state change.
    /// </summary>
    public enum PointerUpdateKind
    {
        /// <summary>
        /// Other pointer event.
        /// </summary>
        Other,

        /// <summary>
        /// The left device button was pressed.
        /// </summary>
        LeftButtonPressed,

        /// <summary>
        /// The left device button was released.
        /// </summary>
        LeftButtonReleased,

        /// <summary>
        /// The right device button was pressed.
        /// </summary>
        RightButtonPressed,

        /// <summary>
        /// The right device button was released.
        /// </summary>
        RightButtonReleased,

        /// <summary>
        /// The middle device button was pressed.
        /// </summary>
        MiddleButtonPressed,

        /// <summary>
        /// The middle device button was released.
        /// </summary>
        MiddleButtonReleased,

        /// <summary>
        /// The device X-button 1 was pressed.
        /// </summary>
        XButton1Pressed,

        /// <summary>
        /// The device X-button 1 was released.
        /// </summary>
        XButton1Released,

        /// <summary>
        /// The device X-button 2 was pressed.
        /// </summary>
        XButton2Pressed,

        /// <summary>
        /// The device X-button 2 was released.
        /// </summary>
        XButton2Released,
    }
}